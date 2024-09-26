using System.Text.Json;

namespace SSHC.Client
{
    public class HttpHandlerOptions
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; set; } = null;
    }
}
