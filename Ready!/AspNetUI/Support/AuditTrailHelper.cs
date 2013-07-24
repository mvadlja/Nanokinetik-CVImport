using System;
using System.Collections.Generic;
using Ready.Model;
using System.Web.UI.WebControls;
using System.Web.UI;
using AspNetUIFramework;
using System.Threading;

namespace AspNetUI.Support
{
    public class AuditTrailHelper
    {

        public static void SaveAuditTrail(string entityPK, 
                                          string oldValue, 
                                          string newValue, 
                                          string tableName, 
                                          string session_token,
                                          IAuditingMasterOperations _auditing_master_PKOperations,
                                          IAuditingDetailOperations _auditing_detail_PKOperations)
        {
            if (oldValue == newValue) return;

            Int32? master_PK = _auditing_master_PKOperations.GetAuditMasterIDBySessionToken(session_token);
            AuditingDetail _auditDetail = new AuditingDetail();
            _auditDetail.ColumnName = tableName;
            _auditDetail.MasterID = master_PK;
            _auditDetail.NewValue = newValue;
            _auditDetail.OldValue = oldValue;
            _auditDetail.PKValue = entityPK;
            _auditing_detail_PKOperations.Save(_auditDetail);
        }

        /// <summary>
        /// Saves audit trail details
        /// </summary>
        /// <param name="complexAuditNewValue"></param>
        /// <param name="complexAuditOldValue"></param>
        /// <param name="auditTrailSessionToken"></param>
        /// <param name="pkValue"></param>
        /// <param name="columnName"></param>
        public static void SaveAuditDetail(string complexAuditNewValue, string complexAuditOldValue, string auditTrailSessionToken, string pkValue, string columnName)
        {
            IAuditingMasterOperations auditingMasterOperations = new AuditingMasterDAL();
            IAuditingDetailOperations auditingDetailOperations = new AuditingDetailDAL();
            if (complexAuditNewValue == complexAuditOldValue) return;
            var masterPk = auditingMasterOperations.GetAuditMasterIDBySessionToken(auditTrailSessionToken);
            var auditDetail = new AuditingDetail
            {
                ColumnName = columnName,
                MasterID = masterPk,
                NewValue = complexAuditNewValue,
                OldValue = complexAuditOldValue,
                PKValue = pkValue
            };

            auditingDetailOperations.Save(auditDetail);
        }

        public static List<int> GetPanelHashValue(Control panel, ListBox logBox = null)
        {
            List<int> hashValues = new List<int>();
            if (panel != null && panel.Controls != null)
            {
                foreach (Control cnt in panel.Controls)
                {
                    //if (!cnt.Visible) continue;
                    List<int> values = GetControlHash(cnt, logBox);
                    foreach (int value in values) hashValues.Add(value);

                    if (values.Count > 0 && logBox != null)
                    {
                        string logstring = cnt.ID + ": ";
                        foreach (int i in values) logstring += i.ToString() + ",";
                        ListItem item = new ListItem();
                        item.Text = logstring;
                        logBox.Items.Add(item);
                    }

                }
            }
            hashValues.Sort();

            return hashValues;
        }

        public static List<int> GetControlHash(Control cnt, ListBox logBox = null)
        {
            
            List<int> retList = new List<int>();

            if (cnt is HyperLink && cnt.ID == "hlName")
            {
                retList.Add((cnt.ID + (cnt as HyperLink).Text).GetHashCode());
            }
            else if (cnt is Label_CT || 
                cnt.GetType().Name.StartsWith("uccontrols_popupcontrols"))
            { 
                // skip those
            }
            else if (cnt is SearcherDisplay)
            {
                retList.Add(GetUcSearcherHashCode(cnt as SearcherDisplay));
            }
            else if (cnt is Panel || cnt is UpdatePanel)
            {
                List<int> values = GetPanelHashValue(cnt, logBox);
                retList.AddRange(values);
            }
            else if (cnt is RadioButton)
            {
                retList.Add((cnt.ID + (cnt as RadioButton).Checked).ToString().GetHashCode());
            }
            else if (cnt is ListBox_CT)
            {
                retList.Add(GetListBox_CT_HashCode(cnt as ListBox_CT));
            }
            else if (cnt is ListBoxAER_CT)
            {
                retList.Add(GetLIstBoxAER_CT_HashCode(cnt as ListBoxAER_CT));
            }
            else if (cnt is IControlData)
            {
                retList.Add((cnt.ID + (cnt as IControlData).ControlValue.ToString()).GetHashCode());
            }
            else if (cnt.Controls != null && cnt.Controls.Count > 0)
            {
                List<int> values = GetPanelHashValue(cnt, logBox);
                retList.AddRange(values);
            }

            return retList;
        }

        public static void UpdateLastChange(int? entity_PK, 
                                            string table_name, 
                                            ILast_change_PKOperations _last_change_PKOperations,
                                            IUSEROperations _userOperations)
        {
            Last_change_PK lastChange = _last_change_PKOperations.GetEntityLastChange(table_name, entity_PK);
            if (lastChange == null) lastChange = new Last_change_PK();
            lastChange.change_date = DateTime.Now;
            lastChange.change_table = table_name;
            lastChange.entity_FK = entity_PK;

            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
                lastChange.user_FK = user.User_PK;

            _last_change_PKOperations.Save(lastChange);
        }

