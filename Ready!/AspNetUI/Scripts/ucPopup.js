
/************************************************
Exact values handlers
*************************************************/
var currentForm;
function ExactNumeratorValueChanged(control) {

    var control = currentForm.find("input[clientID_CT=ctlExactNumVal]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "ExactNumeratorValueChanged",
                    "controlValue": control.val(),
                    "concentrationTypeCode": conType
                },
                function (response) { ExactNumeratorValueChangedResponse(response); }
             );
}

function ExactNumeratorValueChangedResponse(responseText) {


    var response = jQuery.parseJSON(responseText);
    if (response.action == "none") return;

    if (response.action == "clear") {
        currentForm.find("input[clientID_CT=ctlExactNumVal]").first().val("");
        currentForm.find("#infoRangeNMin").html("");
        currentForm.find("#infoConRangeNMin").html("");
    } else if (response.action == "update") {
        currentForm.find("input[clientID_CT=ctlExactNumVal]").first().val(response.newValue);
        currentForm.find("#infoRangeNMin").html(" " + response.newValue);
        currentForm.find("#infoConRangeNMin").html(" " + response.newValue);
        currentForm.find("#infoConcTypeText2").show();
    }

}

function ExactNumPrefixChanged(control) {
    var control = currentForm.find("select[clientID_CT=ctlExactNumPrefix]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "ExactNumPrefixChanged",
                    "controlValue": control.children("option:selected").val() != null ? control.children("option:selected").val() : "",
                    "concentrationTypeCode": conType
                },
                function (response) { ExactNumPrefixChangedResponse(response); }
             );

}

function ExactNumPrefixChangedResponse(responseText) {
    var response = jQuery.parseJSON(responseText);

    if (response.action == "none") return;

    if (response.action == "clear") {
        currentForm.find("select[clientID_CT=ctlExactNumPrefix]").first().val("");
        currentForm.find("#infoNumMaxPrefix").html("");
        currentForm.find("#infoConNumMaxPrefix").html("");
    } else if (response.action == "update") {

        currentForm.find("#infoNumMaxPrefix").html(response.infoNumMaxPrefix);
        currentForm.find("#infoConNumMaxPrefix").html(response.infoConNumMaxPrefix);
        if (currentForm.find("select[clientID_CT=ctlExactNumPrefix]").children("option:selected").text().toLowerCase().indexOf("single") != -1)
            currentForm.find("#infoConNUnit").attr("style", "margin-left:0px;");
        else
            currentForm.find("#infoConNUnit").attr("style", "margin-left:-3px;");
    }
}

function ExactNumUnitChanged(control) {
    var control = currentForm.find("select[clientID_CT=ctlExactNumUnit]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "ExactNumUnitChanged",
                    "controlValue": control.children("option:selected").val() != null ? control.children("option:selected").val() : "",
                    "concentrationTypeCode": conType
                },
                function (response) { ExactNumUnitChangedResponse(response); }
             );
}

function ExactNumUnitChangedResponse(responseText) {

    var response = jQuery.parseJSON(responseText);
    if (response.action == "none") return;
    if (response.action == "clear") {
        currentForm.find("#infoUnit").text("");
        currentForm.find("#infoConNUnit").text("");
        currentForm.find("#infoConNUnit").attr("style", "margin-left:0px;");
    } else if (response.action == "update") {

        currentForm.find("#infoUnit").text(currentForm.find("select[clientID_CT=ctlExactNumUnit]").first().children("option:selected").text());
        currentForm.find("#infoConNUnit").text(response.infoConNUnit);
    }

    if (response.action != "none" && currentForm.find("select[clientID_CT=ctlExactNumUnit]").first().children("option:selected").text().toLowerCase().indexOf("count") > -1) {
        currentForm.find("#infoConNUnit").attr("class", "red");
    }
    else {
        currentForm.find("#infoConNUnit").attr("class", "blue");
    }
}



function ExactDenominatorValueChanged(control) {

    var control = currentForm.find("input[clientID_CT=ctlExactDenVal]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();
    var expressedAs = currentForm.find("select[clientID_CT=ctlExpressedAs]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "ExactDenominatorValueChanged",
                    "controlValue": control.val(),
                    "concentrationTypeCode": conType,
                    "expressedAs": expressedAs
                },
                function (response) { ExactDenominatorValueChangedResponse(response); }
             );
}

function ExactDenominatorValueChangedResponse(responseText) {

    var response = jQuery.parseJSON(responseText);

    if (response.action == "none") return;

    if (response.action == "clear") {
        currentForm.find("input[clientID_CT=ctlExactDenVal]").first().val("");
        currentForm.find("#infoRangeDMin").html("");
        currentForm.find("#infoConRangeDMin").html("");
        currentForm.find("#infoConDenPrefix").html(currentForm.find("#infoConDenPrefix").text().trimStart());
        currentForm.find("#infoConDenPrefix").attr("style", "margin-left:-3px;");
    } else if (response.action == "update") {
        currentForm.find("input[clientID_CT=ctlExactDenVal]").first().val(response.newValue);
        if (response.newValue != "1") {
            currentForm.find("#infoUnitText").show();
            currentForm.find("#infoRangeDMin").html(" " + response.newValue);
            currentForm.find("#infoConRangeDMin").html("/" + response.newValue);
            currentForm.find("#infoConDenPrefix").html(" " + currentForm.find("#infoConDenPrefix").text().trimStart());
            currentForm.find("#infoConDenPrefix").attr("style", "margin-left:0px;");
        }
        else {
            currentForm.find("#infoUnitText").show();
            currentForm.find("#infoRangeDMin").html("");
            currentForm.find("#infoConRangeDMin").html("/");
            currentForm.find("#infoConDenPrefix").html(currentForm.find("#infoConDenPrefix").text().trimStart());
            currentForm.find("#infoConDenPrefix").attr("style", "margin-left:-3px;");
        }
    }
}

function ExactDenPrefixChanged(control) {
    var control = currentForm.find("select[clientID_CT=ctlExactDenPrefix]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();
    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "ExactDenPrefixChanged",
                    "controlValue": control.children("option:selected").val() != null ? control.children("option:selected").val() : "",
                    "concentrationTypeCode": conType
                },
                function (response) { ExactDenPrefixChangedResponse(response); }
             );
}

