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

            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
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
        /// PUT API (Sửa dữ liệu, Trả về Object)
        /// </summary>
        public static async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>();

            // Đọc lỗi từ API
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        /// <summary>
        /// DELETE API (Xóa dữ liệu)
        /// </summary>
        public static async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                
                if (response.IsSuccessStatusCode)
                    return true;

                // Nếu Backend chặn không cho xóa (vd: Đã có hóa đơn, Còn nợ...)
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(errorContent);
            }
            catch (HttpRequestException)
            {
                throw new Exception("Không thể kết nối đến máy chủ.");
            }
        }

    }
}