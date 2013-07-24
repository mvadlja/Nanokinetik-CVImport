// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	10.1.2013. 10:35:34
// Description:	GEM2 Generated class for table ReadyDev.dbo.REMINDER_REPEATING_MODES
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Reminder_repeating_mode_PKDAL : GEMDataAccess<Reminder_repeating_mode_PK>, IReminder_repeating_mode_PKOperations
	{
		public Reminder_repeating_mode_PKDAL() : base() { }
		public Reminder_repeating_mode_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IReminder_repeating_mode_PKOperations Members



		#endregion

		#region ICRUDOperations<Reminder_repeating_mode_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_REPEATING_MODES_GetEntity", OperationType = GEMOperationType.Select)]
		public override Reminder_repeating_mode_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_REPEATING_MODES_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Reminder_repeating_mode_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_REPEATING_MODES_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Reminder_repeating_mode_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_REPEATING_MODES_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Reminder_repeating_mode_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_REPEATING_MODES_Save", OperationType = GEMOperationType.Save)]
		public override Reminder_repeating_mode_PK Save(Reminder_repeating_mode_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_REPEATING_MODES_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Reminder_repeating_mode_PK> SaveCollection(List<Reminder_repeating_mode_PK> entities)
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
