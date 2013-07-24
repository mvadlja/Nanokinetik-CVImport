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
using System.Threading;
using System.Data.Common;
using System.Configuration;
using System.Data;

namespace GEM2Common
{
    public class GEMOperationLoggingProvider : IGEMOperationLogging
    {
        #region IGEMOperationLogging Members

        public void LogOperation(DateTime operationStart, string typeName, string methodName, string dataSourceId)
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            string machineName = Environment.MachineName;
            DateTime dateEnd = DateTime.Now;

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
                command.CommandText = "proc_LoggedOperations_LogOperation";

                // Username
                parameter = command.CreateParameter();
                parameter.ParameterName = "Username";
                parameter.Value = username;
                parameter.DbType = DbType.String;
                command.Parameters.Add(parameter);

                // ServerName
                parameter = command.CreateParameter();
                parameter.ParameterName = "ServerName";
                parameter.Value = machineName;
                parameter.DbType = DbType.String;
                command.Parameters.Add(parameter);

                // TypeName
                parameter = command.CreateParameter();
                parameter.ParameterName = "TypeName";
                parameter.Value = typeName;
                parameter.DbType = DbType.String;
                command.Parameters.Add(parameter);

                // MethodName
                parameter = command.CreateParameter();
                parameter.ParameterName = "MethodName";
                parameter.Value = methodName;
                parameter.DbType = DbType.String;
                command.Parameters.Add(parameter);

                // DateStart
                parameter = command.CreateParameter();
                parameter.ParameterName = "DateStart";
                parameter.Value = operationStart;
                parameter.DbType = DbType.DateTime;
                command.Parameters.Add(parameter);

                // DateEnd
                parameter = command.CreateParameter();
                parameter.ParameterName = "DateEnd";
                parameter.Value = dateEnd;
                parameter.DbType = DbType.DateTime;
                command.Parameters.Add(parameter);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
