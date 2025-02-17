using MediatR;

namespace Datum.Blog.Application.Commands.User;

public class CreateUserCommand : IRequest<Guid>
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }

    public CreateUserCommand(string nome, string email, string senhaHash)
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
    }
}
