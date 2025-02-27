using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Entites;
using System.Diagnostics;
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
            Debug.WriteLine(passwordHash);
            var userEntity = await _context.User
                .FirstOrDefaultAsync(u => u.Login == login && u.Password == passwordHash);

            if (userEntity == null)
            {
                return null;
            }
            var user = new User
            {
                Id = userEntity.Id,
                Login = userEntity.Login,
                Password = userEntity.Password,
                Role = userEntity.Role,
                IsActive = userEntity.IsActive
            };
            return user;
        }

        public async Task<Guid> CreateUser(User user)
        {
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Login = user.Login,
                Password = HashPassword(user.Password),
                Role = user.Role,
                IsActive = user.IsActive
            };
            await _context.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var usersEntity = await _context.User.
                AsNoTracking().
                ToListAsync();

            return usersEntity.Select(u => User.Create(
                u.Id,
                u.Login,
                u.Password,
                u.Role,
                u.IsActive).user).ToList();
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return Guid.Empty;
            }
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }



        public async Task<Guid> UpdateUser(User user)
        {
            var userEntity = await _context.User.FindAsync(user.Id);
            if (userEntity == null)
            {
                return Guid.Empty;
            }


            userEntity.Login = user.Login;
            userEntity.Password = HashPassword(user.Password);
            userEntity.IsActive = user.IsActive;
            await _context.SaveChangesAsync();



            _context.User.Update(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
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
                Password = userEntity.Password,
                Role = userEntity.Role
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