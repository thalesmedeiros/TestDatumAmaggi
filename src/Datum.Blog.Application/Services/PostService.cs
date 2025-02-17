using AutoMapper;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Datum.Blog.Application.Services
{
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
            _logger.LogInformation("Obtendo todas as postagens");
            var post = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PostDto>>(post);
        }

        public async Task<PostDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Obtendo postagem com ID: {PostId}", id);

            var post = await _repository.GetByIdAsync(id);
            if (post is not null)
            {
                return _mapper.Map<PostDto>(post);
            }

            _logger.LogWarning("Postagem com ID: {PostId} não encontrada", id);

            return null;
        }

        public async Task<Guid> AddAsync(PostDto data)
        {
            _logger.LogInformation("Adicionando nova postagem com Título: {Title} e Conteúdo: {Conteudo}", data.Titulo, data.Conteudo);

            var post = _mapper.Map<Post>(data);
            post.DataCriacao = DateTime.UtcNow;
            post.Publicado = false;
            await _repository.AddAsync(post);

            try
            {

                await _notificationService.NotifyAsync($"Nova postagem: {post.Titulo}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar notificação para a nova postagem");
                return Guid.Empty;  
            }

            _logger.LogInformation("Postagem adicionada com sucesso com ID: {PostId}", post.Id);
            return post.Id;
        }


        public async Task<bool> UpdateAsync(PostDto data)
        {
            _logger.LogInformation("Atualizando postagem com ID: {PostId}", data.Id);

            var post = await _repository.GetByIdAsync(data.Id);
            if (post == null || post.AutorId != data.AutorId)
            {
                return false;
            }

            _mapper.Map(data, post);
            post.DataCriacao = DateTime.UtcNow;
            await _repository.UpdateAsync(post);

            _logger.LogInformation("Postagem com ID: {PostId} atualizada com sucesso", data.Id);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deletando postagem com ID: {PostId}", id);

            var post = await _repository.GetByIdAsync(id);
            if (post is null)
            {
                _logger.LogWarning("Postagem com ID: {PostId} não encontrada. Deleção abortada", id);
                return false;
            }

            await _repository.DeleteAsync(id);

            _logger.LogInformation("Postagem com ID: {PostId} deletada com sucesso", id);

            return true;
        }
    }
}
