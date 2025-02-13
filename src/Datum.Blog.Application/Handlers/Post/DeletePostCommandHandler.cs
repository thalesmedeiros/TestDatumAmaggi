using MediatR;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Commands.Post;

namespace Datum.Blog.Application.Handlers.Post;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
{
    private readonly IPostService _service;

    public DeletePostCommandHandler(IPostService service)
    {
        _service = service;
    }

    public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(request.Id);
        return result;
    }
}