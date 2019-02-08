using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fulton.Exceptions;

namespace Fulton.Tests
{
    /// <summary>
    /// 순환 의존성 테스트
    /// </summary>
    [TestClass]
    public class CircularReferencesTests
    {
        public class Foz { public Foz(Baz baz) { } }
        public class Baz { public Baz(Foz foz) { } }

        [TestMethod]
        [ExpectedException(typeof(CircularReferenceException))]
        public void ShouldBeCircularReferencesExceptionTest()
        {
            var container = new Container();
            container.Register<Foz, Foz>();
            container.Register<Baz, Baz>();

            container.Resolve<Foz>();
        }
    }
}
