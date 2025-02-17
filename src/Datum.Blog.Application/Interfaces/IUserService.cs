using Datum.Blog.Application.DTOs;

namespace Datum.Blog.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<Guid> AddAsync(UserDto data);
    Task<bool> UpdateAsync(UserDto data);
    Task<bool> DeleteAsync(Guid id);
}