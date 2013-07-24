// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	6.9.2012. 17:11:51
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_ATTACHMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "MA_ATTACHMENT")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Ma_attachment_PK
	{
        private Int32? _ma_attachment_PK;
        private String _file_name;
        private String _file_path;
        private Byte[] _file_data;
        private DateTime? _last_change;
        private Boolean? _deleted;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? ma_attachment_PK
        {
            get { return _ma_attachment_PK; }
            set { _ma_attachment_PK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? last_change
        {
            get { return _last_change; }
            set { _last_change = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        #endregion

        public Ma_attachment_PK() { }
        public Ma_attachment_PK(Int32? ma_attachment_PK, String file_name, String file_path, Byte[] file_data, DateTime? last_change, Boolean? deleted)
        {
            this.ma_attachment_PK = ma_attachment_PK;
            this.file_name = file_name;
            this.file_path = file_path;
            this.file_data = file_data;
            this.last_change = last_change;
            this.deleted = deleted;
        }
    }

	public interface IMa_attachment_PKOperations : ICRUDOperations<Ma_attachment_PK>
	{
        Dictionary<String,DateTime> GetNameAndDateForEntities();
        Ma_attachment_PK GetEntityByFileName(String fileName);
        int? GetAttachmentPK(String fileName);
	}
}
