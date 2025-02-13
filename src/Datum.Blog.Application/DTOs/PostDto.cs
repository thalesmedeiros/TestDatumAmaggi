using Datum.Blog.Domain.Entities;

namespace Datum.Blog.Application.DTOs;

public class PostDto
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; } 
    public string? Conteudo { get; set; }
    public Guid AutorId { get; set; }
    public UserDto Autor { get; set; } = null!;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? UltimaAtualizacao { get; set; }
    public bool Publicado { get; set; }
}