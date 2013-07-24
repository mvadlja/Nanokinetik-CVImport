$().ready(function () {
    window["UrlLocation"] = window.location.protocol + "//" + window.location.host + window.AppVirtualPath + "/";
});

var treeInitialized = false;

function pageLoad() {

    var popupLoaded = $(".EDMS-modal-popup").css("display");
    if (popupLoaded == "block" || popupLoaded == "inline") {
        initLeftPaneTree();
        treeInitialized = true;
    } else {
        treeInitialized = false;
    }

    $("#EDMS-left-pane").resizable({
        handles: "e",
        minWidth: 200,
        maxWidth: 800,
    });

    var leftPaneWidth = 250;
    var rightPane = $("#EDMS-right-pane");
    $("#EDMS-left-pane").on("resize", function (event, ui) {
        leftPaneWidth = $("#EDMS-left-pane").width();

        rightPane.width(990 - leftPaneWidth - 11);
        rightPane.css("left", leftPaneWidth + 10);
        //console.log(leftPaneWidth);
    });

    $("#EDMS-left-pane").on("resizestop", function (event, ui) {
        var leftPaneWidth = $("#EDMS-left-pane").width();
        var rightPaneWidth = $("#EDMS-right-pane").width();;

        var cookieValues = { "leftPaneWidth": leftPaneWidth, "rightPaneWidth": rightPaneWidth };

        $.cookie("EDMS-splitter-values", JSON.stringify(cookieValues), {
            path: "/" //document.URL.replace(/^(?:\/\/|[^\/]+)*/, "")
        });
    });

    if ($.cookie("EDMS-splitter-values") != null) {
        var cookie = JSON.parse($.cookie("EDMS-splitter-values"));

        $("#EDMS-left-pane").width(cookie.leftPaneWidth);
        $("#EDMS-right-pane").width(990 - cookie.leftPaneWidth);
        $("#EDMS-right-pane").css("left", cookie.leftPaneWidth);
        $("#EDMS-left-pane").trigger("resize"); // fix to resize PossGrid to current width
    }
}

window.EDMS_state = "";

function initLeftPaneTree() {
    var childrenText = "[";
    var nodesToAdd;

    if (localStorage != null && localStorage != undefined && localStorage.getItem("EDMS") != undefined) {
        var EDMSstate = localStorage.getItem("EDMS");

        if (EDMSstate != null) {
            childrenText = JSON.parse(EDMSstate);
        }

        $("#EDMS-left-pane").dynatree({
            persist: false,
            clickFolderMode: 1,
            onActivate: treeNodeActivated,
            onSelect: treeNodeSelected,
            onClick: treeNodeClick,
            initAjax: {
                url: window["UrlLocation"] + "Services/EDMS.ashx?MethodName?GetGridResult",
            },
            onLazyRead: treeOnLazyRead,
            onExpand: treeOnQueryExpand,
            debugLevel: 0,
            children: childrenText
        });

        if (!treeInitialized) {
            var activeNode = $("#EDMS-left-pane").dynatree("getActiveNode");
            if (activeNode != null) treeNodeActivated(activeNode);
        }

    } else {
        // block here
        getFoldersJson('', false).success(function (data) {
            nodesToAdd = JSON.parse(data);
        });

        $("#EDMS-left-pane").dynatree({
            persist: false,
            clickFolderMode: 1,
            onActivate: treeNodeActivated,
            onSelect: treeNodeSelected,
            onClick: treeNodeClick,
            initAjax: {
                url: window["UrlLocation"] + "Services/EDMS.ashx?MethodName?GetGridResult",
            },
            onLazyRead: treeOnLazyRead,
            onExpand: treeOnQueryExpand,
            debugLevel: 0,
            children: nodesToAdd
        });

        var rootNode = $("#EDMS-left-pane").dynatree("getRoot");
        rootNode.removeChildren();

        for (var i = 0; i < nodesToAdd.documents.length; i++) {
            rootNode.addChild({
                title: nodesToAdd.documents[i].folderName,
                key: nodesToAdd.documents[i].folderId,
                mode: "nodeLazyLoad",
                isFolder: true,
                isLazy: true,
                expand: false,
                autoFocus: true,
            });
        }
    }
}

