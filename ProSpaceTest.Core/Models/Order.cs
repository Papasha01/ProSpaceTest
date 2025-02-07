namespace ProSpaceTest.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public int OrderNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public static (Order Order, string Error) Create(Guid id, Guid customerId, DateTime orderDate, DateTime? shipmentDate, int orderNumber, string status)
        {
            var error = string.Empty;
            if (orderDate > DateTime.Now)
            {
                error = "Order date cannot be in the future.";
            }

            var order = new Order { Id = id, CustomerId = customerId, OrderDate = orderDate, ShipmentDate = shipmentDate, OrderNumber = orderNumber, Status = status };
            return (order, error);
        }
    }
}
