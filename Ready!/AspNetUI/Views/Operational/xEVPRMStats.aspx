<%@ Page Title="" Language="C#" MasterPageFile="../Shared/Template/Default.Master" AutoEventWireup="True" CodeBehind="xEVPRMStats.aspx.cs" Inherits="AspNetUI.Views.xEVPRMStats" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">

    <div style="margin-left: 15px;">
        <br />
        <asp:Panel ID="pnlService" runat="server" GroupingText="Service information" CssClass="fieldset-with-border" Width="552px">
            <br />
            <asp:Label ID="ctlServiceStatus" runat="server" CssClass="serviceStatusLabel"></asp:Label>
            <div class="floatRight" style="margin-top: -4px;">
            <asp:Button Text="Start service" runat="server" ID="btnToggleService" Width="130px" />
            </div>
            <div id="divServiceProcessingCommands" runat="server" visible="False">
                <br />
                <asp:Label ID="lblXevprmSubmission" runat="server" CssClass="serviceStatusLabel" Text="Process xEVPRM messages ready for submission:"></asp:Label>
                <div class="floatRight" style="margin-top: -4px;">
                    <asp:Button Text="RUN" runat="server" ID="btnXevprmSubmission" Width="130px" OnClick="btnXevprmSubmission_OnClick" />
                </div>
                <br /><br />
                <asp:Label ID="lblReceivedMessageProcessing" runat="server" CssClass="serviceStatusLabel" Text="Process received messages:"></asp:Label>
                <div class="floatRight" style="margin-top: -4px;">
                    <asp:Button Text="RUN" runat="server" ID="btnReceivedMessageProcessing" Width="130px" OnClick="btnReceivedMessageProcessing_OnClick" />
                </div>
                <br /><br />
                <asp:Label ID="lblMDNSubmission" runat="server" CssClass="serviceStatusLabel" Text="Process xEVPRM messages ready for MDN submission:"></asp:Label>
                <div class="floatRight" style="margin-top: -4px;">
                    <asp:Button Text="RUN" runat="server" ID="btnMDNSubmission" Width="130px" OnClick="btnMDNSubmission_OnClick" />
                </div>
            </div>
            <br />
            <br />
            <div id="divServiceInformation" runat="server">
                <table width="520px">
                    <tr>
                        <td width="380px">
                            <customControls:Label_CT ID="lblPollingInterval" runat="server" ControlLabel="Polling interval:" />
                        </td>
                        <td style="float: right">
                            <div id="ctlPollingInterval" runat="server" style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <customControls:Label_CT ID="lblLastProcessingTime" runat="server" ControlLabel="Most recent processing time:" />
                        </td>
                        <td style="float: right">
                            <div id="ctlLastProcessingTime" runat="server" style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <customControls:Label_CT ID="lblLogFileDownload" runat="server" ControlLabel="AS2 handler inbound log file:" />
                        </td>
                        <td style="float: right">
                            <asp:HyperLink ID="hlLogFileDownload" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divServiceError" runat="server" style="margin-top: -4px; float: left; margin-left: 5px;" visible="false">
                <br />
                <asp:Label ID="serviceError" runat="server" Visible="true" ForeColor="Red" />
                <br />
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlServiceEmailNotifications" runat="server" GroupingText="AS2 service email notifications" CssClass="fieldset-with-border" Width="552px">
            <div id="div1" runat="server">
                <table width="520px">
                    <tr>
                        <td>
                            <customControls:Label_CT ID="lblEmailNotificationsLogFileDownload" runat="server" ControlLabel="AS2 email notifications log file:" />
                        </td>
                        <td style="float: right">
                            <asp:HyperLink ID="hlEmailNotificationLogFileDownload" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="messagesStatus" runat="server" GroupingText="Messages statistics" CssClass="fieldset-with-border" Width="550px">
            <br />
            <table width="520px">
                <tr>
                    <td width="450px">
                        <customControls:Label_CT ID="Label_CT1" runat="server" ControlLabel="Number of sent messages:" />
                    </td>
                    <td style="float: right">
                        <div id="ctlNumberOfSentMessages" runat="server" style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <customControls:Label_CT ID="Label_CT2" runat="server" ControlLabel="Number of sent messages waiting for ACK:" />
                    </td>
                    <td style="float: right">
                        <div id="ctlSentMessageWaitingACK" runat="server" style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <customControls:Label_CT ID="Label_CT3" runat="server" ControlLabel="Number of received messages:" />
                    </td>
                    <td style="float: right">
                        <div id="ctlNumberOfReceivedMessage" runat="server" style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <customControls:Label_CT ID="Label_CT6" runat="server" ControlLabel="Number of messages waiting for processing:" />
                    </td>
                    <td style="float: right">
                        <div id="ctlPendingProcessing" runat="server" style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <customControls:Label_CT ID="Label_CT4" runat="server" ControlLabel="Number of received ACK 01:" />
                    </td>
                    <td style="float: right">
                        <div id="ctlReceivedACKSuccess" runat="server" style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <customControls:Label_CT ID="Label_CT5" runat="server" ControlLabel="Number of received ACK 02:" />
                    </td>
                    <td style="float: right">
                        <div id="ctlReceivedACKPartialy" runat="server" style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <customControls:Label_CT ID="Label_CT7" runat="server" ControlLabel="Number of received ACK 03:" />
                    </td>
                    <td style="float: right">
                        <div id="ctlReceivedACKError" runat="server" style="text-align: right;" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="float: right; margin-right: -2px;">
                <asp:Button runat="server" ID="btnRefresh" Text="Refresh" Width="130px" OnClick="btnRefresh_OnClick"/>
            </div>
        </asp:Panel>
        <br />
    </div>
</asp:Content>



