<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.PharmaceuticalProductView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/QuickLinksPopup.ascx" TagName="QuickLinksPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/ColumnsPopup.ascx" TagName="ColumnsPopup" TagPrefix="uc" %>

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
                <uc:TextBoxSr ID="txtSrProduct" runat="server" Label="Product:" SearchType="Product" />
                <uc:TextBox ID="txtPharmaceuticalProductName" runat="server" Label="Name:" MaxLength="450" LabelWidth="150px" TextWidth="330px" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" LabelWidth="150px" TextWidth="336px" />
                <uc:TextBox ID="txtDescription" runat="server" Label="Description:" MaxLength="50" LabelWidth="150px" TextWidth="330px" TextMode="SingleLine" />
                <uc:DropDownList ID="ddlPharmaceuticalForm" runat="server" Label="Pharmaceutical form:" LabelWidth="150px" TextWidth="336px" />
                <uc:TextBox ID="txtAdministrationRoutes" runat="server" Label="Administration routes:" MaxLength="450" LabelWidth="150px" TextWidth="330px" />
                <uc:TextBox ID="txtActiveIngredients" runat="server" Label="Active ingredients:" MaxLength="450" LabelWidth="150px" TextWidth="330px" />
                <uc:TextBox ID="txtExcipients" runat="server" Label="Excipients:" MaxLength="450" LabelWidth="150px" TextWidth="330px" />
                <uc:TextBox ID="txtAdjuvants" runat="server" Label="Adjuvants:" MaxLength="450" LabelWidth="150px" TextWidth="330px" />
                <uc:TextBox ID="txtMedicalDevices" runat="server" Label="Medical devices:" MaxLength="450" LabelWidth="150px" TextWidth="330px" />
                <uc:TextBox ID="txtComment" runat="server" Label="Comment:" MaxLength="50" LabelWidth="150px" TextWidth="330px" TextMode="SingleLine" />

                <asp:Panel ID="pnlSearchButtons" runat="server" CssClass="pnl-search-buttons">
                    <br />
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchClick"  CssClass="button"/>&nbsp;&nbsp;
                    <asp:LinkButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClearClick" CssClass="button" />
                    <asp:LinkButton ID="btnSaveSearch" runat="server" Text="Save Quick link" OnClick="btnSaveSearchClick" Visible="true" CssClass="CreateQuickLink button" />
                    <asp:LinkButton ID="btnDeleteSearch" runat="server" Text="Delete Quick link" OnClick="btnDeleteSearchClick" Visible="false" CssClass="DeleteQuickLink button" />
                    <div style="position: absolute; right: 10px">
                        <asp:LinkButton ID="btnExportLower" runat="server" Text="Export" Style="margin-top: -20px" Visible="False" CssClass="button" />
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
        <div class="subtabs">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <possGrid:PossGrid ID="PharmaceuticalProductGrid" GridId="PharmaceuticalProductGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="pharmaceutical_product_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="Name" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="pharmaceutical_product_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="15%" Caption="Name" FieldName="Name" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlId" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext=PharmaceuticalProduct&idPharmProd=" + Eval("pharmaceutical_product_PK")  %>'
                                Text='<%# HandleMissing(Eval("Name")) %>' Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="5%" Caption="ID" FieldName="ID" FilterType="Text" />
                <possGrid:PossTemplateField Width="17%" Caption="Products" FieldName="Products" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID='pnlProducts' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="10%" Caption="Pharmaceutical form" FieldName="PharmaceuticalForm" FilterType="Text" />
                <possGrid:PossBoundField Width="9%" Caption="Administration routes" FieldName="AdministrationRoutes" FilterType="Text" />
                <possGrid:PossTemplateField Width="15%" Caption="Active ingredients" FieldName="ActiveSubstances" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID='pnlActiveSubstances' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="10%" Caption="Excipients" FieldName="Excipients" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID='pnlExcipients' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="15%" Caption="Adjuvants" FieldName="Adjuvants" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID='pnlAdjuvants' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="4%" ItemStyle-HorizontalAlign="Right" Caption="Docs" FieldName="DocCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlPreviewDoc" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/List.aspx?EntityContext=PharmaceuticalProduct&idPharmProd=" +  Eval("pharmaceutical_product_PK") %>' Text='<%# Eval("DocCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewDocument" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/Form.aspx?EntityContext=PharmaceuticalProduct&Action=New&idPharmProd=" + Eval("pharmaceutical_product_PK") %>' Text="Add"></asp:HyperLink>
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

