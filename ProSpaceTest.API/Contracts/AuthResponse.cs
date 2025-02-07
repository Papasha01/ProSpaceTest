namespace ProSpaceTest.API.Contracts
{
    public record AuthResponse(
         Guid Id,
         string Login,
         string Role,
         Guid? CustomerId,
         string Firstname,
         string Lastname,
         bool IsActive,
         DateTime Created,
         DateTime? LastLogin);
}