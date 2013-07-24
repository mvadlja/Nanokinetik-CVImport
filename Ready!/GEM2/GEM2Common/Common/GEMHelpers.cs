//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GEM2Common
{
    /// <summary>
    /// Various helper methods.
    /// </summary>
    public static class GEMHelpers
    {
        /// <summary>
        /// GUID validation
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsValidGUID(string data)
        {
            string RE_GUID = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";
            Regex r = new Regex(RE_GUID, RegexOptions.Compiled);
            return r.IsMatch(data);
        }
    }
}
