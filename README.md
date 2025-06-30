## About

SesH, Simons extra simple HttpClient, is a stripped-down, modern C# wrapper for Microsofts HttpClient, allowing fast & easy access to some specifications of Get & Post methods, as well as a simple code generator making use of this wrapper to automatically create functioning client classes for your server-side controllers in a project/directory of your choosing.

Meant for people who often set up larger server-side controllers, or many of them, and want to reduce manually writing boiler-plate while testing. As a bonus, it keeps the code somewhat organized out-of-the-box. 

**These libraries are intended for prototyping use only. Not safe for production, as many safety features are not supported.**
**As of writing of this note (09.03.2025), only json-based APIs are supported by the wrapper. Should this ever change, this note will be updated**

.NET 9.0+ support only. This is a preview still. If there's anything missing which you'd like to see, feel free to open an issue or send me a message.

The current master is available as pre-built DLLs here: [build.zip](https://github.com/user-attachments/files/19150109/build.zip).
If you wish to only use the wrapper, you do not need to reference the included generator .dll.

## How to use?

### 1. Using the Wrapper

1. Add the "Sesh.Clients.Http.dll" to your project's dependecies
2. Create a new instance of "Sesh" using the constructor that best fits your project structure. If you intend to use multiple instances for multiple controllers in a 1-to-1 fashion, make sure to pass a value for the "route" parameter, so that you can use Sesh's "Uri()" method.
3. Call Get(Uri("action")) or Post((Uri("action")) on the Sesh instance. If "route" wasn't passed in the constructor, e.g. when you use a single instance for multiple controllers, you cannot use Uri() have to call the Get() and Post() passing the full api action route.

### 2. Using the client generator

1. Add the "Sesh.Client.Http.dll" and the "Sesh.Generators.HttpClient.dll" to your projects dependecies
2. Annotate the controllers you want to generate a client class for with the AutoGenerateSeshClientAttribute
3. Annotate the controllers method with the ReturnsAttribute, passing the type that is returned inside the ObjectResult. If nothing is returned, you may annotate with typeof(void). If nothing is annotated, "object" will be used in the client class method signatures instead.
4. Run the generator. You can do this either via an extra project, most conveniently a console project, or use an extra build configuration and add the call to the generator to your main entry point's Main() method.
5. Create an instance of the client where needed. You can now call your backend controllers methods through the client by invoking the client class' methods of the same name. E.g. if your backend controllers has a "GetMotorcycles(string manufacturer)" method, so will your client. Name and signature match exactly if your attribute-based annotations are accurate.

**This is a prototyping library I created for personal use. It strongly correlates with how I code, and therefore, many features of ASP.NET Controllers are not yet supported. Feel free to add to the project by opening and issue and a corresponding pull request.**
**For now, there's no further documentation. To get more information on how certain features work, you'll have to dig through the source. You can also shoot me a message here and I'll do my best to help out.** 
