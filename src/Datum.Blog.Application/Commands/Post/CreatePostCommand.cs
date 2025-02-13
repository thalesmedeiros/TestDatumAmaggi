using MediatR;

namespace Datum.Blog.Application.Commands.Post;

public class CreatePostCommand : IRequest<Guid>
{
    public string Titulo { get; set; }
    public string Conteudo { get; set; }
    public Guid AutorId { get; set; }

    public CreatePostCommand(string titulo, string conteudo, Guid autorId)
    {
        Titulo = titulo;
        Conteudo = conteudo;
        AutorId = autorId;
    }
}
