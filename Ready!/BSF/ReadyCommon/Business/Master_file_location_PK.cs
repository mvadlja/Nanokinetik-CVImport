// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	19.10.2011. 9:56:43
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.MASTER_FILE_LOCATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "MASTER_FILE_LOCATION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Master_file_location_PK
	{
		private Int32? _master_file_location_PK;
		private String _localnumber;
		private String _ev_code;
		private String _mflcompany;
		private String _mfldepartment;
		private String _mflbuilding;
		private String _mflstreet;
		private String _mflcity;
		private String _mflstate;
		private String _mflpostcode;
		private String _mflcountrycode;
		private String _comments;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? master_file_location_PK
		{
			get { return _master_file_location_PK; }
			set { _master_file_location_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String localnumber
		{
			get { return _localnumber; }
			set { _localnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ev_code
		{
			get { return _ev_code; }
			set { _ev_code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mflcompany
		{
			get { return _mflcompany; }
			set { _mflcompany = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mfldepartment
		{
			get { return _mfldepartment; }
			set { _mfldepartment = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mflbuilding
		{
			get { return _mflbuilding; }
			set { _mflbuilding = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mflstreet
		{
			get { return _mflstreet; }
			set { _mflstreet = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mflcity
		{
			get { return _mflcity; }
			set { _mflcity = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mflstate
		{
			get { return _mflstate; }
			set { _mflstate = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mflpostcode
		{
			get { return _mflpostcode; }
			set { _mflpostcode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mflcountrycode
		{
			get { return _mflcountrycode; }
			set { _mflcountrycode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comments
		{
			get { return _comments; }
			set { _comments = value; }
		}

		#endregion

		public Master_file_location_PK() { }
		public Master_file_location_PK(Int32? master_file_location_PK, String localnumber, String ev_code, String mflcompany, String mfldepartment, String mflbuilding, String mflstreet, String mflcity, String mflstate, String mflpostcode, String mflcountrycode, String comments)
		{
			this.master_file_location_PK = master_file_location_PK;
			this.localnumber = localnumber;
			this.ev_code = ev_code;
			this.mflcompany = mflcompany;
			this.mfldepartment = mfldepartment;
			this.mflbuilding = mflbuilding;
			this.mflstreet = mflstreet;
			this.mflcity = mflcity;
			this.mflstate = mflstate;
			this.mflpostcode = mflpostcode;
			this.mflcountrycode = mflcountrycode;
			this.comments = comments;
		}
	}

	public interface IMaster_file_location_PKOperations : ICRUDOperations<Master_file_location_PK>
	{
        DataSet MFLSearcher(String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
