using AutoMapper;
using MediatR;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Commands.Post;

namespace Datum.Blog.Application.Handlers.Post;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IPostService _service;

    public CreatePostCommandHandler(IMapper mapper, IPostService service)
    {
        _mapper = mapper;
        _service = service;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var create = _mapper.Map<PostDto>(request);
        var id = await _service.AddAsync(create);
        return id;
    }
}