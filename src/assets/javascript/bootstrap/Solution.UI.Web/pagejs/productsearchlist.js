function resetcheckvalue(valid) {
    if (document.getElementById("prepage1") != null) {
        document.getElementById("prepage1").style.display = '';

    }
    document.getElementById('ContentPlaceHolder1_hdnallpages').value = '11';

    document.getElementById('ContentPlaceHolder1_hdnsearchvalue').value = valid; __doPostBack('ctl00$ContentPlaceHolder1$lnkresetdata', '');
}
function IndexValidation() {
    if ((document.getElementById('ContentPlaceHolder1_txtFrom').value).replace(/^\s*\s*$/g, '') != '' || (document.getElementById('ContentPlaceHolder1_txtTo').value).replace(/^\s*\s*$/g, '') != '') {
        if ((document.getElementById('ContentPlaceHolder1_txtFrom').value).replace(/^\s*\s*$/g, '') == '') {
            alert('Please Enter Valid Price.');
            document.getElementById('ContentPlaceHolder1_txtFrom').focus();
            return false;
        }
        if ((document.getElementById('ContentPlaceHolder1_txtTo').value).replace(/^\s*\s*$/g, '') == '') {
            alert('Please Enter Valid Price.');
            document.getElementById('ContentPlaceHolder1_txtTo').focus();
            return false;
        }
        var FromPrice = parseFloat((document.getElementById('ContentPlaceHolder1_txtFrom').value)).toFixed(2);
        var ToPrice = parseFloat((document.getElementById('ContentPlaceHolder1_txtTo').value)).toFixed(2);
        if (parseFloat(ToPrice) < parseFloat(FromPrice)) {
            alert('Low Price should be Less than High Price.');
            document.getElementById('ContentPlaceHolder1_txtFrom').focus();
            return false;
        }
    }
    document.getElementById("ContentPlaceHolder1_hdnallpages").value = '2';
    return true;
}
function checkPricevalidation() {

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
function checkallpage() {
    $(".toggle1").slideToggle('1100');

    if (document.getElementById("option-arrow") != null && document.getElementById("option-arrow").src.toString().toLowerCase().indexOf('option-arrow-up.png') > -1) {
        document.getElementById("option-arrow").src = '/images/option-arrow-down.png';



    }
    else if (document.getElementById("option-arrow") != null && document.getElementById("option-arrow").src.toString().toLowerCase().indexOf('option-arrow-down.png') > -1) {
        document.getElementById("option-arrow").src = '/images/option-arrow-up.png';
    }

}
function CheckSelection() {
    if (document.getElementById("prepage1") != null) {
        document.getElementById("prepage1").style.display = '';
    }
    document.getElementById("ContentPlaceHolder1_hdnallpages").value = '1';
    document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
}

function ColorSelection(clrvalue) {
    if (document.getElementById("prepage1") != null) {
        document.getElementById("prepage1").style.display = '';
    }
    document.getElementById("ContentPlaceHolder1_hdnallpages").value = '1';
    document.getElementById("ContentPlaceHolder1_hdnColorSelection").value = clrvalue;
    document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
}
function pagingSelection(pageval) {


    document.getElementById("ContentPlaceHolder1_hdnallpages").value = '8';
    document.getElementById("ContentPlaceHolder1_hdnpagenumber").value = pageval;
    if (document.getElementById("ContentPlaceHolder1_hdnquickview").value == '1') {
        document.getElementById('form1').submit();
    }
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

function Checkselectcheckbox(ChkValue) {
    var allElts = document.getElementById('divPrice').getElementsByTagName('INPUT');
    var i;
    for (i = 0; i < allElts.length; i++) {
        var elt = allElts[i];
        if (elt.type == "checkbox") {
            if (elt.value == ChkValue) {
                elt.checked = true;
            }
        }
    }
}


function CheckSelectedCustomValue(ChkValue) {
    var allElts = document.getElementById('divCustom').getElementsByTagName('INPUT');
    var i;
    for (i = 0; i < allElts.length; i++) {
        var elt = allElts[i];
        if (elt.type == "checkbox") {
            if (elt.value == ChkValue) {
                elt.checked = true;
            }
        }
    }
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

//for newsletter
function ValidNewSletter() {
    var element; if (document.getElementById('txtSubscriber'))
    { element = document.getElementById('txtSubscriber'); }
    if (document.getElementById('txtSubscriber'))
    { element = document.getElementById('txtSubscriber'); }
    if (element.value == '') {
        alert('Please enter your E-mail Address.'); if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        return false;
    }
    else if (element.value == 'Enter your E-Mail Address') {
        alert('Please enter your E-Mail Address.'); if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        return false;
    }
    else {
        var testresults; var str = element.value; var filter = /^.+@.+\..{2,3}$/; if (filter.test(str))
        { return true; }
        else {
            alert("Please enter valid E-Mail Address.")
            if (document.getElementById('txtSubscriber'))
            { document.getElementById('txtSubscriber').focus(); }
            if (document.getElementById('txtSubscriber'))
            { document.getElementById('txtSubscriber').focus(); }
            return false;
        }
    }
}
function clear_text() {
    if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value == "Enter your E-Mail Address")
    { document.getElementById('txtSubscriber').value = ""; }
    if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value == "Enter your E-Mail Address")
    { document.getElementById('txtSubscriber').value = ""; }
    return false;
}
function clear_NewsLetter(myControl) {
    if (myControl && myControl.value == "Enter your E-Mail Address")
        myControl.value = "";
}
function ChangeNewsLetter(myControl) {
    if (myControl != null && myControl.value == '')
        myControl.value = "Enter your E-Mail Address";
}
function Change() {
    if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
        document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
            document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; return false;
}


//For Search
function clear_Search(myControl) {
    if (myControl && myControl.value == "Search by Keyword")
        myControl.value = "";
}
function ChangeSearch(myControl) {
    if (myControl != null && myControl.value == '')
        myControl.value = "Search by Keyword";
}


function ValidSearch() {
    var myControl;

    if (document.getElementById('txtSearch')) {
        myControl = document.getElementById('txtSearch');
    }



    if (myControl.value == '' || myControl.value == 'Search by Keyword') {
        alert("Please enter something to search");

        if (document.getElementById('txtSearch')) {
            document.getElementById('txtSearch').focus();
        }


        return false;
    }

    if (myControl.value.length < 3) {
        alert("Please enter at least 3 characters to search");
        myControl.focus();
        return false;
    }
    return true;
}


var pc = 0;
var ProductId = ',';
$(document).ready(function () {
    $("#ContentPlaceHolder1_grid_view").attr('class', 'grid-click');
    $("#ContentPlaceHolder1_grid_bottom").attr('class', 'grid-click');
});
function DeleteProductData(i, id) {


    if (i == -1 && id == -1) {

        if (document.getElementById('ContentPlaceHolder1_topMiddle') != null) {
            var allElts = document.getElementById('ContentPlaceHolder1_topMiddle').getElementsByTagName('input');
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        elt.checked = false;
                    }
                }
            }
        }


        compare(-1, -1);
    }
    else {
        var chkProduct = 'add_to_compare' + id.toString();
        if (document.getElementById(chkProduct) != null) {
            document.getElementById(chkProduct).checked = false;
        }
        chkProduct = 'add_to_compare_list' + id.toString();
        if (document.getElementById(chkProduct) != null) {
            document.getElementById(chkProduct).checked = false;
        }
        compare(id, 0);

    }

}
function comaprebutonclicklist(id) {

    var pid = 0;
    var cid = 0;
    if (document.getElementById(id) != null && document.getElementById(id).checked == true) {
        pid = id.replace('add_to_compare_list', '');
        cid = 1;
        compare(pid, cid);
    }
    else {
        pid = id.replace('add_to_compare_list', '');
        cid = 0;
        compare(pid, cid);

    }
    $('html, body').animate({ scrollTop: 0 }, 'slow');
}

