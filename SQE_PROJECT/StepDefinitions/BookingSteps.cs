using NUnit.Framework;
using SQE_PROJECT.Pages;
using TechTalk.SpecFlow;
using System.Threading.Tasks;

namespace SQE_PROJECT.StepDefinitions
{
    [Binding]
    public class BookingSteps
    {
        private readonly BookingPage _bookingPage;
        private readonly ScenarioContext _scenarioContext;

        public BookingSteps(BookingPage bookingPage, ScenarioContext scenarioContext)
        {
            _bookingPage = bookingPage;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I am logged into the hotel booking app")]
        public async Task GivenIAmLoggedIntoTheHotelBookingApp()
        {
            await _bookingPage.Login();
        }

        [When(@"I search for a hotel and select one")]
        public async Task WhenISearchForAHotelAndSelectOne()
        {
            await _bookingPage.SearchHotel();
            await _bookingPage.SelectHotel();
        }

        [When(@"I enter booking details")]
        public async Task WhenIEnterBookingDetails()
        {
            await _bookingPage.EnterBookingDetails();
        }

        [Then(@"the booking should be successful")]
        public async Task ThenTheBookingShouldBeSuccessful()
        {
            var result = await _bookingPage.IsBookingSuccessful();
            Assert.IsTrue(result, "Booking failed");
        }
    }
}
