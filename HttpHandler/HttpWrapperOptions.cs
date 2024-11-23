using System.Text.Json;

namespace Simons.Clients.Http
{
    public class HttpWrapperOptions
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; set; } = null;
        public bool ThrowOnStatusCodeUnsuccessful { get; set; } = true;
    }
}
