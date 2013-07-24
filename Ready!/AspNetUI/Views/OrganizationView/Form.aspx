<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.OrganizationView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:TextBox ID="txtOrganizationName" runat="server" Label="Name:" Required="True" />
                <uc:TextBox ID="txtEvcode" runat="server" Label="EVCODE:" />
                <uc:TextBox ID="txtMflEvCode" runat="server" Label="MFL EVCODE:" />
                <uc:TextBox ID="txtOrganizationSenderId" runat="server" Label="Organization sender ID:" />
                <uc:TextBox ID="txtAddress" runat="server" Label="Address:" FontItalic="True"/>
                <uc:TextBox ID="txtCity" runat="server" Label="City:" />
                <uc:TextBox ID="txtState" runat="server" Label="State:" />
                <uc:TextBox ID="txtPostcode" runat="server" Label="Postcode:" FontItalic="True"/>
                <uc:DropDownList ID="ddlCountry" runat="server" Label="Country:" />
                <uc:TextBox ID="txtTelephoneNumber" runat="server" Label="Tel. number:" />
                <uc:TextBox ID="txtTelephoneExtension" runat="server" Label="Tel. extension:" />
                <uc:TextBox ID="txtTelephoneCountryCode" runat="server" Label="Tel. country code:" />
                <uc:TextBox ID="txtFaxNumber" runat="server" Label="Fax number:" />
                <uc:TextBox ID="txtFaxExtension" runat="server" Label="Fax extension:" />
                <uc:TextBox ID="txtFaxCountryCode" runat="server" Label="Fax country code:" />
                <uc:TextBox ID="txtEmail" runat="server" Label="Email:" />
                <uc:TextBox ID="txtComment" runat="server" Label="Comment:" FontItalic="True"/>
                <uc:DropDownList ID="ddlType" runat="server" Label="Type:" />
                <uc:ListBoxAu ID="lbAuOrganizationRole" runat="server" Label="Role Assignments:" SelectionMode="Multiple" VisibleRowsFrom="15" VisibleRowsTo="15" AllowDoubleClick="True" />
            </fieldset>
        </div>
        <div id="divRightPane" class="rightPane" style="margin-top: 0px; margin-left: -74px;">
            <fieldset>
                <uc:TextBox ID="txtCompany" runat="server" Label="Company:" FontItalic="True"/>
                <uc:TextBox ID="txtDepartment" runat="server" Label="Department:" FontItalic="True"/>
                <uc:TextBox ID="txtBuilding" runat="server" Label="Building:" FontItalic="True"/>
                <uc:TextBox ID="txtLastChange" runat="server" Label="Last change:" />
            </fieldset>
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
