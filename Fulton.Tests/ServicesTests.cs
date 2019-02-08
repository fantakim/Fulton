using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Fulton.Tests
{
    [TestClass]
    public class ServicesTests
    {
        public interface ILogger
        {
            void Log(string message);
        }

        public class Logger : ILogger
        {
            public void Log(string message)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    Debug.WriteLine(string.Format("[{0}] {1}\r\n", DateTime.Now, message));
                }
            }
        }

        public class ConsoleLogger
        {
            private ILogger logger;

            public ConsoleLogger(ILogger logger)
            {
                this.logger = logger;
                this.DisplayMessage();
            }

            public void DisplayMessage()
            {
                logger.Log("Hi..See it is working!!");
            }
        }

        [TestMethod]
        public void ServicesTest()
        {
            var container = new Container();
            container.Register<ILogger, Logger>();
            container.Register<ConsoleLogger, ConsoleLogger>();

            var logger = container.Resolve<ConsoleLogger>();
            logger.DisplayMessage();
        }
    }
}
