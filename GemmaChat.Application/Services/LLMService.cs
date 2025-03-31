using System.Net.Http.Json;
using GemmaChat.Application.Dto;
using GemmaChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GemmaChat.Application.Services
{
    public class LLMService(IHttpClientFactory clientFactory, DbContext context)
    {
        private readonly HttpClient _client = clientFactory.CreateClient("llm-client");

        public async Task<List<Message>> GetPreviousMessagesAsync(User user)
        {
            return await context
                .Set<User>()
                .AsNoTracking()
                .Include(x => x.Conversation)
                    .ThenInclude(x => x.Messages)
                .Where(x => x.ChatId == user.ChatId)
                .SelectMany(x => x.Conversation.Messages)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<User?> GetUserAsync(long chatId)
        {
            return await context
                .Set<User>()
                .AsNoTracking()
                .Include(x => x.Conversation)
                    .ThenInclude(x => x.Messages)
                .FirstOrDefaultAsync(x => x.ChatId == chatId);
        }

        public async Task<User> CreateUserAsync(Telegram.Bot.Types.Chat chat)
        {
            var user = new User
            {
                ChatId = chat.Id,
                Username = chat.Username,
                FirstName = chat.FirstName,
                LastName = chat.LastName,
                Created = DateTime.Now,
                Conversation = new Conversation
                {
                    Model = Constants.Model,
                    IsActive = true,
                    Temperature = 0.7,
                    Created = DateTime.Now
                }
            };

            await context.Set<User>().AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task InsertMessageAsync(Message message)
        {
            await context.Set<Message>().AddAsync(message);
            await context.SaveChangesAsync();
        }

        public async Task UpdateLastRequest(User user)
        {
            await context
                .Set<User>()
                .Where(x => x.ChatId == user.ChatId)
                .ForEachAsync(x => x.LastRequest = DateTime.Now);
        }

        public async Task<IEnumerable<string>> SendMessageAsync(string message, User user)
        {
            if (!user.Conversation.IsActive)
                return ["The administrator has blocked you! \n Write to him for request access!"];

            await UpdateLastRequest(user);

            var messages = new List<Message>()
            {
                new()
                {
                    ConversationId = user.Conversation.Id,
                    Role = "system",
                    Content = Constants.SystemMessage
                }
            };

            messages.AddRange(await GetPreviousMessagesAsync(user));

            var userMessage = new Message
            {
                ConversationId = user.Conversation.Id,
                Role = "user",
                Content = message,
                Created = DateTime.Now
            };

            await InsertMessageAsync(userMessage);
            messages.Add(userMessage);

            var request = new Request()
            {
                Model = user.Conversation.Model,
                Temperature = user.Conversation.Temperature,
                MaxTokens = -1,
                Messages = messages.Select(x => new MessageDto { Content = x.Content, Role = x.Role}).ToList(),
                Stream = false
            };

            try
            {
                var response = await _client.PostAsJsonAsync(Constants.Completions, request);
                var result = await response.Content.ReadFromJsonAsync<Response>();

                if (result != null)
                {
                    var assistantMessages = result.Choices.Select(x => x.Message).Select(x => new Message
                    {
                        ConversationId = user.Conversation.Id,
                        Role = x.Role,
                        Content = x.Content,
                        Created = DateTime.Now
                    });

                    foreach (var assistentMessage in assistantMessages)
                        await InsertMessageAsync(assistentMessage);

                    return result.Choices.Select(x => x.Message).Select(x => x.Content);
                }
            }
            catch
            {
                return ["The chatbot was unable to generate a response because it took a long time to do so."];
            }

            return [];
        }
    }
}
