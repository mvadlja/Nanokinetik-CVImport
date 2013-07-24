<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenu.ascx.cs" Inherits="AspNetUI.Support.TopMenu" %>
<asp:UpdatePanel runat="server" id="TopMenuUpdatePanel" updatemode="Conditional">
    <ContentTemplate>
        <asp:Table ID="tblTopMenu" runat="server" CellPadding="0" CellSpacing="0"></asp:Table>
    </ContentTemplate>
</asp:UpdatePanel>