// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	6.12.2011. 15:46:50
// Description:	GEM2 Generated class for table SSI.dbo.VERSION_SUBSTANCE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Version_substance_mn_PKDAL : GEMDataAccess<Version_substance_mn_PK>, IVersion_substance_mn_PKOperations
	{
		public Version_substance_mn_PKDAL() : base() { }
		public Version_substance_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IVersion_substance_mn_PKOperations Members



		#endregion

		#region ICRUDOperations<Version_substance_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_SUBSTANCE_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Version_substance_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_SUBSTANCE_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Version_substance_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_SUBSTANCE_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Version_substance_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_SUBSTANCE_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Version_substance_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_SUBSTANCE_MN_Save", OperationType = GEMOperationType.Save)]
		public override Version_substance_mn_PK Save(Version_substance_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_VERSION_SUBSTANCE_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Version_substance_mn_PK> SaveCollection(List<Version_substance_mn_PK> entities)
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
