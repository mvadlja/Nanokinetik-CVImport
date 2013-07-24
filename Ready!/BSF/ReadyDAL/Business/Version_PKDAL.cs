// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:11:15
// Description:	GEM2 Generated class for table SSI.dbo.VERSION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Version_PKDAL : GEMDataAccess<Version_PK>, IVersion_PKOperations
	{
		public Version_PKDAL() : base() { }
		public Version_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IVersion_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_SUBSTANCE_MN_GetVERBySubstancePK", OperationType = GEMOperationType.Select)]
        public List<Version_PK> GetVERBySubstancePK(Int32? SubstancePK)
        {
            DateTime methodStart = DateTime.Now;
            List<Version_PK> entities = new List<Version_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (SubstancePK != null) parameters.Add(new GEMDbParameter("SubstancePK", SubstancePK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Version_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_GetEntity", OperationType = GEMOperationType.Select)]
		public override Version_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Version_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Version_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Version_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_Save", OperationType = GEMOperationType.Save)]
		public override Version_PK Save(Version_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Version_PK> SaveCollection(List<Version_PK> entities)
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
