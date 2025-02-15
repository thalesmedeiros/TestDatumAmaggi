using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;
using Datum.Blog.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Datum.Blog.Infrastructure.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtService _jwtService; 

        public AuthenticationService(IConfiguration configuration, IUserRepository userRepository, IPasswordHasher<User> passwordHasher, JwtService jwtService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService; 
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Usuário não encontrado.");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.SenhaHash, password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Senha incorreta.");
            }

            var token = _jwtService.GenerateJwtToken(user); 
            return token;
        }
    }
}
