// ======================================================================================================================
// Author:		space-monkey\dpetek
// Create date:	15.5.2012. 11:16:09
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AD_DOMAIN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Ad_domain_PKDAL : GEMDataAccess<Ad_domain_PK>, IAd_domain_PKOperations
	{
		public Ad_domain_PKDAL() : base() { }
		public Ad_domain_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAd_domain_PKOperations Members



		#endregion

		#region ICRUDOperations<Ad_domain_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AD_DOMAIN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Ad_domain_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AD_DOMAIN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Ad_domain_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AD_DOMAIN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Ad_domain_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AD_DOMAIN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Ad_domain_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AD_DOMAIN_Save", OperationType = GEMOperationType.Save)]
		public override Ad_domain_PK Save(Ad_domain_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AD_DOMAIN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Ad_domain_PK> SaveCollection(List<Ad_domain_PK> entities)
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
