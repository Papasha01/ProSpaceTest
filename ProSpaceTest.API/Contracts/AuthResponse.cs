namespace ProSpaceTest.API.Contracts
{
    public record AuthResponse(
        Guid Id,
        string Login,
        string Role,
        bool IsActive);
}