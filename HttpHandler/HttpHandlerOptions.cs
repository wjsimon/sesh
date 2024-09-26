using System.Text.Json;

namespace SSHC
{
    public class HttpHandlerOptions
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; set; } = null;
    }
}
