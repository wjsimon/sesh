using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.CompilerServices;

namespace SSHC.Client
{
    public abstract class ApiClient
    {
        public ApiClient(HttpClient httpClient)
        {
            _httpHandler = new HttpHandler(httpClient);
        }

        public ApiClient(IHttpHandler httpHandler)
        {
            this._httpHandler = httpHandler;
        }

        protected readonly IHttpHandler _httpHandler;
        protected abstract string ApiControllerName { get; init; }
        protected virtual Task<TValue?> GetAsync<TValue>(string? requestUri)
            => _httpHandler.GetAsync<TValue>(requestUri);
        protected virtual Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload)
            => _httpHandler.PostAsync<TValue, TResult>(requestUri, payload);
        protected virtual Task<TResult?> PostAsync<TResult>(string? requestUri)
            => _httpHandler.PostAsync<TResult>(requestUri);

        protected virtual string Uri([CallerMemberName] string? caller = null)
        {
            if (caller is null) { throw new ArgumentException($"{GetType().Name}.{nameof(Uri)} requires non-null caller via CallerMemberName Attribute"); }
            return $"{ApiControllerName}/{AdjustCallerMemberName(caller)}";
        }

        protected virtual string Uri(Dictionary<string, string?> param, [CallerMemberName] string? caller = null)
            => QueryHelpers.AddQueryString(Uri(caller), param);

        protected string Uri(string paramName, string? paramValue, [CallerMemberName] string? caller = null)
        {
            Dictionary<string, string?> param = new()
            {
                { paramName, paramValue }
            };

            return Uri(param, caller);
        }

        protected string Uri((string, string) names, (string?, string?) values, [CallerMemberName] string? caller = null)
        {
            Dictionary<string, string?> param = new()
            {
                { names.Item1, values.Item1 },
                { names.Item2, values.Item2 }
            };

            return Uri(param, caller);
        }

        protected virtual string AdjustCallerMemberName(string caller) //check overrides if you wan't to understand what this does
            => caller;
    }
}
