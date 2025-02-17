using Datum.Blog.Application.DTOs;
using Datum.Blog.Application.Handlers.User;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Queries.User;
using Moq;

namespace Datum.Blog.Tests.Application.Handlers
{
    public class GetByIdUserQueryHandlerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly GetByIdUserQueryHandler _handler;

        public GetByIdUserQueryHandlerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _handler = new GetByIdUserQueryHandler(_userServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Guid como Id
            var expectedUser = new UserDto
            {
                Id = userId,
                Nome = "John Doe",
                Email = "john.doe@example.com",
                SenhaHash = "hashedpassword123",
                DataCriacao = DateTime.UtcNow
            };

            _userServiceMock.Setup(service => service.GetByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            var query = new GetByIdUserQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result?.Id);
            Assert.Equal(expectedUser.Nome, result?.Nome);
            Assert.Equal(expectedUser.Email, result?.Email);
            Assert.Equal(expectedUser.SenhaHash, result?.SenhaHash);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Guid como Id
            _userServiceMock.Setup(service => service.GetByIdAsync(userId))
                .ReturnsAsync((UserDto?)null);

            var query = new GetByIdUserQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
