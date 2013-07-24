// ======================================================================================================================
// Author:		TomoZ560\Tomo
// Create date:	21.10.2011. 0:03:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ATTACHMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ATTACHMENT")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Attachment_PK
	{
        private Int32? _attachment_PK;
        private Byte[] _disk_file;
        private Int32? _document_FK;
        private String _attachmentname;
        private String _filetype;
        private Int32? _userID;
        private String _pom_type;
        private Guid? _session_id;
        private String _ev_code;
        private String _type_for_fts;
        private DateTime? _modified_date;
        private String _EDMSDocumentId;
        private String _EDMSBindingRule;
        private String _EDMSAttachmentFormat;
	    private int? _lock_owner_FK;
	    private DateTime? _lock_date;
	    private int? _check_in_for_attach_FK;
	    private Guid? _check_in_session_id;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? attachment_PK
        {
            get { return _attachment_PK; }
            set { _attachment_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Binary)]
        public Byte[] disk_file
        {
            get { return _disk_file; }
            set { _disk_file = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? document_FK
        {
            get { return _document_FK; }
            set { _document_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String attachmentname
        {
            get { return _attachmentname; }
            set { _attachmentname = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String filetype
        {
            get { return _filetype; }
            set { _filetype = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? userID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pom_type
        {
            get { return _pom_type; }
            set { _pom_type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Guid)]
        public Guid? session_id
        {
            get { return _session_id; }
            set { _session_id = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ev_code
        {
            get { return _ev_code; }
            set { _ev_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String type_for_fts
        {
            get { return _type_for_fts; }
            set { _type_for_fts = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? modified_date
        {
            get { return _modified_date; }
            set { _modified_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String EDMSDocumentId
        {
            get { return _EDMSDocumentId; }
            set { _EDMSDocumentId = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String EDMSBindingRule
        {
            get { return _EDMSBindingRule; }
            set { _EDMSBindingRule = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String EDMSAttachmentFormat
        {
            get { return _EDMSAttachmentFormat; }
            set { _EDMSAttachmentFormat = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? lock_owner_FK
        {
            get { return _lock_owner_FK; }
            set { _lock_owner_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? lock_date
        {
            get { return _lock_date; }
            set { _lock_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public int? check_in_for_attach_FK
        {
            get { return _check_in_for_attach_FK; }
            set { _check_in_for_attach_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Guid)]
        public Guid? check_in_session_id
        {
            get { return _check_in_session_id; }
            set { _check_in_session_id = value; }
        }
        #endregion

        public Attachment_PK() { }
        public Attachment_PK(Int32? attachment_PK, Byte[] disk_file, Int32? document_FK, String attachmentname, String filetype, Int32? userID, String pom_type, Guid? session_id, String ev_code, String type_for_fts, DateTime? modified_date, String EDMSDocumentId, String EDMSBindingRule, String EDMSAttachmentFormat, int? lock_owner_FK, DateTime? lock_date, int? check_in_for_attach_FK, Guid? check_in_session_id)
        {
            this.attachment_PK = attachment_PK;
            this.disk_file = disk_file;
            this.document_FK = document_FK;
            this.attachmentname = attachmentname;
            this.filetype = filetype;
            this.userID = userID;
            this.pom_type = pom_type;
            this.session_id = session_id;
            this.ev_code = ev_code;
            this.type_for_fts = type_for_fts;
            this.modified_date = modified_date;
            this.EDMSDocumentId = EDMSDocumentId;
            this.EDMSBindingRule = EDMSBindingRule;
            this.EDMSAttachmentFormat = EDMSAttachmentFormat;
            this.lock_owner_FK = lock_owner_FK;
            this.lock_date = lock_date;
            this.check_in_for_attach_FK = check_in_for_attach_FK;
            this.check_in_session_id = check_in_session_id;
        }

        public static List<Attachment_PK> GetAttachmentsForThisSession(IAttachment_PKOperations attachmentOperations, int? idDoc, Guid? attachmentSessionId)
        {
            var attachmentsForThisSession = new List<Attachment_PK>();

            if (attachmentSessionId.HasValue) attachmentsForThisSession = attachmentOperations.GetAttachmentsBySessionId(attachmentSessionId.Value);
            
            if(idDoc.HasValue) 
            {
                var attachmentsFromDb = attachmentOperations.GetAttachmentsForDocument(idDoc.Value);
                attachmentsForThisSession = attachmentsForThisSession.Union(attachmentsFromDb).ToList();
            }

            //attachmentsForThisSession.Insert(0,new Attachment_PK(){attachment_PK = -1, attachmentname = ""});

            return attachmentsForThisSession;
        }
    }

	public interface IAttachment_PKOperations : ICRUDOperations<Attachment_PK>
	{
        List<Attachment_PK> GetAttachmentsForDocumentWP(Int32 document_FK, int pageNumber, int pageSize, out int totalRecordsCount);
        List<Attachment_PK> GetAttachmentsForDocument(Int32 document_FK);
        void DeleteNULLByUserID(Int32? userID);
        Attachment_PK GetEntityWithoutDiskFile(Int32? attachment_PK);
        Attachment_PK SaveWithoutDiskFile(Int32? attachment_PK, Int32? document_FK, String attachmentname, String filetype, String typeForFts, Int32? userID);
        Attachment_PK SaveLinkToDocument(Int32? attachment_PK, Int32? document_FK);
        List<Attachment_PK> GetAttachmentsForDocumentWithDiskFile(Int32 document_FK);
        List<Attachment_PK> GetEntitiesWithoutDiskFile();
        Attachment_PK GetEntityByEVCode(string ev_code);
        List<Attachment_PK> GetAttachmentsBySessionId(Guid sessionId);
	    Attachment_PK GetCheckedInAttachment(int? attachmentPk, Guid sessionId);
	}
}
