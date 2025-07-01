## About

SesH, Simons extra simple HttpClient, is a simplified wrapper for Microsofts HttpClient, allowing fast & easy access to some specifications of Get & Post methods, as well as a simple code generator making use of this wrapper to automatically create functioning client classes for your server-side controllers in a project/directory of your choosing.

Meant for reducing the need to manually write boiler-plate code when setting up larger, or many, server-side controllers.

**These libraries are intended for prototyping use only. Not safe for production, as many safety features are not supported.**
**As of writing of this note (09.03.2025), only json-based APIs are supported by the wrapper. Should this ever change, this note will be updated**

.NET 10.0+ support only. This is a preview still. If there's anything missing which you'd like to see, feel free to open an issue or send me a message.

The current master is available as pre-built DLLs here: [build.zip](https://github.com/user-attachments/files/19150109/build.zip).
If you wish to only use the wrapper, you do not need to reference the included generator .dll.

## How to use?

### 1. Using the Wrapper

1. Add the "Sesh.Clients.Http.dll" to your project's dependecies
2. Create a new instance of "Sesh" using the constructor that best fits your project structure.
3. Call GetAsync<T>() or one of the PostAsync() methods on the Sesh instance.

Here is an example creating a simple usable instance of Sesh within the all-time-favourite Visual Studio "WeatherForecast" Web API project template.

![image](https://github.com/user-attachments/assets/55fd672d-7a8c-48a2-a768-5c14970f815a)

### 2. Using the client generator

1. Add the "Sesh.Client.Http.dll" and the "Sesh.Generators.HttpClient.dll" to your projects dependecies
2. Annotate the controllers you want to generate a client class for with the AutoGenerateSeshClientAttribute
3. Annotate the controllers method with the ReturnsAttribute, passing the type that is returned inside the ObjectResult. If nothing is returned, you may annotate with typeof(void). If nothing is annotated, "object" will be used in the client class method signatures instead.
4. Run the generator. You can do this either via an extra project, e.g. a console project, or use an extra build configuration and add the call to the generator to your main entry point's Main() method.

   Note: Starting from the .NET 10 Preview, 'dotnet run app.cs' is available. You can therefore also use the included 'sesh-generate.cs' instead of using an extra project or the build config switch. Adjust file to your project configuration before use.

6. Create an instance of the client where needed. You can now call your backend controllers methods through the client by invoking the client class' methods of the same name. E.g. if your backend controllers has a "GetMotorcycles(string manufacturer)" method, so will your client. Name and signature match exactly if your attribute-based annotations are accurate.

Here is an example of the annotated WeatherForecastController from the same template project, the generator call, and the output class.
#### Annotated controller class
![image](https://github.com/user-attachments/assets/301193ef-68d0-493c-b237-2f0d2759c3df)

#### Generator call
![image](https://github.com/user-attachments/assets/cf3c66ba-6225-42ac-9b48-2d3a47fd4078)

#### Output class
![image](https://github.com/user-attachments/assets/abdf7464-75cc-43ee-98c5-f490effb3a6c)

**Note how I used an 'IEnumerable<int>' instead of 'int[]' in the "Returns" annotation. The generator does not check the controller methods actual return type when generation the client method even if there is one.**

## Configurations

### 1. Custom Error Handler

You can adjust the wrappers error handling for all GetAsync() and PostAsync() calls by supplying your own handler. By default, errors are simply logged and then null is returned.

**This is a prototyping library I created for personal use. It strongly correlates with how I code, and therefore, many features of ASP.NET Controllers are not yet supported. Feel free to add to the project by opening and issue and a corresponding pull request.**
**For now, there's no further documentation. To get more information on how certain features work, you'll have to dig through the source. You can also shoot me a message here and I'll do my best to help out.** 
