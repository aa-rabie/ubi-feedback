using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using UbiClub.Feedback.Data.Interfaces;
using UbiClub.Feedback.Tests.Helpers;

namespace UbiClub.Feedback.Tests.Services

{
    [TestFixture]
    internal class GameSessionServiceTests : BaseTest
    {
        [Test]
        public async Task GetAsync_ValidInput_ReturnData()
        {
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var svc = scope.ServiceProvider.GetRequiredService<IGameSessionService>();
                //Act
                var dto = await svc.GetAsync(ExpectedTestData.GameSessionIds[0]);
                //Assert
                Assert.NotNull(dto);
                Assert.AreEqual(dto.Id, ExpectedTestData.GameSessionIds[0]);
            }
        }

        [Test]
        public async Task GetAsync_DataNotFound_ReturnsNull()
        {
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var svc = scope.ServiceProvider.GetRequiredService<IGameSessionService>();
                //Act
                var dto = await svc.GetAsync(Guid.NewGuid());
                //Assert
                Assert.Null(dto);
            }
        }
    }
}