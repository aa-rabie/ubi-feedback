using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UbiClub.Feedback.Api.Exceptions;
using UbiClub.Feedback.Api.RequestHandlers;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Core.Models;
using UbiClub.Feedback.Data.Interfaces;
using UbiClub.Feedback.Tests.Helpers;

namespace UbiClub.Feedback.Tests.RequestHandlers
{
    [TestFixture]
    internal class FeedbackCreateRequestHandlerTests : BaseTest
    {
        [Test]
        public async Task HandleAsync_InvalidSessionId_ExceptionThrown()
        {
            //Arrange
            var gameSessionServiceMock = new Mock<IGameSessionService>();
            GameSessionDto nullDto = null;
            gameSessionServiceMock.Setup(m => m.GetAsync(It.IsAny<Guid>())).ReturnsAsync<IGameSessionService,GameSessionDto>(nullDto);

            var feedbackServiceMock = new Mock<IFeedbackService>();

            var handler = new FeedbackCreateRequestHandler(gameSessionServiceMock.Object
                , feedbackServiceMock.Object);
            var model = new FeedbackCreateModel
            {
                SessionId = ExpectedTestData.GameSessionIds[0],
                UserId = Guid.NewGuid(),
                Rating = 4
            };

            // Act
            AsyncTestDelegate actDelegate = async () => await handler.HandleAsync(model);

            //Assert
            Assert.ThrowsAsync<SessionNotFoundException>(actDelegate);
            gameSessionServiceMock.Verify(m => m.GetAsync(It.IsAny<Guid>())
            , Times.Once());
        }

        [Test]
        public async Task HandleAsync_UserSubmitMoreThanOnceInSession_ExceptionThrown()
        {
            //Arrange
            var gameSessionServiceMock = new Mock<IGameSessionService>();
            GameSessionDto gameDto = new GameSessionDto()
            {
                Id = ExpectedTestData.GameSessionIds[0],

            };
            gameSessionServiceMock.Setup(m => m.GetAsync(It.IsAny<Guid>())).ReturnsAsync<IGameSessionService, GameSessionDto>(gameDto);

            var feedbackServiceMock = new Mock<IFeedbackService>();
            feedbackServiceMock.Setup(m => m.GetFeedbackCountPerUserSessionAsync
                (It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(1);

            var handler = new FeedbackCreateRequestHandler(gameSessionServiceMock.Object
                , feedbackServiceMock.Object);
            var model = new FeedbackCreateModel
            {
                SessionId = ExpectedTestData.GameSessionIds[0],
                UserId = Guid.NewGuid(),
                Rating = 4
            };

            // Act
            AsyncTestDelegate actDelegate = async () => await handler.HandleAsync(model);

            //Assert
            Assert.ThrowsAsync<FeedbackCreateRequestNotAllowedException>(actDelegate);
            gameSessionServiceMock.Verify(m => m.GetAsync(It.IsAny<Guid>())
                , Times.Once());
            feedbackServiceMock.Verify(m => m.GetFeedbackCountPerUserSessionAsync
                    (It.IsAny<Guid>(), It.IsAny<Guid>())
                , Times.Once());
        }

        [Test]
        public async Task HandleAsync_ValidData_Pass()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var sessionId = ExpectedTestData.GameSessionIds[0];
            byte rating = 4;
            var gameSessionServiceMock = new Mock<IGameSessionService>();
            GameSessionDto gameDto = new GameSessionDto()
            {
                Id = ExpectedTestData.GameSessionIds[0],

            };
            var feedbackDto = new SessionFeedbackDto()
            {
                UserId = userId,
                SessionId = sessionId,
                Rating = rating
            };

            gameSessionServiceMock.Setup(m => m.GetAsync(sessionId)).ReturnsAsync<IGameSessionService, GameSessionDto>(gameDto);

            var feedbackServiceMock = new Mock<IFeedbackService>();
            feedbackServiceMock.Setup(m => m.GetFeedbackCountPerUserSessionAsync
                (sessionId, userId)).ReturnsAsync(0);

            feedbackServiceMock.Setup(f => f.AddFeedbackAsync(sessionId, userId, rating))
                .ReturnsAsync(feedbackDto);

            var handler = new FeedbackCreateRequestHandler(gameSessionServiceMock.Object
                , feedbackServiceMock.Object);
            var createModel = new FeedbackCreateModel
            {
                SessionId = sessionId,
                UserId = userId,
                Rating = rating
            };

            // Act
            var actualFeedbackDto = await handler.HandleAsync(createModel);

            //Assert
           Assert.NotNull(actualFeedbackDto);
           Assert.That(actualFeedbackDto.UserId, Is.EqualTo(feedbackDto.UserId));
           Assert.That(actualFeedbackDto.SessionId, Is.EqualTo(feedbackDto.SessionId));
           Assert.That(actualFeedbackDto.Rating, Is.EqualTo(feedbackDto.Rating));
            gameSessionServiceMock.Verify(m => m.GetAsync(sessionId)
                , Times.Once());
            feedbackServiceMock.Verify(m => m.GetFeedbackCountPerUserSessionAsync
                    (sessionId, userId)
                , Times.Once());
            feedbackServiceMock.Verify(m => m.AddFeedbackAsync
                    (sessionId, userId, rating)
                , Times.Once());
        }
    }
}