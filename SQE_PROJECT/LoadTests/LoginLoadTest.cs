using System;
using System.Net.Http;
using System.Text;
using NBomber.CSharp;
using SQE_PROJECT.Helpers;

namespace SQE_PROJECT.LoadTests
{
    public class LoginLoadTest
    {
        public void Run()
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("login_load", async ctx =>
            {
                var request = new HttpRequestMessage(HttpMethod.Post, Config.ApiUrl + "/login")
                {
                    Content = new StringContent("{\"Username\":\"testuser\",\"Password\":\"test123\"}", Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
            })
            .WithLoadSimulations(
                Simulation.Inject(rate: 100,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run(); // ❌ Don't await this
        }
    }
}
