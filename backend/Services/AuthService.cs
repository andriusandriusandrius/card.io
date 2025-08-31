using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IAuthService
{
    Task<ApiResponse<string>> Registrate(RegisterRequest registerRequest);
    Task<ApiResponse<string>> Login(LoginRequest loginRequest);
}
public class AuthService : IAuthService
{
    private readonly CardioContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(CardioContext db, IPasswordHasher hasher, IJwtProvider jwtProvider)
    {
        _db = db;
        _hasher = hasher;
        _jwtProvider = jwtProvider;
    }
    public async Task<ApiResponse<string>> Registrate(RegisterRequest registerRequest)
    {
        try
        {
            if (await _db.Users.AnyAsync(u => u.Email == registerRequest.Email))
                return new ApiResponse<string> { Success = false, Message = "This email is already registered", Data = null };

            User user = new()
            {
                Email = registerRequest.Email,
                HashedPassword = _hasher.Hash(registerRequest.Password)
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new ApiResponse<string> { Success = true, Message = "Registered succesfully!", Data = user.Email };
        }
        catch(Exception ex)
        {
            return new ApiResponse<string> { Success = false, Message = $"Registration failed: {ex.Message}", Data = null };
        }
    }
    public async Task<ApiResponse<string>> Login(LoginRequest loginRequest)
    {
        try
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null)
                return new ApiResponse<string> { Success = false, Message = "No such email found", Data = null };

            if (!_hasher.Verify(loginRequest.Password, user.HashedPassword))
                return new ApiResponse<string> { Success = false, Message = "Wrong credentials", Data = null };

            string token = _jwtProvider.GenerateToken(user);
            return new ApiResponse<string> { Success = true, Message = "Login succesful!", Data = token };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string> { Success = false, Message = "Login failed", Data = null };
        }
    }
}