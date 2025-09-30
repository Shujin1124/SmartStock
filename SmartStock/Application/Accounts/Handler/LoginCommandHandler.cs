using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartStock.Application.Accounts.Command;
using SmartStock.Application.Dtos;
using SmartStock.Infrastructure.Data;
using SmartStock.Infrastructure.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartStock.Application.Accounts.Handler
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public LoginCommandHandler(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 🔍 Check user by email
            var user = await _db.Accounts.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (user.Verified == null)
                throw new UnauthorizedAccessException("Account not verified.");

            if (user.Status == "Inactive")
                throw new UnauthorizedAccessException("This account is inactive. Please contact support.");

            // 🔑 Verify password
            if (!PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            // ✅ Add claims including AccountId
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // AccountId
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),       // Email
                new Claim(ClaimTypes.Role, user.Role),                    // Role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 🔐 Create JWT security key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]
                    ?? throw new InvalidOperationException("JWT Key not configured"))
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 📅 Create token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // expires in 1 hour
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // ✅ Return login response
            return new LoginResponse
            {
                Token = tokenString,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
