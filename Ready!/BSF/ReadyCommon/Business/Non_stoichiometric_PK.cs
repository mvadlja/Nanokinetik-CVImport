// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:47:15
// Description:	GEM2 Generated class for table SSI.dbo.NON_STOICHIOMETRIC
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "NON_STOICHIOMETRIC")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Non_stoichiometric_PK
	{
		private Int32? _non_stoichiometric_PK;
		private Int32? _number_moieties;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? non_stoichiometric_PK
		{
			get { return _non_stoichiometric_PK; }
			set { _non_stoichiometric_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? number_moieties
		{
			get { return _number_moieties; }
			set { _number_moieties = value; }
		}

		#endregion

		public Non_stoichiometric_PK() { }
		public Non_stoichiometric_PK(Int32? non_stoichiometric_PK, Int32? number_moieties)
		{
			this.non_stoichiometric_PK = non_stoichiometric_PK;
			this.number_moieties = number_moieties;
		}
	}

	public interface INon_stoichiometric_PKOperations : ICRUDOperations<Non_stoichiometric_PK>
	{

	}
}
