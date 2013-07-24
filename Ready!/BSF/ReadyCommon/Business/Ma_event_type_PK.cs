// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	11.9.2012. 10:00:59
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_EVENT_TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "MA_EVENT_TYPE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Ma_event_type_PK
	{
		private Int32? _ma_event_type_PK;
		private String _name;
		private String _enum_name;
		private Int32? _ma_event_type_severity_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ma_event_type_PK
		{
			get { return _ma_event_type_PK; }
			set { _ma_event_type_PK = value; }
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

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ma_event_type_severity_FK
		{
			get { return _ma_event_type_severity_FK; }
			set { _ma_event_type_severity_FK = value; }
		}

		#endregion

		public Ma_event_type_PK() { }
		public Ma_event_type_PK(Int32? ma_event_type_PK, String name, String enum_name, Int32? ma_event_type_severity_FK)
		{
			this.ma_event_type_PK = ma_event_type_PK;
			this.name = name;
			this.enum_name = enum_name;
			this.ma_event_type_severity_FK = ma_event_type_severity_FK;
		}
	}

	public interface IMa_event_type_PKOperations : ICRUDOperations<Ma_event_type_PK>
	{

	}
}
