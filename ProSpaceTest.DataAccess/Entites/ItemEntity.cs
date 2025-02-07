
namespace ProSpaceTest.DataAccess.Entites
{
    public class ItemEntity
    {
        public Guid Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string? Category { get; set; }

        public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
    }
}
