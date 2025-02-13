using Datum.Blog.Application.Commands.Post;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Queries.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Datum.Blog.Api.Controllers;

/// <summary>
///     Controlador responsável pelo gerenciamento de postagens.
/// </summary>
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    ///     Inicializa uma nova instância da classe <see cref="PostController" />.
    /// </summary>
    /// <param name="mediator">O mediador utilizado para envio de comandos e consultas.</param>
    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Obtém os dados de uma postagem pelo identificador especificado.
    /// </summary>
    /// <param name="id">O identificador único da postagem.</param>
    /// <returns>  <see cref="IActionResult" /> contendo os dados da postagem, se encontrada; caso contrário, retorna 404 Not Found.</returns>
    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdPostQuery(id);
        var postDto = await _mediator.Send(query);

        if (postDto is null)
        {
            return NotFound();
        }

        return Ok(postDto);
    }

    /// <summary>
    ///     Obtém todas as postagens cadastradas.
    /// </summary>
    /// <returns>Uma lista de postagens cadastradas.</returns>
    [HttpGet("get/all")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllPostQuery();
        var posts = await _mediator.Send(query);
        return Ok(posts);
    }

    /// <summary>
    ///     Cria uma nova postagem com os dados especificados.
    /// </summary>
    /// <param name="request">O objeto contendo os detalhes da postagem.</param>
    /// <returns>Uma resposta 201 Created com a localização da nova postagem.</returns>
    [HttpPost("[action]")]
    public async Task<IActionResult> Add([FromBody] PostDto request)
    {
        var command = new CreatePostCommand(request.Titulo!, request.Conteudo!, request.AutorId);
        var createdId = await _mediator.Send(command);
        return CreatedAtAction("GetById", new { id = createdId }, request);
    }

    /// <summary>
    ///     Atualiza os dados de uma postagem pelo identificador especificado.
    /// </summary>
    /// <param name="id">O identificador único da postagem.</param>
    /// <param name="request">O objeto contendo os dados atualizados da postagem.</param>
    /// <returns>Um <see cref="IActionResult" /> indicando o resultado da operação de atualização.</returns>
    [HttpPut("[action]/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatPostDto request)
    {

        var command = new UpdatePostCommand(request.Titulo, request.Conteudo, request.Publicado, id, request.AutorId);
        var result = await _mediator.Send(command);

        return result ? Accepted() : NotFound();
    }

    /// <summary>
    ///     Exclui uma postagem com o identificador especificado.
    /// </summary>
    /// <param name="id">O identificador único da postagem a ser excluída.</param>
    /// <returns>Um <see cref="IActionResult" /> indicando o resultado da operação de exclusão.</returns>
    [HttpDelete("[action]/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeletePostCommand(id);
        var result = await _mediator.Send(command);

        return result ? Accepted() : NotFound();
    }
}
