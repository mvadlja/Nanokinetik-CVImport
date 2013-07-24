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
    /// Cache for GEM configured providers and related configuration.
    /// </summary>
    public static class GEMConfigurationHelper
    {
        private static GEMConfiguration _gemConfig;
        private static Dictionary<string, object> _instances = new Dictionary<string, object>();

        private static string _useDataSourceId;
        private static bool _useAuditing;
        private static bool _useExceptionLogging;
        private static bool _useConcurrencyChecking;
        private static bool _useOperationsLogging;

        #region Properties

        public static string UseDataSourceId
        {
            get { return _useDataSourceId; }
        }

        public static bool UseAuditing
        {
            get { return _useAuditing; }
        }

        public static bool UseExceptionLogging
        {
            get { return _useExceptionLogging; }
        }

        public static bool UseConcurrencyChecking
        {
            get { return _useConcurrencyChecking; }
        }

        public static bool UseOperationsLogging
        {
            get { return _useOperationsLogging; }
        }

        #endregion

        static GEMConfigurationHelper()
        {
            try
            {
                _gemConfig = (GEMConfiguration)ConfigurationManager.GetSection("gemConfiguration");

                if (_gemConfig == null)
                {
                    _useDataSourceId = "Default";
                    _useAuditing = false;
                    _useExceptionLogging = false;
                    _useConcurrencyChecking = false;
                    _useOperationsLogging = false;
                }
                else
                {
                    _useDataSourceId = String.IsNullOrEmpty(_gemConfig.UseDataSourceId) ? String.Empty : _gemConfig.UseDataSourceId;
                    _useAuditing = String.IsNullOrEmpty(_gemConfig.UseAuditing) ? false : Convert.ToBoolean(_gemConfig.UseAuditing);
                    _useExceptionLogging = String.IsNullOrEmpty(_gemConfig.UseExceptionLogging) ? false : Convert.ToBoolean(_gemConfig.UseExceptionLogging);
                    _useConcurrencyChecking = String.IsNullOrEmpty(_gemConfig.UseConcurrencyChecking) ? false : Convert.ToBoolean(_gemConfig.UseConcurrencyChecking);
                    _useOperationsLogging = String.IsNullOrEmpty(_gemConfig.UseOperationsLogging) ? false : Convert.ToBoolean(_gemConfig.UseOperationsLogging);

                    if (_useAuditing) _instances.Add(typeof(IGEMAuditing).Name, InstanceManager.GetInstance<IGEMAuditing>(_gemConfig.AuditingProvider));
                    if (_useExceptionLogging) _instances.Add(typeof(IGEMExceptionLogging).Name, InstanceManager.GetInstance<IGEMExceptionLogging>(_gemConfig.ExceptionLoggingProvider));
                    if (_useConcurrencyChecking) _instances.Add(typeof(IGEMConcurrencyChecking).Name, InstanceManager.GetInstance<IGEMConcurrencyChecking>(_gemConfig.ConcurrencyCheckingProvider));
                    if (_useOperationsLogging) _instances.Add(typeof(IGEMOperationLogging).Name, InstanceManager.GetInstance<IGEMOperationLogging>(_gemConfig.OperationsLoggingProvider));
                }
            }
            catch (Exception ex)
            {
                throw new GEMConfigurationException("Exception in GEMConfigurationHelper. See inner exception for more details.", ex);
            }
        }

        public static IGEMAuditing GetAuditingProvider()
        {
            IGEMAuditing auditingProvider = null;

            try
            {
                auditingProvider = (IGEMAuditing)_instances[typeof(IGEMAuditing).Name];
            }
            catch (Exception ex)
            {
                throw new GEMConfigurationException("Exception in GEMConfigurationHelper. See inner exception for more details.", ex);
            }

            return auditingProvider;
        }

        public static IGEMExceptionLogging GetExceptionLoggingProvider()
        {
            IGEMExceptionLogging exceptionLoggingProvider = null;

            try
            {
                exceptionLoggingProvider = (IGEMExceptionLogging)_instances[typeof(IGEMExceptionLogging).Name];
            }
            catch (Exception ex)
            {
                throw new GEMConfigurationException("Exception in GEMConfigurationHelper. See inner exception for more details.", ex);
            }

            return exceptionLoggingProvider;
        }

        public static IGEMConcurrencyChecking GetConcurrencyCheckingProvider()
        {
            IGEMConcurrencyChecking concurrencyCheckingProvider = null;

            try
            {
                concurrencyCheckingProvider = (IGEMConcurrencyChecking)_instances[typeof(IGEMConcurrencyChecking).Name];
            }
            catch (Exception ex)
            {
                throw new GEMConfigurationException("Exception in GEMConfigurationHelper. See inner exception for more details.", ex);
            }

            return concurrencyCheckingProvider;
        }

        public static IGEMOperationLogging GetOperationsLoggingProvider()
        {
            IGEMOperationLogging operationsLoggingProvider = null;

            try
            {
                operationsLoggingProvider = (IGEMOperationLogging)_instances[typeof(IGEMOperationLogging).Name];
            }
            catch (Exception ex)
            {
                throw new GEMConfigurationException("Exception in GEMConfigurationHelper. See inner exception for more details.", ex);
            }

            return operationsLoggingProvider;
        }

        public static String GenerateNewSessionToken(int n) {
            String ret = "";
            Random rnd = new Random();
            const String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789";

            for (int i = 0; i < n; ++i)
            {
                ret += chars[rnd.Next(chars.Length)];
            }
            return ret;
        }
    }
}
