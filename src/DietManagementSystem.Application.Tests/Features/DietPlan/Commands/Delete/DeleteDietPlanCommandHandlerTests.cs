using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.DietPlan.Commands.Delete;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.DietPlan.Commands.Delete;

[TestFixture]
public class DeleteDietPlanCommandHandlerTests
{
    private Mock<IDietPlanRepository> _dietPlanRepositoryMock;
    private DeleteDietPlanCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _dietPlanRepositoryMock = new Mock<IDietPlanRepository>();
        _handler = new DeleteDietPlanCommandHandler(_dietPlanRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenDietPlanExists_ShouldMarkAsDeleted()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var command = new DeleteDietPlanCommand(dietPlanId);

        var existingDietPlan = new Domain.Entities.DietPlan
        {
            Id = dietPlanId,
            Title = "Diet Plan to Delete",
            IsDeleted = false,
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
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
            Assert.That(existingDietPlan.IsDeleted, Is.True);
            Assert.That(existingDietPlan.UpdatedAt, Is.GreaterThan(DateTime.UtcNow.AddSeconds(-5)));
        });

        _dietPlanRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenDietPlanDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var command = new DeleteDietPlanCommand(dietPlanId);

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