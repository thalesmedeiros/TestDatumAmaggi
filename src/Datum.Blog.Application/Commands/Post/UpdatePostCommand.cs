using MediatR;

namespace Datum.Blog.Application.Commands.Post;

public class UpdatePostCommand : IRequest<bool>
{
    public Guid? Id { get; private set; }
    public Guid AutorId { get; set; }
    public string? Titulo { get; private set; }
    public string? Conteudo { get; private set; }
    public bool? Publicado { get; private set; }

    public UpdatePostCommand(string? titulo, string? conteudo, bool? publicado, Guid? id, Guid autorId)
    {
        Titulo = titulo;
        Conteudo = conteudo;
        Publicado = publicado;
        Id = id;
        Id = id;
        AutorId = autorId;
    }
}
