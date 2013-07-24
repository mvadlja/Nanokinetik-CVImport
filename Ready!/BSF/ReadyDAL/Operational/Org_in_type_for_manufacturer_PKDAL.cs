// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	19.10.2011. 12:00:31
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORG_TYPE_FOR_MANUFACTURER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Org_in_type_for_manufacturer_PKDAL : GEMDataAccess<Org_in_type_for_manufacturer_PK>, IOrg_in_type_for_manufacturer_PKOperations
	{
		public Org_in_type_for_manufacturer_PKDAL() : base() { }
		public Org_in_type_for_manufacturer_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IOrg_in_type_for_manufacturer_PKOperations Members



		#endregion

		#region ICRUDOperations<Org_in_type_for_manufacturer_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_MANUFACTURER_GetEntity", OperationType = GEMOperationType.Select)]
		public override Org_in_type_for_manufacturer_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_MANUFACTURER_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_manufacturer_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_MANUFACTURER_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_manufacturer_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_MANUFACTURER_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_manufacturer_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_MANUFACTURER_Save", OperationType = GEMOperationType.Save)]
		public override Org_in_type_for_manufacturer_PK Save(Org_in_type_for_manufacturer_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_TYPE_FOR_MANUFACTURER_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Org_in_type_for_manufacturer_PK> SaveCollection(List<Org_in_type_for_manufacturer_PK> entities)
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
