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
    public class AuthResponse
    {
        public string? Token { set; get; }
        public Guid UserId { set; get; }
        public string? Email { set; get; }
    }
}