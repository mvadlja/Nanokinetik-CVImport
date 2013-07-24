// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	5.9.2012. 14:33:35
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_MESSAGE_HEADER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "MA_MESSAGE_HEADER")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Ma_message_header_PK
    {
        private Int32? _ma_message_header_PK;
        private String _messageformatversion;
        private String _messageformatrelease;
        private String _registrationnumber;
        private Int64? _registrationid;
        private String _readymessageid;
        private String _messagedateformat;
        private DateTime? _messagedate;
        private String _ready_id_FK;
        private String _message_file_name;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? ma_message_header_PK
        {
            get { return _ma_message_header_PK; }
            set { _ma_message_header_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String messageformatversion
        {
            get { return _messageformatversion; }
            set { _messageformatversion = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String messageformatrelease
        {
            get { return _messageformatrelease; }
            set { _messageformatrelease = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String registrationnumber
        {
            get { return _registrationnumber; }
            set { _registrationnumber = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int64)]
        public Int64? registrationid
        {
            get { return _registrationid; }
            set { _registrationid = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String readymessageid
        {
            get { return _readymessageid; }
            set { _readymessageid = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String messagedateformat
        {
            get { return _messagedateformat; }
            set { _messagedateformat = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? messagedate
        {
            get { return _messagedate; }
            set { _messagedate = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ready_id_FK
        {
            get { return _ready_id_FK; }
            set { _ready_id_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String message_file_name
        {
            get { return _message_file_name; }
            set { _message_file_name = value; }
        }

        #endregion

        public Ma_message_header_PK() { }
        public Ma_message_header_PK(Int32? ma_message_header_PK, String messageformatversion, String messageformatrelease, String registrationnumber, Int64? registrationid, String readymessageid, String messagedateformat, DateTime? messagedate, String ready_id_FK, String message_file_name)
        {
            this.ma_message_header_PK = ma_message_header_PK;
            this.messageformatversion = messageformatversion;
            this.messageformatrelease = messageformatrelease;
            this.registrationnumber = registrationnumber;
            this.registrationid = registrationid;
            this.readymessageid = readymessageid;
            this.messagedateformat = messagedateformat;
            this.messagedate = messagedate;
            this.ready_id_FK = ready_id_FK;
            this.message_file_name = message_file_name;
        }
    }


    public interface IMa_message_header_PKOperations : ICRUDOperations<Ma_message_header_PK>
    {
        Ma_message_header_PK GetEntityByReadyId(String readyId);
    }
}
