//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Data.Common;
using System.Threading;
using System.Configuration;
using System.Data;

namespace GEM2Common
{
    public class GEMExceptionLoggingProvider : IGEMExceptionLogging
    {
        #region IGEMExceptionLogging Members

        public void LogException(Exception exception, string dataSourceId)
        {
            // Transaction supression is a must!
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                string username = Thread.CurrentPrincipal.Identity.Name;
                string machineName = Environment.MachineName;
                DateTime date = DateTime.Now;

                string connectionString = null;
                DbProviderFactory dbFactory = null;
                DbConnection connection = null;
                DbCommand command = null;
                DbParameter parameter = null;

                // Choosing data source dynamically based on dataSourceId
                switch (dataSourceId)
                {
                    case "MSSQL":
                        dbFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["GEM2Operational_MSSQL"].ProviderName);
                        connectionString = ConfigurationManager.ConnectionStrings["GEM2Operational_MSSQL"].ConnectionString;
                        break;
                    case "MySQL":
                        dbFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["GEM2Operational_MySQL"].ProviderName);
                        connectionString = ConfigurationManager.ConnectionStrings["GEM2Operational_MySQL"].ConnectionString;
                        break;
                    default: // "Default"
                        dbFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["GEM2Operational_MSSQL"].ProviderName);
                        connectionString = ConfigurationManager.ConnectionStrings["GEM2Operational_MSSQL"].ConnectionString;
                        break;
                }

                using (connection = dbFactory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "proc_LoggedExceptions_LogException";

                    // Username
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "Username";
                    parameter.Value = username;
                    parameter.DbType = DbType.String;
                    command.Parameters.Add(parameter);

                    // ExceptionType
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "ExceptionType";
                    parameter.Value = exception.GetType().Name;
                    parameter.DbType = DbType.String;
                    command.Parameters.Add(parameter);

                    // ExceptionMessage
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "ExceptionMessage";
                    parameter.Value = exception.Message;
                    parameter.DbType = DbType.String;
                    command.Parameters.Add(parameter);

                    // TargetSite
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "TargetSite";
                    parameter.Value = exception.TargetSite == null ? null : exception.TargetSite.Name;
                    parameter.DbType = DbType.String;
                    command.Parameters.Add(parameter);

                    // StackTrace
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "StackTrace";
                    parameter.Value = exception.StackTrace;
                    parameter.DbType = DbType.String;
                    command.Parameters.Add(parameter);

                    // Source
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "Source";
                    parameter.Value = exception.Source;
                    parameter.DbType = DbType.String;
                    command.Parameters.Add(parameter);

                    // Date
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "Date";
                    parameter.Value = date;
                    parameter.DbType = DbType.DateTime;
                    command.Parameters.Add(parameter);

                    // ServerName
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "ServerName";
                    parameter.Value = machineName;
                    parameter.DbType = DbType.String;
                    command.Parameters.Add(parameter);

                    // UniqueKey
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "UniqueKey";
                    parameter.Value = exception.Data["GUID"] == null ? Guid.Empty : (Guid?)new Guid(exception.Data["GUID"].ToString());
                    parameter.DbType = DbType.Guid;
                    command.Parameters.Add(parameter);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Logging inner exception if exists
                if (exception.InnerException != null)
                {
                    LogException(exception.InnerException, dataSourceId);
                }
            }
        }

        #endregion
    }
}
