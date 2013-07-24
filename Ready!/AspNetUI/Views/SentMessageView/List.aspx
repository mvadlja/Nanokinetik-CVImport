<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.SentMessageView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:ModalPopup ID="mpDelete" runat="server" />
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
        <div class="entity-name">
            <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Task:" Visible="False" />
        </div>
        <div class="subtabs" id="subtabs" runat="server">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <possGrid:PossGrid ID="SentMessageReportGrid" GridId="SentMessageReportGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="sent_message_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="sent_time" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="5%" Caption="ID" FieldName="sent_message_PK" FilterType="Text" />
                <possGrid:PossBoundField Width="10%" Caption="Sent" FieldName="sent_time" FilterType="Text" />
                <possGrid:PossTemplateField Width="10%" Caption="Message data" FieldName="msg_data_name" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/GetMessageData.aspx?id=" + Eval("sent_message_PK") + "&type=sent" %>' Text='<%# Eval("msg_data_name") %>' Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="15%" Caption="Message type" FieldName="message_type" FilterType="Text" />
                <possGrid:PossTemplateField Width="15%" Caption="XEVPRM" FieldName="xevmpd_FK" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/GetPDF.aspx?id=" + Eval("xevmpd_FK") %>' Text='<%# Eval("xevmpd_FK") %>' Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="15%" Caption="Entity type" FieldName="entity_type" FilterType="Text" />
                <possGrid:PossTemplateField Width="10%" Caption="Entity" FieldName="entity_name" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/Business/APPropertiesView.aspx?f=l&id=" + Eval("entity_FK") %>' Text='<%# Eval("entity_name") %>' Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="15%" Caption="EVCODE" FieldName="entity_evcode" FilterType="Text" />
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />

        </possGrid:PossGrid>
    </asp:Panel>
</asp:Content>
