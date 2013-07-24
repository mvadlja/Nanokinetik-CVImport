// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:46:20
// Description:	GEM2 Generated class for table SSI.dbo.GENE_ELEMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "GENE_ELEMENT")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Gene_element_PK
	{
		private Int32? _gene_element_PK;
		private String _ge_type;
		private String _ge_id;
		private String _ge_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? gene_element_PK
		{
			get { return _gene_element_PK; }
			set { _gene_element_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ge_type
		{
			get { return _ge_type; }
			set { _ge_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ge_id
		{
			get { return _ge_id; }
			set { _ge_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ge_name
		{
			get { return _ge_name; }
			set { _ge_name = value; }
		}

		#endregion

		public Gene_element_PK() { }
		public Gene_element_PK(Int32? gene_element_PK, String ge_type, String ge_id, String ge_name)
		{
			this.gene_element_PK = gene_element_PK;
			this.ge_type = ge_type;
			this.ge_id = ge_id;
			this.ge_name = ge_name;
		}
	}

	public interface IGene_element_PKOperations : ICRUDOperations<Gene_element_PK>
	{
        List<Gene_element_PK> GetGEByRIPK(Int32? RIPK);
	}
}
