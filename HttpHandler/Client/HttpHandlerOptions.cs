using System.Text.Json;

namespace Simons.Http
{
    public class HttpHandlerOptions
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; set; } = null;
        public bool ThrowOnStatusCodeUnsuccessful { get; set; } = true;
    }
}
