// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:49:00
// Description:	GEM2 Generated class for table SSI.dbo.PROPERTY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "PROPERTY")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Property_PK
	{
		private Int32? _property_PK;
		private String _property_type;
		private String _property_name;
		private String _substance_id;
		private String _substance_name;
		private String _amount_type;
		private Int32? _amount_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? property_PK
		{
			get { return _property_PK; }
			set { _property_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String property_type
		{
			get { return _property_type; }
			set { _property_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String property_name
		{
			get { return _property_name; }
			set { _property_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substance_id
		{
			get { return _substance_id; }
			set { _substance_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substance_name
		{
			get { return _substance_name; }
			set { _substance_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String amount_type
		{
			get { return _amount_type; }
			set { _amount_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? amount_FK
		{
			get { return _amount_FK; }
			set { _amount_FK = value; }
		}

		#endregion

		public Property_PK() { }
		public Property_PK(Int32? property_PK, String property_type, String property_name, String substance_id, String substance_name, String amount_type, Int32? amount_FK)
		{
			this.property_PK = property_PK;
			this.property_type = property_type;
			this.property_name = property_name;
			this.substance_id = substance_id;
			this.substance_name = substance_name;
			this.amount_type = amount_type;
			this.amount_FK = amount_FK;
		}
	}

	public interface IProperty_PKOperations : ICRUDOperations<Property_PK>
	{
        List<Property_PK> GetPropertyByNonStoPK(Int32? NonStoPK);

	}
}
