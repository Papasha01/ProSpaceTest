﻿namespace ProSpaceTest.API.Contracts
{
    public record UserResponse(
    Guid Id,
    string Login,
    string Role,
    bool IsActive);
}