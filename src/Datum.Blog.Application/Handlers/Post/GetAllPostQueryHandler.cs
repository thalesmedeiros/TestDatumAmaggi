using MediatR;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Queries.Post;

namespace Datum.Blog.Application.Handlers.Post;

public class GetAllPostQueryHandler : IRequestHandler<GetAllPostQuery, List<PostDto>>
{
    private readonly IPostService _service;

    public GetAllPostQueryHandler(IPostService service)
    {
        _service = service;
    }

    public async Task<List<PostDto>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
    {
        var enumerable = await _service.GetAllAsync();
        return enumerable.ToList();
    }
}