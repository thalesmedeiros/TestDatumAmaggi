using MediatR;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Queries.Post;

namespace Datum.Blog.Application.Handlers.Post;

public class GetByIdPostQueryHandler : IRequestHandler<GetByIdPostQuery, PostDto?>
{
    private readonly IPostService _service;

    public GetByIdPostQueryHandler(IPostService service)
    {
        _service = service;
    }

    public async Task<PostDto?> Handle(GetByIdPostQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(request.Id);
    }
}