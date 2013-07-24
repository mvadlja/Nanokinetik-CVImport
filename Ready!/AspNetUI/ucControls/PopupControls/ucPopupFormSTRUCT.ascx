<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormSTRUCT.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormSTRUCT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="toolkit" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormISO.ascx" TagName="ucPopupFormISO" TagPrefix="uc1" %>

<div id="PopupControls_Struct_Container" runat="server" class="modal">
    <div id="PopupControls_Struct_ContainerContent" class="modal_container">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <br />
            <customControls:DropDownList_CT ID="ctlStructRepresentationType" runat="server" LabelColumnWidth="200px"
                ControlInputWidth="333px" ControlLabel="Structural representation type:" IsMandatory="true"
                ControlErrorMessage="" ControlEmptyErrorMessage="Structural representation type can't be empty." />
            <customControls:TextBox_CT ID="ctlStructRepresentation" runat="server" LabelColumnWidth="200px"
                MaxLength="300" ControlInputWidth="327px" ControlLabel="Structural representation:"
                IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Structural representation can't be empty." />
            <asp:Panel ID="pnlUploadFiles" Style="padding-left: 135px;" runat="server">
                <table style="left">
                    <tr>
                        <td>
                            <asp:Label ID="ctlAttachment" Text="Attachment:" runat="server" />
                        </td>
                        <td width ="9px" />
                        <td>
                            <asp:UpdatePanel runat="server" ID="upMain">
                                <ContentTemplate>
                                    <toolkit:AsyncFileUpload ID="FileUpload2" ThrobberID="Throbber" OnClientUploadComplete="uploadComplete"
                                        ClientIDMode="AutoID" runat="server" UploadingBackColor="#CCFFFF" OnUploadedComplete="AsyncFileUpload1_UploadedComplete" />
                                    <asp:Label runat="server" ID="lblUploadedFile" Text="" Visible="false"></asp:Label>
                                    <asp:GridView ID="gvData" runat="server" AllowSorting="False" AutoGenerateColumns="true"
                                        DataKeyNames="struct_repres_attach_PK" OnRowCommand="gvData_RowCommand" OnRowDataBound="gvData_RowDataBound"
                                        ShowHeader="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Attachment name">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlName" runat="server" CssClass="gvLinkTextBold" Text='<%# Eval("attachmentname") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle Width="350" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDownload" runat="server" CommandArgument='<%# Eval("struct_repres_attach_PK") %>'
                                                        CommandName="Download" Text="Download" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="ibtDeleteItem" runat="server" CommandArgument='<%# Eval("struct_repres_attach_PK") %>'
                                                        CommandName="Delete" OnClientClick='<%# "DisplayConfirmModalPopup(\"Warning!\", \"Are you sure that you want to delete this record?\", this, " + Container.DataItemIndex + "); return false;" %>'
                                                        ToolTip="Delete">
                                                        <asp:Image ID="imgDel" runat="server" ImageUrl="~/Images/delete.gif" /></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="Throbber" runat="server" Style="display: none">
                                <asp:Image ID="Image1" ImageUrl="~/Images/loading.gif" runat="server" /></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <customControls:DropDownList_CT ID="ctlStereochemistry" runat="server" LabelColumnWidth="200px"
                ControlInputWidth="333px" ControlLabel="Stereochemistry :" IsMandatory="true"
                ControlErrorMessage="" ControlEmptyErrorMessage="Stereochemistry can't be empty." />
            <customControls:TextBox_CT ID="ctlOpticalActivity" runat="server" LabelColumnWidth="200px"
                MaxLength="300" ControlInputWidth="327px" ControlLabel="Optical activity: " IsMandatory="true"
                ControlErrorMessage="" ControlEmptyErrorMessage="Optical activity can't be empty." />
            <customControls:TextBox_CT ID="ctlMolecularFormula" runat="server" LabelColumnWidth="200px"
                MaxLength="300" ControlInputWidth="327px" ControlLabel="Molecular formula: "
                IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Molecular formula can't be empty." />
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="200" style='table-layout: fixed' align="right">
                        <asp:Label ID="lblISOTitleDetails" runat="server">Isotope:</asp:Label>
                    </td>
                    <td valign="top" rowspan="3">
                        <customControls:ListBox_CT ID="ctlISO" runat="server" ControlMultipleValueSelection="true"
                            LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                            IsMandatory="false" OnInputValueChanged="ctlISOListInputValueChanged" />
                    </td>
                    <td valign="middle">
                        <asp:Button ID="btnAddISO" runat="server" Text="Add" Width="75px" OnClick="btnAddISOOnClick" />
                        <uc1:ucPopupFormISO ID="PopupFormISO" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td valign="middle">
                        <asp:Button ID="btnEditISO" runat="server" Text="Edit" Width="75px" OnClick="btnEditISO_Click"
                            Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td valign="middle">
                        <asp:Button ID="btnRemoveISO" runat="server" Text="Remove" OnClick="btnRemoveISOOnClick"
                            Enabled="false" Width="75px" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" OnClick="btnOk_Click"
                    UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click"
                    UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>