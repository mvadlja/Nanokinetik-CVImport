// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	30.11.2011. 9:17:14
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TIME_UNIT_NAME
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TIME_UNIT_NAME")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Time_unit_name_PK
	{
		private Int32? _time_unit_name_PK;
		private String _time_unit_name;
        private Boolean? _billable;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? time_unit_name_PK
		{
			get { return _time_unit_name_PK; }
			set { _time_unit_name_PK = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String time_unit_name
        {
            get { return _time_unit_name; }
            set { _time_unit_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? billable
        {
            get { return _billable; }
            set { _billable = value; }
        }

		#endregion

		public Time_unit_name_PK() { }
        public Time_unit_name_PK(Int32? time_unit_name_PK, String time_unit_name, Boolean? billable)
		{
			this.time_unit_name_PK = time_unit_name_PK;
			this.time_unit_name = time_unit_name;
            this.billable = billable;
		}
	}

	public interface ITime_unit_name_PKOperations : ICRUDOperations<Time_unit_name_PK>
	{
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
