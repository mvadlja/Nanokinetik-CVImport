<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="PharmaceuticalProductForm.ascx.cs" Inherits="AspNetUI.Views.Shared.Form.PharmaceuticalProductForm" %>

<%@ Register Src="../UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../UserControl/LabelError.ascx" TagName="LabelError" TagPrefix="uc" %>
<%@ Register Src="../UserControl/ListBoxSr.ascx" TagName="ListBoxSr" TagPrefix="uc" %>
<%@ Register Src="../UserControl/Popup/PPSubstancePopup.ascx" TagName="PPSubstancePopup" TagPrefix="uc" %>
<%@ Register Src="../UserControl/Popup/XevprmValidationErrorPopup.ascx" TagName="XevprmValidationErrorPopup" TagPrefix="uc" %>
<%@ Register Src="../UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Panel ID="pnlForm" runat="server" class="form">
    <uc:XevprmValidationErrorPopup ID="XevprmValidationErrorPopup" runat="server" />
    <uc:PPSubstancePopup ID="PPSubstancePopup" runat="server" />
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <uc:ModalPopup ID="modalPopup" runat="server" />
    <div id="divLeftPane" class="leftPane">
        <fieldset>
            <uc:ListBoxSr ID="lbSrProducts" runat="server" Label="Products:" LabelWidth="150px" SelectionMode="Single" VisibleRows="5" TextWidth="336px" SearchType="Product" Actions="Add, Remove" />
            <uc:TextBox ID="txtPharmaceuticalProductName" runat="server" Label="Name:" MaxLength="450" LabelWidth="150px" TextWidth="330px" />
            <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" LabelWidth="150px" TextWidth="336px" />
            <uc:TextBox ID="txtDescription" runat="server" Label="Description:" MaxLength="50" LabelWidth="150px" TextWidth="330px" TextMode="MultiLine" Rows="5" />
            <uc:DropDownList ID="ddlPharmaceuticalForm" runat="server" Label="Pharmaceutical form:" Required="True" LabelWidth="150px" TextWidth="336px" />
            <uc:ListBoxSr ID="lbSrAdministrationRoutes" runat="server" Label="Administration routes:" LabelWidth="150px" SelectionMode="Single" VisibleRows="5" TextWidth="336px" SearchType="AdministrationRoute" Actions="Add, Remove" />
            <uc:ListBoxSr ID="lbSrMedicalDevices" runat="server" Label="Medical devices:" LabelWidth="150px" SelectionMode="Single" VisibleRows="5" TextWidth="336px" SearchType="MedicalDevice" Actions="Add, Remove" />
            <uc:TextBox ID="txtId" runat="server" Label="ID:" MaxLength="300" LabelWidth="150px" TextWidth="330px" />
            <uc:TextBox ID="txtBookedSlots" runat="server" Label="Booked slot(s):" MaxLength="50" LabelWidth="150px" TextWidth="330px" TextMode="MultiLine" Rows="5" />
            <uc:TextBox ID="txtComment" runat="server" Label="Comment:" MaxLength="50" LabelWidth="150px" TextWidth="330px" TextMode="MultiLine" Rows="5" />
            <br /><br />
        </fieldset>
    </div>
    <br/>
    <div id="divActiveIngredients" class="clear form-grid-border-bottom pp-grids-margins">
        <uc:TextBox ID="txtActiveIngredientsHidden" runat="server" Visible="False" />
        <div class="form-grid-header">
            <h2 class="bold-text-important">Active ingredients</h2>
            <asp:LinkButton ID="btnAddActiveIngredient" runat="server" Text="New" CssClass="button New ppLeaveThisPage" ></asp:LinkButton>
        </div>
        <possGrid:PossGrid ID="ActiveIngredientsGrid" GridId="ActiveIngredientsGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="ppsubstance_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="SubstanceName" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="10000" Position="None" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="ppsubstance_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="36%" Caption="Substance name" FieldName="SubstanceName" FilterType="Text">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbtnEditActiveIngredient" Text='<%# HandleMissing(Eval("SubstanceName")) %>' CommandName="EditActiveIngredient" CommandArgument='<%# Eval("ppsubstance_PK") %>' CssClass="gvLinkTextBold" OnClick="btnEditSubstance_OnClick"></asp:LinkButton>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <%--<possGrid:PossBoundField Width="6%" Caption="Conc. type" FieldName="ConcentrationType" FilterType="Text" />--%>
                <possGrid:PossBoundField Width="5%" Caption="LNum value" FieldName="LowNumValue" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="5%" Caption="LNum prefix" FieldName="LowNumPrefix" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="LNum unit" FieldName="LowNumUnit" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="LDen value" FieldName="LowDenValue" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="5%" Caption="LDen prefix" FieldName="LowDenPrefix" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="LDen unit" FieldName="LowDenUnit" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="HNum value" FieldName="HighNumValue" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="5%" Caption="HNum prefix" FieldName="HighNumPrefix" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="HNum unit" FieldName="HighNumUnit" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="HDen value" FieldName="HighDenValue" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="5%" Caption="HDen prefix" FieldName="HighDenPrefix" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="HDen unit" FieldName="HighDenUnit" FilterType="Text" />
                <possGrid:PossTemplateField Width="2%" Caption="" FieldName="Errors" FilterType="None" Visible="False">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbtnActiveIngredientErrors" Visible="False" Text="Errors" CommandName="Errors" CommandArgument='<%# Eval("ppsubstance_FK") %>' CssClass="gvLinkTextBold" OnClick="lbtnActiveIngredientErrors_OnClick" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="2%" Caption="" FieldName="Delete" FilterType="None">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="lbtnDeleteActiveIngredient" Text="" ImageUrl="~/Images/delete.gif" CommandName="DeleteActiveIngredient" CommandArgument='<%# Eval("ppsubstance_PK") %>' CssClass="gvLinkTextBold" OnClick="btnDeleteSubstance_OnClick" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />
        </possGrid:PossGrid>
        <uc:LabelError ID="lblErrActiveIngredient" runat="server"/>
    </div>
    <br/> <br/>
    <div id="divExcipients" class="clear form-grid-border-bottom pp-grids-margins">
        <uc:TextBox ID="txtExcipientsHidden" runat="server" Visible="False" />
        <div class="form-grid-header">
            <h2 class="bold-text-important">Excipients</h2> 
            <asp:LinkButton ID="btnAddExcipient" runat="server" Text="New" CssClass="button New ppLeaveThisPage"></asp:LinkButton>
        </div>
        <possGrid:PossGrid ID="ExcipientsGrid" GridId="ExcipientsGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="ppsubstance_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="SubstanceName" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="10000" Position="None" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="ppsubstance_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="98%" Caption="Substance name" FieldName="SubstanceName" FilterType="Text">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbtnEditExcipient" Text='<%# HandleMissing(Eval("SubstanceName")) %>' CommandName="EditExcipient" CommandArgument='<%# Eval("ppsubstance_PK") %>' CssClass="gvLinkTextBold" OnClick="btnEditSubstance_OnClick"></asp:LinkButton>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="2%" Caption="" FieldName="Errors" FilterType="None" Visible="False">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbtnExcipientErrors" Visible="False" Text="Errors" CommandName="Errors" CommandArgument='<%# Eval("ppsubstance_FK") %>' CssClass="gvLinkTextBold" OnClick="lbtnExcipientErrors_OnClick" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
               <possGrid:PossTemplateField Width="2%" Caption="" FieldName="Delete" FilterType="None">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="lbtnDeleteExcipient" Text="" ImageUrl="~/Images/delete.gif" CommandName="DeleteExcipient" CommandArgument='<%# Eval("ppsubstance_PK") %>' CssClass="gvLinkTextBold" OnClick="btnDeleteSubstance_OnClick" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />
        </possGrid:PossGrid>
        <uc:LabelError ID="lblErrExcipient" runat="server"/>
    </div>
    <br/> <br/>
    <div id="divAdjuvants" class="clear form-grid-border-bottom pp-grids-margins">
        <uc:TextBox ID="txtAdjuvantsHidden" runat="server" Visible="False" />
        <div class="form-grid-header">
            <h2 class="bold-text-important">Adjuvants</h2>           
            <asp:LinkButton ID="btnAddAdjuvant" runat="server" Text="New" CssClass="button New ppLeaveThisPage"></asp:LinkButton>
        </div>
        <possGrid:PossGrid ID="AdjuvantsGrid" GridId="AdjuvantsGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="ppsubstance_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="SubstanceName" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="10000" Position="None" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="ppsubstance_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="36%" Caption="Substance name" FieldName="SubstanceName" FilterType="Text">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbtnEditAdjuvant" Text='<%# HandleMissing(Eval("SubstanceName")) %>' CommandName="EditAdjuvant" CommandArgument='<%# Eval("ppsubstance_PK") %>' CssClass="gvLinkTextBold" OnClick="btnEditSubstance_OnClick"></asp:LinkButton>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="5%" Caption="LNum value" FieldName="LowNumValue" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="5%" Caption="LNum prefix" FieldName="LowNumPrefix" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="LNum unit" FieldName="LowNumUnit" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="LDen value" FieldName="LowDenValue" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="5%" Caption="LDen prefix" FieldName="LowDenPrefix" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="LDen unit" FieldName="LowDenUnit" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="HNum value" FieldName="HighNumValue" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="5%" Caption="HNum prefix" FieldName="HighNumPrefix" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="HNum unit" FieldName="HighNumUnit" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="HDen value" FieldName="HighDenValue" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="5%" Caption="HDen prefix" FieldName="HighDenPrefix" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="HDen unit" FieldName="HighDenUnit" FilterType="Text" />
                <possGrid:PossTemplateField Width="2%" Caption="" FieldName="Errors" FilterType="None" Visible="False">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lbtnAdjuvantErrors" Visible="False" Text="Errors" CommandName="Errors" CommandArgument='<%# Eval("ppsubstance_FK") %>' CssClass="gvLinkTextBold" OnClick="lbtnAdjuvantErrors_OnClick" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="2%" Caption="" FieldName="Delete" FilterType="None">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="lbtnDeleteAdjuvant" Text="" CommandName="DeleteAdjuvant" ImageUrl="~/Images/delete.gif" CommandArgument='<%# Eval("ppsubstance_PK") %>' CssClass="gvLinkTextBold" OnClick="btnDeleteSubstance_OnClick" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />
        </possGrid:PossGrid>
        <uc:LabelError ID="lblErrAdjuvant" runat="server"/>
        <br />
    </div>
</asp:Panel>
