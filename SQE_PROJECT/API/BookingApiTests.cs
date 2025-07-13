using System;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using SQE_PROJECT.API.Contracts;
using SQE_PROJECT.Helpers;

namespace SQE_PROJECT.API.Tests
{
    [TestFixture]
    [Category("API")]
    public class BookingApiTests : IDisposable
    {
        private readonly ApiClient _client;

        // ✅ Constructor initializes the readonly field
        public BookingApiTests()
        {
            _client = new ApiClient();
        }

        // No need for Setup method now
        //[OneTimeSetUp]
        //public void Setup() => _client = new ApiClient();

        [Test]
        public async Task CreateBooking_ValidRequest_Returns201Created()
        {
            var request = new BookingRequest
            {
                HotelId = 1,
                CheckInDate = DateTime.UtcNow.AddDays(1),
                CheckOutDate = DateTime.UtcNow.AddDays(3),
                GuestCount = 2
            };

            using var response = await _client.PostAsync("/bookings", request);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers.Location, Is.Not.Null);
            });
        }

        public void Dispose() => _client?.Dispose();
    }
}
