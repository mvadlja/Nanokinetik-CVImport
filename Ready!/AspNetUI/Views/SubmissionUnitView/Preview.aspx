<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Preview.aspx.cs" Inherits="AspNetUI.Views.SubmissionUnitView.Preview" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvSubmissionUnit" runat="server" Label="Submission unit:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel ID="pnlProperties" runat="server" class="preview" CssClass="properties padding-0">
        <div id="linkedEntities" class="linkedEntities">
            <uc:LabelPreview ID="lblPrvProducts" runat="server" Label="Products:" Required="True" TextWidth="400px" TextBold="True" />
            <uc:LabelPreview ID="lblPrvActivity" runat="server" Label="Activity:" TextWidth="400px" TextBold="True" />
            <uc:LabelPreview ID="lblPrvTask" runat="server" Label="Task:" TextWidth="400px" TextBold="True" Required="True" />
        </div>
        <div id="divLeftPane" class="leftPane">
            <uc:LabelPreview ID="lblPrvSubmissionUnitDescription" runat="server" Label="Submission description:" Required="True"/>
            <uc:LabelPreview ID="lblPrvResponsibleUser" runat="server" Label="Responsible user:" />
            <uc:LabelPreview ID="lblPrvAgencies" runat="server" Label="Agencies:" Required="True"/>
            <uc:LabelPreview ID="lblPrvRms" runat="server" Label="RMS:" />
            <uc:LabelPreview ID="lblPrvSubmissionId" runat="server" Label="Submission ID:" />
            <uc:LabelPreview ID="lblPrvSubmissionFormat" runat="server" Label="Submission format:" />
            <uc:LabelPreview ID="lblPrvSequence" runat="server" Label="Sequence:"/>
            <uc:LabelPreview ID="lblPrvComment" runat="server" Label="Comment:" />
            <uc:LabelPreview ID="lblPrvDispatchDate" runat="server" Label="Dispatch date:" />
            <uc:LabelPreview ID="lblPrvReceiptDate" runat="server" Label="Receipt date:" />
            <uc:LabelPreview ID="lblPrvSubmissionTypeAttachments" runat="server" Label="NeeS attachments:" />
            <uc:LabelPreview ID="lblPrvAttachments" runat="server" Label="Attachments:" />
            <uc:LabelPreview ID="lblPrvLastChange" runat="server" Label="Last change:" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlFooter" runat="server" class="previewBottom clear">
        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="button Delete"></asp:LinkButton>
    </asp:Panel>
</asp:Content>
