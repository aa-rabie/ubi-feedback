using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Api.ModelFactory;
using UbiClub.Feedback.Tests.Fakes;

namespace UbiClub.Feedback.Tests.ModelFactories
{
    [TestFixture]
    internal class FeedbackGetModelFactoryTests : BaseTest
    {

        [Test]
        public void Create_ValidInput_ModelCreatedWithRating()
        {
            //Act
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var factory = scope.ServiceProvider.GetRequiredService<IFeedbackGetModelFactory>();
                var queryCollection = new FakeQueryCollection {{FeedbackGetModelFactory.RatingQueryParamName, "3"}};

                //Act
                var model = factory.Create(queryCollection);

                //Assert
                Assert.True(model.Rating.HasValue && model.Rating.Value == 3);
            }
        }

        [Test]
        public void Create_InvalidInput_ModelCreatedWithoutRating()
        {
            //Act
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var factory = scope.ServiceProvider.GetRequiredService<IFeedbackGetModelFactory>();
                var queryCollection = new FakeQueryCollection();
                queryCollection.Add(FeedbackGetModelFactory.RatingQueryParamName, "text");
                //Act
                var model = factory.Create(queryCollection);

                //Assert
                Assert.False(model.Rating.HasValue);
            }
        }

        [Test]
        public void Create_EmptyQueryCollection_ModelCreatedWithoutRating()
        {
            //Act
            using (var scope = _container.CreateScope())
            {
                //Arrange
                var factory = scope.ServiceProvider.GetRequiredService<IFeedbackGetModelFactory>();
                var queryCollection = new FakeQueryCollection();
                //Act
                var model = factory.Create(queryCollection);

                //Assert
                Assert.False(model.Rating.HasValue);
            }
        }
    }
}