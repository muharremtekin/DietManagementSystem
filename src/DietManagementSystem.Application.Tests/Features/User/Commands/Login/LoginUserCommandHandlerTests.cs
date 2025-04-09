using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.User.Commands.Login;
using DietManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.User.Commands.Login
{
    [TestFixture]
    public class LoginUserCommandHandlerTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IConfiguration> _configurationMock;
        private LoginUserCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x["Jwt:Key"]).Returns("your-secret-key-here-which-is-long-enough");
            _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("test-issuer");
            _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("test-audience");
            _configurationMock.Setup(x => x["Jwt:ExpireDays"]).Returns("7");

            _handler = new LoginUserCommandHandler(_userManagerMock.Object, _configurationMock.Object);
        }

        [Test]
        public async Task Handle_WithValidUsernameAndPassword_ReturnsLoginDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var username = "testuser";
            var password = "Test123!";
            var user = new ApplicationUser { Id = userId, UserName = username };
            var roles = new List<string> { "User" };

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, password))
                .ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);

            var command = new LoginUserCommand(username, null, password);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Null);
            Assert.That(result.TokenExpiryTime, Is.GreaterThan(DateTime.UtcNow));
            _userManagerMock.Verify(x => x.FindByNameAsync(username), Times.Once);
            _userManagerMock.Verify(x => x.CheckPasswordAsync(user, password), Times.Once);
        }

        [Test]
        public async Task Handle_WithValidEmailAndPassword_ReturnsLoginDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "musteri@gmail.com";
            var password = "Asd123!";
            var user = new ApplicationUser { Id = userId, Email = email, UserName = email };
            var roles = new List<string> { "Client" };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, password))
                .ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);

            var command = new LoginUserCommand(null, email, password);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Null);
            Assert.That(result.TokenExpiryTime, Is.GreaterThan(DateTime.UtcNow));
            _userManagerMock.Verify(x => x.CheckPasswordAsync(user, password), Times.Once);
            _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
        }

        [Test]
        public void Handle_WithInvalidUsername_ThrowsBadRequestException()
        {
            // Arrange
            var username = "nonexistentuser";
            var password = "Test123!";

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync((ApplicationUser)null);

            var command = new LoginUserCommand(username, null, password);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_WithInvalidPassword_ThrowsBadRequestException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var username = "testuser";
            var password = "WrongPassword";
            var user = new ApplicationUser { Id = userId, UserName = username };

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, password))
                .ReturnsAsync(false);

            var command = new LoginUserCommand(username, null, password);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithMultipleRoles_IncludesAllRolesInToken()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var username = "testuser";
            var password = "Test123!";
            var user = new ApplicationUser { Id = userId, UserName = username };
            var roles = new List<string> { "User", "Admin" };

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, password))
                .ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);

            var command = new LoginUserCommand(username, null, password);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Null);
            Assert.That(result.TokenExpiryTime, Is.GreaterThan(DateTime.UtcNow));
            _userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
        }
    }
}