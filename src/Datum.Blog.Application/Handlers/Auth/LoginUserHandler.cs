using Datum.Blog.Application.Models.Request;
using Datum.Blog.Application.Models.Response;
using Datum.Blog.Domain.Interfaces;
using MediatR;

namespace Datum.Blog.Application.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginUserHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
                return new LoginUserResponse(token);  
            }
            catch (UnauthorizedAccessException)
            {
               
                return new LoginUserResponse(null); // Ou você pode lançar uma exceção personalizada, dependendo da sua necessidade
            }
        }
    }
}
