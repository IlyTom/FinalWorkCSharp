using ModelsLibrary.MessageModels.DTO;
using ModelsLibrary.UserModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.MessageModels.Response
{
    public class MessageResponse
    {
        public bool IsSuccess { get; set; }
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
        public List<ErrorResponse> Errors { get; set; } = new List<ErrorResponse>();
    }
}
