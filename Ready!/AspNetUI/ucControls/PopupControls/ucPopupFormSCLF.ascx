<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormSCLF.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormSCLF" %>

<%@ Register Src="~/ucControls/PopupControls/ucPopupFormRS.ascx" TagName="ucPopupFormRS" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormST.ascx" TagName="ucPopupFormST" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" tagname="ListBox_CT" tagprefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_SCLF">
        
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" HorizontalAlign="Center" Width="100%">
                <br />
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlSclf" runat="server" Visible="true">
                                <br />
                                <customControls:DropDownList_CT ID="ctlSclfDomain" runat="server" AutoPostback ="false" EnableViewState="true" Visible="true" LabelColumnWidth="150px" ControlInputWidth="333px" ControlLabel="Domain:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Domain can't be empty" />
                                <customControls:DropDownList_CT ID="ctlSclf" runat="server" AutoPostback ="true" EnableViewState="true" LabelColumnWidth="150px" ControlInputWidth="333px" ControlLabel="Substance classification:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Substance classification can't be empty." OnInputValueChanged="ctlSclfInputValueChanged" />
                                <customControls:DropDownList_CT ID="ctlSclfType" runat="server" LabelColumnWidth="150px" ControlInputWidth="333px" ControlLabel="Type:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Substance classification type can't be empty." />
                                <customControls:TextBox_CT ID="ctlSclfCode" runat="server" LabelColumnWidth="150px" MaxLength="250" ControlInputWidth="327px" ControlLabel="ID: " IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                <asp:Panel ID="pnlSubtype" runat="server">
                                 <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="160" style='table-layout:fixed' align="right">
                                        
                                        <asp:Label ID="lblSubtypeTitle" runat="server">Subtype:</asp:Label>
                                            <asp:Label ID="lblSubtypeReq" runat="server" ForeColor="Red">*</asp:Label></td>
                                        <td valign="top" rowspan="3">
                                            <div class="marginLeft_10" style="margin-left:-1px">
                                                <customControls:ListBox_CT ID="ctlSt" runat="server" ControlMultipleValueSelection="true"
                                                    LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlStInputValueChanged"
                                                     />
                                            </div>
                                        </td>
                                        <td valign="middle">
                                            <asp:Button ID="btnAddSt" runat="server" Text="Add" Width="75px" OnClick="btnAddStOnClick"/>
                                            <uc1:ucPopupFormST ID="UcPopupFormST" runat="server" ViewStateMode="Enabled" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td valign="middle">
                                            <asp:Button ID="btnEditSt" runat="server" Text="Edit" Width="75px" 
                                                Enabled="false" OnClick="btnEditStOnClick"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td valign="middle">
                                            <asp:Button ID="btnRemoveSt" runat="server" Text="Remove" Width="75px" 
                                                Enabled="false" OnClick="btnRemoveStOnClick"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp
                                        </td>
                                    </tr>
                                </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlRs" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="160" style='table-layout:fixed' align="right"><asp:Label ID="lblRefSourceTitle" runat="server">Reference source:</asp:Label>
                                            <asp:Label ID="lblRefSourceReq" runat="server" ForeColor="Red">*</asp:Label></td>
                                            <td valign="top" rowspan="3">
                                                <div class="marginLeft_10" style="margin-left:-1px">
                                                    <customControls:ListBox_CT ID="ctlRs" runat="server" ControlMultipleValueSelection="true"
                                                        LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlRsInputValueChanged"
                                                         />
                                                </div>
                                            </td>
                                            <td valign="middle">
                                                <asp:Button ID="btnAddRs" runat="server" Text="Add" Width="75px" OnClick="btnAddRsOnClick"/>
                                                <uc1:ucPopupFormRS ID="UcPopupFormRs" runat="server" ViewStateMode="Enabled" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td valign="middle">
                                                <asp:Button ID="btnEditRs" runat="server" Text="Edit" Width="75px" OnClick="btnEditRsOnClick" 
                                                    Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td valign="middle">
                                                <asp:Button ID="btnRemoveRs" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveRsOnClick"
                                                    Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                &nbsp
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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