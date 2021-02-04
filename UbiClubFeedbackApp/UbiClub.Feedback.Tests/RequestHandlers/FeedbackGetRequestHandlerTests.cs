using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UbiClub.Feedback.Api.RequestHandlers;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Core.Models;
using UbiClub.Feedback.Data.Interfaces;

namespace UbiClub.Feedback.Tests.RequestHandlers
{
    [TestFixture]
    internal class FeedbackGetRequestHandlerTests : BaseTest
    {
        [Test]
        public async Task HandleAsync_AssureRating_PassedCorrectly()
        {
            //Arrange
            var model = new FeedbackGetModel
            {
                Rating = 5
            };
            var fetchedData = new List<SessionFeedbackDto>();
            var offset = 0;
            var limit = 15;
            var feedbackServiceMock = new Mock<IFeedbackService>();
            feedbackServiceMock.Setup(m => m.GetFeedbackListAsync(model.Rating, offset, limit)).ReturnsAsync<IFeedbackService, List<SessionFeedbackDto>>(fetchedData);

            var handler = new FeedbackGetRequestHandler(feedbackServiceMock.Object);

            //Act
            var actualData = await handler.HandleAsync(model);

            //Assert
            Assert.NotNull(actualData);
            feedbackServiceMock.Verify(m => m.GetFeedbackListAsync(
                It.Is<byte>(val => val == model.Rating)
                , It.Is<int>(a => a == offset)
            , It.Is<int>(a => a == limit)), Times.Once);
        }
    }
}