using AutoMapper;
using MessageApi.Abstraction;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.MessageModels.DTO;
using ModelsLibrary.MessageModels.Entity;
using ModelsLibrary.MessageModels.Response;
using ModelsLibrary.UserModels.Response;
using UserApi.Context;

namespace MessageApi.Services
{
    public class MessageService : IMessageService
    {

        private readonly IMapper _mapper;
        private readonly ContextApp _context;
        
        public MessageService(IMapper mapper, ContextApp context)
        {
            _context = context;
            _mapper = mapper;
        }
        
        
        public MessageResponse GetNewMessages(string recipientEmail)
        {
            var response = new MessageResponse();
            using(_context)
            {
                var messages = _context.Messages
                    .Include(x => x.Recipient)
                    .Include(x => x.Sender)
                    .Where(x => x.Recipient.Email == recipientEmail && !x.IsRead).ToList();

                foreach (var message in messages)
                {
                    message.IsRead = true;
                }
                _context.UpdateRange(messages);
                _context.SaveChanges();

                response.Messages.AddRange(messages.Select(_mapper.Map<MessageModel>));
                response.IsSuccess = true;
            }
            return response;
        }

        public MessageResponse SendMessage(MessageModel model)
        {
            var response = new MessageResponse();

            using(_context)
            {
                var sender = _context.Users.AsNoTracking().FirstOrDefault(x => x.Email == model.SenderEmail);
                var recipient = _context.Users.AsNoTracking().FirstOrDefault(x => x.Email == model.RecipientEmail);

                if (sender == null || recipient == null) 
                { 
                    response.IsSuccess = false;
                    response.Errors.Add(new ErrorResponse { Message = "Error. One of the participants is missing! " });
                    return response;
                }

                var message = new MessageEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    RecipientId = recipient.Id,
                    SenderId = sender.Id,
                    Text = model.Text
                };

                _context.Messages.Add(message);
                _context.SaveChanges();

                var entity = _context.Messages
                    .Include(x => x.Recipient)
                    .Include(x => x.Sender)
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == message.Id);

                response.Messages.Add(_mapper.Map<MessageModel>(entity));
                response.IsSuccess = true;

            }
            return response;
        }
    }
}
