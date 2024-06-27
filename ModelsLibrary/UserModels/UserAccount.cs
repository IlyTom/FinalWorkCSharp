using ModelsLibrary.UserModels.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModelsLibrary.UserModels
{
    public class UserAccount : UserModel
    {
        private string? Token { get; set; }

        public string UpdateToken(string updateToken)
        {
            Token = updateToken;
            return Token;
        }

        public string GetToken() => Token;

        public void Login(UserModel model)
        {
            Id = model.Id;
            Email = model.Email;
            Name = model.Name;
            Surname = model.Surname;
            Role = model.Role;
        }

        public void Logout()
        {
            Id = null;
            Email = null;
            Name = null;
            Surname = null;
            Role = null;
            Token = null;
        }
    }
}
