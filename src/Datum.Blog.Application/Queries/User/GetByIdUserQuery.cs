using MediatR;
using Datum.Blog.Application.DTOs;

namespace Datum.Blog.Application.Queries.User;

public class GetByIdUserQuery : IRequest<UserDto?>
{
    public GetByIdUserQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}