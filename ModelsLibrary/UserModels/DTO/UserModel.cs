using ModelsLibrary.UserModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.UserModels.DTO
{
    public class UserModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Surname { get; set; }

        public UserRole? Role { get; set; }
    }
}
