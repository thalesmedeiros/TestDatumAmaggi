using MediatR;
using Datum.Blog.Application.DTOs;

namespace Datum.Blog.Application.Queries.Post;

public class GetByIdPostQuery : IRequest<PostDto?>
{
    public GetByIdPostQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}