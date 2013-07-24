<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SSIFForm_details.ascx.cs" Inherits="AspNetUI.Views.SSIFForm_details" %>

<asp:Panel ID="pnlDataDetails" runat="server" CssClass="preventableClose" >
     <%--<div id="info" runat="server" style="margin-left: 30px;">
     
     </div>--%>


    <div id="blank1" runat="server" style="height: 18px;"></div>

    <table id="tblcommon" runat="server" style="margin-left: 30px;">
        <tr style="height:10px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>SUB30779</td>
        </tr>
        <tr style="height:19px;">
            <td>Single</td>
        </tr>
    </table>

    <table id="tblsubstancename1" runat="server" style="margin-left: 30px;">
        <tr style="height:10px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Clopidogrel hydrochloride</td>
        </tr>
        <tr style="height:19px;">
            <td>Official Name</td>
        </tr>
        <tr style="height:19px;">
            <td>Eng</td>
        </tr>
    </table>
    <div id="substancename1BLANK" runat="server" style="height: 19px;"></div>

    <table id="tblofficialname" runat="server" style="margin-left: 30px;">
        <tr style="height:40px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>INN</td>
        </tr>
        <tr style="height:19px;">
            <td>Current</td>
        </tr> 
    </table>


    <table id="tblofficialnamedomaincv" runat="server" style="margin-left: 30px;">
        <tr style="height:52px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>HumanPharmaceuticals (EU)</td>
        </tr>
    </table>
    <%--<div id="officialnamedomainsBLANK" runat="server" style="height: 19px;"></div>
--%>

    <table id="tblofficialnamejurisdictioncv" runat="server" style="margin-left: 30px;">
        <tr style="height:35px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>EU</td>
        </tr>
    </table>
    <%--<div id="officialnamejurisdictionsBLANK" runat="server" style="height: 19px;"></div>
