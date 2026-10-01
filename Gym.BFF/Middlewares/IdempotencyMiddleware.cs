using Gym.BFF.Options;
using Gym.BFF.Services;
using Gym.Redis.Client.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Gym.BFF.Middlewares
{
    public class IdempotencyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IIdempotencyKeyValidator _idempotencyKeyValidator;
        private readonly IProblemDetailsService _problemDetailsService;
        private readonly IIdempotencyRecordKeyGenerator _idempotencyRecordKeyGenerator;
        private readonly ISaveIdempotencyRecordService _saveIdempotencyRecordService;
        private readonly IGetIdempotencyRecordService _getIdempotencyRecordService;

        public IdempotencyMiddleware(
            RequestDelegate next,
            IIdempotencyKeyValidator idempotencyKeyValidator,
            IProblemDetailsService problemDetailsService,
            IIdempotencyRecordKeyGenerator idempotencyRecordKeyGenerator,
            ISaveIdempotencyRecordService saveIdempotencyRecordService,
            IGetIdempotencyRecordService getIdempotencyRecordService)
        {
            _next = next;
            _idempotencyKeyValidator = idempotencyKeyValidator;
            _problemDetailsService = problemDetailsService;
            _idempotencyRecordKeyGenerator = idempotencyRecordKeyGenerator;
            _saveIdempotencyRecordService = saveIdempotencyRecordService;
            _getIdempotencyRecordService = getIdempotencyRecordService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientSessionKey = context.User.FindFirst(ExtendedClaimTypes.ClientSessionKey)?.Value;

            if (clientSessionKey is null || this.ShouldSkipIdempotency(context.Request.Method))
            {
                await _next(context);
                return;
            }

            if (!this.IsIdempotencyKeyPresent(context.Request))
            {
                await _next(context);
                return;
            }

            if (!_idempotencyKeyValidator.IsValid(context.Request.Headers["Idempotency-Key"]!))
            {
                await this.WriteProblemDetailsAsync(
                    context,
                    "Invalid Idempotency-Key",
                    "The Idempotency-Key header must be a valid GUID."
                );
                return;
            }

            var idempotencyRecordKey = _idempotencyRecordKeyGenerator
                .Generate(clientSessionKey, context.Request.Headers["Idempotency-Key"]!);

            Boolean keySaved = await _saveIdempotencyRecordService
                .HandleAsync(idempotencyRecordKey, new IdempotencyRecord { State = IdempotencyRecordState.InProgress }, When.NotExists);

            if (keySaved)
            {
                await this.ExecuteNewIdempotentRequestAsync(context, idempotencyRecordKey);
            }
            else
            {
                await this.ReturnSavedIdempotentResponseAsync(context, idempotencyRecordKey);
            }
        }

        private Boolean ShouldSkipIdempotency(String method)
        {
            return method.Equals("GET", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("HEAD", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("DELETE", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("TRACE", StringComparison.OrdinalIgnoreCase);
        }

        private Boolean IsIdempotencyKeyPresent(HttpRequest request) =>
            request.Headers.ContainsKey("Idempotency-Key");

        private async Task WriteProblemDetailsAsync(HttpContext context, String title, String detail, Int32 statusCode = StatusCodes.Status400BadRequest)
        {
            context.Response.StatusCode = statusCode;

            if (_problemDetailsService is not null)
            {
                await _problemDetailsService.WriteAsync(new ProblemDetailsContext
                {
                    HttpContext = context,
                    ProblemDetails = new ProblemDetails
                    {
                        Title = title,
                        Status = statusCode,
                        Detail = detail
                    }
                });
            }
            else
            {
                await context.Response.WriteAsync($"{title}: {detail}");
            }
        }

        private Boolean IsContentTypeForSerialization(String? contentType)
        {
            if (String.IsNullOrEmpty(contentType)) return false;

            if (!MediaTypeHeaderValue.TryParse(contentType, out var parsed))
                return false;

            if (parsed.MediaType is null) return false;

            var mediaType = parsed.MediaType;

            return mediaType.Equals(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase);
        }

        private async Task ExecuteNewIdempotentRequestAsync(HttpContext context, String idempotencyRecordKey)
        {
            var originalBody = context.Response.Body;
            using var buffer = new MemoryStream();
            context.Response.Body = buffer;

            try
            {
                await _next(context);

                if (this.IsContentTypeForSerialization(context.Response.ContentType))
                {
                    buffer.Position = 0;

                    String responseBody;
                    using (var reader = new StreamReader(
                        buffer,
                        detectEncodingFromByteOrderMarks: false,
                        leaveOpen: true))
                    {
                        responseBody = await reader.ReadToEndAsync();
                    }

                    var record = new IdempotencyRecord
                    {
                        State = IdempotencyRecordState.Completed,
                        StatusCode = context.Response.StatusCode,
                        ContentType = context.Response.ContentType,
                        Location = context.Response.Headers.Location.ToString(),
                        ResponseBody = responseBody
                    };

                    Boolean keyUpdated = await _saveIdempotencyRecordService.HandleAsync(idempotencyRecordKey, record, When.Exists);
                    if (!keyUpdated)
                    {
                        //TODO: Log error: failed to update idempotency record in Redis
                    }
                }

                context.Response.Headers.ContentLength = buffer.Length;
                buffer.Position = 0;
                await buffer.CopyToAsync(originalBody);
            }
            catch (Exception)
            {
                //TODO: Log the exception. Delete the idempotency record from Redis to allow retrying the request.
                throw;
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private async Task ReturnSavedIdempotentResponseAsync(HttpContext context, String idempotencyRecordKey)
        {
            var record = await _getIdempotencyRecordService.HandleAsync(idempotencyRecordKey);

            //Operation was deleted or TTL expired, so we can retry the request
            if (record is null || record is { State: IdempotencyRecordState.InProgress })
            {
                context.Response.Headers.RetryAfter = "1";
                await this.WriteProblemDetailsAsync(
                    context,
                    "Request idempotency conflict",
                    "The request is being processed or its state expired. Retry later.",
                    StatusCodes.Status409Conflict
                );
                return;
            }

            context.Response.StatusCode = record.StatusCode ?? StatusCodes.Status200OK;

            if (!String.IsNullOrEmpty(record.ContentType))
                context.Response.ContentType = record.ContentType;

            if (!String.IsNullOrEmpty(record.Location))
                context.Response.Headers.Location = record.Location;

            await context.Response.WriteAsync(record.ResponseBody ?? String.Empty);
            return;
        }

    }
}
