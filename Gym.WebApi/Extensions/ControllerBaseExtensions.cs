using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult InternalErrorProblem(this ControllerBase controller, String detail) 
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal error",
                Detail = detail,
                Instance = controller.HttpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            return controller.StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
        }

        public static ObjectResult BadRequestProblem(this ControllerBase controller, String detail)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad request",
                Detail = detail,
                Instance = controller.HttpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            return controller.BadRequest(problemDetails);
        }

        public static ObjectResult ConflictProblem(this ControllerBase controller, String detail)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = detail,
                Instance = controller.HttpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            return controller.Conflict(problemDetails);
        }
    }
}
