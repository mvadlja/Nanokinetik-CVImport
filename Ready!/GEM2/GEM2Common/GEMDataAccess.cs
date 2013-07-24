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
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Transactions;

namespace GEM2Common
{
    /// <summary>
    /// Base generic data access class with implemented ICRUDOperations for any entity of type T
    /// </summary>
    /// <typeparam name="T">Entity on which operations are performed</typeparam>
    public abstract class GEMDataAccess<T> : ICRUDOperations<T> where T : class
    {
        private PropertyInfo[] _properties_T;
        private PropertyInfo _entityPKInfo;
        private string _entityPKParameterName;
        private GEMPropertyBindingAttribute _entityPKPropertyBinding;
        private string _dataSourceId;
        private GEMDataSourceBindingAttribute _dataSourceBinding_T;
        private ConnectionStringSettings _dataSourceConnectionSettings;
        private DbProviderFactory _dataSourceProviderFactory;
        
        #region Properties

        /// <summary>
        /// Properties of entity type T
        /// </summary>
        protected PropertyInfo[] Properties_T
        {
            get { return _properties_T; }
            private set { _properties_T = value; }
        }

        /// <summary>
        /// Property of T entity that is configured as primary key on data source 
        /// </summary>
        protected PropertyInfo EntityPKInfo
        {
            get { return _entityPKInfo; }
            private set { _entityPKInfo = value; }
        }

        /// <summary>
        /// Name of primary key parameter of T entity on data source
        /// </summary>
        protected string EntityPKParameterName
        {
            get { return _entityPKParameterName; }
            private set { _entityPKParameterName = value; }
        }

        /// <summary>
        /// GEMPropertyBindingAttribute of primary key property on data source
        /// </summary>
        protected GEMPropertyBindingAttribute EntityPKPropertyBinding
        {
            get { return _entityPKPropertyBinding; }
            private set { _entityPKPropertyBinding = value; }
        }

        /// <summary>
        /// Data source identifier (multiple data sources can be mapped to entity T)
        /// </summary>
        protected string DataSourceId
        {
            get { return _dataSourceId; }
            private set { _dataSourceId = value; }
        }

        /// <summary>
        /// Data source mapped to entity T selected by data source id
        /// </summary>
        protected GEMDataSourceBindingAttribute DataSourceBinding_T
        {
            get { return _dataSourceBinding_T; }
            private set { _dataSourceBinding_T = value; }
        }

        /// <summary>
        /// Connection string settings for selected data source from config file
        /// </summary>
        protected ConnectionStringSettings DataSourceConnectionSettings
        {
            get { return _dataSourceConnectionSettings; }
            private set { _dataSourceConnectionSettings = value; }
        }

        /// <summary>
        /// Generic database factory based on database provider from connection string of selected data source
        /// </summary>
        protected DbProviderFactory DataSourceProviderFactory
        {
            get { return _dataSourceProviderFactory; }
            private set { _dataSourceProviderFactory = value; }
        }

        #endregion

