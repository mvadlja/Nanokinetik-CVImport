// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:21:00
// Description:	GEM2 Generated class for table SSI.dbo.ISOTOPE_STRUCTURE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "ISOTOPE_STRUCTURE_MN")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Isotope_structure_mn_PK
	{
		private Int32? _isotope_structure_mn_PK;
		private Int32? _isotope_FK;
		private Int32? _structure_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? isotope_structure_mn_PK
		{
			get { return _isotope_structure_mn_PK; }
			set { _isotope_structure_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? isotope_FK
		{
			get { return _isotope_FK; }
			set { _isotope_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? structure_FK
		{
			get { return _structure_FK; }
			set { _structure_FK = value; }
		}

		#endregion

		public Isotope_structure_mn_PK() { }
		public Isotope_structure_mn_PK(Int32? isotope_structure_mn_PK, Int32? isotope_FK, Int32? structure_FK)
		{
			this.isotope_structure_mn_PK = isotope_structure_mn_PK;
			this.isotope_FK = isotope_FK;
			this.structure_FK = structure_FK;
		}
	}

	public interface IIsotope_structure_mn_PKOperations : ICRUDOperations<Isotope_structure_mn_PK>
	{

	}
}
