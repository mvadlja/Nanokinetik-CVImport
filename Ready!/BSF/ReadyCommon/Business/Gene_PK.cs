// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:46:04
// Description:	GEM2 Generated class for table SSI.dbo.GENE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "GENE")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Gene_PK
	{
		private Int32? _gene_PK;
		private String _gene_sequence_origin;
		private String _gene_id;
		private String _gene_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? gene_PK
		{
			get { return _gene_PK; }
			set { _gene_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String gene_sequence_origin
		{
			get { return _gene_sequence_origin; }
			set { _gene_sequence_origin = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String gene_id
		{
			get { return _gene_id; }
			set { _gene_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String gene_name
		{
			get { return _gene_name; }
			set { _gene_name = value; }
		}

		#endregion

		public Gene_PK() { }
		public Gene_PK(Int32? gene_PK, String gene_sequence_origin, String gene_id, String gene_name)
		{
			this.gene_PK = gene_PK;
			this.gene_sequence_origin = gene_sequence_origin;
			this.gene_id = gene_id;
			this.gene_name = gene_name;
		}
	}

	public interface IGene_PKOperations : ICRUDOperations<Gene_PK>
	{
        List<Gene_PK> GetGeneByRIPK(Int32? RIPK);
	}
}
