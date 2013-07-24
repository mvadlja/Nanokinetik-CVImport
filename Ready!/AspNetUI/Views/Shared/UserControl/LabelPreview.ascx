<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="LabelPreview.ascx.cs" Inherits=" AspNetUI.Views.Shared.UserControl.LabelPreview" %>

<div ID="divLabelPreview" runat="server" class="row">
    <div class="labelPv">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="lblValue"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input">
        <asp:Label ID="lblValue" runat="server" CssClass="valuePv" Visible="true"></asp:Label>
        <asp:Panel ID="pnlLinks" runat="server" CssClass="pnlLinks word-wrap-break-word" Visible="false"></asp:Panel>
        <asp:LinkButton ID="lnkSetReminder" runat="server" Text="" Visible="False" CssClass="reminder-icon reminder-icon-not-exists" />
    </div>
    <div class="clear"></div>
</div>


