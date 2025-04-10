using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Progress.Queries.GetProgressQuery;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;
using DietManagementSystem.Application.Tests.Features.DietPlan.Queries.GetAllDietPlansQuery;

namespace DietManagementSystem.Application.Tests.Features.Progress.Queries.GetProgressQuery;

[TestFixture]
public class GetProgressQueryHandlerTests
{
    private Mock<IProgressRepository> _mockProgressRepository;
    private GetProgressQueryHandler _handler;
    private Domain.Entities.Progress _progress;

    [SetUp]
    public void Setup()
    {
        _mockProgressRepository = new Mock<IProgressRepository>();
        _handler = new GetProgressQueryHandler(_mockProgressRepository.Object);

        // Test data
        _progress = new Domain.Entities.Progress
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Today,
            Weight = 82.5m,
            Notes = "Test progress note"
        };

        var progressList = new List<Domain.Entities.Progress> { _progress };
        var mockDbSet = progressList.AsQueryable().ToMockDbSet();
        _mockProgressRepository.Setup(r => r.AsQueryable()).Returns(mockDbSet.Object);
    }

    [Test]
    public async Task Handle_WithValidId_ShouldReturnProgress()
    {
        // Arrange
        var query = new Application.Features.Progress.Queries.GetProgressQuery.GetProgressQuery(_progress.Id);
        _mockProgressRepository.Setup(r => r.GetSingleAsync(
            It.Is<Expression<Func<Domain.Entities.Progress, bool>>>(expr => 
                expr.Compile().Invoke(_progress)),
            true))
            .ReturnsAsync(_progress);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(_progress.Id));
            Assert.That(result.Date, Is.EqualTo(_progress.Date));
            Assert.That(result.Weight, Is.EqualTo(_progress.Weight));
            Assert.That(result.Notes, Is.EqualTo(_progress.Notes));
        });
    }

    [Test]
    public async Task Handle_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var query = new Application.Features.Progress.Queries.GetProgressQuery.GetProgressQuery(invalidId);
        _mockProgressRepository.Setup(r => r.GetSingleAsync(
            It.IsAny<Expression<Func<Domain.Entities.Progress, bool>>>(),
            true))
            .ReturnsAsync((Domain.Entities.Progress)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(query, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo("Progress not found."));
    }
}