<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.PersonView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxExt.ascx" TagName="ListBoxExt" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/QppvCodesPopup.ascx" TagName="QppvCodesPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:QppvCodesPopup ID="QppvCodePopup" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvPerson" runat="server" Label="Person:" />
    </div>
    <div class="subtabs">
         <uc:TabMenu ID="tabMenu" runat="server" Visible="True" />
    </div>
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:DropDownList ID="ddlCountryCode" runat="server" Label="Country code:" />
                <uc:TextBox ID="txtName" runat="server" Label="Name:" Required="True" />
                <uc:TextBox ID="txtFamilyName" runat="server" Label="Family name:" />
                <uc:TextBox ID="txtTitle" runat="server" Label="Title:" />
                <uc:ListBoxExt ID="lbExtQppvCodes" runat="server" Label="QPPV Codes:" SelectionMode="Single" VisibleRows="5" Actions="Add, Edit, Remove" AutoPostBack="True" />
                <uc:TextBox ID="txtCompany" runat="server" Label="Company:" />
                <uc:TextBox ID="txtDepartment" runat="server" Label="Department:" />
                <uc:TextBox ID="txtBuilding" runat="server" Label="Building:" />
                <uc:TextBox ID="txtStreet" runat="server" Label="Street:" />
                <uc:TextBox ID="txtState" runat="server" Label="State:" />
                <uc:TextBox ID="txtPostcode" runat="server" Label="Postcode:" />
                <uc:TextBox ID="txtCity" runat="server" Label="City:" />
                <uc:TextBox ID="txtPhoneCountryCode" runat="server" Label="Phone country code:" TextWidth="100" />
                <uc:TextBox ID="txtPhoneNumber" runat="server" Label="Phone number:" />
                <uc:TextBox ID="txtPhoneExtension" runat="server" Label="Phone extension:" />
                <uc:TextBox ID="txtMobileCountryCode" runat="server" Label="Mobile country code:" TextWidth="100" />
                <uc:TextBox ID="txtMobileNumber" runat="server" Label="Mobile number:" />
                <uc:TextBox ID="txtFaxCountryCode" runat="server" Label="Fax country code:" TextWidth="100" />
                <uc:TextBox ID="txtFaxNumber" runat="server" Label="Fax number:" />
                <uc:TextBox ID="txtFaxExtension" runat="server" Label="Fax extension:" />
                <uc:TextBox ID="txtEmail" runat="server" Label="Email:" />
                <uc:TextBox ID="txt24hTelNumber" runat="server" Label="24h tel. number:" />
                <uc:ListBoxAu ID="lbAuPersonRole" runat="server" Label="Role Assignments:" SelectionMode="Multiple" VisibleRowsFrom="15" VisibleRowsTo="15" AllowDoubleClick="True" />
                <uc:TextBox ID="txtLastChange" runat="server" Label="Last change:" />
            </fieldset>
        </div>
        <div id="divRightPane" class="rightPane" style="margin-top: 0px; margin-left: -74px;">
           
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlFooter" runat="server" class="bottom clear bottomControlsHolder" valign="center">
        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="button Cancel" OnClick="btnCancel_OnClick" />
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="button Save" OnClick="btnSave_OnClick" />
    </asp:Panel>

    <script type="text/javascript">
        $(document).ready(function () {
            $("[lbAuAllowDblClick='true']").live("dblclick", function () {
                var lbInputDblClick = $(this).siblings().first();
                lbInputDblClick.val("doubleclicked");
                window.__doPostBack($(this).attr("id"), lbInputDblClick.attr('name'));
            });
        });
    </script>
</asp:Content>
