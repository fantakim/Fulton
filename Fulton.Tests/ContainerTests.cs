using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fulton.Tests
{
    /// <summary>
    /// 컨테이너 기능 테스트
    /// </summary>
    [TestClass]
    public class ContainerTests
    {
        public interface IFoo { }
        public interface IBar { }
        public class Foo : IFoo { }
        public class Bar : IBar { public Bar(IFoo foo) { } }

        [TestMethod]
        public void ShouldBeRegisterTest()
        {
            var container = new Container();

            var foo = new Foo();
            container.Register<IFoo>(r => foo);

            var bar = new Bar(container.Resolve<IFoo>());
            container.Register<IBar>(r => bar);

            var resolveFoo = container.Resolve<IFoo>();
            var resolveBar = container.Resolve<IBar>();
            
            Assert.AreSame(foo, resolveFoo);
            Assert.AreSame(bar, resolveBar);
        }
    }
}
