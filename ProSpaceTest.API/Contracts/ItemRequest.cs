namespace ProSpaceTest.API.Contracts
{
    public record ItemRequest(
        string Code,
        string Name,
        decimal Price,
        string Category);
}