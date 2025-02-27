using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Core.Abstractions
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string login, string password);
        Task<User> GetUserById(Guid id);
        Task<List<User>> GetAllUsers();
        Task<Guid> CreateUser(User user);
        Task<Guid> UpdateUser(User user);
        Task<Guid> DeleteUser(Guid id);
    }
}