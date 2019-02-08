using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Fulton.Tests
{
    /// <summary>
    /// 다중스레드에 대한 안전성 테스트
    /// </summary>
    [TestClass]
    public class ConcurrencyTests
    {
        public interface IFoo { }
        public interface IBar { }
        public class Foo : IFoo { }
        public class Bar : IBar { public Bar(IFoo foo) { } }

        [TestMethod]
        public async Task ShouldBeThreadSafeTest()
        {
            var container = new Container();
            container.Register<IFoo>(r => new Foo());
            container.Register<IBar>(r => new Bar(r.Resolve<IFoo>()));

            var tasks = new List<Task>();
            for (int i = 0; i < 5000; i++)
            {
                if (i % 2 == 0)
                    tasks.Add(ResolveTask1(container));
                else
                    tasks.Add(ResolveTask2(container));
            }

            await Task.WhenAll(tasks);
        }

        private Task ResolveTask1(IContainer container)
        {
            return Task.Run(() =>
            {
                var foo = container.Resolve<IFoo>();
                Debug.WriteLine(string.Format("Task1 - [{0}] {1}", Thread.CurrentThread.ManagedThreadId, foo.GetHashCode()));
            });
        }

        private Task ResolveTask2(IContainer container)
        {
            return Task.Run(() =>
            {
                var bar = container.Resolve<IBar>();
                Debug.WriteLine(string.Format("Task2 - [{0}] {1}", Thread.CurrentThread.ManagedThreadId, bar.GetHashCode()));
            });
        }
    }
}
