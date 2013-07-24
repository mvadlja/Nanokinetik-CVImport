//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace GEM2Common
{
    [Serializable()]
    public class GEMExceptionLoggingException : Exception
    {
        public GEMExceptionLoggingException() : base() { }
        public GEMExceptionLoggingException(string message) : base(message) { }
        public GEMExceptionLoggingException(string message, Exception innerException) : base(message, innerException) { }

        protected GEMExceptionLoggingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
