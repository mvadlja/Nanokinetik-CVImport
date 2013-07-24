<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.PersonView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>
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
                <asp:Panel ID="pnlSearchButtons" Style="margin-left: 166px;" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Panel ID="pnlGrid" runat="server" Visible="True">
        <uc:ColumnsPopup ID="ColumnsPopup" runat="server" />
        <uc:ModalPopup ID="mpDelete" runat="server" />
        <div class="entity-name">
            <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Task:" Visible="False" />
        </div>
        <div class="subtabs" id="subtabs" runat="server">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <possGrid:PossGrid ID="PersonGrid" GridId="PersonGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="person_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="FullName" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="5%" Caption="ID" FieldName="person_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="20%" Caption="Name" FieldName="FullName" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlName" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/PersonView/Form.aspx?EntityContext=Person&Action=Edit&From=Person&idPerson=" + Eval("person_PK")%>' Text='<%# Eval("FullName") %>' Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>

                <possGrid:PossBoundField Caption="EVCODE" FieldName="EvCode" Width="10%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Title" FieldName="Title" Width="10%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Street" FieldName="Street" Width="10%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Company" FieldName="Company" Width="10%" FilterType="Text" />
                <possGrid:PossBoundField Caption="State" FieldName="State" Width="8%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Department" FieldName="Department" Width="10%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Postcode" FieldName="Postcode" Width="5%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Building" FieldName="Building" Width="7%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Country code" FieldName="CoutryCode" Width="8%" FilterType="Text" />

                <possGrid:PossTemplateField Width="2%" Caption="" FieldName="Delete" FilterType="None" CellStyleCssClass="not-sortable">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("person_PK") %>' ImageUrl="~/Images/delete.gif" OnClick="btnDeleteEntity_OnClick" />
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
