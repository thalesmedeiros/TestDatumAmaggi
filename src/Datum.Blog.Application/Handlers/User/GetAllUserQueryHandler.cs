using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Queries.User;
using MediatR;

namespace Datum.Blog.Application.Handlers.User;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<UserDto>>
{
    private readonly IUserService _service;

    public GetAllUserQueryHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<List<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var enumerable = await _service.GetAllAsync();
        return enumerable.ToList();
    }
}