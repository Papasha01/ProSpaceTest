
namespace ProSpaceTest.DataAccess.Entites
{
    public class OrderEntity
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public CustomerEntity? Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public int OrderNumber { get; set; }

        public string? Status { get; set; }

        public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
    }
}
