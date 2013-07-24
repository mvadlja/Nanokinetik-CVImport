<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.UserSecurityView.Form" EnableViewState="true" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:ModalPopup ID="mpInfo" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvPerson" runat="server" Label="Person:" />
    </div>
    
     <div class="subtabs">
         <uc:TabMenu ID="tabMenu" runat="server" Visible="True" />
    </div>
    
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:RadioButtonYn ID="rbYnActiveDirectoryUser" runat="server" Label="Active Directory user:" />
                <uc:DropDownList ID="ddlActiveDirectoryDomain" runat="server" Label="Active Directory domain:" Visible="False"/>
                <uc:TextBox ID="txtUsername" runat="server" Label="Username:" Required="False" />
                <uc:TextBox ID="txtPassword" runat="server" Label="Password:" Required="False" TextMode="Password" />
                <uc:DropDownList ID="ddlStatus" runat="server" Label="Status:" />
                <uc:ListBoxAu ID="lbAuUserRole" runat="server" Label="User role assignments:" SelectionMode="Multiple" VisibleRowsFrom="30" VisibleRowsTo="30" AllowDoubleClick="True" />
                <uc:TextBox ID="txtLastChange" runat="server" Label="Last change:" Enabled="False" />
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
