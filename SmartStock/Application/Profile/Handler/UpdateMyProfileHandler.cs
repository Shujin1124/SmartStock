using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStock.Application.Profile.Command;
using SmartStock.Application.Profile.DTO_s;
using SmartStock.Infrastructure.Data;

namespace SmartStock.Application.Profile.Handler
{
    public class UpdateMyProfileHandler : IRequestHandler<UpdateMyProfileCommand, ProfileUpdateResponse>
    {
        private readonly AppDbContext _db;
        public UpdateMyProfileHandler(AppDbContext db) { _db = db; }

        public async Task<ProfileUpdateResponse> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
        {
            var account = await _db.Accounts
                .Include(a => a.Profile)
                .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken);

            if (account == null)
            {
                return new ProfileUpdateResponse
                {
                    Success = false,
                    Message = "Account not found."
                };
            }

            if (account.Profile == null)
            {
                account.Profile = new Domain.ProfileInfo { AccountId = account.Id };
            }

            try
            {
                account.Profile.Title = request.Dto.Title;
                account.Profile.FirstName = request.Dto.FirstName;
                account.Profile.LastName = request.Dto.LastName;
                account.Profile.Phone = request.Dto.Phone;
                account.Profile.ProfilePic = request.Dto.ProfilePic;
                account.Updated = DateTime.UtcNow;

                await _db.SaveChangesAsync(cancellationToken);

                return new ProfileUpdateResponse
                {
                    Success = true,
                    Message = "Profile updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ProfileUpdateResponse
                {
                    Success = false,
                    Message = $"Update failed: {ex.Message}"
                };
            }
        }
    }
}
