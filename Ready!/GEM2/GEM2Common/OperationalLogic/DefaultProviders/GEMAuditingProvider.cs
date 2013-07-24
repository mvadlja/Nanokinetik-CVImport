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
using System.Threading;
using System.Data.Common;
using System.Configuration;
using System.Data;

namespace GEM2Common
{
    public class GEMAuditingProvider : IGEMAuditing
    {
        #region IGEMAuditing<T> Members

        public void AuditOperation<T>(T oldValue, T newValue, GEMOperationType operationType, string dataSourceId) where T : class
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            PropertyInfo entityPKInfo = null;
            object entityPK = null;
            object masterID = null;
            DateTime auditTime = DateTime.Now;

            Dictionary<string, string> oldValues = new Dictionary<string, string>();
            Dictionary<string, string> newValues = new Dictionary<string, string>();

            GEMAuditingAttribute entityAuditingInfo = GetAuditingBindingForEntity(typeof(T), dataSourceId);
            string dbName = null;
            string tableName = null;

            string connectionString = null;
            DbProviderFactory dbFactory = null;
            DbConnection connection = null;
            DbCommand masterCommand = null;
            DbCommand detailCommand = null;
            DbParameter parameter = null;

            // Audit only if entity has GEMAuditingAttribute for specified data source
            if (entityAuditingInfo != null)
            {
                dbName = entityAuditingInfo.Database;
                tableName = entityAuditingInfo.Table;

                entityPKInfo = GetEntityPKInfo(typeof(T), dataSourceId);

                // Insert
                if (operationType == GEMOperationType.Insert && newValue == null) throw new ArgumentException("New value cannot be null on insert.");
                // Delete
                if (operationType == GEMOperationType.Delete && oldValue == null) throw new ArgumentException("Old value cannot be null on delete.");
                // Update
                if (operationType == GEMOperationType.Update && (oldValue == null || newValue == null)) throw new ArgumentException("New and old values cannot be null on update.");

                // Auditing by operation
                if (operationType == GEMOperationType.Insert)
                {
                    newValues = GetPropertiesToAudit<T>(newValue, dataSourceId);
                    entityPK = entityPKInfo.GetValue(newValue, null);
                }
                else if (operationType == GEMOperationType.Delete)
                {
                    oldValues = GetPropertiesToAudit<T>(oldValue, dataSourceId);
                    entityPK = entityPKInfo.GetValue(oldValue, null);
                }
                else if (operationType == GEMOperationType.Update || operationType == GEMOperationType.Complex)
                {
                    oldValues = GetPropertiesToAudit<T>(oldValue, dataSourceId);
                    newValues = GetPropertiesToAudit<T>(newValue, dataSourceId);
                    entityPK = entityPKInfo.GetValue(newValue, null);
                }
                else
                {
                    throw new NotSupportedException("Operation not supported.");
                }

                // Auditing master data
                if (newValues.Count > 0 || oldValues.Count > 0)
                {
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
                        masterCommand = connection.CreateCommand();
                        masterCommand.CommandType = CommandType.StoredProcedure;
                        masterCommand.CommandText = "proc_AuditingMaster_AuditOperation";
                        detailCommand = connection.CreateCommand();
                        detailCommand.CommandType = CommandType.StoredProcedure;
                        detailCommand.CommandText = "proc_AuditingDetails_AuditSingleColumn";

                        // Username
                        parameter = masterCommand.CreateParameter();
                        parameter.ParameterName = "Username";
                        parameter.Value = username;
                        parameter.DbType = DbType.String;
                        masterCommand.Parameters.Add(parameter);

                        // DBName
                        parameter = masterCommand.CreateParameter();
                        parameter.ParameterName = "DBName";
                        parameter.Value = dbName;
                        parameter.DbType = DbType.String;
                        masterCommand.Parameters.Add(parameter);

                        // TableName
                        parameter = masterCommand.CreateParameter();
                        parameter.ParameterName = "TableName";
                        parameter.Value = tableName;
                        parameter.DbType = DbType.String;
                        masterCommand.Parameters.Add(parameter);

                        // Date
                        parameter = masterCommand.CreateParameter();
                        parameter.ParameterName = "Date";
                        parameter.Value = DateTime.Now;
                        parameter.DbType = DbType.DateTime;
                        masterCommand.Parameters.Add(parameter);

                        // Operation
                        parameter = masterCommand.CreateParameter();
                        parameter.ParameterName = "Operation";
                        parameter.Value = operationType.ToString()[0];
                        parameter.DbType = DbType.String;
                        masterCommand.Parameters.Add(parameter);

                        // ServerName
                        parameter = masterCommand.CreateParameter();
                        parameter.ParameterName = "ServerName";
                        parameter.Value = Environment.MachineName;
                        parameter.DbType = DbType.String;
                        masterCommand.Parameters.Add(parameter);

                        String sessionValue = String.Empty;

                        if (System.Web.HttpContext.Current == null ||
                            System.Web.HttpContext.Current.Session == null ||
                            System.Web.HttpContext.Current.Session["AUDIT_TRAIL_TOKEN"] == null)
                        {
                            sessionValue = GEMConfigurationHelper.GenerateNewSessionToken(32);
                        }
                        else {
                            sessionValue = (String)System.Web.HttpContext.Current.Session["AUDIT_TRAIL_TOKEN"];
                        }

                        parameter = masterCommand.CreateParameter();
                        parameter.ParameterName = "SessionToken";
                        parameter.Value = sessionValue;
                        parameter.DbType = DbType.String;
                        masterCommand.Parameters.Add(parameter);

                        connection.Open();
                        // Master audit
                        masterID = masterCommand.ExecuteScalar();
                        
                        if (operationType == GEMOperationType.Insert)
                        {
                            foreach (KeyValuePair<string, string> kvp in newValues)
                            {
                                detailCommand.Parameters.Clear();

                                // MasterID
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "MasterID";
                                parameter.Value = Convert.ToInt32(masterID);
                                parameter.DbType = DbType.Int32;
                                detailCommand.Parameters.Add(parameter);

                                // ColumnName
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "ColumnName";
                                parameter.Value = kvp.Key;
                                parameter.DbType = DbType.String;
                                detailCommand.Parameters.Add(parameter);

                                // OldValue
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "OldValue";
                                parameter.Value = null;
                                parameter.DbType = DbType.String;
                                detailCommand.Parameters.Add(parameter);

                                // NewValue
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "NewValue";
                                parameter.Value = kvp.Value;
                                parameter.DbType = DbType.String;
                                detailCommand.Parameters.Add(parameter);

                                // PKValue
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "PKValue";
                                parameter.Value = entityPK.ToString();
                                parameter.DbType = DbType.String;
                                detailCommand.Parameters.Add(parameter);

                                // Detail audit
                                detailCommand.ExecuteNonQuery();
                            }
                        }
                        else if (operationType == GEMOperationType.Delete)
                        {
                            foreach (KeyValuePair<string, string> kvp in oldValues)
                            {
                                detailCommand.Parameters.Clear();

                                // MasterID
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "MasterID";
                                parameter.Value = Convert.ToInt32(masterID);
                                parameter.DbType = DbType.Int32;
                                detailCommand.Parameters.Add(parameter);

                                // ColumnName
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "ColumnName";
                                parameter.Value = kvp.Key;
                                parameter.DbType = DbType.String;
                                detailCommand.Parameters.Add(parameter);

                                // OldValue
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "OldValue";
                                parameter.Value = kvp.Value;
                                parameter.DbType = DbType.String;
                                detailCommand.Parameters.Add(parameter);

                                // NewValue
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "NewValue";
                                parameter.Value = null;
                                parameter.DbType = DbType.String;
                                detailCommand.Parameters.Add(parameter);

                                // PKValue
                                parameter = masterCommand.CreateParameter();
                                parameter.ParameterName = "PKValue";
                                parameter.Value = entityPK.ToString();
                                parameter.DbType = DbType.String;
                                detailCommand.Parameters.Add(parameter);

                                // Detail audit
                                detailCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<string, string> kvp in oldValues)
                            {
                                if (kvp.Value.Trim() != newValues[kvp.Key].Trim())
                                {
                                    detailCommand.Parameters.Clear();

                                    // MasterID
                                    parameter = masterCommand.CreateParameter();
                                    parameter.ParameterName = "MasterID";
                                    parameter.Value = Convert.ToInt32(masterID);
                                    parameter.DbType = DbType.Int32;
                                    detailCommand.Parameters.Add(parameter);

                                    // ColumnName
                                    parameter = masterCommand.CreateParameter();
                                    parameter.ParameterName = "ColumnName";
                                    parameter.Value = kvp.Key;
                                    parameter.DbType = DbType.String;
                                    detailCommand.Parameters.Add(parameter);

                                    // OldValue
                                    parameter = masterCommand.CreateParameter();
                                    parameter.ParameterName = "OldValue";
                                    parameter.Value = kvp.Value;
                                    parameter.DbType = DbType.String;
                                    detailCommand.Parameters.Add(parameter);

                                    // NewValue
                                    parameter = masterCommand.CreateParameter();
                                    parameter.ParameterName = "NewValue";
                                    parameter.Value = newValues[kvp.Key];
                                    parameter.DbType = DbType.String;
                                    detailCommand.Parameters.Add(parameter);

                                    // PKValue
                                    parameter = masterCommand.CreateParameter();
                                    parameter.ParameterName = "PKValue";
                                    parameter.Value = entityPK.ToString();
                                    parameter.DbType = DbType.String;
                                    detailCommand.Parameters.Add(parameter);

                                    // Detail audit
                                    detailCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion


        // Extracting all mapped non confidential properties from entity to dictionary
        private Dictionary<string, string> GetPropertiesToAudit<T>(T entity, string dataSourceId)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            GEMPropertyBindingAttribute pba = null;
            string columnName = String.Empty;
            object columnTempValue = null;
            string columnValue = String.Empty;

            // Passing through all properties of object
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                pba = GetPropertyBindingAttributeForPropertyInfo(pi, dataSourceId);

                // Only bound properties are audited
                if (pba != null)
                {
                    // Skipping confidential data
                    if (pba.IsConfidential) continue;
                    else
                    {
                        columnName = GetParameterNameForPropertyInfo(pi, dataSourceId);
                        
                        // Getting property value from entity
                        columnTempValue = entity.GetType().GetProperty(pi.Name).GetValue(entity, null);
                        columnValue = columnTempValue == null ? String.Empty : columnTempValue.ToString();

                        // EXCEPTIONS !!!!
                        if (pba.ParameterType == DbType.Boolean) {
                            if (columnValue == "True" || columnValue == "1") columnValue = "Yes";
                            if (columnValue == "False" || columnValue == "0") columnValue = "No";
                        }
                        if (columnName == "intensive_monitoring") {
                            if (columnValue == "1") columnValue = "Yes";
                            if (columnValue == "2") columnValue = "No";
                        }

                        // Adding name-value pair to dictionary
                        properties.Add(columnName, columnValue);
                    }
                }
            }

            return properties;
        }

        private GEMAuditingAttribute GetAuditingBindingForEntity(Type type, string dataSourceId)
        {
            GEMAuditingAttribute ab = null;
            GEMAuditingAttribute[] allAbs = null;

            allAbs = (GEMAuditingAttribute[])(type.GetCustomAttributes(typeof(GEMAuditingAttribute), true));

            if (allAbs != null)
            {
                foreach (GEMAuditingAttribute tempAb in allAbs)
                {
                    if (tempAb.DataSourceId == dataSourceId)
                    {
                        ab = tempAb;
                        break;
                    }
                }
            }

            return ab;
        }

        private PropertyInfo GetEntityPKInfo(Type type, string dataSourceId)
        {
            PropertyInfo pi = null;
            GEMPropertyBindingAttribute pba = null;

            foreach (PropertyInfo tempPi in type.GetProperties())
            {
                pba = GetPropertyBindingAttributeForPropertyInfo(tempPi, dataSourceId);

                if (pba != null && pba.IsPrimaryKey)
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

        private string GetParameterNameForPropertyInfo(PropertyInfo pi, string dataSourceId)
        {
            string pn = null;
            GEMPropertyBindingAttribute pba = null;

            pba = GetPropertyBindingAttributeForPropertyInfo(pi, dataSourceId);

            if (pba != null)
            {
                if (!String.IsNullOrEmpty(pba.ParameterName))
                {
                    pn = pba.ParameterName;
                }
                else
                {
                    pn = pi.Name;
                }
            }

            return pn;
        }
    }
}
