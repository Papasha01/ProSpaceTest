namespace ProSpaceTest.Core.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public int ItemsCount { get; set; }
        public decimal ItemPrice { get; set; }
        

        public static (OrderItem OrderItem, string Error) Create(Guid id, Guid orderId, Guid itemId, int itemsCount, decimal itemPrice)
        {
            var error = string.Empty;
            if (itemsCount <= 0)
            {
                error = "Items count must be greater than 0.";
            }
            else if (itemPrice <= 0)
            {
                error = "Item price must be greater than 0.";
            }

            var orderItem = new OrderItem { Id = id, OrderId = orderId, ItemId = itemId, ItemsCount = itemsCount, ItemPrice = itemPrice };
            return (orderItem, error);
        }
    }

}
