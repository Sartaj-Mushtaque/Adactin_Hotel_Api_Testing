using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Threading.Tasks;

namespace SQE_PROJECT.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        [Given(@"I am on the login page")]
        public Task GivenIAmOnTheLoginPage()
        {
            // Your navigation logic here
            return Task.CompletedTask;
        }

        [When(@"I login with username ""(.*)"" and password ""(.*)""")]
        public Task WhenILoginWithUsernameAndPassword(string username, string password)
        {
            // Your login logic here
            return Task.CompletedTask;
        }

        [Then(@"I should see the dashboard")]
        public Task ThenIShouldSeeTheDashboard()
        {
            // Your assertion logic here
            return Task.CompletedTask;
        }
    }
}
