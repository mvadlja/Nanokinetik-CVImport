// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	9.11.2011. 10:31:47
// Description:	GEM2 Generated class for table SSI.dbo.STEREOCHEMISTRY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "STEREOCHEMISTRY")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Stereochemistry_PK
	{
		private Int32? _stereochemistry_PK;
		private String _name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? stereochemistry_PK
		{
			get { return _stereochemistry_PK; }
			set { _stereochemistry_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion

		public Stereochemistry_PK() { }
		public Stereochemistry_PK(Int32? stereochemistry_PK, String name)
		{
			this.stereochemistry_PK = stereochemistry_PK;
			this.name = name;
		}
	}

	public interface IStereochemistry_PKOperations : ICRUDOperations<Stereochemistry_PK>
	{

	}
}
