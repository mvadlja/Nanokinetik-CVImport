using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using AspNetUIFramework;
using System.Configuration;
using xEVMPD;
using Ready.Model;

namespace AspNetUI.Views.Business
{
    public partial class ValidationReport : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //string msgid = Request.QueryString["msgID"];

            //if (ValidationHelper.IsValidInt(msgid))
            //{
                //int activeRow = 1;//The row to which record will be added.
                //Control errors = xEVPRMessage.ValidationErrorsAP(Convert.ToInt32(msgid), XevprmOperationType.Insert);

                //string fullPath = Server.MapPath("~") + "\\" + ConfigurationManager.AppSettings["AttachTmpDir"] + "\\ValidationReport"+DateTime.Now.GetHashCode()+".xlsx";

                //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                //Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
                //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                //foreach (Control levelOne in errors.Controls)
                //{
                //    if (((levelOne as Label).Text).Trim().ToLower().Contains("pharmaceutical products"))
                //    {
                //        bool noErrors = true;
                //        foreach (Control item in levelOne.Controls)
                //        {
                //            if (item.Controls.Count != 0)
                //                noErrors = false;
                //        }
                //        if (noErrors)
                //            continue;
                //        xlWorkSheet.get_Range("A" + activeRow.ToString()).Value2 = (levelOne as Label).Text;
                //        xlWorkSheet.get_Range("A" + activeRow.ToString(), "B" + activeRow.ToString()).Merge();

                //        activeRow++;    
                //    }
                //    else
                //    {
                //        //Display AP,P
                //        xlWorkSheet.get_Range("A" + activeRow.ToString()).Value2 = (levelOne as Label).Text;
                //        xlWorkSheet.get_Range("A" + activeRow.ToString(), "B" + activeRow.ToString()).Merge();
                //        activeRow++;
                //    }

                //    string applicationURL = ConfigurationManager.AppSettings["ApplicationURL"];
                //    string applicationURLSecure = ConfigurationManager.AppSettings["ApplicationURLSecure"];
                //    string appVirtualPath = ConfigurationManager.AppSettings["AppVirtualPath"];

                //    string URL = Request.Url.Authority; // default value if none of config section is present
                //    string scheme = Request.Url.Scheme;

                //    if (scheme == "http") URL = applicationURL;
                //    if (scheme == "https") URL = applicationURLSecure;

                //    string applicationURLWithScheme = scheme + "://" + URL + appVirtualPath;

                //    foreach (Control levelTwo in levelOne.Controls)
                //    {
                //        //Display AP and P errors
                //        if (levelTwo is HyperLink)
                //        {
                //            HyperLink hl = new HyperLink();
                //            hl.Text = (levelTwo as HyperLink).Text;
                //            hl.NavigateUrl = (levelTwo as HyperLink).NavigateUrl;
                //            hl.Target = (levelTwo as HyperLink).Target;

                //            if (hl.Text.Contains(':'))
                //            {
                //                string[] hlText = hl.Text.Split(':');
                //                xlWorkSheet.get_Range("A" + activeRow.ToString()).Value2 = hlText[0];
                //                xlWorkSheet.Hyperlinks.Add(xlWorkSheet.get_Range("B" + activeRow.ToString()), applicationURLWithScheme + "/Views/Business/" + hl.NavigateUrl, Type.Missing, Type.Missing, hlText[1]);
                //                activeRow++;
                //            }
                //            else
                //            {
                //                xlWorkSheet.Hyperlinks.Add(xlWorkSheet.get_Range("B" + activeRow.ToString()), applicationURLWithScheme + "/Views/Business/" + hl.NavigateUrl, Type.Missing, Type.Missing, hl.Text);
                //                activeRow++;
                //            }
                //        }
                //        else if (levelTwo is LiteralControl)
                //        {
                //            String literalControl = (levelTwo as LiteralControl).Text;
                //            Label lc = new Label();
                //            lc.Text = literalControl;

                //            xlWorkSheet.get_Range("B" + activeRow.ToString()).Value2 = lc.Text;
                //            activeRow++;

                //        }
                //        else if (levelTwo is Label)
                //        {
                //            Label lvlThreeLabel = levelTwo as Label;

                //            xlWorkSheet.get_Range("B" + activeRow.ToString()).Value2 = lvlThreeLabel.Text;
                //            xlWorkSheet.get_Range("A" + activeRow.ToString(), "B" + activeRow.ToString()).Merge();
                //            activeRow++;
                //        }
                //        //Display PP errors
                //        foreach (Control levelThree in levelTwo.Controls)
                //        {
                //            if (levelThree is HyperLink)
                //            {
                //                HyperLink hl = new HyperLink();
                //                hl.Text = (levelThree as HyperLink).Text;

                //                hl.NavigateUrl = (levelThree as HyperLink).NavigateUrl;
                //                hl.Target = (levelThree as HyperLink).Target;

                //                if (hl.Text.Contains(':'))
                //                {
                //                    string[] hlText = hl.Text.Split(':');
                //                    xlWorkSheet.get_Range("A" + activeRow.ToString()).Value2 = hlText[0];
                //                    xlWorkSheet.Hyperlinks.Add(xlWorkSheet.get_Range("B" + activeRow.ToString()), applicationURLWithScheme + "/Views/Business/" + hl.NavigateUrl, Type.Missing, Type.Missing, hlText[1]);
                //                    activeRow++;
                //                }
                //                else
                //                {
                //                    xlWorkSheet.Hyperlinks.Add(xlWorkSheet.get_Range("B" + activeRow.ToString()), applicationURLWithScheme + "/Views/Business/" + hl.NavigateUrl, Type.Missing, Type.Missing, hl.Text);
                //                    activeRow++;
                //                }
                //            }
                //            else if (levelThree is LiteralControl)
                //            {
                //                String literalControl = (levelThree as LiteralControl).Text;
                //                Label lc = new Label();
                //                lc.Text = literalControl;

                //                xlWorkSheet.get_Range("B" + activeRow.ToString()).Value2 = lc.Text;
                //                activeRow++;
                //            }
                //            else if (levelThree is Label)
                //            {
                //                Label lvlThreeLabel = levelThree as Label;

                //                xlWorkSheet.get_Range("B" + activeRow.ToString()).Value2 = lvlThreeLabel.Text;
                //                activeRow++;
                //            }
                //        }
                //    }
                //}
                //xlWorkSheet.Rows.AutoFit();
                //xlWorkSheet.Columns.AutoFit();
                //xlWorkBook.SaveAs(fullPath);
                //xlWorkBook.Close(true, Type.Missing, Type.Missing);
                //xlApp.Quit();

                //Response.Clear();
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + "grid.xlsx" + "");
                //Response.ContentType = "application/octet-stream";
                //Response.TransmitFile(fullPath);
                //Response.Flush();
                //Response.Close();
                //try
                //{
                //    new FileInfo(fullPath).Delete();
                //}
                //catch (Exception) { /*Deleting file is not crucial action*/}

            //}

        }
    }
}