using MediatR;

namespace Datum.Blog.Application.Commands.Auth
{
    public class RegisterUserCommand : IRequest<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
