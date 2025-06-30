
namespace Sesh.Clients.Http
{
	public class TestSeshClient : SeshBase
	{
		public TestSeshClient(IHttpWrapper httpWrapper) : base(httpWrapper) { }
		public TestSeshClient(HttpClient httpClient) : base(httpClient) { }
		public TestSeshClient(HttpClientHandler httpClientHandler) : base(httpClientHandler) { }
		
		private string _apiControllerRoute = "TestController";
		
		public override string ApiControllerRoute { get => _apiControllerRoute; init => _apiControllerRoute = value; }
		
		public Task<Dictionary<string, string>?> TwoParametersPost(int index, string name, object payload) 
			=> PostAsync<object, Dictionary<string, string>>(Uri([("index", index.ToString()), ("name", name.ToString())]),payload);
	}
}
