// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:46:33
// Description:	GEM2 Generated class for table SSI.dbo.ISOTOPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "ISOTOPE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Isotope_PK
	{
		private Int32? _isotope_PK;
		private String _nuclide_id;
		private String _nuclide_name;
		private String _substitution_type;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? isotope_PK
		{
			get { return _isotope_PK; }
			set { _isotope_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String nuclide_id
		{
			get { return _nuclide_id; }
			set { _nuclide_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String nuclide_name
		{
			get { return _nuclide_name; }
			set { _nuclide_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substitution_type
		{
			get { return _substitution_type; }
			set { _substitution_type = value; }
		}

		#endregion

		public Isotope_PK() { }
		public Isotope_PK(Int32? isotope_PK, String nuclide_id, String nuclide_name, String substitution_type)
		{
			this.isotope_PK = isotope_PK;
			this.nuclide_id = nuclide_id;
			this.nuclide_name = nuclide_name;
			this.substitution_type = substitution_type;
		}
	}

	public interface IIsotope_PKOperations : ICRUDOperations<Isotope_PK>
	{
        List<Isotope_PK> GetISOByStructPK(Int32? StructurePK);

	}
}
