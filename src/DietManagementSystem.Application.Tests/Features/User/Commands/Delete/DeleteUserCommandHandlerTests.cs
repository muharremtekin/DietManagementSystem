using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.User.Commands.Delete;
using DietManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace DietManagementSystem.Application.Tests.Features.User.Commands.Delete;

[TestFixture]
public class DeleteUserCommandHandlerTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private DeleteUserCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            new OptionsManager<IdentityOptions>(
                new OptionsFactory<IdentityOptions>(
                    new List<IConfigureOptions<IdentityOptions>>(),
                    new List<IPostConfigureOptions<IdentityOptions>>())),
            new PasswordHasher<ApplicationUser>(),
            null, null, null, null, null, null);

        _handler = new DeleteUserCommandHandler(_userManagerMock.Object);
    }

    [Test]
    public async Task Handle_WhenUserExists_ShouldMarkUserAsDeleted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);

        var existingUser = new ApplicationUser
        {
            Id = userId,
            Email = "test@example.com",
            FullName = "Test User",
            IsDeleted = false
        };

        _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(existingUser);

        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u => 
            u.Id == userId && 
            u.IsDeleted == true)), Times.Once);
    }

    [Test]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);

        _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync((ApplicationUser)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
        
        Assert.That(exception.Message, Is.EqualTo("User not found."));
    }

    [Test]
    public async Task Handle_WhenUpdateFails_ShouldThrowBadRequestException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);

        var existingUser = new ApplicationUser
        {
            Id = userId,
            Email = "test@example.com",
            FullName = "Test User",
            IsDeleted = false
        };

        _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(existingUser);

        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

        // Act & Assert
        var exception = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Failed to update user: Update failed"));
    }
} 