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
    public class GEMOptimisticConcurrencyException : Exception
    {
        public GEMOptimisticConcurrencyException() : base() { }
        public GEMOptimisticConcurrencyException(string message) : base(message) { }
        public GEMOptimisticConcurrencyException(string message, Exception innerException) : base(message, innerException) { }

        protected GEMOptimisticConcurrencyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
