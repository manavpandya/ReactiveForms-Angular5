function IndexValidation() {
    if (document.getElementById("txtFrom") != null && document.getElementById("txtFrom").value == "") {
        alert("Please enter from price.");
        document.getElementById("txtFrom").focus();
        return false;
    }
    else if (document.getElementById("txtTo") != null && document.getElementById("txtTo").value == "") {
        alert("Please enter to price.");
        document.getElementById("txtTo").focus();
        return false;
    }
    else {
        var fromPrice = parseFloat(document.getElementById("txtFrom").value);
        var toPrice = parseFloat(document.getElementById("txtTo").value);
        if (parseFloat(fromPrice) > parseFloat(toPrice)) {
            alert("Please enter valid price range.");
            document.getElementById("txtFrom").focus();
            return false;
        }
        else {
            return true;
        }
    }
    return true;
}




function CheckSelection() {

}

function ColorSelection(clrvalue) {
    document.getElementById("hdnColorSelection").value = clrvalue;

}

function CheckSelectionall() {
    document.getElementById("btnIndexPriceGo1").click();
}

function keyRestrict(e, validchars) {
    var key = '', keychar = '';
    key = getKeyCode(e);
    if (key == null) return true;
    keychar = String.fromCharCode(key);
    keychar = keychar.toLowerCase();
    validchars = validchars.toLowerCase();
    if (validchars.indexOf(keychar) != -1)
        return true;
    if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
        return true;
    return false;
}
function getKeyCode(e) {
    if (window.event)
        return window.event.keyCode;
    else if (e)
        return e.which;
    else
        return null;
}

function unselectcheckbox(chkelement) {
    var allElts = document.getElementById('divPrice').getElementsByTagName('INPUT');
    var i;
    for (i = 0; i < allElts.length; i++) {
        var elt = allElts[i];
        if (elt.type == "checkbox") {
            if (elt.name != chkelement) {
                elt.checked = false;
            }
        }
    }
    CheckSelection();
}

function unselectcheckboxforCustom(chkelement) {
    var allElts = document.getElementById('divCustom').getElementsByTagName('INPUT');
    var i;
    for (i = 0; i < allElts.length; i++) {
        var elt = allElts[i];
        if (elt.type == "checkbox") {
            if (elt.name != chkelement) {
                elt.checked = false;
            }
        }
    }
    CheckSelection();
}