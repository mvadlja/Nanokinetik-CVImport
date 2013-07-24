// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:03
// Description:	
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "XEVPRM_MESSAGE")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Xevprm_message_PK
    {
        private Int32? _xevprm_message_PK;
        private Int32? _ap_FK;
        private String _message_number;
        private Int32? _message_status_FK;
        private DateTime? _message_creation_date;
        private Int32? _user_FK;
        private String _xml;
        private String _xml_hash;
        private String _sender_ID;
        private String _ack;
        private Int32? _ack_type;
        private DateTime? _gateway_submission_date;
        private DateTime? _gateway_ack_date;
        private Int32? _submitted_FK;
        private String _generated_file_name;
        private Boolean? _deleted;
        private Int32? _operation_type;
        private Int32? _received_message_FK;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? xevprm_message_PK
        {
            get { return _xevprm_message_PK; }
            set { _xevprm_message_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ap_FK
        {
            get { return _ap_FK; }
            set { _ap_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String message_number
        {
            get { return _message_number; }
            set { _message_number = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? message_status_FK
        {
            get { return _message_status_FK; }
            set { _message_status_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? message_creation_date
        {
            get { return _message_creation_date; }
            set { _message_creation_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? user_FK
        {
            get { return _user_FK; }
            set { _user_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String xml
        {
            get { return _xml; }
            set { _xml = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String xml_hash
        {
            get { return _xml_hash; }
            set { _xml_hash = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String sender_ID
        {
            get { return _sender_ID; }
            set { _sender_ID = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ack
        {
            get { return _ack; }
            set { _ack = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ack_type
        {
            get { return _ack_type; }
            set { _ack_type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? gateway_submission_date
        {
            get { return _gateway_submission_date; }
            set { _gateway_submission_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? gateway_ack_date
        {
            get { return _gateway_ack_date; }
            set { _gateway_ack_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? submitted_FK
        {
            get { return _submitted_FK; }
            set { _submitted_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String generated_file_name
        {
            get { return _generated_file_name; }
            set { _generated_file_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? operation_type
        {
            get { return _operation_type; }
            set { _operation_type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? received_message_FK
        {
            get { return _received_message_FK; }
            set { _received_message_FK = value; }
        }

        public XevprmStatus XevprmStatus
        {
            get { return message_status_FK != null && Enum.IsDefined(typeof(XevprmStatus), message_status_FK) ? (XevprmStatus)message_status_FK : XevprmStatus.NULL; }
            set { message_status_FK = (int)value == 0 ? (int?)null : (int)value; }
        }

        #endregion
        public Xevprm_message_PK() { }
        public Xevprm_message_PK(Int32? xevprm_message_PK, Int32? ap_FK, String message_number, Int32? message_status_FK, DateTime? message_creation_date, Int32? user_FK, String xml, String xml_hash, String sender_ID, String ack, Int32? ack_type, DateTime? gateway_submission_date, DateTime? gateway_ack_date, Int32? submitted_FK, String generated_file_name, Boolean? deleted, Int32? operation_type, Int32? received_message_FK)
        {
            this.xevprm_message_PK = xevprm_message_PK;
            this.ap_FK = ap_FK;
            this.message_number = message_number;
            this.message_status_FK = message_status_FK;
            this.message_creation_date = message_creation_date;
            this.user_FK = user_FK;
            this.xml = xml;
            this.xml_hash = xml_hash;
            this.sender_ID = sender_ID;
            this.ack = ack;
            this.ack_type = ack_type;
            this.gateway_submission_date = gateway_submission_date;
            this.gateway_ack_date = gateway_ack_date;
            this.submitted_FK = submitted_FK;
            this.generated_file_name = generated_file_name;
            this.deleted = deleted;
            this.operation_type = operation_type;
            this.received_message_FK = received_message_FK;
        }
    }

    public interface IXevprm_message_PKOperations : ICRUDOperations<Xevprm_message_PK>
    {
        List<int> GetEntitiesPksReadyForSubmission();
        List<int> GetEntitiesPksReadyForMDNSubmission();

        Xevprm_message_PK GetEntityByMessageNumber(string messageNumber);
        Xevprm_message_PK GetEntityByMA(int? maPk);
        Xevprm_message_PK GetLatestEntityByXevprmEntity(int xevprmEntityPk, int xevprmEntityType);
        string GetLatestMessageNumberByXevprmEntity(int xevprmEntityPk, int xevprmEntityType);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetXevprmStatistic();
    }

    public enum XevprmOperationType
    {
        NULL,
        // This operation type allows the Sender Organisation to provide new medicinal product
        // information via an XEVPRM.
        Insert = 1,

        // This operation type allows the Sender Organisation to update the content of medicinal
        // product information previously submitted via an XEVPRM.
        Update = 2,

        // This operation type allows the Sender Organisation to update medicinal product
        // information of the marketing authorisation of an authorised medicinal product via an XEVPRM. This
        // update is related to variations of a marketing authorisation. This operation applies only to
        // authorised medicinal products.
        Variation = 3,

        // This operation type allows the Sender Organisation to nullify medicinal product
        // information previously submitted via an XEVPRM.
        Nullify = 4,

        // This operation allows the Sender Organisation to inform about the withdrawal of an
        // authorised medicinal product from the market via an XEVPRM. This operation applies only to
        // authorised medicinal products.
        Withdraw = 6
    }

    public enum XevprmStatus
    {
        NULL,
        Created,
        ValidationFailed,
        ValidationSuccessful,
        ReadyToSubmit,
        SubmittingMessage,
        SubmissionFailed,
        SubmissionAborted,
        MDNPending,
        MDNReceivedError,
        MDNReceivedSuccessful,
        ACKReceived,
        SubmittingMDN,
        ACKDeliveryFailed,
        ACKDelivered
    }

    public enum XevprmEntityType
    {
        NULL,
        AuthorisedProduct,
        Attachment
    }
}
