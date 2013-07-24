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
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "INTENSE_MONITORING")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Intense_monitoring_PK
	{
		private Int32? _intense_monitoring_PK;
		private Int32? _name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? intense_monitoring_PK
		{
			get { return _intense_monitoring_PK; }
			set { _intense_monitoring_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion

		public Intense_monitoring_PK() { }
		public Intense_monitoring_PK(Int32? intense_monitoring_PK, Int32? name)
		{
			this.intense_monitoring_PK = intense_monitoring_PK;
			this.name = name;
		}
	}

	public interface IIntense_monitoring_PKOperations : ICRUDOperations<Intense_monitoring_PK>
	{

	}
}
