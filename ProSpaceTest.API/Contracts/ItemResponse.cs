namespace ProSpaceTest.API.Contracts
{
    public record ItemResponse(
        Guid Id,
        string Code,
        string Name,
        decimal Price,
        string Category);
}