using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Core.Abstractions
{
    public interface IAccountCreationRepository
    {
        Task<Guid> CreateAccount(Customer customer, User user);
    }
}
