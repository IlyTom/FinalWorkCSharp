using AutoMapper;
using ModelsLibrary.MessageModels.DTO;
using ModelsLibrary.MessageModels.Entity;

namespace MessageApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MessageEntity, MessageModel>().ConvertUsing(new EntityToModelConverter());

        }
        private class EntityToModelConverter : ITypeConverter<MessageEntity, MessageModel>
        {
            public MessageModel Convert(MessageEntity source, MessageModel destination, ResolutionContext context)
            {
                return new MessageModel
                {
                    Id = source.Id,
                    CreatedAt = source.CreatedAt,
                    IsRead = source.IsRead,
                    RecipientEmail = source.Recipient.Email,
                    SenderEmail = source.Sender.Email,
                    Text = source.Text

                };
            }
        }
    }
}
