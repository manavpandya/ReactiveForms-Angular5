function IndexValidation() {
    if (document.getElementById("ContentPlaceHolder1_txtFrom") != null && document.getElementById("ContentPlaceHolder1_txtFrom").value == "") {
        alert("Please enter from price.");
        document.getElementById("ContentPlaceHolder1_txtFrom").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtTo") != null && document.getElementById("ContentPlaceHolder1_txtTo").value == "") {
        alert("Please enter to price.");
        document.getElementById("ContentPlaceHolder1_txtTo").focus();
        return false;
    }
    else {
        var fromPrice = parseFloat(document.getElementById("ContentPlaceHolder1_txtFrom").value);
        var toPrice = parseFloat(document.getElementById("ContentPlaceHolder1_txtTo").value);
        if (parseFloat(fromPrice) > parseFloat(toPrice)) {
            alert("Please enter valid price range.");
            document.getElementById("ContentPlaceHolder1_txtFrom").focus();
            return false;
        }
        else {
            return true;
        }
    }
    return true;
}


function CheckSelection() {
    document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
}

function ColorSelection(clrvalue) {
    document.getElementById("ContentPlaceHolder1_hdnColorSelection").value = clrvalue;

    $.ajax(
                {
                    type: "POST",
                    url: "/TestMail.aspx/GetUrl",
                    data: "{Colorname: '" + clrvalue + "',Patternname: '' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: "true",
                    cache: "false",
                    success: function (msg) {
                        window.location.href = msg.d;
                    },
                    Error: function (x, e) {
                    }
                });



}
function CheckpatternSelection(clrvalue) {
    document.getElementById("ContentPlaceHolder1_hdnpattern").value = clrvalue;
    $.ajax(
               {
                   type: "POST",
                   url: "/TestMail.aspx/GetUrl",
                   data: "{Colorname: '',Patternname: '" + clrvalue + "' }",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   async: "true",
                   cache: "false",
                   success: function (msg) {
                       window.location.href = msg.d;
                   },
                   Error: function (x, e) {
                   }
               });

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

function ShowModelQuick(id) {

    document.getElementById('header').style.zIndex = -1;
    document.getElementById("frmquickview").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmquickview').height = '425px';
    document.getElementById('frmquickview').width = '750px';
    document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:750px;height:425px;border:solid 1px #7d7d7d;");
    document.getElementById('popupContact').style.width = '750px';
    document.getElementById('popupContact').style.height = '425px';
    window.scrollTo(0, 0);
    document.getElementById('btnreadmore').click();
    document.getElementById('frmquickview').src = '/QuickView.aspx?PID=' + id;

}
function adtocart(price, id) {
    if (document.getElementById('prepage') != null) {
        document.getElementById('prepage').style.display = '';
    }
    document.getElementById("ContentPlaceHolder1_hdnPrice").value = price;
    document.getElementById("ContentPlaceHolder1_hdnproductId").value = id;
    document.getElementById("ContentPlaceHolder1_btnAddtocart").click();
}