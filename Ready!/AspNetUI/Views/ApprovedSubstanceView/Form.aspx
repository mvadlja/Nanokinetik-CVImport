<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.ApprovedSubstanceView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxExt.ascx" TagName="ListBoxExt" TagPrefix="uc" %>

<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSUBTRN.ascx" TagName="PopupSubstanceTranslation" TagPrefix="uc" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormINTCOD.ascx" TagName="PopupInternationalCode" TagPrefix="uc" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormPREEVCODE.ascx" TagName="PopupPreviousEvCode" TagPrefix="uc" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSUBALS.ascx" TagName="PopupSubstanceAlias" TagPrefix="uc" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSUBATT.ascx" TagName="PopupSubstanceAttachment" TagPrefix="uc" %>


<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    
    <uc:PopupSubstanceTranslation ID="PopupSubstanceTranslation" runat="server" />
    <uc:PopupInternationalCode ID="PopupInternationalCode" runat="server" />
    <uc:PopupPreviousEvCode ID="PopupPreviousEvCode" runat="server" />
    <uc:PopupSubstanceAlias ID="PopupSubstanceAlias" runat="server" />
    <uc:PopupSubstanceAttachment ID="PopupSubstanceAttachment" runat="server" />

    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:TextBoxSr ID="txtSrSubstanceEvCode" runat="server" Label="EVCODE:" Required="False" SearchType="Substance" />
                <uc:TextBox ID="txtSubstanceName" runat="server" Label="Substance name:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" Required="True"/>
                <uc:TextBox ID="txtCasNumber" runat="server" Label="CAS number:" />
                <uc:TextBox ID="txtMolecularFormula" runat="server" Label="Molecular formula:" />
                <uc:DropDownList ID="ddlSubstanceClass" runat="server" Label="Substance class:" />
                <uc:TextBox ID="txtCbd" runat="server" Label="CBD:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:ListBoxExt ID="lbExtSubstanceTranslation" runat="server" Label="Substance translation:" Actions="Add, Edit, Remove" />
                <uc:ListBoxExt ID="lbExtSubstanceAliases" runat="server" Label="Substance aliases:" Actions="Add, Edit, Remove" />
                <uc:ListBoxExt ID="lbExtInternationalCodes" runat="server" Label="International codes:" Actions="Add, Edit, Remove" />
                <uc:ListBoxExt ID="lbExtPreviousEvCodes" runat="server" Label="Previous EVCODEs:" Actions="Add, Edit, Remove" />
                <uc:ListBoxExt ID="lbExtSubstanceAttachment" runat="server" Label="Substance attachment:" Actions="Add, Edit, Remove" />
                <uc:TextBox ID="txtComments" runat="server" Label="Comments:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
            </fieldset>
        </div>
        <div id="divRightPane" class="rightPane" style="margin-top: 0px; margin-left: -74px;">
            <fieldset>
                
            </fieldset>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlFooter" runat="server" class="bottom clear bottomControlsHolder" valign="center">
        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="button Cancel" OnClick="btnCancel_OnClick" />
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="button Save" OnClick="btnSave_OnClick" />
    </asp:Panel>
</asp:Content>
