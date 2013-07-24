<%@ Page Language="C#" MasterPageFile="../Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="CustomerTimeReport.aspx.cs" Inherits="AspNetUI.Views.Business.CustomerTimeReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    
    <div>
        <table style="width: 100%; padding-top: 15px; padding-bottom: 15px;">
            <tr>
                <td style="width: 700px">
                    <customControls:DropDownList_CT ID="ctlCustomer" runat="server" LabelColumnWidth="200px" ControlInputWidth="330px" ControlLabel="Customer:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
                    <customControls:DropDownList_CT ID="ctlPerson" runat="server" LabelColumnWidth="200px" ControlInputWidth="330px" ControlLabel="Responsible user:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
                    <customControls:DropDownList_CT ID="ctlProject" runat="server" LabelColumnWidth="200px" ControlInputWidth="330px" ControlLabel="Project:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />

                    <div style="float: left; margin-top: 8px;">
                        <customControls:DateBox_CT ID="ctlDateFrom" runat="server" LabelColumnWidth="200px" ControlValueFormat="dd.MM.yyyy" ControlLabel="Date from:" IsMandatory="false" ControlEmptyErrorMessage="" ControlErrorMessage="" />
                    </div>
                    <div style="float: left; margin-top: 8px; margin-left: 13px;">
                        <customControls:DateBox_CT ID="ctlDateTo" runat="server" LabelColumnWidth="50px" ControlValueFormat="dd.MM.yyyy" ControlLabel="Date to:" IsMandatory="false" ControlEmptyErrorMessage="" ControlErrorMessage="" />
                    </div>

                    <div style="float: left; margin-top: 8px; width: 700px;">
                        <table>
                            <tr>
                                <td />
                                <td>
                                    <asp:Label runat="server" ID="authNumYes" Text="Yes" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="authNumNo" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-right: 13px;">
                                    <asp:Label runat="server" ID="ctlMarketed" Text="Show Authorisation number:" Width="200px" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="authNumYesBtn" GroupName="ShowAuthNumber" runat="server" AutoPostBack="false" EnableViewState="true" Visible="true" LabelColumnWidth="150px" ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="authNumNoBtn" GroupName="ShowAuthNumber" runat="server" AutoPostBack="false" EnableViewState="true" Visible="true" LabelColumnWidth="150px" ControlErrorMessage="" ControlEmptyErrorMessage="" Checked="True" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: left; margin-top: 5px; width: 700px;">
                        <table>
                            <tr>
                                <td />
                                <td>
                                    <asp:Label runat="server" ID="billingRateYes" Text="Yes" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="billingRateNo" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-right: 13px;">
                                    <asp:Label runat="server" ID="ctlBillingRate" Text="Billing rate:" Width="200px" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbBillingRateYes" GroupName="BillingRate" runat="server" AutoPostBack="true" EnableViewState="true" Visible="true" LabelColumnWidth="150px" ControlErrorMessage="" ControlEmptyErrorMessage="" OnCheckedChanged="rbBillingRateYes_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbBillingRateNo" GroupName="BillingRate" runat="server" AutoPostBack="true" EnableViewState="true" Visible="true" LabelColumnWidth="150px" ControlErrorMessage="" ControlEmptyErrorMessage="" Checked="True" OnCheckedChanged="rbBillingRateNo_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="clear">
                    </div>
                </td>
                <td align="right" valign="middle"></td>
            </tr>
        </table>
        <div style="margin-left: 219px;">
            <asp:Button ID="btnCreateReport" runat="server" Text="Create report" OnClick="btnCreateReportClick" />
        </div>
        <br />
        <div style="word-break: break-all;">
            <rsweb:ReportViewer ID="rvMain" runat="server" Font-Names="Verdana" Font-Size="9pt" ProcessingMode="Remote" SizeToReportContent="True" Style="width: 100%;">
                <ServerReport ReportServerUrl="" />
            </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
