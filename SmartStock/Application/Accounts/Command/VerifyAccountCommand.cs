using MediatR;

namespace SmartStock.Application.Accounts.Command
{
    public record VerifyAccountCommand(string Token) : IRequest<string>;
}
