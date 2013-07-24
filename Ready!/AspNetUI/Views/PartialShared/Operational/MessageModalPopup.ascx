<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageModalPopup.ascx.cs" Inherits="AspNetUI.Support.MessageModalPopup" %>


<div id="messageModalPopupContainer" runat="server" class="modal">

    <div id="messageModalPopupContainerContent" class="modal_container_small">
        
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" onclick="btnClose_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <div id="divMessage" runat="server" />
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="Close" onclick="btnOk_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>



<%--<center>
    <div id="messageModalPopupContainer" runat="server" style="display: none;">
        
        <div class="modalOverlay"></div>
        <div id="messageModalPopupContainerContent" class="modalPopupPanel">
            <center>
                <table cellpadding="0" cellspacing="0" class="modalPopupContainerContentInner">
                    <tr class="modalPopupContainerHeader">
                        <td>
                            <center>
                                <div style="float: left; vertical-align: middle; padding: 2px 0px 0px 5px;">
                                     <div id="divHeader" runat="server" />
                                </div>
                                <div style="float: right; vertical-align: middle; padding: 0px 5px 0px 0px;">
                                    <asp:LinkButton ID="lbtClose" runat="server" onclick="btnClose_Click">                             
                                        <asp:Image ID="imgClose" runat="server" ImageUrl="../ModalPopupContainer_Resources/close.gif" style="vertical-align: middle; cursor: pointer;" />
                                    </asp:LinkButton>
                                </div>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div class="modalPopupContainerBody">
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
                                                <div style="text-align: center">
                                                    <asp:Button ID="btnOk" runat="server" Text="Close" onclick="btnOk_Click" UseSubmitBehavior="false" />
                                                    <%--<input id="btnOk" runat="server" type="button" value="Close" onclick="HideModalPopup('messageModalPopupContainer');"  />--%>
    <%--                                            </div>
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
