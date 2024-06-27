using ModelsLibrary.MessageModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.UserModels.Entity
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual List<MessageEntity> SendMessages { get; set; }
        public virtual List<MessageEntity> ReceiveMessages { get; set; }
        public virtual RoleEntity RoleType { get; set; }
    }
}