--%>

    <table id="tblreferencesource1" runat="server" style="margin-left: 30px;">
        <tr style="height:34px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Yes</td>
        </tr>
        <tr style="height:19px;">
            <td>Substance registration System</td>
        </tr>
        <tr style="height:19px;">
            <td>Literature</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Substance registration System, Clopidogrel Hydrochloride Monograph</td>
        </tr>
    </table>
    <div id="referencesource1BLANK" runat="server" style="height: 20px;"></div>


    <table id="tblsubstancename2" runat="server" style="margin-left: 30px;">
        <tr style="height:14px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Thieno[3,2-c]pyridine-5(4H)-acetid acid, a-(2-chlorophenyl) -6, 7-dihydro-,met</td>
        </tr>
        <tr style="height:19px;">
            <td>Other Name</td>
        </tr>
        <tr style="height:19px;">
            <td>Eng</td>
        </tr>
    </table>
    <div id="substancename2BLANK" runat="server" style="height: 19px;"></div>


    <table id="tblreferencesource2" runat="server" style="margin-left: 30px;">
        <tr style="height:34px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Yes</td>
        </tr>
        <tr style="height:19px;">
            <td>Marketing authorisation application</td>
        </tr>
        <tr style="height:19px;">
            <td>Regulatory Submission</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>eCTD Marketing application "XXX" section 2-3-S</td>
        </tr>
    </table>


    <table id="tblsubstancecode" runat="server" style="margin-left: 30px;">
        <tr style="height:10px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>120202-65-5</td>
        </tr>
        <tr style="height:19px;">
            <td>CAS</td>
        </tr>
        <tr style="height:19px;">
            <td>100000123553</td>
        </tr>
        <tr style="height:19px;">
            <td>Current</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
    </table>
    <div id="substancecodeBLANK" runat="server" style="height: 19px;"></div>


    <table id="tblreferencesource3" runat="server" style="margin-left: 30px;">
        <tr style="height:25px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Yes</td>
        </tr>
        <tr style="height:19px;">
            <td>Substance registration System</td>
        </tr>
        <tr style="height:19px;">
            <td>Literature</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Substance registration System, Clopidogrel Hydrochloride Monograph</td>
        </tr>
    </table>


     <table id="tblversion" runat="server" style="margin-left: 30px;">
        <tr style="height:38px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>1</td>
        </tr>
        <tr style="height:19px;">
            <td>2011-12-10</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
    </table>
    <div id="versionBLANK" runat="server" style="height: 19px;"></div>
   <%-- <div id="version1BLANK" runat="server" style="height: 19px;"></div>--%>

    <table id="tblsubstanceclassification" runat="server" style="margin-left: 30px;">
        <tr style="height:75px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Human Pharmaceuticals</td>
        </tr>
        <tr style="height:19px;">
            <td>Single</td>
        </tr>
        <tr style="height:19px;">
            <td>Chemical</td>
        </tr>
    </table>

    <table id="tblsubstanceclassificationsubtype" runat="server" style="margin-left: 30px;">
        <tr style="height:49px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Not applicable</td>
        </tr>
    </table>


    <table id="tblreferencesource4" runat="server" style="margin-left: 30px;">
        <tr style="height:35px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Yes</td>
        </tr>
        <tr style="height:19px;">
            <td>ISO/FDIS</td>
        </tr>
        <tr style="height:19px;">
            <td>Public NOS</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>ISO/FDIS 11238</td>
        </tr>
    </table>
    <div id="referenceinformationBLANK" runat="server" style="height: 20px;"></div>

    <table id="tbltarget" runat="server" style="margin-left: 30px;">
        <tr style="height:27px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>CHEMBL2001</td>
        </tr>
        <tr style="height:19px;">
            <td>Purinergic receptor P2Y12</td>
        </tr>
        <tr style="height:19px;">
            <td>irreversible inhibition</td>
        </tr>
        <tr style="height:19px;">
            <td>Human</td>
        </tr>
        <tr style="height:19px;">
            <td>Protein</td>
        </tr>
    </table>
    <div id="targetsBLANK" runat="server" style="height: 18px;"></div>
    <%--<div id="targetBLANK" runat="server" style="height: 18px;"></div>--%>


     <table id="tblreferencesource5" runat="server" style="margin-left: 30px;">
        <tr style="height:34px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Yes</td>
        </tr>
        <tr style="height:19px;">
            <td>European Bioinformatic Institute</td>
        </tr>
        <tr style="height:19px;">
            <td>Literature</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>European Bioinformatic Institute, Purinergic receptor P2Y12 Monograph</td>
        </tr>
    </table>
    
    <div id="extensionBLANK" runat="server" style="height: 18px;"></div>
    <div id="singleBLANK" runat="server" style="height: 18px;"></div>
    <div id="structuresBLANK" runat="server" style="height: 18px;"></div>
    
    <table id="tblstructure1" runat="server" style="margin-left: 30px;">
        <tr style="height:70px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>InChI</td>
        </tr>
        <tr style="height:19px;">
            <td>nChI-1/C16H16C1N02S.C1H/c1-20-16(19)15(12-4-2-3-5-13(12)17)|18-8-6-14-11(10-18)7-9-21-14</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Chiral</td>
        </tr>
        <tr style="height:19px;">
            <td>(+)</td>
        </tr>
        <tr style="height:19px;">
            <td>C16H16C1N02S.HCl</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
    </table>
    <div id="structure1BLANK" runat="server" style="height: 18px;"></div>
      

     <table id="tblstructure2" runat="server" style="margin-left: 30px;">
        <tr style="height:8px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>SMILE</td>
        </tr>
        <tr style="height:19px;">
            <td>N1([C@@H]c2c(cccc2)C1)C(-O)OC)Cc2c(CC1)scc2.C1</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Chiral</td>
        </tr>
        <tr style="height:19px;">
            <td>(+)</td>
        </tr>
        <tr style="height:19px;">
            <td>C16H16C1N02S.HCl</td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
    </table>
    <div id="structure2BLANK" runat="server" style="height: 18px;"></div>

    <div id="chemicalBLANK" runat="server" style="height: 18px;"></div>

    <table id="tblstoichiometric" runat="server" style="margin-left: 30px;">
        <tr style="height:10px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td></td>
        </tr>
    </table>
    <%-- <div id="stoichiometricBLANK" runat="server" style="height: 18px;"></div>
--%>
    <table id="tblreferencesource6" runat="server" style="margin-left: 30px;">
        <tr style="height:62px;">
            <td></td>
        </tr>
        <tr style="height:19px;">
            <td>Yes</td>
        </tr>
        <tr style="height:19px;">
            <td>Marketing authorisation application</td>
        </tr>
        <tr style="height:19px;">
            <td>Regulatory Submission</td>
        </tr>
        <tr style="height:19px;">
            <td>aaaaaaaaaaa</td>
        </tr>
        <tr style="height:19px;">
            <td>eCTD Marketing application "XXX" section 2.3.S</td>
        </tr>
    </table>

</asp:Panel>
