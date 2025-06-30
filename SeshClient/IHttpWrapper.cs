namespace SeshLib.Clients.Http
{
    public interface IHttpWrapper
    {
        Task<TValue?> GetAsync<TValue>(string? requestUri);
        Task PostAsync<TValue>(string? requestUri, TValue? payload);
        Task<TResult?> PostAsync<TResult>(string? requestUri);
        Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload);
    }
}
