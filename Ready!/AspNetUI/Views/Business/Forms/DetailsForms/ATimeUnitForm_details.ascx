<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ATimeUnitForm_details.ascx.cs" Inherits="AspNetUI.Views.ATimeUnitForm_details" %>
<!-- Operational controls -->
<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>
<!-- Custom input controls -->
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" TagName="TextArea_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/ucControls/PopupControls/ucSearcher.ascx" TagName="ucSearcher" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucConfirmAction.ascx" TagName="ucConfirmAction" TagPrefix="uc" %>

<uc:ucConfirmAction ID="ConfirmAction" runat="server"  />

<asp:Panel ID="pnlDataDetails" runat="server"  CssClass="preventableClose">
<customControls:Label_CT ID="ctlID" runat="server" ControlLabelAlign="left" TotalControlWidth="100%" visible="false"/>
  <asp:Panel ID="pnlSearchActivity" runat="server">
        <table cellpadding="2" cellspacing="0">
            <tr>
                <td>
                    <div style="margin-left: -3px">
                        <customControls:Label_CT ID="Label_CT2" runat="server" ControlLabel="Activity:" LabelColumnWidth="155px" ControlLabelAlign="right" IsMandatory="false" />
                    </div>
                </td>
                <td>
                    <div style="margin-left: -68px">
                        <uc1:ucSearcher ID="ActivitySearcher" runat="server" />
                        <operational:SearcherDisplay ID="ActivitySearcherDisplay" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <%--<customControls:TextBox_CT ID="ctltask_FK" runat="server" LabelColumnWidth="150px" MaxLength="300" ControlInputWidth="300px" ControlLabel="task_FK" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />--%>
    <customControls:DropDownList_CT ID="ctltime_unit_name_FK" runat="server" LabelColumnWidth="150px" ControlInputWidth="336px" ControlLabel="Name:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Name can't be empty." />
    <customControls:DropDownList_CT ID="ctlresponsible_user" runat="server" LabelColumnWidth="150px" ControlInputWidth="336px" ControlLabel="Responsible user:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
    <customControls:TextArea_CT ID="ctlDescription" runat="server" LabelColumnWidth="150px" MaxLength="50" ControlInputWidth="330px" ControlLabel="Description:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
  
    <customControls:DateBox_CT ID="ctlactual_date" runat="server" LabelColumnWidth="150px" ControlLabel="Actual date:" IsMandatory="false" ControlValueFormat="dd.MM.yyyy" ControlEmptyErrorMessage="" ControlErrorMessage="Actual date is not valid date." />
    <table>
        <tr>
            <td>
                <div style="margin-left: -3px;padding-right:5px;">
                    <customControls:TextBox_CT ID="ctltime_hours" runat="server" LabelColumnWidth="150px" MaxLength="330" ControlInputWidth="30px" ControlLabel="Time:" IsMandatory="false" ControlErrorMessage="Time hours is not valid number."
                        ControlEmptyErrorMessage="" />
                </div>
            </td>
            <td>
                <div style="margin-left: -60px">
                    <asp:Label ID="lbl_time_hours" Text="Hours" runat="server" />
                </div>
            </td>
            <td>
                <div style="margin-left: -40px">
                    <customControls:TextBox_CT ID="ctltime_minutes" runat="server" LabelColumnWidth="0px" MaxLength="300" ControlInputWidth="30px" ControlLabel="" IsMandatory="false" ControlErrorMessage="Time minutes is not valid number."
                        ControlEmptyErrorMessage="" />
                </div>
            </td>
            <td>
                <div style="margin-left: -60px">
                    <asp:Label ID="lbl_time_mins" Text="Mins" runat="server" />
                </div>
            </td>
        </tr>
    </table>
    <customControls:TextArea_CT ID="ctlcomment" runat="server" LabelColumnWidth="150px" MaxLength="50" ControlInputWidth="330px" ControlLabel="Comment:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
    <br />
    <table>
        <tr>
            <td width="162px">
                &nbsp;
            </td>
            <td align="center">
                &nbsp;
            </td>
        </tr>
    </table>
    <%--<customControls:TextBox_CT ID="ctlactivity_FK" runat="server" LabelColumnWidth="150px" MaxLength="300" ControlInputWidth="300px" ControlLabel="activity_FK" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />--%>
</asp:Panel>
<asp:Panel ID="pnlTimeUnitProperties" runat="server">
    <table>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="Label1" runat="server" Font-Bold="false">Products:</asp:Label>
            </td>
            <td class="wordbreakProperty">
                <asp:Label ID="Label2" runat="server" Font-Bold="True"></asp:Label><div id="divProductsLinks" runat="server" style="font-weight: bold">
                </div>
            </td>
        </tr>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="Label3" runat="server" Font-Bold="false">Activity:</asp:Label>
            </td>
            <td class="wordbreakProperty">
                <asp:Label ID="Label4" runat="server" Font-Bold="True"></asp:Label><div id="divActivityLink" runat="server" style="font-weight: bold">
                </div>
            </td>
        </tr>
    </table>
    <hr />
    <table>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="lblTaskNameTitle" runat="server" Font-Bold="false">Name:</asp:Label>
                <asp:Label ID="lblTaskNameTitleReq" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTaskName" runat="server" Font-Bold="True" Visible="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="lblResponsibleUserTitle" runat="server" Font-Bold="false">Responsible user:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblResponsibleUser" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="Label5" runat="server" Font-Bold="false">Inserted by:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblInsertedBy" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="lblDescriptionTitle" runat="server" Font-Bold="false">Description:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblDescription" runat="server" Font-Bold="True" valign="top"> </asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="lblActualDateTitle" runat="server" Font-Bold="false">Actual date:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblActualDate" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="lblTimeTitle" runat="server" Font-Bold="false">Time:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTime" Font-Bold="True" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="lblCommentTitle" runat="server" Font-Bold="false">Comment:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblComment" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4" valign="top">
                <asp:Label ID="lblLastChangeTitle" runat="server" Font-Bold="false">Last change:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblLastChange" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="bottom_controls">
        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button Delete" OnClick="btnDeleteOnClick"></asp:Button>
    </div>
    <asp:Label ID="FirstBind" runat="server" Visible="false" Text="0"/>
</asp:Panel>
<asp:Panel runat="server" ID="pnlFooter" CssClass="preventableClose">
    <div class="bottomControlsHolder" valign="center">
        <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="button Cancel" OnClick="btnCancelOnClick"></asp:Button>
        <asp:Button ID="Button2" runat="server" Text="Save" CssClass="button Save" OnClick="btnSaveOnClick"></asp:Button>
    </div>
</asp:Panel>