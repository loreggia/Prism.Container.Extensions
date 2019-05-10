# Prism.Container.Extensions

The Prism Container Extensions provide various additional extensions making the Prism Container easier to use with Splat, IServiceCollection/IServiceProvider and in scenarios where you may require a Singleton container that may need to be initialized from Platform specific code prior to PrismApplication being created.

[![Build Status](https://dev.azure.com/dansiegel/Prism.Plugins/_apis/build/status/dansiegel.Prism.Container.Extensions?branchName=master)](https://dev.azure.com/dansiegel/Prism.Plugins/_build/latest?definitionId=41?branchName=master)

## NuGet

You can add the MyGet CI feed to nuget by adding it as a source in Visual Studio:

`https://www.myget.org/F/prism-plugins/api/v3/index.json`

| Package | NuGet | MyGet |
|-------|:-----:|:------:|
| Prism.Container.Extensions | [![ContainerExtensionsShield]][ContainerExtensionsNuGet] | [![ContainerExtensionsMyGetShield]][ContainerExtensionsMyGet] |
| Prism.DryIoc.Extensions | [![DryIocExtensionsShield]][DryIocExtensionsNuGet] | [![DryIocExtensionsMyGetShield]][DryIocExtensionsMyGet] |
| Shiny.Prism.DryIoc | [![ShinyPrismDryIocShield]][ShinyPrismDryIocNuGet] | [![ShinyPrismDryIocMyGetShield]][ShinyPrismDryIocMyGet] |

## Initialization

The PrismContainerExtension can be initialized automatically and accessed by simply calling `PrismContainerExtension.Current`. You can also create a new container with any of the following methods:

```cs
PrismContainerExtension.Create(new Container());

// OR

new PrismContainerExtension(new Container());
```

**NOTE** That by default the container extension will ensure that the underlying container is properly configured to work with Prism Applications.

## Modifying PrismApplication

When using the extended container extension you simply need to add the following to your PrismApplication to ensure that it uses the same instance that may have been created prior to the initialization of PrismApplication.

```cs
protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;
```

## About the Extensions

The Extensions provide support for integration with Splat which

## Working With Shiny

Shiny uses the Microsoft.Extensions.DependencyInjection pattern found in ASP.NET Core applications with a Startup class. This in particular is a use case in which you will need to initialize a container prior to Forms.Init being called on the native platform. To work with Shiny you simply need to do something like the following:

```cs
public class PrismStartup : Startup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Register services with Shiny like: 
        services.UseGpsBackground<MyDelegate>();
    }

    public override IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        return PrismContainerExtension.Current.CreateServiceProvider(services);
    }
}
```

### Using the Shiny Prism Extensions

The Shiny.Prism.DryIoc package is designed to make it even easier for you to integrate these fantastic packages. As you'll see using the `ShinyPrismApplication` is identical in every way to creating a typical Prism Application. The only difference is that you are installing the Shiny.Prism.DryIoc package instead of Prism.DryIoc.Forms.

```xml
<prism:PrismApplication xmlns="http://xamarin.com/schemas/2014/forms"
                        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                        xmlns:prism="http://prismlibrary.com"
                        x:Class="Contoso.Awesome.App">
```

```cs
public partial class App
{
    protected override void OnInitialized()
    {
        InitializeComponent();
        NavigationService.NavigateAsync("MainPage");
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Register your services like normal
        containerRegistry.RegisterForNavigation<MainPage>();
    }
}
```

With your App using the PrismApplication from Shiny.Prism.DryIoc you now only need to reference the PrismStartup as the base class for your Startup class like:

```cs
public class MyStartup : PrismStartup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Register services with Shiny like: 
        services.UseGpsBackground<MyDelegate>();
    }
}
```

You can now pass your startup to the ShinyHost at your application's startup and use full Dependency Injection of Shiny's services in your app, full DI of services from Prism's container within services that are resolved by Shiny. **NOTE** that Shiny uses `IServiceProvider` which does not support the use of named services.

[ContainerExtensionsNuGet]: https://www.nuget.org/packages/Prism.Container.Extensions
[ContainerExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.Container.Extensions.svg
[ContainerExtensionsMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.Container.Extensions
[ContainerExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.Container.Extensions.svg

[DryIocExtensionsNuGet]: https://www.nuget.org/packages/Prism.DryIoc.Extensions
[DryIocExtensionsShield]: https://img.shields.io/nuget/vpre/Prism.DryIoc.Extensions.svg
[DryIocExtensionsMyGet]: https://www.myget.org/feed/prism/package/nuget/Prism.DryIoc.Extensions
[DryIocExtensionsMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Prism.DryIoc.Extensions.svg

[ShinyPrismDryIocNuGet]: https://www.nuget.org/packages/Shiny.Prism.DryIoc
[ShinyPrismDryIocShield]: https://img.shields.io/nuget/vpre/Shiny.Prism.DryIoc.svg
[ShinyPrismDryIocMyGet]: https://www.myget.org/feed/prism/package/nuget/Shiny.Prism.DryIoc
[ShinyPrismDryIocMyGetShield]: https://img.shields.io/myget/prism-plugins/vpre/Shiny.Prism.DryIoc.svg
