<%@ Page Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="ActivityTimeReport.aspx.cs" Inherits="AspNetUI.Views.Business.ActivityTimeReport" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>

<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>

<%--<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <rsweb:ReportViewer ID="rvMain" runat="server" Font-Names="Verdana" 
            Font-Size="9pt" ProcessingMode="Remote" 
            SizeToReportContent="True" style="width:100%;">
            <ServerReport ReportServerUrl="" />
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>--%>


<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div>
        <table style="width: 100%; padding-top: 15px; padding-bottom: 15px;">
            <tr>
                <td style="width: 700px">
                    <customControls:DropDownList_CT ID="ctlClient" runat="server" LabelColumnWidth="150px"
                        ControlInputWidth="330px" ControlLabel="Client:" IsMandatory="false" ControlErrorMessage=""
                        ControlEmptyErrorMessage="" />
                    <customControls:DropDownList_CT ID="ctlPerson" runat="server" LabelColumnWidth="150px"
                        ControlInputWidth="330px" ControlLabel="Responsible user:" IsMandatory="false"
                        ControlErrorMessage="" ControlEmptyErrorMessage="" />
                    <div style="float: left; margin-top: 8px;">
                        <customControls:DateBox_CT ID="ctlDateFrom" runat="server" LabelColumnWidth="150px"
                            ControlValueFormat="dd.MM.yyyy" ControlLabel="Date from:" IsMandatory="false"
                            ControlEmptyErrorMessage="" ControlErrorMessage="" />
                    </div>
                    <div style="float: left; margin-top: 8px; margin-left: 13px;">
                        <customControls:DateBox_CT ID="ctlDateTo" runat="server" LabelColumnWidth="50px"
                            ControlValueFormat="dd.MM.yyyy" ControlLabel="Date to:" IsMandatory="false" ControlEmptyErrorMessage=""
                            ControlErrorMessage="" />
                    </div>
                </td>
                <td align="right" valign="middle">
                </td>
            </tr>
        </table>
        <div style="margin-left: 169px;">
            <asp:Button ID="btnCreate" runat="server" Text="Create report" OnClick="btnCreateClick" />
        </div>
        <br/>
        <rsweb:ReportViewer ID="rvMain" runat="server" Font-Names="Verdana" 
            Font-Size="9pt" ProcessingMode="Remote" 
            SizeToReportContent="True" style="width:100%;">
            <ServerReport ReportServerUrl="" />
        </rsweb:ReportViewer>
    </div>
   
</asp:Content>
