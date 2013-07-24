// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:42:52
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.DOMAIN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "DOMAIN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Domain_PK
	{
		private Int32? _domain_PK;
		private String _name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? domain_PK
		{
			get { return _domain_PK; }
			set { _domain_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion

		public Domain_PK() { }
		public Domain_PK(Int32? domain_PK, String name)
		{
			this.domain_PK = domain_PK;
			this.name = name;
		}
	}

	public interface IDomain_PKOperations : ICRUDOperations<Domain_PK>
	{
        List<Domain_PK> GetAvailableEntitiesByProduct(int productPk);
        List<Domain_PK> GetAssignedEntitiesByProduct(int productPk);
	}
}
