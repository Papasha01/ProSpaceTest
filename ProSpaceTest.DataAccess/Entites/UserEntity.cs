using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProSpaceTest.DataAccess.Entites;

namespace ProSpaceTest.Core.Models
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public CustomerEntity? Customer { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
