// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:56:22
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_MD_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PP_MD_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Pp_md_mn_PK
	{
		private Int32? _pp_md_mn_PK;
		private Int32? _pp_medical_device_FK;
		private Int32? _pharmaceutical_product_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? pp_md_mn_PK
		{
			get { return _pp_md_mn_PK; }
			set { _pp_md_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? pp_medical_device_FK
		{
			get { return _pp_medical_device_FK; }
			set { _pp_medical_device_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? pharmaceutical_product_FK
		{
			get { return _pharmaceutical_product_FK; }
			set { _pharmaceutical_product_FK = value; }
		}

		#endregion

		public Pp_md_mn_PK() { }
		public Pp_md_mn_PK(Int32? pp_md_mn_PK, Int32? pp_medical_device_FK, Int32? pharmaceutical_product_FK)
		{
			this.pp_md_mn_PK = pp_md_mn_PK;
			this.pp_medical_device_FK = pp_medical_device_FK;
			this.pharmaceutical_product_FK = pharmaceutical_product_FK;
		}
	}

	public interface IPp_md_mn_PKOperations : ICRUDOperations<Pp_md_mn_PK>
	{
        List<Pp_md_mn_PK> GetMedDevicesByPPPK(Int32? pharmaceutical_product_FK);
	    void DeleteByPharmaceuticalProduct(int pharmaceuticalProductPk);
	}
}