        /// <summary>
        /// If not specified otherwise, "Default" data source id is used
        /// </summary>
        public GEMDataAccess() : this(null) { }
        public GEMDataAccess(string dataSourceId)
        {
            try
            {
                // If dataSourceId is null or empty, use value from config file
                if (String.IsNullOrEmpty(dataSourceId))
                {
                    dataSourceId = GEMConfigurationHelper.UseDataSourceId;

                    // If not configured, use "Default"
                    if (String.IsNullOrEmpty(dataSourceId))
                    {
                        DataSourceId = "Default";
                    }
                }

                DataSourceId = dataSourceId;

                // Extracts properties of T
                Properties_T = typeof(T).GetProperties();

                // T data source binding
                DataSourceBinding_T = GetDataSourceBindingForEntity(DataSourceId);

                if (DataSourceBinding_T == null) throw new Exception("Entity of type " + typeof(T).FullName + " does not have valid GEMDataSourceBindingAttribute for requested DataSourceId (" + DataSourceId + ").");

                // Entity's primary key property
                EntityPKInfo = GetEntityPKInfo(DataSourceId);

                if (EntityPKInfo == null) throw new Exception("Entity of type " + typeof(T).FullName + " does not have property marked with GEMPropertyBindingAttribute that is configured as primary key, for requested DataSourceId (" + DataSourceId + ").");

                // Entity's primary key parameter name
                EntityPKParameterName = GetParameterNameForPropertyInfo(EntityPKInfo, DataSourceId);

                // Entity's GEMPropertyBindingAttribute
                EntityPKPropertyBinding = GetPropertyBindingForPropertyInfo(EntityPKInfo, DataSourceId);

                // Query string initialization
                DataSourceConnectionSettings = ConfigurationManager.ConnectionStrings[DataSourceBinding_T.ConnectionStringName];

                if (DataSourceConnectionSettings == null) throw new Exception("Connection string with name " + DataSourceBinding_T.ConnectionStringName + " is not found.");

                // Database factory initialization
                DataSourceProviderFactory = DbProviderFactories.GetFactory(DataSourceConnectionSettings.ProviderName);

                if (DataSourceProviderFactory == null) throw new Exception("Data source provider " + DataSourceConnectionSettings.ProviderName + " is not found.");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        #region Data source operations helpers

        /// <summary>
        /// Creates data source connection, creates and prepares command, executes stored procedure, retreives single row and transforms it to type T. 
        /// </summary>
        /// <param name="parameters">Input and output parameters</param>
        /// <returns></returns>
        protected T ExecuteProcedureReturnEntity(List<GEMDbParameter> parameters)
        {
            T entity = null;
            List<T> entities = new List<T>();

            Dictionary<string, object> dummyDict = new Dictionary<string, object>();
            entities = ExecuteProcedureReturnEntities(parameters, out dummyDict);

            if (entities.Count > 0) entity = entities[0];

            return entity;
        }

        /// <summary>
        /// Creates data source connection, creates and prepares command, executes stored procedure, retreives rows and transforms them to type List&lt;T&gt;. 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected List<T> ExecuteProcedureReturnEntities(List<GEMDbParameter> parameters)
        {
            Dictionary<string, object> dummyDict = new Dictionary<string, object>();
            return ExecuteProcedureReturnEntities(parameters, out dummyDict);
        }

        /// <summary>
        /// Creates data source connection, creates and prepares command, executes stored procedure, retreives rows, transforms them to type List&lt;T&gt; and retreives output values. 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="outputValues"></param>
        /// <returns></returns>
        protected List<T> ExecuteProcedureReturnEntities(List<GEMDbParameter> parameters, out Dictionary<string, object> outputValues)
        {
            List<T> entities = new List<T>();
            outputValues = new Dictionary<string, object>();

            using (DbConnection connection = CreateDbConnection())
            {
                DbCommand command = CreateDbCommandForStoredProcedure(connection);

                if (parameters != null)
                {
                    foreach (GEMDbParameter parameter in parameters)
                    {
                        SetCommandParameter(command, parameter.ParameterName, parameter.Value, parameter.DbType, parameter.Direction);
                    }
                }

                connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            entities.Add(GetEntitiyFromDataReader(reader, DataSourceId));
                        }
                    }

                    // Total records count
                    if (reader.NextResult())
                    {
                        reader.Read();

                        // Retreiving total count output value
                        outputValues.Add("totalRecordsCount", reader.GetInt32(0));
                    }
                }

                // Retreiving output values
                foreach (DbParameter parameter in command.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Input) continue;

                    outputValues.Add(parameter.ParameterName, parameter.Value);
                }
            }

