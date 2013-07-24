// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	5.9.2012. 13:03:37
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_FILE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "MA_FILE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Ma_file_PK
	{

        public enum FileType { 
            NULL,
            MAFile,
            MAxEVPRM,
            MAAck,
            MAStatusReport_Received,
            MAStatusReport_ReceivedErrors,
            MAStatusReport_ValidationSuccessful,
            MAStatusReport_ValidationFailed,
            MAStatusReport_SentToEMA,
            MAStatusReport_ACKReceivedFromEMA
        }

        private Int32? _ma_file_PK;
        private Int32? _file_type_FK;
        private String _file_name;
        private String _file_path;
        private Byte[] _file_data;
        private String _ready_id_FK;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? ma_file_PK
        {
            get { return _ma_file_PK; }
            set { _ma_file_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? file_type_FK
        {
            get { return _file_type_FK; }
            set { _file_type_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String file_name
        {
            get { return _file_name; }
            set { _file_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String file_path
        {
            get { return _file_path; }
            set { _file_path = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Binary)]
        public Byte[] file_data
        {
            get { return _file_data; }
            set { _file_data = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ready_id_FK
        {
            get { return _ready_id_FK; }
            set { _ready_id_FK = value; }
        }

        #endregion

        public Ma_file_PK() { }
        public Ma_file_PK(Int32? ma_file_PK, Int32? file_type_FK, String file_name, String file_path, Byte[] file_data, String ready_id_FK)
        {
            this.ma_file_PK = ma_file_PK;
            this.file_type_FK = file_type_FK;
            this.file_name = file_name;
            this.file_path = file_path;
            this.file_data = file_data;
            this.ready_id_FK = ready_id_FK;
        }
    }

    public interface IMa_file_PKOperations : ICRUDOperations<Ma_file_PK>
    {
        Ma_file_PK GetEntityByReadyIdAndType(String readyId, Ma_file_PK.FileType fileType);
        Ma_file_PK GetEntityByFileNameAndType(String fileName, Ma_file_PK.FileType fileType);
        List<Ma_file_PK> GetEntitiesByFileName(String fileName);
    }
}
