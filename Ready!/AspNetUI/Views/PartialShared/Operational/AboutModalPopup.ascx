<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AboutModalPopup.ascx.cs" Inherits="AspNetUI.Support.AboutModalPopup" %>

<div id="aboutModalPopupContainer" runat="server" class="modal">

    <div id="aboutModalPopupContainerContent" class="modal_container">
        
        <div class="modal_title_bar">
            <div id="divHeader">About Ready!</div>
            <asp:LinkButton ID="lbtClose" runat="server" onclick="btnClose_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <a href="http://www.nanokinetik.com" target="_blank" class="logo">
                <asp:Image ID="imgPossLogo" runat="server" ImageUrl="~/Images/logo_possimus.png" />
            </a>

            <b>Ready! &copy; 2013</b><br />
            Version <asp:Label ID="lblVersion" runat="server"></asp:Label><br />
            <br />
            &copy; Nanokinetik. All rights reserved.<br />
            <a href="http://www.nanokinetik.com" target="_blank">www.nanokinetik.com</a><br /><br /><br />

            <asp:Button ID="btnOk" runat="server" Text="Close" onclick="btnOk_Click" UseSubmitBehavior="false" CssClass="center_button" />
        </div>
    </div>
</div>
