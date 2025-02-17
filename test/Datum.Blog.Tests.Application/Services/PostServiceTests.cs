using AutoMapper;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Services;
using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Datum.Blog.Tests.Application.Services;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<PostService>> _loggerMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _repositoryMock = new Mock<IPostRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<PostService>>();
        _notificationServiceMock = new Mock<INotificationService>();
        _postService = new PostService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object, _notificationServiceMock.Object);
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnPost_WhenPostExists()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var post = new Post { Id = postId, Titulo = "Post 1", Conteudo = "Conteúdo 1" };
        _repositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(post);
        _mapperMock.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns(new PostDto { Titulo = "Post 1", Conteudo = "Conteúdo 1" });

        // Act
        var result = await _postService.GetByIdAsync(postId);

        // Assert
        result.Should().NotBeNull();
        result.Titulo.Should().Be("Post 1");
        result.Conteudo.Should().Be("Conteúdo 1");
    }

    [Fact]
    public async Task AddAsync_ShouldReturnPostId_WhenPostIsAdded()
    {
        // Arrange
        var postDto = new PostDto { Titulo = "Post 1", Conteudo = "Conteúdo 1" };
        var post = new Post { Id = Guid.NewGuid(), Titulo = "Post 1", Conteudo = "Conteúdo 1" };
        _mapperMock.Setup(m => m.Map<Post>(postDto)).Returns(post);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);
        _notificationServiceMock.Setup(n => n.NotifyAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await _postService.AddAsync(postDto);

        // Assert
        result.Should().NotBe(Guid.Empty);
        _notificationServiceMock.Verify(n => n.NotifyAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnEmptyGuid_WhenNotificationFails()
    {
        // Arrange
        var postDto = new PostDto { Titulo = "Post 1", Conteudo = "Conteúdo 1" };
        var post = new Post { Id = Guid.NewGuid(), Titulo = "Post 1", Conteudo = "Conteúdo 1" };
        _mapperMock.Setup(m => m.Map<Post>(postDto)).Returns(post);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);
        _notificationServiceMock.Setup(n => n.NotifyAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Erro"));

        // Act
        var result = await _postService.AddAsync(postDto);

        // Assert
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenPostIsUpdated()
    {
        // Arrange
        var postDto = new PostDto { Id = Guid.NewGuid(), Titulo = "Updated Post", Conteudo = "Updated Content", AutorId = Guid.NewGuid() };
        var post = new Post { Id = postDto.Id, Titulo = "Old Post", Conteudo = "Old Content", AutorId = postDto.AutorId };
        _repositoryMock.Setup(r => r.GetByIdAsync(postDto.Id)).ReturnsAsync(post);
        _mapperMock.Setup(m => m.Map(It.IsAny<PostDto>(), It.IsAny<Post>())).Verifiable();
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);

        // Act
        var result = await _postService.UpdateAsync(postDto);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenPostIsDeleted()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var post = new Post { Id = postId, Titulo = "Post 1", Conteudo = "Conteúdo 1" };
        _repositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(post);
        _repositoryMock.Setup(r => r.DeleteAsync(postId)).Returns(Task.CompletedTask);

        // Act
        var result = await _postService.DeleteAsync(postId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenPostNotFound()
    {
        // Arrange
        var postId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync((Post)null);

        // Act
        var result = await _postService.DeleteAsync(postId);

        // Assert
        result.Should().BeFalse();
    }
}