            return entities;
        }

        /// <summary>
        /// Creates data source connection, creates and prepares command, executes stored procedure and optionally returns single result. 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object ExecuteProcedureReturnScalar(List<GEMDbParameter> parameters)
        {
            object result = null;

            using (DbConnection connection = CreateDbConnection())
            {
                DbCommand command = CreateDbCommandForStoredProcedure(connection);

                if (parameters != null)
                {
                    foreach (GEMDbParameter parameter in parameters)
                    {
                        SetCommandParameter(command, parameter.ParameterName, parameter.Value, parameter.DbType, parameter.Direction);
                    }
                }

                connection.Open();
                result = command.ExecuteScalar();
            }

            return result;
        }

        /// <summary>
        /// Creates data source connection, creates and prepares command, executes stored procedure, retreives rows and transforms them to DataSet.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected DataSet ExecuteProcedureReturnDataSet(List<GEMDbParameter> parameters)
        {
            Dictionary<string, object> dummyDict = new Dictionary<string, object>();
            return ExecuteProcedureReturnDataSet(parameters, out dummyDict);
        }

        /// <summary>
        /// Creates data source connection, creates and prepares command, executes stored procedure, retreives rows, transforms them to DataSet and retreives output values. 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="outputValues"></param>
        /// <returns></returns>
        protected DataSet ExecuteProcedureReturnDataSet(List<GEMDbParameter> parameters, out Dictionary<string, object> outputValues)
        {
            DataSet ds = new DataSet();
            outputValues = new Dictionary<string, object>();

            using (DbConnection connection = CreateDbConnection())
            {
                DbCommand command = CreateDbCommandForStoredProcedure(connection);

                if (parameters != null)
                {
                    foreach (GEMDbParameter parameter in parameters)
                    {
                        SetCommandParameter(command, parameter.ParameterName, parameter.Value, parameter.DbType, parameter.Direction);
                    }
                }

                DbDataAdapter da = DataSourceProviderFactory.CreateDataAdapter();
                da.SelectCommand = command;
                da.Fill(ds);

                // Retreiving total count output value
                if (ds.Tables.Count > 1)
                {
                    outputValues.Add("totalRecordsCount", ds.Tables[ds.Tables.Count - 1].Rows[0][0]);
                }

                // Retreiving output values
                foreach (DbParameter parameter in command.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Input) continue;

                    outputValues.Add(parameter.ParameterName, parameter.Value);
                }
            }

            return ds;
        }

        /// <summary>
        /// Creates DbConnection from configured provider factory and connection string
        /// </summary>
        /// <returns></returns>
        private DbConnection CreateDbConnection()
        {
            DbConnection connection = DataSourceProviderFactory.CreateConnection();
            connection.ConnectionString = DataSourceConnectionSettings.ConnectionString;

            return connection;
        }

   

        /// <summary>
        /// Creates DbCommand for requested entity operation
        /// </summary>
        /// <param name="connection">Data source connection</param>
        /// <returns></returns>
        private DbCommand CreateDbCommandForStoredProcedure(DbConnection connection)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;

            MethodBase entityOperation = null;
            GEMOperationBindingAttribute oba = null;
       
        
            // Looping through StackFrames (up to 9 calls on call stack) untill reaching operation method that has OperationBindingAttribute (requested entity operation)
            for (int i = 1; i < 10; i++)
            {
                entityOperation = new StackFrame(i,false).GetMethod();
                oba = GetOperationBindingForEntityOperation(entityOperation, DataSourceId);

                if (oba != null) break;
            }

            if (oba == null) throw new Exception("Requested entity operation (" + entityOperation.Name + ") does not have GEMOperationBindingAttribute defined for requested DataSourceId (" + DataSourceId + ").");

            command.CommandText = oba.ProcedureName;

            return command;
        }

        /// <summary>
        /// Creates and sets DbParameter to specified DbCommand
        /// </summary>
        /// <param name="command">Command to which parameter will be added</param>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="type">Parameter type</param>
        /// <param name="direction">Parameter direction</param>
        private void SetCommandParameter(DbCommand command, string name, object value, DbType? type, ParameterDirection? direction)
        {
            // If value is not null
            if (value != null)
            {
                // Preparing value
                if (value.GetType().Equals(typeof(string))) value = PrepareParameterValue(value.ToString());
            }

            // Existing parameter
            if (command.Parameters.Contains(name)) command.Parameters[name].Value = value;
            // New parameter
            else
            {
                DbParameter param = command.CreateParameter();
                param.ParameterName = name;
                param.Value = value;

                if (type.HasValue) param.DbType = type.Value;
                if (direction.HasValue) param.Direction = direction.Value;

                command.Parameters.Add(param);
            }
        }

        #endregion

        #region Attributes extraction helpers

        /// <summary>
        /// Extracts GEMDataSourceBindingAttribute from T entity for specified data source
        /// </summary>
        /// <param name="dataSourceId">Data source identifier</param>
        /// <returns></returns>
        private GEMDataSourceBindingAttribute GetDataSourceBindingForEntity(string dataSourceId)
        {
            GEMDataSourceBindingAttribute dsb = null;
            GEMDataSourceBindingAttribute[] allDsbs = null;

            allDsbs = (GEMDataSourceBindingAttribute[])(typeof(T).GetCustomAttributes(typeof(GEMDataSourceBindingAttribute), true));

            if (allDsbs != null)
            {
                foreach (GEMDataSourceBindingAttribute tempDsb in allDsbs)
                {
                    if (tempDsb.DataSourceId == dataSourceId)
                    {
                        dsb = tempDsb;
                        break;
                    }
                }
            }

            return dsb;
        }

        /// <summary>
        /// Extracts GEMPropertyBindingAttribute from entity property for specified data source
        /// </summary>
        /// <param name="pi">Targeted property</param>
        /// <param name="dataSourceId">Data source identifier</param>
        /// <returns></returns>
        /// 
        Dictionary<PropertyInfo, GEMPropertyBindingAttribute[]> cache= new Dictionary<PropertyInfo,GEMPropertyBindingAttribute[]>();
        private GEMPropertyBindingAttribute GetPropertyBindingForPropertyInfo(PropertyInfo pi, string dataSourceId)
        {
            GEMPropertyBindingAttribute pb = null;
            GEMPropertyBindingAttribute[] allPbs = null;

            if (!cache.ContainsKey(pi)) {
                cache.Add(pi,(GEMPropertyBindingAttribute[])pi.GetCustomAttributes(typeof(GEMPropertyBindingAttribute), true));
            }
            allPbs = cache[pi];

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

        /// <summary>
        /// Extracts GEMOperationBindingAttribute from requested entity operation for specified data source
        /// </summary>
        /// <param name="entityOperation">Entity method that is being invoked</param>
        /// <param name="dataSourceId">Data source identifier</param>
        /// <returns></returns>
        private GEMOperationBindingAttribute GetOperationBindingForEntityOperation(MethodBase entityOperation, string dataSourceId)
        {
            GEMOperationBindingAttribute oba = null;
            GEMOperationBindingAttribute[] allObas = null;

            allObas = (GEMOperationBindingAttribute[])entityOperation.GetCustomAttributes(typeof(GEMOperationBindingAttribute), true);

            if (allObas != null)
            {
                foreach (GEMOperationBindingAttribute tempOba in allObas)
                {
                    if (tempOba.DataSourceId == dataSourceId)
                    {
                        oba = tempOba;
                        break;
                    }
                }
            }

            return oba;
        }

        /// <summary>
        /// Retreives GEMAuditingAttribute from T entity for specified data source.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataSourceId"></param>
        /// <returns></returns>
        private GEMAuditingAttribute GetAuditingBindingForEntity(string dataSourceId)
        {
            GEMAuditingAttribute ab = null;
            GEMAuditingAttribute[] allAbs = null;

            allAbs = (GEMAuditingAttribute[])(typeof(T).GetCustomAttributes(typeof(GEMAuditingAttribute), true));

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

        /// <summary>
        /// Retreives GEMOperationsLoggingAttribute from T entity for specified data source.
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <returns></returns>
        private GEMOperationsLoggingAttribute GetOperationsLoggingBindingForEntity(string dataSourceId)
        {
            GEMOperationsLoggingAttribute olb = null;
            GEMOperationsLoggingAttribute[] allOlbs = null;

            allOlbs = (GEMOperationsLoggingAttribute[])(typeof(T).GetCustomAttributes(typeof(GEMOperationsLoggingAttribute), true));

            if (allOlbs != null)
            {
                foreach (GEMOperationsLoggingAttribute tempOlb in allOlbs)
                {
                    if (tempOlb.DataSourceId == dataSourceId)
                    {
                        olb = tempOlb;
                        break;
                    }
                }
            }

            return olb;
        }

        /// <summary>
        /// Retreives GEMConcurrencyCheckingAttribute from T entity for specified data source.
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <returns></returns>
        private GEMConcurrencyCheckingAttribute GetConcurrencyCheckingBindingForEntity(string dataSourceId)
        {
            GEMConcurrencyCheckingAttribute ccb = null;
            GEMConcurrencyCheckingAttribute[] allCcbs = null;

            allCcbs = (GEMConcurrencyCheckingAttribute[])(typeof(T).GetCustomAttributes(typeof(GEMConcurrencyCheckingAttribute), true));

            if (allCcbs != null)
            {
                foreach (GEMConcurrencyCheckingAttribute tempCcb in allCcbs)
                {
                    if (tempCcb.DataSourceId == dataSourceId)
                    {
                        ccb = tempCcb;
                        break;
                    }
                }
            }

            return ccb;
        }

        #endregion

        #region Other helpers

        /// <summary>
        /// Exception handleing - wrapping original exception to GEMDataAccessException and re-throwing
        /// </summary>
        /// <param name="ex">Original exception</param>
        protected void HandleException(Exception ex)
        {
            // Exception ID
            Guid exceptionKey = Guid.NewGuid();

            // Wrapping exception to its final form
            GEMDataAccessException dae = new GEMDataAccessException("Exception in GEMDataAccess. See inner exception for more details." + (ex.Message != null ? " InnerException: " + ex.Message : ""), ex);

            // Setting exception key through exception hierarchy
            SetExceptionKey(dae, exceptionKey);

            // Logging wrapped exception through exception logging provider - if applicable
            LogException(dae);

            // Propagating GEMDataAccessException
            throw dae;
        }

        /// <summary>
        /// Creates ORDER BY clause
        /// </summary>
        /// <param name="orderByConditions">Collection of ORDER BY conditions</param>
        /// <returns></returns>
        protected string CreateOrderByClause(List<GEMOrderBy> orderByConditions)
        {
            string orderBy = String.Empty;

            // If null or empty, ordering by primary key ASC
            if (orderByConditions == null || orderByConditions.Count == 0)
            {
                orderBy = EntityPKParameterName + " ASC";
            }
            else
            {
                foreach (GEMOrderBy obc in orderByConditions)
                {
                    orderBy += PrepareParameterValue(obc.OrderByElement) + " " + obc.OrderByType.ToString() + ", ";
                }

                orderBy = orderBy.Remove(orderBy.Length - 2, 2);
            }

            return orderBy;
        }

        /// <summary>
        /// Finds primary key entity property for specified data source
        /// </summary>
        /// <param name="dataSourceId">Data source identifier</param>
        /// <returns></returns>
        private PropertyInfo GetEntityPKInfo(string dataSourceId)
        {
            PropertyInfo pi = null;
            GEMPropertyBindingAttribute pba = null;

            foreach (PropertyInfo tempPi in Properties_T)
            {
                pba = GetPropertyBindingForPropertyInfo(tempPi, dataSourceId);

                if (pba != null && pba.IsPrimaryKey)
                {
                    pi = tempPi;
                    break;
                }
            }

            return pi;
        }

        /// <summary>
        /// Finds row version entity property for specified data source
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <returns></returns>
        private PropertyInfo GetEntityRowVersionInfo(string dataSourceId)
        {
            PropertyInfo pi = null;
            GEMPropertyBindingAttribute pba = null;

            foreach (PropertyInfo tempPi in Properties_T)
            {
                pba = GetPropertyBindingForPropertyInfo(tempPi, dataSourceId);

                if (pba != null && pba.IsRowVersion)
                {
                    pi = tempPi;
                    break;
                }
            }

            return pi;
        }

        /// <summary>
        /// Retreives parameter name from property (property name if ParameterName is not specified in GEMPropertyBindingAttribute)
        /// </summary>
        /// <param name="pi">Information from entity's property</param>
        /// <param name="dataSourceId">Data source identifier</param>
        /// <returns></returns>
        private string GetParameterNameForPropertyInfo(PropertyInfo pi, string dataSourceId)
        {
            string pn = null;
            GEMPropertyBindingAttribute pba = null;

            pba = GetPropertyBindingForPropertyInfo(pi, dataSourceId);

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

        /// <summary>
        /// Sets GUID on exception and its inner exception hierarchy
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exceptionKey"></param>
        private void SetExceptionKey(Exception ex, Guid exceptionKey)
        {
            ex.Data["GUID"] = exceptionKey;

            if (ex.InnerException != null) SetExceptionKey(ex.InnerException, exceptionKey);
        }

        /// <summary>
        /// Creates entity of type T based on resultset from data reader, will NOT close IDataReader
        /// </summary>
        /// <param name="reader">Active data reader</param>
        /// <param name="dataSourceId">Data source identifier</param>
        /// <returns></returns>
        private T GetEntitiyFromDataReader(DbDataReader reader, string dataSourceId)
        {
            T entity = Activator.CreateInstance(typeof(T)) as T;

            GEMPropertyBindingAttribute pba = null;
            string parameterName = null;

            // Filling every property from this entity which is bound to data source parameter
            for (int i = 0; i < Properties_T.Length; i++)
            {
                pba = GetPropertyBindingForPropertyInfo(Properties_T[i], dataSourceId);

                // Proceeding only if current property is bound to data source parameter
                if (pba != null)
                {
                    // Getting data source parameter name from current bound property
                    parameterName = GetParameterNameForPropertyInfo(Properties_T[i], dataSourceId);

                    try
                    {
                        // Is value DBNull.Value
                        if (reader[parameterName] != DBNull.Value)
                        {
                            Properties_T[i].SetValue(entity, reader[parameterName], null);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Nothing, skipping this property
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// Prevents SQL Injection
        /// </summary>
        /// <param name="value">Value to be prepared</param>
        /// <returns></returns>
        protected string PrepareParameterValue(string value)
        {
            return value;
            //if (value != null) value = value.Replace("'", "''");
            //return value;
        }

        
        /// <summary>
        /// Not implemented!
        /// </summary>
        /// <param name="conditionsGroups"></param>
        /// <returns></returns>
        private string CreateWherePartFromConditions(List<GEMConditionGroup> conditionsGroups)
        {
            throw new NotImplementedException();

            //StringBuilder wherePart = new StringBuilder();
            //StringBuilder tempGroupPart = null;
            //StringBuilder tempConditionPart = null;

            //for (int i = 0; i < conditionsGroups.Count; i++)
            //{
            //    tempGroupPart = new StringBuilder();

            //    // First group
            //    if (i == 0)
            //    {
            //        if (conditionsGroups[i].ConditionGroupJoiner != GEMConditionJoiner.NONE)
            //            throw new Exception("First group condition joiner must be set to GEMConditionJoiner.NONE.");
            //    }
            //    else
            //    {
            //        // Group joiner
            //        tempGroupPart.Append(" " + conditionsGroups[i].ConditionGroupJoiner.ToString());
            //    }

            //    // Opening group bracket
            //    tempGroupPart.Append(" (");

            //    for (int j = 0; j < conditionsGroups[i].ConditionsCollection.Count; j++)
            //    {
            //        tempConditionPart = new StringBuilder();

            //        if (j == 0)
            //        {
            //            if (conditionsGroups[i].ConditionsCollection[j].ConditionJoiner != GEMConditionJoiner.NONE)
            //                throw new Exception("First condition joiner must be set to GEMConditionJoiner.NONE.");
            //        }
            //        else
            //        {
            //            // Condition joiner
            //            tempConditionPart.Append(" " + conditionsGroups[i].ConditionsCollection[j].ConditionJoiner.ToString());
            //        }

            //        // Column name
            //        // ...
            //    }
            //}

            //return wherePart.ToString();
        }

        #endregion

        #region ICRUDOperations<T> Members

        public virtual T GetEntity<PKType>(PKType entityId)
        {
            DateTime methodStart = DateTime.Now;
            T entity = null;

            try
            {
                // Input parameters validation
                if (entityId == null) return entity;

                List<GEMDbParameter> parameters = new List<GEMDbParameter>()
                {
                    new GEMDbParameter(EntityPKParameterName, entityId, EntityPKPropertyBinding.ParameterType, ParameterDirection.Input)
                };

                entity = ExecuteProcedureReturnEntity(parameters);

                LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return entity;
        }

        public virtual List<T> GetEntities()
        {
            DateTime methodStart = DateTime.Now;
            List<T> entities = new List<T>();

            try
            {
                entities = ExecuteProcedureReturnEntities(null);

                // Logging operation if applicable
                LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            
            return entities;
        }

        public virtual List<T> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            List<T> entities = new List<T>();
            totalRecordsCount = 0;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>()
                {
                    new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input),
                    new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input)
                };

                entities = ExecuteProcedureReturnEntities(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            
            return entities;
        }

        public virtual List<T> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            List<T> entities = new List<T>();
            totalRecordsCount = 0;
            string orderBy = null;

            try
            {
                // Generating order by clause
                orderBy = CreateOrderByClause(orderByConditions);

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>()
                {
                    new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input),
                    new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input),
                    new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input)
                };

                entities = ExecuteProcedureReturnEntities(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return entities;
        }

        public virtual T Save(T entity)
        {
            DateTime methodStart = DateTime.Now;
            
            if (entity == null) return null;
            
            GEMPropertyBindingAttribute tempPba = null;
            string parameterName = null;
            object parameterValue = null;
            PropertyInfo rowVersionProperty = null;
            object scopeIdentity = null;
            T oldValue = null;
            GEMOperationType recognizedOperationType = GEMOperationType.None;
            List<GEMDbParameter> parameters = new List<GEMDbParameter>();

            try
            {
                // Extracting primary key value
                object entityPKValue = EntityPKInfo.GetValue(entity, null);
                
                // Update
                if (entityPKValue != null)
                {
                    recognizedOperationType = GEMOperationType.Update;

                    // Getting old value from data source, if auditing or concurrency providers are configured
                    if (GEMConfigurationHelper.UseAuditing || GEMConfigurationHelper.UseConcurrencyChecking) oldValue = GetEntity<object>(entityPKValue);

                    // Optimistic concurrency check, if applicable
                    VersionCheck<T>(oldValue, entity); 
                }
                // Insert
                else
                {
                    recognizedOperationType = GEMOperationType.Insert;
                }
                
                rowVersionProperty = GetEntityRowVersionInfo(DataSourceId);

                // Updateing entity RowVersion
                if (rowVersionProperty != null) rowVersionProperty.SetValue(entity, DateTime.Now, null);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (PropertyInfo pi in Properties_T)
                    {
                        // If this property is null, continue
                        if (pi.GetValue(entity, null) == null) continue;
                        else parameterValue = pi.GetValue(entity, null);

                        tempPba = GetPropertyBindingForPropertyInfo(pi, DataSourceId);

                        if (tempPba != null)
                        {
                            parameterName = GetParameterNameForPropertyInfo(pi, DataSourceId);
                            parameters.Add(new GEMDbParameter(parameterName, parameterValue, tempPba.ParameterType, ParameterDirection.Input));
                        }
                    }

                    scopeIdentity = ExecuteProcedureReturnScalar(parameters);

                    // If applicable, setting primary key value on entity
                    if (scopeIdentity != null) EntityPKInfo.SetValue(entity, scopeIdentity, null);

                    // Auditing operation if applicable
                    AuditOperation<T>(oldValue, entity, recognizedOperationType);

                    // Logging operation if applicable
                    LogOperation(methodStart);

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return entity;
        }

        public virtual void Delete<PKType>(PKType entityId)
        {
            DateTime methodStart = DateTime.Now;
            T oldValue = null;

            try
            {
                if (entityId == null) return;

                // Getting old value from datasource, if auditing is configured
                if (GEMConfigurationHelper.UseAuditing) oldValue = GetEntity<object>(entityId);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Dictionary<string, object> returnValues = new Dictionary<string, object>();
                    List<GEMDbParameter> parameters = new List<GEMDbParameter>()
                    {
                        new GEMDbParameter(EntityPKParameterName, entityId, EntityPKPropertyBinding.ParameterType, ParameterDirection.Input)
                    };

                    ExecuteProcedureReturnScalar(parameters);

                    // Auditing operation if applicable
                    AuditOperation<T>(oldValue, null, GEMOperationType.Delete);

                    // Logging operation if applicable
                    LogOperation(methodStart);

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public virtual List<T> SaveCollection(List<T> entities)
        {
            DateTime methodStart = DateTime.Now;
            
            if (entities != null)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        entities[i] = Save(entities[i]);
                    }

                    // Logging operation if applicable
                    LogOperation(methodStart);

                    ts.Complete();
                }
            }

            return entities;
        }

        public virtual void DeleteCollection<PKType>(List<PKType> entityPKs)
        {
            DateTime methodStart = DateTime.Now;
            
            if (entityPKs != null)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (PKType entityPK in entityPKs)
                    {
                        Delete<PKType>(entityPK);
                    }

                    // Logging operation if applicable
                    LogOperation(methodStart);

                    ts.Complete();
                }
            }
        }

        #endregion

        #region Operational support

        // Wrapper arround auditing provider - only entities that have GEMAuditingAttribute will be audited, and if global auditing is set to true
        protected void AuditOperation<T>(T oldValue, T newValue, GEMOperationType operationType) where T : class
        {
            try
            {
                if (GEMConfigurationHelper.UseAuditing)
                {
                    GEMAuditingAttribute attribute = GetAuditingBindingForEntity(DataSourceId);

                    if (attribute != null && attribute.Active == true)
                    {
                        GEMConfigurationHelper.GetAuditingProvider().AuditOperation<T>(oldValue, newValue, operationType, DataSourceId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new GEMAuditingException("Exception in auditing provider." + (ex.Message != null ? " InnerException: " + ex.Message : ""), ex);
            }
        }

        // Wrapper arround exception logging provider
        protected void LogException(Exception exception)
        {
            try
            {
                if (GEMConfigurationHelper.UseExceptionLogging)
                {
                    GEMConfigurationHelper.GetExceptionLoggingProvider().LogException(exception, DataSourceId);
                }
            }
            catch (Exception ex)
            {
                throw new GEMExceptionLoggingException("Exception in exception logging provider.", ex);
            }
        }

        // Wrapper arround concurrency checking provider - only entities that have GEMConcurrencyCheckingAttribute will be checked, and if global concurrency checking is set to true
        protected void VersionCheck<T>(T oldValue, T newValue) where T : class
        {
            bool match = false;

            try
            {
                if (GEMConfigurationHelper.UseConcurrencyChecking)
                {
                    GEMConcurrencyCheckingAttribute attribute = GetConcurrencyCheckingBindingForEntity(DataSourceId);

                    if (attribute != null && attribute.Active == true)
                    {
                        match = GEMConfigurationHelper.GetConcurrencyCheckingProvider().VersionCheck<T>(oldValue, newValue, DataSourceId);
                    }
                    else match = true;
                }
                else match = true;
            }
            catch (Exception ex)
            {
                throw new GEMOptimisticConcurrencyException("Exception in concurrency checking provider.", ex);
            }

            if (match == false)
            {
                throw new GEMOptimisticConcurrencyException("Save aborted due to optimistic concurrency. Version of entity " + typeof(T).Name + " with primary key " + EntityPKInfo.GetValue(oldValue, null).ToString() + " in data source changed after it was retreived from data source.");
            }
        }

        // Wrapper arround operation logging provider - only operations on entities that have GEMOperationsLoggingAttribute will be logged, and if global operations logging is set to true
        protected void LogOperation(DateTime operationStart)
        {
            try
            {
                if (GEMConfigurationHelper.UseOperationsLogging)
                {
                    GEMOperationsLoggingAttribute attribute = GetOperationsLoggingBindingForEntity(DataSourceId);

                    if (attribute != null && attribute.Active == true)
                    {
                        // Name of this type
                        string typeName = this.GetType().FullName;
                        // Name of method that called this method
                        string methodName = new StackFrame(1).GetMethod().Name;

                        GEMConfigurationHelper.GetOperationsLoggingProvider().LogOperation(operationStart, typeName, methodName, DataSourceId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new GEMOperationLoggingException("Exception in operation logging provider.", ex);
            }
        }

        #endregion
    }
}
