// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	28.10.2011. 12:44:12
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_MEDICAL_DEVICE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PP_MEDICAL_DEVICE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Medicaldevice_PK
	{
		private Int32? _medicaldevice_PK;
		private String _medicaldevicecode;
        private String _ev_code;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? medicaldevice_PK
		{
			get { return _medicaldevice_PK; }
			set { _medicaldevice_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public String medicaldevicecode
		{
			get { return _medicaldevicecode; }
			set { _medicaldevicecode = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public String ev_code
        {
            get { return _ev_code; }
            set { _ev_code = value; }
        }

		#endregion

		public Medicaldevice_PK() { }
        public Medicaldevice_PK(Int32? medicaldevice_PK, String medicaldevicecode, String ev_code)
		{
			this.medicaldevice_PK = medicaldevice_PK;
			this.medicaldevicecode = medicaldevicecode;
            this.ev_code = ev_code;
		}
	}

	public interface IMedicaldevice_PKOperations : ICRUDOperations<Medicaldevice_PK>
	{
        DataSet GetMedDevicesByCodeSearcher(String code, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Medicaldevice_PK> GetEntitiesByPharmaceuticalProduct(int pharmaceuticalProductPk);
	}
}
