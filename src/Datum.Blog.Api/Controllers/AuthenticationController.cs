using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Datum.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthenticationController(IAuthenticationService authenticationService, IPasswordHasher<User> passwordHasher,IUserService userService)
        {
            _authenticationService = authenticationService;
            _passwordHasher = passwordHasher;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username ou Password não podem ser vazios.");
            }

            try
            {
     
                var token = await _authenticationService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Credenciais inválidas.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest, [FromBody] string nome)
        {
            if (registerRequest == null || string.IsNullOrEmpty(registerRequest.Email) || string.IsNullOrEmpty(registerRequest.Password) || string.IsNullOrEmpty(nome))
            {
                return BadRequest("Todos os campos devem ser preenchidos.");
            }

            try
            {
                var existingUser = await _userService.GetByEmailAsync(registerRequest.Email);
                if (existingUser != null)
                {
                    return Conflict("Usuário já existe.");
                }

                var newUser = new UserDto
                {
                    Nome = nome,
                    Email = registerRequest.Email,
                    SenhaHash = _passwordHasher.HashPassword(null, registerRequest.Password),
                    DataCriacao = DateTime.UtcNow
                };

                await _userService.AddAsync(newUser);

                return Ok(new { Message = "Usuário registrado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
