using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.User.Commands.Create;
using DietManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.User.Commands.Create;

[TestFixture]
public class CreateUserCommandHandlerTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;
    private CreateUserCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        // Setup RoleManager mock
        var roleStoreMock = new Mock<IRoleStore<IdentityRole<Guid>>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole<Guid>>>(
            roleStoreMock.Object, null, null, null, null);

        _handler = new CreateUserCommandHandler(_userManagerMock.Object, _roleManagerMock.Object);
    }

    [Test]
    public async Task Handle_WhenUserDoesNotExist_ShouldCreateUser()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            UserName = "testuser",
            FullName = "Test User",
            Password = "Password123!",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Role = "User"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((ApplicationUser)null);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), command.Password))
            .ReturnsAsync(IdentityResult.Success);

        _roleManagerMock.Setup(x => x.RoleExistsAsync(command.Role))
            .ReturnsAsync(true);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), command.Role))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), command.Password), Times.Once);
        _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), command.Role), Times.Once);
    }

    [Test]
    public async Task Handle_WhenUserExists_ShouldThrowBadRequestException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "existing@example.com",
            UserName = "existinguser",
            FullName = "Existing User",
            Password = "Password123!",
            DateOfBirth = DateTime.Now.AddYears(-20)
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(new ApplicationUser { Email = command.Email });

        // Act & Assert
        var exception = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
        
        Assert.That(exception.Message, Is.EqualTo("User already exists."));
    }

    [Test]
    public async Task Handle_WhenRoleDoesNotExist_ShouldCreateRoleAndAddUserToIt()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            UserName = "testuser",
            FullName = "Test User",
            Password = "Password123!",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Role = "NewRole"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((ApplicationUser)null);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), command.Password))
            .ReturnsAsync(IdentityResult.Success);

        _roleManagerMock.Setup(x => x.RoleExistsAsync(command.Role))
            .ReturnsAsync(false);

        _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole<Guid>>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), command.Role))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _roleManagerMock.Verify(x => x.CreateAsync(It.IsAny<IdentityRole<Guid>>()), Times.Once);
        _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), command.Role), Times.Once);
    }
}