        public static string GetLastChangeFormattedString(int? entity_PK,
                                                          string table_name,
                                                          ILast_change_PKOperations _last_change_PKOperations,
                                                          IPerson_PKOperations _person_PKOperations)
        {
            Last_change_PK lastChange = _last_change_PKOperations.GetEntityLastChange(table_name, entity_PK);
   
            if (lastChange != null)
            {
                Person_PK lastChangePerson = _person_PKOperations.GetPersonByUserID(lastChange.user_FK);
                String personFullName;
                if (lastChangePerson != null && lastChangePerson.person_PK != null && !string.IsNullOrWhiteSpace(lastChangePerson.FullName))
                {
                    personFullName = lastChangePerson.FullName;
                }
                else
                {
                    personFullName = Thread.CurrentPrincipal.Identity.Name;
                }
                personFullName = personFullName.Trim();
                if (String.IsNullOrEmpty(personFullName)) personFullName = "-";
                return ((DateTime)lastChange.change_date).ToShortDateString().TrimEnd(new char[] { '.' }) + ", " +
                       ((DateTime)lastChange.change_date).ToShortTimeString() +
                       " by " + personFullName;
            }
            else
            {
                return "-";
            }
        }


        private static int GetListBox_CT_HashCode(ListBox_CT listBox)
        {
            int result = 0;
            foreach (ListItem item in listBox.ControlBoundItems)
            {
                result ^= (item.Text.ToString() + item.Value.ToString() + item.Selected.ToString()).ToString().GetHashCode();
            }
            return result;
        }

        private static int GetLIstBoxAER_CT_HashCode(ListBoxAER_CT listBox)
        {
            int result = 0;
            foreach (ListItem item in listBox.ControlBoundItems)
            {
                result ^= (item.Text.ToString() + item.Value.ToString() + item.Selected.ToString()).ToString().GetHashCode();
            }
            return result;
        }

        private static int GetUcSearcherHashCode(SearcherDisplay searcher)
        {
            int result = 0;
            if (searcher.SelectedObject != null)
            {
                result = (searcher.SelectedObject).GetHashCode();
            }
            return result;
        }


        public static List<int> GetPanelHashValueRedesign(Control panel, ListBox logBox = null)
        {
            var hashValues = new List<int>();
            if (panel != null)
            {
                foreach (Control cnt in panel.Controls)
                {
                    var values = GetControlHashRedesign(cnt, logBox);
                    hashValues.AddRange(values);

                    if (values.Count > 0 && logBox != null)
                    {
                        string logstring = cnt.ID + ": ";
                        foreach (int i in values) logstring += i.ToString() + ",";
                        ListItem item = new ListItem();
                        item.Text = logstring;
                        logBox.Items.Add(item);
                    }

                }
            }
            hashValues.Sort();

            return hashValues;
        }

        public static List<int> GetControlHashRedesign(Control control, ListBox logBox = null)
        {
            var retList = new List<int>();

            if (control is HyperLink && control.ID == "hlName")
            {
                retList.Add((control.ID + (control as HyperLink).Text).GetHashCode());
            }
            else if (control is Label_CT ||
                     control.GetType().Name.StartsWith("uccontrols_popupcontrols"))
            {
                // skip those
            }
            else if (control is SearcherDisplay)
            {
                retList.Add(GetUcSearcherHashCode(control as SearcherDisplay));
            }
            else if (control is Panel || control is UpdatePanel)
            {
                List<int> values = GetPanelHashValue(control, logBox);
                retList.AddRange(values);
            }
            else if (control is RadioButton)
            {
                retList.Add((control.ID + (control as RadioButton).Checked).ToString().GetHashCode());
            }
            else if (control is ListBox_CT)
            {
                retList.Add(GetListBox_CT_HashCode(control as ListBox_CT));
            }
            else if (control is ListBoxAER_CT)
            {
                retList.Add(GetLIstBoxAER_CT_HashCode(control as ListBoxAER_CT));
            }
            else if (control is IControlData)
            {
                retList.Add((control.ID + (control as IControlData).ControlValue.ToString()).GetHashCode());
            }
            else if (control.Controls != null && control.Controls.Count > 0)
            {
                List<int> values = GetPanelHashValue(control, logBox);
                retList.AddRange(values);
            }

            return retList;
        }