function ExactDenPrefixChangedResponse(responseText) {
    var response = jQuery.parseJSON(responseText);
    if (response.action == "none") return;
    if (response.action == "clear") {
        currentForm.find("#infoDenPrefix").html("");
        currentForm.find("#infoConDenPrefix").html("");
    } else if (response.action == "update") {

        currentForm.find("#infoDenPrefix").html(response.infoDenPrefix);
        currentForm.find("#infoConDenPrefix").html(response.infoConDenPrefix);
        if (response.processEnd != null) {

            if (currentForm.find("input[clientID_CT=ctlExactDenVal]").first().val() == "1") {
                concentrationTypeCode
                currentForm.find("#infoConDenPrefix").html(currentForm.find("#infoConDenPrefix").html().trimStart());
                currentForm.find("#infoConDenPrefix").attr("style", "margin-left:-3px;");
                if (currentForm.find("select[clientID_CT=ctlExactDenPrefix]").first().children("option:selected").text().toLowerCase().indexOf("single") != -1)
                    currentForm.find("#infoConDenExp").attr("style", "margin-left:0px;");
                else
                    currentForm.find("#infoConDenExp").attr("style", "margin-left:-3px;");

            }
            else {
                currentForm.find("#infoConDenPrefix").html(" " + currentForm.find("#infoConDenPrefix").text().trimStart());
                currentForm.find("#infoConDenPrefix").attr("style", "margin-left:0px;");
                if (currentForm.find("select[clientID_CT=ctlExactDenPrefix]").first().children("option:selected").text().toLowerCase().indexOf("single") != -1)
                    currentForm.find("#infoConDenExp").attr("style", "margin-left:0px;");
                else
                    currentForm.find("#infoConDenExp").attr("style", "margin-left:-3px;");
            }
        }
    }
}



function ExactDenUnitChanged(control) {

    var control = currentForm.find("select[clientID_CT=ctlExactDenUnit]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();
    var expressedAs = currentForm.find("select[clientID_CT=ctlExpressedAs]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "ExactDenUnitChanged",
                    "controlValue": control.children("option:selected").val() != null ? control.children("option:selected").val() : "",
                    "concentrationTypeCode": conType,
                    "expressedAs": expressedAs
                },
                function (response) { ExactDenUnitChangedResponse(response); }
             );
}

function ExactDenUnitChangedResponse(responseText) {
    var response = jQuery.parseJSON(responseText);

    if (response.action == "none") return;

    currentForm.find("#infoDenExp").text("");
    currentForm.find("#infoConDenExp").text("");
    if (response.action == "clear") {
        currentForm.find("#infoUnit").text("");
        currentForm.find("#infoConNUnit").text("");
        currentForm.find("#infoConNUnit").attr("style", "margin-left:0px;");
    } else if (response.action == "update") {

        currentForm.find("#infoUnitText").show();
        currentForm.find("#infoDenExp").text(" " + currentForm.find("select[clientID_CT=ctlExactDenUnit]").first().children("option:selected").text());
        currentForm.find("#infoConDenExp").text(response.infoConDenExp);
        if (currentForm.find("select[clientID_CT=ctlExactDenUnit]").first().children("option:selected").text().toLowerCase().indexOf("each") > -1) {
            currentForm.find("#infoDenExp").text("");
            currentForm.find("#infoConDenExp").text("");
        }

    }

}

/************************************************
LowLimit values handlers
*************************************************/

function switchRangeNoRange(range) {
    var infoRangeNMin = currentForm.find("#infoRangeNMin").text().trim();
    if (range == "fullRange") {
        currentForm.find("#infoConcTypeText1").show();
        currentForm.find("#infoRangeMinText").show();
        currentForm.find("#infoConcType").text("Range");
        currentForm.find("#infoConcTypeText2").hide();
    } else if (range == "sameRange") {
        currentForm.find("#infoConcTypeText1").hide();
        currentForm.find("#infoConcType").text("");
        currentForm.find("#infoRangeMinText").show();
        currentForm.find("#infoConcTypeText2").hide();
    } else if (range == "noRange") {

        currentForm.find("#infoConcTypeText1").hide();
        currentForm.find("#infoConcType").text("");
        currentForm.find("#infoRangeMinText").hide();
        currentForm.find("#infoConcTypeText2").show();
    }

    if (infoRangeNMin == "") {
        currentForm.find("#infoConcTypeText1").hide();
        currentForm.find("#infoConcType").text("");
        currentForm.find("#infoRangeMinText").hide();
        currentForm.find("#infoConcTypeText2").hide();
    }
}

function LowLimitNumValueChanged(control) {

    var control = currentForm.find("input[clientID_CT=ctlLowLimitNumVal]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "LowLimitNumValueChanged",
                    "controlValue": control.val(),
                    "concentrationTypeCode": conType
                },
                function (response) { LowLimitNumValueChangedResponse(response); }
             );
}

function LowLimitNumValueChangedResponse(responseText) {
    var response = jQuery.parseJSON(responseText);

    currentForm.find("#infoConRangeNMin").text("");
    currentForm.find("#infoRangeNMin").text("");

    if (response.action == "none") return;

    if (response.action == "clear") {
        currentForm.find("input[clientID_CT=ctlLowLimitNumVal]").first().val("");
        currentForm.find("#infoRangeNMin").html("");
        currentForm.find("#infoConRangeNMin").html("");
    } else if (response.action == "update") {
        currentForm.find("input[clientID_CT=ctlLowLimitNumVal]").first().val(response.newValue);

        currentForm.find("#infoRangeMinText").hide();

        currentForm.find("#infoRangeNMin").html(" " + response.newValue);
        currentForm.find("#infoConRangeNMin").html(" " + response.newValue);

        var ctlHighLimitNumVal = currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first();
        var ctlLowLimitNumVal = currentForm.find("input[clientID_CT=ctlLowLimitNumVal]").first();
        var ctlHighLimitNumPrefix = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
        var ctlLowLimitNumPrefix = currentForm.find("select[clientID_CT=ctlLowLimitNumPrefix]").first();

        if (ctlHighLimitNumVal.val() == ctlLowLimitNumVal.val() &&
                ctlHighLimitNumPrefix.val() == ctlLowLimitNumPrefix.val()) {
            currentForm.find("#infoRangeNText").hide();
            currentForm.find("#infoRangeNMax").text("");
            currentForm.find("#infoConRangeNMax").text("");
            currentForm.find("#infoNumMinPrefix").text("");
            currentForm.find("#infoConNumMinPrefix").text("");
            switchRangeNoRange("sameRange");
        }
        else {
            currentForm.find("#infoRangeNText").show();

            currentForm.find("#infoRangeNMax").text(ctlHighLimitNumVal.val());
            currentForm.find("#infoConRangeNMax").text(" - " + ctlHighLimitNumVal.val());
            switchRangeNoRange("fullRange");
            LowLimitNumPrefixChanged(null);
        }
    }
}

function LowLimitNumPrefixChanged(control) {


    var control = currentForm.find("select[clientID_CT=ctlLowLimitNumPrefix]").first();
    var highLimitPrefix = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();
    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "LowLimitNumPrefixChanged",
                    "controlValue": control.children("option:selected").val() != null ? control.children("option:selected").val() : "",
                    "concentrationTypeCode": conType,
                    "highLimitPrefix": highLimitPrefix.val()
                },
                function (response) { LowLimitNumPrefixChangedResponse(response); }
             );
}

