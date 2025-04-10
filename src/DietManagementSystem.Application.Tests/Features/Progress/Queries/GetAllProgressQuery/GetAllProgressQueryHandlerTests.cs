using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Progress.Queries.GetAllProgressQuery;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;
using DietManagementSystem.Application.Tests.Features.DietPlan.Queries.GetAllDietPlansQuery;

namespace DietManagementSystem.Application.Tests.Features.Progress.Queries.GetAllProgressQuery;

[TestFixture]
public class GetAllProgressQueryHandlerTests
{
    private Mock<IProgressRepository> _mockProgressRepository;
    private Mock<IDietPlanRepository> _mockDietPlanRepository;
    private GetAllProgressQueryHandler _handler;
    private List<Domain.Entities.Progress> _progressList;
    private Domain.Entities.DietPlan _dietPlan;
    private Guid _userId;

    [SetUp]
    public void Setup()
    {
        _mockProgressRepository = new Mock<IProgressRepository>();
        _mockDietPlanRepository = new Mock<IDietPlanRepository>();
        _handler = new GetAllProgressQueryHandler(_mockProgressRepository.Object, _mockDietPlanRepository.Object);

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

        var baseDate = DateTime.Today;
        _progressList = new List<Domain.Entities.Progress>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Date = baseDate.AddDays(-2),
                Weight = 82.5m,
                Notes = "Initial weight",
                ClientId = _userId,
                DietPlanId = _dietPlan.Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Date = baseDate.AddDays(-1),
                Weight = 82.0m,
                Notes = "Good progress",
                ClientId = _userId,
                DietPlanId = _dietPlan.Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Date = baseDate,
                Weight = 81.5m,
                Notes = "Continuing well",
                ClientId = _userId,
                DietPlanId = _dietPlan.Id
            }
        };

        // Mock repository setup
        var mockDbSet = _progressList.AsQueryable().ToMockDbSet();
        _mockProgressRepository.Setup(r => r.AsQueryable()).Returns(mockDbSet.Object);

        _mockDietPlanRepository
            .Setup(r => r.GetSingleAsync(It.IsAny<Expression<Func<Domain.Entities.DietPlan, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Domain.Entities.DietPlan, object>>[]>()))
            .ReturnsAsync(_dietPlan);
    }

    [Test]
    public async Task Handle_WithNoFilters_ShouldReturnAllProgress()
    {
        // Arrange
        var query = new Application.Features.Progress.Queries.GetAllProgressQuery.GetAllProgressQuery(null, null)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.First().Weight, Is.EqualTo(81.5m)); // En son kayıt (OrderByDescending)
    }

    [Test]
    public async Task Handle_WithUserIdFilter_ShouldReturnUserProgress()
    {
        // Arrange
        var query = new Application.Features.Progress.Queries.GetAllProgressQuery.GetAllProgressQuery(null, _userId)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task Handle_WithDietPlanIdFilter_ShouldReturnDietPlanProgress()
    {
        // Arrange
        var query = new Application.Features.Progress.Queries.GetAllProgressQuery.GetAllProgressQuery(_dietPlan.Id, null)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task Handle_WithDateRangeFilter_ShouldReturnProgressInRange()
    {
        // Arrange
        var baseDate = DateTime.Today;
        var query = new Application.Features.Progress.Queries.GetAllProgressQuery.GetAllProgressQuery(null, null, baseDate.AddDays(-1), baseDate)
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2)); // Son 2 kayıt
        Assert.That(result.All(p => p.Date >= baseDate.AddDays(-1) && p.Date <= baseDate), Is.True);
    }

    [Test]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var query = new Application.Features.Progress.Queries.GetAllProgressQuery.GetAllProgressQuery(null, null)
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

        var query = new Application.Features.Progress.Queries.GetAllProgressQuery.GetAllProgressQuery(invalidDietPlanId, null)
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

