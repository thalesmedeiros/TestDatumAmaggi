using Datum.Blog.Application.Commands.User;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Datum.Blog.Api.Controllers;

/// <summary>
///     Controlador responsável pelo gerenciamento de usuários.
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    ///     Inicializa uma nova instância da classe <see cref="UserController" />.
    /// </summary>
    /// <param name="mediator">O mediador utilizado para envio de comandos e consultas.</param>
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Obtém os dados de um usuário pelo identificador especificado.
    /// </summary>
    /// <param name="id">O identificador único do usuário.</param>
    /// <returns>O <see cref="IActionResult" /> contendo os dados do usuário, se encontrado; caso contrário, retorna 404 Not Found.</returns>
    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdUserQuery(id);
        var userDto = await _mediator.Send(query);

        if (userDto is null)
        {
            return NotFound();
        }

        return Ok(userDto);
    }

    /// <summary>
    ///     Obtém todos os usuários cadastrados.
    /// </summary>
    /// <returns>Uma lista de usuários cadastrados.</returns>
    [HttpGet("get/all")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllUserQuery();
        var users = await _mediator.Send(query);
        return Ok(users);
    }

    /// <summary>
    ///     Cria um novo usuário com os dados especificados.
    /// </summary>
    /// <param name="request">O objeto contendo os detalhes do usuário.</param>
    /// <returns>Uma resposta 201 Created com a localização do novo usuário.</returns>
    [HttpPost("[action]")]
    public async Task<IActionResult> Add([FromBody] UserDto request)
    {
        var command = new CreateUserCommand(request.Nome!, request.Email!, request.SenhaHash!);
        var createdId = await _mediator.Send(command);
        return CreatedAtAction("GetById", new { id = createdId }, request);
    }

    /// <summary>
    ///     Atualiza os dados de um usuário pelo identificador especificado.
    /// </summary>
    /// <param name="id">O identificador único do usuário.</param>
    /// <param name="request">O objeto contendo os dados atualizados do usuário.</param>
    /// <returns>Um <see cref="IActionResult" /> indicando o resultado da operação de atualização.</returns>
    [HttpPut("[action]/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatUserDto request)
    {

        var command = new UpdateUserCommand(id, request.Nome, request.Email, request.Senha);
        var result = await _mediator.Send(command);

        return result ? Accepted() : NotFound();
    }

    /// <summary>
    ///     Exclui um usuário pelo identificador especificado.
    /// </summary>
    /// <param name="id">O identificador único do usuário a ser excluído.</param>
    /// <returns>Um <see cref="IActionResult" /> indicando o resultado da operação de exclusão.</returns>
    [HttpDelete("[action]/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command);

        return result ? Accepted() : NotFound();
    }
}
