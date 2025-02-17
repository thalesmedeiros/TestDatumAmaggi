using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Queries.User;
using MediatR;

namespace Datum.Blog.Application.Handlers.User;

public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, UserDto?>
{
    private readonly IUserService _service;

    public GetByIdUserQueryHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<UserDto?> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(request.Id);
    }
}