function comaprebutonclick(id) {

    var pid = 0;
    var cid = 0;

    if (document.getElementById(id) != null && document.getElementById(id).checked == true) {
        pid = id.replace('add_to_compare', '');
        cid = 1;
        compare(pid, cid);
    }
    else {

        pid = id.replace('add_to_compare', '');
        cid = 0;
        compare(pid, cid);
    }

    $('html, body').animate({ scrollTop: 0 }, 'slow');
}

function compare(pid, chkstatus) {



    if (pid != -1 && chkstatus != -1) {
        var numItems = $('.comparison-box').length;

        if (numItems > 4 && chkstatus != 0) {

            alert('You can compare max. 5 product');

            if (document.getElementById('view1').value == "grid") {
                $("input:checkbox[id= add_to_compare" + pid + "]").attr("checked", false);
            }
            if (document.getElementById('view1').value == "list") {
                $("input:checkbox[id= add_to_compare_list" + pid + "]").attr("checked", false);
            }
            $('html, body').animate({ scrollTop: 0 }, 'slow');

            return;

        }
    }


    var requestUrl = "/comparetable.aspx?cid=" + chkstatus + "&pid=" + pid;

    $.ajax({
        type: "POST",
        url: "/comparetable.aspx?cid=" + chkstatus + "&pid=" + pid + "",
        data: "",
        dataType: "html",
        async: "false",
        cache: "false",
        success: function (msg) {
            $('#co').html(msg);
        },
        Error: function (x, e) {
            alert("Sorry but there was an error");
        }
    });


    if (document.getElementById('view1').value == "grid") {
        if (chkstatus == 0 && pid != 0) { $("input:checkbox[id= add_to_compare" + pid + "]").attr("checked", false); }
    }
    if (document.getElementById('view1').value == "list") {
        if (chkstatus == 0 && pid != 0) { $("input:checkbox[id= add_to_compare_list" + pid + "]").attr("checked", false); }
    }
}
//   $(document).ready(function () {
var flagin = 0;

