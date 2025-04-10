using DietManagementSystem.Application.Features.User.Queries.GetUsers;
using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.User.Queries.GetUsers;

[TestFixture]
public class GetAllUsersQueryHandlerTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;
    private GetAllUsersQueryHandler _handler;
    private List<ApplicationUser> _users;
    private IdentityRole<Guid> _role;

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

        // Setup RoleManager mock
        var roleStoreMock = new Mock<IRoleStore<IdentityRole<Guid>>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole<Guid>>>(
            roleStoreMock.Object, null, null, null, null);

        _handler = new GetAllUsersQueryHandler(_userManagerMock.Object, _roleManagerMock.Object);

        // Setup test data
        _role = new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "TestRole" };
        _users = new List<ApplicationUser>
        {
            new() { Id = Guid.NewGuid(), UserName = "user1", Email = "user1@test.com", FullName = "User One", DateOfBirth = DateTime.Now.AddYears(-25) },
            new() { Id = Guid.NewGuid(), UserName = "user2", Email = "user2@test.com", FullName = "User Two", DateOfBirth = DateTime.Now.AddYears(-30) },
            new() { Id = Guid.NewGuid(), UserName = "user3", Email = "user3@test.com", FullName = "User Three", DateOfBirth = DateTime.Now.AddYears(-35) }
        };
    }

    [Test]
    public async Task Handle_WhenRoleExists_ShouldReturnPagedUsers()
    {
        // Arrange
        var query = new GetAllUsersQuery { Role = "TestRole", PageNumber = 1, PageSize = 2 };

        _roleManagerMock.Setup(x => x.FindByNameAsync(query.Role))
            .ReturnsAsync(_role);

        var queryableUsers = _users.AsQueryable();
        _userManagerMock.Setup(x => x.Users)
            .Returns(queryableUsers);

        _userManagerMock.Setup(x => x.GetUsersInRoleAsync(query.Role))
            .ReturnsAsync(_users.Take(2).ToList());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.MetaData.TotalCount, Is.EqualTo(2));
            Assert.That(result.MetaData.CurrentPage, Is.EqualTo(1));
            Assert.That(result.MetaData.PageSize, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task Handle_WhenRoleDoesNotExist_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllUsersQuery { Role = "NonExistentRole", PageNumber = 1, PageSize = 10 };

        _roleManagerMock.Setup(x => x.FindByNameAsync(query.Role))
            .ReturnsAsync((IdentityRole<Guid>)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Empty);
            Assert.That(result.MetaData.TotalCount, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task Handle_WhenNoUsersInRole_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllUsersQuery { Role = "TestRole", PageNumber = 1, PageSize = 10 };

        _roleManagerMock.Setup(x => x.FindByNameAsync(query.Role))
            .ReturnsAsync(_role);

        var queryableUsers = _users.AsQueryable();
        _userManagerMock.Setup(x => x.Users)
            .Returns(queryableUsers);

        _userManagerMock.Setup(x => x.GetUsersInRoleAsync(query.Role))
            .ReturnsAsync(new List<ApplicationUser>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Empty);
            Assert.That(result.MetaData.TotalCount, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var query = new GetAllUsersQuery { Role = "TestRole", PageNumber = 2, PageSize = 1 };

        _roleManagerMock.Setup(x => x.FindByNameAsync(query.Role))
            .ReturnsAsync(_role);

        var queryableUsers = _users.AsQueryable();
        _userManagerMock.Setup(x => x.Users)
            .Returns(queryableUsers);

        _userManagerMock.Setup(x => x.GetUsersInRoleAsync(query.Role))
            .ReturnsAsync(_users);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.MetaData.CurrentPage, Is.EqualTo(2));
            Assert.That(result.MetaData.TotalCount, Is.EqualTo(3));
            Assert.That(Math.Ceiling(result.MetaData.TotalCount / (double)result.MetaData.PageSize), Is.EqualTo(3));
        });
    }
} 