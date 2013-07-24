// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:10:03
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_RELATIONSHIP
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SUBSTANCE_RELATIONSHIP")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Substance_relationship_PK
	{
		private Int32? _substance_relationship_PK;
		private String _relationship;
		private String _interaction_type;
		private String _substance_id;
		private String _substance_name;
		private String _amount_type;
		private Int32? _amount_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_relationship_PK
		{
			get { return _substance_relationship_PK; }
			set { _substance_relationship_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String relationship
		{
			get { return _relationship; }
			set { _relationship = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String interaction_type
		{
			get { return _interaction_type; }
			set { _interaction_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substance_id
		{
			get { return _substance_id; }
			set { _substance_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substance_name
		{
			get { return _substance_name; }
			set { _substance_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String amount_type
		{
			get { return _amount_type; }
			set { _amount_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? amount_FK
		{
			get { return _amount_FK; }
			set { _amount_FK = value; }
		}

		#endregion

		public Substance_relationship_PK() { }
		public Substance_relationship_PK(Int32? substance_relationship_PK, String relationship, String interaction_type, String substance_id, String substance_name, String amount_type, Int32? amount_FK)
		{
			this.substance_relationship_PK = substance_relationship_PK;
			this.relationship = relationship;
			this.interaction_type = interaction_type;
			this.substance_id = substance_id;
			this.substance_name = substance_name;
			this.amount_type = amount_type;
			this.amount_FK = amount_FK;
		}
	}

	public interface ISubstance_relationship_PKOperations : ICRUDOperations<Substance_relationship_PK>
	{
        List<Substance_relationship_PK> GetRELByRIPK(Int32? RIPK);
	}
}
