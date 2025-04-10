using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.User.Commands.Update;
using DietManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace DietManagementSystem.Application.Tests.Features.User.Commands.Update;

[TestFixture]
public class UpdateUserCommandHandlerTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private UpdateUserCommandHandler _handler;
    private IPasswordHasher<ApplicationUser> _passwordHasher;

    [SetUp]
    public void Setup()
    {
        // Setup UserManager mock with a real password hasher
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _passwordHasher = new PasswordHasher<ApplicationUser>();
        
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            new OptionsManager<IdentityOptions>(
                new OptionsFactory<IdentityOptions>(
                    new List<IConfigureOptions<IdentityOptions>>(),
                    new List<IPostConfigureOptions<IdentityOptions>>())),
            _passwordHasher,
            null, null, null, null, null, null);

        _handler = new UpdateUserCommandHandler(_userManagerMock.Object);
    }

    [Test]
    public async Task Handle_WhenUserExists_ShouldUpdateUser()
    {
        // Arrange
        var command = new UpdateUserCommand(
            "updated@example.com",
            "newpassword",
            "Updated User",
            DateTime.Now.AddYears(-25));

        var existingUser = new ApplicationUser
        {
            Email = "updated@example.com",
            FullName = "Old User",
            DateOfBirth = DateTime.Now.AddYears(-20)
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u => 
            u.FullName == command.FullName && 
            u.DateOfBirth == command.DateOfBirth &&
            u.PasswordHash != null)), Times.Once);
    }

    [Test]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateUserCommand(
            "nonexistent@example.com",
            "password",
            "Test User",
            DateTime.Now.AddYears(-25));

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
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
        var command = new UpdateUserCommand(
            "test@example.com",
            "password",
            "Test User",
            DateTime.Now.AddYears(-25));

        var existingUser = new ApplicationUser
        {
            Email = command.Email,
            FullName = "Old User",
            DateOfBirth = DateTime.Now.AddYears(-20)
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

        // Act & Assert
        var exception = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Failed to update user: Update failed"));
    }
}