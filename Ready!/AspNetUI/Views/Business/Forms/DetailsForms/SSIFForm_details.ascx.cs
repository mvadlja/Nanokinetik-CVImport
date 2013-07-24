using System;
using System.Web.UI.WebControls;
using AspNetUIFramework;

using AspNetUI.Support;

namespace AspNetUI.Views
{
    public partial class SSIFForm_details : DetailsForm
    {

        #region Declarations

        TreeNodeCollection twCommonNodes = new TreeNodeCollection();
        TreeNodeCollection twExtensionNodes = new TreeNodeCollection();

        #endregion


        #region Init handlers

		protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);


        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) { }
            string page = Request.QueryString["page"] != null ? Request.QueryString["page"].ToString() : null;
            MasterMain m = (MasterMain)Page.Master;
        }


        #endregion

        protected void m_OnTwCommonChanged(object sender, FormDetailsEventArgs e)
        {
            twExtensionNodes = (TreeNodeCollection)e.DataItem;
            ShowExtended(twExtensionNodes[0]);
        }


        protected void m_OnTwExtensionChanged(object sender, FormDetailsEventArgs e)
        {
            twCommonNodes = (TreeNodeCollection)e.DataItem;
            ShowExtended(twCommonNodes[0]);
        }


        #region FormOverrides

        public override object SaveForm(object id, string arg)
        {
            object entity = new object();
            return entity;
        }

        // Clears form
        public override void ClearForm(string arg)
        {
           
        }

		// Fills all form controls with data
        public override void FillDataDefinitions(string arg)
        {     
         
        }

    
        // Binds form
        public override void BindForm(object id, string arg)
        {
            
        }



        private void ShowExtended(TreeNode parentNode)
        {
            TreeNodeCollection nodes = parentNode.ChildNodes;
            foreach (TreeNode node in nodes)
            {
                if (node.ChildNodes.Count > 0)
                {
                    if (node.Value != "")
                    {
                        if (node.Value == "common")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblcommon.Visible = true;
                            else
                                tblcommon.Visible = false;
                        }

                        if (node.Value == "substancename1")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                tblsubstancename1.Visible = true;
                                substancename1BLANK.Visible = false;
                            }
                            else
                            {
                                tblsubstancename1.Visible = false;
                                substancename1BLANK.Visible = true;
                            }
                        }

                        if (node.Value == "officialname")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblofficialname.Visible = true;
                            else
                                tblofficialname.Visible = false;
                        }

                        if (node.Value == "officialnamedomaincv")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblofficialnamedomaincv.Visible = true;
                            else
                                tblofficialnamedomaincv.Visible = false;
                        }

                        //if (node.Value == "officialnamedomains")
                        //{
                        //    if (node.Expanded == true && AllParentsExtended(node.Parent))
                        //        officialnamedomainsBLANK.Visible = false;
                        //    else
                        //        officialnamedomainsBLANK.Visible = true;
                        //}

                        if (node.Value == "officialnamejurisdictioncv")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblofficialnamejurisdictioncv.Visible = true;
                            else
                                tblofficialnamejurisdictioncv.Visible = false;
                        }

                        //if (node.Value == "officialnamejurisdictions")
                        //{
                        //    if (node.Expanded == true && AllParentsExtended(node.Parent))
                        //        officialnamejurisdictionsBLANK.Visible = false;
                        //    else
                        //        officialnamejurisdictionsBLANK.Visible = true;
                        //}


                        if (node.Value == "referencesource1")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                tblreferencesource1.Visible = true;
                                referencesource1BLANK.Visible = false;
                            }
                            else
                            {
                                tblreferencesource1.Visible = false;
                                referencesource1BLANK.Visible = true;
                            }
                        }
                        
                        if (node.Value == "substancename2")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                tblsubstancename2.Visible = true;
                                substancename2BLANK.Visible = false;
                            }
                            else
                            {
                                tblsubstancename2.Visible = false;
                                substancename2BLANK.Visible = true;
                            }
                        }

                        if (node.Value == "referencesource2")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblreferencesource2.Visible = true;
                            else
                                tblreferencesource2.Visible = false;
                        }

                        if (node.Value == "substancecode")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                tblsubstancecode.Visible = true;
                                substancecodeBLANK.Visible = false;
                            }
                            else
                            {
                                tblsubstancecode.Visible = false;
                                substancecodeBLANK.Visible = true;
                            }
                        }

                        if (node.Value == "referencesource3")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblreferencesource3.Visible = true;
                            else
                                tblreferencesource3.Visible = false;
                        }

                        if (node.Value == "version")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                tblversion.Visible = true;
                                versionBLANK.Visible = false;
                            }
                            else
                            {
                                tblversion.Visible = false;
                                versionBLANK.Visible = true;
                            }
                        }


                        //if (node.Value == "version1")
                        //{
                        //    if (node.Expanded == true && AllParentsExtended(node.Parent))
                        //    {
                        //        version1BLANK.Visible = false;
                        //    }
                        //    else
                        //    {
                        //        version1BLANK.Visible = true;
                        //    }
                        //}

                        if (node.Value == "substanceclassification")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblsubstanceclassification.Visible = true;
                            else
                                tblsubstanceclassification.Visible = false;
                        }

                        if (node.Value == "substanceclassificationsubtype")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblsubstanceclassificationsubtype.Visible = true;
                            else
                                tblsubstanceclassificationsubtype.Visible = false;
                        }


                        if (node.Value == "referencesource4")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblreferencesource4.Visible = true;
                            else
                                tblreferencesource4.Visible = false;
                        }

                        if (node.Value == "referenceinformation")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                referenceinformationBLANK.Visible = false;
                            else
                                referenceinformationBLANK.Visible = true;
                        }
                        
                        if (node.Value == "target")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                tbltarget.Visible = true;
                                //targetBLANK.Visible = false;
                            }
                            else
                            {
                                tbltarget.Visible = false;
                                //targetBLANK.Visible = true;
                            }
                        }

                        if (node.Value == "targets")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                targetsBLANK.Visible = false;
                            else
                                targetsBLANK.Visible = true;
                        }


                        if (node.Value == "referencesource5")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblreferencesource5.Visible = true;
                            else
                                tblreferencesource5.Visible = false;
                        }


                        if (node.Value == "extension")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                extensionBLANK.Visible = false;
                            else
                                extensionBLANK.Visible = true;
                        }

                        if (node.Value == "single")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                singleBLANK.Visible = true;
                            else
                                singleBLANK.Visible = true;
                        }

                        if (node.Value == "structures")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                structuresBLANK.Visible = true;
                            }
                            else
                            {
                                structuresBLANK.Visible = true;
                            }
                        }
                        
                        if (node.Value == "structure1")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                tblstructure1.Visible = true;
                                structure1BLANK.Visible = false;
                                structuresBLANK.Visible = false;
                                singleBLANK.Visible = false;
                            }
                            else
                            {
                                tblstructure1.Visible = false;
                                structure1BLANK.Visible = true;
                            }
                        }

                        if (node.Value == "structure2")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                            {
                                tblstructure2.Visible = true;
                                structure2BLANK.Visible = false;
                                structuresBLANK.Visible = false;
                                singleBLANK.Visible = false;
                            }
                            else
                            {
                                tblstructure2.Visible = false;
                                structure2BLANK.Visible = true;
                            }
                        }

                        if (node.Value == "chemical")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                chemicalBLANK.Visible = true;
                            else
                                chemicalBLANK.Visible = false;
                        }

                        if (node.Value == "stoichiometric")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblstoichiometric.Visible = true;
                            else
                                tblstoichiometric.Visible = false;
                        }

                        if (node.Value == "referencesource6")
                        {
                            if (node.Expanded == true && AllParentsExtended(node.Parent))
                                tblreferencesource6.Visible = true;
                            else
                                tblreferencesource6.Visible = false;
                        }

                    }

                    
                    //info.InnerHtml += "Text: " + node.Text + " |\t Expanded: <b>" + node.Expanded.ToString() + "</b> <br />";
                    ShowExtended(node);
                }
               
            } 
            return;
        }

        bool AllParentsExtended(TreeNode node) {
            if (node.Parent == null) return (bool)node.Expanded;
            else return (bool)node.Expanded && AllParentsExtended(node.Parent);
        }


        // Validates form
        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            
            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }


        #endregion

        #region Security

        public override DetailsPermissionType CheckAccess()
        {
            if (SecurityOperations.CheckUserRole("Office"))
            {
                return DetailsPermissionType.READ_WRITE;
            }

            if (SecurityOperations.CheckUserRole("User"))
            {
                return DetailsPermissionType.READ;
            }

            return DetailsPermissionType.READ;
        }

        #endregion

    }
}