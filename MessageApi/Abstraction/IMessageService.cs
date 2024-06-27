using ModelsLibrary.MessageModels.DTO;
using ModelsLibrary.MessageModels.Response;

namespace MessageApi.Abstraction
{
    public interface IMessageService
    {
        MessageResponse GetNewMessages(string recipientEmail);
        MessageResponse SendMessage(MessageModel model);
    }
}
