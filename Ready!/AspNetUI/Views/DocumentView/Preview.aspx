<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="False" CodeBehind="Preview.aspx.cs" Inherits="AspNetUI.Views.DocumentView.Preview" %>

<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>

<%@ Register Src="../Shared/UserControl/Reminder.ascx" TagName="Reminder" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:Reminder ID="Reminder" runat="server" />
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Document:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel ID="pnlProperties" runat="server" CssClass="properties padding-0">
        <div id="linkedEntities" class="linkedEntities">
            <uc:LabelPreview ID="lblPrvRelatedEntityName" runat="server" Label="Related entity:" TextWidth="400px" TextBold="True" Required="True" />
        </div>
        <div id="divLeftPane" class="leftPane">
            <uc:LabelPreview ID="lblPrvDocumentName" runat="server" Label="Document name:" Required="True" />
            <uc:LabelPreview ID="lblPrvDescription" runat="server" Label="Description:" />
            <uc:LabelPreview ID="lblPrvDocumentType" runat="server" Label="Document type:" Required="True" />
            <uc:LabelPreview ID="lblPrvVersionNumber" runat="server" Label="Version number:" Required="True" />
            <uc:LabelPreview ID="lblPrvResponsibleUser" runat="server" Label="Responsible user:" Required="True" />
            <uc:LabelPreview ID="lblPrvVersionLabel" runat="server" Label="Version label:" Required="True" />
            <uc:LabelPreview ID="lblPrvDocumentNumber" runat="server" Label="Document number:" />
            <uc:LabelPreview ID="lblPrvRegulatoryStatus" runat="server" Label="Regulatory status:" Required="True" />
            <uc:LabelPreview ID="lblPrvComment" runat="server" Label="Comment:" />
            <uc:LabelPreview ID="lblPrvAttachmentType" runat="server" Label="Attachment type:" />
            <uc:LabelPreview ID="lblPrvLanguageCode" runat="server" Label="Language code:" />
            <uc:LabelPreview ID="lblPrvEDMSBindingRule" runat="server" Label="Binding rule:" />
            <uc:LabelPreview ID="lblPrvAttachments" runat="server" Label="Attachment(s):" Required="True" TextBold="True" />
        </div>
        <div id="divRightPane" class="rightPane">
            <uc:LabelPreview ID="lblPrvChangeDate" runat="server" Label="Change date:" />
            <uc:LabelPreview ID="lblPrvEffectiveStartDate" runat="server" Label="Effective start date:" ShowReminder="True" />
            <uc:LabelPreview ID="lblPrvEffectiveEndDate" runat="server" Label="Effective end date:" ShowReminder="True" />
            <uc:LabelPreview ID="lblPrvEDMSModifyDate" runat="server" Label="Modify date:" />
            <uc:LabelPreview ID="lblPrvVersionDate" runat="server" Label="Version date:" />
            <uc:LabelPreview ID="lblPrvEvcode" runat="server" Label="EVCODE:" />
            <uc:LabelPreview ID="lblPrvLastChange" runat="server" Label="Last change:" />
        </div>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlFooter" runat="server" class="previewBottom clear">
        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="button Delete" OnClick="btnDelete_OnClick"></asp:LinkButton>
    </asp:Panel>
</asp:Content>
