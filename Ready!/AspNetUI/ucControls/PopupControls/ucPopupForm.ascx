<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupForm.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupForm" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container">
        <div class="modal_title_bar" style="text-align:center;" >
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>
      
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlAdjuvants" runat="server" Visible="false">

             
                               <table cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div id="positioner" style="margin-left: 45px;width: 580px;">
                                                <customControls:TextBox_CT ID="substanceSearch" runat="server"  AutoPostback="false" ControlLabel="Substance name:" IsMandatory="true" LabelColumnWidth="150px" ControlInputWidth="418px"  ClientIDMode="Static" ClientID_CT="substanceSearch" />
                                             </div>
                                        </td>
                                        <td>
                                             <div style="margin-left:-34px; width:100px">    
                                                <a class="boldText" href="javascript:startSubstanceSearch();">Select </a>  
                                                <a  class="boldText"  href="javascript:removeSelectedSubstance();"> | Remove</a> 
                                             </div> 
                                        </td>
                                    </tr>
                                </table>
                                <input type="hidden" id="ctlSubstance_PK" name="ctlSubstance_PK" value="" runat="server" clientidmode="Static" onchange="SubstanceChanged(this);"  />

                                <customControls:DropDownList_CT ID="ctlConTypeCode" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Concentration type:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Concentration type can't be empty."
                                    AutoPostback="false" ClientID_CT="ctlConTypeCode" ChangedJS="ConcentrationTypeChanged(this);"   OnInputValueChanged="ConcentrationType_Changed" />
                              
                              
                                
                                
                               <customControls:DropDownList_CT ID="ctlExpressedAs" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Expressed as:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage=""
                                    AutoPostback="true" OnInputValueChanged="ExpressedAs_Changed" ClientID_CT="ctlExpressedAs" />
                                <br />
                                <asp:Panel ID="pnlStrength" runat="server" GroupingText="Strength" Visible="false">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <customControls:TextBox_CT ID="ctlStrengthValue" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="Value: " IsMandatory="true" ControlErrorMessage="Strength value is not a valid number."
                                                    ControlEmptyErrorMessage="Strength value can't be empty." />
                                            </td>
                                            <td>
                                                <customControls:TextBox_CT ID="ctlStrengthUnit" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="Unit:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Strength unit can't be empty." />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlInfo" runat="server" GroupingText="Full description" ClientIDMode="Static" >
                                    <div id="info" runat="server" >
                                        <span id="infoSubName" runat="server" class="green" clientidmode="Static" ></span>
                                        <span id="infoSubNameText" runat="server" visible="false" clientidmode="Static">as </span>
                                        <span id="infoType" runat="server" class="blue" clientidmode="Static" ></span>
                                        <span id="infoConcTypeText1" runat="server" visible="false" class="blue" clientidmode="Static" >In a </span>
                                        <span id="infoConcTypeText2" runat="server" visible="false" class="blue" clientidmode="Static" >At </span>
                                        <span id="infoConcType" runat="server" class="blue" clientidmode="Static" ></span>
                                        <span id="infoRangeMinText" runat="server" visible="false" class="blue" clientidmode="Static" >of </span>
                                        <span id="infoRangeNMin" runat="server" class="green" clientidmode="Static"></span>
                                        <span id="infoNumMinPrefix" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoRangeNText" runat="server" clientidmode="Static">to </span>
                                        <span id="infoRangeNMax"  runat="server" class="green" clientidmode="Static"></span>
                                        <span id="infoNumMaxPrefix" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoUnit" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoUnitText" runat="server" visible="false" clientidmode="Static"> per </span>
                                        <span id="infoRangeDMin" runat="server" class="green" clientidmode="Static"></span>
                                        <span id="infoRangeDText" runat="server" clientidmode="Static" >to </span>
                                        <span id="infoRangeDMax" runat="server" class="green" clientidmode="Static"></span>
                                        <span id="infoMeasure" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoDenPrefix" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoDenExp" runat="server" class="blue" clientidmode="Static"></span>
                                        <br />
                                        <span id="infoSubNameText2" runat="server" visible="false" clientidmode="Static">Concise: </span>
                                        <span id="infoConSubName" runat="server" class="green margin_left" clientidmode="Static"></span>
                                        <span id="infoConRangeNMin" runat="server" class="green" clientidmode="Static" ></span>
                                        <span id="infoConNumMinPrefix" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoConRangeNMax" runat="server" class="green" clientidmode="Static"></span>
                                        <span id="infoConNumMaxPrefix" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoConNUnit" runat="server" class="blue" style="margin-left:-3px;" clientidmode="Static"></span>
                                        <span id="infoMeasure2" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoConRangeDMin" runat="server" class="green" style="margin-left:-3px;" clientidmode="Static"></span>
                                        <span id="infoConRangeDMax" runat="server" class="green" clientidmode="Static"></span>
                                        <span id="infoConDenPrefix" runat="server" class="blue" clientidmode="Static"></span>
                                        <span id="infoConDenExp" runat="server" class="blue" style="margin-left:-3px;" clientidmode="Static"></span>
                                    </div>
                                </asp:Panel>
                                <div id="panelGroup" style="display:none;">
                                <br />
                                <asp:Panel ID="pnlExactLimit" runat="server" GroupingText="Exact value" Visible="true" ClientIDMode="Static" >
                                    <input type="hidden" runat="server" value="" id="pnlExactLimitVisible" clientidmode="Static" />
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
                                                <customControls:TextBox_CT ID="ctlExactNumVal" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="Exact value numerator value is not a valid number."
                                                    ControlEmptyErrorMessage="Exact value numerator value can't be empty." AutoPostback="false" OnInputValueChanged="ExactNumVal_Changed" ChangedJS="ExactNumeratorValueChanged(this);" ClientID_CT="ctlExactNumVal" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlExactNumPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Exact value numerator prefix can't be empty."
                                                    AutoPostback="false" ChangedJS="ExactNumPrefixChanged(this);"   OnInputValueChanged="ExactNumPrefix_Changed"  ClientID_CT="ctlExactNumPrefix" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlExactNumUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Exact value numerator unit can't be empty."
                                                    AutoPostback="false" ChangedJS="ExactNumUnitChanged(this);" OnInputValueChanged="ExactNumUnit_Changed" ClientID_CT="ctlExactNumUnit" />
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
                                                <span style="padding-left: 15px" runat="server" id="ExactLimitExp" clientidmode="Static" >of Measure</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Denominator
                                            </td>
                                            <td>
                                                <customControls:TextBox_CT ID="ctlExactDenVal" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="Exact value denominator value is not a valid number."
                                                    ControlEmptyErrorMessage="Exact value denominator value can't be empty." AutoPostback="false" ChangedJS="ExactDenominatorValueChanged(this);" OnInputValueChanged="ExactDenVal_Changed" ClientID_CT="ctlExactDenVal"  />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlExactDenPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Exact value denominator prefix can't be empty."
                                                    AutoPostback="false" ChangedJS="ExactDenPrefixChanged(this);"  OnInputValueChanged="ExactDenPrefix_Changed" ClientID_CT="ctlExactDenPrefix"  />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlExactDenUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Exact value denominator unit can't be empty."
                                                     AutoPostback="false" ChangedJS="ExactDenUnitChanged(this);" OnInputValueChanged="ExactDenUnit_Changed" ClientID_CT="ctlExactDenUnit" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel> 
                                <br />
                                
                                <asp:Panel ID="pnlLowLimit"  runat="server" GroupingText="Low Limit" Visible="true" ClientIDMode="Static" >
                                    <input type="hidden" runat="server" value="" id="pnlLowLimitVisible" clientidmode="Static" />
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
                                                <customControls:TextBox_CT ID="ctlLowLimitNumVal" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="Low limit numerator value is not a valid number."
                                                    ControlEmptyErrorMessage="Low limit numerator value can't be empty." AutoPostback="false" ClientID_CT="ctlLowLimitNumVal" ChangedJS="LowLimitNumValueChanged(this);"  OnInputValueChanged="LowLimitNumVal_Changed" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlLowLimitNumPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Low limit numerator prefix can't be empty."
                                                    OnInputValueChanged="LowLimitNumPrefix_Changed" AutoPostback="false" ClientID_CT="ctlLowLimitNumPrefix" ChangedJS="LowLimitNumPrefixChanged(this);" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlLowLimitNumUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Low limit numerator unit can't be empty."
                                                    OnInputValueChanged="LowLimitNumUnit_Changed" AutoPostback="false" ClientID_CT="ctlLowLimitNumUnit" ChangedJS="LowLimitNumUnitChanged(this);" />
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
                                                <span style="padding-left: 15px" runat="server" id="LowLimitExp" clientidmode="Static" >of Measure</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Denominator
                                            </td>
                                            <td>
                                                <customControls:TextBox_CT ID="ctlLowLimitDenVal" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="Low limit denominator value is not a valid number."
                                                    ControlEmptyErrorMessage="Low limit denominator value can't be empty."  AutoPostback="false" ClientID_CT="ctlLowLimitDenVal" ChangedJS="LowLimitDenValueChanged(this);" OnInputValueChanged="LowLimitDenVal_Changed" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlLowLimitDenPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Low limit denominator prefix can't be empty."
                                                    AutoPostback="false" ClientID_CT="ctlLowLimitDenPrefix" ChangedJS="LowLimitDenPrefixChanged(this);" OnInputValueChanged="LowLimitDenPrefix_Changed" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlLowLimitDenUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Low limit denominator unit can't be empty."
                                                    AutoPostback="false" ClientID_CT="ctlLowLimitDenUnit" ChangedJS="LowLimitDenUnitChanged(this);" OnInputValueChanged="LowLimitDenUnit_Changed" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="pnlHighLimit" runat="server" GroupingText="High Limit" Visible="true" ClientIDMode="Static">
                                    <input type="hidden" runat="server" value="" id="pnlHighLimitVisible" clientidmode="Static" />
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
                                                <customControls:TextBox_CT ID="ctlHighLimitNumVal" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="High limit numerator value is not a valid number."
                                                    ControlEmptyErrorMessage="High limit numerator value can't be empty." AutoPostback="false" ChangedJS="HighLimitNumValueChanged(this);" ClientID_CT="ctlHighLimitNumVal" OnInputValueChanged="HighLimitNumVal_Changed" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlHighLimitNumPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="High limit numerator prefix can't be empty."
                                                    AutoPostback="false" ChangedJS="HighLimitNumPrefixChanged(this);" ClientID_CT="ctlHighLimitNumPrefix" OnInputValueChanged="HighLimitNumPrefix_Changed" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlHighLimitNumUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="High limit numerator unit can't be empty."
                                                    AutoPostback="false"  ClientID_CT="ctlHighLimitNumUnit"  OnInputValueChanged="HighLimitNumUnit_Changed" />
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
                                                <span style="padding-left: 15px" runat="server" id="HighLimitExp" clientidmode="Static">of Measure</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Denominator
                                            </td>
                                            <td>
                                                <customControls:TextBox_CT ID="ctlHighLimitDenVal" runat="server" LabelColumnWidth="50px" MaxLength="250" ControlInputWidth="100px" ControlLabel="" IsMandatory="true" ControlErrorMessage="High limit denominator value is not a valid number."
                                                    ControlEmptyErrorMessage="High limit denominator value can't be empty." AutoPostback="false"  ChangedJS="HigHLimitDenValueChanged(this);" ClientID_CT="ctlHighLimitDenVal"  OnInputValueChanged="HighLimitDenVal_Changed" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlHighLimitDenPrefix" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="High limit denominator prefix can't be empty."
                                                    AutoPostback="false"  ClientID_CT="ctlHighLimitDenPrefix"  OnInputValueChanged="HighLimitDenPrefix_Changed" />
                                            </td>
                                            <td>
                                                <customControls:DropDownList_CT ID="ctlHighLimitDenUnit" runat="server" LabelColumnWidth="50px" ControlInputWidth="200px" ControlLabel="" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="High limit denominator unit can't be empty."
                                                    AutoPostback="false" ClientID_CT="ctlHighLimitDenUnit" OnInputValueChanged="HighLimitDenUnit_Changed" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>
        <input type="hidden" value="" runat="server" id="ctlFormType" clientidmode="Static" />
        <input type="hidden" value="" runat="server" id="ctlUcPopupVisible" clientidmode="Static" />
        <div class="modal" id="substanceSelectorModal" >
        <div id="substanceSelector" class="modal_container">
     
            <div id="substanceSearchBox" >
                <span>Name: </span><input type="text" value="" id="subSearchName"  title="Search by name"  onkeypress="preventFormSubmission(event);" />  <span style="margin-left:10px;">EVCODE: </span><input type="text" value="" onkeypress="preventFormSubmission(event);" id="subSearchEvCode" title="Search by EVCODE" />
                <div class="close" onclick="cancelSusbtanceSearch();" title="Close" ></div>
            </div>
            <select id="subSearchList" size="10" >
            </select>
            <div id="substanceSearchPager">
                <div class="prev" onclick="GetSubstancePage('-1');" title="Previous page"></div>
                <div class="center">
                    <div class="text" style="margin-right:5px;">Page: </div> <input type="text" value="0" id="substanceSearchCurrentPage" onchange="GetSubstancePage('exact');"  onkeypress="preventFormSubmission(event);" /> <div class="text" style="margin-left:5px;" id="substanceSearchTotalPages"> / 0</div>
                </div>
                <div class="next" onclick="GetSubstancePage('+1');" title="Next page"></div>
            </div>
             <input id="subListName" type="hidden" value="" />
             <input id="subListEvcode" type="hidden" value="" />
            <input id="subListPage" type="hidden" value="0" />
            <input id="subListPageTotalPages" type="hidden" value="0" />
        </div>
        </div>
    </div>
</div>



