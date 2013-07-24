// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	5.1.2012. 14:05:44
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.APPROVED_SUBST_SUBST_ALIAS_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Approved_substance_subst_alias_PKDAL : GEMDataAccess<Approved_substance_subst_alias_PK>, IApproved_substance_subst_alias_PKOperations
	{
		public Approved_substance_subst_alias_PKDAL() : base() { }
		public Approved_substance_subst_alias_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IApproved_substance_subst_alias_PKOperations Members



		#endregion

		#region ICRUDOperations<Approved_substance_subst_alias_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_SUBST_ALIAS_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Approved_substance_subst_alias_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_SUBST_ALIAS_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Approved_substance_subst_alias_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_SUBST_ALIAS_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Approved_substance_subst_alias_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_SUBST_ALIAS_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Approved_substance_subst_alias_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_SUBST_ALIAS_MN_Save", OperationType = GEMOperationType.Save)]
		public override Approved_substance_subst_alias_PK Save(Approved_substance_subst_alias_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_SUBST_ALIAS_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Approved_substance_subst_alias_PK> SaveCollection(List<Approved_substance_subst_alias_PK> entities)
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
