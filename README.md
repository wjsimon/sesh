## About

A stripped-down, modern C# wrapper for Microsofts HttpClient, allowing fast & easy access to some specifications of Get & Post methods, as well as a simple but effective code generator making use of this wrapper to automatically create functioning client classes for your server-side controllers in a project/directory of your choosing.

Meant for people who often set up larger server-side controllers, or many of them, and want to reduce manually writing boiler-plate while testing. As a bonus, it keeps the code somewhat organized out-of-the-box. 

**These libraries are intended for prototyping use only. Not safe for production, as many safety features are not supported.**

.NET 9.0+ support only. This is a preview still. If there's anything missing which you'd like to see, feel free to open an issue or send me a message.

The current master is available as pre-built DLLs here: [build.zip](https://github.com/user-attachments/files/19150109/build.zip).
If you wish to only use the wrapper, you do not need to reference the included generator .dll.

## How to use?

### 1. Creating your own implementation of the wrapper class

The absolute easiest way to get started is to simply use the included "FastApiClient", which is a stub class meant to provide access to the wrapper methods without any added conveniences.
        
        FastApiClient client = new(new HttpClient());

From here, you get immediate access to the Post & Get Methods.

        client.GetAsync<int>("My/Get");
        client.PostAsync<bool, int>("My/Post", true);
        
To get started for real however, you should instead opt to derive from the included "FastApiClientBase". As each class definition is conceptually scoped to a controller name, a strict 1-to-1 mapping is encouraged, but not enforced.
Here is an example of such a class definition and it's corresponding server-side controller.

    [Route("[controller]")]
    public class MyController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() { return Ok(true); }
    }
    
    public class MyApiClient : FastApiClientBase
    {
        public MyApiClient(IHttpWrapper httpHandler) : base(httpHandler) { }
        public MyApiClient(HttpClient httpClient) : base(httpClient) { }
        public MyApiClient(HttpClientHandler httpClientHandler) : base(httpClientHandler) { }
        
        public string ApiControllerRoute => "My"; //Asp.Net Core Route attribute is responsible for creating the "My/" route from "MyController"
    }

Please note how the controller uses many features of Microsoft's ASP.NET Core MVC library. This is not mandatory in order to use the client, but is when using the generator.

From here, all that is left is to provide public methods in your class that piece together the correct API call via the wrapper. For this example, I will provide the implementation of a method that would get give us access to the result of our "Get" method defined in "MyController" in the snippet above.

    public class MyApiClient : FastApiClientBase
    {
        public MyApiClient(IHttpWrapper httpHandler) : base(httpHandler) { }
        public MyApiClient(HttpClient httpClient) : base(httpClient) { }
        public MyApiClient(HttpClientHandler httpClientHandler) : base(httpClientHandler) { }

        public override string ApiControllerRoute => "My";

        public Task<bool> Get() 
        {
            return GetAsync<bool>(Uri());
        }
    }

For now, simply note that the methods name is "Get", which corresponds to the server-side controllers method accessed, which is also called "Get". This can be configured and controlled to a greater degree than shown here, but is explained farther down.
Once you have created your constructable class definition, all you need is something to call it with. As you can see, this is either an IHttpWrapper, HttpClient or HttpClientHandler. Which one you choose depends on your intended project structure and is beyond the scope of this example. Therefore, we will simply create a new HttpClient here. This is not recommended, and goes against Microsoft's Best Practices for using the HttpClient class. More construction scenarios are found in the "Tips" section.

    MyApiClient myClient = new MyApiClient(new HttpClient());
    bool result = await myClient.Get();

Now, how does this compare to simply using Microsofts HttpClient?

    HttpClient client2 = new HttpClient();
    bool result = await client.GetAsync<bool>("My/Get");

As you can see, it's not at all shorter. We also went through the trouble of creating an entire new class. So, why bother with this at all? The answer is mass. Some things are obfuscated here, the FastApiClientBase includes e.g. error handling, but more importantly it's how POST requests are shortened in comparison to HttpClient.PostAsync, while GET requests are clearly just as fast. Sounds great, right? Let's look at an example.

I've added a simple post method to the controller, as well as it's counterpart to our custom client class defintion.

    [Route("[controller]")]
    public class MyController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() { return Ok(true); }

        [HttpPost]
        public IActionResult Post([FromQuery] int index, [FromQuery] string name, [FromBody] object payload) { return Ok(); }
    }
    
    public class MyApiClient : FastApiClientBase
    {
        public MyApiClient(IHttpWrapper httpHandler) : base(httpHandler) { }
        public MyApiClient(HttpClient httpClient) : base(httpClient) { }
        public MyApiClient(HttpClientHandler httpClientHandler) : base(httpClientHandler) { }

        public override string ApiControllerRoute => "My";

        public Task<bool> Get() 
        {
            return GetAsync<bool>(Uri());
        }

        public Task<Dictionary<string, string>?> Post(int index, string name, object payload)
        {
                Dictionary<string, string> dict = new();
                dict.Add("index", index.ToString());
                dict.Add("name", name.ToString());

                return PostAsync<Dictionary<string, string>, object>(Uri(("index", index), ("name", name), payload));
        }
    }

If we now compare what the framework does, and what we'd usually have to do using Microsoft's HttpClient, the advantages should become apparent.
First, using the FastApiClientBase.

    MyApiClient myClient = new MyApiClient(new HttpClient());
    object payload = new object();
    bool result = await myClient.Post(10, "user", payload);

And now the same result, including error handling, using what Microsoft offers via the HttpClient.

    HttpClient client = new HttpClient();
    try
    {
        Dictionary<string, string?> dict = new();
        dict.Add("index", "10");
        dict.Add("name", "user");

        object payload = new object();

        var response = await client.PostAsJsonAsync(QueryHelpers.AddQueryString("My/Post", dict), payload);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<bool>(content);
        }
        else
        {
            return false;
        }
    }
    catch (Exception ex)
    {
        //handle error
    }

**Please note an important limitation: As of now (09.03.2025), only json-based APIs are supported by the wrapper. Should this ever change, this note will be updated**

Now imagine doing this everytime you want to make a complex POST request call. You'd go and wrap it in a method, wouldn't you? Using the FastApiClientBase, that's exactly what we do, with the added benefit of containing all the boiler-plate error handling and parsing for us, while also allowing explicitly typed parameters and dealing with default values.

We also are able to reap some advantages through IDE use, e.g. Visual Studio's IntelliSense, as the method definitions in our client class offers better IDE integration compared to raw strings passed into the HttpClient.GetAsync/PostAsync methods. 

However, we can go even faster, by getting rid of pretty much all of the remaining boiler-plate derivable from the controller definition.

### 2. Using the code generator

THIS DOCUMENT IS A WORK IN PROGRESS.
