namespace Datum.Blog.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; } 
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? SenhaHash { get; set; } 
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    //public ICollection<Post> Posts { get; set; } = new List<Post>();
}