// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:45:52
// Description:	GEM2 Generated class for table SSI.dbo.CHEMICAL
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "CHEMICAL")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Chemical_PK
	{
		private Int32? _chemical_PK;
		private Byte[] _stoichiometric;
		private String _comment;
		private Int32? _non_stoichio_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? chemical_PK
		{
			get { return _chemical_PK; }
			set { _chemical_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Binary)]
		public Byte[] stoichiometric
		{
			get { return _stoichiometric; }
			set { _stoichiometric = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? non_stoichio_FK
		{
			get { return _non_stoichio_FK; }
			set { _non_stoichio_FK = value; }
		}

		#endregion

		public Chemical_PK() { }
		public Chemical_PK(Int32? chemical_PK, Byte[] stoichiometric, String comment, Int32? non_stoichio_FK)
		{
			this.chemical_PK = chemical_PK;
			this.stoichiometric = stoichiometric;
			this.comment = comment;
			this.non_stoichio_FK = non_stoichio_FK;
		}
	}

	public interface IChemical_PKOperations : ICRUDOperations<Chemical_PK>
	{

	}
}
