<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Preview.aspx.cs" Inherits="AspNetUI.Views.AuthorisedProductView.Preview" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Reminder.ascx" TagName="Reminder" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:Reminder ID="Reminder" runat="server" />
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvAuthorisedProduct" runat="server" Label="Authorised product:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel runat="server" ID="pnlPreview">
    <asp:Panel ID="pnlProperties" runat="server" class="preview" CssClass="properties">
        <div id="linkedEntities" class="linkedEntities">
            <uc:LabelPreview ID="lblPrvRelatedProductName" runat="server" Label="Related product:" Required="True" TextWidth="400px"  TextBold="True" />
        </div>
        <div id="divLeftPane" class="leftPane">
            <uc:LabelPreview ID="lblPrvArticle57Reporting" runat="server" Label="Article 57 reporting:" />
            <uc:LabelPreview ID="lblPrvSubstanceTranslations" runat="server" Label="Substance translations:" />
            <uc:LabelPreview ID="lblPrvEvcode" runat="server" Label="EVCODE:" />
            <uc:LabelPreview ID="lblPrvXevprmStatus" runat="server" Label="xEVPRM status:" />
            <div class="spacer"></div>
            <uc:LabelPreview ID="lblPrvFullPresentationName" runat="server" Label="Full presentation name:" Required="true" />
            <uc:LabelPreview ID="lblPrvProductShortName" runat="server" Label="Product short name:" />
            <uc:LabelPreview ID="lblPrvProductGenericName" runat="server" Label="Product generic name:" />
            <uc:LabelPreview ID="lblPrvProductCompanyName" runat="server" Label="Product company name:" />
            <uc:LabelPreview ID="lblPrvProductStrengthName" runat="server" Label="Product strength name:" />
            <uc:LabelPreview ID="lblPrvProductFormName" runat="server" Label="Product form name:" />
            <uc:LabelPreview ID="lblPrvPackageDescription" runat="server" Label="Package description:" />
            <uc:LabelPreview ID="lblPrvCommentEVPRM" runat="server" Label="Comment (EVPRM):" />
            <div class="spacer"></div>
            <uc:LabelPreview ID="lblPrvResponsibleUser" runat="server" Label="Responsible user:" />
            <uc:LabelPreview ID="lblPrvDescription" runat="server" Label="Description:" />
            <uc:LabelPreview ID="lblPrvAuthorisedProductID" runat="server" Label="Authorised product ID:" />
            <div class="spacer"></div>
            <uc:LabelPreview ID="lblPrvDistributors" runat="server" Label="Distributor Assignments:" />
            <uc:LabelPreview ID="lblPrvShelfLife" runat="server" Label="Shelf life:" />
            <uc:LabelPreview ID="lblPrvMarketed" runat="server" Label="Marketed:" />
            <uc:LabelPreview ID="lblPrvReimbursmentStatus" runat="server" Label="Reimbursment status:" />
            <uc:LabelPreview ID="lblPrvReservationConfirmed" runat="server" Label="Reservation confirmed:" />
            <uc:LabelPreview ID="lblPrvLegalStatus" runat="server" Label="Legal status:" />
        </div>
        <div id="divRightPane" class="rightPane">
            <uc:LabelPreview ID="lblPrvLicenceHolder" runat="server" Label="Licence holder:" />
            <uc:LabelPreview ID="lblPrvLicenceHolderGroup" runat="server" Label="Licence holder group:" />
            <uc:LabelPreview ID="lblPrvLocalRepresentative" runat="server" Label="Local representative:" />
            <uc:LabelPreview ID="lblPrvQppv" runat="server" Label="QPPV:" />
            <uc:LabelPreview ID="lblPrvLocalQppv" runat="server" Label="Local PV Contact:" />
            <uc:LabelPreview ID="lblPrvMasterFileLocation" runat="server" Label="Master File Location:" />
            <uc:LabelPreview ID="lblPrvPhVEmail" runat="server" Label="PhV EMail:" />
            <uc:LabelPreview ID="lblPrvPhVPhone" runat="server" Label="PhV Phone:" />
            <uc:LabelPreview ID="lblPrvIndications" runat="server" Label="Indications:" />
            <div class="spacer"></div>
            <uc:LabelPreview ID="lblPrvAuthorisationCountry" runat="server" Label="Authorisation country:" Required="true" />
            <uc:LabelPreview ID="lblPrvAuthorisationStatus" runat="server" Label="Authorisation status:" />
            <uc:LabelPreview ID="lblPrvAuthorisationNumber" runat="server" Label="Authorisation number:" />
            <uc:LabelPreview ID="lblPrvComment" runat="server" Label="Comment:" />
            <div class="spacer"></div>
            <uc:LabelPreview ID="lblPrvAuthorisationDate" runat="server" Label="Authorisation date:" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvAuthorisationExpiryDate" runat="server" Label="Authorisation expiry date:" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvLaunchDate" runat="server" Label="Launch date:" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvWithdrawnDate" runat="server" Label="Withdrawn date:" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvInfoDate" runat="server" Label="Info date:" />
            <uc:LabelPreview ID="lblPrvSunsetClause" runat="server" Label="Sunset clause:" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvReservedTo" runat="server" Label="Reserved to:" />
            <uc:LabelPreview ID="lblPrvLocalCodes" runat="server" Label="Local codes:" />
            <uc:LabelPreview ID="lblPrvPackSize" runat="server" Label="Pack size (approved national):" />
            <div class="spacer"></div>
            <uc:LabelPreview ID="lblPrvPpiAttachment" runat="server" Label="PPI Attachment:" />
            <div class="spacer"></div>
            <uc:LabelPreview ID="lblPrvLastChange" runat="server" Label="Last change:" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlFooter" runat="server" class="previewBottom clear">
        <asp:LinkButton ID="btnAddDocument" runat="server" Text="Add document" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="button Delete marginLeft"></asp:LinkButton>
    </asp:Panel>
    </asp:Panel>
</asp:Content>