        public static readonly Dictionary<string, string> ColumnNames = new Dictionary<string, string>
        {
            {"orphan_drug", "Orphan drug"},
            {"attachment_name", "Name"},
            {"intensive_monitoring", "Intensive monitoring"},
            {"authorisation_procedure", "Authorisation procedure"},
            {"comments", "Comments"},
            {"responsible_user_person_FK", "Responsible user"},
            {"psur", "PSUR cycle"},
            {"next_dlp", "Next DLP"},
            {"name", "Name"},
            {"description", "Description"},
            {"client_organization_FK", "Client"},
            {"type_product_FK", "Product type"},
            {"product_number", "Product number"},
            {"product_ID", "Product ID"},
            {"PRODUCT_COUNTRY_MN", "Countries"},
            {"PRODUCT_DOMAIN_MN", "Domain"},
            {"PRODUCT_PP_MN", "Pharmaceutical products"},
            {"PRODUCT_ATC_MN", "Drug ATCs"},
            {"PRODUCT_MANUFACTURER_MN", "Manufacturer"},
            {"PRODUCT_PARTNER_MN", "Partner"},
            {"activity_ID", "Activity ID"},
            {"responsible_user_FK", "Responsible user"},
            {"booked_slots", "Booked slots"},
            {"PP_ADMINISTRATION_ROUTES", "Administration routes"},
            {"PP_MEDICAL_DEVICES", "Medical devices"},
            {"PP_ADJUVANTS", "Adjuvants"},
            {"PP_EXCIPIENTS", "Excipients"},
            {"PP_ACTIVE_INGREDIENTS", "Active ingredients"},
            {"PP_PRODUCTS", "Products"},
            {"Pharmform_FK", "Pharmaceutical form"},
            {"document_PK", "Document PK"},
            {"person_FK", "Responsible user"},
            {"type_FK", "Document Type"},
            {"document_code", "Document number"},
            {"comment", "Comment"},
            {"regulatory_status", "Regulatory status"},
            {"version_number", "Version number"},
            {"version_label", "Version label"},
            {"change_date", "Change date"},
            {"effective_start_date", "Effective start date"},
            {"effective_end_date", "Effective end date"},
            {"attachment_type_FK", "Attachment type"},
            {"DOCUMENT_LANGUAGE_CODE", "Language codes"},
            {"DOCUMENT_PROJECT_MN", "Projects"},
            {"DOCUMENT_PRODUCT_MN", "Products"},
            {"DOCUMENT_AP_MN", "Authorised products"},
            {"DOCUMENT_PP_MN", "Pharmaceutical products"},
            {"DOCUMENT_ACTIVITY_MN", "Activities"},
            {"DOCUMENT_ATTACHMENTS", "Attachments"},
            {"version_date", "Version date"},
            {"product_FK", "Related product"},
            {"authorisationcountrycode_FK", "Authorisation country"},
            {"organizationmahcode_FK", "Licence holder"},
            {"product_name", "Full presentation name"},
            {"productshortname", "Product short name"},
            {"authorisationnumber", "Authorisation number"},
            {"authorisationstatus_FK", "Authorisation status"},
            {"authorisationdate", "Authorisation date"},
            {"authorisationexpdate", "Authorization expiry date"},
            {"authorisationwithdrawndate", "Withdrawn date"},
            {"packagedesc", "Package description"},
            {"marketed", "Marketed"},
            {"legalstatus", "Legal status"},
            {"mflcode_FK", "Master File Location"},
            {"qppvcode_person_FK", "QPPV"},
            {"ev_code", "EVCODE"},
            {"launchdate", "Launch date"},
            {"evprm_comments", "Comment (EVPRM)"},
            {"shelflife", "Shelf life"},
            {"productgenericname", "Product generic name"},
            {"productcompanyname", "Product company name"},
            {"productstrenght", "Product strength name"},
            {"productform", "Product form name"},   
            {"infodate", "Info date"},
            {"phv_email", "PhV EMail"},
            {"article_57_reporting", "Article 57 reporting"},
            {"sunsetclause", "Sunset clause"},
            {"substance_translations", "Substance translations"}, 
            {"AP_ORGANIZATION_DIST_MN", "Distributor Assignments"}, 
            {"MEDDRA_AP_MN", "Indications"},
            {"phv_phone", "PhV Phone"},
            {"XEVPRM_status", "XEVPRM status"},
            {"user_FK", "Responsible user"},
            {"mode_FK", "Activity mode"},
            {"procedure_type_FK", "Procedure type"},
            {"regulatory_status_FK", "Regulatory status"},
            {"start_date", "Start date"},
            {"expected_finished_date", "Expected finished date"},
            {"actual_finished_date", "Actual finished date"},
            {"approval_date", "Approval date"},
            {"submission_date", "Submission date"},
            {"procedure_number", "Procedure number"},
            {"legal", "Legal basis of application"},
            {"internal_status_FK", "Internal status"},
            {"ACTIVITY_PRODUCT_MN", "Products"},
            {"ACTIVITY_TYPE_MN", "Type"},
            {"ACTIVITY_APPLICANT_MN", "Applicant"},      
            {"ACTIVITY_COUNTRY_MN", "Countries"},
            {"ACTIVITY_PROJECT_MN", "Projects"},
            {"billable", "Billable"},
            {"automatic_alerts_on", "Automatic alerts"},
            {"local_representative_FK", "Local representative"},
            {"qppv_code_FK", "QPPV"}   
        };
    }
}