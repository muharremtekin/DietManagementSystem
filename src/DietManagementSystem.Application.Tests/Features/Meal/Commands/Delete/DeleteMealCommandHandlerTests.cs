using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Meal.Commands.Delete;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.Meal.Commands.Delete;

[TestFixture]
public class DeleteMealCommandHandlerTests
{
    private Mock<IMealRepository> _mealRepositoryMock;
    private DeleteMealCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mealRepositoryMock = new Mock<IMealRepository>();
        _handler = new DeleteMealCommandHandler(_mealRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenMealExists_ShouldSoftDeleteMeal()
    {
        // Arrange
        var mealId = Guid.NewGuid();
        var command = new DeleteMealCommand(mealId);

        var existingMeal = new Domain.Entities.Meal
        {
            Id = mealId,
            Title = "Breakfast",
            IsDeleted = false
        };

        _mealRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Meal, bool>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync(existingMeal);

        _mealRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(existingMeal.IsDeleted, Is.True);
            Assert.That(existingMeal.UpdatedAt, Is.Not.EqualTo(default(DateTime)));
        });

        _mealRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenMealDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var mealId = Guid.NewGuid();
        var command = new DeleteMealCommand(mealId);

        _mealRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Meal, bool>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync((Domain.Entities.Meal?)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Meal not found"));
        _mealRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
} 