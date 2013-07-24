<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormST.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormST" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_small">
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
                            <asp:Panel ID="pnlSubtype" runat="server" Visible="true">
                                <br />
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="150" style='table-layout:fixed' align="right">
                                        <asp:Label ID="lblSubtypeTitle" runat="server">Substance class subtype:</asp:Label>
                                        <asp:Label ID="lblSubtypeReq" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                        <td>
                                        <customControls:DropDownList_CT ID="tbxSubstanceClassSubtype" runat="server" LabelColumnWidth="150px"
                                        ControlInputWidth="200px" ControlLabel="" IsMandatory="false"
                                        ControlErrorMessage="" ControlEmptyErrorMessage="Substance class subtype can't be empty" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </asp:Panel>
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
