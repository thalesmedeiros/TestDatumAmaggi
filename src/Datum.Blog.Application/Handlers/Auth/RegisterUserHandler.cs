using Datum.Blog.Application.Commands.Auth;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Datum.Blog.Application.Handlers.Auth
{

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserDto> _passwordHasher;
        private readonly IUserService _service;

        public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher<UserDto> passwordHasher, IUserService service)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _service = service;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new UserDto
            {
                Nome = request.Nome,
                Email = request.Email
            };

            user.SenhaHash = _passwordHasher.HashPassword(user, request.Senha);

            var createdUser = await _service.AddAsync(user);
            return createdUser;
        }
    }

}
