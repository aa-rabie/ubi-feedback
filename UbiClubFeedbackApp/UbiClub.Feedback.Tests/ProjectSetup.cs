using NUnit.Framework;
using UbiClub.Feedback.Tests.Infrastructure;

namespace UbiClub.Feedback.Tests
{
    [SetUpFixture]
    public class ProjectSetup
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            new TestDbInitializer().Init();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            // ...
        }
    }
}