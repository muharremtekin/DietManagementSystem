using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Meal.Commands.Create;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;
using NUnit.Framework;

namespace DietManagementSystem.Application.Tests.Features.Meal.Commands.Create;

[TestFixture]
public class CreateMealCommandHandlerTests
{
    private Mock<IMealRepository> _mealRepositoryMock;
    private Mock<IDietPlanRepository> _dietPlanRepositoryMock;
    private CreateMealCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mealRepositoryMock = new Mock<IMealRepository>();
        _dietPlanRepositoryMock = new Mock<IDietPlanRepository>();
        _handler = new CreateMealCommandHandler(_mealRepositoryMock.Object, _dietPlanRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenDietPlanExists_ShouldCreateMeal()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var startTime = new TimeSpan(8, 0, 0); // 8:00 AM
        var endTime = new TimeSpan(9, 0, 0);   // 9:00 AM
        
        var command = new CreateMealCommand(
            dietPlanId,
            "Breakfast",
            startTime,
            endTime,
            "Oatmeal with fruits");

        var existingDietPlan = new Domain.Entities.DietPlan
        {
            Id = dietPlanId,
            Title = "Weight Loss Plan"
        };

        _dietPlanRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.DietPlan, bool>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync(existingDietPlan);

        _mealRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Meal>()))
            .Returns(Task.CompletedTask);

        _mealRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mealRepositoryMock.Verify(x => x.AddAsync(It.Is<Domain.Entities.Meal>(m =>
            m.Title == command.Title &&
            m.StartTime == command.StartTime &&
            m.EndTime == command.EndTime &&
            m.Content == command.Content &&
            m.DietPlanId == command.DietPlanId)), Times.Once);

        _mealRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenDietPlanDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var startTime = new TimeSpan(8, 0, 0);
        var endTime = new TimeSpan(9, 0, 0);
        
        var command = new CreateMealCommand(
            dietPlanId,
            "Breakfast",
            startTime,
            endTime,
            "Oatmeal with fruits");

        _dietPlanRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.DietPlan, bool>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync((Domain.Entities.DietPlan?)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Diet plan not found."));
        _mealRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Meal>()), Times.Never);
        _mealRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
} 