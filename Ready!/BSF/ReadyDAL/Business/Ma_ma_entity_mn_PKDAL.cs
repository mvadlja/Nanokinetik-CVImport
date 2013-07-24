// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	6.9.2012. 10:14:18
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_MA_ENTITY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Ma_ma_entity_mn_PKDAL : GEMDataAccess<Ma_ma_entity_mn_PK>, IMa_ma_entity_mn_PKOperations
	{
		public Ma_ma_entity_mn_PKDAL() : base() { }
		public Ma_ma_entity_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IMa_ma_entity_mn_PKOperations Members



		#endregion

		#region ICRUDOperations<Ma_ma_entity_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MA_ENTITY_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Ma_ma_entity_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MA_ENTITY_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Ma_ma_entity_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MA_ENTITY_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Ma_ma_entity_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MA_ENTITY_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Ma_ma_entity_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MA_ENTITY_MN_Save", OperationType = GEMOperationType.Save)]
		public override Ma_ma_entity_mn_PK Save(Ma_ma_entity_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MA_ENTITY_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Ma_ma_entity_mn_PK> SaveCollection(List<Ma_ma_entity_mn_PK> entities)
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
