// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	6.12.2011. 15:40:28
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_SUBSTANCE_CODE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Substance_substance_code_mn_PKDAL : GEMDataAccess<Substance_substance_code_mn_PK>, ISubstance_substance_code_mn_PKOperations
	{
		public Substance_substance_code_mn_PKDAL() : base() { }
		public Substance_substance_code_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubstance_substance_code_mn_PKOperations Members



		#endregion

		#region ICRUDOperations<Substance_substance_code_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_SUBSTANCE_CODE_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Substance_substance_code_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_SUBSTANCE_CODE_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Substance_substance_code_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_SUBSTANCE_CODE_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Substance_substance_code_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_SUBSTANCE_CODE_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Substance_substance_code_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_SUBSTANCE_CODE_MN_Save", OperationType = GEMOperationType.Save)]
		public override Substance_substance_code_mn_PK Save(Substance_substance_code_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_SUBSTANCE_CODE_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Substance_substance_code_mn_PK> SaveCollection(List<Substance_substance_code_mn_PK> entities)
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
