
namespace Simons.Clients.Http
{
	public class TestApiClient : FastApiClientBase
	{
		public TestApiClient(IHttpWrapper httpWrapper) : base(httpWrapper) { }
		public TestApiClient(HttpClient httpClient) : base(httpClient) { }
		public TestApiClient(HttpClientHandler httpClientHandler) : base(httpClientHandler) { }
		
		private string _apiControllerRoute = "TestController";
		
		public override string ApiControllerRoute { get => _apiControllerRoute; init => _apiControllerRoute = value; }
		
		public Task<Dictionary<string, string>?> TwoParametersPost(int index, string name, object payload) 
			=> PostAsync<object, Dictionary<string, string>>(Uri([("index", index.ToString()), ("name", name.ToString())]),payload);
	}
}
