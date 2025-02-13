using AutoMapper;
using MediatR;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Commands.Post;

namespace Datum.Blog.Application.Handlers.Post;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IPostService _service;

    public UpdatePostCommandHandler(IMapper mapper, IPostService service)
    {
        _mapper = mapper;
        _service = service;
    }

    public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var update = _mapper.Map<PostDto>(request);
        var result = await _service.UpdateAsync(update);
        return result;
    }
}