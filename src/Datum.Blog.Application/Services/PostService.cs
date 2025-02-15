using AutoMapper;
using Microsoft.Extensions.Logging;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;


namespace Datum.Blog.Application.Services;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly IMapper _mapper;
    private readonly IPostRepository _repository;
    private readonly INotificationService _notificationService;

    public PostService(IPostRepository repository, IMapper mapper, ILogger<PostService> logger, INotificationService notificationService)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<PostDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all Posts");
        var post = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PostDto>>(post);
    }

    public async Task<PostDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting Post with ID: {PostId}", id);

        var post = await _repository.GetByIdAsync(id);
        if (post is not null)
        {
            return _mapper.Map<PostDto>(post);
        }

        _logger.LogWarning("Post with ID: {PostId} not found", id);

        return null;
    }

    public async Task<Guid> AddAsync(PostDto data)
    {
        _logger.LogInformation("Adding new Post with Title: {Title} and IdPerson: {Conteudo}", data.Titulo, data.Conteudo);

        var post = _mapper.Map<Post>(data);
        post.DataCriacao = DateTime.UtcNow;
        post.Publicado = false;
        await _repository.AddAsync(post);

        await _notificationService.NotifyAsync($"Nova postagem: {post.Titulo}");

        _logger.LogInformation("Post added successfully with ID: {PostId}", post.Id);

        return post.Id;
    }

    public async Task<bool> UpdateAsync(PostDto data)
    {
        _logger.LogInformation("Updating Post with ID: {PostId}", data.Id);

        var post = await _repository.GetByIdAsync(data.Id);
        if (post == null || post.AutorId != data.AutorId) 
        {
            return false;
        }

        _mapper.Map(data, post);
        post.DataCriacao = DateTime.UtcNow;
        await _repository.UpdateAsync(post);

        _logger.LogInformation("Post with ID: {PostId} updated successfully", data.Id);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting Post with ID: {PostId}", id);

        var post = await _repository.GetByIdAsync(id);
        if (post is null)
        {
            _logger.LogWarning("Post with ID: {PostId} not found. Deletion aborted", id);
            return false;
        }

        await _repository.DeleteAsync(id);

        _logger.LogInformation("Post with ID: {PostId} deleted successfully", id);

        return true;
    }
}