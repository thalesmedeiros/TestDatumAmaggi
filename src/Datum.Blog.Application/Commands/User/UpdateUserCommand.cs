using MediatR;

namespace Datum.Blog.Application.Commands.User;

public class UpdateUserCommand : IRequest<bool>
{
    public Guid Id { get; private set; }
    public string? Nome { get; private set; }
    public string? Email { get; private set; }
    public string? SenhaHash { get; private set; }

    public UpdateUserCommand(Guid id, string? nome, string? email, string? senhaHash)
    {
        Id = id;
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
    }
}
