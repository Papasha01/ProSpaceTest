namespace ProSpaceTest.API.Contracts
{
    public record UserRequest(
    string Login,
    string Password,
    bool IsActive);
}