﻿using System.Runtime.CompilerServices;

namespace SeshLib.Clients.Http
{
    public class Sesh : SeshBase
    {
        public Sesh(IHttpWrapper httpHandler) : base(httpHandler) { }
        public Sesh(HttpClient httpClient) : base(httpClient) { }
        public Sesh(HttpClientHandler httpClientHandler) : base(httpClientHandler) { }

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