using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CommonTypes
{
    [Serializable()]
    public class OperationalException : Exception
    {
        public OperationalException() : base() { }
        public OperationalException(string message) : base(message) { }
        public OperationalException(string message, Exception inner) : base(message, inner) { }

        protected OperationalException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
