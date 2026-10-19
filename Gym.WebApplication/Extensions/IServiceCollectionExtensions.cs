using FluentValidation;
using Gym.Chat.Client;
using Gym.WebApplication.Authentication;
using Gym.WebApplication.JSAdapters;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.RequestHandlers;
using Gym.WebApplication.Scanners;
using Gym.WebApplication.ViewModels.AccountHistory;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Gym.WebApplication.Extensions;

public static partial class IServiceCollectionExtensions
{
    public static IServiceCollection AddOptionsFromConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<BffOptions>()
            .Bind(configuration.GetRequiredSection(BffOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ChatWSOptions>()
            .Bind(configuration.GetRequiredSection(ChatWSOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddBffNamedClient(this IServiceCollection services, String key, String baseUrl)
    {
        services.AddHttpClient(key, client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("X-Static-Header", "1");
        });

        return services;
    }

    /// <summary>
    /// Registers <see cref="ChatMediator"/> as a singleton with a pre-configured
    /// SignalR <see cref="HubConnection"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Why this configuration is non-obvious:</b>
    /// </para>
    /// <para>
    /// The client is Blazor WASM talking to a separate Chat API on a different port
    /// (same domain, different origin). Auth is cookie-based: the BFF issues a session
    /// cookie, and the Chat API reads it in <c>JwtBearerEvents.OnMessageReceived</c>
    /// to obtain the access token.
    /// </para>
    /// <para>
    /// <b>Problem 1 — negotiate cannot carry the cookie.</b>
    /// The default flow starts with <c>POST /negotiate</c>, which is a cross-origin
    /// <c>fetch</c>. Browsers do not attach <c>SameSite=Lax</c> cookies to
    /// cross-origin <c>fetch</c>, even for same-site ports. Negotiate therefore
    /// reaches the server without the cookie, and with <c>[Authorize]</c> on the hub
    /// it fails with 401 (<c>DenyAnonymousAuthorizationRequirement</c>).
    /// The WebSocket handshake, by contrast, is a plain <c>GET</c> and does receive
    /// <c>SameSite=Lax</c> cookies for same-site ports.
    /// </para>
    /// <para>
    /// <b>Solution — <c>SkipNegotiation = true</c>.</b>
    /// Removes the problematic <c>POST /negotiate</c> entirely. The client goes
    /// straight to the WebSocket handshake, where the cookie is available, so
    /// <c>[Authorize]</c> on the hub class authenticates successfully.
    /// </para>
    /// <para>
    /// <b>Problem 2 — <c>SkipNegotiation</c> requires WebSockets only.</b>
    /// The flag is valid only when WebSockets is the sole enabled transport.
    /// <c>Transports = HttpTransportType.WebSockets</c> pins it explicitly so a
    /// future config change cannot silently reintroduce negotiate or an unsupported
    /// fallback.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddChatClientMediator(this IServiceCollection services)
    {
        services.AddSingleton<ChatMediator>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ChatWSOptions>>();

            return new ChatMediator(() =>
                new HubConnectionBuilder()
                    .WithUrl(options.Value.Url, HttpTransportType.WebSockets, options =>
                    {
                        options.SkipNegotiation = true;

                        options.Transports = HttpTransportType.WebSockets;
                    })
                    .WithAutomaticReconnect(new ChatWSForeverRetryPolicy())
                    .WithStatefulReconnect()
                    .Build());
        });

        return services;
    }

    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<UserAuthState>();
        services.AddScoped<AuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthStateProvider>());

        services.AddScoped<ICheckSessionService, CheckSessionService>();
        services.AddScoped<ITelegramInitService, TelegramInitService>();
        services.AddScoped<ISessionInfoService, SessionInfoService>();

        services.AddScoped<ILogoutService, LogoutService>();

        return services;
    }

    public static IServiceCollection AddLocalStorage(this IServiceCollection services)
    {
        services.AddScoped<LocalStorageAdapter>();
        return services;
    }

    public static IServiceCollection AddFormValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateClientFormModel>, CreateClientFormModel.Validator>();
        services.AddScoped<IValidator<ChangePasswordFormModel>, ChangePasswordFormModel.Validator>();
        services.AddScoped<IValidator<ChargeClientFormModel>, ChargeClientFormModel.Validator>();
        services.AddScoped<IValidator<CreatePollFormModel>, CreatePollFormModel.Validator>();
        services.AddScoped<IValidator<CreateCalendarEventFormModel>, CreateCalendarEventFormModel.Validator>();
        services.AddScoped<IValidator<CreateTrainingFormModel>, CreateTrainingFormModel.Validator>();
        services.AddScoped<IValidator<CreateInstructorFormModel>, CreateInstructorFormModel.Validator>();
        services.AddScoped<IValidator<CreatePersonalTrainingFormModel>, CreatePersonalTrainingFormModel.Validator>();

        return services;
    }

    public static IServiceCollection AddBffServices(this IServiceCollection services)
    {
        services.AddScoped<AccountHistoryViewModelMapper>();
        RequestHandlersScanner.ScanAssembly(typeof(Program).Assembly, services);

        return services;
    }

    public static IServiceCollection AddOperationStateNotifier<TRequest, TResponse>(this IServiceCollection services)
    {
        services.AddScoped<AsyncOperationStateNotifier<TRequest, TResponse>>();
        return services;
    }

}