function LowLimitNumPrefixChangedResponse(responseText) {
    var response = jQuery.parseJSON(responseText);

    if (response.action == "none") return;

    if (response.action == "clear") {

    } else if (response.action == "update") {

        var ctlHighLimitNumVal = currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first();
        var ctlLowLimitNumVal = currentForm.find("input[clientID_CT=ctlLowLimitNumVal]").first();
        var ctlHighLimitNumPrefix = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
        var ctlLowLimitNumPrefix = currentForm.find("select[clientID_CT=ctlLowLimitNumPrefix]").first();

        currentForm.find("#infoRangeNText").show();
        currentForm.find("#infoRangeNMax").text(ctlHighLimitNumVal.val());
        currentForm.find("#infoConRangeNMax").text(" - " + ctlHighLimitNumVal.val());

        if (response.prefixNumMin == response.prefixNumMax) {
            response.prefixNumMin = "";
            response.prefixConNumMin = "";

            if (ctlHighLimitNumVal.val() == ctlLowLimitNumVal.val()) {
                currentForm.find("#infoRangeNText").hide();
                currentForm.find("#infoRangeNMax").text("");
                currentForm.find("#infoConRangeNMax").text("");
                switchRangeNoRange("sameRange");
            } else {
                switchRangeNoRange("fullRange");
            }
        } else {
            switchRangeNoRange("fullRange");
        }

        if (ctlHighLimitNumPrefix.children("option:selected").text().toLowerCase().indexOf("single") > -1)
            currentForm.find("#infoConNUnit").attr("style", "margin-left:0px;");
        else
            currentForm.find("#infoConNUnit").attr("style", "margin-left:-3px;");

        currentForm.find("#infoNumMinPrefix").text(response.prefixNumMin);
        currentForm.find("#infoConNumMinPrefix").text(response.prefixConNumMin);
        currentForm.find("#infoNumMaxPrefix").text(response.prefixNumMax);
        currentForm.find("#infoConNumMaxPrefix").text(response.prefixConNumMax);
    }
}

function LowLimitNumUnitChanged(control) {

    var control = currentForm.find("select[clientID_CT=ctlLowLimitNumUnit]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "LowLimitNumUnitChanged",
                    "controlValue": control.children("option:selected").val() != null ? control.children("option:selected").val() : "",
                    "concentrationTypeCode": conType
                },
                function (response) { LowLimitNumUnitChangedResponse(response); }
             );
}

function LowLimitNumUnitChangedResponse(responseText) {

    var response = jQuery.parseJSON(responseText);
    if (response.action == "none") return;

    if (response.action == "clear") {

        currentForm.find("#infoUnit").text("");
        currentForm.find("#infoConNUnit").text("");
    } else if (response.action == "update") {


        var ctlLowLimitNumUnit = currentForm.find("select[clientID_CT=ctlLowLimitNumUnit]").first();
        var ctlHighLimitNumUnit = currentForm.find("select[clientID_CT=ctlHighLimitNumUnit]").first();

        var ctlHighLimitNumPrefixSelected = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first().children("option:selected");

        currentForm.find("#infoUnit").text("");
        currentForm.find("#infoConNUnit").text("");

        if (ctlLowLimitNumUnit.children("option:selected") != null) {
            currentForm.find("#infoUnit").text(ctlLowLimitNumUnit.children("option:selected").text().toString().trim());
            currentForm.find("#infoConNUnit").text(response.infoConNUnit);


            if (ctlLowLimitNumUnit.children("option:selected").text().toLowerCase().indexOf("count") > -1) {
                currentForm.find("#infoConNUnit").attr("class", "red");
            }
            else {
                currentForm.find("#infoConNUnit").attr("class", "blue");
            }

            if (ctlHighLimitNumPrefixSelected != null && ctlHighLimitNumPrefixSelected.text().toLowerCase().indexOf("single") > -1)
                currentForm.find("#infoConNUnit").attr("style", "margin-left:0px;");
            else
                currentForm.find("#infoConNUnit").attr("style", "margin-left:-3px;");

            ctlHighLimitNumUnit.val(ctlLowLimitNumUnit.val());

        } else {
            currentForm.find("#infoConNUnit").attr("style", "margin-left:0px;");
        }

    }


}



function LowLimitDenValueChanged(control) {

    var control = currentForm.find("input[clientID_CT=ctlLowLimitDenVal]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();
    var expressedAs = currentForm.find("select[clientID_CT=ctlExpressedAs]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "LowLimitDenValueChanged",
                    "controlValue": control.val(),
                    "concentrationTypeCode": conType,
                    "expressedAs": expressedAs
                },
                function (response) { LowLimitDenValueChangedResponse(response); }
             );
}

function LowLimitDenValueChangedResponse(responseText) {

    var response = jQuery.parseJSON(responseText);

    currentForm.find("#infoConRangeDMin").text("");
    currentForm.find("#infoRangeDMin").text("");

    if (response.action == "none") return;

    if (response.action == "clear") {
        currentForm.find("input[clientID_CT=ctlLowLimitDenVal]").first().val("");
        currentForm.find("input[clientID_CT=ctlHighLimitDenVal]").first().val("");
        currentForm.find("#infoRangeDMin").text("");
        currentForm.find("#infoConRangeDMin").text("/" + "");

    } else if (response.action == "update") {

        currentForm.find("#infoUnitText").show();
        if (response.newValue != null) {
            currentForm.find("input[clientID_CT=ctlLowLimitDenVal]").first().val(response.newValue);
            currentForm.find("input[clientID_CT=ctlHighLimitDenVal]").first().val(response.newValue);
        }

        currentForm.find("#infoRangeDMin").text(response.newValue);
        currentForm.find("#infoConRangeDMin").text("/" + response.newValue);

        var ctlHighLimitDenVal = currentForm.find("input[clientID_CT=ctlHighLimitDenVal]").first();
        var ctlLowLimitDenVal = currentForm.find("input[clientID_CT=ctlLowLimitDenVal]").first();

        if (ctlHighLimitDenVal.val() != ctlLowLimitDenVal.val()) {
            currentForm.find("#infoUnitText").show();
            currentForm.find("#infoRangeDText").show();
            currentForm.find("#infoRangeDMax").text(ctlHighLimitDenVal.val());
            currentForm.find("#infoConRangeDMax").text(" - " + ctlHighLimitDenVal.val());
            currentForm.find("#infoConDenPrefix").attr("style", "margin-left:0px;");
            currentForm.find("#infoConDenPrefix").text(" " + currentForm.find("#infoConDenPrefix").text().trimStart());
            switchRangeNoRange("fullRange");
        }
        else {
            currentForm.find("#infoRangeDText").hide();
            currentForm.find("#infoRangeDMax").text("");
            currentForm.find("#infoConRangeDMax").text("");

            if (ctlLowLimitDenVal.val() == "1") {
                currentForm.find("#infoRangeDMin").html("");
                currentForm.find("#infoConRangeDMin").html("/");
                currentForm.find("#infoConDenPrefix").html(currentForm.find("#infoConDenPrefix").text().trimStart());
                currentForm.find("#infoConDenPrefix").attr("style", "margin-left:-3px;");
            }
            else {
                currentForm.find("#infoConDenPrefix").text(" " + currentForm.find("#infoConDenPrefix").text().trimStart());
                currentForm.find("#infoConDenPrefix").attr("style", "margin-left:0px;");
            }
        }

    }
}

function LowLimitDenPrefixChanged(control) {

    var control = currentForm.find("select[clientID_CT=ctlLowLimitDenPrefix]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "LowLimitDenPrefixChanged",
                    "controlValue": control.children("option:selected").val(),
                    "concentrationTypeCode": conType
                },
                function (response) { LowLimitDenPrefixChangedResponse(response); }
             );
}

