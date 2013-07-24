// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AS_PREVIOUS_EV_CODE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "AS_PREVIOUS_EV_CODE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class As_previous_ev_code_PK
	{
		private Int32? _as_previous_ev_code_PK;
		private String _devevcode;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? as_previous_ev_code_PK
		{
			get { return _as_previous_ev_code_PK; }
			set { _as_previous_ev_code_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String devevcode
		{
			get { return _devevcode; }
			set { _devevcode = value; }
		}

		#endregion

		public As_previous_ev_code_PK() { }
		public As_previous_ev_code_PK(Int32? as_previous_ev_code_PK, String devevcode)
		{
			this.as_previous_ev_code_PK = as_previous_ev_code_PK;
			this.devevcode = devevcode;
		}
	}

	public interface IAs_previous_ev_code_PKOperations : ICRUDOperations<As_previous_ev_code_PK>
	{

        List<As_previous_ev_code_PK> GetPrevEvcodeByAs(int? as_PK);
    }
}
