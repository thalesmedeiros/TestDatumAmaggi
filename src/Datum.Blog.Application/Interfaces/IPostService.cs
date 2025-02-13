using Datum.Blog.Application.DTOs;

namespace Datum.Blog.Application.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostDto>> GetAllAsync();
    Task<PostDto?> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(PostDto data);
    Task<bool> UpdateAsync(PostDto data);
    Task<bool> DeleteAsync(Guid id);
}