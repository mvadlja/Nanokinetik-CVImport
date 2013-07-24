// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	10.1.2013. 10:37:15
// Description:	GEM2 Generated class for table ReadyDev.dbo.REMINDER_STATUS
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "REMINDER_STATUS")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Reminder_status_PK
	{
		private Int32? _reminder_status_PK;
		private String _name;
		private String _enum_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? reminder_status_PK
		{
			get { return _reminder_status_PK; }
			set { _reminder_status_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String enum_name
		{
			get { return _enum_name; }
			set { _enum_name = value; }
		}

		#endregion

		public Reminder_status_PK() { }
		public Reminder_status_PK(Int32? reminder_status_PK, String name, String enum_name)
		{
			this.reminder_status_PK = reminder_status_PK;
			this.name = name;
			this.enum_name = enum_name;
		}
	}

	public interface IReminder_status_PKOperations : ICRUDOperations<Reminder_status_PK>
	{

	}
}
