using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class FileUploadCtrl : System.Web.UI.UserControl
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivFileUploadCtrl
        {
            get { return divFileUploadCtrl; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.FileUpload FileUpload
        {
            get { return fileUpload; }
        }

        public Label LblError
        {
            get { return lblError; }
        }

        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public bool FontItalic
        {
            get { return lblName.Font.Italic; }
            set { lblName.Font.Italic = value; }
        }

        public bool HasFile
        {
            get { return fileUpload.HasFile; }
        }

        public HttpPostedFile PostedFile
        {
            get { return fileUpload.PostedFile; }
        }

        public string FileName
        {
            get { return fileUpload.FileName; }
        }

        public bool Required
        {
            get { return spanRequired.Visible; }
            set { spanRequired.Visible = value; }
        }

        public bool Enabled
        {
            get { return fileUpload.Enabled; }
            set { fileUpload.Enabled = value; }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        #endregion
    }
}