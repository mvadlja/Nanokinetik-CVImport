// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	9.11.2011. 10:32:08
// Description:	GEM2 Generated class for table SSI.dbo.STRUCT_REPRESENTATION_TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "STRUCT_REPRESENTATION_TYPE")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Struct_repres_type_PK
	{
		private Int32? _struct_repres_type_PK;
		private String _name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? struct_repres_type_PK
		{
			get { return _struct_repres_type_PK; }
			set { _struct_repres_type_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion

		public Struct_repres_type_PK() { }
		public Struct_repres_type_PK(Int32? struct_repres_type_PK, String name)
		{
			this.struct_repres_type_PK = struct_repres_type_PK;
			this.name = name;
		}
	}

	public interface IStruct_repres_type_PKOperations : ICRUDOperations<Struct_repres_type_PK>
	{

	}
}
