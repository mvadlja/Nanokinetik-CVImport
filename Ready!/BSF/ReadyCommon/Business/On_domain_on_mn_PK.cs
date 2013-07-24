// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:36:34
// Description:	GEM2 Generated class for table SSI.dbo.ON_DOMAIN_ON_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "ON_DOMAIN_ON_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class On_domain_on_mn_PK
	{
		private Int32? _on_domain_on_mn_PK;
		private Int32? _on_domain_FK;
		private Int32? _on_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? on_domain_on_mn_PK
		{
			get { return _on_domain_on_mn_PK; }
			set { _on_domain_on_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? on_domain_FK
		{
			get { return _on_domain_FK; }
			set { _on_domain_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? on_FK
		{
			get { return _on_FK; }
			set { _on_FK = value; }
		}

		#endregion

		public On_domain_on_mn_PK() { }
		public On_domain_on_mn_PK(Int32? on_domain_on_mn_PK, Int32? on_domain_FK, Int32? on_FK)
		{
			this.on_domain_on_mn_PK = on_domain_on_mn_PK;
			this.on_domain_FK = on_domain_FK;
			this.on_FK = on_FK;
		}
	}

	public interface IOn_domain_on_mn_PKOperations : ICRUDOperations<On_domain_on_mn_PK>
	{
        List<On_domain_on_mn_PK> GetEntitiesByONPK(Int32? ONPK);
	}
}
