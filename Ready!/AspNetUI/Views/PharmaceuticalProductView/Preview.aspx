<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="False" CodeBehind="Preview.aspx.cs" Inherits="AspNetUI.Views.PharmaceuticalProductView.Preview" %>

<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>

<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvPharmaceuticalProduct" runat="server" Label="Pharmaceutical product:" LabelWidth="50px" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel runat="server" ID="pnlPreview">
        <asp:Panel ID="pnlProperties" runat="server" CssClass="properties">
            <div id="divLinkedEntities" runat="server" class="linkedEntities">
                <uc:LabelPreview ID="lblPrvProducts" runat="server" Label="Products:" TextWidth="400px" />
            </div>
            <div id="divPropertiesLeftColumn" class="leftPane">
                <uc:LabelPreview ID="lblPrvPharmaceuticalProductName" runat="server" Label="Name:" />
                <uc:LabelPreview ID="lblPrvResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:LabelPreview ID="lblPrvDescription" runat="server" Label="Description:" />
                <uc:LabelPreview ID="lblPrvPharmaceuticalForm" runat="server" Label="Pharmaceutical form:" Required="True" />
                <uc:LabelPreview ID="lblPrvAdministrationRoutes" runat="server" Label="Administration routes:" />
                <%--<uc:LabelPreview ID="lblPrvActiveIngredients" runat="server" Label="Active ingredients:" Required="True" />
            <uc:LabelPreview ID="lblPrvExcipients" runat="server" Label="Excipients:" />
            <uc:LabelPreview ID="lblPrvAdjuvants" runat="server" Label="Adjuvants:" />--%>
                <uc:LabelPreview ID="lblPrvMedicalDevices" runat="server" Label="Medical devices:" />
                <uc:LabelPreview ID="lblPrvId" runat="server" Label="ID:" />
                <uc:LabelPreview ID="lblPrvBookedSlots" runat="server" Label="Booked slot(s):" />
                <uc:LabelPreview ID="lblPrvComment" runat="server" Label="Comment:" />
                <uc:LabelPreview ID="lblPrvLastChange" runat="server" Label="Last change:" />
            </div>
            <div id="divPropertiesRightColumn" class="rightPane">
            </div>
            <div id="divSubstances" class="clear">
            </div>
            <div id="divActiveIngredients" class="clear form-grid-border-bottom pp-grids-margins">
                <div class="form-grid-header">
                    <h2 class="bold-text-important">Active ingredients</h2>
                </div>
                <possGrid:PossGrid ID="ActiveIngredientsGrid" GridId="ActiveIngredientsGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
                    CellPadding="4" DataKeyNames="ppsubstance_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="SubstanceName" DefaultSortingOrder="ASC">
                    <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
                    <SettingsPager AlwaysShowPager="true" DefaultPageSize="10000" Position="None" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
                    <Columns>
                        <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="ppsubstance_PK" Visible="False" FilterType="Text" />
                        <possGrid:PossBoundField Width="38%" Caption="Substance name" FieldName="SubstanceName" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="LNum value" FieldName="LowNumValue" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                        <possGrid:PossBoundField Width="5%" Caption="LNum prefix" FieldName="LowNumPrefix" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="LNum unit" FieldName="LowNumUnit" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="LDen value" FieldName="LowDenValue" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                        <possGrid:PossBoundField Width="5%" Caption="LDen prefix" FieldName="LowDenPrefix" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="LDen unit" FieldName="LowDenUnit" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="HNum value" FieldName="HighNumValue" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                        <possGrid:PossBoundField Width="5%" Caption="HNum prefix" FieldName="HighNumPrefix" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="HNum unit" FieldName="HighNumUnit" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="HDen value" FieldName="HighDenValue" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                        <possGrid:PossBoundField Width="5%" Caption="HDen prefix" FieldName="HighDenPrefix" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="HDen unit" FieldName="HighDenUnit" FilterType="Text" />
                    </Columns>

                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
                    <AlternatingRowStyle BackColor="White" />
                </possGrid:PossGrid>
            </div>
            <br />
            <br />
            <div id="divExcipients" class="clear form-grid-border-bottom pp-grids-margins">
                <div class="form-grid-header">
                    <h2 class="bold-text-important">Excipients</h2>
                </div>
                <possGrid:PossGrid ID="ExcipientsGrid" GridId="ExcipientsGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
                    CellPadding="4" DataKeyNames="ppsubstance_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="SubstanceName" DefaultSortingOrder="ASC">
                    <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
                    <SettingsPager AlwaysShowPager="true" DefaultPageSize="10000" Position="None" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
                    <Columns>
                        <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="ppsubstance_PK" Visible="False" FilterType="Text" />
                        <possGrid:PossBoundField Width="100%" Caption="Substance name" FieldName="SubstanceName" FilterType="Text" />
                    </Columns>

                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
                    <AlternatingRowStyle BackColor="White" />
                </possGrid:PossGrid>
            </div>
            <br />
            <br />
            <div id="divAdjuvants" class="clear form-grid-border-bottom pp-grids-margins">
                <div class="form-grid-header">
                    <h2 class="bold-text-important">Adjuvants</h2>
                </div>
                <possGrid:PossGrid ID="AdjuvantsGrid" GridId="AdjuvantsGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
                    CellPadding="4" DataKeyNames="ppsubstance_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="SubstanceName" DefaultSortingOrder="ASC">
                    <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
                    <SettingsPager AlwaysShowPager="true" DefaultPageSize="10000" Position="None" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
                    <Columns>
                        <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="ppsubstance_PK" Visible="False" FilterType="Text" />
                        <possGrid:PossBoundField Width="38%" Caption="Substance name" FieldName="SubstanceName" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="LNum value" FieldName="LowNumValue" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                        <possGrid:PossBoundField Width="5%" Caption="LNum prefix" FieldName="LowNumPrefix" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="LNum unit" FieldName="LowNumUnit" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="LDen value" FieldName="LowDenValue" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                        <possGrid:PossBoundField Width="5%" Caption="LDen prefix" FieldName="LowDenPrefix" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="LDen unit" FieldName="LowDenUnit" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="HNum value" FieldName="HighNumValue" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                        <possGrid:PossBoundField Width="5%" Caption="HNum prefix" FieldName="HighNumPrefix" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="HNum unit" FieldName="HighNumUnit" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="HDen value" FieldName="HighDenValue" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                        <possGrid:PossBoundField Width="5%" Caption="HDen prefix" FieldName="HighDenPrefix" FilterType="Text" />
                        <possGrid:PossBoundField Width="5%" Caption="HDen unit" FieldName="HighDenUnit" FilterType="Text" />
                    </Columns>

                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
                    <AlternatingRowStyle BackColor="White" />
                </possGrid:PossGrid>
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlFooter" runat="server" class="previewBottom clear">
            <asp:LinkButton ID="btnAddDocument" runat="server" Text="Add document" CssClass="button"></asp:LinkButton>
            <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="button Delete marginLeft" OnClick="btnDelete_OnClick"></asp:LinkButton>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
