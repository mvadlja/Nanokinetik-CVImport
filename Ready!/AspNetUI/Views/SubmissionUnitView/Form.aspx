<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.SubmissionUnitView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeBox.ascx" TagName="DateTimeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxSr.ascx" TagName="ListBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/UploadPanel.ascx" TagName="UploadPanel" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <uc:ModalPopup ID="mpSequenceUploadError" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Submission unit:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:ListBoxSr ID="lbSrProducts" runat="server" Label="Products:" Required="True" SearchType="Product" Actions="Add, Remove"/>
                <uc:DropDownList ID="ddlActivity" runat="server" Label="Activity:" AutoPostback="True" Required="True" />
                <uc:DropDownList ID="ddlTask" runat="server" Label="Task:" Required="True" />
                <uc:DropDownList ID="ddlSubmissionDescription" runat="server" Label="Submission description:" Required="True" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:ListBoxAu ID="lbAuAgencies" runat="server" Label="Agencies:" SelectionMode="Multiple" VisibleRowsFrom="5" VisibleRowsTo="5" Required="True" />
                <uc:DropDownList ID="ddlRms" runat="server" Label="RMS:" Visible="False" />
                <uc:TextBox ID="txtSubmissionId" runat="server" Label="Submission ID:" />
                <uc:DropDownList ID="ddlSubmissionFormat" runat="server" Label="Submission format:" AutoPostback="True" />
                <uc:TextBox ID="txtSequence" runat="server" Label="Sequence:" />
                <uc:TextBox ID="txtComment" runat="server" Label="Comment:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:UploadPanel ID="upSubmissionFormatSequenceType" runat="server" Label="Sequence:" />
                <uc:UploadPanel ID="upSubmissionFormatSequenceTypeWorking" runat="server" Label="Working:" />
                <uc:DateTimeBox ID="dtDispatchDate" runat="server" Label="Dispatch date:" MaxLength="100"  />
                <uc:DateTimeBox ID="dtReceiptDate" runat="server" Label="Receipt date:" MaxLength="100"  />
                <uc:UploadPanel ID="upAttachments" runat="server" Label="Attachments:" />
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
