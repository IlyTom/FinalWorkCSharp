using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.MessageModels.DTO
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
    }
}
