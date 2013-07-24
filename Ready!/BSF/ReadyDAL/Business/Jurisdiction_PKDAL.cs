// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:48:18
// Description:	GEM2 Generated class for table SSI.dbo.OFFICIAL_NAME_JURISDICTION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Jurisdiction_PKDAL : GEMDataAccess<Jurisdiction_PK>, IJurisdiction_PKOperations
	{
		public Jurisdiction_PKDAL() : base() { }
		public Jurisdiction_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IJurisdiction_PKOperations Members



		#endregion

		#region ICRUDOperations<Jurisdiction_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_JURISDICTION_GetEntity", OperationType = GEMOperationType.Select)]
		public override Jurisdiction_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_JURISDICTION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Jurisdiction_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_JURISDICTION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Jurisdiction_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_JURISDICTION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Jurisdiction_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_JURISDICTION_Save", OperationType = GEMOperationType.Save)]
		public override Jurisdiction_PK Save(Jurisdiction_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_JURISDICTION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Jurisdiction_PK> SaveCollection(List<Jurisdiction_PK> entities)
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
