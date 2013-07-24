<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.AuditTrailView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <div class="buttonsHolder">
        <div class="rightSection">
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
        <div class="entity-name">
            <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Product:" Visible="False" />
        </div>
        <div class="subtabs" id="subtabs" runat="server">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 40%; vertical-align: top;">
                    <possGrid:PossGrid ID="MasterGrid" GridId="MasterGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
                        CellPadding="4" DataKeyNames="ChangeDate" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="ChangeDate" DefaultSortingOrder="ASC">
                        <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
                        <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="Bottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
                        <Columns>
                            <possGrid:PossBoundField Width="20%" Caption="Version" FieldName="Version" FilterType="Text" />
                            <possGrid:PossBoundField Width="30%" Caption="Change Date" FieldName="ChangeDate" FilterType="Text" />
                            <possGrid:PossBoundField Width="25%" Caption="Changed by" FieldName="Username" FilterType="Text" />

                            <possGrid:PossTemplateField Width="25%" Caption="Details" FieldName="Details" FilterType="None" CellStyleCssClass="not-sortable">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetails" CommandArgument='<%# Eval("SessionToken") %>' runat="server" Text="Details" OnClick="Details_OnCLick"></asp:LinkButton>
                                </ItemTemplate>
                            </possGrid:PossTemplateField>
                        </Columns>

                        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                        <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="White" />
                    </possGrid:PossGrid>
                </td>
                <td style="width: 5%"></td>
                <td style="width: 55%; vertical-align: top;">
                    <asp:HiddenField ID="hfSessionToken" runat="server" />
                    <possGrid:PossGrid ID="DetailsGrid" GridId="DetailsGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
                        CellPadding="4" DataKeyNames="ColumnName" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="ColumnName" DefaultSortingOrder="ASC">
                        <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
                        <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="Bottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
                        <Columns>
                            <possGrid:PossBoundField Width="1%" Caption="IDAuditingDetail" FieldName="IDAuditingDetail" FilterType="Text" Visible="False" />
                            <possGrid:PossBoundField Width="30%" Caption="Column Name" FieldName="ColumnName" FilterType="Text" />
                            <possGrid:PossTemplateField Width="35%" Caption="Old Value" FieldName="OldValue" FilterType="Text">
                                <ItemTemplate>
                                    <asp:Panel ID="pnlOldValue" runat="server" />
                                </ItemTemplate>
                            </possGrid:PossTemplateField>
                            <possGrid:PossTemplateField Width="35%" Caption="New Value" FieldName="NewValue" FilterType="Text">
                                <ItemTemplate>
                                    <asp:Panel ID="pnlNewValue" runat="server" />
                                </ItemTemplate>
                            </possGrid:PossTemplateField>
                        </Columns>

                        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                        <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="White" />
                    </possGrid:PossGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
