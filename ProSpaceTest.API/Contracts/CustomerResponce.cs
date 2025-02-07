namespace ProSpaceTest.API.Contracts
{
    public record CustomerResponse(
        Guid Id,
        string Name,
        string Code,
        string Address,
        decimal Discount);
}