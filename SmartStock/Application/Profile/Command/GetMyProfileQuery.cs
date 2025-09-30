using MediatR;
using SmartStock.Application.Profile.DTO_s;

namespace SmartStock.Application.Profile.Command
{
    public record GetMyProfileQuery(int AccountId) : IRequest<ProfileDto?>;

    // Command: Update profile
    public record UpdateMyProfileCommand(int AccountId, UpdateProfileDto Dto)
       : IRequest<ProfileUpdateResponse>;
}
