// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:09:06
// Description:	GEM2 Generated class for table SSI.dbo.STRUCTURE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "STRUCTURE")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Structure_PK
	{
		private Int32? _structure_PK;
		private Int32? _struct_repres_type_FK;
		private String _struct_representation;
		private Int32? _struct_repres_attach_FK;
        private Int32? _stereochemistry_FK;
		private String _optical_activity;
		private String _molecular_formula;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? structure_PK
		{
			get { return _structure_PK; }
			set { _structure_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? struct_repres_type_FK
		{
			get { return _struct_repres_type_FK; }
			set { _struct_repres_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String struct_representation
		{
			get { return _struct_representation; }
			set { _struct_representation = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? struct_repres_attach_FK
		{
			get { return _struct_repres_attach_FK; }
			set { _struct_repres_attach_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? stereochemistry_FK
		{
			get { return _stereochemistry_FK; }
			set { _stereochemistry_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String optical_activity
		{
			get { return _optical_activity; }
			set { _optical_activity = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String molecular_formula
		{
			get { return _molecular_formula; }
			set { _molecular_formula = value; }
		}

		#endregion

		public Structure_PK() { }
        public Structure_PK(Int32? structure_PK, Int32? struct_repres_type_FK, String struct_representation, Int32? struct_repres_attach_FK, Int32? stereochemistry_FK, String optical_activity, String molecular_formula)
		{
			this.structure_PK = structure_PK;
			this.struct_repres_type_FK = struct_repres_type_FK;
			this.struct_representation = struct_representation;
			this.struct_repres_attach_FK = struct_repres_attach_FK;
			this.stereochemistry_FK = stereochemistry_FK;
			this.optical_activity = optical_activity;
			this.molecular_formula = molecular_formula;
		}
	}

	public interface IStructure_PKOperations : ICRUDOperations<Structure_PK>
	{
        List<Structure_PK> GetStructBySingPK(Int32? SingPK);

	}
}
