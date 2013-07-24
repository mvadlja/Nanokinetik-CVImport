<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorInfo.aspx.cs" Inherits="AspNetUI.Views.RestrictedAreaView.ErrorInfo" %>
<%@ Register Src="~/Views/PartialShared/Operational/AboutModalPopup.ascx" TagName="AboutModalPopup" TagPrefix="operational" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>READY!</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HyperLink class="layoutHeader" ID="lnkLayoutHeader" runat="server">
                <a class="layoutHeaderLeft" id="lnkLayoutHeaderLeft" runat="server"></a>
                <a class="layoutHeaderRight" href="http://www.nanokinetik.com" target="_blank"></a>
            </asp:HyperLink>
            <div class="layoutMenuBar">

                <div class="loginInfo">
                    <asp:HyperLink ID="lnkReminders" runat="server" Font-Underline="false" Visible="false"></asp:HyperLink>
                    <asp:Label ID="lblRemindersSeparator" Text="|" runat="server" Font-Bold="false" Font-Underline="false" Visible="false"></asp:Label>
                    Logged in as
                    <asp:HyperLink ID="lblLoginName" runat="server" Font-Bold="true" Font-Underline="false"></asp:HyperLink>.
                    <asp:LinkButton ID="lbtLogOut" runat="server" CssClass="button LogOut" OnClick="lbtLogOut_Click">Logout</asp:LinkButton>
                </div>
            </div>

            <div class="mainContent">
                <asp:Label ID="lblInfo" runat="server" CssClass="restricted-area-info"></asp:Label>    
                <div class="restricted-area-div-go-back">
                    <asp:Button Text="Go back" ID="btnBack" runat="server" OnClick="btnBack_Click" Width="180" CssClass="restricted-area-btn-go-back" Visible="false" />
                </div> 
            </div>

            <div class="layoutFooter">
                <asp:LinkButton ID="lbtAbout" runat="server" CssClass="gvLinkTextBold" OnClick="lbtAbout_Click">About</asp:LinkButton>
                READY!
            <asp:Label ID="lblAppVersion" runat="server"></asp:Label>
                <div class="copyright">
                    &copy; 2012
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://www.nanokinetik.com" Target="_blank" Font-Bold="true">Nanokinetik</asp:HyperLink>
                    - All rights reserved.
                </div>
            </div>
            <operational:AboutModalPopup ID="aboutModalPopup" runat="server" />
         </div>
    </form>
</body>
</html>
