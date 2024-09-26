using System.Net.Http.Json;
using System.Text.Json;

namespace SSHC
{
    internal sealed class HttpHandler(HttpClient http, HttpHandlerOptions options = null) : IHttpHandler
    {
        private readonly HttpClient _httpClient = http;
        private readonly HttpHandlerOptions? _options = options;

        public JsonSerializerOptions? JsonSerializerOptions => _options?.JsonSerializerOptions;

        public async Task<TResult?> GetAsync<TResult>(string? requestUri)
        {
            HttpResponseMessage response = await this._httpClient.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode) { return default; } //throw instead?

            return await ParseResponse<TResult>(response);
        }

        public Task<TResult?> PostAsync<TResult>(string? requestUri)
            => this.PostAsyncInternal<object, TResult>(requestUri, null);

        public Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload)
            => this.PostAsyncInternal<TValue, TResult>(requestUri, payload);

        private async Task<TResult?> PostAsyncInternal<TValue, TResult>(string? requestUri, TValue? payload)
        {
            var response = await this._httpClient.PostAsJsonAsync(requestUri, payload, JsonSerializerOptions);
            if (!response.IsSuccessStatusCode) { return default; } //throw instead?
            return await ParseResponse<TResult>(response);
        }

        private async Task<TResult?> ParseResponse<TResult>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (typeof(TResult) == typeof(string)) { return (TResult)(object)content; } //strings are handled differently by HttpResponseMessage

            var value = JsonSerializer.Deserialize<TResult>(content, JsonSerializerOptions);
            return value;
        }
    }
}
