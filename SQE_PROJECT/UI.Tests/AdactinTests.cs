using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace SQE_PROJECT.UI.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class AdactinTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;

        private const string BaseUrl = "https://adactinhotelapp.com/";
        private const string Username = "SartajMushtaque";
        private const string Password = "SartajMushtaque";

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 200 });
            Console.WriteLine("Browser launched");
        }

        [SetUp]
        public async Task Setup()
        {
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            await _page.GotoAsync(BaseUrl);
        }

        [TearDown]
        public async Task Teardown()
        {
            await _context.CloseAsync();
        }

        [OneTimeTearDown]
        public async Task GlobalTeardown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        // Helper Methods
        private async Task LoginAsync()
        {
            await _page.FillAsync("#username", Username);
            await _page.FillAsync("#password", Password);
            await _page.ClickAsync("#login");
            await _page.WaitForSelectorAsync("text=Search Hotel");
        }

        private async Task SearchHotelAsync(string location = "Sydney", string hotel = "Hotel Creek")
        {
            await _page.SelectOptionAsync("#location", location);
            await _page.SelectOptionAsync("#hotels", hotel);
            await _page.ClickAsync("#Submit");
        }

        private async Task FillBookingFormAsync()
        {
            await _page.FillAsync("#first_name", "John");
            await _page.FillAsync("#last_name", "Doe");
            await _page.FillAsync("#address", "123 Test St");
            await _page.FillAsync("#cc_num", "4111111111111111");
            await _page.SelectOptionAsync("#cc_type", "VISA");
            await _page.SelectOptionAsync("#cc_exp_month", "December");
            await _page.SelectOptionAsync("#cc_exp_year", "2025");
            await _page.FillAsync("#cc_cvv", "123");
        }

        private async Task RunTest(Func<Task> testLogic, string testName)
        {
            Console.WriteLine($"Running: {testName}");
            try
            {
                await testLogic();
            }
            catch (Exception ex)
            {
                Assert.Fail($"{testName} failed: {ex.Message}");
            }
        }

        #region Test Cases

        [Test, Order(1)]
        public async Task TC01_LoginValidUser() =>
            await RunTest(async () => { await LoginAsync(); }, nameof(TC01_LoginValidUser));

        [Test, Order(2)]
        public async Task TC02_VerifyLocationDropdown() =>
            await RunTest(async () => {
                await LoginAsync();
                Assert.IsTrue(await _page.IsVisibleAsync("#location"));
            }, nameof(TC02_VerifyLocationDropdown));

        [Test, Order(3)]
        public async Task TC03_SearchHotel() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
            }, nameof(TC03_SearchHotel));

        [Test, Order(4)]
        public async Task TC04_SelectHotelAndContinue() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("#radiobutton_0");
                await _page.ClickAsync("#continue");
            }, nameof(TC04_SelectHotelAndContinue));

        [Test, Order(5)]
        public async Task TC05_FillBookingDetails() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("#radiobutton_0");
                await _page.ClickAsync("#continue");
                await FillBookingFormAsync();
            }, nameof(TC05_FillBookingDetails));

        [Test, Order(6)]
        public async Task TC06_SubmitBooking() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("#radiobutton_0");
                await _page.ClickAsync("#continue");
                await FillBookingFormAsync();
                await _page.ClickAsync("#book_now");
            }, nameof(TC06_SubmitBooking));

        [Test, Order(7)]
        public async Task TC07_CheckOrderNumber() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("#radiobutton_0");
                await _page.ClickAsync("#continue");
                await FillBookingFormAsync();
                await _page.ClickAsync("#book_now");
                await _page.WaitForSelectorAsync("#order_no");
                var orderNo = await _page.InputValueAsync("#order_no");
                Assert.IsNotEmpty(orderNo);
            }, nameof(TC07_CheckOrderNumber));

        [Test, Order(8)]
        public async Task TC08_LogoutUser() =>
            await RunTest(async () => {
                await LoginAsync();
                await _page.ClickAsync("text=Logout");
                Assert.IsTrue(await _page.IsVisibleAsync("#username"));
            }, nameof(TC08_LogoutUser));

        [Test, Order(9)]
        public async Task TC09_VerifyRoomDropdown() =>
            await RunTest(async () => {
                await LoginAsync();
                Assert.IsTrue(await _page.IsVisibleAsync("#room_type"));
            }, nameof(TC09_VerifyRoomDropdown));

        [Test, Order(10)]
        public async Task TC10_VerifyCreditCardField() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("#radiobutton_0");
                await _page.ClickAsync("#continue");
                Assert.IsTrue(await _page.IsVisibleAsync("#cc_num"));
            }, nameof(TC10_VerifyCreditCardField));

        [Test, Order(11)]
        public async Task TC11_CheckItinerary() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("#radiobutton_0");
                await _page.ClickAsync("#continue");
                await FillBookingFormAsync();
                await _page.ClickAsync("#book_now");
                await _page.ClickAsync("text=My Itinerary");
                Assert.IsTrue((await _page.InnerTextAsync("body")).Contains("Hotel"));
            }, nameof(TC11_CheckItinerary));

        [Test, Order(12)]
        public async Task TC12_VerifyInvalidLogin() =>
            await RunTest(async () => {
                await _page.FillAsync("#username", "wrong");
                await _page.FillAsync("#password", "wrong");
                await _page.ClickAsync("#login");
                Assert.IsTrue((await _page.InnerTextAsync("body")).Contains("Invalid"));
            }, nameof(TC12_VerifyInvalidLogin));

        [Test, Order(13)]
        public async Task TC13_VerifyHotelDropdown() =>
            await RunTest(async () => {
                await LoginAsync();
                Assert.IsTrue(await _page.IsVisibleAsync("#hotels"));
            }, nameof(TC13_VerifyHotelDropdown));

        [Test, Order(14)]
        public async Task TC14_SearchDifferentLocation() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync("Melbourne", "Hotel Sunshine");
            }, nameof(TC14_SearchDifferentLocation));

        [Test, Order(15)]
        public async Task TC15_VerifyBookingPage() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("#radiobutton_0");
                await _page.ClickAsync("#continue");
                Assert.IsTrue(await _page.IsVisibleAsync("text=Book A Hotel"));
            }, nameof(TC15_VerifyBookingPage));

        [Test, Order(16)]
        public async Task TC16_SubmitEmptyForm() =>
            await RunTest(async () => {
                await LoginAsync();
                await _page.ClickAsync("#Submit");
                Assert.IsTrue((await _page.InnerTextAsync("body")).Contains("Please"));
            }, nameof(TC16_SubmitEmptyForm));

        [Test, Order(17)]
        public async Task TC17_BackToSearchHotel() =>
            await RunTest(async () => {
                await LoginAsync();
                await _page.ClickAsync("text=Booked Itinerary");
                await _page.ClickAsync("text=Search Hotel");
                Assert.IsTrue(await _page.IsVisibleAsync("#location"));
            }, nameof(TC17_BackToSearchHotel));

        [Test, Order(18)]
        public async Task TC18_VerifyCancelBookingOption() =>
            await RunTest(async () => {
                await LoginAsync();
                await _page.ClickAsync("text=Booked Itinerary");
                Assert.IsTrue(await _page.IsVisibleAsync("input[value='Cancel All']"));
            }, nameof(TC18_VerifyCancelBookingOption));

        [Test, Order(19)]
        public async Task TC19_VerifyCVVFieldPresence() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("#radiobutton_0");
                await _page.ClickAsync("#continue");
                Assert.IsTrue(await _page.IsVisibleAsync("#cc_cvv"));
            }, nameof(TC19_VerifyCVVFieldPresence));

        [Test, Order(20)]
        public async Task TC20_VerifyResetButton() =>
            await RunTest(async () => {
                await LoginAsync();
                await SearchHotelAsync();
                await _page.ClickAsync("input[type='reset']");
                // No assertion as reset is visual – consider checking form is cleared
            }, nameof(TC20_VerifyResetButton));

        #endregion
    }
}
