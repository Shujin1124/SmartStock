using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStock.Application.Accounts.Command;
using SmartStock.Application.Dtos;
using SmartStock.Domain;
using SmartStock.Infrastructure.Data;
using SmartStock.Infrastructure.Security;
using SmartStock.Infrastructure.Services;

namespace SmartStock.Application.Accounts.Handler
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly AppDbContext _db;
        private readonly EmailService _email;
        private readonly IConfiguration _config;

        public RegisterCommandHandler(AppDbContext db, EmailService email, IConfiguration config)
        {
            _db = db;
            _email = email;
            _config = config;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // ✅ Check password confirmation
            if (request.Password != request.ConfirmPassword)
            {
                return new RegisterResponse
                {
                    Message = "Password and Confirm Password do not match."
                };
            }

            // ✅ Check if email exists
            if (await _db.Accounts.AnyAsync(x => x.Email == request.Email, cancellationToken))
            {
                return new RegisterResponse { Message = "Email already registered." };
            }

            // ✅ Validate CampusId exists
            var campusExists = await _db.Campuses.AnyAsync(c => c.Id == request.CampusId, cancellationToken);
            if (!campusExists)
            {
                return new RegisterResponse
                {
                    Message = "Invalid CampusId. Campus does not exist."
                };
            }

            // ✅ Generate verification token
            var token = Guid.NewGuid().ToString();

            var user = new Account
            {
                Email = request.Email,
                PasswordHash = PasswordHelper.HashPassword(request.Password),
                VerificationTokens = token,
                Created = DateTime.UtcNow,
                Role = "User",
                Status = "Pending",
                CampusId = request.CampusId,
                Profile = new ProfileInfo
                {
                    Title = request.Title,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                }
            };

            _db.Accounts.Add(user);
            await _db.SaveChangesAsync(cancellationToken);

            // ✅ Always send verification email
            var baseUrl = _config["Frontend:BaseUrl"];
            var verifyUrl = $"{baseUrl}/verify?token={token}";

            await _email.SendEmailAsync(request.Email, "Verify your account",
                $"""
                <h2>Welcome {request.FirstName}!</h2>
                <p>Please verify your account by clicking <a href='{verifyUrl}'>here</a>.</p>
                <p>Or use this token manually:</p>
                <h3>{token}</h3>
                """);

            // ✅ If request came from API/Swagger → return token in response too
            if (request.IsFromApi)
            {
                return new RegisterResponse
                {
                    Message = "Registration successful. Token has been sent to your email and returned here for API usage.",
                    VerificationTokens = token
                };
            }

            // ✅ Default frontend flow
            return new RegisterResponse
            {
                Message = "Registration successful. Please check your email for verification."
            };
        }
    }
}
