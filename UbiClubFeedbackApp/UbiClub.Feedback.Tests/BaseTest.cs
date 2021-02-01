using NUnit.Framework;
using UbiClub.Feedback.Tests.Infrastructure;

namespace UbiClub.Feedback.Tests
{
    internal abstract class BaseTest
    {
        protected DiContainer _container;
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
    }
}