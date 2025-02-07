using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Entites;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProSpaceTest.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopDbContext _context;

        public UserRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string login, string password)
        {
            string passwordHash = HashPassword(password);
            var userEntity = await _context.User
                .FirstOrDefaultAsync(u => u.Login == login && u.PasswordHash == passwordHash);

            if (userEntity == null)
            {
                return null;
            }
            var user = new User
            {
                Id = userEntity.Id,
                Login = userEntity.Login,
                PasswordHash = userEntity.PasswordHash,
                Role = userEntity.Role,
                CustomerId = userEntity.CustomerId,
                Firstname = userEntity.Firstname,
                Lastname = userEntity.Lastname,
                IsActive = userEntity.IsActive,
                Created = userEntity.Created,
                LastLogin = userEntity.LastLogin
            };
            return user;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var userEntity = await _context.User.FindAsync(id);

            if (userEntity == null)
            {
                return null;
            }
            var user = new User
            {
                Id = userEntity.Id,
                Login = userEntity.Login,
                PasswordHash = userEntity.PasswordHash,
                Role = userEntity.Role,
                CustomerId = userEntity.CustomerId,
                Firstname = userEntity.Firstname,
                Lastname = userEntity.Lastname,
                IsActive = userEntity.IsActive,
                Created = userEntity.Created,
                LastLogin = userEntity.LastLogin
            };
            return user;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}