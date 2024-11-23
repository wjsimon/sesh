using System.Net.Http.Json;
using System.Text.Json;

namespace Simons.Clients.Http
{
    internal sealed class SimonsHttpWrapper(HttpClient http, HttpWrapperOptions? options = null) : IHttpWrapper
    {
        private readonly HttpClient _httpClient = http;
        private readonly HttpWrapperOptions? _options = options;

        public JsonSerializerOptions? JsonSerializerOptions => _options?.JsonSerializerOptions;

        public Task<TResult?> GetAsync<TResult>(string? requestUri)
            => ParseResponse<TResult>(_httpClient.GetAsync(requestUri));

        public Task PostAsync<TValue>(string? requestUri, TValue? payload)
            => PostAsyncInternal<TValue, object>(requestUri, payload);

        public Task<TResult?> PostAsync<TResult>(string? requestUri)
            => PostAsyncInternal<object, TResult>(requestUri, null);

        public Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload)
            => PostAsyncInternal<TValue, TResult>(requestUri, payload);

        private async Task<TResult?> PostAsyncInternal<TValue, TResult>(string? requestUri, TValue? payload)
            => await ParseResponse<TResult>(_httpClient.PostAsJsonAsync(requestUri, payload, JsonSerializerOptions));

        private async Task<TResult?> ParseResponse<TResult>(Task<HttpResponseMessage> responseTask)
            => await ParseResponse<TResult>(await responseTask);

        private async Task<TResult?> ParseResponse<TResult>(HttpResponseMessage response)
        {
            if (CheckResponseSuccessAndThrowIfNeeded(response)) { return default; }

            var content = await response.Content.ReadAsStringAsync();
            if (typeof(TResult) == typeof(string)) { return (TResult)(object)content; } //strings are handled differently by HttpResponseMessage

            var value = JsonSerializer.Deserialize<TResult>(content, JsonSerializerOptions);
            return value;
        }

        private bool CheckResponseSuccessAndThrowIfNeeded(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (_options is not null && _options.ThrowOnStatusCodeUnsuccessful)
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }

                return false;
            }

            return true;
        }
    }
}
