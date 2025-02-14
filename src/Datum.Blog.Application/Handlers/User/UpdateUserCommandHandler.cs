using AutoMapper;
using Datum.Blog.Application.Commands.User;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using MediatR;

namespace Datum.Blog.Application.Handlers.User;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IUserService _service;

    public UpdateUserCommandHandler(IMapper mapper, IUserService service)
    {
        _mapper = mapper;
        _service = service;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var update = _mapper.Map<UserDto>(request);
        var result = await _service.UpdateAsync(update);
        return result;
    }
}