<%@ Page Language="C#" AutoEventWireup="True" Inherits="AspNetUI.LoginActiveDirectory" CodeBehind="LoginActiveDirectory.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript">

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-4558236-8']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

   </script>
    <title>READY!</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="layoutLogin">
            <div class="innerLogin">
                <asp:Login ID="LoginForm" DisplayRememberMe="true" Width="100%" runat="server" FailureText="Login failed!" DestinationPageUrl="~/Views/ActivityView/List.aspx" Font-Bold="False" OnAuthenticate="LoginForm_Authenticate">
                    <LayoutTemplate>
                        <div class="formLogin">
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" CssClass="error" ControlToValidate="UserName" ErrorMessage="Username required!" ToolTip="Username required!" ValidationGroup="LoginForm">*</asp:RequiredFieldValidator>
                                        
                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" CssClass="error" ControlToValidate="Password" ErrorMessage="Password required!" ToolTip="Password required!" ValidationGroup="LoginForm">*</asp:RequiredFieldValidator> 
                        </div>

                        <span class="errorFailure"><asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Login" ValidationGroup="LoginForm" CssClass="button" />      
                    </LayoutTemplate>
                </asp:Login>
            </div>
        </div>
    </form>
</body>
</html>
