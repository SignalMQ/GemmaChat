using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GemmaChat.Application.Services
{
    public class TelegramService(ITelegramBotClient botClient, LLMService llmService)
    {
        public void StartReceiving(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message]
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleError,
                receiverOptions,
                cancellationToken);
        }

        private Task HandleError(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            throw exception;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Message is not { } message) return;
            if (message.Text is not { } messageText) return;
            if (message.Text.StartsWith('/')) return;

            messageText = messageText.Replace("@"+(await client.GetMe()).Username, "");

            var user = await llmService.GetUserAsync(update.Message.Chat.Id) 
                ?? await llmService.CreateUserAsync(update.Message.Chat);

            var responseMessages = await llmService.SendMessageAsync(messageText, user);

            foreach (var content in responseMessages)
            {
                await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: content,
                cancellationToken: token);
            }
        }
    }
}
