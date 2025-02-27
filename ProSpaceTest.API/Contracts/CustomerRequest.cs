namespace ProSpaceTest.API.Contracts
{
    public record CustomerRequest(
        Guid UserId,
        string Name,
        string Code,
        string Address,
        decimal Discount);

    public record CustomerUpdateRequest(
    string Name,
    string Code,
    string Address,
    decimal Discount);
}