function LowLimitDenPrefixChangedResponse(responseText) {


    var response = jQuery.parseJSON(responseText);

    currentForm.find("#infoDenPrefix").text("");
    currentForm.find("#infoConDenPrefix").text("");

    if (response.action == "none") return;
    if (response.action == "clear") {
        currentForm.find("#infoDenPrefix").html("");
        currentForm.find("#infoConDenPrefix").html("");
    } else if (response.action == "update") {

        if (response.infoDenPrefix != null) {
            currentForm.find("#infoDenPrefix").html(response.infoDenPrefix);
            currentForm.find("#infoConDenPrefix").html(response.infoConDenPrefix);
            currentForm.find("#infoUnitText").show();
        }

        var ctlHighLimitDenVal = currentForm.find("input[clientID_CT=ctlHighLimitDenVal]").first();
        var ctlLowLimitDenVal = currentForm.find("input[clientID_CT=ctlLowLimitDenVal]").first();

        if (ctlLowLimitDenVal.val() == "1" && ctlHighLimitDenVal.val() == "1") {
            currentForm.find("#infoConDenPrefix").textcurrentForm.find("#infoConDenPrefix").text().trimStart();
            currentForm.find("#infoConDenPrefix").attr("style", "margin-left:-3px;");
        }
        else {
            currentForm.find("#infoConDenPrefix").text(" " + currentForm.find("#infoConDenPrefix").text().trimStart());
            currentForm.find("#infoConDenPrefix").attr("style", "margin-left:0px;");
        }

        var conTypectlLowLimitDenPrefixText = currentForm.find("select[clientID_CT=ctlLowLimitDenPrefix]").first().children("option:selected").text();
        if (conTypectlLowLimitDenPrefixText.toLowerCase().indexOf("single") > -1)
            currentForm.find("#infoConDenExp").attr("style", "margin-left:0px;");
        else
            currentForm.find("#infoConDenExp").attr("style", "margin-left:-3px;");

        currentForm.find("select[clientID_CT=ctlHighLimitDenPrefix]").first().val(currentForm.find("select[clientID_CT=ctlLowLimitDenPrefix]").first().val());

    }
}



function LowLimitDenUnitChanged(control) {

    var control = currentForm.find("select[clientID_CT=ctlLowLimitDenUnit]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();
    var expressedAs = currentForm.find("select[clientID_CT=ctlExpressedAs]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "LowLimitDenUnitChanged",
                    "controlValue": control.children("option:selected").val() != null ? control.children("option:selected").val() : "",
                    "controlTextValue": control.children("option:selected").text() != null ? control.children("option:selected").text() : "",
                    "concentrationTypeCode": conType,
                    "expressedAs": expressedAs
                },
                function (response) { LowLimitDenUnitChangedResponse(response); }
             );
}

function LowLimitDenUnitChangedResponse(responseText) {
    var response = jQuery.parseJSON(responseText);
    if (response.action == "none") return;

    currentForm.find("#infoConDenExp").text("");
    currentForm.find("#infoDenExp").text("");

    if (response.action == "clear") {
        currentForm.find("#infoDenExp").text("");
        currentForm.find("#infoConDenExp").text("");
    } else if (response.action == "update") {

        currentForm.find("#infoUnitText").show();
        currentForm.find("#infoDenExp").text(response.infoDenExp);
        currentForm.find("#infoConDenExp").text(response.infoConDenExp);

        var ctlHighLimitDenUnit = currentForm.find("select[clientID_CT=ctlHighLimitDenUnit]").first();
        var ctlLowLimitDenUnit = currentForm.find("select[clientID_CT=ctlLowLimitDenUnit]").first();


        if (currentForm.find("select[client_ID=ctlLowLimitDenUnit]").children("option:selected").text().toLowerCase().indexOf("each") > -1) {
            infoDenExp.InnerText = "";
            infoConDenExp.InnerText = "";
        }

        ctlHighLimitDenUnit.val(ctlLowLimitDenUnit.val());

    }

}

/************************************************
HighLimit values handlers
*************************************************/

function HighLimitNumValueChanged(control) {

    var control = currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();
    var message = "action=HighLimitNumValueChanged;controlValue=" + control.val() + ";concetrationTypeCode=" + conType;
    var context = 'HighLimitNumValueChanged';
    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "HighLimitNumValueChanged",
                    "controlValue": control.val(),
                    "concentrationTypeCode": conType
                },
                function (response) { HighLimitNumValueChangedResponse(response); }
             );
}

function HighLimitNumValueChangedResponse(responseText) {
    var response = jQuery.parseJSON(responseText);

    currentForm.find("#infoConRangeNMax").text("");
    currentForm.find("#infoRangeNMax").text("");

    if (response.action == "none") return;

    if (response.action == "clear") {
        currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first().val("");
        currentForm.find("#infoConRangeNMax").text("");
    } else if (response.action == "clear2") {
        currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first().val("");
    }
    else if (response.action == "update") {
        currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first().val(response.newValue);


        var ctlHighLimitNumVal = currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first();
        var ctlLowLimitNumVal = currentForm.find("input[clientID_CT=ctlLowLimitNumVal]").first();
        var ctlHighLimitNumPrefix = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
        var ctlLowLimitNumPrefix = currentForm.find("select[clientID_CT=ctlLowLimitNumPrefix]").first();


        if (ctlHighLimitNumVal.val() == ctlLowLimitNumVal.val() &&
                ctlHighLimitNumPrefix.val() == ctlLowLimitNumPrefix.val()) {
            currentForm.find("#infoRangeNText").hide();
            currentForm.find("#infoRangeNMax").text("");
            currentForm.find("#infoConRangeNMax").text("");
            currentForm.find("#infoNumMinPrefix").text("");
            currentForm.find("#infoConNumMinPrefix").text("");
            switchRangeNoRange("sameRange");
        }
        else {

            currentForm.find("#infoRangeNText").show();
            currentForm.find("#infoRangeNMax").text(ctlHighLimitNumVal.val());
            currentForm.find("#infoConRangeNMax").text(" - " + ctlHighLimitNumVal.val());
            LowLimitNumPrefixChanged(null);
        }
    }
}

function HighLimitNumPrefixChanged(control) {

    var control = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
    var lowLimitPrefix = currentForm.find("select[clientID_CT=ctlLowLimitNumPrefix]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "HighLimitNumPrefixChanged",
                    "controlValue": control.children("option:selected").val() != null ? control.children("option:selected").val() : "",
                    "concentrationTypeCode": conType,
                    "lowLimitPrefix": lowLimitPrefix.val() != null ? lowLimitPrefix.val() : ""
                },
                function (response) { HighLimitNumPrefixChangedResponse(response); }
             );
}

