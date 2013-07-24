<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.ProductView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeRangeBox.ascx" TagName="DateTimeRangeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/QuickLinksPopup.ascx" TagName="QuickLinksPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/ColumnsPopup.ascx" TagName="ColumnsPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxSr.ascx" TagName="ListBoxSr" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <div class="buttonsHolder">
        <div class="rightSection">
            <asp:Button ID="btnColumns" runat="server" Text="Columns" CssClass="button" Visible="True" />
            <asp:Button ID="btnSaveLayout" runat="server" Text="Save" CssClass=" SaveLay button" Visible="True" />
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
                <uc:TextBox ID="txtProductName" runat="server" Label="Name:" MaxLength="2000" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:TextBoxSr ID="txtSrPharmaceuticalProducts" runat="server" Label="Pharmaceutical product:" SearchType="PharmaceuticalProduct" />
                <uc:TextBox ID="txtProductNumber" runat="server" Label="Product number:" MaxLength="100" />
                <uc:DropDownList ID="ddlAuthorisationProcedure" runat="server" Label="Type of procedure:" />
                <uc:ListBoxSr ID="lbSrActiveSubstances" runat="server" Label="Active Substance:" SelectionMode="Single" VisibleRows="5" SearchType="Substance" Actions="Add, Remove" />
                <uc:TextBox ID="txtDrugAtcs" runat="server" Label="Drug ATCs:" />
                <uc:TextBox ID="txtClient" runat="server" Label="Client:" />
                <uc:DropDownList ID="ddlDomains" runat="server" Label="Domain:" />
                <uc:DropDownList ID="ddlType" runat="server" Label="Type:" />
                <uc:TextBox ID="txtProductId" runat="server" Label="Product ID:" MaxLength="250" />
                <uc:DropDownList ID="ddlCountry" runat="server" Label="Country:" />
                <uc:TextBoxSr ID="txtSrManufacturers" runat="server" Label="Manufacturer:" SearchType="Manufacturer" />
                <uc:TextBox ID="txtPsurCycle" runat="server" Label="PSUR cycle:" MaxLength="300" />
                <uc:RadioButtonYn ID="rbYnArticle57Reporting" runat="server" Label="Article 57 reporting:" />
                <uc:DateTimeRangeBox ID="dtRngNextDlp" runat="server" Label="Next DLP:" />

                <asp:Panel ID="pnlSearchButtons" runat="server" CssClass="pnl-search-buttons">
                    <br />
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchClick"  CssClass="button"/>&nbsp;&nbsp;
                    <asp:LinkButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClearClick"  CssClass="button"/>
                    <asp:LinkButton ID="btnSaveSearch" runat="server" Text="Save Quick link" OnClick="btnSaveSearchClick" Visible="true" CssClass="CreateQuickLink button" />
                    <asp:LinkButton ID="btnDeleteSearch" runat="server" Text="Delete Quick link" OnClick="btnDeleteSearchClick" Visible="false" CssClass="DeleteQuickLink button"/>
                    <div style="position: absolute; right: 10px">
                        <asp:LinkButton ID="btnExportLower" runat="server" Text="Export" Style="margin-top: -20px" Visible="False"  CssClass="button"/>
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
        <possGrid:PossGrid ID="ProductGrid" GridId="ProductGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="product_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="ProductName" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="product_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="16%" Caption="Product" FieldName="ProductName" FilterType="Text">
                    <ItemTemplate>
                       <asp:HyperLink ID="hlId" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ProductView/Preview.aspx?EntityContext=Product&idProd=" + Eval("product_PK")  %>'
                                Text='<%# HandleMissing(Eval("ProductName")) %>' Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="8%" Caption="Client" FieldName="Client" FilterType="Text" />
                <possGrid:PossTemplateField Width="5%" ItemStyle-HorizontalAlign="Right" Caption="Auth. product" FieldName="AuthProdCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlPreviewAuthProd" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/AuthorisedProductView/List.aspx?EntityContext=Product&idProd=" +  Eval("product_PK") %>' Text='<%# Eval("AuthProdCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewAuthProd" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/AuthorisedProductView/Form.aspx?EntityContext=Product&Action=New&idProd=" + Eval("product_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="10%" Caption="Product number" FieldName="product_number" FilterType="Text" />
                <possGrid:PossTemplateField Width="25%" Caption="Active substance (Form)" FieldName="ActiveSubstances" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID='pnlActiveSubstances' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="10%" Caption="Authorisation procedure" FieldName="AuthorisationProcedure" FilterType="Combo" />
                <possGrid:PossBoundField Width="7%" Caption="Countries" FieldName="Countries" FilterType="Text" />
                <possGrid:PossTemplateField Width="5%" ItemStyle-HorizontalAlign="Right" Caption="Pharm. product" FieldName="PharProdCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlPreviewPharProd" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/PharmaceuticalProductView/List.aspx?EntityContext=Product&idProd=" +  Eval("product_PK") %>' Text='<%# Eval("PharProdCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewPharmProd" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/PharmaceuticalProductView/Form.aspx?EntityContext=Product&Action=New&idProd=" + Eval("product_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="4%" ItemStyle-HorizontalAlign="Right" Caption="S. Units" FieldName="SubUnitCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlPreviewSubUnit" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/SubmissionUnitView/List.aspx?EntityContext=Product&idProd=" +  Eval("product_PK") %>' Text='<%# Eval("SubUnitCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewSubUnit" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/SubmissionUnitView/Form.aspx?EntityContext=Product&Action=New&idProd=" + Eval("product_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="6%" ItemStyle-HorizontalAlign="Right" Caption="Activities" FieldName="ActCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlPreviewAct" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ActivityView/List.aspx?EntityContext=Product&idProd=" +  Eval("product_PK") %>' Text='<%# Eval("ActCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewActivity" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ActivityView/Form.aspx?EntityContext=Product&Action=New&idProd=" + Eval("product_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="4%" ItemStyle-HorizontalAlign="Right" Caption="Docs" FieldName="DocCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlPreviewDoc" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/List.aspx?EntityContext=Product&idProd=" +  Eval("product_PK") %>' Text='<%# Eval("DocCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewDocument" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/Form.aspx?EntityContext=Product&Action=New&idProd=" + Eval("product_PK") %>' Text="Add"></asp:HyperLink>
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
