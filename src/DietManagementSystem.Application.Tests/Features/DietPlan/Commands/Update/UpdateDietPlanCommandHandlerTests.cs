using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.DietPlan.Commands.Update;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.DietPlan.Commands.Update;

[TestFixture]
public class UpdateDietPlanCommandHandlerTests
{
    private Mock<IDietPlanRepository> _dietPlanRepositoryMock;
    private UpdateDietPlanCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _dietPlanRepositoryMock = new Mock<IDietPlanRepository>();
        _handler = new UpdateDietPlanCommandHandler(_dietPlanRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenDietPlanExists_ShouldUpdateDietPlan()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddMonths(3);

        var command = new UpdateDietPlanCommand(
            "Updated Diet Plan",
            startDate,
            endDate,
            75.5m,
            clientId)
        {
            Id = dietPlanId
        };

        var existingDietPlan = new Domain.Entities.DietPlan
        {
            Id = dietPlanId,
            Title = "Original Diet Plan",
            StartDate = DateTime.Now.AddDays(-7),
            EndDate = DateTime.Now.AddMonths(2),
            InitialWeight = 80.5m,
            ClientId = clientId
        };

        _dietPlanRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.DietPlan, bool>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync(existingDietPlan);

        _dietPlanRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(existingDietPlan.Title, Is.EqualTo(command.Title));
            Assert.That(existingDietPlan.StartDate, Is.EqualTo(command.StartDate));
            Assert.That(existingDietPlan.EndDate, Is.EqualTo(command.EndDate));
            Assert.That(existingDietPlan.InitialWeight, Is.EqualTo(command.InitialWeight));
        });

        _dietPlanRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenDietPlanDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddMonths(3);

        var command = new UpdateDietPlanCommand(
            "Updated Diet Plan",
            startDate,
            endDate,
            75.5m,
            clientId)
        {
            Id = dietPlanId
        };

        _dietPlanRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.DietPlan, bool>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync((Domain.Entities.DietPlan)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Diet plan not found."));
        _dietPlanRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}