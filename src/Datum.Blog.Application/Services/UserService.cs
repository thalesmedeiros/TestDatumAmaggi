using AutoMapper;
using Microsoft.Extensions.Logging;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;

namespace Datum.Blog.Application.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all Users");
        var people = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(people);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting User with ID: {UserId}", id);

        var user = await _repository.GetByIdAsync(id);
        if (user is not null)
        {
            return _mapper.Map<UserDto>(user);
        }

        _logger.LogWarning("User with ID: {UserId} not found", id);

        return null;
    }

    public async Task<Guid> AddAsync(UserDto data)
    {
        _logger.LogInformation("Adding new User");

        var user = _mapper.Map<User>(data);
        user.DataCriacao = DateTime.UtcNow;
        await _repository.AddAsync(user);

        _logger.LogInformation("User added successfully with ID: {UserId}", user.Id);

        return user.Id;
    }

    public async Task<bool> UpdateAsync(UserDto data)
    {
        _logger.LogInformation("Updating Task with ID: {TaskId}", data.Id);

        var user = await _repository.GetByIdAsync(data.Id);
        if (user is null)
        {
            _logger.LogWarning("Task with ID: {TaskId} not found. Update aborted", data.Id);
            return false;
        }

        _mapper.Map(data, user);
        user.DataCriacao = DateTime.UtcNow;
        await _repository.UpdateAsync(user);

        _logger.LogInformation("Task with ID: {TaskId} updated successfully", data.Id);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting Task with ID: {TaskId}", id);

        var user = await _repository.GetByIdAsync(id);
        if (user is null)
        {
            _logger.LogWarning("Task with ID: {TaskId} not found. Deletion aborted", id);
            return false;
        }

        await _repository.DeleteAsync(id);

        _logger.LogInformation("Task with ID: {TaskId} deleted successfully", id);

        return true;
    }
}