// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:58:25
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ADJUVANT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PP_ADJUVANT_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Pp_adjuvant_mn_PK
	{
		private Int32? _pp_adjuvant_mn_PK;
		private Int32? _pp_adjuvant_FK;
		private Int32? _pharmaceutical_product_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? pp_adjuvant_mn_PK
		{
			get { return _pp_adjuvant_mn_PK; }
			set { _pp_adjuvant_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? pp_adjuvant_FK
		{
			get { return _pp_adjuvant_FK; }
			set { _pp_adjuvant_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? pharmaceutical_product_FK
		{
			get { return _pharmaceutical_product_FK; }
			set { _pharmaceutical_product_FK = value; }
		}

		#endregion

		public Pp_adjuvant_mn_PK() { }
		public Pp_adjuvant_mn_PK(Int32? pp_adjuvant_mn_PK, Int32? pp_adjuvant_FK, Int32? pharmaceutical_product_FK)
		{
			this.pp_adjuvant_mn_PK = pp_adjuvant_mn_PK;
			this.pp_adjuvant_FK = pp_adjuvant_FK;
			this.pharmaceutical_product_FK = pharmaceutical_product_FK;
		}
	}

	public interface IPp_adjuvant_mn_PKOperations : ICRUDOperations<Pp_adjuvant_mn_PK>
	{

	}
}
