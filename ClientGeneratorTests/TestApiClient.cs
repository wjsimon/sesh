
namespace TestSpace
{
	public class TestApiClient : ApiClient
	{
		public string ApiControllerName => "TestController";
		
		public Task<string> Get() 
		{
		}
		
		public Task<int> RenamedGet() 
		{
		}
		
		public Task<bool> ParameterizedGet(int index, string name, object value) 
		{
		}
		
		public Task Post() 
		{
		}
	}
}
