# TinyIoC
TinyIoC is a tiny inversion-of-control container. The library only contains 3 exported types.
The small surface area of this API allows your project to follow a consistent and predictable dependency injection experience.
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

public class TinyContainer : ITinyRegistry, ITinyResolver
{
	...
```
# Examples
```c#
public interface ISimple { }

public class Simple : ISimple { }
```
Basic Usage
```c#
var tiny = new TinyContainer();

tiny.Register<Simple>();
var instance = tiny.Resolve<Simple>();

tiny.Register<ISimple, Simple>();
var instance = tiny.Resolve<ISimple>();
```
Registration With Factories
```c#
tiny.Register(factory => new Simple());
tiny.Register<ISimple>(factory => factory.Resolve<Simple>());

var instance = tiny.Resolve<ISimple>();
```
Delegate Registration
```c#
tiny.Register<Func<Simple>>(factory => () => new Simple());
tiny.Register<Func<ISimple>>(factory => factory.Resolve<Simple>);

var instance = tiny.Resolve<Func<Simple>>()();
```
