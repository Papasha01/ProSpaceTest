namespace ProSpaceTest.API.Contracts
{
    public record OrderResponse(
        Guid Id,
        Guid CustomerId,
        DateTime OrderDate,
        DateTime? ShipmentDate,
        int OrderNumber,
        string Status,
        List<OrderItemResponse> OrderItems);
}