using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using UbiClub.Feedback.Data.Interfaces;
using UbiClub.Feedback.Entities;
using UbiClub.Feedback.Tests.Helpers;
using UbiClub.Feedback.Tests.Infrastructure;

namespace UbiClub.Feedback.Tests.Services
{
    [TestFixture]
    public class FeedbackServiceTests
    {
        private DiContainer _container;
        [OneTimeSetUp]
        public void Setup()
        {
            _container = new DiContainer(TestDbInitializer.TestDbConnectionString);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _container?.Dispose();
        }

        [Test]
        public async Task AddFeedback_Pass()
        {
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var svc = scope.ServiceProvider.GetRequiredService<IFeedbackService>();
                var sessionId = ExpectedTestData.GameSessionIds[0];
                var userId = Guid.NewGuid();
                byte rating = 5;
                
                //Act
                var dto = await svc.AddFeedbackAsync(sessionId, userId, rating);
                var count = await svc.GetFeedbackCountPerUserSessionAsync(sessionId, userId);
                //Assert
                Assert.NotNull(dto);
                Assert.AreEqual(sessionId, dto.SessionId);
                Assert.AreEqual(userId , dto.UserId);
                Assert.AreEqual(rating, dto.Rating);
                Assert.AreEqual(1, count);
            }
        }

        [Test]
        public async Task GetFeedbackCountPerUserSessionAsync_NoData_ReturnZero()
        {
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var svc = scope.ServiceProvider.GetRequiredService<IFeedbackService>();
                var sessionId = ExpectedTestData.GameSessionIds[1];
                var userId = Guid.NewGuid();
                
                //Act
                var count = await svc.GetFeedbackCountPerUserSessionAsync(sessionId, userId);
                
                //Assert
                Assert.AreEqual(0, count);
            }
        }

        [Test]
        public async Task GetFeedbackListAsync_RatingDoesNotExist_ReturnNoData()
        {
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var svc = scope.ServiceProvider.GetRequiredService<IFeedbackService>();
                
                //Act
                var data = await svc.GetFeedbackListAsync(11, 0,15);

                //Assert
                Assert.AreEqual(0, data.Count);
            }
        }

        [Test]
        public async Task GetFeedbackListAsync_FilterByValidRating_ReturnData()
        {
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var svc = scope.ServiceProvider.GetRequiredService<IFeedbackService>();
                var sessionId = ExpectedTestData.GameSessionIds[1];
                byte ratingToSearchFor = 4;
                var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository>();
                var initialFeedbackCount = await repo.CountAsync<SessionFeedback>(f => f.Rating == ratingToSearchFor);

                for (byte rating = 3; rating <= 5; rating++)
                {
                    await svc.AddFeedbackAsync(sessionId, Guid.NewGuid(), rating);
                }

                //Act
                var filteredDataByRating = await svc.GetFeedbackListAsync(ratingToSearchFor, 0, 5);

                //Assert
                Assert.NotNull(filteredDataByRating);
                Assert.True(filteredDataByRating.All(f => f.Rating == ratingToSearchFor));
                Assert.AreEqual(initialFeedbackCount + 1, filteredDataByRating.Count);
            }
        }

        [Test]
        public async Task GetFeedbackListAsync_NoFilterByRating_ReturnData()
        {
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var svc = scope.ServiceProvider.GetRequiredService<IFeedbackService>();
                var sessionId = ExpectedTestData.GameSessionIds[1];
                var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository>();
                var initialFeedbackCount = await repo.CountAsync<SessionFeedback>();
                var dataSize = 5;
                Guid[] userIds = new Guid[dataSize];
                for (var index = 0; index < dataSize; index++)
                {
                    userIds[index] = Guid.NewGuid();
                }

                for (var index = 0; index < dataSize; index++)
                {
                    await svc.AddFeedbackAsync(sessionId, userIds[index] , 5);
                }

                //Act
                var data = await svc.GetFeedbackListAsync(null, 0, dataSize);

                //Assert
                Assert.NotNull(data);
                for (var index = 0; index < dataSize; index++)
                {
                    var dto = data[index];
                    Assert.True(dto.UserId == userIds[dataSize - index - 1]);
                    Assert.True(dto.SessionId == sessionId);
                    if (index > 0)
                    {
                        var previousDto = data[index - 1];
                        Assert.True(dto.CreatedDate <= previousDto.CreatedDate);
                    }
                }

                Assert.AreEqual(dataSize, data.Count);
            }
        }
    }
}