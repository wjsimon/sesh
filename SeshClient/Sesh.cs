using System.Runtime.CompilerServices;

namespace SeshLib.Clients.Http
{
    //easy access stub implementation
    public class Sesh : SeshBase
    {
        public Sesh(IHttpWrapper httpHandler, string? route = null) : base(httpHandler) { if (route != null) { _route = route; }; }
        public Sesh(HttpClient httpClient, string? route = null) : base(httpClient) { if (route != null) { _route = route; }; }
        public Sesh(HttpClientHandler httpClientHandler, string? route = null) : base(httpClientHandler) { if (route != null) { _route = route; }; }

        private readonly string _route = string.Empty;

        public override string Route { get => _route; init => _route = string.Empty; }

        protected override string Uri(Dictionary<string, string?> dict, [CallerMemberName] string? caller = null)
            => throw new NotSupportedException();
        
        protected override string Uri((string, string?)[]? parameters = null, [CallerMemberName] string? caller = null)
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
