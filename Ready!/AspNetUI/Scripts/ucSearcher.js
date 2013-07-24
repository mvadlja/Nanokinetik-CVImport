//var ucSearchCurrentForm  = null;
//var searchType = null;
//var selectedColor = "#FFFF00";
//var plainColor = "#ebebeb";
//var plainColorAlternate = "#ffffff";
//var hoverColor = "#ffffdd";
//var tableHeaderBg = "#CDDEF0";
//function ucSearcherInitalize(sender, args) {


//    var ctl = $("#ctlUcSearcherVisible[value='true']").first();
//    ucSearchCurrentForm  = ctl.closest(".modal_container");

//    if (ucSearchCurrentForm  == null || ucSearchCurrentForm  == undefined) {
//        return;
//    }
//    if (ucSearchCurrentForm.find("#ctlSearcherType").val() != "async") return;

//    var searchableControls = jQuery.parseJSON(ucSearchCurrentForm.find("#ctlSearchableControls").val());
//    for (i = 0; i < searchableControls.controls.length; i++) {
//        ucSearchCurrentForm.find("#" + searchableControls.controls[i].clientID).change(function () { ucSearcherSearch(null,'true'); });    
//    }


//    ucSearchCurrentForm.find("#btnSearch").attr("onclick", "ucSearcherSearch(null,'true');");

//    searchType = ucSearchCurrentForm.find("#ctlSearchType").val();

//    setTimeout("ucSearcherSearch();",5);
//};

//function ucSearcherSearch(page, reload) {

//    if (reload != null) ucSearchCurrentForm.find("#ctlSelectedItems").val("");

//    var sortedByColumn = ucSearchCurrentForm .find("#ctlSortedByColumn").first().val();
//    var sortOrder = ucSearchCurrentForm .find("#ctlSortOrder").val();
//    var newPage;
//    var curentPage = parseInt(ucSearchCurrentForm.find("#ctlCurrentPage").val());
//    var totalPages = parseInt(ucSearchCurrentForm.find("#ctlTotalPages").val());
//    if (page == null) {
//        newPage = curentPage;
//    } else {

//        if (page.indexOf('+') > -1 || page.indexOf('-') > -1) {
//            if (page == '-1' && curentPage > 1) newPage = curentPage - 1;
//            if (page == '+1' && curentPage < totalPages) newPage =curentPage+1;
//        } else {
//            newPage = parseInt(page);
//        }
//    }
//    if (newPage < 1) newPage = 1;
//    if (newPage > totalPages) pagenewPage = totalPages;


//    var context = "ucSearcherSearch";
//    var message = "action=search;currentPage=" + newPage
//        + ";searchType=" + searchType + ";";

//    var searchableControls = jQuery.parseJSON(ucSearchCurrentForm.find("#ctlSearchableControls").val());
//    for (i = 0; i < searchableControls.controls.length; i++) {
//        message += (searchableControls.controls[i].columnName+"=" + ucSearchCurrentForm.find("#"+searchableControls.controls[i].clientID).val() + ";");
//    }

       
//    message += "sid=" + Math.random() + ";";
//    message += "sortedByColumn=" + sortedByColumn + ";sortOrder=" + sortOrder + ";"

//    eval(ucSearcherCallbackInovcation);
//}

//function ucSearcherSearchResponse(responseText) {

//    var response = jQuery.parseJSON(responseText);

//    if (response == null) return;
//    if (response.action == null || response.action == "none") return;

//    if (response.action=="update"){
//        var table = ucSearchCurrentForm .find("#tblData");
//        table.find("tr").each(
//                function (key, row) {
//                    $(row).remove();
//                }
//            );

//        var tableProperties = jQuery.parseJSON(ucSearchCurrentForm.find("#ctlTableProperties").val());
//        var selectedItems = ucSearchCurrentForm.find("#ctlSelectedItems").val();

//        var rowString = "<tr style=\"background-color:"+tableHeaderBg+";\">";
//        for (j = 0; j < tableProperties.colls.length; j++) {
//            rowString += ("<th id=\"collOrderBy" + tableProperties.colls[j].orderBy + "\" width=\"" + tableProperties.colls[j].width + "\">" + tableProperties.colls[j].name + "</th>");

//        }
//        table.append(rowString);
//        for (j = 0; j < tableProperties.colls.length; j++) {
//            if (tableProperties.colls[j].orderBy != "") {
//                var ordClause = tableProperties.colls[j].orderBy;
//                ucSearchCurrentForm.find("#" + "collOrderBy" + tableProperties.colls[j].orderBy).click(
//                    function () { toggleOrderBy(ordClause); }
//                );
//            }

//        }
     
//        for (i = 0; i < response.entities.length; i++) {
//            var color=i%2==0?plainColor:plainColorAlternate;
//            var selected = "notSelected";
//            if (selectedItems.indexOf("-"+response.entities[i].ID+"-")>-1) {
//                color=selectedColor;
//                selected="selected";
//            }
//            var rowString = "<tr entityId=\"" + response.entities[i].ID + "\" rowSelected=\"" + selected + "\" style=\"background-color:" + color + ";\" >";
//            for (j=0; j<tableProperties.colls.length; j++) {
//                rowString+=("<td>" + response.entities[i][tableProperties.colls[j].name] + "</td>");
//            }
//            table.append(rowString);
//        }

