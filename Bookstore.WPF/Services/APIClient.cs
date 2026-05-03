using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookstore.WPF.Services
{
    public static class ApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        static ApiClient()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7001/");
        }

        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>();
            return default;
        }

        public static async Task<bool> PostAndCheckSuccessAsync<TRequest>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> PostNoBodyAsync(string endpoint)
        {
            var response = await _httpClient.PostAsync(endpoint, null);
            return response.IsSuccessStatusCode;
        }
    }
}