// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:47:15
// Description:	GEM2 Generated class for table SSI.dbo.NON_STOICHIOMETRIC
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Non_stoichiometric_PKDAL : GEMDataAccess<Non_stoichiometric_PK>, INon_stoichiometric_PKOperations
	{
		public Non_stoichiometric_PKDAL() : base() { }
		public Non_stoichiometric_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region INon_stoichiometric_PKOperations Members



		#endregion

		#region ICRUDOperations<Non_stoichiometric_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NON_STOICHIOMETRIC_GetEntity", OperationType = GEMOperationType.Select)]
		public override Non_stoichiometric_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NON_STOICHIOMETRIC_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Non_stoichiometric_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NON_STOICHIOMETRIC_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Non_stoichiometric_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NON_STOICHIOMETRIC_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Non_stoichiometric_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NON_STOICHIOMETRIC_Save", OperationType = GEMOperationType.Save)]
		public override Non_stoichiometric_PK Save(Non_stoichiometric_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NON_STOICHIOMETRIC_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Non_stoichiometric_PK> SaveCollection(List<Non_stoichiometric_PK> entities)
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
