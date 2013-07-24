<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.ProductView.Form" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeBox.ascx" TagName="DateTimeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxSr.ascx" TagName="ListBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxExt.ascx" TagName="ListBoxExt" TagPrefix="uc" %>

<%@ Register Src="../Shared/UserControl/Popup/ManufacturerPopup.ascx" TagName="ManufacturerPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/PartnerPopup.ascx" TagName="PartnerPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Reminder.ascx" TagName="Reminder" TagPrefix="uc" %>

<%@ Register Src="../Shared/Form/PharmaceuticalProductForm.ascx" TagName="PharmaceuticalProductForm" TagPrefix="form" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:ManufacturerPopup ID="ManufacturerPopup" runat="server" />
    <uc:PartnerPopup ID="PartnerPopup" runat="server" />
    <uc:Reminder ID="Reminder" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvProduct" runat="server" Label="Product:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>

    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose" >
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:TextBox ID="txtProductName" runat="server" Label="Name:" Required="True" MaxLength="2000" />
                <uc:TextBox ID="txtDescription" runat="server" Label="Description:" MaxLength="50" TextMode="MultiLine" Rows="5" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:ListBoxSr ID="lbSrPharmaceuticalProducts" runat="server" Label="Pharmaceutical products:" Required="True" SelectionMode="Single" VisibleRows="5" SearchType="PharmaceuticalProduct" Actions="New, Add, Remove" />
                <uc:TextBox ID="txtProductNumber" runat="server" Label="Product number (Procedure number):" MaxLength="100" />
                <uc:DropDownList ID="ddlAuthorisationProcedure" runat="server" Label="Authorisation procedure:" />
                <uc:ListBoxSr ID="lbSrDrugAtcs" runat="server" Label="Drug ATCs:" SelectionMode="Single" VisibleRows="5" SearchType="Atc" Actions="Add, Remove" />
                <uc:RadioButtonYn runat="server" ID="rbYnOrphanDrug" Label="Orphan drug:" />
                <uc:RadioButtonYn runat="server" ID="rbYnIntensiveMonitoring" Label="Intensive monitoring:" />
                <uc:DropDownList ID="ddlClient" runat="server" Label="Client:" Required="True" />
                <uc:DropDownList ID="ddlClientGroup" runat="server" Label="Client group:" />
                <uc:ListBoxAu ID="lbAuDomains" runat="server" Label="Domains:" Required="True" SelectionMode="Multiple" VisibleRowsFrom="5" VisibleRowsTo="5" AllowDoubleClick="True" />
                <uc:DropDownList ID="ddlType" runat="server" Label="Type:" />
                <uc:TextBox ID="txtProductId" runat="server" Label="Product ID:" MaxLength="250" />
                <uc:ListBoxAu ID="lbAuCountries" runat="server" Label="Countries:" Required="True" SelectionMode="Multiple" VisibleRowsFrom="5" VisibleRowsTo="5" AllowDoubleClick="True" />
                <uc:DropDownList ID="ddlRegion" runat="server" Label="Region:" />
                <uc:ListBoxExt ID="lbExtManufacturers" runat="server" Label="Manufacturer:" SelectionMode="Single" VisibleRows="5" Actions="Add, Edit, Remove" />
                <uc:ListBoxExt ID="lbExtPartners" runat="server" Label="Partner:" SelectionMode="Single" VisibleRows="5" Actions="Add, Edit, Remove" />
                <uc:TextBox ID="txtComment" runat="server" Label="Comment:" MaxLength="300" TextMode="MultiLine" Rows="5" />
                <uc:TextBox ID="txtPsurCycle" runat="server" Label="PSUR cycle:" MaxLength="300" />
                <uc:DateTimeBox ID="dtNextDlp" runat="server" Label="Next DLP:" MaxLength="100" ShowReminder="True" CssClass="row CreateAjaxToolbox" />
                <uc:TextBox ID="txtBatchSize" runat="server" Label="Batch size:" MaxLength="500" TextMode="MultiLine" Rows="3"/>
                <uc:TextBox ID="txtPackSize" runat="server" Label="Pack size (approved by procedure):" MaxLength="500" TextMode="MultiLine" Rows="3"/>
                <uc:DropDownList ID="ddlStorageConditions" runat="server" Label="Storage conditions:" />
                <uc:ListBoxAu ID="lbAuPackagingMaterials" runat="server" Label="Packaging materials:" SelectionMode="Multiple" VisibleRowsFrom="5" VisibleRowsTo="5" AllowDoubleClick="True" />
            </fieldset>
        </div>
    </asp:Panel>
    
    <form:PharmaceuticalProductForm ID="PharmaceuticalProductForm" runat="server" Visible="False" />

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

