// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 16:25:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORG_TYPE_FOR_PARTNER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Org_type_for_partner_PKDAL : GEMDataAccess<Org_type_for_partner_PK>, IOrg_type_for_partner_PKOperations
	{
		public Org_type_for_partner_PKDAL() : base() { }
		public Org_type_for_partner_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IOrg_type_for_partner_PKOperations Members



		#endregion

		#region ICRUDOperations<Org_type_for_partner_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_PARTNER_GetEntity", OperationType = GEMOperationType.Select)]
		public override Org_type_for_partner_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_PARTNER_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Org_type_for_partner_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_PARTNER_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Org_type_for_partner_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_PARTNER_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Org_type_for_partner_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_PARTNER_Save", OperationType = GEMOperationType.Save)]
		public override Org_type_for_partner_PK Save(Org_type_for_partner_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_PARTNER_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Org_type_for_partner_PK> SaveCollection(List<Org_type_for_partner_PK> entities)
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
