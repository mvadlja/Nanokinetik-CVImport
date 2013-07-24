// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:10:41
// Description:	GEM2 Generated class for table SSI.dbo.SUBTYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Subtype_PKDAL : GEMDataAccess<Subtype_PK>, ISubtype_PKOperations
	{
		public Subtype_PKDAL() : base() { }
		public Subtype_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubtype_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_GetSubtypeBySCLFPK", OperationType = GEMOperationType.Select)]
        public List<Subtype_PK> GetSubtypeBySCLFPK(Int32? SCLFPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Subtype_PK> entities = new List<Subtype_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (SCLFPK != null) parameters.Add(new GEMDbParameter("SCLFPK", SCLFPK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Subtype_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Subtype_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Subtype_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Subtype_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Subtype_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_Save", OperationType = GEMOperationType.Save)]
		public override Subtype_PK Save(Subtype_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Subtype_PK> SaveCollection(List<Subtype_PK> entities)
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
