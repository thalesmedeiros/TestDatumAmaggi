using MediatR;

namespace Datum.Blog.Application.Commands.User;

public class DeleteUserCommand : IRequest<bool>
{
    public DeleteUserCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}