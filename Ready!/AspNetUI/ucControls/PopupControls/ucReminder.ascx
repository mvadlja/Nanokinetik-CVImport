<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReminder.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucReminder" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="toolkit" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" TagName="TextArea_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>
<%@ Register TagPrefix="operational" TagName="SearcherDisplay" Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucSearcher" Src="~/ucControls/PopupControls/ucSearcher.ascx" %>

<div id="messageModalPopupContainer" runat="server" class="modal">
    <div id="messageModalPopupContainerContent" class="modal_container_medium" style="width: 700px">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content" >
            <%--<div style="max-height:350px;overflow:auto;">--%>
            <customControls:Label_CT ID="ctlReminderType" runat="server" ControlLabel="Type:"  LabelColumnWidth="200px"/>
            <customControls:Label_CT ID="ctlRelatedAttributeName" runat="server" ControlLabel="Related date name:" LabelColumnWidth="200px"/>
            <customControls:Label_CT ID="ctlRelatedAttributeValue" runat="server" ControlLabel="Related date:" LabelColumnWidth="200px"/>
            <customControls:TextBox_CT ID="ctlTimeBeforeActivation" runat="server" LabelColumnWidth="200px" MaxLength="250" ControlInputWidth="330px" ControlLabel="Days Before:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Product company name can't be empty." OnInputValueChanged="TimeBeforeActivationChanged" AutoPostback="true"/>

            <div style="margin-left: 148px; position: relative;">
                <span style="padding-top: 3px; display: inline-block;">Alert date:</span><span style="display: inline-block; margin-left: 4px; margin-right: 4px; color: red; vertical-align: top;">*</span>
                <%--<asp:ListBox runat="server" ID="lbReminderDates" Width="100" SelectionMode="Multiple" OnSelectedIndexChanged="lbReminderDates_SelectedIndexChanged" AutoPostBack="True" />--%>
                <div style="display: inline-block; vertical-align: top;">
                    <asp:TextBox runat="server" ID="txtReminderDate" Width="70" />
                    <span style="vertical-align: middle; display: inline-block;">
                        <asp:Image ID="imgDateTime" runat="server" ImageUrl="~/Images/calendar.png" />
                    </span>
                </div>
                <%--<div style="position: absolute; left: 192px; top: 29px;">
                    <asp:ImageButton ID="lnkAddReminderDate" runat="server" Text="" Width="16px" OnClick="lnkAddReminderDate_OnClick" ImageUrl="~/Images/plus.png"> 
                    </asp:ImageButton>
                    <asp:ImageButton ID="lnkRemoveReminderDate" runat="server" Text="" Width="16px" OnClick="lnkRemoveReminderDate_OnClick"  ImageUrl="~/Images/minus.png" >
                    </asp:ImageButton>
                </div>--%>
                <toolkit:CalendarExtender ID="ceInput" FirstDayOfWeek="Monday" runat="server" Format="dd.MM.yyyy" Enabled="True" PopupButtonID="imgDateTime" TargetControlID="txtReminderDate">
                </toolkit:CalendarExtender>
                <div style="display: inline-block; margin-left: 118px; vertical-align: top;">
                    <span style="vertical-align: middle; display: inline-block;">
                        Repeat: 
                    </span>
                    <asp:DropDownList runat="server" ID="ddlReminderRepeatMode" />
                </div>
                <div id="AlertDateValidationError" runat="server" style="color: red; margin-left: 97px; margin-top: 5px;"></div>
            </div>

<%--            <customControls:DateBox_CT ID="ctlReminderDate" AutoPostback="true" runat="server" LabelColumnWidth="200px" ControlValueFormat="dd.MM.yyyy" ControlLabel="Alert date:" IsMandatory="true" ControlEmptyErrorMessage="Alert date can't be empty." ControlErrorMessage="Alert date is not a valid date." />
           --%>
            
             <customControls:TextBox_CT ID="ctlReminderTime" runat="server" LabelColumnWidth="200px" MaxLength="250" ControlInputWidth="330px" ControlLabel="Reminder time:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Product company name can't be empty." Visible="False" />
            
            <customControls:TextArea_CT ID="ctlDescription" runat="server" LabelColumnWidth="200px" MaxLength="500" ControlInputWidth="330px" ControlLabel="Alert description:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
            <customControls:DropDownList_CT ID="ctlResponsibleUser" runat="server" ControlEmptyErrorMessage="Responsible user can't be empty." ControlErrorMessage="" ControlInputWidth="336px" ControlLabel="Responsible user:" IsMandatory="true" LabelColumnWidth="200px" OnInputValueChanged="ResponsibleUserChanged" AutoPostback="true"/>
            <table>
                <tr>
                    <td style="text-align: right; width: 200px;">
                        <asp:Label ID="Label1" Text="Send Email:" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="text-align: left;">
                        <asp:CheckBox ID="cbRemindOnEmail" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="cbRemindOnEmail_CheckedChanged" />
                    </td>
                </tr>
            </table>
            <div id="divEmails" runat="server" Visible="False">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="230px" style='table-layout: fixed' align="right" valign="top">
                        <div style="padding-left: 26px;">
                            <customControls:Label_CT ID="ctlEmailRecipientsTitle" runat="server" ControlLabel="Email recipients:" LabelColumnWidth="230px" TotalControlWidth="230px" ControlLabelAlign="right" IsMandatory="false" />
                        </div>
                    </td>
                    <td>
                        <div style="margin-left: -56px">
                            <customControls:ListBox_CT ID="ctlEmailRecipients" runat="server" ControlMultipleValueSelection="true" LabelColumnWidth="" ControlInputWidth="336px" ControlLabel="" AutoPostback="True" OnInputValueChanged="ctlEmailrecipientsInputValueChanged"
                                ControlLabelAlign="left" />
                        </div>
                    </td>
                    <td>
                        <div style="margin-left: -35px">
                            <uc1:ucSearcher ID="EmailSearcher" runat="server" />
                            <operational:SearcherDisplay ID="EmailSearcherDisplay" runat="server" VisibleSearcherData="false" />
                        </div>
                        <div style="margin-left: -40px">
                            <asp:LinkButton ID="btnRemoveEmail" runat="server" CssClass="boldText" Text="Remove" Width="75px" OnClick="btnRemoveEmailOnClick" />
                        </div>
                    </td>
                </tr>
            </table>
            <customControls:TextArea_CT ID="ctlAdditionalEmails" runat="server" LabelColumnWidth="200px" MaxLength="500" ControlInputWidth="330px" ControlLabel="Additional Emails:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
            <%--</div>--%>
            </div>
            <div class="center" style="margin-top: 30px">
                <asp:Button ID="btnOK" runat="server" Text="OK" SkinID="blue" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
            </div>
        </div>
    </div>
</div>
