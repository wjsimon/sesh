using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSHC
{
    public interface IHttpHandler
    {
        Task<TValue?> GetAsync<TValue>(string? requestUri);
        Task<TResult?> PostAsync<TResult>(string? requestUri);
        Task<TResult?> PostAsync<TValue, TResult>(string? requestUri, TValue? payload);
    }
}
