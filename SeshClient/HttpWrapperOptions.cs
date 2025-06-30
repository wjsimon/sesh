using System.Text.Json;

namespace Sesh.Clients.Http
{
    public class HttpWrapperOptions
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; set; } = null;
        public bool ThrowOnStatusCodeUnsuccessful { get; set; } = true;
    }
}
