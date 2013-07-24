<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormRS.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormRS" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" tagname="TextArea_CT" tagprefix="customControls" %>
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
                            <asp:Panel ID="pnlRefSource" runat="server" Visible="true">
                                <table>
                                    <tr>
                                        <td/>    
                                        <td><div style="margin-left:3px"><asp:Label runat="server" ID="PDYes" Text="Yes" /></div></td>
                                        <td><div style="margin-left:3px"><asp:Label runat="server" ID="PDNo" Text="No" /></div></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <div style="margin-left:-3px">
                                            <asp:Label runat="server" ID="ctlPublicDomain" Text="Public domain: " Width="100px" CssClass="alignRight"/>
                                            <asp:Label runat="server" ID="asterix" Text="*" ForeColor="Red" />
                                        </div>
                                        </td>

                                        <td>
                                        <div style="margin-left:3px">
                                            <asp:RadioButton ID="RadioButtonYes" runat="server" AutoPostback ="false" GroupName="rbtPD" EnableViewState="true" Visible="true" LabelColumnWidth="150px" ControlLabel="Public domain: " ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                        </div>
                                        </td>
                                        <td> 
                                        <div style="margin-left:3px">
                                            <asp:RadioButton ID="RadioButtonNo" runat="server" AutoPostback ="false" GroupName="rbtPD" EnableViewState="true" Visible="true" LabelColumnWidth="150px" ControlLabel="Public domain: " ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                        </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />

                                <customControls:DropDownList_CT ID="ctlRefSourceType" runat="server" LabelColumnWidth="100px" ControlInputWidth="302px" ControlLabel="Type: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Reference source type can't be empty."  />
                                <customControls:DropDownList_CT ID="ctlRefSourceClass" runat="server" LabelColumnWidth="100px" ControlInputWidth="302px" ControlLabel="Class: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Reference source class can't be empty." />
                                <div style="margin-left:-2px">
                                <customControls:TextBox_CT ID="ctlRefSourceId" runat="server" LabelColumnWidth="100px" MaxLength="12" ControlInputWidth="150px" ControlLabel="ID:   " IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                </div>
                                <customControls:TextArea_CT ID="ctlRefSourceCitation" runat="server" LabelColumnWidth="126px" MaxLength="2500" ControlInputWidth="302px" ControlLabel="Citation:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
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
