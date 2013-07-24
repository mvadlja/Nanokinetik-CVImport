<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotActive.aspx.cs" Inherits="AspNetUI.NotActive" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>READY!</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="layoutMain">
            
            <!-- HEADER -->
            <table cellpadding="0" cellspacing="0" class="layoutHeader">
                <tr>
                    <td class="layoutHeaderLeftRightCells">
                        <a href="~/Default.aspx" id="linkLogo" runat="server">
                            <asp:Image ID="imgHeadLogo" ImageUrl="~/Images/logo_head.png" runat="server" />
                        </a>
                    </td>

                    <td class="layoutHeaderMiddleCell">
                        <asp:Label ID="lblHeader" runat="server" Text="Ready!" CssClass="mainTitle"></asp:Label>
                    </td>
                        
                    <td class="layoutHeaderLeftRightCells" align="right">
                        <div style="padding: 5px">

                        </div>
                    </td>
                </tr>
            </table>
                                
            <!-- CONTENT -->
            <table cellpadding="3" cellspacing="0" width="100%">
                <tr>
                    <td class="layoutContent">
                        <br />
                        <table cellpadding="10" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="2"><asp:Label id="lblHead" runat="server" Text="Application is currently not active!" Font-Bold="true" Font-Size="12px" /></td>
                            </tr>
                            <tr>
                                <td><asp:Label id="lblError" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2"><hr /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
                
            <!-- FOOTER -->
            <table cellpadding="5" cellspacing="0" width="100%" class="layoutFooter">
                <tr>
                    <td align="left">
                        Copyright &copy; 2013. All rights reserved.
                    </td>
                    <td align="right">
                        <asp:Label ID="Label1" runat="server" ForeColor="White"></asp:Label>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://www.nanokinetik.com" Target="_blank" Font-Bold="true"><asp:Label ID="lblFooter" runat="server" ForeColor="White" Text="Nanokinetik"></asp:Label></asp:HyperLink>
                    </td>
                </tr>
            </table>

        </div>
    </div>
    </form>
</body>
</html>

