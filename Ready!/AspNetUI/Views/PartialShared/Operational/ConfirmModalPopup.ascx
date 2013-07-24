<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmModalPopup.ascx.cs" Inherits="AspNetUI.Support.ConfirmModalPopup" %>


<div id="confirmModalPopupContainer" runat="server" class="modal">

    <div id="confirmModalPopupContainerContent" class="modal_container_small">
        
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="imgClose" runat="server" OnClientClick="AnswerConfirmModalPopup('no');" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <div id="divMessage" runat="server" />
            <input id="hiddenConfirmModalPostbackControlID" value="" type="hidden" />
            <input id="hiddenConfirmModalPostbackArgument" value="" type="hidden" />
            <div class="center">
                <asp:Button ID="btnYes" runat="server" Text=" Yes " OnClientClick="AnswerConfirmModalPopup('yes'); return false;" UseSubmitBehavior="false" />
                <input id="btnNo" type="button" value=" No " onclick="AnswerConfirmModalPopup('no');" />
            </div>
        </div>

    </div>
        
</div>







<%--<center>
    <div id="confirmModalPopupContainer" runat="server" style="display: none;">
        
        <div class="modalOverlay"></div>
        <div id="confirmModalPopupContainerContent" class="modalPopupPanelSmall">
            <center>
                <table cellpadding="0" cellspacing="0" class="modalPopupContainerContentInnerSmall">
                    <tr class="modalPopupContainerHeader">
                        <td>
                            <center>
                                <div style="float: left; vertical-align: middle; padding: 2px 0px 0px 5px;">
                                     <div id="divHeader" runat="server" />
                                </div>
                                <div style="float: right; vertical-align: middle; padding: 0px 5px 0px 0px; cursor: pointer;" onclick="AnswerConfirmModalPopup('no');">
                                    <asp:Image ID="imgClose" runat="server" ImageUrl="../ModalPopupContainer_Resources/close.gif" style="vertical-align: middle;" />
                                </div>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div class="modalPopupContainerBodySmall">
                                    <table id="modalPopupContainerBody" runat="server" cellpadding="0" cellspacing="0" style="padding: 10px;">
                                        <tr>
                                            <td>
                                                <div id="divMessage" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input id="hiddenConfirmModalPostbackControlID" value="" type="hidden" />
                                                <input id="hiddenConfirmModalPostbackArgument" value="" type="hidden" />
                                                <div style="text-align: center">
                                                    <asp:Button ID="btnYes" runat="server" Text=" Yes " OnClientClick="AnswerConfirmModalPopup('yes'); return false;" UseSubmitBehavior="false" />
                                                    <input id="btnNo" type="button" value=" No " onclick="AnswerConfirmModalPopup('no');" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        
    </div>
</center>--%>