// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/21/2012 2:25:00 PM
// Description:	GEM2 Generated class for table ready_billev_production.dbo.XEVPRM_AP_DETAILS
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "XEVPRM_AP_DETAILS")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Xevprm_ap_details_PK
	{
        private Int32? _xevprm_ap_details_PK;
        private Int32? _ap_FK;
        private String _ap_name;
        private String _package_description;
        private String _authorisation_country_code;
        private Int32? _related_product_FK;
        private String _related_product_name;
        private String _licence_holder;
        private String _authorisation_status;
        private String _authorisation_number;
        private Int32? _operation_type;
        private String _ev_code;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? xevprm_ap_details_PK
        {
            get { return _xevprm_ap_details_PK; }
            set { _xevprm_ap_details_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ap_FK
        {
            get { return _ap_FK; }
            set { _ap_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ap_name
        {
            get { return _ap_name; }
            set { _ap_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String package_description
        {
            get { return _package_description; }
            set { _package_description = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String authorisation_country_code
        {
            get { return _authorisation_country_code; }
            set { _authorisation_country_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? related_product_FK
        {
            get { return _related_product_FK; }
            set { _related_product_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String related_product_name
        {
            get { return _related_product_name; }
            set { _related_product_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String licence_holder
        {
            get { return _licence_holder; }
            set { _licence_holder = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String authorisation_status
        {
            get { return _authorisation_status; }
            set { _authorisation_status = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String authorisation_number
        {
            get { return _authorisation_number; }
            set { _authorisation_number = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? operation_type
        {
            get { return _operation_type; }
            set { _operation_type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ev_code
        {
            get { return _ev_code; }
            set { _ev_code = value; }
        }

        public XevprmOperationType OperationType
        {
            get { return operation_type != null && Enum.IsDefined(typeof(XevprmOperationType), operation_type) ? (XevprmOperationType)operation_type : XevprmOperationType.NULL; }
            set { operation_type = (int)value == 0 ? (int?)null : (int)value; }
        }

        #endregion

        public Xevprm_ap_details_PK() { }
        public Xevprm_ap_details_PK(Int32? xevprm_ap_details_PK, Int32? ap_FK, String ap_name, String package_description, String authorisation_country_code, Int32? related_product_FK, String related_product_name, String licence_holder, String authorisation_status, String authorisation_number, Int32? operation_type, String ev_code)
        {
            this.xevprm_ap_details_PK = xevprm_ap_details_PK;
            this.ap_FK = ap_FK;
            this.ap_name = ap_name;
            this.package_description = package_description;
            this.authorisation_country_code = authorisation_country_code;
            this.related_product_FK = related_product_FK;
            this.related_product_name = related_product_name;
            this.licence_holder = licence_holder;
            this.authorisation_status = authorisation_status;
            this.authorisation_number = authorisation_number;
            this.operation_type = operation_type;
            this.ev_code = ev_code;
        }
    }

	public interface IXevprm_ap_details_PKOperations : ICRUDOperations<Xevprm_ap_details_PK>
	{
        Xevprm_ap_details_PK GetEntityForXevprm(int? xevprm_message_PK);
        List<Xevprm_ap_details_PK> GetEntitiesByXevprm(int? xevprm_message_PK);
	}
}
