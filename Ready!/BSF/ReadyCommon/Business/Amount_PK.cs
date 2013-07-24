// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:55:44
// Description:	GEM2 Generated class for table SSI.dbo.AMOUNT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "AMOUNT")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Amount_PK
	{
		private Int32? _amount_PK;
		private String _quantity;
		private String _lownumvalue;
		private String _lownumunit;
		private String _lownumprefix;
		private String _lowdenomvalue;
		private String _lowdenomunit;
		private String _lowdenomprefix;
		private String _highnumvalue;
		private String _highnumunit;
		private String _highnumprefix;
		private String _highdenomvalue;
		private String _highdenomunit;
		private String _highdenomprefix;
		private String _average;
		private String _prefix;
		private String _unit;
		private String _nonnumericvalue;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? amount_PK
		{
			get { return _amount_PK; }
			set { _amount_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String lownumvalue
		{
			get { return _lownumvalue; }
			set { _lownumvalue = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String lownumunit
		{
			get { return _lownumunit; }
			set { _lownumunit = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String lownumprefix
		{
			get { return _lownumprefix; }
			set { _lownumprefix = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String lowdenomvalue
		{
			get { return _lowdenomvalue; }
			set { _lowdenomvalue = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String lowdenomunit
		{
			get { return _lowdenomunit; }
			set { _lowdenomunit = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String lowdenomprefix
		{
			get { return _lowdenomprefix; }
			set { _lowdenomprefix = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String highnumvalue
		{
			get { return _highnumvalue; }
			set { _highnumvalue = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String highnumunit
		{
			get { return _highnumunit; }
			set { _highnumunit = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String highnumprefix
		{
			get { return _highnumprefix; }
			set { _highnumprefix = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String highdenomvalue
		{
			get { return _highdenomvalue; }
			set { _highdenomvalue = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String highdenomunit
		{
			get { return _highdenomunit; }
			set { _highdenomunit = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String highdenomprefix
		{
			get { return _highdenomprefix; }
			set { _highdenomprefix = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String average
		{
			get { return _average; }
			set { _average = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String prefix
		{
			get { return _prefix; }
			set { _prefix = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String unit
		{
			get { return _unit; }
			set { _unit = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String nonnumericvalue
		{
			get { return _nonnumericvalue; }
			set { _nonnumericvalue = value; }
		}

		#endregion

		public Amount_PK() { }
		public Amount_PK(Int32? amount_PK, String quantity, String lownumvalue, String lownumunit, String lownumprefix, String lowdenomvalue, String lowdenomunit, String lowdenomprefix, String highnumvalue, String highnumunit, String highnumprefix, String highdenomvalue, String highdenomunit, String highdenomprefix, String average, String prefix, String unit, String nonnumericvalue)
		{
			this.amount_PK = amount_PK;
			this.quantity = quantity;
			this.lownumvalue = lownumvalue;
			this.lownumunit = lownumunit;
			this.lownumprefix = lownumprefix;
			this.lowdenomvalue = lowdenomvalue;
			this.lowdenomunit = lowdenomunit;
			this.lowdenomprefix = lowdenomprefix;
			this.highnumvalue = highnumvalue;
			this.highnumunit = highnumunit;
			this.highnumprefix = highnumprefix;
			this.highdenomvalue = highdenomvalue;
			this.highdenomunit = highdenomunit;
			this.highdenomprefix = highdenomprefix;
			this.average = average;
			this.prefix = prefix;
			this.unit = unit;
			this.nonnumericvalue = nonnumericvalue;
		}
	}

	public interface IAmount_PKOperations : ICRUDOperations<Amount_PK>
	{

	}
}
