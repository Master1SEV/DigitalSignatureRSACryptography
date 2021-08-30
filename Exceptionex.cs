using System;
using System.Runtime.Serialization;

namespace WindowsFormsApp3
{
    [Serializable]
    internal class Exceptionex : Exception
    {
        public Exceptionex()
        {
        }

        public Exceptionex(string message) : base(message)
        {
        }

        public Exceptionex(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected Exceptionex(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}