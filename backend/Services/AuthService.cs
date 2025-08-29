using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IAuthService
{
    Task<(bool success, string message, User? user)> Registrate(RegisterRequest registerRequest);
}
public class AuthService : IAuthService
{
    private readonly CardioContext _db;
    private readonly IPasswordHasher _hasher;

    public AuthService(CardioContext db, IPasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }
    public async Task<(bool success, string message, User? user)> Registrate(RegisterRequest registerRequest)
    {
        if (await _db.Users.AnyAsync(u => u.Email == registerRequest.Email))
            return (false, "This email is already registered", null);

        User user = new()
        {
            Email = registerRequest.Email,
            HashedPassword = _hasher.Hash(registerRequest.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return (true, "Registered succesfully!", user);
    }
}