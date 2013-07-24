// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	11.1.2012. 11:24:28
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUB_ALIAS_SUB_ALIAS_TRAN_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Sub_alias_sub_alias_tran_PKDAL : GEMDataAccess<Sub_alias_sub_alias_tran_PK>, ISub_alias_sub_alias_tran_PKOperations
	{
		public Sub_alias_sub_alias_tran_PKDAL() : base() { }
		public Sub_alias_sub_alias_tran_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISub_alias_sub_alias_tran_PKOperations Members
       

		#endregion

		#region ICRUDOperations<Sub_alias_sub_alias_tran_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Sub_alias_sub_alias_tran_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Sub_alias_sub_alias_tran_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Sub_alias_sub_alias_tran_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Sub_alias_sub_alias_tran_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_Save", OperationType = GEMOperationType.Save)]
		public override Sub_alias_sub_alias_tran_PK Save(Sub_alias_sub_alias_tran_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Sub_alias_sub_alias_tran_PK> SaveCollection(List<Sub_alias_sub_alias_tran_PK> entities)
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
