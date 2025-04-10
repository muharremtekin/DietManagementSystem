using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.DietPlan.Queries.GetDietPlanQuery;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;

namespace DietManagementSystem.Application.Tests.Features.DietPlan.Queries.GetDietPlanQuery;

[TestFixture]
public class GetDietPlanQueryHandlerTests
{
    private Mock<IDietPlanRepository> _mockDietPlanRepository;
    private GetDietPlanQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockDietPlanRepository = new Mock<IDietPlanRepository>();
        _handler = new GetDietPlanQueryHandler(_mockDietPlanRepository.Object);
    }

    [Test]
    public async Task Handle_WhenDietPlanExists_ShouldReturnDietPlanViewModel()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var client = new Domain.Entities.ApplicationUser { Id = Guid.NewGuid(), FullName = "John Doe" };
        
        var dietPlan = new Domain.Entities.DietPlan
        {
            Id = dietPlanId,
            Title = "Test Diet Plan",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(30),
            InitialWeight = 80.5m,
            Client = client,
            Meals = new List<Domain.Entities.Meal>
            {
                new() { Id = Guid.NewGuid(), Title = "Breakfast", StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(9), Content = "Oatmeal", IsDeleted = false },
                new() { Id = Guid.NewGuid(), Title = "Lunch", StartTime = TimeSpan.FromHours(13), EndTime = TimeSpan.FromHours(14), Content = "Salad", IsDeleted = true }
            },
            ProgressEntries = new List<Domain.Entities.Progress>
            {
                new() { Id = Guid.NewGuid(), Date = DateTime.Today, Weight = 79.5m, Notes = "Good progress", IsDeleted = false },
                new() { Id = Guid.NewGuid(), Date = DateTime.Today.AddDays(-1), Weight = 80.0m, Notes = "Started", IsDeleted = true }
            }
        };

        _mockDietPlanRepository.Setup(r => r.GetSingleAsync(
            It.Is<Expression<Func<Domain.Entities.DietPlan, bool>>>(expr => true),
            It.IsAny<bool>(),
            It.IsAny<Expression<Func<Domain.Entities.DietPlan, object>>[]>()))
            .ReturnsAsync(() => {
                var filteredMeals = dietPlan.Meals.Where(m => !m.IsDeleted).ToList();
                var filteredProgress = dietPlan.ProgressEntries.Where(p => !p.IsDeleted).ToList();
                
                return new Domain.Entities.DietPlan
                {
                    Id = dietPlan.Id,
                    Title = dietPlan.Title,
                    StartDate = dietPlan.StartDate,
                    EndDate = dietPlan.EndDate,
                    InitialWeight = dietPlan.InitialWeight,
                    Client = dietPlan.Client,
                    Meals = filteredMeals,
                    ProgressEntries = filteredProgress
                };
            });

        // Act
        var result = await _handler.Handle(new Application.Features.DietPlan.Queries.GetDietPlanQuery.GetDietPlanQuery(dietPlanId), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(dietPlanId));
        Assert.That(result.Title, Is.EqualTo("Test Diet Plan"));
        Assert.That(result.StartDate, Is.EqualTo(DateTime.Today));
        Assert.That(result.EndDate, Is.EqualTo(DateTime.Today.AddDays(30)));
        Assert.That(result.InitialWeight, Is.EqualTo(80.5m));
        Assert.That(result.ClientFullName, Is.EqualTo("John Doe"));
        
        // Should only include non-deleted meals
        Assert.That(result.Meals.Count(), Is.EqualTo(1));
        Assert.That(result.Meals.First().Title, Is.EqualTo("Breakfast"));
        
        // Should only include non-deleted progress entries
        Assert.That(result.Processes.Count(), Is.EqualTo(1));
        Assert.That(result.Processes.First().Weight, Is.EqualTo(79.5m));
    }

    [Test]
    public void Handle_WhenDietPlanDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        _mockDietPlanRepository.Setup(r => r.GetSingleAsync(
            It.IsAny<Expression<Func<Domain.Entities.DietPlan, bool>>>(),
            It.IsAny<bool>(),
            It.IsAny<Expression<Func<Domain.Entities.DietPlan, object>>[]>()))
            .ReturnsAsync((Domain.Entities.DietPlan)null);

        // Act & Assert
        Assert.ThrowsAsync<NotFoundException>(async () => 
            await _handler.Handle(new Application.Features.DietPlan.Queries.GetDietPlanQuery.GetDietPlanQuery(dietPlanId), CancellationToken.None));
    }

    [Test]
    public async Task Handle_WhenDietPlanHasNoMealsOrProgress_ShouldReturnEmptyCollections()
    {
        // Arrange
        var dietPlanId = Guid.NewGuid();
        var client = new Domain.Entities.ApplicationUser { Id = Guid.NewGuid(), FullName = "John Doe" };
        
        var dietPlan = new Domain.Entities.DietPlan
        {
            Id = dietPlanId,
            Title = "Test Diet Plan",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(30),
            InitialWeight = 80.5m,
            Client = client,
            Meals = new List<Domain.Entities.Meal>(),
            ProgressEntries = new List<Domain.Entities.Progress>()
        };

        _mockDietPlanRepository.Setup(r => r.GetSingleAsync(
            It.IsAny<Expression<Func<Domain.Entities.DietPlan, bool>>>(),
            It.IsAny<bool>(),
            It.IsAny<Expression<Func<Domain.Entities.DietPlan, object>>[]>()))
            .ReturnsAsync(dietPlan);

        // Act
        var result = await _handler.Handle(new Application.Features.DietPlan.Queries.GetDietPlanQuery.GetDietPlanQuery(dietPlanId), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Meals, Is.Empty);
        Assert.That(result.Processes, Is.Empty);
    }
} 