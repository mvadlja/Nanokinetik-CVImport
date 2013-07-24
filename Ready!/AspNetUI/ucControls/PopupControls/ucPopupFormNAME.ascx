<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormNAME.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormNAME" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/CheckBoxList_CT.ascx" TagName="CheckBoxList_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_NAME">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <customControls:TextBox_CT runat="server" ID="ctlSearchName" ControlLabel="Quick link name"
                    LabelColumnWidth="150px" IsMandatory="true" ControlEmptyErrorMessage="Quick link name can't be empty" />
                <%-- <asp:Label runat="server" Text="Public search" /><asp:CheckBox runat="server" ID="cbxPublic" />--%>
                <table>
                    <tr>
                        <td>
                            <customControls:CheckBoxList_CT ID="CheckBoxList_CT1" runat="server" LabelColumnWidth="150px"
                                ControlLabel="Public" />
                        </td>
                        <td >
                            <asp:CheckBox runat="server" ID="cbxPublic" style="margin-left:-66px;" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_Click"
                    UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px"
                    OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
