<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.AuthorisedProductView.List" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeRangeBox.ascx" TagName="DateTimeRangeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/QuickLinksPopup.ascx" TagName="QuickLinksPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/ColumnsPopup.ascx" TagName="ColumnsPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content runat="server" ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <div class="buttonsHolder">
        <div class="rightSection">
            <asp:Button ID="btnColumns" runat="server" Text="Columns" CssClass="button" Visible="True" />
            <asp:Button ID="btnSaveLayout" runat="server" Text="Save" CssClass="SaveLay button" Visible="True" />
            <asp:Button ID="btnClearLayout" runat="server" Text="Clear" CssClass="LoadLay button" Visible="True" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="ResetLay button" Visible="True" />
            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Export button" Style="margin-left: 25px;" />
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlSearch" Visible="True">
        <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <uc:QuickLinksPopup ID="QuickLinksPopup" runat="server" />
                <uc:TextBoxSr ID="txtSrRelatedProduct" runat="server" Label="Related product:" SearchType="Product" />
                <uc:RadioButtonYn ID="rbYnArticle57Reporting" runat="server" Label="Article 57 reporting:" />
                <uc:RadioButtonYn ID="rbYnSubstanceTranslations" runat="server" Label="Substance translations:" />
                <uc:TextBox ID="txtEvcode" runat="server" Label="EVCODE:" MaxLength="2000" />
                <uc:TextBox ID="txtFullPresentationName" runat="server" Label="Full presentation name:" MaxLength="2000" />
                <uc:TextBox ID="txtPackageDescription" runat="server" Label="Package description:" MaxLength="2000" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:RadioButtonYn runat="server" ID="rbYnMarketed" Label="Marketed:" />
                <uc:DropDownList ID="ddlLegalStatus" runat="server" Label="Legal status:" />
                <uc:TextBoxSr ID="txtSrLicenceHolder" runat="server" Label="Licence holder:" SearchType="LicenceHolder" />
                <uc:DropDownList ID="ddlLocalRepresentative" runat="server" Label="Local representative:" />
                <uc:DropDownList ID="ddlQppv" runat="server" Label="QPPV:" />
                <uc:DropDownList ID="ddlLocalQppv" runat="server" Label="Local PV Contact:" />
                <uc:DropDownList ID="ddlMasterFileLocation" runat="server" Label="PSMF Location:" />
                <uc:TextBox ID="txtIndications" runat="server" Label="Indications:" />
                <uc:DropDownList ID="ddlAuthorisationCountry" runat="server" Label="Country:" />
                <uc:DropDownList ID="ddlAuthorisationStatus" runat="server" Label="Authorisation status:" />
                <uc:TextBoxSr ID="txtSrClient" runat="server" Label="Client:" SearchType="Client" />
                <uc:DateTimeRangeBox ID="dtRngAuthorisationDate" runat="server" Label="Authorisation date:" />
                <uc:DateTimeRangeBox ID="dtRngAuthorisationExpiryDate" runat="server" Label="Authorisation expiry date:" />
                <uc:DateTimeRangeBox ID="dtRngSunsetClause" runat="server" Label="Sunset clause" />

                <asp:Panel id="pnlSearchButtons" runat="server" CssClass="pnl-search-buttons">
                    <br />
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchClick" CssClass="button" />&nbsp;&nbsp;
                    <asp:LinkButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClearClick"  CssClass="button"/>
                    <asp:LinkButton ID="btnSaveSearch" runat="server" Text="Save Quick link" OnClick="btnSaveSearchClick" Visible="true"  CssClass="CreateQuickLink button" />
                    <asp:LinkButton ID="btnDeleteSearch" runat="server" Text="Delete Quick link" OnClick="btnDeleteSearchClick" Visible="false" CssClass="DeleteQuickLink button"/>
                    <div style="position: absolute; right: 10px">
                        <asp:LinkButton ID="btnExportLower" runat="server" Text="Export" Style="margin-top: -20px" Visible="false" CssClass="button" />
                    </div>
                  </asp:Panel>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </asp:Panel>
    <asp:Panel ID="pnlGrid" runat="server" Visible="True">
        <uc:ColumnsPopup ID="ColumnsPopup" runat="server" />
        <div class="entity-name">
            <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Product:" Visible="False" />
        </div>
        <div class="subtabs" id="subtabs" runat="server">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <possGrid:PossGrid ID="APGrid" GridId="APGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18"
            AutoGenerateColumns="false" CellPadding="4" DataKeyNames="ap_PK"
            AllowSorting="true"
            GridLines="None" GridHeight="250"
            Width="100%" MainSortingColumn="AuthorisedProductName" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="ap_PK" Visible="false" FilterType="Text" />
                <possGrid:PossTemplateField Width="11%" Caption="Authorised product" FieldName="AuthorisedProductName" FilterType="Text" CellStyleCssClass="test">
                    <ItemTemplate>
                        <div class="status">
                            <span runat="server" id="pnlStatusColorAP"></span>
                            <asp:HyperLink ID="hlId" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/AuthorisedProductView/Preview.aspx?EntityContext=AuthorisedProduct&From=AuthProd&idAuthProd=" + Eval("ap_PK")  %>'
                                Text='<%# HandleMissing(Eval("AuthorisedProductName")) %>' Font-Bold="true"></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="5%" Caption="Client" FieldName="Client" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="EVCODE" FieldName="evcode" FilterType="Text" />
                <possGrid:PossTemplateField Width="5%" Caption="Article 57 status" FieldName="XevprmStatus" FilterType="Combo">
                    <ItemTemplate>
                        <asp:Label ID='lblStatus' Font-Bold="true" Text='<%# Eval("XevprmStatus") %>' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="5%" Caption="Article 57 reporting" FieldName="Article57Relevant" FilterType="Combo" />
                <possGrid:PossTemplateField Width="7%" Caption="Package description" FieldName="PackageDescription" FilterType="Text">
                    <ItemTemplate>
                        <div style="overflow: hidden">
                            <span runat="server" id="pnlStatusColorPackageDesc"></span>
                            <asp:Literal ID="descLiteralID" runat="server" Text='<%# Eval("PackageDescription") %>'></asp:Literal>
                        </div>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="5%" Caption="Country" FieldName="Country" FilterType="Text" />
                <possGrid:PossTemplateField Width="8%" Caption="Related product" FieldName="RelatedProduct" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlViewProduct" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ProductView/Preview.aspx?EntityContext=Product&idProd="+Eval("product_FK") %>' Text='<%# Eval("RelatedProduct") %>'></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="10%" Caption="Active substances" FieldName="ActiveSubstances" Visible="false" />
                <possGrid:PossBoundField Width="7%" Caption="Auth. Status" FieldName="AuthorisationStatus" FilterType="Combo" />
                <possGrid:PossBoundField Width="7%" Caption="Auth. Number" FieldName="AuthorisationNumber" FilterType="Text" />
                <possGrid:PossBoundField Width="6%" Caption="Auth. Date" FieldName="AuthorisationDate" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" />
                <possGrid:PossBoundField Width="7%" Caption="Licence holder" FieldName="LicenceHolder" FilterType="Text" />
                <possGrid:PossBoundField Width="6%" Caption="Expiry date" FieldName="AuthorisationExpiryDate" DataFormatString="{0:dd.MM.yyyy}" ItemStyle-HorizontalAlign="Right" FilterType="Text" />                
                <possGrid:PossboundField Width="6%" Caption="Local PV Contact" Fieldname="LocalQppv" Filtertype="Text" />                
                <possGrid:PossBoundField Width="6%" Caption="PSMF location" FieldName="MasterFileLocation" FilterType="Text" />
                <possGrid:PossTemplateField Width="4%" ItemStyle-HorizontalAlign="Right" Caption="Docs" FieldName="DocumentsCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlViewDocs" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/List.aspx?EntityContext=AuthorisedProduct&idAuthProd=" +  Eval("ap_PK") %>' Text='<%# Eval("DocumentsCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewDocument" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/Form.aspx?EntityContext=AuthorisedProduct&Action=New&idAuthProd=" + Eval("ap_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />

        </possGrid:PossGrid>
    </asp:Panel>
</asp:Content>
