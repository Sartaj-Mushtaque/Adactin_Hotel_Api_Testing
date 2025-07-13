using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SQE_PROJECT.Helpers
{
    public sealed class ApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private bool _disposed;

        public ApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Config.ApiUrl),
                Timeout = TimeSpan.FromSeconds(Config.TimeoutSeconds)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<HttpResponseMessage> PostAsync<TRequest>(
            string endpoint,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(
                endpoint.TrimStart('/'),
                content,
                cancellationToken);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _httpClient?.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}