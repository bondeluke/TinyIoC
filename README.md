# TinyIoC
A tiny inversion-of-control container
```c#
public interface ISimple { }

public class Simple : ISimple { }
```
```c#
var tiny = new TinyContainer();

tiny.Register<ISimple, Simple>();
tiny.Resolve<ISimple>();
```
```c#
tiny.Register<ISimple>();
```
