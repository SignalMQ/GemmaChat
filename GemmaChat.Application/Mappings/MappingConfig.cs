using GemmaChat.Application.Dto;
using GemmaChat.Domain.Entities;
using Mapster;

namespace GemmaChat.Application.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.Default.Settings.NameMatchingStrategy = NameMatchingStrategy.IgnoreCase;

            config
                .NewConfig<ImageUrl, ImageUrlDto>()
                .TwoWays();

            config
                .NewConfig<ContentDto, Content>()
                .Map(dst => dst.Type, src => src.Type)
                .Map(dst => dst.Text, src => src.Text)
                .Map(dst => dst.ImageUrl, src => src.ImageUrl)
                .Map(dst => dst.Created, src => DateTime.Now);

            config
                .NewConfig<Content, ContentDto>();

            config
                .NewConfig<ResponseMessageDto, MessageDto>()
                .Map(dst => dst.Role, src => src.Role)
                .Map(dst => dst.Content, src => new List<ContentDto>                 {
                    new ContentDto
                    {
                        Type = "text",
                        Text = src.Content
                    }
                });

            config
                .NewConfig<MessageDto, Message>()
                .Map(dst => dst.Role, src => src.Role)
                .Map(dst => dst.Content, src => src.Content)
                .Map(dst => dst.Created, src => DateTime.Now);

            config
                .NewConfig<Message, MessageDto>();
        }
    }
}
