# Fulton
Simple IoC Container for C#

# Example

```c#
var container = new Container();

container.Register<IFoo, Foo>();
container.Register<IBar>(r => new Bar(r.Resolve<IFoo>()));

/* ... */

var resolveFoo = container.Resolve<IFoo>();
var resolveBar = container.Resolve<IBar>();
```
