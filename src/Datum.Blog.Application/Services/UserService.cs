using AutoMapper;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Datum.Blog.Application.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger, IPasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        _logger.LogInformation("Obtendo todos os usuários");
        var people = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(people);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Obtendo usuário com ID: {UserId}", id);

        var user = await _repository.GetByIdAsync(id);
        if (user is not null)
        {
            return _mapper.Map<UserDto>(user);
        }

        _logger.LogWarning("Usuário com ID: {UserId} não encontrado", id);

        return null;
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        _logger.LogInformation("Obtendo usuário com email: {UserEmail}", email);

        var user = await _repository.GetByEmailAsync(email);
        if (user is not null)
        {
            return _mapper.Map<UserDto>(user);
        }

        _logger.LogWarning("Usuário com email: {UserEmail} não encontrado", email);

        return null;
    }

    public async Task<Guid> AddAsync(UserDto data)
    {
        _logger.LogInformation("Adicionando novo usuário");

        var user = _mapper.Map<User>(data);
        user.DataCriacao = DateTime.UtcNow;
        user.Id = Guid.NewGuid();
        user.SenhaHash = _passwordHasher.HashPassword(null!, data.SenhaHash!);
        await _repository.AddAsync(user);

        _logger.LogInformation("Usuário adicionado com sucesso com ID: {UserId}", user.Id);

        return user.Id;
    }

    public async Task<bool> UpdateAsync(UserDto data)
    {
        _logger.LogInformation("Atualizando usuário com ID: {UserId}", data.Id);

        var user = await _repository.GetByIdAsync(data.Id);
        if (user is null)
        {
            _logger.LogWarning("Usuário com ID: {UserId} não encontrado. Atualização abortada", data.Id);
            return false;
        }

        _mapper.Map(data, user);
        user.DataCriacao = DateTime.UtcNow;
        user.SenhaHash = _passwordHasher.HashPassword(null!, data.SenhaHash!);
        await _repository.UpdateAsync(user);

        _logger.LogInformation("Usuário com ID: {UserId} atualizado com sucesso", data.Id);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deletando usuário com ID: {UserId}", id);

        var user = await _repository.GetByIdAsync(id);
        if (user is null)
        {
            _logger.LogWarning("Usuário com ID: {UserId} não encontrado. Deleção abortada", id);
            return false;
        }

        await _repository.DeleteAsync(id);

        _logger.LogInformation("Usuário com ID: {UserId} deletado com sucesso", id);

        return true;
    }
}
