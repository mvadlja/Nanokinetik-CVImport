<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="TimeUnitForm_list.ascx.cs" Inherits="AspNetUI.Views.TimeUnitForm_list" %>

<%@ Register Src="~/ucControls/PopupControls/ucExporter.ascx" TagName="Exporter" TagPrefix="operational" %>

<span id="rightCount" runat="server" style="display:none"></span>
<!-- Optional searcher goes here -->
<asp:Panel ID="pnlDataSearcher" runat="server" GroupingText="Searcher" Visible="False">
    <br />
</asp:Panel>
<!-- Result list goes here -->
<asp:Panel ID="pnlDataList" runat="server">
    <operational:Exporter ID="ucExporter" runat="server"></operational:Exporter>
    <div class="controlsHolder">
        <input type="button" ID="btnExport" runat="server" value="Export" CssClass="min_margin_top_button8 Export" onclick="ExportToExcel();" />
    </div>
   <div id="gridHolder" runat="server" clientidmode="Static" >
   </div>
   
</asp:Panel>
<script type="text/javascript" language="javascript">

    function refreshCount(newValue) {
        $(".dxpSummary").each(function () {
            var originalText = $(this).text();
            originalText = originalText.replace(/Page ([0-9]+) of ([0-9]+) \(([0-9]+) items\)/g, "Page $1 of $2 (" + newValue + " items)");;
            $(this).text(originalText);
        });
    }

    $(function () {
        $('form input:text:enabled:first').focus();
    });

    $(function () {
        respositionContextMenu();
    });
    function pageLoad() {
        respositionContextMenu();
    }

    (function ($) {
        $.fn.outerHTML = function () {
            return $(this).clone().wrap('<div></div>').parent().html();
        }
    })(jQuery);
    $(document).ready(function () {

        LoadGrid();

    });
    var filterTimeout = null;
    var focusedItem = null;
    var pagerClickDisabled = false;
    function InitializeGrid() {

        var nr = 0;
        $(".grid").find("tr").first().children().each(
			function (index, obj) {
			    if (!($(obj).attr("id")) || ($(obj).attr("id")) == "") {
			        ($(obj).attr("id", "cnt-" + nr));
			        nr++;
			    }
			}
		);
        initGrid($(".grid"));
        initGrouppingTable($(".grid"));
        $(".grid").addClass("draggable");
        if (dragtable != null) {
            dragtable.init($(".grid")[0]);
        }
        $(".grid input").blur(function (e) {



            FilterGrid(false);
        });
        $(".grid input").focus(function (e) {

            focusedItem = e.target.id;

        });

        $(".grid input").keydown(function (event) {

            if (event.keyCode == 13) {
                FilterGrid(true);
                event.preventDefault ? event.preventDefault() : event.returnValue = false;
            }
        });

        if (focusedItem != null) {
            $("#" + focusedItem).focus();
            $("#" + focusedItem).val($("#" + focusedItem).val());
            focusedItem = null;

        }
        pagerClickDisabled = false;
    }

    function LoadGrid() {
        var url = window.location.pathname;
        var pageName = url.substring(url.lastIndexOf('/') + 1);
        var idSearch = getParameterByName("idSearch");
        var idAct = getParameterByName("idAct");
        var entityContext = getParameterByName("EntityContext");

        jQuery.get(
                timUnitInvokeCallback + "?EntityContext=" + entityContext + "&pageName=" + pageName + "&loadState=true" + (idAct != "" ? ("&idAct=" + idAct) : "") + (idSearch != "" ? ("&idSearch=" + idSearch) : ""),
                function (response) { $("#gridHolder").html(response); InitializeGrid(); }
             );
    }

    function extractPageNum(url) {
        var pagePart = url.substring(url.indexOf("page=") + 5);
        if (pagePart.indexOf("&") > -1) pagePart = pagePart.substring(0, pagePart.indexOf("&"));
        return pagePart * 1;
    }

    var cachedData = new Array();

    var inProgressData = new Array();

    function prefetchPages(getUrl) {

        var pageNum = extractPageNum(getUrl);
        var prevUrl = getUrl.replace("page=" + pageNum, "page=" + (pageNum - 1));
        var nextUrl = getUrl.replace("page=" + pageNum, "page=" + (pageNum + 1));

        if (cachedData[nextUrl] == null) {
            inProgressData[nextUrl] = "inProgress";
            jQuery.get(
                    nextUrl + "&cache=true",

                    function (response) {
                        cachedData[nextUrl] = response;
                        if (inProgressData[nextUrl] == "refreshNow") {
                            $("#gridHolder").html(response); InitializeGrid();
                            prefetchPages(nextUrl);
                        }
                        inProgressData[nextUrl] = null;

                    }
                 );
        }

        if (cachedData[prevUrl] == null) {
            inProgressData[prevUrl] = "inProgress";
            jQuery.get(
            prevUrl + "&cache=true",

            function (response) {
                cachedData[prevUrl] = response;
                if (inProgressData[prevUrl] == "refreshNow") {
                    $("#gridHolder").html(response); InitializeGrid();
                    prefetchPages(prevUrl);
                }
                inProgressData[prevUrl] = null;

            }
            );
        }
    }

    function RefreshGrid(getData) {
        if (pagerClickDisabled) return;
        pagerClickDisabled = true;
        var getUrl = timUnitInvokeCallback + "?" + getData;
        var url = window.location.pathname;
        var pageName = url.substring(url.lastIndexOf('/') + 1);
        if (getUrl.indexOf("pageName") < 0) getUrl += ("&pageName=" + pageName);
        var idSearch = getParameterByName("idSearch");
        if (getUrl.indexOf("idSearch") < 0) getUrl = getUrl + (idSearch != "" ? ("&idSearch=" + idSearch) : "");
        var entityContext = getParameterByName("EntityContext");
        if (getUrl.indexOf("EntityContext") < 0) getUrl = getUrl + (entityContext != "" ? ("&EntityContext=" + entityContext) : "");

        if (cachedData[getUrl] != null) {

            $("#gridHolder").html(cachedData[getUrl]);
            InitializeGrid();
            prefetchPages(getUrl);
        }
        else if (inProgressData[getUrl] != "inProgress") {

            jQuery.get(
                getUrl,

                function (response) {
                    cachedData[getUrl] = response;
                    $("#gridHolder").html(response);
                    InitializeGrid();
                    prefetchPages(getUrl);
                }
             );
        } else {
            inProgressData[getUrl] = "refreshNow";
        }

        //notify server about last url
        jQuery.get(
                getUrl + "&saveLastUrl=true",

                function (response) { }
                );

    }

    function FilterGrid(restoreFocus) {
        var getUrl = timUnitInvokeCallback;
        var filterData = "";
        var url = window.location.pathname;
        var pageName = url.substring(url.lastIndexOf('/') + 1);
        if (restoreFocus) focusedItem = $(document.activeElement).attr("id");
        $(".grid input").each(function (index, value) {

            if ($(value).attr("name") != "") {
                filterData += ("&" + $(value).attr("name") + "=" + $(value).val());
            }
        });

        if (getUrl.indexOf("ashx?") < 0) {
            filterData = filterData.substr(1);
            getUrl = getUrl + "?" + filterData;
        }
        else {
            getUrl = getUrl + "&" + filterData;
        }

        if (filterData.length > 0) {
            filterData = filterData.substring(1);
        }
        getUrl = getUrl + "&applyFilters=true";
        if (getUrl.indexOf("pageName") < 0) getUrl += ("&pageName=" + pageName);
        var idSearch = getParameterByName("idSearch");
        if (getUrl.indexOf("idSearch") < 0) getUrl += (idSearch != "" ? ("&idSearch=" + idSearch) : "");
        var entityContext = getParameterByName("EntityContext");
        if (getUrl.indexOf("EntityContext") < 0) getUrl = getUrl + (entityContext != "" ? ("&EntityContext=" + entityContext) : "");
        jQuery.get(
                getUrl,

                function (response) { $("#gridHolder").html(response); InitializeGrid(); }
             );
    }

    function ExportToExcel() {
        ////
        var getUrl = timUnitInvokeCallback;
        var filterData = "";
        var url = window.location.pathname;
        var pageName = url.substring(url.lastIndexOf('/') + 1);
        $(".grid input").each(function (index, value) {

            if ($(value).attr("name") != "") {
                filterData += ("&" + $(value).attr("name") + "=" + $(value).val());
            }
        });

        if (getUrl.indexOf("ashx?") < 0) {
            filterData = filterData.substr(1);
            getUrl = getUrl + "?" + filterData;
        }
        else {
            getUrl = getUrl + "&" + filterData;
        }

        var entityContext = getParameterByName("EntityContext");
        if (getUrl.indexOf("EntityContext") < 0) getUrl = getUrl + (entityContext != "" ? ("&EntityContext=" + entityContext) : "");

        if (filterData.length > 0) {
            filterData = filterData.substring(1);
        }
        getUrl = getUrl + "&applyFilters=true";
        if (getUrl.indexOf("pageName") < 0) getUrl += ("&pageName=" + pageName);
        var idSearch = getParameterByName("idSearch");
        if (getUrl.indexOf("idSearch") < 0) getUrl += (idSearch != "" ? ("&idSearch=" + idSearch) : "");
        getUrl = getUrl + "&exportExcel=true";

        window.location.href = getUrl;

    }

    function getParameterByName(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.search);
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    }

</script> 
