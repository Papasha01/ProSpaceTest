namespace ProSpaceTest.DataAccess.Entites
{
    public class OrderItemEntity
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public OrderEntity? Order { get; set; }

        public Guid ItemId { get; set; }

        public ItemEntity? Item { get; set; }

        public int ItemsCount { get; set; }

        public decimal ItemPrice { get; set; }
    }
}
