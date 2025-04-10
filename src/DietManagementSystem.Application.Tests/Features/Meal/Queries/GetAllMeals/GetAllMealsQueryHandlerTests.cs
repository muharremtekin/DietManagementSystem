using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Meal.Queries.GetAllMeals;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace DietManagementSystem.Application.Tests.Features.Meal.Queries.GetAllMeals;

[TestFixture]
public class GetAllMealsQueryHandlerTests
{
    private Mock<IMealRepository> _mockMealRepository;
    private Mock<IDietPlanRepository> _mockDietPlanRepository;
    private GetAllMealsQueryHandler _handler;
    private List<Domain.Entities.Meal> _meals;
    private Domain.Entities.DietPlan _dietPlan;
    private Guid _userId;

    [SetUp]
    public void Setup()
    {
        _mockMealRepository = new Mock<IMealRepository>();
        _mockDietPlanRepository = new Mock<IDietPlanRepository>();
        _handler = new GetAllMealsQueryHandler(_mockMealRepository.Object, _mockDietPlanRepository.Object);

        // Test verisi hazırlama
        _userId = Guid.NewGuid();
        var client = new ApplicationUser { Id = _userId, FullName = "John Doe" };
        
        _dietPlan = new Domain.Entities.DietPlan
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            Client = client,
            Title = "Test Diet Plan"
        };

        _meals = new List<Domain.Entities.Meal>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Breakfast",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(9, 0, 0),
                Content = "Oatmeal with fruits",
                DietPlanId = _dietPlan.Id,
                DietPlan = _dietPlan
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Lunch",
                StartTime = new TimeSpan(12, 0, 0),
                EndTime = new TimeSpan(13, 0, 0),
                Content = "Grilled chicken salad",
                DietPlanId = _dietPlan.Id,
                DietPlan = _dietPlan
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Dinner",
                StartTime = new TimeSpan(19, 0, 0),
                EndTime = new TimeSpan(20, 0, 0),
                Content = "Fish with vegetables",
                DietPlanId = _dietPlan.Id,
                DietPlan = _dietPlan
            }
        };

        // Mock repository setup
        var mockDbSet = _meals.AsQueryable().ToMockDbSet();
        _mockMealRepository.Setup(r => r.AsQueryable()).Returns(mockDbSet.Object);
        
        _mockDietPlanRepository
            .Setup(r => r.GetSingleAsync(It.IsAny<Expression<Func<Domain.Entities.DietPlan, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Domain.Entities.DietPlan, object>>[]>()))
            .ReturnsAsync(_dietPlan);
    }

    [Test]
    public async Task Handle_WithNoFilters_ShouldReturnAllMeals()
    {
        // Arrange
        var query = new GetAllMealsQuery(null, null, null, null)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.MetaData.TotalCount, Is.EqualTo(3));
    }

    [Test]
    public async Task Handle_WithUserIdFilter_ShouldReturnUserMeals()
    {
        // Arrange
        var query = new GetAllMealsQuery(_userId, null, null, null)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.All(m => m.DietPlanId == _dietPlan.Id), Is.True);
    }

    [Test]
    public async Task Handle_WithDietPlanIdFilter_ShouldReturnDietPlanMeals()
    {
        // Arrange
        var query = new GetAllMealsQuery(null, _dietPlan.Id, null, null)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.All(m => m.DietPlanId == _dietPlan.Id), Is.True);
    }

    [Test]
    public async Task Handle_WithTimeRangeFilter_ShouldReturnMealsInRange()
    {
        // Arrange
        var startDate = DateTime.Today.AddHours(7); // 07:00
        var endDate = DateTime.Today.AddHours(14);  // 14:00
        var query = new GetAllMealsQuery(null, null, startDate, endDate)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2)); // Should return breakfast and lunch
        Assert.That(result.Any(m => m.Title == "Breakfast"), Is.True);
        Assert.That(result.Any(m => m.Title == "Lunch"), Is.True);
    }

    [Test]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var query = new GetAllMealsQuery(null, null, null, null)
        {
            PageNumber = 1,
            PageSize = 2
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.MetaData.TotalCount, Is.EqualTo(3));
        Assert.That(result.MetaData.CurrentPage, Is.EqualTo(1));
        Assert.That(result.MetaData.PageSize, Is.EqualTo(2));
        Assert.That(result.MetaData.TotalPage, Is.EqualTo(2));
    }

    [Test]
    public async Task Handle_WithInvalidDietPlanId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidDietPlanId = Guid.NewGuid();
        _mockDietPlanRepository
            .Setup(r => r.GetSingleAsync(It.IsAny<Expression<Func<Domain.Entities.DietPlan, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Domain.Entities.DietPlan, object>>[]>()))
            .ReturnsAsync((Domain.Entities.DietPlan)null);

        var query = new GetAllMealsQuery(null, invalidDietPlanId, null, null)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(async () => 
            await _handler.Handle(query, CancellationToken.None));
        
        Assert.That(ex.Message, Is.EqualTo("Diet plan not found"));
    }
}

// Yardımcı extension metot
public static class MockDbSetExtensions
{
    public static Mock<DbSet<T>> ToMockDbSet<T>(this IQueryable<T> source) where T : class
    {
        var mockDbSet = new Mock<DbSet<T>>();

        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(source.Provider));
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(source.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(source.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(source.GetEnumerator());
        mockDbSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<T>(source.GetEnumerator()));

        return mockDbSet;
    }
}

// Test için asenkron destek
internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression, _inner);
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression, _inner);
    public object Execute(Expression expression) => _inner.Execute(expression);
    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);
    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Task.FromResult(_inner.Execute<TResult>(expression)).GetAwaiter().GetResult();
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    private readonly IQueryProvider _provider;

    public TestAsyncEnumerable(Expression expression, IQueryProvider provider) : base(expression)
    {
        _provider = provider;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => _provider;
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
    public ValueTask DisposeAsync() => default;
} 