// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	29.10.2011. 10:22:30
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.INTENSE_MONITORING
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Intense_monitoring_PKDAL : GEMDataAccess<Intense_monitoring_PK>, IIntense_monitoring_PKOperations
	{
		public Intense_monitoring_PKDAL() : base() { }
		public Intense_monitoring_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IIntense_monitoring_PKOperations Members



		#endregion

		#region ICRUDOperations<Intense_monitoring_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_INTENSE_MONITORING_GetEntity", OperationType = GEMOperationType.Select)]
		public override Intense_monitoring_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_INTENSE_MONITORING_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Intense_monitoring_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_INTENSE_MONITORING_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Intense_monitoring_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_INTENSE_MONITORING_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Intense_monitoring_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_INTENSE_MONITORING_Save", OperationType = GEMOperationType.Save)]
		public override Intense_monitoring_PK Save(Intense_monitoring_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_INTENSE_MONITORING_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Intense_monitoring_PK> SaveCollection(List<Intense_monitoring_PK> entities)
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
