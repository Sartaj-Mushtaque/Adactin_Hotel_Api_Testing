using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SQE_PROJECT.Pages
{
    public class SearchPage
    {
        private readonly IPage _page;

        public SearchPage(IPage page) => _page = page;

        public async Task SearchHotels(string location)
        {
            await _page.SelectOptionAsync("#location", location);
            await _page.ClickAsync("#Submit");
        }

        public async Task<int> GetAvailableHotelsCount()
        {
            var hotels = await _page.QuerySelectorAllAsync(".hotel_name");
            return hotels.Count;
        }
    }
}