function HighLimitNumPrefixChangedResponse(responseText) {

    var response = jQuery.parseJSON(responseText);

    if (response.action == "none") return;

    if (response.action == "clear") {

    } else if (response.action == "update") {

        var ctlHighLimitNumVal = currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first();
        var ctlLowLimitNumVal = currentForm.find("input[clientID_CT=ctlLowLimitNumVal]").first();
        var ctlHighLimitNumPrefix = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
        var ctlLowLimitNumPrefix = currentForm.find("select[clientID_CT=ctlLowLimitNumPrefix]").first();


        currentForm.find("#infoRangeNText").show();
        currentForm.find("#infoRangeNMax").text(ctlHighLimitNumVal.val());
        currentForm.find("#infoConRangeNMax").text(" - " + ctlHighLimitNumVal.val());

        if (response.prefixNumMin == response.prefixNumMax) {
            response.prefixNumMin = "";
            response.prefixConNumMin = "";

            if (ctlHighLimitNumVal.val() == ctlLowLimitNumVal.val()) {
                currentForm.find("#infoRangeNText").hide();
                currentForm.find("#infoRangeNMax").text("");
                currentForm.find("#infoConRangeNMax").text("");
                switchRangeNoRange("sameRange");
            } else {
                switchRangeNoRange("fullRange");
            }
        } else {
            switchRangeNoRange("fullRange");
        }


        if (ctlHighLimitNumPrefix.children("option:selected").text().toLowerCase().indexOf("single") > -1)
            currentForm.find("#infoConNUnit").attr("style", "margin-left:0px;");
        else
            currentForm.find("#infoConNUnit").attr("style", "margin-left:-3px;");

        currentForm.find("#infoNumMinPrefix").text(response.prefixNumMin);
        currentForm.find("#infoConNumMinPrefix").text(response.prefixConNumMin);
        currentForm.find("#infoNumMaxPrefix").text(response.prefixNumMax);
        currentForm.find("#infoConNumMaxPrefix").text(response.prefixConNumMax);
    }
}

function HighLimitNumUnitChanged(control) {
}

function HighLimitNumUnitChangedResponse(responseText) {
}



function HighLimitDenValueChanged(control) {

    var control = currentForm.find("input[clientID_CT=ctlHighLimitDenVal]").first();
    var conType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text();
    var expressedAs = currentForm.find("select[clientID_CT=ctlExpressedAs]").first().children("option:selected").text();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "HighLimitDenValueChanged",
                    "controlValue": control.val(),
                    "concentrationTypeCode": conType,
                    "expressedAs": expressedAs
                },
                function (response) { HighLimitDenValueChangedResponse(response); }
             );
}

function HighLimitDenValueChangedResponse(responseText) {

    var response = jQuery.parseJSON(responseText);

    currentForm.find("#infoConRangeDMax").text("");
    currentForm.find("#infoRangeDMax").text("");

    if (response.action == "none") return;

    if (response.action == "clear") {
        currentForm.find("input[clientID_CT=ctlHighLimitDenVal]").first().val("");


    } else if (response.action == "update") {

        if (response.newValue != null) {
            currentForm.find("input[clientID_CT=ctlHighLimittDenVal]").first().val(response.newValue);
        }
        var ctlHighLimitDenVal = currentForm.find("input[clientID_CT=ctlHighLimitDenVal]").first();
        var ctlLowLimitDenVal = currentForm.find("input[clientID_CT=ctlLowLimitDenVal]").first();

        if (ctlHighLimitDenVal.val() != ctlHighLimitDenVal.val()) {
            currentForm.find("#infoUnitText").show();
            currentForm.find("#infoRangeDText").show();
            currentForm.find("#infoRangeDMax").text(ctlHighLimitDenVal.val());
            currentForm.find("#infoConRangeDMax").text(" - " + ctlHighLimitDenVal.val());
            currentForm.find("#infoRangeDMin").text(ctlLowLimitDenVal.val());
            currentForm.find("#infoConRangeDMin").text("/" + ctlLowLimitDenVal.val());
            currentForm.find("#infoConDenPrefix").attr("style", "margin-left:0px;");
            currentForm.find("#infoConDenPrefix").text(" " + currentForm.find("#infoConDenPrefix").text().trimStart());
        }
        else {
            currentForm.find("#infoRangeDText").hide();
            currentForm.find("#infoRangeDMax").text("");
            currentForm.find("#infoConRangeDMax").text("");

            if (ctlLowLimitDenVal.val() == "1") {
                currentForm.find("#infoRangeDMin").html("");
                currentForm.find("#infoConRangeDMin").html("/");
                currentForm.find("#infoConDenPrefix").html(currentForm.find("#infoConDenPrefix").text().trimStart());
                currentForm.find("#infoConDenPrefix").attr("style", "margin-left:-3px;");
            }
            else {
                currentForm.find("#infoConDenPrefix").text(" " + currentForm.find("#infoConDenPrefix").text().trimStart());
                currentForm.find("#infoConDenPrefix").attr("style", "margin-left:0px;");
            }
        }

    }
}

function HighLimitDenPrefixChanged(control) {
}

function HighLimitDenPrefixChangedResponse(responseText) {
}

function HighLimitDenUnitChanged(control) {
}

function HighLimitDenUnitChangedResponse(responseText) {
}

function ConcentrationTypeChanged(ctrl) {

    var control = currentForm.find("select[clientID_CT=ctlConTypeCode]").first();
    var message = "action=ConcentrationTypeChanged;controlValue=" + control.children("option:selected").val();
    var context = 'ConcentrationTypeChanged';

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "ConcentrationTypeChanged",
                    "controlValue": control.children("option:selected").val()

                },
                function (response) { ConcentrationTypeChangedResponse(response); }
             );
}