function expandNodes(node) {
    if (node == null || node == 'undefined') return;

    var children = node.getChildren();

    if (node.data.expand) {
        node.expand(true);
        if (node.data != null) console.log("Expanded: " + node.data.title + " node.expand:" + node.data.expand + "\n");
    }

    if (node.expand && children != null && children.length > 0) {
        for (var i = 0; i < children.length; i++) {

            //if (node.data != null) console.log(children[i].data.title);
            expandNodes(children[i]);
        }

    } else {
        return;
    }
}

function treeNodeActivated(node) {
    if (node.data.isFolder) {
        var cookieValues = { "EDMS": $("#EDMS-left-pane").dynatree("getTree").toDict() };

        if (localStorage != null && localStorage != undefined) {
            localStorage.setItem("EDMS", JSON.stringify(cookieValues.EDMS.children));
        }

        bindGrid(node);
        //console.log("\n-----------------------------BEGIN PRINT FROM treeNodeActivated - [" + (new Date()).format("hh:mm:ss.fff") + "]------------------------------\n" + JSON.stringify(cookieValues.EDMS.children) + "\n---------------------------END PRINT;--------------------------------\n");
    }
}

function treeOnQueryExpand(flag, node) {
    var cookieValues = { "EDMS": $("#EDMS-left-pane").dynatree("getTree").toDict() };
    //if (cookieValues != null) console.log("Cookie values length: " + JSON.stringify(cookieValues.EDMS.children).length);

    if (localStorage != null && localStorage != undefined) {
        localStorage.setItem("EDMS", JSON.stringify(cookieValues.EDMS.children));
    }

    //console.log("\n-----------------------------BEGIN PRINT FROM treeOnQueryExpand - [" + (new Date()).format("hh:mm:ss.fff") + "]------------------------------\n" + JSON.stringify(cookieValues.EDMS.children) + "\n---------------------------END PRINT;--------------------------------\n");
}

function treeNodeSelected(node) {

}

function treeNodeClick(node) {

}

function treeOnLazyRead(node) {
    switch (node.data.mode) {
        case "nodeLazyLoad":
            addNodes(node);
            break;
        default:
            throw "Invalid Mode " + dtnode.data.mode;
    }

    return true;
}

function getFoldersJson(folderId, async) {
    var _async = typeof async !== 'undefined' ? async : true;

    return $.ajax({
        url: window["UrlLocation"] + "Services/EDMS.ashx",
        type: "POST",
        data: { MethodName: "GetGridResult", FolderId: folderId },
        async: _async,
        error: getFoldersJsonError
    });
}

function getFoldersJsonError(xhr, ajaxOptions, thrownError) {
    alert("Greška prilikom dohvata foldera. Status: " + xhr.status);
}

function addNodes(node) {
    var i = 0;
    var nodesToAdd;

    getFoldersJson(node.data.key, true).success(function (data) {
        nodesToAdd = JSON.parse(data);
        for (i = 0; i < nodesToAdd.documents.length; i++) {
            node.addChild({
                title: nodesToAdd.documents[i].folderName,
                key: nodesToAdd.documents[i].folderId,
                mode: "nodeLazyLoad",
                isFolder: true,
                isLazy: true
            });
        }

        var cookieValues = { "EDMS": $("#EDMS-left-pane").dynatree("getTree").toDict() };
        //if (cookieValues != null) console.log("Cookie values length: " + JSON.stringify(cookieValues.EDMS.children).length);

        if (localStorage != null && localStorage != undefined) {
            localStorage.setItem("EDMS", JSON.stringify(cookieValues.EDMS.children));
        }

        //console.log("\n-----------------------------BEGIN PRINT FROM addNodes - [" + (new Date()).format("hh:mm:ss.fff") + "]------------------------------\n" + JSON.stringify(cookieValues.EDMS.children) + "\n---------------------------END PRINT;--------------------------------\n");
    });
}

function bindGrid(node) {
    $("input[type='hidden'][id$='hfSelectedFolderId']").val(node.data.key);
    $("input[type='hidden'][id$='hfSelectedDocumentId']").val("");

    $("input[type='submit'].btnBindGrid").click();
}