namespace ProSpaceTest.API.Contracts
{
    public record CreateAccountRequest(
        string Login,
        string Password,
        string Role,
        bool IsActive,
        string Name,
        string Code,
        string Address,
        decimal Discount
        );
}