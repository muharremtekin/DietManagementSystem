using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Meal.Queries.GetMealQuery;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;

namespace DietManagementSystem.Application.Tests.Features.Meal.Queries.GetMealQuery;

[TestFixture]
public class GetMealQueryHandlerTests
{
    private Mock<IMealRepository> _mockMealRepository;
    private GetMealQueryHandler _handler;
    private Domain.Entities.Meal _testMeal;
    private Guid _mealId;

    [SetUp]
    public void Setup()
    {
        _mockMealRepository = new Mock<IMealRepository>();
        _handler = new GetMealQueryHandler(_mockMealRepository.Object);

        // Test verisi hazırlama
        _mealId = Guid.NewGuid();
        var dietPlanId = Guid.NewGuid();

        _testMeal = new Domain.Entities.Meal
        {
            Id = _mealId,
            Title = "Breakfast",
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(9, 0, 0),
            Content = "Oatmeal with fruits",
            DietPlanId = dietPlanId
        };

        _mockMealRepository
            .Setup(r => r.GetSingleAsync(
                It.IsAny<Expression<Func<Domain.Entities.Meal, bool>>>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Domain.Entities.Meal, object>>[]>()))
            .ReturnsAsync(_testMeal);
    }

    [Test]
    public async Task Handle_WithValidId_ShouldReturnMealViewModel()
    {
        // Arrange
        var query = new Application.Features.Meal.Queries.GetMealQuery.GetMealQuery(_mealId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(_testMeal.Id));
            Assert.That(result.Title, Is.EqualTo(_testMeal.Title));
            Assert.That(result.StartTime, Is.EqualTo(_testMeal.StartTime));
            Assert.That(result.EndTime, Is.EqualTo(_testMeal.EndTime));
            Assert.That(result.Content, Is.EqualTo(_testMeal.Content));
            Assert.That(result.DietPlanId, Is.EqualTo(_testMeal.DietPlanId));
        });
    }

    [Test]
    public async Task Handle_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        _mockMealRepository
            .Setup(r => r.GetSingleAsync(
                It.IsAny<Expression<Func<Domain.Entities.Meal, bool>>>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Domain.Entities.Meal, object>>[]>()))
            .ReturnsAsync((Domain.Entities.Meal)null);

        var query = new Application.Features.Meal.Queries.GetMealQuery.GetMealQuery(invalidId);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(query, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo("Meal not found"));
    }

    [Test]
    public async Task Handle_ShouldCallGetSingleAsyncWithCorrectId()
    {
        // Arrange
        var query = new Application.Features.Meal.Queries.GetMealQuery.GetMealQuery(_mealId);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockMealRepository.Verify(
            x => x.GetSingleAsync(
                It.Is<Expression<Func<Domain.Entities.Meal, bool>>>(expr =>
                    expr.Compile()(new Domain.Entities.Meal { Id = _mealId })),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Domain.Entities.Meal, object>>[]>()),
            Times.Once);
    }
}