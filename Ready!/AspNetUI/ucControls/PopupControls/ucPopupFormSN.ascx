<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPopupFormSN.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormSN" %>

<%@ Register Src="~/ucControls/PopupControls/ucPopupFormRS.ascx" TagName="ucPopupFormRS" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormON.ascx" TagName="ucPopupFormON" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" tagname="ListBox_CT" tagprefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_SN">

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
                            <asp:Panel ID="pnlSubName" runat="server" Visible="true" >
                                <br />
                                <div style="margin-left:-17px">
                                <customControls:TextBox_CT ID="ctlSubName" runat="server" LabelColumnWidth="150px" MaxLength="4000" ControlInputWidth="195px" ControlLabel="Name: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Name can't be empty." />
                                <customControls:DropDownList_CT ID="ctlSubNameType" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Name type: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Name type can't be empty." OnInputValueChanged="ctlNameTypeInputValueChanged" AutoPostback="true" />
                                <customControls:DropDownList_CT ID="ctlLanguage" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Language: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Language can't be empty."  AutoPostback="true" />
                                </div>
                                <br />
                                <asp:Panel ID="pnlOfficialName" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="150" style='table-layout:fixed' align="right">
                                            <div style="margin-right:7px">
                                            <asp:Label ID="lblONTitle" runat="server">Official name: </asp:Label>
                                            <asp:Label ID="lblONReq" runat="server" ForeColor="Red" Visible=false>*</asp:Label>
                                            </div>
                                            </td>
                                            <td valign="top" rowspan = "3"><customControls:ListBox_CT ID="ctlON"  runat="server" ControlMultipleValueSelection = "true" LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlONListInputValueChanged"/> </td>
                                            <td valign="middle"><asp:Button ID="btnAddON" runat="server" Text="Add" Width="75px" OnClick="btnAddONOnClick"/>
                                            <uc1:ucPopupFormON ID="ONPopupForm" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle"><asp:Button ID="btnEditON" runat="server" Text="Edit" Width="75px" OnClick="btnEditONOnClick"/></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle"><asp:Button ID="btnRemoveON" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveONOnClick"/></td>
                                        </tr>
                                    </table>
                                    <br />
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="pnlRefSource" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="150" style='table-layout:fixed' align="right">
                                            <div style="margin-right:8px">
                                                <asp:Label ID="lblRefSourceTitle" runat="server">Reference source:</asp:Label>
                                                <asp:Label ID="lblRefSourceReq" runat="server" ForeColor="Red">*</asp:Label>
                                            </div>
                                            </td>
                                            <td valign="top" rowspan = "3">
                                                <div class="marginLeft_10">
                                                    <customControls:ListBox_CT ID="ctlRefSources"  runat="server" ControlMultipleValueSelection = "true" LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlRefSourcesListInputValueChanged"/> 
                                                </div>
                                            </td>
                                            <td valign="middle"><asp:Button ID="btnAddRefSource" runat="server" Text="Add" Width="75px" OnClick="btnAddRefSourceOnClick"/>
                                            <uc1:ucPopupFormRS ID="RefSourcesPopupForm" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle"><asp:Button ID="btnEditRefSource" runat="server" Text="Edit" Width="75px" OnClick="btnEditRefSourceOnClick"/></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle"><asp:Button ID="btnRemoveRefSources" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveRefSourcesOnClick"/></td>
                                        </tr>
                                    </table>
                                    <br />
                                </asp:Panel>
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>