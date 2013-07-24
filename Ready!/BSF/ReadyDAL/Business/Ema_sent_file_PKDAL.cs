// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	14.9.2012. 10:33:59
// Description:	GEM2 Generated class for table ready_dev.dbo.EMA_SENT_FILE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Ema_sent_file_PKDAL : GEMDataAccess<Ema_sent_file_PK>, IEma_sent_file_PKOperations
	{
		public Ema_sent_file_PKDAL() : base() { }
		public Ema_sent_file_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IEma_sent_file_PKOperations Members



		#endregion

		#region ICRUDOperations<Ema_sent_file_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_SENT_FILE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Ema_sent_file_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_SENT_FILE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Ema_sent_file_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_SENT_FILE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Ema_sent_file_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_SENT_FILE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Ema_sent_file_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_SENT_FILE_Save", OperationType = GEMOperationType.Save)]
		public override Ema_sent_file_PK Save(Ema_sent_file_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_SENT_FILE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Ema_sent_file_PK> SaveCollection(List<Ema_sent_file_PK> entities)
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
