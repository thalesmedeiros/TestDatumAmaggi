using Moq;
using Xunit;
using FluentAssertions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Datum.Blog.Application.Services;
using Datum.Blog.Application.DTOs;
using Datum.Blog.Domain.Entities;
using Datum.Blog.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Datum.Blog.Tests.Application.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnUserId_WhenUserIsAddedSuccessfully()
    {
        // Arrange
        var userDto = new UserDto { Id = Guid.NewGuid(), Email = "user@example.com", SenhaHash = "hashedPassword" };
        var user = new User { Id = userDto.Id, Email = userDto.Email, SenhaHash = userDto.SenhaHash, DataCriacao = DateTime.UtcNow };

        _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);
        _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _passwordHasherMock.Setup(hasher => hasher.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashedPassword");

        // Act
        var result = await _userService.AddAsync(userDto);

        // Assert
        result.Should().Be(user.Id);
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        _passwordHasherMock.Verify(hasher => hasher.HashPassword(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
    }
}
