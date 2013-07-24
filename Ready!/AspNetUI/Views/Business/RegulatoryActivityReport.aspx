<%@ Page Language="C#" MasterPageFile="../Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="RegulatoryActivityReport.aspx.cs" Inherits="AspNetUI.Views.Business.RegulatoryActivityReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/ucControls/PopupControls/ucSearcher.ascx" TagName="ucSearcher" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucSearcherSUBST.ascx" TagName="ucSearcherSUBST" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    
    <div>
        <table style="width: 100%; padding-top: 15px; padding-bottom: 15px;">
            <tr>
                <td>
                    <customControls:DropDownList_CT ID="ctlClient" runat="server" LabelColumnWidth="150px"
                        ControlInputWidth="336px" ControlLabel="Client:" IsMandatory="false" ControlErrorMessage=""
                        ControlEmptyErrorMessage="" />
                    <div style="margin-top: -2px; padding-bottom: 15px;">
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td align="right" style="width: 150px; padding-right: 12px;">
                                    <asp:Label ID="lblProduct" runat="server" Text="Product: " ControlLabelAlign="right" />
                                </td>
                                <td>
                                    <uc1:ucSearcher ID="ctlProducts" runat="server" />
                                    <operational:SearcherDisplay ID="ctlProductSearcherDisplay" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <customControls:DropDownList_CT ID="ctlActivityType" runat="server" LabelColumnWidth="150px"
                        ControlInputWidth="336px" ControlLabel="Activity type:" IsMandatory="false" ControlErrorMessage=""
                        ControlEmptyErrorMessage="" />
                    <%--<customControls:DropDownList_CT ID="ctlActiveIngredient" runat="server" LabelColumnWidth="150px"
                        ControlInputWidth="336px" ControlLabel="Active ingredient:" IsMandatory="false"
                        ControlErrorMessage="" ControlEmptyErrorMessage="" />  --%>
                    <div>
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td style="padding-left: 60px; padding-right: 24px;">
                                    <asp:Label ID="Label1" runat="server" Text="Active ingredient:" ControlLabelAlign="right" />
                                </td>
                                <td>
                                    <div style="margin-left: -12px">
                                        <uc1:ucSearcherSUBST ID="actIngrSearcher" runat="server" />
                                        <operational:SearcherDisplay ID="actIngrSearcherDisplay1" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <customControls:DropDownList_CT ID="ctlCountry" runat="server" LabelColumnWidth="150px"
                        ControlInputWidth="336px" ControlLabel="Country:" IsMandatory="false" ControlErrorMessage=""
                        ControlEmptyErrorMessage="" />
                    <customControls:DropDownList_CT ID="ctlRegulatoryStatus" runat="server" LabelColumnWidth="150px"
                        ControlInputWidth="336px" ControlLabel="Regulatory status:" IsMandatory="false"
                        ControlErrorMessage="" ControlEmptyErrorMessage="" />
                    <br />
                    <table>
                        <tr>
                            <td align="right" width="149px">Submission date:
                            </td>
                            <td align="left" width="120px" style="padding-left: 12px">
                                <customControls:DateBox_CT ID="ctlSubmissionDateFrom" runat="server" LabelColumnWidth="100px"
                                    ControlValueFormat="dd.MM.yyyy" ControlLabel="From:" IsMandatory="false" ControlEmptyErrorMessage=""
                                    ControlErrorMessage="" />
                            </td>
                            <td align="left">
                                <div style="margin-left: -50px">
                                    <customControls:DateBox_CT ID="ctlSubmissionDateTo" runat="server" LabelColumnWidth="100px"
                                        ControlValueFormat="dd.MM.yyyy" ControlLabel="To:" IsMandatory="false" ControlEmptyErrorMessage=""
                                        ControlErrorMessage="" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="149px">Approval date:
                            </td>
                            <td align="left" width="120px" style="padding-left: 12px">
                                <customControls:DateBox_CT ID="ctlApprovalDateFrom" runat="server" LabelColumnWidth="100px"
                                    ControlValueFormat="dd.MM.yyyy" ControlLabel="From:" IsMandatory="false" ControlEmptyErrorMessage=""
                                    ControlErrorMessage="" />
                            </td>
                            <td align="left">
                                <div style="margin-left: -50px">
                                    <customControls:DateBox_CT ID="ctlApprovalDateTo" runat="server" LabelColumnWidth="100px"
                                        ControlValueFormat="dd.MM.yyyy" ControlLabel="To:" IsMandatory="false" ControlEmptyErrorMessage=""
                                        ControlErrorMessage="" />
                                </div>
                            </td>
                        </tr>
                    </table>

                </td>
                <%--<td align="right" valign="middle" style="border-width:1px; border-left-style:solid; width:150px;" >
                     
                </td>--%>
            </tr>
        </table>
        <div style="margin-left: 172px; margin-bottom: 15px;">Activities with the Type or Status set to "N/A" will not be displayed.</div>
        <div style="margin-left: 169px;">
            <asp:Button ID="btnCreate" runat="server" Text="Create report" OnClick="btnCreateClick" />
        </div>
        <br />
        <div style="word-break: break-all;">
            <rsweb:ReportViewer ID="rvMain" runat="server" Font-Names="Verdana" Font-Size="9pt"
                ProcessingMode="Remote" SizeToReportContent="True" Style="width: 100%;">
                <ServerReport ReportServerUrl="" />
                
            </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
