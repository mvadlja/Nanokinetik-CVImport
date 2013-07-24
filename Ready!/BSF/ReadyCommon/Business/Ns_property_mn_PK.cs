// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:35:35
// Description:	GEM2 Generated class for table SSI.dbo.NS_PROPERTY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "NS_PROPERTY_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Ns_property_mn_PK
	{
		private Int32? _ns_property_mn_PK;
		private Int32? _ns_FK;
		private Int32? _property_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ns_property_mn_PK
		{
			get { return _ns_property_mn_PK; }
			set { _ns_property_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ns_FK
		{
			get { return _ns_FK; }
			set { _ns_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? property_FK
		{
			get { return _property_FK; }
			set { _property_FK = value; }
		}

		#endregion

		public Ns_property_mn_PK() { }
		public Ns_property_mn_PK(Int32? ns_property_mn_PK, Int32? ns_FK, Int32? property_FK)
		{
			this.ns_property_mn_PK = ns_property_mn_PK;
			this.ns_FK = ns_FK;
			this.property_FK = property_FK;
		}
	}

	public interface INs_property_mn_PKOperations : ICRUDOperations<Ns_property_mn_PK>
	{

	}
}
