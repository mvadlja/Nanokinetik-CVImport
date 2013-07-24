<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecoverPassword.aspx.cs"
    Inherits="AspNetUI.RecoverPassword" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT"
    TagPrefix="customControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>READY!</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="layoutMain">
            <!-- HEADER -->
            <asp:HyperLink ID="HyperLink2" class="layoutHeader" Height="80px" runat="server"
                url="~/Default.aspx">
                <a class="layoutHeaderLeft" href="http://www.nanokinetik.com" target="_blank"></a>
                <a class="layoutHeaderRight" href="http://www.nanokinetik.com" target="_blank"></a>
            </asp:HyperLink>
            <!-- CONTENT -->
            <div class="forgot_password">
            <br />
                <asp:Label ID="lblInfo" runat="server" Text="Please contact your system administrator!"
                    Font-Bold="true" Font-Size="12px" />
                <br /><br /><br />
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" Width="125px"
                    Height="20px" />
                <asp:Panel ID="pnlRecoverPassword" runat="server" Visible="false">
                    <br />
                    <asp:Label ID="lblHead" runat="server" Text="Before we can reset your password, you need to enter your email address below to help identify your account"
                        Font-Bold="true" Font-Size="12px" />
                    <div style="margin-left: -40px">
                        <customControls:TextBox_CT ID="ctlEmail" runat="server" MaxLength="300" ControlInputWidth="330px"
                            ControlLabel="Email" IsMandatory="true" ControlErrorMessage="Email is not a valid email."
                            ControlEmptyErrorMessage="Email can't be empty." />
                    </div>
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="true" />
                    <br />
                    <%--<asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" Width="125px"
                        Height="20px" />--%>
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Back"
                        Width="125px" Height="20px" />
                </asp:Panel>
            </div>
            <!-- FOOTER -->
            <table cellpadding="5" cellspacing="0" width="100%" class="layoutFooter">
                <tr>
                    <td align="left">
                        Copyright &copy; 2011. All rights reserved.
                    </td>
                    <td align="right">
                    <div style="margin-right:20px;">
                        <asp:Label ID="Label1" runat="server" ForeColor="White"></asp:Label>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://www.nanokinetik.com"
                            Target="_blank" Font-Bold="true">
                            <asp:Label ID="lblFooter" runat="server" ForeColor="White" Text="Nanokinetik"></asp:Label></asp:HyperLink>
                            </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
