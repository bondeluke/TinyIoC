# TinyIoC
A tiny inversion-of-control container
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
