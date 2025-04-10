using DietManagementSystem.Application.Features.DietPlan.Queries.GetAllDietPlansQuery;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace DietManagementSystem.Application.Tests.Features.DietPlan.Queries.GetAllDietPlansQuery
{
    [TestFixture]
    public class GetAllDietPlansQueryHandlerTests
    {
        private Mock<IDietPlanRepository> _mockDietPlanRepository;
        private GetAllDietPlansQueryHandler _handler;
        private List<Domain.Entities.DietPlan> _dietPlans;

        [SetUp]
        public void Setup()
        {
            _mockDietPlanRepository = new Mock<IDietPlanRepository>();
            _handler = new GetAllDietPlansQueryHandler(_mockDietPlanRepository.Object);

            // Test verisi
            var client1 = new ApplicationUser { Id = Guid.NewGuid(), FullName = "John Doe" };
            var client2 = new ApplicationUser { Id = Guid.NewGuid(), FullName = "Jane Smith" };
            var dietitianId = Guid.NewGuid();

            _dietPlans = new List<Domain.Entities.DietPlan>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Diet Plan 1",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(30),
                    InitialWeight = 80.5m,
                    Client = client1,
                    ClientId = client1.Id,
                    DietitianId = dietitianId,
                    IsDeleted = false
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Diet Plan 2",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(60),
                    InitialWeight = 75.0m,
                    Client = client2,
                    ClientId = client2.Id,
                    DietitianId = dietitianId,
                    IsDeleted = false
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Deleted Diet Plan",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(30),
                    InitialWeight = 90.0m,
                    Client = client1,
                    ClientId = client1.Id,
                    DietitianId = dietitianId,
                    IsDeleted = true
                }
            };

            // DbSet mock’unu asenkron destekleyecek şekilde yapılandır
            var mockDbSet = _dietPlans.AsQueryable().ToMockDbSet();
            _mockDietPlanRepository.Setup(r => r.AsQueryable()).Returns(mockDbSet.Object);
        }

        [Test]
        public async Task Handle_WithNoFilters_ShouldReturnAllNonDeletedDietPlans()
        {
            // Arrange
            var query = new Application.Features.DietPlan.Queries.GetAllDietPlansQuery.GetAllDietPlansQuery(null, null)
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(dp => !dp.Title.Contains("Deleted")), Is.True);
        }

        [Test]
        public async Task Handle_WithClientIdFilter_ShouldReturnOnlyClientDietPlans()
        {
            // Arrange
            var clientId = _dietPlans.First().ClientId;
            var query = new Application.Features.DietPlan.Queries.GetAllDietPlansQuery.GetAllDietPlansQuery(null, clientId)
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.All(dp => dp.Title == "Diet Plan 1"), Is.True);
        }

        [Test]
        public async Task Handle_WithDietitianIdFilter_ShouldReturnOnlyDietitianDietPlans()
        {
            // Arrange
            var dietitianId = _dietPlans.First().DietitianId;
            var query = new Application.Features.DietPlan.Queries.GetAllDietPlansQuery.GetAllDietPlansQuery(dietitianId, null)
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Handle_WithPagination_ShouldReturnCorrectPage()
        {
            // Arrange
            var query = new Application.Features.DietPlan.Queries.GetAllDietPlansQuery.GetAllDietPlansQuery(null, null)
            {
                PageNumber = 1,
                PageSize = 1
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.MetaData.TotalCount, Is.EqualTo(2));
            Assert.That(result.MetaData.CurrentPage, Is.EqualTo(1));
            Assert.That(result.MetaData.PageSize, Is.EqualTo(1));
            Assert.That(result.MetaData.TotalPage, Is.EqualTo(2));
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
}