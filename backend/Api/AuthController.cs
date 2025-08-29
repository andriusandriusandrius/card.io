using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        (bool success, string message, User? user) result = await _authService.Registrate(registerRequest);
        if (!result.success) return BadRequest(result.message);

        return Ok(result.user?.Email);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        (bool success, string message, User? user) result = await _authService.Login(loginRequest);
        if (!result.success) return BadRequest(result.message);

        return Ok(result.user?.Email);
    }
    
}