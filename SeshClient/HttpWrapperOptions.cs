using System.Text.Json;

namespace SeshLib.Clients.Http
{
    public class HttpWrapperOptions
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; set; } = null;
        public bool ThrowOnStatusCodeUnsuccessful { get; set; } = true;
    }
}
