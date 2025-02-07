namespace ProSpaceTest.API.Contracts
{
    public record OrderItemRequest(
        Guid ItemId,
        int ItemsCount,
        decimal ItemPrice);
}