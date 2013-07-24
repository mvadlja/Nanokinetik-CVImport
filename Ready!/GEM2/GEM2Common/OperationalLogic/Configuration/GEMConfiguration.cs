//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace GEM2Common
{
    /// <summary>
    /// GEM configuration reader
    /// </summary>
    public class GEMConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("useDataSourceId", IsRequired = false)]
        public string UseDataSourceId
        {
            get { return this["useDataSourceId"].ToString(); }
        }

        [ConfigurationProperty("useAuditing", IsRequired = false)]
        public string UseAuditing
        {
            get { return this["useAuditing"].ToString(); }
        }

        [ConfigurationProperty("auditingProvider", IsRequired = false)]
        public string AuditingProvider
        {
            get { return this["auditingProvider"].ToString(); }
        }

        [ConfigurationProperty("useExceptionLogging", IsRequired = false)]
        public string UseExceptionLogging
        {
            get { return this["useExceptionLogging"].ToString(); }
        }

        [ConfigurationProperty("exceptionLoggingProvider", IsRequired = false)]
        public string ExceptionLoggingProvider
        {
            get { return this["exceptionLoggingProvider"].ToString(); }
        }

        [ConfigurationProperty("useConcurrencyChecking", IsRequired = false)]
        public string UseConcurrencyChecking
        {
            get { return this["useConcurrencyChecking"].ToString(); }
        }

        [ConfigurationProperty("concurrencyCheckingProvider", IsRequired = false)]
        public string ConcurrencyCheckingProvider
        {
            get { return this["concurrencyCheckingProvider"].ToString(); }
        }

        [ConfigurationProperty("useOperationsLogging", IsRequired = false)]
        public string UseOperationsLogging
        {
            get { return this["useOperationsLogging"].ToString(); }
        }

        [ConfigurationProperty("operationsLoggingProvider", IsRequired = false)]
        public string OperationsLoggingProvider
        {
            get { return this["operationsLoggingProvider"].ToString(); }
        }
    }
}
