<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ASPropertiesForm_list.ascx.cs" Inherits="AspNetUI.Views.ASPropertiesForm_list" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="toolkit" %>
<!-- Operational controls -->
<%@ Register Src="~/Views/PartialShared/Operational/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="operational" %>
<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>
<!-- Custom input controls -->
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" TagName="TextArea_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/MoneyBox_CT.ascx" TagName="MoneyBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/RadioButtonList_CT.ascx" TagName="RadioButtonList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/CheckBoxList_CT.ascx" TagName="CheckBoxList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ComboBox_CT.ascx" TagName="ComboBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/AutoCompleteBox_CT.ascx" TagName="AutoCompleteBox_CT" TagPrefix="customControls" %>

<!-- Optional searcher goes here -->
<asp:Panel ID="pnlDataSearcher" runat="server" GroupingText="Searcher" Visible="False">
    <br />
</asp:Panel>
<!-- Result list goes here -->
<asp:Panel ID="pnlDataList" runat="server">
    <table>
        <tr>
            <td style="width: 1000px">
                <table cellpadding="3" cellspacing="0">
                    <%-- <tr>
                        <td class="style4" valign="top">
                            <asp:Label ID="lblSubstanceNameTitle" runat="server">Substance name:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSubstanceName" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="style4" valign="top">
                            <asp:Label ID="lblEvcode" runat="server">EVCODE:</asp:Label>
                            <%--<asp:Label ID="lblProductNameReq" runat="server" ForeColor="Red">*</asp:Label>--%>
                        </td>
                        <td>
                            <asp:Label ID="lblEvcodeName" runat="server" Font-Bold="True" Visible="true"></asp:Label>
                        </td>
                        <td style="width: 125px">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style4" valign="top">
                            <asp:Label ID="lblCasNumberTitle" runat="server">CAS number:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCasNumber" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style4" valign="top">
                            <asp:Label ID="Label1" runat="server">Molecular formula:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMolecularFormula" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style4" valign="top">
                            <asp:Label ID="Label2" runat="server">Substance Class:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblClass" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style4" valign="top">
                            <asp:Label ID="Label3" runat="server">CBD:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCbd" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <%-- <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="Label4" runat="server">Substance translation:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSubstanceTranslation" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                         <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="Label5" runat="server">Substance aliases:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSubstanceAliases" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                         <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="Label6" runat="server">Internationals code:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblInternationalCode" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                         <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="Label7" runat="server">Previous EVCODE:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPreviousEvcode" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                         <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="Label8" runat="server">Substance attachment:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSubstanceAttachment" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>--%>
                    <tr>
                        <td class="style4" valign="top">
                            <asp:Label ID="Label9" runat="server">Comments:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblComment" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style4" valign="top">
                            <asp:Label ID="lblLastChangeTitle" runat="server">Last change:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblLastChange" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
