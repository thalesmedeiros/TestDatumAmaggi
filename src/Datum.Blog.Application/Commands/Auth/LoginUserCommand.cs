using MediatR;

namespace Datum.Blog.Application.Commands.Auth
{
    public class LoginUserCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
