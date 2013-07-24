<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormPREEVCODE.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormPREEVCODE" %>

<%@ Register Src="~/ucControls/PopupControls/ucSearcher.ascx" TagName="ucSearcher" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>

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
                            <asp:Panel ID="pnlSearchEVCODE" runat="server">
                                <table cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td align="right" style="width: 150px; text-align: right;">
                                            <asp:Label ID="Label6" Text="DEVEVCODE:" runat="server"></asp:Label>
                                            <asp:Label ID="Label1" Text="*" ForeColor="Red" runat="server"></asp:Label>
                                        </td>
                                        <td width="6px" />
                                        <td>
                                            <uc1:ucSearcher ID="EVCODESearcher" runat="server" />
                                            <operational:SearcherDisplay ID="EVCODESearcherDisplay" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
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
