<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.AccountView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <asp:Panel ID="pnlConfirmationMsg" runat="server" BackColor="#DFFFDF" ForeColor="#005F00" BorderColor="#9FCF9F" BorderWidth="1px" HorizontalAlign="Center" Visible="false">Password successfully updated.</asp:Panel>
            <fieldset>
                <uc:TextBox ID="txtOldPassword" runat="server" Label="Old password:" Required="True" TextMode="Password" />
                <uc:TextBox ID="txtNewPassword" runat="server" Label="New password:" Required="True" TextMode="Password" />
                <uc:TextBox ID="txtRepeatedNewPassword" runat="server" Label="Confirm password:" Required="True" TextMode="Password"/>
            </fieldset>
        </div>
        <div id="divRightPane" class="rightPane" style="margin-top: 0px; margin-left: -74px;">
            <fieldset>
                
            </fieldset>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlFooter" runat="server" class="bottom clear bottomControlsHolder" valign="center">
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="button Save" OnClick="btnSave_OnClick" />
    </asp:Panel>
</asp:Content>
