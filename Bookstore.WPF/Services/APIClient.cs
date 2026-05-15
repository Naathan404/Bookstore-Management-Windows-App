using Bookstore.Share.DTOs;
using Newtonsoft.Json;
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

        /// <summary>
        /// GET API
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<TResponse>();
                return default;
            }
            catch (Exception)
            {
                // Bắt lỗi kết nối
                return default;
            }
        }

        /// <summary>
        /// POST API
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>();
            return default;
        }

        /// <summary>
        /// POST and check RESULT
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<bool> PostAndCheckSuccessAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        /// <summary>
        /// POST no body
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static async Task<bool> PostNoBodyAsync(string endpoint)
        {
            var response = await _httpClient.PostAsync(endpoint, null);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// PUT API
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<bool> PutAndCheckSuccessAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(endpoint, data);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        /// <summary>
        /// DELETE API
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteAndCheckSuccessAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        /// <summary>
        /// GET DASHBOARD DATA
        /// </summary>
        /// <returns></returns>
        public static async Task<DashboardOverviewDto> GetDashboardOverviewAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/dashboard/overview");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DashboardOverviewDto>(json);
            }

            return null; 
        }
    }
}