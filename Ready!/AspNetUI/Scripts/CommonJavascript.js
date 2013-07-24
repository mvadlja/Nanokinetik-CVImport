// JScript File
//////////////////////////////////
// Async postback
//////////////////////////////////

function __doPostBackAsync(eventName, eventArgs) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    if (!Array.contains(prm._asyncPostBackControlIDs, eventName)) {
        prm._asyncPostBackControlIDs.push(eventName);
    }

    if (!Array.contains(prm._asyncPostBackControlClientIDs, eventName)) {
        prm._asyncPostBackControlClientIDs.push(eventName);
    }

    __doPostBack(eventName, eventArgs);
}


//////////////////////////////////
// Grid View row mouse over effect
//////////////////////////////////

var OldColor;
function MouseIn(obj) {
    try {
        OldColor = obj.style.backgroundColor;
        obj.style.backgroundColor = "#FFFFDD";
    }
    catch (er) { }
}
function MouseOut(obj) {
    try {
        obj.style.backgroundColor = OldColor;
    }
    catch (er) { }
}
