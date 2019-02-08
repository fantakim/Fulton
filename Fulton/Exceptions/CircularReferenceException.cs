using System;
using System.Runtime.Serialization;

namespace Fulton.Exceptions
{
    /// <summary>
    /// 순환 의존성 예외
    /// </summary>
    [Serializable]
    public class CircularReferenceException : Exception
    {
        public CircularReferenceException()
        {
        }

        public CircularReferenceException(string message) : base(message)
        {
        }

        public CircularReferenceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CircularReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
