# TinyIoC
TinyIoC is a tiny inversion-of-control container.
This fail-fast, to-the-point, no-fuss API keeps your DI consistent and predictable.
The most important types are `TinyContainer`, `ITinyRegistry` and `ITinyResolver`.
```c#
public interface ITinyRegistry
{
    ITinyRegistry Register<T>();
    ITinyRegistry Register<TService, TImplementation>() where TImplementation : TService;
    ITinyRegistry Register<T>(Func<ITinyResolver, T> factory);
}

public interface ITinyResolver
{
    T Resolve<T>();
}

public class TinyContainer : ITinyRegistry, ITinyResolver { ... }
```
Here's all you really need to know:
- All registrations are transient-scoped. Build your own caching layer!
- Registrations are explicit (no forgiveness or magic auto-binding).
- Re-registration of the same type throws an exception.

That's it! Have fun!
## Examples
```c#
public interface ISimple { }

public class Simple : ISimple { }
```
### Basic Usage
```c#
var tiny = new TinyContainer();

tiny.Register<Simple>();
var instance = tiny.Resolve<Simple>();

tiny.Register<ISimple, Simple>();
var instance = tiny.Resolve<ISimple>();
```
### Registration with Factories
```c#
tiny.Register(factory => new Simple());
var instance = tiny.Resolve<Simple>();

tiny.Register<ISimple>(factory => factory.Resolve<Simple>());
var instance = tiny.Resolve<ISimple>();
```
### Delegate Registration
```c#
tiny.Register<Func<Simple>>(factory => () => new Simple());
tiny.Register<Func<ISimple>>(factory => factory.Resolve<Simple>);

var instance = tiny.Resolve<Func<Simple>>()();
```
### Modules
```c#
public class MyModule : ITinyModule
{
    public void RegisterServices(ITinyRegistry registry)
    {
        registry.Register<Simple>();
    }
}
```
```c#
new MyModule().RegisterServices(container);
// or
container.RegisterModule(new MyModule());
// or
container.RegisterModule<MyModule>();

var instance = tiny.Resolve<Func<Simple>>()();
```