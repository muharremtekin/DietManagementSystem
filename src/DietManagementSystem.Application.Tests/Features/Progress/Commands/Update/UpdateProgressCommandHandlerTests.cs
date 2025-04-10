using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Features.Progress.Commands.Update;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Moq;

namespace DietManagementSystem.Application.Tests.Features.Progress.Commands.Update;

[TestFixture]
public class UpdateProgressCommandHandlerTests
{
    private Mock<IProgressRepository> _progressRepositoryMock;
    private UpdateProgressCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _progressRepositoryMock = new Mock<IProgressRepository>();
        _handler = new UpdateProgressCommandHandler(_progressRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenProgressExists_ShouldUpdateProgress()
    {
        // Arrange
        var progressId = Guid.NewGuid();
        var date = DateTime.Now;

        var command = new UpdateProgressCommand(date, 76.5m, "Updated progress notes")
        {
            Id = progressId
        };

        var existingProgress = new Domain.Entities.Progress
        {
            Id = progressId,
            Date = DateTime.Now.AddDays(-1),
            Weight = 77.0m,
            Notes = "Original notes"
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
            Assert.That(existingProgress.Date, Is.EqualTo(command.Date));
            Assert.That(existingProgress.Weight, Is.EqualTo(command.Weight));
            Assert.That(existingProgress.Notes, Is.EqualTo(command.Notes));
        });

        _progressRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_WhenProgressDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var progressId = Guid.NewGuid();
        var date = DateTime.Now;

        var command = new UpdateProgressCommand(date, 76.5m, "Updated progress notes")
        {
            Id = progressId
        };

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