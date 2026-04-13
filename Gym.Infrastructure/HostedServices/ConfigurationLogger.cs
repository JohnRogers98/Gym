using Gym.Infrastructure.Configurations;
using Gym.Infrastructure.Telegram;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.HostedServices
{
    internal class ConfigurationLogger(
        ILogger<ConfigurationLogger> _logger,
        TelegramBotToken _telegramBotToken,
        MongoDbOptions _mongoDbOptions,
        ProxyOptions _proxyOptions) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Current configurations:");
            _logger.LogInformation($"{nameof(ProxyOptions)} {_proxyOptions.ToString()}");
            _logger.LogInformation($"{nameof(MongoDbOptions)} {_mongoDbOptions.ToString()}");
            _logger.LogInformation($"{nameof(TelegramBotToken)} {_telegramBotToken.Value.ToString()}");
        }

        public async Task StopAsync(CancellationToken cancellationToken) { }
    }
}
