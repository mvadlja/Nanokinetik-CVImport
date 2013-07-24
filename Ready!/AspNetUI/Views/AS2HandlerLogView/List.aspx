<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.AS2HandlerLogView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <div class="buttonsHolder">
        <div class="rightSection">
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
        <possGrid:PossGrid ID="AS2HandlerLogGrid" GridId="AS2HandlerLogGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="as2_handler_log_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="FullName" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Caption="ID" FieldName="as2_handler_log_PK" Width="5%" FilterType="Text" Visible="False" />
                <possGrid:PossBoundField Caption="Log time" FieldName="log_time" Width="10%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Received" FieldName="received_time" Width="10%" FilterType="Text" />
                <possGrid:PossBoundField Caption="From" FieldName="as2_from" Width="7%" FilterType="Text" />
                <possGrid:PossBoundField Caption="To" FieldName="as2_to" Width="7%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Message-ID" FieldName="message_ID" Width="15%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Message type" FieldName="message_type" Width="10%" FilterType="Text" />
                <possGrid:PossTemplateField Caption="Received Message Number" FieldName="received_message_FK" Width="7%" FilterType="Text" >
                    <ItemTemplate>
                        <asp:HyperLink ID="hlMsgData" runat="server" CssClass="gvLinkTextBold" Text='<%# Eval("received_message_FK") %>' Target="_blank" Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Caption="Entity type" FieldName="entity_type" Width="10%" FilterType="Text" />
                <possGrid:PossTemplateField Caption="Entity" FieldName="entity_name" Width="10%" FilterType="Text" >
                    <ItemTemplate>
                        <asp:HyperLink ID="hlEntity" runat="server" CssClass="gvLinkTextBold" Text='<%# Eval("entity_name") %>' Target="_blank" Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Caption="EVCODE" FieldName="entity_evcode" Width="10%" FilterType="Text" />
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />

        </possGrid:PossGrid>
    </asp:Panel>
</asp:Content>
