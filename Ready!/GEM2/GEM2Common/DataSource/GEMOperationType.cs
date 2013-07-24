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
    /// Defines CRUD operation types for data source.
    /// </summary>
    public enum GEMOperationType
    {
        None = 0,
        Select = 1,
        Save = 2,
        Insert = 4,
        Update = 8,
        Delete = 16,
        Complex = 32
    }
}
