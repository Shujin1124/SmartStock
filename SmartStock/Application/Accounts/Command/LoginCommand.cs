using MediatR;
using SmartStock.Application.Dtos;

namespace SmartStock.Application.Accounts.Command
{
    public record LoginCommand(
       string Email,
       string Password
   ) : IRequest<LoginResponse>;
}
