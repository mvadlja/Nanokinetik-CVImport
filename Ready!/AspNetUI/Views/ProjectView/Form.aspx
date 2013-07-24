<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.ProjectView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeBox.ascx" TagName="DateTimeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Reminder.ascx" TagName="Reminder" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:Reminder ID="Reminder" runat="server" />
    <uc:ModalPopup ID="mpConfirmInternalStatus" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvProject" runat="server" Label="Project:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:TextBox ID="txtName" runat="server" Label="Name:" Required="True"/>
                <uc:TextBox ID="txtDescription" runat="server" Label="Description:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:DropDownList ID="ddlInternalStatus" runat="server" Label="Internal status:" />
                <uc:ListBoxAu ID="lbAuCountries" runat="server" Label="Countries:" SelectionMode="Multiple" VisibleRowsFrom="5" VisibleRowsTo="5" AllowDoubleClick="True" />
                <uc:TextBox ID="txtComment" runat="server" Label="Comment:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:DateTimeBox ID="dtStartDate" runat="server" Label="Start date:" MaxLength="100" ErrorDisplayInline="True" ShowReminder="True" />
                <uc:DateTimeBox ID="dtExpectedFinishedDate" runat="server" Label="Expected finished date:" MaxLength="100" ErrorDisplayInline="True" ShowReminder="True" />
                <uc:DateTimeBox ID="dtActualFinishedDate" runat="server" Label="Actual finished date:" MaxLength="100" ErrorDisplayInline="True" ShowReminder="False" />
                <uc:RadioButtonYn ID ="rbYnAutomaticAlerts" runat="server" Label="Automatic alerts:" />
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
