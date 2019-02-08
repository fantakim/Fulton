using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fulton.Tests
{
    [TestClass]
    public class ServiceLocatorAntiPatternTests
    {
        public interface IFoo { }
        public interface IBar { }
        public class Foo : IFoo { }
        public class Bar : IBar { public Bar(IFoo foo) { } }

        [TestInitialize]
        public void Initialize()
        {
            var container = new Container();
            container.Register<IFoo, Foo>();
            container.Register<IBar, Bar>();

            ServiceLocator.InitializeWith(container);
        }

        [TestMethod]
        public void JustProofTest()
        {
            var bar = ServiceLocator.Get<IBar>();
        }
    }
}
