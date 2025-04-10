using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Meal.Commands.Update;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.Meal.Commands.Update;

[TestFixture]
public class UpdateMealCommandHandlerTests
{
    private Mock<IMealRepository> _mealRepositoryMock;
    private UpdateMealCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mealRepositoryMock = new Mock<IMealRepository>();
        _handler = new UpdateMealCommandHandler(_mealRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenMealExists_ShouldUpdateMeal()
    {
        // Arrange
        var mealId = Guid.NewGuid();
        var startTime = new TimeSpan(8, 0, 0); // 8:00 AM
        var endTime = new TimeSpan(9, 0, 0);   // 9:00 AM
        
        var command = new UpdateMealCommand(
            mealId,
            "Updated Breakfast",
            startTime,
            endTime,
            "Updated oatmeal with berries");

        var existingMeal = new Domain.Entities.Meal
        {
            Id = mealId,
            Title = "Original Breakfast",
            StartTime = new TimeSpan(7, 0, 0),
            EndTime = new TimeSpan(8, 0, 0),
            Content = "Original oatmeal"
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
            Assert.That(existingMeal.Title, Is.EqualTo(command.Title));
            Assert.That(existingMeal.StartTime, Is.EqualTo(command.StartTime));
            Assert.That(existingMeal.EndTime, Is.EqualTo(command.EndTime));
            Assert.That(existingMeal.Content, Is.EqualTo(command.Content));
        });

        _mealRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenMealDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var mealId = Guid.NewGuid();
        var startTime = new TimeSpan(8, 0, 0);
        var endTime = new TimeSpan(9, 0, 0);
        
        var command = new UpdateMealCommand(
            mealId,
            "Updated Breakfast",
            startTime,
            endTime,
            "Updated oatmeal with berries");

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