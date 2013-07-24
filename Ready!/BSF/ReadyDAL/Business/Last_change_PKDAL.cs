// ======================================================================================================================
// Author:		space-monkey\dpetek
// Create date:	4.9.2012. 16:05:09
// Description:	GEM2 Generated class for table ready_dev.dbo.ENTITY_LAST_CHANGE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Last_change_PKDAL : GEMDataAccess<Last_change_PK>, ILast_change_PKOperations
	{
		public Last_change_PKDAL() : base() { }
		public Last_change_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ILast_change_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ENTITY_LAST_CHANGE_GetEntityLastChange", OperationType = GEMOperationType.Select)]
        public Last_change_PK GetEntityLastChange(string table_name, int? entity_PK)
        {
            DateTime methodStart = DateTime.Now;
            Last_change_PK entity = null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("table_name", table_name, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("entity_PK", entity_PK, DbType.Int32, ParameterDirection.Input));
                entity = ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }

		#endregion

		#region ICRUDOperations<Last_change_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ENTITY_LAST_CHANGE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Last_change_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ENTITY_LAST_CHANGE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Last_change_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ENTITY_LAST_CHANGE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Last_change_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ENTITY_LAST_CHANGE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Last_change_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ENTITY_LAST_CHANGE_Save", OperationType = GEMOperationType.Save)]
		public override Last_change_PK Save(Last_change_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ENTITY_LAST_CHANGE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Last_change_PK> SaveCollection(List<Last_change_PK> entities)
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
