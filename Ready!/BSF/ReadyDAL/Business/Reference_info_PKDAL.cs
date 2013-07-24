// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:49:12
// Description:	GEM2 Generated class for table SSI.dbo.REFERENCE_INFORMATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Reference_info_PKDAL : GEMDataAccess<Reference_info_PK>, IReference_info_PKOperations
	{
		public Reference_info_PKDAL() : base() { }
		public Reference_info_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IReference_info_PKOperations Members



		#endregion

		#region ICRUDOperations<Reference_info_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_INFORMATION_GetEntity", OperationType = GEMOperationType.Select)]
		public override Reference_info_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_INFORMATION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Reference_info_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_INFORMATION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Reference_info_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_INFORMATION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Reference_info_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_INFORMATION_Save", OperationType = GEMOperationType.Save)]
		public override Reference_info_PK Save(Reference_info_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_INFORMATION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Reference_info_PK> SaveCollection(List<Reference_info_PK> entities)
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
