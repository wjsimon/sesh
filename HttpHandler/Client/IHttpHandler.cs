namespace SSHC.Client
{
    public interface IHttpHandler
    {
        Task<TValue?> GetAsync<TValue>(string? requestUri);
        Task<TResult?> PostAsync<TResult>(string? requestUri);
        Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload);
    }
}
