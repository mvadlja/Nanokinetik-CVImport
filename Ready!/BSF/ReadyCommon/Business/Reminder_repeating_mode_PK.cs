// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	10.1.2013. 10:35:34
// Description:	GEM2 Generated class for table ReadyDev.dbo.REMINDER_REPEATING_MODES
// ======================================================================================================================

using System;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "ReadyDev", Table = "REMINDER_REPEATING_MODES")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Reminder_repeating_mode_PK
	{
		private Int32? _reminder_repeating_mode_PK;
		private String _name;
		private String _enum_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? reminder_repeating_mode_PK
		{
			get { return _reminder_repeating_mode_PK; }
			set { _reminder_repeating_mode_PK = value; }
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

		public Reminder_repeating_mode_PK() { }
		public Reminder_repeating_mode_PK(Int32? reminder_repeating_mode_PK, String name, String enum_name)
		{
			this.reminder_repeating_mode_PK = reminder_repeating_mode_PK;
			this.name = name;
			this.enum_name = enum_name;
		}
	}

	public interface IReminder_repeating_mode_PKOperations : ICRUDOperations<Reminder_repeating_mode_PK>
	{

	}
}
