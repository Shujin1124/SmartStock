using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Accounts.Command
{
    public record RegisterCommand(
        string Email,
        string Password,
        string ConfirmPassword,
        string Title,
        string FirstName,
        string LastName,
        int CampusId,
        bool IsFromApi = false // 👈 just a constructor parameter
    ) : IRequest<RegisterResponse>;
}
