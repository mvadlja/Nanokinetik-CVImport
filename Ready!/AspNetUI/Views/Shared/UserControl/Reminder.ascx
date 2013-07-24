<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Reminder.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Reminder" %>

<%@ Register Src="TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="DateTimeBox.ascx" TagName="DateTimeBox" TagPrefix="uc" %>
<%@ Register Src="DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="CheckBox.ascx" TagName="CheckBox" TagPrefix="uc" %>
<%@ Register Src="ListBoxExt.ascx" TagName="ListBoxExt" TagPrefix="uc" %>
<%@ Register Src="Searcher.ascx" TagName="Searcher" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<div id="reminderContainer" runat="server" class="modal">
    <div id="reminderContainerMain" class="modal_container_medium" style="width: 650px">
        <div id="reminderHeader" runat="server" class="modal_title_bar">
            <div runat="server" id="divHeader"></div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_Click" />
        </div>
        <div id="reminderBody" runat="server" class="modal_content DivWithScroll">
            <uc:LabelPreview ID="lblPrvReminderType" runat="server" Label="Type:" TextBold="false" />
            <uc:LabelPreview ID="lblPrvRelatedAttributeName" runat="server" Label="Related date name:" TextBold="false" />
            <uc:LabelPreview ID="lblPrvRelatedAttributeValue" runat="server" Label="Related date:" TextBold="false" />
            <uc:TextBox ID="txtTimeBeforeActivation" runat="server" Label="Days Before:" AutoPostback="true" />

            <div style="margin-left: 91px; position: relative;">
                <span style="padding-top: 3px; display: inline-block;">Alert date:</span><span style="display: inline-block; margin-left: 5px; margin-right: 5px; color: red; vertical-align: top;">*</span>
                <%--<asp:ListBox runat="server" ID="lbReminderDates" Width="100" SelectionMode="Multiple" OnSelectedIndexChanged="lbReminderDates_SelectedIndexChanged" AutoPostBack="True" />--%>
                <div style="display: inline-block; vertical-align: top;">
                    <asp:TextBox runat="server" ID="txtReminderDate" Width="70" AutoPostBack="True" />
                    <span style="vertical-align: middle; display: inline-block;">
                        <asp:Image ID="imgDateTime" runat="server" ImageUrl="~/Images/calendar.png" />
                    </span>
                </div>
                <%--<div style="position: absolute; left: 192px; top: 29px;">
                    <asp:LinkButton ID="lnkAddReminderDate" runat="server" Text="" Width="24px" OnClick="lnkAddReminderDate_OnClick">
                        <asp:Image ID="imgAddReminderDate" runat="server" ImageUrl="~/Images/plus.png" />
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkRemoveReminderDate" runat="server" Text="" Width="24px" OnClick="lnkRemoveReminderDate_OnClick">
                        <asp:Image ID="imgRemoveReminderDate" runat="server" ImageUrl="~/Images/minus.png" />
                    </asp:LinkButton>
                </div>--%>
                <act:CalendarExtender ID="ceInput" FirstDayOfWeek="Monday" runat="server" Format="dd.MM.yyyy" Enabled="True" PopupButtonID="imgDateTime" TargetControlID="txtReminderDate">
                </act:CalendarExtender>
                <div style="display: inline-block; margin-left: 118px; vertical-align: top;">
                    <span style="vertical-align: middle; display: inline-block;">Repeat: 
                    </span>
                    <asp:DropDownList runat="server" ID="ddlReminderRepeatMode" />
                </div>
                <div id="AlertDateValidationError" runat="server" style="color: red; margin-left: 83px; margin-top: 5px;"></div>
            </div>

            <uc:DateTimeBox ID="dtReminderTime" runat="server" Label="Reminder time:" Required="True" AutoPostback="true" Visible="false" />
            <uc:TextBox ID="txtDescription" runat="server" Label="Alert description:" TextMode="MultiLine" Rows="6" Columns="55" />
            <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" Required="True" AutoPostback="true" />
            <uc:CheckBox ID="cbRemindOnEmail" runat="server" Label="Send Email:" AutoPostback="true" />
            <uc:ListBoxExt ID="lbExtEmails" runat="server" Label="Email recipients:" LabelWidth="150px" SelectionMode="Single" VisibleRows="5" Actions="Add, Remove" Visible="False" />
            <uc:TextBox ID="txtAdditionalEmails" runat="server" Label="Additional Emails:" TextMode="MultiLine" Rows="6" Columns="55" Visible="False" />
            <uc:DropDownList ID="ddlAlerterUserStatus" runat="server" Label="Status:" />
            <uc:TextBox ID="txtComment" runat="server" Label="Comment:" TextMode="MultiLine" Rows="6" Columns="55" />
            <uc:Searcher ID="EmailSearcher" runat="server" />
        </div>
        <div style="margin-top: 20px; margin-bottom: 10px; text-align: center;">
            <asp:Button ID="btnOK" runat="server" Text="OK" SkinID="blue" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
        </div>
    </div>
</div>
