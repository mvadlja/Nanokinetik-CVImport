<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPopupFormVER.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormVER" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" tagname="TextArea_CT" tagprefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_VER">
        
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
                            <asp:Panel ID="pnlVER" runat="server">
                            <customControls:TextBox_CT 
                                    ID="txtVersionNumber" 
                                    runat="server" 
                                    LabelColumnWidth="150px" 
                                    MaxLength="4" 
                                    ControlInputWidth="300px" 
                                    ControlLabel="Version number:" 
                                    IsMandatory="true" 
                                    ControlErrorMessage="Version number must be numerical" 
                                    ControlEmptyErrorMessage="Version number can't be empty."/>
                            <customControls:DateBox_CT 
                                    ID="dbxEffectiveDate" 
                                    runat="server" 
                                    LabelColumnWidth="150px" 
                                    MaxLength="300" 
                                    ControlInputWidth="150px" 
                                    ControlLabel="Effective date:" 
                                    IsMandatory="false"
                                    ControlValueFormat="dd.MM.yyyy"
                                    ControlErrorMessage="Effective date is not a valid date." />
                            <customControls:TextArea_CT
                                    ID="txtChangeMade" 
                                    runat="server" 
                                    LabelColumnWidth="150px" 
                                    MaxLength="300" 
                                    ControlInputWidth="300px" 
                                    ControlLabel="Change made:" 
                                    IsMandatory="false"/>

	                        
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>
