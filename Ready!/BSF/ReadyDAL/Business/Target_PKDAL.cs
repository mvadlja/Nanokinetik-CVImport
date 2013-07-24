// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:11:05
// Description:	GEM2 Generated class for table SSI.dbo.TARGET
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Target_PKDAL : GEMDataAccess<Target_PK>, ITarget_PKOperations
	{
		public Target_PKDAL() : base() { }
		public Target_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ITarget_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TARGET_GetTargetByRIPK", OperationType = GEMOperationType.Select)]
        public List<Target_PK> GetTargetByRIPK(Int32? RIPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Target_PK> entities = new List<Target_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (RIPK != null) parameters.Add(new GEMDbParameter("RIPK", RIPK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

		#endregion

		#region ICRUDOperations<Target_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TARGET_GetEntity", OperationType = GEMOperationType.Select)]
		public override Target_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TARGET_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Target_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TARGET_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Target_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TARGET_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Target_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TARGET_Save", OperationType = GEMOperationType.Save)]
		public override Target_PK Save(Target_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TARGET_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Target_PK> SaveCollection(List<Target_PK> entities)
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
