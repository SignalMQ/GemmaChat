using System.Net.Http.Json;
using GemmaChat.Application.Dto;
using GemmaChat.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GemmaChat.Application.Services
{
    public class LMSService(IHttpClientFactory clientFactory, DbContext context)
    {
        private readonly HttpClient _client = clientFactory.CreateClient("lms-client");

        public async Task<List<Message>> GetPreviousMessagesAsync(User user)
        {
            return await context
                .Set<User>()
                .AsNoTracking()
                .Include(x => x.Conversation)
                    .ThenInclude(x => x.Messages)
                    .ThenInclude(x => x.Content)
                    .ThenInclude(x => x.ImageUrl)
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

        public async Task InsertMessageAsync(Message message, User user)
        {
            var userFromDb = await context.Set<User>()
                .Include(x => x.Conversation)
                .ThenInclude(x => x.Messages)
                .FirstOrDefaultAsync(x => x.Id == user.Id);
            
            if (userFromDb != null)
            {
                userFromDb.Conversation.Messages.Add(message);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateLastRequest(User user)
        {
            await context
                .Set<User>()
                .Where(x => x.ChatId == user.ChatId)
                .ForEachAsync(x => x.LastRequest = DateTime.Now);
        }

        public async Task<IEnumerable<string>> SendMessageAsync(MessageDto messageDto, User user)
        {
            try
            {
                if (!user.Conversation.IsActive)
                    throw new Exception("Conversation is not active! Please contact the administrator.");

                await UpdateLastRequest(user);

                List<MessageDto> messageDtos =
                [
                    new MessageDto
                    {
                        Role = "system",
                        Content =
                        [
                            new ContentDto() {
                                Type = "text",
                                Text = Constants.SystemMessage
                            }
                        ]
                    }
                ];

                messageDtos.AddRange((await GetPreviousMessagesAsync(user)).Adapt<List<MessageDto>>());

                await InsertMessageAsync(messageDto.Adapt<Message>(), user);

                messageDtos.Add(messageDto);

                var request = new Request()
                {
                    Model = user.Conversation.Model,
                    Temperature = user.Conversation.Temperature,
                    MaxTokens = -1,
                    Messages = messageDtos,
                    Stream = false
                };

                var response = await _client.PostAsJsonAsync(Constants.Completions, request);
                var result = await response.Content.ReadFromJsonAsync<Response>();

                if (result != null)
                {
                    var assistantMessageDtos = result.Choices.Select(x => x.Message).Adapt<List<MessageDto>>();

                    foreach (var assistentMessageDto in assistantMessageDtos)
                        await InsertMessageAsync(assistentMessageDto.Adapt<Message>(), user);

                    return result.Choices.Select(x => x.Message.Content);
                }
                else
                {
                    throw new Exception("Assistant has no response!");
                }
            }
            catch (Exception ex)
            {
                return [ ex.Message ];
            }
        }
    }
}
