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
    public interface IGEMConcurrencyChecking
    {
        bool VersionCheck<T>(T oldValue, T newValue, string dataSourceId) where T : class;
    }
}
