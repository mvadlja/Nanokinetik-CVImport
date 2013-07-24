// ======================================================================================================================
// Author:		BRUNO-NOTEBOOK\Bruno
// Create date:	20.2.2011. 9:57:27
// Description:	GEM2 Generated class for table Kmis.dbo.Downtimes
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Kmis.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Kmis", Table = "Downtimes")]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Downtime
	{
		private Int32? _iDDowntime;
		private Int32? _countryID;
		private DateTime? _dateFrom;
		private DateTime? _dateTo;
		private String _comment;
		private String _displayComment;
		private Boolean? _active;
		private String _userShutdowner;
		private DateTime? _rowVersion;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? IDDowntime
		{
			get { return _iDDowntime; }
			set { _iDDowntime = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? CountryID
		{
			get { return _countryID; }
			set { _countryID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? DateFrom
		{
			get { return _dateFrom; }
			set { _dateFrom = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? DateTo
		{
			get { return _dateTo; }
			set { _dateTo = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String Comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String DisplayComment
		{
			get { return _displayComment; }
			set { _displayComment = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? Active
		{
			get { return _active; }
			set { _active = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String UserShutdowner
		{
			get { return _userShutdowner; }
			set { _userShutdowner = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", IsRowVersion = true, ParameterType = DbType.DateTime)]
		public DateTime? RowVersion
		{
			get { return _rowVersion; }
			set { _rowVersion = value; }
		}

		#endregion

		public Downtime() { }
		public Downtime(Int32? iDDowntime, Int32? countryID, DateTime? dateFrom, DateTime? dateTo, String comment, String displayComment, Boolean? active, String userShutdowner, DateTime? rowVersion)
		{
			this.IDDowntime = iDDowntime;
			this.CountryID = countryID;
			this.DateFrom = dateFrom;
			this.DateTo = dateTo;
			this.Comment = comment;
			this.DisplayComment = displayComment;
			this.Active = active;
			this.UserShutdowner = userShutdowner;
			this.RowVersion = rowVersion;
		}
	}

	public interface IDowntimeOperations : ICRUDOperations<Downtime>
	{
        List<Downtime> GetCurrentActiveDowntimesByCountryID(Int32 countryID);
        List<Downtime> GetEntitiesByCountryID(Int32 countryID, int pageNumber, int pageSize, out int totalRecordsCount);
    }
}
