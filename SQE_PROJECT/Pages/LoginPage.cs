using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SQE_PROJECT.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        private readonly string _usernameField = "#username";
        private readonly string _passwordField = "#password";
        private readonly string _loginButton = "#login";

        public LoginPage(IPage page) => _page = page;

        public async Task Login(string username, string password)
        {
            await _page.FillAsync(_usernameField, username);
            await _page.FillAsync(_passwordField, password);
            await _page.ClickAsync(_loginButton);
        }
    }
}