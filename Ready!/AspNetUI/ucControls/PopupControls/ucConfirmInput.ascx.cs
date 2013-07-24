using System;
using System.Web.UI;
using AspNetUIFramework;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucConfirmInput : UserControl
    {
        public event EventHandler<EventArgs> OnConfirmInputButtonSave_Click;

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_ConfirmInput_Container.Style[ "Width" ]; }
            set { PopupControls_ConfirmInput_Container.Style[ "Width" ] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_ConfirmInput_Container.Style[ "Height" ]; }
            set { PopupControls_ConfirmInput_Container.Style[ "Height" ] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style[ "padding" ]; }
            set { modalPopupContainerBody.Style[ "padding" ] = value; }
        }

        #endregion

        #region Operations

        public void ShowModalPopup( DetailsForm form )
        {
            string newValues = string.Empty;

            foreach ( Control c in form.Controls )
            {
                newValues += GetFilledControls( c );
            }
            if ( !string.IsNullOrWhiteSpace( newValues ) )
                newValues = newValues.Remove( newValues.Length - 2 );

            lblResult.Text = newValues;

            PopupControls_ConfirmInput_Container.Style[ "display" ] = "inline";
        }

        #endregion
        
        #region FormOverrides

        protected override void OnInit( EventArgs e )
        {
            PopupControls_ConfirmInput_Container.Style[ "display" ] = "none";

            base.OnInit( e );
        }

        #endregion

        #region methods

        //protected void btnSearch_Click( object sender, EventArgs e )
        //{
        //    //Search( "" );
        //}

        //protected void btnClear_Click( object sender, EventArgs e )
        //{
        //    //ClearForm( "" );
        //}

        #endregion

        #region Conrol extra stuff

        private string GetFilledControls( Control c )
        {
            string str = string.Empty;

            if ( c is IControlCommon )
            {
                IControlCommon ctl = c as IControlCommon;
                if ( !string.IsNullOrWhiteSpace( ctl.ControlTextValue ) && !string.IsNullOrWhiteSpace( ctl.ControlValue.ToString() ) )
                {
                    str += "<b>" + ctl.ControlLabel + "</b>" + ": " + ctl.ControlTextValue + "; ";
                }
            }
            if ( c is SearcherDisplay )
            {
                SearcherDisplay ctl = c as SearcherDisplay;
                if ( !string.IsNullOrWhiteSpace( ctl.PrintableLabelValue ) && !string.IsNullOrWhiteSpace( ctl.PrintableTextValue ) )
                {
                    str += "<b>" + ctl.PrintableLabelValue + "</b>" + ": " + ctl.PrintableTextValue + "; ";
                }
            }

            foreach ( Control child in c.Controls )
            {
                str += GetFilledControls( child );
            }

            return str;
        }

        #endregion

        protected void btnSave_Click( object sender, EventArgs e )
        {
            if ( OnConfirmInputButtonSave_Click != null )
            {
                PopupControls_ConfirmInput_Container.Style[ "display" ] = "none";
                OnConfirmInputButtonSave_Click( sender, e );
            }
        }

        protected void btnCancel_Click( object sender, EventArgs e )
        {
            PopupControls_ConfirmInput_Container.Style[ "display" ] = "none";
        }
    }
}