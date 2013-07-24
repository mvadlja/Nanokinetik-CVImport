// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:45:52
// Description:	GEM2 Generated class for table SSI.dbo.CHEMICAL
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Chemical_PKDAL : GEMDataAccess<Chemical_PK>, IChemical_PKOperations
	{
		public Chemical_PKDAL() : base() { }
		public Chemical_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IChemical_PKOperations Members



		#endregion

		#region ICRUDOperations<Chemical_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CHEMICAL_GetEntity", OperationType = GEMOperationType.Select)]
		public override Chemical_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CHEMICAL_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Chemical_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CHEMICAL_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Chemical_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CHEMICAL_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Chemical_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CHEMICAL_Save", OperationType = GEMOperationType.Save)]
		public override Chemical_PK Save(Chemical_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CHEMICAL_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Chemical_PK> SaveCollection(List<Chemical_PK> entities)
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
