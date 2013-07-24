<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormSUBALS.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormSUBALS" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" TagName="TextArea_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormALSTRN.ascx" TagName="ucPopupFormALSTRN" TagPrefix="uc1" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_ISO">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" class="text-align-center" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content disable-table-hover">
            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                           <%-- <customControls:TextBox_CT ID="ctlSourceCode" runat="server" LabelColumnWidth="150px"
                                ControlInputWidth="300px" ControlLabel="Language" IsMandatory="true" ControlErrorMessage=""
                                ControlEmptyErrorMessage="Language can't be empty." />--%>
                            <customControls:TextArea_CT ID="ctlAliasName" runat="server" LabelColumnWidth="161px" ControlInputWidth="294px"
                                ControlLabel="Alias name" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Alias name can't be empty."
                                MaxLength="2000" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlAliasTranslation" runat="server" GroupingText="Alias translation"
                    Visible="true" CssClass="fieldset-without-border">
                    <table cellpadding="0" cellspacing="0" >
                        <tr>
                            <td width="150" style='table-layout: fixed; text-align: right;' align="right">
                                <asp:Label ID="lblAliasTranslation" runat="server">Alias translation:</asp:Label>
                            </td>
                            <td valign="top" rowspan="3">
                                <div style="margin-left: -3px">
                                    <customControls:ListBox_CT ID="ctlAliasTranslation" runat="server" ControlMultipleValueSelection="true"
                                        LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                        OnInputValueChanged="ctlAliasTranslationListInputValueChanged" />
                                </div>
                            </td>
                            <td valign="middle">
                                <div style="margin-left: -23px">
                                    <asp:Button ID="btnAddAliasTranslation" runat="server" Text="Add" Width="75px" OnClick="btnAddAliasTranslationOnClick" />
                                    <uc1:ucPopupFormALSTRN ID="ucPopupFormALSSTRN" runat="server" ViewStateMode="Enabled" />
                                </div>
                            </td>
                        </tr>
                        <tr class="background-transparent">
                            <td>
                            </td>
                            <td valign="middle">
                                <div style="margin-left: -23px">
                                    <asp:Button ID="btnEditAliasTranslation" runat="server" Text="Edit" Width="75px" Enabled="false"
                                        OnClick="btnEditAliasTranslationOnClick" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td valign="middle">
                                <div style="margin-left: -23px">
                                    <asp:Button ID="btnRemoveAliasTranslation" runat="server" Text="Remove" Width="75px"
                                        Enabled="false" OnClick="btnRemoveAliasTranslationOnClick" />
                                </div>
                            </td>
                        </tr>
                        <tr class="background-transparent">
                            <td colspan="3">
                                &nbsp
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <br />
            <br />
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" OnClick="btnOk_Click"
                    UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px"
                    OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
