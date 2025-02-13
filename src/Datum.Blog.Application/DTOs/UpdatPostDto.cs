namespace Datum.Blog.Application.DTOs;

public class UpdatPostDto
{
    public string? Titulo { get; set; } 
    public string? Conteudo { get; set; }
    public Guid AutorId { get; set; }
    public bool Publicado { get; set; }
}