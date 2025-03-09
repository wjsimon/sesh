
using System.Runtime.CompilerServices;

namespace Simons.Clients.Http
{
    //stub class to provide all the wrapper functionality without the need to create your own instance, while also protecting against misuse.
    public class FastApiClient : FastApiClientBase
    {
        public FastApiClient(IHttpWrapper httpHandler) : base(httpHandler) { }
        public FastApiClient(HttpClient httpClient) : base(httpClient) { }
        public FastApiClient(HttpClientHandler httpClientHandler) : base(httpClientHandler) { }

        private readonly string _apiControllerName = string.Empty;

        protected override string ApiControllerRoute { get => _apiControllerName; init => _apiControllerName = string.Empty; }

        protected override string Uri(Dictionary<string, string?> dict, [CallerMemberName] string? caller = null)
            => throw new NotSupportedException();
        
        protected override string Uri([CallerMemberName] string? caller = null, params (string, string?)[]? parameters)
            => throw new NotSupportedException();

        public new Task<TValue?> GetAsync<TValue>(string? requestUri)
            => _httpWrapper.GetAsync<TValue>(requestUri);
        public new Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload)
            => _httpWrapper.PostAsync<TValue, TResult>(requestUri, payload);
        public new Task<TResult?> PostAsync<TResult>(string? requestUri)
            => _httpWrapper.PostAsync<TResult>(requestUri);
        public new Task PostAsync<TValue>(string? requestUri, TValue? payload)
            => _httpWrapper.PostAsync(requestUri, payload);
    }
}
