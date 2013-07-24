<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.AlerterView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeBox.ascx" TagName="DateTimeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/CheckBox.ascx" TagName="CheckBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxExt.ascx" TagName="ListBoxExt" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Searcher.ascx" TagName="Searcher" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register TagPrefix="act" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.50927.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Alert:" Visible="True" />
     </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" Visible="True" />
    </div>
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:LabelPreview ID="lblPrvReminderType" runat="server" Label="Type:" TextBold="false" />
                <uc:LabelPreview ID="lblPrvRelatedAttributeName" runat="server" Label="Related date name:" TextBold="false" />
                <uc:LabelPreview ID="lblPrvRelatedAttributeValue" runat="server" Label="Related date:" TextBold="false" />
                <br/>
                <uc:TextBox ID="txtTimeBeforeActivation" runat="server" Label="Days Before:" AutoPostback="true" />
                <div style="margin-left: 91px; position: relative;">
                    <span style="padding-top: 3px; display: inline-block;">Alert date:</span><span style="display: inline-block; margin-left: 5px; margin-right: 5px; color: red; vertical-align: top;">*</span>
                    <div style="display: inline-block; vertical-align: top;">
                        <asp:TextBox runat="server" ID="txtReminderDate" Width="70" AutoPostBack="True" />
                        <span style="vertical-align: middle; display: inline-block;">
                            <asp:Image ID="imgDateTime" runat="server" ImageUrl="~/Images/calendar.png" />
                        </span>
                    </div>
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
            </fieldset>
        </div>
        <div id="divRightPane" class="rightPane" style="margin-top: 0px; margin-left: -74px;">
            <fieldset>
            </fieldset>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlFooter" runat="server" class="bottom clear bottomControlsHolder" valign="center">
        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="button Cancel" OnClick="btnCancel_OnClick" />
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="button Save" OnClick="btnSave_OnClick" />
    </asp:Panel>
</asp:Content>
