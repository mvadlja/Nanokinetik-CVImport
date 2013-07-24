<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterMain.Master" CodeBehind="StatisticsChartsView.aspx.cs" Inherits="AspNetUI.Views.Business.StatisticsChartsView" %>
<%@ Register Src="~/Views/PartialShared/Operational/TabMenu.ascx" TagName="TabMenu" TagPrefix="operational" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>


<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server" >
    
        <!-- View's forms as partial views -->
     <table cellpadding="0" cellspacing="0" width="100%" runat="server" id="tabMenuContainer">
        <tr>
            <%--<td style="border-bottom: 1px solid #AAAAAA;">--%>
            <td>
                <!-- Navigation for level2 forms -->
                <operational:TabMenu ID="tabMenu" runat="server" />
            </td>
        </tr>
    </table>

    <div id="centering" style="width:850px; margin:0px auto;">
        <asp:Panel runat="server" ID="pnlTimeFilter" Visible="false" style="width:200px; margin:0px auto;">
            <table>
                <tr>
                    <td>
                        <customControls:DropDownList_CT  runat="server" AutoPostback="true" ID="ctlTimePeriod" ControlLabel="Period: " ControlInputWidth="140px"  />
                    </td>
                    <td>
                        <table runat="server" ID="tblTimePeriod" visible="false" >
                            <tr>
                                  <td>
                                     <customControls:DateBox_CT  runat="server"  ID="ctlTimePeriodStart" ControlLabel="Start date" LabelColumnWidth="120px" ControlInputWidth="90px"  Visible="true" style="margin-left:-60px;" />
                                    </td>
                                    <td>
                                        <customControls:DateBox_CT  runat="server"  ID="ctlTimePeriodEnd" ControlLabel="End date"  LabelColumnWidth="120px"  ControlInputWidth="90px" Visible="true" />
                                    </td>
                                     <td>
                                        <asp:Button runat="server" ID="btnShow" Text="Show" Visible="true" />
                                    </td>
                                    <td>
                                        <customControls:DropDownList_CT  runat="server" AutoPostback="true" ID="ctlGroupBy" ControlLabel="Group by: " LabelColumnWidth="110px"  ControlInputWidth="100px"  />
                                    </td>
                            </tr>
                        </table>
                    </td>
                   
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlTimeStats" style="margin:0px auto; width:200px;">
             <customControls:DropDownList_CT  runat="server" AutoPostback="true" ID="ctlTimeChartType" ControlLabel="Show" LabelColumnWidth="110px"  ControlInputWidth="100px"  />
        </asp:Panel>
       <asp:Chart ID="ctlChart" runat="server" Width="850px" Height="500px" BackColor="#EDEDED" AntiAliasing="All" >
            <Series>
                <asp:Series Name="mainSeries" />
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="mainChartArea"   >
                </asp:ChartArea>

            </ChartAreas>
        </asp:Chart>

    </div>
      <table>
          <tr>
          <td>
            <asp:HyperLink ID="LinkButton1" NavigateUrl="~/Views/Operational/StatisticsChartsView.aspx?chart=successStatus"  runat="server"  CausesValidation="false" Width="120px" style="text-align:center;margin-left:15px; border:none;;"  >
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/distr-icon.png" />
                <asp:Label ID="Label1" runat="server" >Sucess status</asp:Label>
             </asp:HyperLink>

               <asp:HyperLink ID="lbYear" runat="server"  NavigateUrl="~/Views/Operational/StatisticsChartsView.aspx?chart=sentMessages" CausesValidation="false" Width="120px" style="text-align:center; margin-left:15px; border:none;"  >
                <asp:Image ID="imgFolder" runat="server" ImageUrl="~/Images/sent-icon.png" />
                <asp:Label ID="Label2" runat="server" >Sent Messages</asp:Label>      
             </asp:HyperLink>


             <asp:HyperLink ID="LinkButton2" runat="server"  NavigateUrl="~/Views/Operational/StatisticsChartsView.aspx?chart=messageTimeStatus" CausesValidation="false" Width="120px" style="text-align:center;margin-left:15px; border:none;;"  >
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/ack-time-icon.png" />
                <asp:Label ID="Label3" runat="server" >Message Time stats</asp:Label>
             </asp:HyperLink>
             </td>
           </td>
           <tr>
           <td>
             

             <asp:HyperLink ID="LinkButton5" runat="server" NavigateUrl="~/Views/Operational/StatisticsChartsView.aspx?chart=receivedMessages"  CausesValidation="false" Width="120px" style="text-align:center; margin-left:15px; border:none;"  >
                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/sent-icon.png" />
                    <asp:Label ID="Label4" runat="server" >Received Messages</asp:Label>
             </asp:HyperLink>

             <asp:HyperLink ID="LinkButton3" runat="server" NavigateUrl="~/Views/Operational/StatisticsChartsView.aspx?chart=ackByType"  CausesValidation="false" Width="120px" style="text-align:center;margin-left:15px; border:none;;"  >
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/distr-icon.png" />
                <asp:Label ID="Label5" runat="server">Ack by type</asp:Label>
             </asp:HyperLink>

             <asp:HyperLink ID="LinkButton4" runat="server" NavigateUrl="~/Views/Operational/StatisticsChartsView.aspx?chart=ackByTypeTime"  CausesValidation="false" Width="120px" style="text-align:center;margin-left:5px; border:none;;"  >
                <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/ack-icon.png" />
                <asp:Label ID="Label6" runat="server" >Ack by type/time</asp:Label>
              </asp:HyperLink>
            </td>
            </tr>
            </table>
</asp:Content>
