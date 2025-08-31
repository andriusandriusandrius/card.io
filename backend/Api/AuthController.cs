using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
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
        var result = await _authService.Registrate(registerRequest);
        if (!result.Success) return BadRequest(result.Message);

        return Ok(result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var result = await _authService.Login(loginRequest);
        if (!result.Success) return BadRequest(result.Message);

        return Ok(result);
    }
    
}