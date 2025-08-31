using Microsoft.Net.Http.Headers;

namespace backend.DTOs
{
    public class RegisterRequest
    {
        public string? Email { set; get; }
        public string? Password { set; get; }
    }
    public class LoginRequest
    {
        public string? Email { set; get; }
        public string? Password { set; get; }
    }
}