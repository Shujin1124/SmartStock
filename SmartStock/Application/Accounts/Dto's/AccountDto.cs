namespace SmartStock.Application.Dtos
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty; // 👈 needed for password match check
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int CampusId { get; set; } // 👈 required since handler validates CampusId
        public bool IsFromApi { get; set; } = false;
    }

    public class RegisterResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? VerificationTokens { get; set; }
    }
}
