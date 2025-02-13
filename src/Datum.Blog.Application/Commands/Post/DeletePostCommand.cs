using MediatR;

namespace Datum.Blog.Application.Commands.Post;

public class DeletePostCommand : IRequest<bool>
{
    public DeletePostCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}