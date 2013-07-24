// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	14.9.2012. 10:30:25
// Description:	GEM2 Generated class for table ready_dev.dbo.EMA_RECEIVED_FILE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "EMA_RECEIVED_FILE")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Ema_received_file_PK
    {
        private Int32? _ema_received_file_PK;
        private String _file_type;
        private Byte[] _file_data;
        private String _xevprm_path;
        private String _data_path;
        private Int32? _status;
        private DateTime? _received_time;
        private DateTime? _processed_time;
        private String _as2_from;
        private String _as2_to;
        private String _as2_header;
        private String _mdn_orig_msg_number;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? ema_received_file_PK
        {
            get { return _ema_received_file_PK; }
            set { _ema_received_file_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String file_type
        {
            get { return _file_type; }
            set { _file_type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Binary)]
        public Byte[] file_data
        {
            get { return _file_data; }
            set { _file_data = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String xevprm_path
        {
            get { return _xevprm_path; }
            set { _xevprm_path = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String data_path
        {
            get { return _data_path; }
            set { _data_path = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? status
        {
            get { return _status; }
            set { _status = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? received_time
        {
            get { return _received_time; }
            set { _received_time = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? processed_time
        {
            get { return _processed_time; }
            set { _processed_time = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String as2_from
        {
            get { return _as2_from; }
            set { _as2_from = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String as2_to
        {
            get { return _as2_to; }
            set { _as2_to = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String as2_header
        {
            get { return _as2_header; }
            set { _as2_header = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String mdn_orig_msg_number
        {
            get { return _mdn_orig_msg_number; }
            set { _mdn_orig_msg_number = value; }
        }

        #endregion

        public Ema_received_file_PK() { }
        public Ema_received_file_PK(Int32? ema_received_file_PK, String file_type, Byte[] file_data, String xevprm_path, String data_path, Int32? status, DateTime? received_time, DateTime? processed_time, String as2_from, String as2_to, String as2_header, String mdn_orig_msg_number)
        {
            this.ema_received_file_PK = ema_received_file_PK;
            this.file_type = file_type;
            this.file_data = file_data;
            this.xevprm_path = xevprm_path;
            this.data_path = data_path;
            this.status = status;
            this.received_time = received_time;
            this.processed_time = processed_time;
            this.as2_from = as2_from;
            this.as2_to = as2_to;
            this.as2_header = as2_header;
            this.mdn_orig_msg_number = mdn_orig_msg_number;
        }
    }

    public interface IEma_received_file_PKOperations : ICRUDOperations<Ema_received_file_PK>
    {

        List<Ema_received_file_PK> GetEntitiesByTypeAndStatus(String type, int? status);
    }
}
