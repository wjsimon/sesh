using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.CompilerServices;

namespace Simons.Http
{
    public abstract class ApiClient
    {
        public ApiClient(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        public ApiClient(HttpClient httpClient) : this(new HttpHandler(httpClient)) { }

        protected readonly IHttpHandler _httpHandler;
        protected abstract string ApiControllerName { get; init; }
        
        protected virtual Task<TValue?> GetAsync<TValue>(string? requestUri)
            => _httpHandler.GetAsync<TValue>(requestUri);
        protected virtual Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload)
            => _httpHandler.PostAsync<TValue, TResult>(requestUri, payload);
        protected virtual Task<TResult?> PostAsync<TResult>(string? requestUri)
            => _httpHandler.PostAsync<TResult>(requestUri);
        protected virtual Task PostAsync<TValue>(string? requestUri, TValue? payload)
            => _httpHandler.PostAsync<TValue>(requestUri, payload);

        protected virtual string Uri([CallerMemberName] string? caller = null)
        {
            EnsureValidCaller(caller);
            return $"{ApiControllerName}/{AdjustCallerMemberName(caller)}";
        }

        protected string Uri([CallerMemberName] string? caller = null, params (string, string?)[] parameters)
        {
            EnsureValidCaller(caller);
            Dictionary<string, string?> param = new([
                ..parameters.Select(p => new KeyValuePair<string, string?>(p.Item1, p.Item2))
            ]);

            return Uri(param, caller);
        }

        protected virtual string Uri(Dictionary<string, string?> dict, [CallerMemberName] string? caller = null)
        {
            EnsureValidCaller(caller);
            return QueryHelpers.AddQueryString(Uri(caller), dict);
        }

        protected virtual string AdjustCallerMemberName(string? caller) //check overrides if you wan't to understand what this does
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
