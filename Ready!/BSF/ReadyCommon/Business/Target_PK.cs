// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:11:05
// Description:	GEM2 Generated class for table SSI.dbo.TARGET
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "TARGET")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Target_PK
	{
		private Int32? _target_PK;
		private String _target_gene_id;
		private String _target_gene_name;
		private String _interaction_type;
		private String _target_organism_type;
		private String _target_type;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? target_PK
		{
			get { return _target_PK; }
			set { _target_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String target_gene_id
		{
			get { return _target_gene_id; }
			set { _target_gene_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String target_gene_name
		{
			get { return _target_gene_name; }
			set { _target_gene_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String interaction_type
		{
			get { return _interaction_type; }
			set { _interaction_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String target_organism_type
		{
			get { return _target_organism_type; }
			set { _target_organism_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String target_type
		{
			get { return _target_type; }
			set { _target_type = value; }
		}

		#endregion

		public Target_PK() { }
		public Target_PK(Int32? target_PK, String target_gene_id, String target_gene_name, String interaction_type, String target_organism_type, String target_type)
		{
			this.target_PK = target_PK;
			this.target_gene_id = target_gene_id;
			this.target_gene_name = target_gene_name;
			this.interaction_type = interaction_type;
			this.target_organism_type = target_organism_type;
			this.target_type = target_type;
		}
	}

	public interface ITarget_PKOperations : ICRUDOperations<Target_PK>
	{
        List<Target_PK> GetTargetByRIPK(Int32? RIPK);
	}
}
