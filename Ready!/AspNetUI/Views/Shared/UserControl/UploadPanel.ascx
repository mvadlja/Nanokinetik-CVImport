<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="UploadPanel.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.UploadPanel" EnableViewState="true" %>
<%@ Register TagPrefix="toolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.50927.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<div id="divHrHolder" runat="server">
    <hr style="position: absolute; width: 100%; left: 0px;" />
</div>
<div id="divUploadPanel" runat="server" class="row margin-top-20">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="pnlUploadFilesMain"></asp:Label>
        <span id="spanRequired" runat="server" visible="False">*</span>
    </div>
    <div class="input">
        <asp:Panel ID="pnlUploadFilesMain" runat="server">
            <asp:Panel ID="pnlUploadedFiles" runat="server" Width="600" Visible="true">
                <asp:GridView ID="gvData" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" CellSpacing="0" CellPadding="0" ShowHeader="false" DataKeyNames="attachment_PK" AllowSorting="False" BorderWidth="0" CssClass="gridStyle" HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow" AlternatingRowStyle-CssClass="gridAlternateRow" EmptyDataRowStyle-CssClass="EmptyRowErr">
                    <Columns>
                        <asp:TemplateField HeaderText="Attachment name" SortExpression="attachmentname" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlName" runat="server" Font-Bold="true" CssClass="gvLinkTextBold" Text='<%# Eval("attachmentname") %>'></asp:HyperLink>
                                <span id="spanCheckOutStatus" runat="server" class="check-status"></span>
                            </ItemTemplate>
                            <ItemStyle Width="350" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDownload" ClientIDMode="AutoID" runat="server" Text="" ToolTip="Download" CssClass="download gvLinkTextBold" CommandName="Download" CommandArgument='<%# HandleDocumentArguments(Eval("attachment_PK"), Eval("EDMSDocumentId"), Eval("EDMSBindingRule"), Eval("EDMSAttachmentFormat")) %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div id="divCheckAction" runat="server">
                                    <asp:LinkButton ID="btnCheckOut" runat="server" Text="" ToolTip="Check out" CssClass="check-action-out gvLinkTextBold" CommandName="CheckOut" CommandArgument='<%# Eval("attachment_PK") %>' Visible="False"></asp:LinkButton>
                                    <toolkit:AsyncFileUpload ID="asyncCheckIn" ThrobberID="CheckInThrobber" ClientIDMode="Inherit" runat="server" UploadingBackColor="#CCFFFF" CssClass="check-action-upload" Visible="False"/>
                                    <asp:Label ID="CheckInThrobber" ClientIDMode="AutoID" runat="server" Visible="False">
                                        <asp:Image ID="imgCheckInThrobber" ImageUrl="../../../Images/loading.gif" runat="server" />
                                    </asp:Label>
                                    <asp:LinkButton ID="btnCheckIn" ClientIDMode="AutoID" runat="server" Text="" ToolTip="Check in" CssClass="check-action-in gvLinkTextBold" CommandName="CheckIn" CommandArgument='<%# Eval("attachment_PK") %>' Visible="False"></asp:LinkButton>
                                    <asp:LinkButton ID="btnCancelCheckout" runat="server" Text="" ToolTip="Cancel checkout" CssClass="check-action-cancel gvLinkTextBold" CommandName="CancelCheckout" CommandArgument='<%# Eval("attachment_PK") %>' Visible="False"></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="check-action" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="imgBtnDeleteAttachment" Text="" ToolTip="Delete" ImageUrl="~/Images/delete.gif" CommandName="DeleteAttachment" CommandArgument='<%# Eval("attachment_PK") %>' CssClass="gvLinkTextBold" OnClick="ImgBtnDeleteAttachment_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <div class="td_attachments_input">
                <asp:Panel ID="pnlUploadFilesControl" runat="server" CssClass="imageUploaderField">
                    <asp:Panel ID="pnlAsyncUploadControl" runat="server">
                        <toolkit:AsyncFileUpload ID="asyncFileUpload" ThrobberID="Throbber" ClientIDMode="AutoID" OnClientUploadComplete="uploadComplete" OnClientUploadStarted="uploadStart" runat="server" UploadingBackColor="#CCFFFF" />
                    </asp:Panel>
                    <asp:Panel ID="pnlThrobber" runat="server">
                        <asp:Label ID="Throbber" runat="server">
                            <asp:Image ID="imgThrobber" ImageUrl="../../../Images/loading.gif" runat="server" />
                        </asp:Label>
                    </asp:Panel>
                </asp:Panel>
            </div>
        </asp:Panel>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>
