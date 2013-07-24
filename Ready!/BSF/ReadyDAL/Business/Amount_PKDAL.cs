// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	30.11.2011. 11:31:58
// Description:	GEM2 Generated class for table SSI.dbo.AMOUNT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Amount_PKDAL : GEMDataAccess<Amount_PK>, IAmount_PKOperations
	{
		public Amount_PKDAL() : base() { }
		public Amount_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAmount_PKOperations Members



		#endregion

		#region ICRUDOperations<Amount_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AMOUNT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Amount_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AMOUNT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Amount_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AMOUNT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Amount_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AMOUNT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Amount_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AMOUNT_Save", OperationType = GEMOperationType.Save)]
		public override Amount_PK Save(Amount_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AMOUNT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Amount_PK> SaveCollection(List<Amount_PK> entities)
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
