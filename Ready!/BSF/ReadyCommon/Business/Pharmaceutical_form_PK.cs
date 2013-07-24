// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 12:08:56
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PHARMACEUTICAL_FORM
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PHARMACEUTICAL_FORM")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Pharmaceutical_form_PK
	{
		private Int32? _pharmaceutical_form_PK;
		private String _name;
		private String _ev_code;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? pharmaceutical_form_PK
		{
			get { return _pharmaceutical_form_PK; }
			set { _pharmaceutical_form_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ev_code
		{
			get { return _ev_code; }
			set { _ev_code = value; }
		}

		#endregion

		public Pharmaceutical_form_PK() { }
        public Pharmaceutical_form_PK(Int32? pharmaceutical_form_PK, String name, String ev_code)
        {
            this.pharmaceutical_form_PK = pharmaceutical_form_PK;
            this.name = name;
            this.ev_code = ev_code;
        }
	}

	public interface IPharmaceutical_form_PKOperations : ICRUDOperations<Pharmaceutical_form_PK>
	{

	}
}
