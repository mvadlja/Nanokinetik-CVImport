<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormISO.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormISO" %>

<%@ Register Src="~/ucControls/PopupControls/ucSearcher.ascx" TagName="ucSearcher" TagPrefix="uc1" %>
<%@ Register src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" tagname="SearcherDisplay" tagprefix="operational" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_ISO">
        
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
                            <asp:Panel ID="pnlSearchEVCODE" runat="server">
                                <table cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td align="right" style="width: 160px;">
                                            <asp:Label ID="Label1" Text="Substance ID:" runat="server"></asp:Label>
                    
                                        </td>
                                        <td>
                                            <uc1:ucSearcher ID="EVCODESearcher" runat="server" />
                                            <operational:SearcherDisplay ID="EVCODESearcherDisplay" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <customControls:TextBox_CT ID="ctlNuclideName" runat="server" LabelColumnWidth="150px" MaxLength="300" ControlInputWidth="300px" ControlLabel="ID" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Nuclide name can't be empty." />
	                        <customControls:DropDownList_CT ID="ctlSubstitutionType" runat="server" LabelColumnWidth="150px" ControlInputWidth="300px" ControlLabel="Substitution type" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Substitution type can't be empty."/>
    
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>
