//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace GEM2Common
{
    public class GEMConcurrencyCheckingProvider : IGEMConcurrencyChecking
    {
        #region IGEMConcurrencyChecking Members

        public bool VersionCheck<T>(T oldValue, T newValue, string dataSourceId) where T : class
        {
            bool match = false;
            object oldValueVersion = null;
            object newValueVersion = null;

            // Getting rowversion for dataSourceId
            PropertyInfo pi = GetEntityRowVersionInfo(typeof(T), dataSourceId);

            if (pi != null)
            {
                oldValueVersion = pi.GetValue(oldValue, null);
                newValueVersion = pi.GetValue(newValue, null);

                // If entity in data source has no row version, then version check is true
                if (oldValueVersion == null) match = true;
                // If current entity does not have row version, or it is different as of entity in data source, then version check is false
                else if (newValueVersion == null || Comparer<object>.Default.Compare(oldValueVersion, newValueVersion) != 0) match = false;
                else match = true;
            }
            // No row version property was found, returning true
            else
            {
                match = true;
            }

            return match;
        }

        #endregion


        private PropertyInfo GetEntityRowVersionInfo(Type type, string dataSourceId)
        {
            PropertyInfo pi = null;
            GEMPropertyBindingAttribute pba = null;

            foreach (PropertyInfo tempPi in type.GetProperties())
            {
                pba = GetPropertyBindingAttributeForPropertyInfo(tempPi, dataSourceId);

                if (pba != null && pba.IsRowVersion)
                {
                    pi = tempPi;
                    break;
                }
            }

            return pi;
        }

        private GEMPropertyBindingAttribute GetPropertyBindingAttributeForPropertyInfo(PropertyInfo pi, string dataSourceId)
        {
            GEMPropertyBindingAttribute pb = null;
            GEMPropertyBindingAttribute[] allPbs = null;

            allPbs = (GEMPropertyBindingAttribute[])pi.GetCustomAttributes(typeof(GEMPropertyBindingAttribute), true);

            if (allPbs != null)
            {
                foreach (GEMPropertyBindingAttribute tempPb in allPbs)
                {
                    if (tempPb.DataSourceId == dataSourceId)
                    {
                        pb = tempPb;
                        break;
                    }
                }
            }

            return pb;
        }
    }
}
