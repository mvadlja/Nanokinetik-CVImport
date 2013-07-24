// ======================================================================================================================
// Author:		ANTEC\Kiki
// Create date:	4.12.2011. 10:28:29
// Description:	GEM2 Generated class for table SSI.dbo.OFFICIAL_NAME
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Official_name_PKDAL : GEMDataAccess<Official_name_PK>, IOfficial_name_PKOperations
	{
		public Official_name_PKDAL() : base() { }
		public Official_name_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IOfficial_name_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SN_ON_MN_GetONBySNPK", OperationType = GEMOperationType.Select)]
        public List<Official_name_PK> GetONBySNPK(Int32? SNPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Official_name_PK> entities = new List<Official_name_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (SNPK != null) parameters.Add(new GEMDbParameter("SNPK", SNPK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Official_name_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_GetEntity", OperationType = GEMOperationType.Select)]
		public override Official_name_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Official_name_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Official_name_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Official_name_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_Save", OperationType = GEMOperationType.Save)]
		public override Official_name_PK Save(Official_name_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Official_name_PK> SaveCollection(List<Official_name_PK> entities)
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
