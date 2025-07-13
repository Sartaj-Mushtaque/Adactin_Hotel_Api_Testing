using NUnit.Framework;
using SQE_PROJECT.Pages;
using TechTalk.SpecFlow;
using System.Threading.Tasks;

namespace SQE_PROJECT.StepDefinitions
{
    [Binding]
    public class SearchSteps
    {
        private readonly SearchPage _searchPage;

        public SearchSteps(SearchPage searchPage)
        {
            _searchPage = searchPage;
        }

        [Given(@"I am on the search page")]
        public Task GivenIAmOnTheSearchPage()
        {
            // Assuming navigation is handled prior (e.g., in login steps or hooks)
            return Task.CompletedTask;
        }

        [When(@"I search for hotels in ""(.*)""")]
        public async Task WhenISearchForHotelsIn(string location)
        {
            await _searchPage.SearchHotels(location);
        }

        [Then(@"I should see available hotels")]
        public async Task ThenIShouldSeeAvailableHotels()
        {
            int count = await _searchPage.GetAvailableHotelsCount();
            Assert.Greater(count, 0, "No hotels available in the search results.");
        }
    }
}
