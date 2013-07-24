// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:44:19
// Description:	GEM2 Generated class for table SSI.dbo.SING_STRUCTURE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Sing_structure_mn_PKDAL : GEMDataAccess<Sing_structure_mn_PK>, ISing_structure_mn_PKOperations
	{
		public Sing_structure_mn_PKDAL() : base() { }
		public Sing_structure_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISing_structure_mn_PKOperations Members



		#endregion

		#region ICRUDOperations<Sing_structure_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_STRUCTURE_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Sing_structure_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_STRUCTURE_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Sing_structure_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_STRUCTURE_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Sing_structure_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_STRUCTURE_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Sing_structure_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_STRUCTURE_MN_Save", OperationType = GEMOperationType.Save)]
		public override Sing_structure_mn_PK Save(Sing_structure_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_STRUCTURE_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Sing_structure_mn_PK> SaveCollection(List<Sing_structure_mn_PK> entities)
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