//        table.find("tr").click(function (event) {
//            toggleRowSelect($(event.target));
//        });
//        if (response.currentPage == "0") response.currentPage="1";
//        if (response.totalPages == "0") response.totalPages="1";
//        ucSearchCurrentForm.find("#ctlCurrentPage").val(response.currentPage);
//        ucSearchCurrentForm.find("#ctlTotalPages").val(response.totalPages);

//        ucSearchCurrentForm.find("#currentPageSpan").text(response.currentPage);
//        ucSearchCurrentForm.find("#totalPagesSpan").text(response.totalPages);
//        ucSearchCurrentForm.find("#itemCountSpan").text(response.totalCount);

//        var pager=ucSearchCurrentForm.find("#pagerNumbers");
//        pager.html(""); 

//        var lastPage=parseInt(ucSearchCurrentForm.find("#ctlTotalPages").val());
//        var currentPage=parseInt(ucSearchCurrentForm.find("#ctlCurrentPage").val());
//        var pagerPages=new Array();

//        for ( i=1; i<=3; i++) if (i<=lastPage && pagerPages.indexOf(i)<0) pagerPages[pagerPages.length]=i;
//        for (i=currentPage-1; i<=currentPage+1; i++) if (i>0 && i<=lastPage && pagerPages.indexOf(i)<0) pagerPages[pagerPages.length]=i;
//        for (i=lastPage-2; i<=lastPage; i++) if (i>0 && i<=lastPage && pagerPages.indexOf(i)<0) pagerPages[pagerPages.length]=i;
//        pagerPages.sort(function (a, b) { return a - b });
//        for (i = 0; i < pagerPages.length; i++) {
//            if (pagerPages[i] != currentPage) {
//                pager.append("<a href=\"javascript:ucSearcherSearch('" + pagerPages[i] + "')\"> " + pagerPages[i] + "</a>");
//            } else {
//                pager.append("<a style=\"font-weight:bold\" href=\"javascript:ucSearcherSearch('" + pagerPages[i] + "')\"> [" + pagerPages[i] + "]</a>");
//            }
//            if (i<pagerPages.length-1 && pagerPages[i+1]!=pagerPages[i]+1) pager.append(" <span>...</span>");
//        }
//    }
//}


//function toggleOrderBy(column) {
//    var oldColumn = ucSearchCurrentForm.find("#ctlSortedByColumn").val();
//    var oldOrder = ucSearchCurrentForm.find("#ctlSortOrder").val();
//    if (oldColumn == column) {
//        if (oldOrder == "asc") ucSearchCurrentForm.find("#ctlSortOrder").val("desc"); else ucSearchCurrentForm.find("#ctlSortOrder").val("asc");

//    } else {
//        ucSearchCurrentForm.find("#ctlSortedByColumn").val(column);
//        ucSearchCurrentForm.find("#ctlSortOrder").val("asc");
//    }

//    ucSearcherSearch();
//}

//function toggleRowSelect(row) {
//    row = row.closest("tr");
//    var rowId = row.attr("entityId");
//    if (rowId == null || rowId == undefined || rowId == "") return;
//    if (row.attr("rowSelected") == "notSelected") {
//        row.css("background-color", "#FFFF00");
//        row.attr("rowSelected", "selected");
    
//        var selectedItems = ucSearchCurrentForm.find("#ctlSelectedItems").val();
//        if (selectedItems.indexOf("-" + rowId + "-") < 0) selectedItems += ("-" + rowId + "-");
//        ucSearchCurrentForm.find("#ctlSelectedItems").val(selectedItems);
       

//    } else {
//        row.attr("rowSelected", "notSelected");
//        row.css("background-color", "#FFFFFF");
//        var selectedItems = ucSearchCurrentForm.find("#ctlSelectedItems").val();
//        if (selectedItems.indexOf("-" + rowId + "-") > -1) selectedItem=selectedItems.replace("-" + rowId + "-","");
//        ucSearchCurrentForm.find("#ctlSelectedItems").val(selectedItems);
//    }
//}
//function ucSearcherResult(returnmessage, context) {


//    if (context == "ucSearcherSearch") {
//        ucSearcherSearchResponse(returnmessage); return;
//    }
//}

//function ucSearcherError(returnmessage, context) {
//    alert("Callback Error: " + returnmessage + ", " + context);
//}

//String.prototype.trimStart = function (c) {
//    if (this.length == 0)
//        return this;
//    c = c ? c : ' ';
//    var i = 0;
//    var val = 0;
//    for (; this.charAt(i) == c && i < this.length; i++);
//    return this.substring(i);
//}

//if(!Array.prototype.indexOf) {
//    Array.prototype.indexOf = function(needle) {
//        for(var i = 0; i < this.length; i++) {
//            if(this[i] === needle) {
//                return i;
//            }
//        }
//        return -1;
//    };
//}
