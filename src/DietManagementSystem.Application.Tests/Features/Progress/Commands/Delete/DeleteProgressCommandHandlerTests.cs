using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Progress.Commands.Delete;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.Progress.Commands.Delete;

[TestFixture]
public class DeleteProgressCommandHandlerTests
{
    private Mock<IProgressRepository> _progressRepositoryMock;
    private DeleteProgressCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _progressRepositoryMock = new Mock<IProgressRepository>();
        _handler = new DeleteProgressCommandHandler(_progressRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenProgressExists_ShouldSoftDeleteProgress()
    {
        // Arrange
        var progressId = Guid.NewGuid();
        var command = new DeleteProgressCommand(progressId);

        var existingProgress = new Domain.Entities.Progress
        {
            Id = progressId,
            IsDeleted = false,
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _progressRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Progress, bool>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync(existingProgress);

        _progressRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(existingProgress.IsDeleted, Is.True);
            Assert.That(existingProgress.UpdatedAt, Is.GreaterThan(DateTime.UtcNow.AddSeconds(-5)));
        });

        _progressRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenProgressDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var progressId = Guid.NewGuid();
        var command = new DeleteProgressCommand(progressId);

        _progressRepositoryMock.Setup(x => x.GetSingleAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Progress, bool>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync((Domain.Entities.Progress?)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Progress not found"));
        _progressRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
} 