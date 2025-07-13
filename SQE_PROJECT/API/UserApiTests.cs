using System;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using SQE_PROJECT.API.Contracts;
using SQE_PROJECT.Helpers;

namespace SQE_PROJECT.API.Tests
{
    [TestFixture]
    public class UserApiTests : IDisposable
    {
        private ApiClient _client;

        [OneTimeSetUp]
        public void Setup() => _client = new ApiClient();

        [Test]
        public async Task Login_ValidUser_ReturnsToken()
        {
            var user = TestDataLoader.Load<UserRequest>("users.json");
            var response = await _client.PostAsync("/login", user);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        public void Dispose() => _client?.Dispose();
    }
}