function changeview() {

    var view = document.getElementById('view1');
    var vie = view.value;
    if (vie == 'grid') {
        var change = document.getElementById('view1');
        change.value = 'list';
    } else if (vie == 'list') {
        var change1 = document.getElementById('view1');
        change1.value = 'grid';
    }
}
function Load() {


};

$(window).scroll(function () {

    if ($(window).scrollTop() == $(document).height() - $(window).height()) {

    }
});


function ShowModelQuick(id, pcnt, pcnt1) {
    disablePopup();

    document.getElementById('frmdisplayquick').height = '500px';
    document.getElementById('frmdisplayquick').width = '1000px';
    document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:1000px;height:500px;position:absolute;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact').style.width = '1000px';
    document.getElementById('popupContact').style.height = '500px';
    //            window.scrollTo(0, 0);

    document.getElementById('ContentPlaceHolder1_hdnquickview').value = '1';
    var prev1 = parseInt(pcnt) - parseInt(1);
    var iVarnameprev = 'ContentPlaceHolder1_RepProduct_hdnProductId_' + prev1;
    var next1 = parseInt(pcnt) + parseInt(1);
    var iVarnamenext = 'ContentPlaceHolder1_RepProduct_hdnProductId_' + next1;




    var iVarnameprev1 = 'ContentPlaceHolder1_Repeaterlistview_hdnProductId_' + prev1;

    var iVarnamenext1 = 'ContentPlaceHolder1_Repeaterlistview_hdnProductId_' + next1;


    if (document.getElementById(iVarnameprev) != null) {

        if (document.getElementById(iVarnamenext) != null) {
            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext).value.toString() + '&prev=' + document.getElementById(iVarnameprev).value.toString() + '&pcnt=' + pcnt;
        }
        else {
            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=0&prev=' + document.getElementById(iVarnameprev).value.toString() + '&pcnt=' + pcnt;
        }
    }
    else if (document.getElementById(iVarnamenext) != null) {
        if (document.getElementById(iVarnameprev) != null) {
            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext).value.toString() + '&prev=' + document.getElementById(iVarnameprev).value.toString() + '&pcnt=' + pcnt;
        }
        else {
            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext).value.toString() + '&prev=0&pcnt=' + pcnt;
        }

    }
    else if (document.getElementById(iVarnameprev1) != null) {

        if (document.getElementById(iVarnamenext1) != null) {
            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext1).value.toString() + '&prev=' + document.getElementById(iVarnameprev1).value.toString() + '&pcnt=' + pcnt;
        }
        else {
            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=0&prev=' + document.getElementById(iVarnameprev1).value.toString() + '&pcnt=' + pcnt;
        }
    }
    else if (document.getElementById(iVarnamenext1) != null) {
        if (document.getElementById(iVarnameprev1) != null) {
            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext1).value.toString() + '&prev=' + document.getElementById(iVarnameprev1).value.toString() + '&pcnt=' + pcnt;
        }
        else {
            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext1).value.toString() + '&prev=0&pcnt=' + pcnt;
        }

    }
    else {
        document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=0&prev=0&pcnt=' + pcnt;
    }


    centerPopup();
    loadPopup();


}

function Loader() {
    //   reset values for scrolling
    flagin = 0; var rese = document.getElementById('ContentPlaceHolder1_hdncnt');
    rese.value = '12';
    var setdiv = document.getElementById('ContentPlaceHolder1_divcount');
    setdiv.value = '1';
    //
    if (document.getElementById('prepage') != null) {
        var windowHeight = 0;
        windowHeight = $(document).height(); //window.innerHeight;

        document.getElementById('prepage').style.height = windowHeight + 'px';
        document.getElementById('prepage').style.display = '';
    }
}
function adtocart(price, id) {
    if (document.getElementById('prepage') != null) {
        var windowHeight = 0;
        windowHeight = $(document).height(); //window.innerHeight;

        document.getElementById('prepage').style.height = windowHeight + 'px';
        document.getElementById('prepage').style.display = '';
    }
    document.getElementById("ContentPlaceHolder1_hdnPrice").value = price;
    document.getElementById("ContentPlaceHolder1_hdnproductId").value = id;

}

function chkHeight() {

    if (document.getElementById('prepage')) {
        var windowHeight = 0;
        windowHeight = $(document).height(); //window.innerHeight;

        document.getElementById('prepage').style.height = windowHeight + 'px';
        document.getElementById('prepage').style.display = '';
    }
}