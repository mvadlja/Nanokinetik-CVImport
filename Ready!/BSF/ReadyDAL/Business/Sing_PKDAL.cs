// ======================================================================================================================
// Author:		ANTEC\Kiki
// Create date:	9.1.2012. 3:26:49
// Description:	GEM2 Generated class for table SSI.dbo.SING
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Sing_PKDAL : GEMDataAccess<Sing_PK>, ISing_PKOperations
	{
		public Sing_PKDAL() : base() { }
		public Sing_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISing_PKOperations Members



		#endregion

		#region ICRUDOperations<Sing_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_GetEntity", OperationType = GEMOperationType.Select)]
		public override Sing_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Sing_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Sing_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Sing_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_Save", OperationType = GEMOperationType.Save)]
		public override Sing_PK Save(Sing_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Sing_PK> SaveCollection(List<Sing_PK> entities)
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
