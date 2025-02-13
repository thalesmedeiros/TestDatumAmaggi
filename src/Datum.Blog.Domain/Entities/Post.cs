using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datum.Blog.Domain.Entities;

[Table("Post")]
public class Post
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public Guid AutorId { get; set; } 
    public User Autor { get; set; } = null!;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? UltimaAtualizacao { get; set; }
    public bool Publicado { get; set; } = false;
}
