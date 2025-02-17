using Datum.Blog.Domain.Entities;

namespace Datum.Blog.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User task);
    Task UpdateAsync(User task);
    Task DeleteAsync(Guid id);
}