using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.DietPlan.Commands.Create;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.DietPlan.Commands.Create;

[TestFixture]
public class CreateDietPlanCommandHandlerTests
{
    private Mock<IDietPlanRepository> _dietPlanRepositoryMock;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private CreateDietPlanCommmandHandler _handler;

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

        _dietPlanRepositoryMock = new Mock<IDietPlanRepository>();
        _handler = new CreateDietPlanCommmandHandler(_dietPlanRepositoryMock.Object, _userManagerMock.Object);
    }

    [Test]
    public async Task Handle_WhenAllUsersExist_ShouldCreateDietPlan()
    {
        // Arrange
        var dietitianId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddMonths(3);
        var command = new CreateDietPlanCommmand(
            "Weight Loss Plan",
            startDate,
            endDate,
            80.5m,
            clientId)
        {
            DietitianId = dietitianId
        };

        var dietitian = new ApplicationUser
        {
            Id = dietitianId,
            Email = "dietitian@example.com",
            FullName = "Test Dietitian"
        };

        var client = new ApplicationUser
        {
            Id = clientId,
            Email = "client@example.com",
            FullName = "Test Client"
        };

        _userManagerMock.Setup(x => x.FindByIdAsync(dietitianId.ToString()))
            .ReturnsAsync(dietitian);

        _userManagerMock.Setup(x => x.FindByIdAsync(clientId.ToString()))
            .ReturnsAsync(client);

        _dietPlanRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.DietPlan>()))
            .Returns(Task.CompletedTask);

        _dietPlanRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dietPlanRepositoryMock.Verify(x => x.AddAsync(It.Is<Domain.Entities.DietPlan>(p =>
            p.Title == command.Title &&
            p.StartDate == command.StartDate &&
            p.EndDate == command.EndDate &&
            p.InitialWeight == command.InitialWeight &&
            p.ClientId == command.ClientId &&
            p.DietitianId == command.DietitianId)), Times.Once);

        _dietPlanRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenDietitianDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var dietitianId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddMonths(3);
        var command = new CreateDietPlanCommmand(
            "Weight Loss Plan",
            startDate,
            endDate,
            80.5m,
            clientId)
        {
            DietitianId = dietitianId
        };

        _userManagerMock.Setup(x => x.FindByIdAsync(dietitianId.ToString()))
            .ReturnsAsync((ApplicationUser)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Dietitian not found."));
        _dietPlanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.DietPlan>()), Times.Never);
    }

    [Test]
    public async Task Handle_WhenClientDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var dietitianId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddMonths(3);
        var command = new CreateDietPlanCommmand(
            "Weight Loss Plan",
            startDate,
            endDate,
            80.5m,
            clientId)
        {
            DietitianId = dietitianId
        };

        var dietitian = new ApplicationUser
        {
            Id = dietitianId,
            Email = "dietitian@example.com",
            FullName = "Test Dietitian"
        };

        _userManagerMock.Setup(x => x.FindByIdAsync(dietitianId.ToString()))
            .ReturnsAsync(dietitian);

        _userManagerMock.Setup(x => x.FindByIdAsync(clientId.ToString()))
            .ReturnsAsync((ApplicationUser)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Client not found."));
        _dietPlanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.DietPlan>()), Times.Never);
    }
}