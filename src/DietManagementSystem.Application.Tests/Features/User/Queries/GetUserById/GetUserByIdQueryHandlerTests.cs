using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.User.Queries.GetUserById;
using DietManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.User.Queries.GetUserById;

[TestFixture]
public class GetUserByIdQueryHandlerTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private GetUserByIdQueryHandler _handler;
    private ApplicationUser _testUser;

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

        _handler = new GetUserByIdQueryHandler(_userManagerMock.Object);

        // Setup test user
        _testUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            DateOfBirth = DateTime.Now.AddYears(-25)
        };
    }

    [Test]
    public async Task Handle_WhenUserExists_ShouldReturnUserViewModel()
    {
        // Arrange
        var query = new GetUserByIdQuery(_testUser.Id);

        _userManagerMock.Setup(x => x.FindByIdAsync(_testUser.Id.ToString()))
            .ReturnsAsync(_testUser);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(_testUser.Id));
            Assert.That(result.Email, Is.EqualTo(_testUser.Email));
            Assert.That(result.FullName, Is.EqualTo(_testUser.FullName));
            Assert.That(result.DateOfBirth, Is.EqualTo(_testUser.DateOfBirth));
        });
    }

    [Test]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();
        var query = new GetUserByIdQuery(nonExistentUserId);

        _userManagerMock.Setup(x => x.FindByIdAsync(nonExistentUserId.ToString()))
            .ReturnsAsync((ApplicationUser)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(query, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("User not found."));
    }

    [Test]
    public async Task Handle_WhenUserIsDeleted_ShouldThrowNotFoundException()
    {
        // Arrange
        _testUser.IsDeleted = true;
        var query = new GetUserByIdQuery(_testUser.Id);

        _userManagerMock.Setup(x => x.FindByIdAsync(_testUser.Id.ToString()))
            .ReturnsAsync(_testUser);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(query, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("User not found."));
    }
} 