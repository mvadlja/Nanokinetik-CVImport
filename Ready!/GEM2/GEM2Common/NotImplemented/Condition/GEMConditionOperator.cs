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
    /// 
    /// </summary>
    internal enum GEMConditionOperator
    {
        Equal,
        Different,
        Like,
        NotLike,
        HigherThen,
        LowerThen,
        HigherThenOrEqual,
        LowerThenOrEqual,
        In,
        NotIn,
        Between,
        IsNull,
        IsNotNull
    }
}
