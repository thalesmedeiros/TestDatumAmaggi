using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;
using Datum.Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Datum.Blog.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User post)
    {
        await _context.Users.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User post)
    {
        _context.Users.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var post = await GetByIdAsync(id);
        if (post != null)
        {
            _context.Users.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}