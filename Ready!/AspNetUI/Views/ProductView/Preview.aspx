<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="False" CodeBehind="Preview.aspx.cs" Inherits="AspNetUI.Views.ProductView.Preview" %>

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
        <uc:LabelPreview ID="lblPrvProduct" runat="server" Label="Product:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel runat="server" ID="pnlPreview">
    <asp:Panel id="pnlProperties" runat="server" CssClass="properties">
        <div id="divPropertiesLeftColumn" class="leftPane">
            <uc:LabelPreview ID="lblPrvProductName" runat="server" Label="Name:" Required="True" />
            <uc:LabelPreview ID="lblPrvDescription" runat="server" Label="Description:" />
            <uc:LabelPreview ID="lblPrvResponsibleUser" runat="server" Label="Responsible user:" />
            <uc:LabelPreview ID="lblPrvPharmaceuticalProducts" runat="server" Label="Pharmaceutical products:" Required="True" TextBold="True" />
            <uc:LabelPreview ID="lblPrvProductNumber" runat="server" Label="Product number (Procedure number):" />
            <uc:LabelPreview ID="lblPrvAuthorisationProcedure" runat="server" Label="Authorisation procedure:" />
            <uc:LabelPreview ID="lblPrvDrugAtcs" runat="server" Label="Drug ATCs:" />
            <uc:LabelPreview ID="lblPrvOrphanDrug" runat="server" Label="Orphan drug:" />
            <uc:LabelPreview ID="lblPrvIntesiveMonitoring" runat="server" Label="Intensive monitoring:" />
            <uc:LabelPreview ID="lblPrvClient" runat="server" Label="Client:" Required="True" />
            <uc:LabelPreview ID="lblPrvClientGroup" runat="server" Label="Client group:" />
            <uc:LabelPreview ID="lblPrvDomains" runat="server" Label="Domains:" Required="True" />
            <uc:LabelPreview ID="lblPrvType" runat="server" Label="Type:" />
            <uc:LabelPreview ID="lblPrvProductId" runat="server" Label="Product ID:" />
            <uc:LabelPreview ID="lblPrvCountries" runat="server" Label="Countries:" Required="True" />
            <uc:LabelPreview ID="lblPrvRegion" runat="server" Label="Region:" />
            <uc:LabelPreview ID="lblPrvManufacturer" runat="server" Label="Manufacturer:" />
            <uc:LabelPreview ID="lblPrvPartner" runat="server" Label="Partner:" />
            <uc:LabelPreview ID="lblPrvComment" runat="server" Label="Comment:" />
            <uc:LabelPreview ID="lblPrvPsurCycle" runat="server" Label="PSUR cycle:" />
        </div>
        <div id="divPropertiesRightColumn" class="rightPane">
            <uc:LabelPreview ID="lblPrvNextDlp" runat="server" Label="Next DLP:" ShowReminder="True"/>
            <uc:LabelPreview ID="lblPrvBatchSize" runat="server" Label="Batch size:" />
            <uc:LabelPreview ID="lblPrvPackSize" runat="server" Label="Pack size (approved by procedure):" />
            <uc:LabelPreview ID="lblPrvStorageConditions" runat="server" Label="Storage conditions:" />
            <uc:LabelPreview ID="lblPrvPackagingMaterials" runat="server" Label="Packaging materials:" />
            <uc:LabelPreview ID="lblPrvLastChange" runat="server" Label="Last change:" />
        </div>
        <br/>
    </asp:Panel>
    <asp:Panel id="pnlFooter" runat="server" class="previewBottom clear">
        <asp:LinkButton ID="btnAddDocument" runat="server" Text="Add document" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnAddAuthorisedProduct" runat="server" Text="Add authorised product" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnAddActivity" runat="server" Text="Add activity" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnAddPharmaceuticalProduct" runat="server" Text="Add pharmaceutical product" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnAddSubmissionUnit" runat="server" Text="Add submission unit" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="button Delete marginLeft" OnClick="btnDelete_OnClick"></asp:LinkButton>
    </asp:Panel>
    </asp:Panel>
</asp:Content>
