using System;
using System.Net.Http;
using NBomber.CSharp;
using SQE_PROJECT.Helpers;

namespace SQE_PROJECT.LoadTests
{
    public class BookingLoadTest
    {
        public void Run()
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("booking_load", async ctx =>
            {
                var request = new HttpRequestMessage(HttpMethod.Post, Config.ApiUrl + "/bookings")
                {
                    Content = new StringContent("{\"HotelId\":1,\"CheckInDate\":\"2023-12-01\",\"CheckOutDate\":\"2023-12-05\",\"GuestCount\":2}")
                };

                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
            })
            .WithLoadSimulations(
                Simulation.Inject(rate: 50,
                               interval: TimeSpan.FromSeconds(1),
                               during: TimeSpan.FromSeconds(30))
            );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}