function ConcentrationTypeChangedResponse(responseText) {


    var response = jQuery.parseJSON(responseText);

    if (response.newState != "none") {


        if (response.newState.toLowerCase().indexOf("range") > -1) {
            currentForm.find("#infoConcTypeText1").show();
            currentForm.find("#infoRangeMinText").show();
            currentForm.find("#infoConcTypeText2").hide();
            currentForm.find("#pnlLowLimit").show(); currentForm.find("#pnlLowLimitVisible").val("true");
            currentForm.find("#pnlHighLimit").show(); currentForm.find("#pnlHighLimitVisible").val("true");
            currentForm.find("#pnlExactLimit").hide(); currentForm.find("#pnlExactLimitVisible").val("false");
            currentForm.find("#infoRangeMinText").show();
            currentForm.find("#infoConcType").text(" " + response.newState + " ");

            var ctlHighLimitNumUnit = currentForm.find("select[clientID_CT=ctlHighLimitNumUnit]").first();
            var ctlHighLimitNumPrefix = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
            var ctlHighLimitDenPrefix = currentForm.find("select[clientID_CT=ctlHighLimitDenPrefix]").first();
            var ctlHighLimitDenUnit = currentForm.find("select[clientID_CT=ctlHighLimitDenUnit]").first();

            var ctlLowLimitNumPrefix = currentForm.find("select[clientID_CT=ctlLowLimitNumPrefix]").first();
            var ctlLowLimitNumUnit = currentForm.find("select[clientID_CT=ctlLowLimitNumUnit]").first();
            var ctlLowLimitDenPrefix = currentForm.find("select[clientID_CT=ctlLowLimitDenPrefix]").first();
            var ctlLowLimitDenUnit = currentForm.find("select[clientID_CT=ctlLowLimitDenUnit]").first();

            ctlHighLimitNumUnit.attr("disabled", "disabled");
            ctlHighLimitNumUnit.attr("class", "aspNetDisabled");
            ctlHighLimitDenPrefix.attr("disabled", "disabled");
            ctlHighLimitDenPrefix.attr("class", "aspNetDisabled")
            ctlHighLimitDenUnit.attr("disabled", "disabled");
            ctlHighLimitDenUnit.attr("class", "aspNetDisabled")


            ctlHighLimitNumPrefix.val(ctlLowLimitNumPrefix.val());
            ctlHighLimitNumUnit.val(ctlLowLimitNumUnit.val());
            ctlHighLimitDenPrefix.val(ctlLowLimitDenPrefix.val());
            ctlHighLimitDenUnit.val(ctlLowLimitDenUnit.val());

            BindFullDescription("Range");
        }
        else {

            currentForm.find("#infoConcType").text("");
            currentForm.find("#infoConcTypeText1").hide();
            currentForm.find("#infoConcTypeText2").show();
            currentForm.find("#pnlExactLimit").show(); currentForm.find("#pnlExactLimitVisible").val("true");
            currentForm.find("#pnlLowLimit").hide(); currentForm.find("#pnlLowLimitVisible").val("false");
            currentForm.find("#pnlHighLimit").hide(); currentForm.find("#pnlHighLimitVisible").val("false");
            currentForm.find("#infoRangeMinText").hide();

            var ctlHighLimitNumUnit = currentForm.find("select[clientID_CT=ctlHighLimitNumUnit]").first();
            var ctlHighLimitNumPrefix = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
            var ctlHighLimitDenPrefix = currentForm.find("select[clientID_CT=ctlHighLimitDenPrefix]").first();
            var ctlHighLimitDenUnit = currentForm.find("select[clientID_CT=ctlHighLimitDenUnit]").first();

            ctlHighLimitNumPrefix.removeAttr("disabled"); ctlHighLimitNumPrefix.removeAttr("class");
            ctlHighLimitNumUnit.removeAttr("disabled"); ctlHighLimitNumUnit.removeAttr("class");
            ctlHighLimitDenPrefix.removeAttr("disabled"); ctlHighLimitDenPrefix.removeAttr("class");
            ctlHighLimitDenUnit.removeAttr("disabled"); ctlHighLimitDenUnit.removeAttr("class");

            //pnlExactLimit
            if (response.newState.toLowerCase().indexOf("approximately") > -1) {
                currentForm.find("#pnlExactLimit fieldset:first legend:first").text("Approximate value");
            }
            else if (response.newState.toLowerCase().indexOf("not less than") > -1) {
                currentForm.find("#pnlExactLimit fieldset:first legend:first").text("Minimum value");
            }
            else if (response.newState.toLowerCase().indexOf("up to") > -1) {
                currentForm.find("#pnlExactLimit fieldset:first legend:first").text("Maximum value");

            }
            else if (response.newState.toLowerCase().indexOf("equal") > -1) {
                currentForm.find("#pnlExactLimit fieldset:first legend:first").text("Exact value");
            }
            else if (response.newState.toLowerCase().indexOf("average") > -1) {
                currentForm.find("#pnlExactLimit fieldset:first legend:first").text("Average value");
            }

            BindFullDescription("Exact");
        }
        currentForm.find("#panelGroup").show();
    } else {
        currentForm.find("#panelGroup").hide();
        currentForm.find("#pnlExactLimit").hide(); currentForm.find("#pnlExactLimitVisible").val("false");
        currentForm.find("#pnlLowLimit").hide(); currentForm.find("#pnlLowLimitVisible").val("false");
        currentForm.find("#pnlHighLimit").hide(); currentForm.find("#pnlLowHighVisible").val("false");
        currentForm.find("#infoConcTypeText1").hide();
        currentForm.find("#infoConcTypeText2").hide();
        ClearFullDescription();

    }
}


function SubstanceChanged(control) {

    var control = currentForm.find("#ctlSubstance_PK").first();
    var controlValue = control.val();

    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "SubstanceChanged",
                    "substanceId": controlValue

                },
                function (response) { SubstanceChangedResponse(response); }
             );

}

function SubstanceChangedResponse(responseText) {
    var response = jQuery.parseJSON(responseText);

    if (response.action == "none") return;

    if (response.action == "update") {

        currentForm.find("input[clientID_CT=SubstanceChanged]").first().val(response.substanceName);

        currentForm.find("#infoSubName").text(response.substanceName);
        currentForm.find("#infoConSubName").text(response.substanceName);
        currentForm.find("input[clientID_CT=substanceSearch]").first().val(response.substanceName);
        currentForm.find("#infoSubNameText2").show();
        currentForm.find("#infoConSubName").show();
        currentForm.find("#infoSubNameText").show();
    } else if (response.action == "clear") {
        currentForm.find("#infoSubName").text("");
        currentForm.find("#infoConSubName").text("");
        currentForm.find("#ctlSubstance_PK").first().val("");
        currentForm.find("input[clientID_CT=substanceSearch]").first().val("");
    }


    var searchControl = currentForm.find("#substanceSelector");
    if (searchControl.is(":visible")) repositionSubstanceSearcher();

}
function ExpressedAsChanged(controlValue) {

    var control = currentForm.find("select[clientID_CT=ctlExpressedAs]").first();
    var controlValue = control.children("option:selected").first().val();
    var controlTextValue = control.children("option:selected").first().text();


    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "ExpressedAsChanged",
                    "controlValue": controlValue,
                    "controlTextValue": controlTextValue

                },
                function (response) { ExpressedAsChangedResponse(response); }
             );

}

