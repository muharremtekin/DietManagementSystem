using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Progress.Commands.Create;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.Progress.Commands.Create;

[TestFixture]
public class CreateProgressCommandHandlerTests
{
    private Mock<IProgressRepository> _progressRepositoryMock;
    private Mock<IDietPlanRepository> _dietPlanRepositoryMock;
    private CreateProgressCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _progressRepositoryMock = new Mock<IProgressRepository>();
        _dietPlanRepositoryMock = new Mock<IDietPlanRepository>();
        _handler = new CreateProgressCommandHandler(_progressRepositoryMock.Object, _dietPlanRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenDietPlanExists_ShouldCreateProgress()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var date = DateTime.Now;
        
        var command = new CreateProgressCommand(
            dietPlanId,
            date,
            75.5m,
            "Making good progress");

        var existingDietPlan = new Domain.Entities.DietPlan
        {
            Id = dietPlanId,
            Title = "Weight Loss Plan",
            ClientId = clientId,
            Client = new ApplicationUser { Id = clientId }
        };

        _dietPlanRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.DietPlan, bool>>>(),
            It.IsAny<bool>(),
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.DietPlan, object>>[]>()))
            .ReturnsAsync(existingDietPlan);

        _progressRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Progress>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _progressRepositoryMock.Verify(x => x.AddAsync(It.Is<Domain.Entities.Progress>(p =>
            p.DietPlanId == command.DietPlanId &&
            p.Date == command.Date &&
            p.Weight == command.Weight &&
            p.Notes == command.Notes &&
            p.ClientId == clientId)), Times.Once);
    }

    [Test]
    public async Task Handle_WhenDietPlanDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var date = DateTime.Now;
        
        var command = new CreateProgressCommand(
            dietPlanId,
            date,
            75.5m,
            "Making good progress");

        _dietPlanRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.DietPlan, bool>>>(),
            It.IsAny<bool>(),
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.DietPlan, object>>[]>()))
            .ReturnsAsync((Domain.Entities.DietPlan?)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Diet plan not found"));
        _progressRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Progress>()), Times.Never);
    }
} 