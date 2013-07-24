//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace GEM2Common
{
    /// <summary>
    /// Uniquely identifies type.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class GEMIdentifierAttribute : Attribute
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public GEMIdentifierAttribute() { }
        public GEMIdentifierAttribute(string id)
        {
            this.Id = id;
        }
    }
}
