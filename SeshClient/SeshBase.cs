﻿using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.CompilerServices;

namespace SeshLib.Clients.Http
{
    public abstract class SeshBase
    {
        public SeshBase(IHttpWrapper httpHandler)
        {
            _httpWrapper = httpHandler;
        }

        public SeshBase(HttpClient httpClient) : this(new SeshWrapper(httpClient)) { }
        public SeshBase(HttpClientHandler httpClientHandler) : this(new HttpClient(httpClientHandler)) { }

        protected readonly IHttpWrapper _httpWrapper;
        public abstract string Route { get; init; }

        protected virtual Task<TValue?> GetAsync<TValue>(string? requestUri)
            => _httpWrapper.GetAsync<TValue>(requestUri);
        protected virtual Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload)
            => _httpWrapper.PostAsync<TValue, TResult>(requestUri, payload);
        protected virtual Task<TResult?> PostAsync<TResult>(string? requestUri)
            => _httpWrapper.PostAsync<TResult>(requestUri);
        protected virtual Task PostAsync<TValue>(string? requestUri, TValue? payload)
            => _httpWrapper.PostAsync(requestUri, payload);

        protected virtual string Uri((string, string?)[]? parameters = null, [CallerMemberName] string? caller = null)
        {
            EnsureValidCaller(caller);

            if (parameters is null)
            {
                return $"{Route}/{AdjustCallerMemberName(caller)}";
            }

            Dictionary<string, string?> param = new([
                ..parameters.Select(p => new KeyValuePair<string, string?>(p.Item1, p.Item2))
            ]);

            return Uri(param, caller);
        }

        protected virtual string Uri(Dictionary<string, string?> dict, [CallerMemberName] string? caller = null)
        {
            EnsureValidCaller(caller);
            return QueryHelpers.AddQueryString(Uri(caller: caller), dict);
        }

        protected virtual string AdjustCallerMemberName(string? caller)
        {
            EnsureValidCaller(caller);
            return caller!;
        }

        private void EnsureValidCaller(string? caller, [CallerMemberName] string? consumer = null)
        {
            if (caller is null) { throw new ArgumentException($"{GetType().Name}.{consumer} requires non-null caller Attribute"); }
        }
    }
}
