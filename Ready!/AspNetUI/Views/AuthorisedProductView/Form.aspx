<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.AuthorisedProductView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeBox.ascx" TagName="DateTimeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Reminder.ascx" TagName="Reminder" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/IndicationsPopup.ascx" TagName="IndicationsPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxExt.ascx" TagName="ListBoxExt" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:IndicationsPopup ID="IndicationsPopup" runat="server" />
    <uc:Reminder ID="Reminder" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Authorised product:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:TextBoxSr ID="txtSrRelatedProduct" runat="server" Label="Related product:" Required="True" SearchType="Product" /><br /><br />
                <uc:RadioButtonYn ID="rbYnArticle57Reporting" runat="server" Label="Article 57 reporting:" AutoPostback="True" />
                <uc:RadioButtonYn ID="rbYnSubstanceTranslations" runat="server" Label="Substance translations:" />
                <uc:TextBox ID="txtEvcode" runat="server" Label="EVCODE:" MaxLength="2000" Enabled="False" />
                <uc:TextBox ID="txtXEvprmStatus" runat="server" Label="xEVPRM status:" MaxLength="2000" Enabled="False" />
                <uc:TextBox ID="txtFullPresentationName" runat="server" Label="Full presentation name:" Required="True" MaxLength="2000" />
                <uc:TextBox ID="txtProductShortName" runat="server" Label="Product short name:" MaxLength="2000" />
                <uc:TextBox ID="txtProductGenericName" runat="server" Label="Product generic name:" MaxLength="2000" />
                <uc:TextBox ID="txtProductCompanyName" runat="server" Label="Product company name:" MaxLength="2000" />
                <uc:TextBox ID="txtProductStrengthName" runat="server" Label="Product strength name:" MaxLength="2000" />
                <uc:TextBox ID="txtProductFormName" runat="server" Label="Product form name:" MaxLength="2000" />
                <uc:TextBox ID="txtPackageDescription" runat="server" Label="Package description:" MaxLength="2000" />
                <uc:TextBox ID="txtCommentEvprm" runat="server" Label="Comment (EVPRM):" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:TextBox ID="txtDescription" runat="server" Label="Description:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:TextBox ID="txtAuthorisedProductId" runat="server" Label="Authorised product ID:" MaxLength="2000" />
                <uc:TextBox ID="txtShelfLife" runat="server" Label="Shelf life:" MaxLength="2000" />
                <uc:RadioButtonYn runat="server" ID="rbYnMarketed" Label="Marketed:" />
                <uc:RadioButtonYn runat="server" ID="rbYnReimbursmentStatus" Label="Reimbursment status:" />
                <uc:RadioButtonYn runat="server" ID="rbYnReservationConfirmed" Label="Reservation confirmed:" />
                <uc:DropDownList ID="ddlLegalStatus" runat="server" Label="Legal status:" />
            </fieldset>
        </div>
        <div id="divRightPane" class="rightPane" style="margin-top: 0px; margin-left: -74px;">
            <fieldset>
                <uc:DropDownList ID="ddlLicenceHolder" runat="server" Label="Licence holder:" />
                <uc:DropDownList ID="ddlLicenceHolderGroup" runat="server" Label="Licence holder group:" />
                <uc:DropDownList ID="ddlLocalRepresentative" runat="server" Label="Local representative:" />
                <uc:TextBoxSr ID="txtSrQppv" runat="server" Label="QPPV:" SearchType="Qppv" />
                <uc:TextBoxSr ID="txtSrLocalQppv" runat="server" Label="Local PV Contact:" SearchType ="LocalQPPV" />
                <uc:DropDownList ID="ddlMasterFileLocation" runat="server" Label="Master File Location:" />
                <uc:TextBox ID="txtPhVEmail" runat="server" Label="PhV EMail:" />
                <uc:TextBox ID="txtPhVPhone" runat="server" Label="PhV Phone:" />
                <uc:ListBoxExt ID="lbExtIndications" runat="server" Label="Indications:" SelectionMode="Single" VisibleRows="5" Actions="Add, Edit, Remove" />
                <uc:DropDownList ID="ddlAuthorisationCountry" runat="server" Label="Authorisation country:" Required="True" />
                <uc:DropDownList ID="ddlAuthorisationStatus" runat="server" Label="Authorisation status:" AutoPostback="True" />
                <uc:TextBox ID="txtAuthorisationNumber" runat="server" Label="Authorisation number:" />
                <uc:TextBox ID="txtComment" runat="server" Label="Comment:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:DateTimeBox ID="dtAuthorisationDate" runat="server" Label="Authorisation date:" MaxLength="100" ShowReminder="True" />
                <uc:DateTimeBox ID="dtAuthorisationExpiryDate" runat="server" Label="Authorisation expiry date:" MaxLength="100" ShowReminder="True" />
                <uc:DateTimeBox ID="dtLaunchDate" runat="server" Label="Launch date:" MaxLength="100" ErrorDisplayInline="True" ShowReminder="True" />
                <uc:DateTimeBox ID="dtWithdrawnDate" runat="server" Label="Withdrawn date:" MaxLength="100" ShowReminder="True" />
                <uc:DateTimeBox ID="dtInfoDate" runat="server" Label="Info date:" MaxLength="100" />
                <uc:DateTimeBox ID="dtSunsetClause" runat="server" Label="Sunset clause:" MaxLength="100" ShowReminder="True"/>
                <uc:TextBox ID="txtReservedTo" runat="server" Label="Reserved to:" />
                <uc:TextBox ID="txtLocalCodes" runat="server" Label="Local codes:" />
                <uc:TextBox ID="txtPackSize" runat="server" Label="Pack size (approved national):" />
            </fieldset>
        </div>
        <div id="bottomPane" class="bottomPane" style="clear: both;">
            <fieldset>
                <uc:ListBoxAu ID="lbAuDistributors" runat="server" Label="Distributor Assignments:" SelectionMode="Multiple" VisibleRowsFrom="5" VisibleRowsTo="5" AllowDoubleClick="True" />
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
                __doPostBack($(this).attr("id"), lbInputDblClick.attr('name'));
            });
        });
    </script>
</asp:Content>