function ExpressedAsChangedResponse(responseText) {


    var response = jQuery.parseJSON(responseText);

    var control = currentForm.find("select[clientID_CT=ctlExpressedAs]").first();
    var controlValue = control.children("option:selected").first().val();
    var controlTextValue = control.children("option:selected").first().text();
    var ctlConTypeCode = currentForm.find("select[clientID_CT=ctlConTypeCode]").first();

    if (response.action == "clear") {
        ctlExactDenUnit = currentForm.find("select[clientID_CT=ctlExactDenUnit]").first();
        ctlLowLimitDenUnit = currentForm.find("select[clientID_CT=ctlLowLimitDenUnit]").first();
        ctlHighLimitDenUnit = currentForm.find("select[clientID_CT=ctlHighLimitDenUnit]").first();

        ctlExactDenUnit.empty();
        ctlLowLimitDenUnit.empty();
        ctlHighLimitDenUnit.empty();

        currentForm.find("#HighLimitExp").html("");
        currentForm.find("#LowLimitExp").html("");
        currentForm.find("#ExactLimitExp").html("");
    }

    if (response.action == "update") {

        currentForm.find("#HighLimitExp").html(response.label);
        currentForm.find("#LowLimitExp").html(response.label);
        currentForm.find("#ExactLimitExp").html(response.label);

        ctlExactDenUnit = currentForm.find("select[clientID_CT=ctlExactDenUnit]").first();
        ctlLowLimitDenUnit = currentForm.find("select[clientID_CT=ctlLowLimitDenUnit]").first();
        ctlHighLimitDenUnit = currentForm.find("select[clientID_CT=ctlHighLimitDenUnit]").first();

        ctlExactDenUnit.empty();
        ctlLowLimitDenUnit.empty();
        ctlHighLimitDenUnit.empty();

        ctlExactDenUnit.append($("<option></option>").attr("value", "").text("- Choose -"));
        ctlLowLimitDenUnit.append($("<option></option>").attr("value", "").text("- Choose -"));
        ctlHighLimitDenUnit.append($("<option></option>").attr("value", "").text("- Choose -"));

        for (i = 0; i < response.options.length; i++) {
            ctlExactDenUnit.append($("<option></option>")
            .attr("value", response.options[i]["value"])
            .text(response.options[i]["text"]));

            ctlLowLimitDenUnit.append($("<option></option>")
            .attr("value", response.options[i]["value"])
            .text(response.options[i]["text"]));

            ctlHighLimitDenUnit.append($("<option></option>")
            .attr("value", response.options[i]["value"])
            .text(response.options[i]["text"]));
        }
        ctlExactDenUnit.find("option").attr("selected", false);
        ctlLowLimitDenUnit.find("option").attr("selected", false);
        ctlHighLimitDenUnit.find("option").attr("selected", false);

        ctlExactDenPrefix = currentForm.find("select[clientID_CT=ctlExactDenPrefix]").first();
        ctlLowLimitDenPrefix = currentForm.find("select[clientID_CT=ctlLowLimitDenPrefix]").first();
        ctlHighLimitDenPrefix = currentForm.find("select[clientID_CT=ctlHighLimitDenPrefix]").first();
        ctlExactDenPrefix.find("option").attr("selected", false);
        ctlLowLimitDenPrefix.find("option").attr("selected", false);
        ctlHighLimitDenPrefix.find("option").attr("selected", false);


    }

    if (currentForm.find("select[clientID_CT=ctlConTypeCode]").first().children("option:selected").text().toLowerCase().indexOf("range") > -1) {
        BindFullDescription("Range");
    } else {
        BindFullDescription("Exact");
    }
}

function ClearFullDescription() {
    currentForm.find("#infoMeasure").text("");
    currentForm.find("#infoMeasure2").text("");
    currentForm.find("#infoNumMinPrefix").text("");
    currentForm.find("#infoNumMaxPrefix").text("");
    currentForm.find("#infoRangeNMax").text("");
    currentForm.find("#infoRangeNMin").text("");
    currentForm.find("#infoRangeDMin").text("");
    currentForm.find("#infoRangeDMax").text("");
    currentForm.find("#infoDenPrefix").text("");
    currentForm.find("#infoDenExp").text("");
    currentForm.find("#infoUnit").text("");

    currentForm.find("#infoConRangeNMin").text("");
    currentForm.find("#infoConRangeNMax").text("");
    currentForm.find("#infoConNumMinPrefix").text("");
    currentForm.find("#infoConNumMaxPrefix").text("");
    currentForm.find("#infoConNUnit").text("");
    currentForm.find("#infoConRangeDMin").text("");
    currentForm.find("#infoConRangeDMax").text("");
    currentForm.find("#infoConDenPrefix").text("");
    currentForm.find("#infoConDenExp").text("");

    currentForm.find("#infoUnitText").hide();
    currentForm.find("#infoRangeMinText").hide();
    currentForm.find("#infoRangeNText").hide();
    currentForm.find("#infoRangeDText").hide();
}

function BindFullDescription(arg) {
    ClearFullDescription();
    if (arg == "Range") {

        SubstanceChanged(null);
        LowLimitNumValueChanged(null);
        LowLimitNumPrefixChanged(null);
        LowLimitNumUnitChanged(null);

        LowLimitDenValueChanged(null);
        LowLimitDenPrefixChanged(null);
        LowLimitDenUnitChanged(null);

        HighLimitNumValueChanged(null);

        HighLimitNumPrefixChanged(null);
        //HighLimitNumUnitChanged(null);

        HighLimitDenValueChanged(null);
        //HighLimitDenPrefixChanged(null);
        //HighLimitDenUnitChanged(null);
    }
    else {

        SubstanceChanged(null);
        ExactDenominatorValueChanged(null);
        ExactDenPrefixChanged(null);
        ExactDenUnitChanged(null);
        ExactNumeratorValueChanged(null);
        ExactNumPrefixChanged(null);
        ExactNumUnitChanged(null);
    }
}



function BindForm() {

    var ctl = $("#ctlUcPopupVisible[value='true']").first();
    currentForm = ctl.closest(".modal_container");
    if (currentForm == null || currentForm == undefined) {
        return;
    }

    var formType = currentForm.find("#ctlFormType").val();

    switch (formType) {
        case "Adjuvant":

            if (currentForm.find("#pnlExactLimitVisible").val() == "true") currentForm.find("#pnlExactLimit").show(); else currentForm.find("#pnlExactLimit").hide();
            if (currentForm.find("#pnlLowLimitVisible").val() == "true") currentForm.find("#pnlLowLimit").show(); else currentForm.find("#pnlLowLimit").hide();
            if (currentForm.find("#pnlHighLimitVisible").val() == "true") currentForm.find("#pnlHighLimit").show(); else currentForm.find("#pnlHighLimit").hide();

            currentForm.find("#panelGroup").show();
            break;
        case "Excipient":
            currentForm.find("#panelGroup").hide();
            break;
        case "ActiveIngredient":

            if (currentForm.find("#pnlExactLimitVisible").val() == "true") currentForm.find("#pnlExactLimit").show(); else currentForm.find("#pnlExactLimit").hide();
            if (currentForm.find("#pnlLowLimitVisible").val() == "true") currentForm.find("#pnlLowLimit").show(); else currentForm.find("#pnlLowLimit").hide();
            if (currentForm.find("#pnlHighLimitVisible").val() == "true") currentForm.find("#pnlHighLimit").show(); else currentForm.find("#pnlHighLimit").hide();

            currentForm.find("#panelGroup").show();
            break;
    }

    var ctlConType = currentForm.find("select[clientID_CT=ctlConTypeCode]").first();
    var conType = ctlConType.children("option:selected").text().toLowerCase();
    var infoRangeNMin = currentForm.find("#infoRangeNMin");
    var infoRangeNMax = currentForm.find("#infoRangeNMax");
    if (infoRangeNMin == null || infoRangeNMin.text().trim() == "" || infoRangeNMax == null || infoRangeNMax.text().trim() == "" || ctlConType.children("option:selected").text().toLowerCase().indexOf("range") < 0) {
        currentForm.find("#infoRangeNText").hide();
    }

    var denominator = "";


    if (conType.indexOf("range") > -1) denominator = currentForm.find("input[clientID_CT=ctlLowLimitDenVal]").first().val(); else denominator = currentForm.find("input[clientID_CT=ctlExactDenVal]").first().val();
    if (denominator == null || denominator.toLowerCase().trim() == "") {
        currentForm.find("#infoUnitText").hide();
    }

    currentForm.find("#infoRangeMinText").hide();
    currentForm.find("#infoRangeDText").hide();

    if (conType.indexOf("range") > -1) {
        currentForm.find("#infoConcTypeText2").hide();
        currentForm.find("#infoConcType").text("Range");

        var ctlHighLimitNumVal = currentForm.find("input[clientID_CT=ctlHighLimitNumVal]").first();
        var ctlLowLimitNumVal = currentForm.find("input[clientID_CT=ctlLowLimitNumVal]").first();
        var ctlHighLimitNumPrefix = currentForm.find("select[clientID_CT=ctlHighLimitNumPrefix]").first();
        var ctlLowLimitNumPrefix = currentForm.find("select[clientID_CT=ctlLowLimitNumPrefix]").first();

        if (ctlHighLimitNumPrefix.val() == ctlLowLimitNumPrefix.val() &&
            ctlHighLimitNumVal.val() == ctlLowLimitNumVal.val()) {
            switchRangeNoRange("sameRange");
        } else {
            switchRangeNoRange("fullRange");
        }

    } else {
        currentForm.find("#infoConcTypeText1").hide();
        currentForm.find("#infoConcType").text("");
        switchRangeNoRange("noRange");

    }

    var infoSubName = currentForm.find("#infoSubName");
    if (infoSubName == null || infoSubName.text().trim() == "") {
        currentForm.find("#infoSubNameText").hide();
        currentForm.find("#infoSubNameText2").hide();
    }

    currentForm.find("input[ClientID_CT=substanceSearch]").css("background-color", "#F4F4F4");
    currentForm.find("input[ClientID_CT=substanceSearch]").attr("readonly", "readonly");

    currentForm.find("input").click(
        function (evt) { preventFormSubmission(evt); }
    );

}


