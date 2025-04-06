using GemmaChat.Application.Dto;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GemmaChat.Application.Services
{
    public class TelegramService(ITelegramBotClient botClient, LMSService llmService)
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
            var username = (await client.GetMe(token)).Username ?? "@";

            if (update.Message is not { } message) return;
            if (message.Text != null && message.Text.StartsWith('/')) return;
            if (message is not { Type: MessageType.Text or MessageType.Photo }) return;

            var user = await llmService.GetUserAsync(update.Message.Chat.Id) 
                ?? await llmService.CreateUserAsync(update.Message.Chat);

            MessageDto messageDto = new MessageDto
            {
                Role = "user",
                Content = []
            };

            if (message.Text != null)
            {
                messageDto.Content.Add(new ContentDto
                {
                    Type = "text",
                    Text = message.Text
                });
            }
            else if (message.Photo != null && message.Photo.Length > 0)
            {
                var photo = message.Photo.Last();
                var file = await client.GetFile(photo.FileId, token);
                using var memoryStream = new MemoryStream();
                await client.DownloadFile(file, memoryStream, token);
                var base64 = Convert.ToBase64String(memoryStream.ToArray());

                if (message.Caption != null)
                {
                    messageDto.Content.Add(new ContentDto
                    {
                        Type = "text",
                        Text = message.Caption
                    });
                }

                messageDto.Content.Add(new ContentDto
                {
                    Type = "image_url",
                    ImageUrl = new ImageUrlDto
                    {
                        Url = $"data:image/jpeg;base64,{base64}"
                    }
                });
            }

            var responseMessages = await llmService.SendMessageAsync(messageDto, user);

            foreach (var responseMessage in responseMessages)
                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: EscapeCharacters(responseMessage),
                    parseMode: ParseMode.MarkdownV2,
                    cancellationToken: token);
        }

        public static string GetMimeType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".heic" => "image/heic",
                _ => throw new NotSupportedException($"Format {extension} is not supported.")
            };
        }

        public static string EscapeCharacters(string content)
        {
            return content
                .Replace("[", "\\[")
                .Replace("]", "\\]")
                .Replace("(", "\\(")
                .Replace(")", "\\)")
                .Replace("~", "\\~")
                .Replace(">", "\\>")
                .Replace("#", "\\#")
                .Replace("+", "\\+")
                .Replace("-", "\\-")
                .Replace("=", "\\=")
                .Replace("|", "\\|")
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace(".", "\\.")
                .Replace("!", "\\!");
        }
    }
}
