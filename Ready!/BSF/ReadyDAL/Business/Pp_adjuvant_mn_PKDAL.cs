// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:58:25
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ADJUVANT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Pp_adjuvant_mn_PKDAL : GEMDataAccess<Pp_adjuvant_mn_PK>, IPp_adjuvant_mn_PKOperations
	{
		public Pp_adjuvant_mn_PKDAL() : base() { }
		public Pp_adjuvant_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IPp_adjuvant_mn_PKOperations Members



		#endregion

		#region ICRUDOperations<Pp_adjuvant_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Pp_adjuvant_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Pp_adjuvant_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Pp_adjuvant_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Pp_adjuvant_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_MN_Save", OperationType = GEMOperationType.Save)]
		public override Pp_adjuvant_mn_PK Save(Pp_adjuvant_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Pp_adjuvant_mn_PK> SaveCollection(List<Pp_adjuvant_mn_PK> entities)
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
