namespace ProSpaceTest.API.Contracts
{
    public record CustomerRequest(
        string Name,
        string Code,
        string Address,
        decimal Discount);
}