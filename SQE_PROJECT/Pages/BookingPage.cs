using System.Threading.Tasks;
using Microsoft.Playwright;
using SQE_PROJECT.Helpers;

namespace SQE_PROJECT.Pages
{
    public class BookingPage
    {
        private readonly IPage _page;
        private readonly IBrowser _browser;

        public BookingPage(IPage page, IBrowser browser)
        {
            _page = page;
            _browser = browser;
        }

        public async Task Login()
        {
            await _page.GotoAsync(Config.BaseUrl);
            await _page.FillAsync("#username", "yourUsername");
            await _page.FillAsync("#password", "yourPassword");
            await _page.ClickAsync("#login");
        }

        public async Task SearchHotel()
        {
            await _page.SelectOptionAsync("#location", "Sydney");
            await _page.SelectOptionAsync("#hotels", "Hotel Creek");
            await _page.SelectOptionAsync("#room_type", "Standard");
            await _page.ClickAsync("#Submit");
        }

        public async Task SelectHotel()
        {
            await _page.ClickAsync("#radiobutton_0");
            await _page.ClickAsync("#continue");
        }

        public async Task EnterBookingDetails()
        {
            await _page.FillAsync("#first_name", "John");
            await _page.FillAsync("#last_name", "Doe");
            await _page.FillAsync("#address", "123 Street");
            await _page.FillAsync("#cc_num", "4111111111111111");
            await _page.SelectOptionAsync("#cc_type", "VISA");
            await _page.SelectOptionAsync("#cc_exp_month", "12");
            await _page.SelectOptionAsync("#cc_exp_year", "2026");
            await _page.FillAsync("#cc_cvv", "123");
            await _page.ClickAsync("#book_now");
        }

        public async Task<bool> IsBookingSuccessful()
        {
            await _page.WaitForSelectorAsync("#order_no");
            var orderNo = await _page.GetAttributeAsync("#order_no", "value");
            return !string.IsNullOrEmpty(orderNo);
        }
    }
}