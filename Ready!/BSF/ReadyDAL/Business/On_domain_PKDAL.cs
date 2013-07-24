// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 15:09:13
// Description:	GEM2 Generated class for table SSI.dbo.OFFICIAL_NAME_DOMAIN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class On_domain_PKDAL : GEMDataAccess<On_domain_PK>, IOn_domain_PKOperations
	{
		public On_domain_PKDAL() : base() { }
		public On_domain_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IOn_domain_PKOperations Members



		#endregion

		#region ICRUDOperations<On_domain_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_DOMAIN_GetEntity", OperationType = GEMOperationType.Select)]
		public override On_domain_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_DOMAIN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<On_domain_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_DOMAIN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<On_domain_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_DOMAIN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<On_domain_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_DOMAIN_Save", OperationType = GEMOperationType.Save)]
		public override On_domain_PK Save(On_domain_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_OFFICIAL_NAME_DOMAIN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<On_domain_PK> SaveCollection(List<On_domain_PK> entities)
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
