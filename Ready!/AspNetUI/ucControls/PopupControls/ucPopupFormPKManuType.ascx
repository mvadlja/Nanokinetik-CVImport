<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormPKManuType.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormPKManuType" %>

<%@ Register Src="~/ucControls/PopupControls/ucSearcherSUBST.ascx" TagName="ucSearcherSUBST" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" tagname="SearcherDisplay" tagprefix="operational" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal"  >

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_RS" style="width:700px" >
        
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content"  style="width:700px">

            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlManuTypes" runat="server" Visible="true">
                                <customControls:DropDownList_CT ID="ctlManufacters" runat="server" LabelColumnWidth="170px" ControlInputWidth="433px" ControlLabel="Manufacturer: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Manufacturer can't be empty."  />
                                <customControls:DropDownList_CT ID="ctlManufacterTypes" runat="server" LabelColumnWidth="170px" ControlInputWidth="433px" ControlLabel="Manufacturer type: " IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Manufacturer type can't be empty." AutoPostback="true" />
                                <div style="margin-top: -2px; margin-bottom: -15px;" id="ctlSubstanceSelector">
                                    <table cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td style="padding-left: 0px; text-align:right;">
                                                <asp:Label ID="ctlSubstanceLabel" runat="server" Text="Active substance:" width="174px"  visible="false"  />
                                            </td>
                                            <td>
                                                <div style="margin-left: 8px">
                                                    <customControls:ucSearcherSUBST ID="ctlSubstanceSearcher" runat="server" visible="false" />
                                                    <operational:SearcherDisplay ID="ctlSubstanceSearcherDisplay" runat="server" visible="false" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="center" style="margin-top:12px;">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>

