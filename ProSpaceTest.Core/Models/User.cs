using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProSpaceTest.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public static (User user, string Error) Create(Guid id, string login, string password, string role, bool isActive)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(login) || login.Length > 32)
            {
                error = "Login cannot be empty or longer than 32 symbols.";
            }
            else if (string.IsNullOrEmpty(password) || login.Length > 64)
            {
                error = "Password cannot be empty or longer than 64 symbols.";
            }
            var user = new User { Id = id, Login = login, Password = password, Role = role, IsActive = isActive };
            return (user, error);
        }

    }
}
