<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormAMOUNT.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormAMOUNT" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server">
            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlAmount" runat="server" Visible="true">
                                <br />
                                <table cellpadding="0" width="100%">
                                    <tr>
                                        <td>
                                            <customControls:DropDownList_CT ID="ctlQuantityOper" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Quantity operator: " IsMandatory="true" ControlErrorMessage="" AutoPostback="true"
                                                ControlEmptyErrorMessage="Quantity operator can't be empty" OnInputValueChanged="ctlQuantityOnInputValueChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlExactLimit" runat="server" GroupingText="Exact value" Visible="false">
                                                <table cellpadding="2" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Value</span>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Prefix</span>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Unit</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Numerator
                                                        </td>
                                                        <td>
                                                            <customControls:TextBox_CT ID="ctlExactNumValue" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="Exact value numerator value is not a valid number."
                                                                ControlEmptyErrorMessage="Exact value numerator value can't be empty." AutoPostback="false" OnInputValueChanged="ExactNumVal_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlExactNumPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Exact value numerator prefix can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="ExactNumPrefix_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlExactNumUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Exact value numerator unit can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="ExactNumUnit_Changed" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                        </td>
                                                        <td style="text-align: left;">
                                                        </td>
                                                        <td style="text-align: left;">
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px" runat="server" id="ExactLimitExp">of Measure</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Denominator
                                                        </td>
                                                        <td>
                                                            <customControls:TextBox_CT ID="ctlExactDenomValue" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="Exact value denominator value is not a valid number."
                                                                ControlEmptyErrorMessage="Exact value denominator value can't be empty." AutoPostback="false" OnInputValueChanged="ExactDenVal_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlExactDenomPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Exact value denominator prefix can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="ExactDenPrefix_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlExactDenomUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Exact value denominator unit can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="ExactDenUnit_Changed" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <br />
                                            <asp:Panel ID="pnlLowLimit" runat="server" GroupingText="Low Limit" Visible="false">
                                                <table cellpadding="2" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Value</span>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Prefix</span>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Unit</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Numerator
                                                        </td>
                                                        <td>
                                                            <customControls:TextBox_CT ID="ctlLowLimitNumValue" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="Low limit numerator value is not a valid number."
                                                                ControlEmptyErrorMessage="Low limit numerator value can't be empty." AutoPostback="false" OnInputValueChanged="LowLimitNumVal_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlLowLimitNumPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Low limit numerator prefix can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="LowLimitNumPrefix_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlLowLimitNumUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Low limit numerator unit can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="LowLimitNumUnit_Changed" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                        </td>
                                                        <td style="text-align: left;">
                                                        </td>
                                                        <td style="text-align: left;">
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px" runat="server" id="LowLimitExp">of Measure</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Denominator
                                                        </td>
                                                        <td>
                                                            <customControls:TextBox_CT ID="ctlLowLimitDenomValue" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="Low limit denominator value is not a valid number."
                                                                ControlEmptyErrorMessage="Low limit denominator value can't be empty." AutoPostback="false" OnInputValueChanged="LowLimitDenVal_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlLowLimitDenomPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Low limit denominator prefix can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="LowLimitDenPrefix_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlLowLimitDenomUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Low limit denominator unit can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="LowLimitDenUnit_Changed" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <br />
                                            <asp:Panel ID="pnlHighLimit" runat="server" GroupingText="High Limit" Visible="false">
                                                <table cellpadding="2" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Value</span>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Prefix</span>
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px">Unit</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Numerator
                                                        </td>
                                                        <td>
                                                            <customControls:TextBox_CT ID="ctlHighLimitNumValue" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="High limit numerator value is not a valid number."
                                                                ControlEmptyErrorMessage="High limit numerator value can't be empty." AutoPostback="false" OnInputValueChanged="HighLimitNumVal_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlHighLimitNumPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="High limit numerator prefix can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="HighLimitNumPrefix_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlHighLimitNumUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="High limit numerator unit can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="HighLimitNumUnit_Changed" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                        </td>
                                                        <td style="text-align: left;">
                                                        </td>
                                                        <td style="text-align: left;">
                                                        </td>
                                                        <td>
                                                            <span style="padding-left: 15px" runat="server" id="HighLimitExp">of Measure</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            Denominator
                                                        </td>
                                                        <td>
                                                            <customControls:TextBox_CT ID="ctlHighLimitDenomValue" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="High limit denominator value is not a valid number."
                                                                ControlEmptyErrorMessage="High limit denominator value can't be empty." AutoPostback="false" OnInputValueChanged="HighLimitDenVal_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlHighLimitDenomPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="High limit denominator prefix can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="HighLimitDenPrefix_Changed" />
                                                        </td>
                                                        <td>
                                                            <customControls:DropDownList_CT ID="ctlHighLimitDenomUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="High limit denominator unit can't be empty."
                                                                AutoPostback="false" OnInputValueChanged="HighLimitDenUnit_Changed" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td>
                                            <customControls:TextBox_CT ID="ctlnonNumValue" runat="server" LabelColumnWidth="150px" ControlInputWidth="300px" ControlLabel="Non numeric value: " IsMandatory="false" ControlEmptyErrorMessage="" ControlErrorMessage="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="center">
                                                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
</div>
