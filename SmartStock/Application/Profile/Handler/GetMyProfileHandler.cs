using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStock.Application.Profile.Command;
using SmartStock.Application.Profile.DTO_s;
using SmartStock.Infrastructure.Data;

namespace SmartStock.Application.Profile.Handler
{
    public class GetMyProfileHandler : IRequestHandler<GetMyProfileQuery, ProfileDto?>
    {
        private readonly AppDbContext _db;
        public GetMyProfileHandler(AppDbContext db) { _db = db; }

        public async Task<ProfileDto?> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            return await _db.Accounts
                .Include(a => a.Profile) // join profile table
                .Where(a => a.Id == request.AccountId)
                .Select(a => new ProfileDto
                {
                    // Account info
                    Email = a.Email,


                    // Profile info
                    Title = a.Profile != null ? a.Profile.Title : null,
                    FirstName = a.Profile != null ? a.Profile.FirstName : null,
                    LastName = a.Profile != null ? a.Profile.LastName : null,
                    Phone = a.Profile != null ? a.Profile.Phone : null,
                    ProfilePic = a.Profile != null ? a.Profile.ProfilePic : null,
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
