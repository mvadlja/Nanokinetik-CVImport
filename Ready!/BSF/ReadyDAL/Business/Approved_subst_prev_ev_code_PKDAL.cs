// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.APPROVED_SUBST_AS_PREV_EV_CODE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Approved_subst_prev_ev_code_PKDAL : GEMDataAccess<Approved_subst_prev_ev_code_PK>, IApproved_subst_prev_ev_code_PKOperations
	{
		public Approved_subst_prev_ev_code_PKDAL() : base() { }
		public Approved_subst_prev_ev_code_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IApproved_subst_prev_ev_code_PKOperations Members



		#endregion

		#region ICRUDOperations<Approved_subst_prev_ev_code_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Approved_subst_prev_ev_code_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Approved_subst_prev_ev_code_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Approved_subst_prev_ev_code_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Approved_subst_prev_ev_code_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_Save", OperationType = GEMOperationType.Save)]
		public override Approved_subst_prev_ev_code_PK Save(Approved_subst_prev_ev_code_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Approved_subst_prev_ev_code_PK> SaveCollection(List<Approved_subst_prev_ev_code_PK> entities)
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
