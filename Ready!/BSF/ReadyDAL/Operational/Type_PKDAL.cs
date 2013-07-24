// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 14:25:55
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Type_PKDAL : GEMDataAccess<Type_PK>, IType_PKOperations
	{
		public Type_PKDAL() : base() { }
		public Type_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IType_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetAvailablePackagingMaterialsForProduct", OperationType = GEMOperationType.Select)]
        public List<Type_PK> GetAvailablePackagingMaterialsForProduct(Int32? productPk)
        {
            DateTime methodStart = DateTime.Now;
            List<Type_PK> entities = new List<Type_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProductPk", productPk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetAssignedPackagingMaterialsForProduct", OperationType = GEMOperationType.Select)]
        public List<Type_PK> GetAssignedPackagingMaterialsForProduct(Int32? productPk)
        {
            DateTime methodStart = DateTime.Now;
            List<Type_PK> entities = new List<Type_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProductPk", productPk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetEntityByGroup", OperationType = GEMOperationType.Select)]
        public Type_PK GetEntityByGroup(String group, String name)
        {
            var methodStart = DateTime.Now;
            var entity = new Type_PK();

            try
            {
                // Input parameters validation
                if (group == null) return entity;

                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("group", group, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetTypesForDDL", OperationType = GEMOperationType.Select)]
        public List<Type_PK> GetTypesForDDL(String group)
        {
            DateTime methodStart = DateTime.Now;
            List<Type_PK> entities = new List<Type_PK>();

            try
            {
                // Input parameters validation
                if (group == null) return entities;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("group", group, DbType.String, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetGroups", OperationType = GEMOperationType.Select)]
        public DataSet GetGroups()
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetAvailableTypesForActivity", OperationType = GEMOperationType.Select)]
        public List<Type_PK> GetAvailableTypesForActivity(Int32? activity_pk)
        {
            DateTime methodStart = DateTime.Now;
            List<Type_PK> entities = new List<Type_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_pk", activity_pk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetSelectedTypesForActivity", OperationType = GEMOperationType.Select)]
        public List<Type_PK> GetAssignedTypesForActivity(Int32? activity_pk)
        {
            DateTime methodStart = DateTime.Now;
            List<Type_PK> entities = new List<Type_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_pk", activity_pk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_TYPE_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                var orderBy = base.CreateOrderByClause(orderByConditions);
                var parameters = (from pair in filters where !String.IsNullOrWhiteSpace(pair.Value) select new GEMDbParameter(pair.Key, pair.Value, DbType.String, ParameterDirection.Input)).ToList();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                Dictionary<string, object> outputValues;

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (outputValues.ContainsKey("totalRecordsCount"))
                {
                    totalRecordsCount = (int)outputValues["totalRecordsCount"];
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

		#endregion

		#region ICRUDOperations<Type_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Type_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Type_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Type_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Type_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_Save", OperationType = GEMOperationType.Save)]
		public override Type_PK Save(Type_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TYPE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Type_PK> SaveCollection(List<Type_PK> entities)
		{
			return base.SaveCollection(entities);
		}

		public override void DeleteCollection<PKType>(List<PKType> entityPKs)
		{
			base.DeleteCollection<PKType>(entityPKs);
		}

		#endregion
	}
}
