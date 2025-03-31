using GemmaChat.Application.Services;
using Microsoft.Extensions.Hosting;

namespace GemmaChat.Service
{
    public class Worker(TelegramService telegramService) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            telegramService.StartReceiving(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.FromResult(0);
        }
    }
}
