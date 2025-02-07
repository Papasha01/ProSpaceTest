namespace ProSpaceTest.API.Contracts
{
     public record OrderRequest(
     Guid CustomerId ,
     DateTime OrderDate ,
     DateTime? ShipmentDate ,
     int OrderNumber ,
     string Status,
     List<OrderItemRequest> OrderItems);
}