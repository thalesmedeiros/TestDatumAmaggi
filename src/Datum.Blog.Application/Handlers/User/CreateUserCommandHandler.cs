using AutoMapper;
using Datum.Blog.Application.Commands.User;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using MediatR;

namespace Datum.Blog.Application.Handlers.Post;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IUserService _service;

    public CreateUserCommandHandler(IMapper mapper, IUserService service)
    {
        _mapper = mapper;
        _service = service;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var create = _mapper.Map<UserDto>(request);
        var id = await _service.AddAsync(create);
        return id;
    }
}