namespace ProSpaceTest.API.Contracts
{
    public record OrderItemResponse(
        Guid ItemId,
        int ItemsCount,
        decimal ItemPrice);
}