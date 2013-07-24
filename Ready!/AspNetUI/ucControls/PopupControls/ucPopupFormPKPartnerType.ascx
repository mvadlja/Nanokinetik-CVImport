<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormPKPartnerType.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormPKPartnerType" %>

<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_RS">
        
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlPartnerTypes" runat="server" Visible="true">
                                <customControls:DropDownList_CT ID="ctlPartners" runat="server" LabelColumnWidth="200px" ControlInputWidth="302px" ControlLabel="Partner: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Partner can't be empty."  />
                                <customControls:DropDownList_CT ID="ctlPartnerTypes" runat="server" LabelColumnWidth="200px" ControlInputWidth="302px" ControlLabel="Partner type: " IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Partner type can't be empty." />
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>