function startSubstanceSearch() {

    var searchControl = currentForm.find("#substanceSelectorModal");
    var searchBox = currentForm.find("input[clientID_CT=substanceSearch]");
    searchControl.show();
    var searchName = currentForm.find("#subSearchName");
    searchName.val("");
    searchName.unbind('keyup');
    searchName.keyup(function (event) { RefreshSubstanceList(event) });
    var searchEvCode = currentForm.find("#subSearchEvCode");
    searchEvCode.val("");
    searchEvCode.unbind('keyup');
    searchEvCode.keyup(function (event) { RefreshSubstanceList(event) });
    RefreshSubstanceList();
    searchName.focus();

    repositionSubstanceSearcher();

}

function repositionSubstanceSearcher() {
    var searchControl = currentForm.find("#substanceSelector");
    var searchBox = currentForm.find("input[clientID_CT=substanceSearch]").closest('.modal_container');
    searchControl.offset({ top: searchBox.offset().top + 24, left: searchBox.offset().left });

}

function RefreshSubstanceList(event) {

    var name = currentForm.find("#subSearchName").first();
    var evcode = currentForm.find("#subSearchEvCode").first();
    currentForm.find("#subListPage").val("0");
    jQuery.post(
                ucPopupInvokeCallback,
                { "action": "RefreshSubstanceList",
                    "name": name.val(),
                    "evcode": evcode.val(),
                    "page": "1"

                },
                function (response) { RefreshSubstanceListResponse(response); }
             );

}

function preventFormSubmission(event) {
    if (event != null && event.which == 13 && $(event.target).attr("id") == "substanceSearchCurrentPage") { event.stopPropagation(); event.preventDefault(); GetSubstancePage("Exact"); return false; }
    if (event != null && event.which == 13) { event.stopPropagation(); event.preventDefault(); event.which = null; event.keyCode = null; return false; }

}

function GetSubstancePage(arg) {

    var currentPage = parseInt(currentForm.find("#subListPage").val());
    var newPage = currentPage;
    var name = currentForm.find("#subSearchName").first();
    var evcode = currentForm.find("#subSearchEvCode").first();

    if (arg == "-1") {
        if (newPage > 1) newPage--;
    } else if (arg == "+1") {

        if (newPage + 1 <= parseInt(currentForm.find("#subListPageTotalPages").val())) newPage++;
    } else {
        newPage = parseInt(currentForm.find("#substanceSearchCurrentPage").val());
        if (!(newPage > 0 && newPage <= parseInt(currentForm.find("#subListPageTotalPages").val()))) newPage = -1;
    }

    if (newPage != -1 && newPage != currentPage) {

        currentForm.find("#subListPage").val(newPage);
        jQuery.post(
                    ucPopupInvokeCallback,
                    { "action": "RefreshSubstanceList",
                        "name": name.val(),
                        "evcode": evcode.val(),
                        "page": newPage

                    },
                    function (response) { RefreshSubstanceListResponse(response); }
                );
    }

}


function RefreshSubstanceListResponse(responseText) {


    var response = jQuery.parseJSON(responseText);
    var name = currentForm.find("#subSearchName").first();
    var evcode = currentForm.find("#subSearchEvCode").first();
    if (response.name != name.val().trim() || evcode.val() != response.evcode.trim()) return;
    var subs = currentForm.find("#subSearchList").first();

    subs.empty();

    if (response.substances != null) {

        currentForm.find("#subListPage").val(response.page);
        currentForm.find("#substanceSearchTotalPages").text("/ " + response.totalPages);
        currentForm.find("#substanceSearchCurrentPage").val(response.page);
        currentForm.find("#subListPageTotalPages").val(response.totalPages);
        for (i = 0; i < response.substances.length; i++) {
            subs.append($("<option></option>")
                .attr("value", response.substances[i]["value"])
                .attr("title", response.substances[i]["text"])
                .text(response.substances[i]["text"]));
        }


    }

    subs.scrollTop(0);
    subs.dblclick(function () { substanceClicked(); });
}

function substanceClicked() {
    var subs = currentForm.find("#subSearchList").first();
    var subPk = currentForm.find("#ctlSubstance_PK").first();
    var selected = subs.children("option:selected").first();
    if (selected != null) {

        var searchControl = currentForm.find("#substanceSelectorModal");
        var searchBox = currentForm.find("input[clientID_CT=substanceSearch]");
        searchControl.hide();
        searchBox.val(selected.text());
        subPk.val(selected.val());
        SubstanceChanged(null);

    }
}

function cancelSusbtanceSearch() {
    var searchControl = currentForm.find("#substanceSelectorModal");
    searchControl.hide();
}

function removeSelectedSubstance() {

    var subPk = currentForm.find("#ctlSubstance_PK").first();
    var searchBox = currentForm.find("input[clientID_CT=substanceSearch]");
    searchBox.val("");
    subPk.val("");
    SubstanceChanged(null);
}

function substanceListScroll() {
    var substances = currentForm.find("#subSearchList").first();

}

function ucPopupInitialize(sender, args) {
    BindForm();
}


String.prototype.trimStart = function (c) {
    if (this.length == 0)
        return this;
    c = c ? c : ' ';
    var i = 0;
    var val = 0;
    for (; this.charAt(i) == c && i < this.length; i++);
    return this.substring(i);
}

        

