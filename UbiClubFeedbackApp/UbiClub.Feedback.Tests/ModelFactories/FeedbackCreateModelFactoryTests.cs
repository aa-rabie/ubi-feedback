using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Api.ModelFactory;
using UbiClub.Feedback.Tests.Helpers;

namespace UbiClub.Feedback.Tests.ModelFactories
{
    [TestFixture]
    internal class FeedbackCreateModelFactoryTests : BaseTest
    {

        [Test]
        public void Create_ValidInput_ModelCreatedSuccessfully()
        {
            //Act
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var factory = scope.ServiceProvider.GetRequiredService<IFeedbackCreateModelFactory>();
                var userId = Guid.NewGuid();
                var headers = new HeaderDictionary();
                headers.Append(FeedbackCreateModelFactory.UserIdHeaderName, userId.ToString());
                var sessionId = ExpectedTestData.GameSessionIds[0];
                var requestBody = @"{ ""rating"": ""4""}";

                //Act
                var model = factory.Create(requestBody, headers, sessionId);

                //Assert
                Assert.True(model.Rating.HasValue && model.Rating.Value == 4);
                Assert.True(model.UserId.HasValue && model.UserId.Value == userId);
                Assert.True(model.SessionId.HasValue && model.SessionId.Value == sessionId);
            }
        }

        [Test]
        public void Create_MissingHeader_ModelCreatedWithoutUserId()
        {
            //Act
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var factory = scope.ServiceProvider.GetRequiredService<IFeedbackCreateModelFactory>();
                var headers = new HeaderDictionary();
                var sessionId = ExpectedTestData.GameSessionIds[0];
                var requestBody = @"{ ""rating"": ""4""}";

                //Act
                var model = factory.Create(requestBody, headers, sessionId);

                //Assert
                Assert.True(model.Rating.HasValue && model.Rating.Value == 4);
                Assert.False(model.UserId.HasValue);
                Assert.True(model.SessionId.HasValue && model.SessionId.Value == sessionId);
            }
        }

        [Test]
        public void Create_EmptyRequestBody_ModelCreatedWithoutRating()
        {
            //Act
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var factory = scope.ServiceProvider.GetRequiredService<IFeedbackCreateModelFactory>();
                var userId = Guid.NewGuid();
                var headers = new HeaderDictionary();
                headers.Append(FeedbackCreateModelFactory.UserIdHeaderName, userId.ToString());
                var sessionId = ExpectedTestData.GameSessionIds[0];
                
                //Act
                var model = factory.Create(string.Empty, headers, sessionId);

                //Assert
                Assert.False(model.Rating.HasValue);
                Assert.True(model.UserId.HasValue && model.UserId.Value == userId);
                Assert.True(model.SessionId.HasValue && model.SessionId.Value == sessionId);
            }
        }
    }
}