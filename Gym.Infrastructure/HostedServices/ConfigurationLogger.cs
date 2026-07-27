using Gym.Infrastructure.Configurations;
using Gym.Infrastructure.Telegram;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gym.Infrastructure.HostedServices
{
    internal class ConfigurationLogger(
        ILogger<ConfigurationLogger> _logger,
        TelegramBotToken _telegramBotToken,
        IOptions<MongoDbOptions> _mongoDbOptions,
        IOptions<ProxyOptions> _proxyOptions) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Current configurations:");
            _logger.LogInformation($"{nameof(ProxyOptions)} {_proxyOptions.Value.ToString()}");
            _logger.LogInformation($"{nameof(MongoDbOptions)} {_mongoDbOptions.Value.ToString()}");
            _logger.LogInformation($"{nameof(TelegramBotToken)} {_telegramBotToken.Value.ToString()}");
        }

        public async Task StopAsync(CancellationToken cancellationToken) { }
    }
}
