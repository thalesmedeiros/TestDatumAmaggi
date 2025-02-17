using Datum.Blog.Application.Commands.User;
using Datum.Blog.Application.Interfaces;
using MediatR;

namespace Datum.Blog.Application.Handlers.User;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserService _service;

    public DeleteUserCommandHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(request.Id);
        return result;
    }
}