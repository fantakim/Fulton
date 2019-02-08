using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fulton
{
    public static class ServiceLocator
    {
        static IContainer container;

        public static void InitializeWith(IContainer containerImplementation)
        {
            container = containerImplementation;
        }

        public static T Get<T>()
        {
            return container.Resolve<T>();
        }
    }
}
