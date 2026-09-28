using Gym.Infrastructure.Configurations;
using Gym.Infrastructure.Notifications.Telegram;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gym.Infrastructure.HostedServices
{
    internal class ConfigurationLogger(
        IServiceScopeFactory _serviceLocator,
        ILogger<ConfigurationLogger> _logger,
        IOptions<MongoDbOptions> _mongoDbOptions,
        IOptions<ProxyOptions> _proxyOptions) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Current configurations:");
            _logger.LogInformation($"{nameof(ProxyOptions)} {_proxyOptions.Value.ToString()}");
            _logger.LogInformation($"{nameof(MongoDbOptions)} {_mongoDbOptions.Value.ToString()}");

            await using var scope = _serviceLocator.CreateAsyncScope();
            
            if (scope.ServiceProvider.GetService<TelegramBotToken>() is not null)
            {
                _logger.LogInformation($"{nameof(TelegramBotToken)} {scope.ServiceProvider.GetService<TelegramBotToken>()!.Value.ToString()}");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken) { }
    }
}
