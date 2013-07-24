using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using AspNetUI.ReadyEDMS;
using Ready.Model.Business;

namespace AspNetUI.Views
{
    public partial class TestEDMS : System.Web.UI.Page
    {
        protected void btnFetchEDMS_Click(object sender, EventArgs e)
        {
            var EDMSDocumentId = txtEDMSDocumentId.Text;

            var parentEntitiesLocal = ParentEntityDAL.GetEDMSParentEntities(EDMSDocumentId);
            RenderEDMSGrid(divLocalGrid, EDMSDocumentId, parentEntitiesLocal);
            divLocalDataHeader.Visible = true;

            using(var readyEDMSClient = new ReadyEDMSClient())
            {
                var parentEntitiesService = readyEDMSClient.GetEDMSParentEntities(EDMSDocumentId);
                RenderEDMSGrid(divServiceGrid, EDMSDocumentId, parentEntitiesService.ToList());
                divServiceDataHeader.Visible = true;
            }         
        }

        private void RenderEDMSGrid(HtmlGenericControl divGridContainer, string EDMSDocumentId,  List<ParentEntity> parentEntities)
        {
            divGridContainer.InnerHtml = "<br /><table border='1' cellpadding='5' cellspacing='0'><tr><th style='background-color: darkgrey;'>ID</th><th style='background-color: darkgrey;'>Name</th><th style='background-color: darkgrey;'>Description</th><th style='background-color: darkgrey;'>Type</th><th style='background-color: darkgrey;'>Responsible user</th></tr>";

            if (parentEntities.Count == 0) divGridContainer.InnerHtml += string.Format("<tr><td colspan='5'><br /><br />There are no linked entities for EDMSDocumentID = {0}</td></tr>", EDMSDocumentId);
            else
            {
                foreach (var parentEntity in parentEntities)
                {
                    divGridContainer.InnerHtml += "<tr>";

                    divGridContainer.InnerHtml += string.Format("<td>{0}</td>", parentEntity.ID);
                    divGridContainer.InnerHtml += string.Format("<td>{0}</td>", parentEntity.Name);
                    divGridContainer.InnerHtml += string.Format("<td>{0}</td>", parentEntity.Description);
                    divGridContainer.InnerHtml += string.Format("<td>{0}</td>", parentEntity.Type);
                    divGridContainer.InnerHtml += string.Format("<td>{0}</td>", parentEntity.ResponsibleUser);

                    divGridContainer.InnerHtml += "</tr>";
                }
            }

            divGridContainer.InnerHtml += "</table>";
        }
    }
}