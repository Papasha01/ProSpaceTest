using ProSpaceTest.Core.Models;

namespace ProSpaceTest.DataAccess.Entites
{
    public class CustomerEntity
    {
        public Guid Id { get; set; }
        public UserEntity User { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public decimal Discount { get; set; }

        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();

    }
}
