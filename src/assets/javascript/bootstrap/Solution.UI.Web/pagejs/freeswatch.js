function GatherSwatch2() {
    var allpid = ",";
    var allprice = ",";
    var allqty = ",";
    var k = "";
    var k2 = "";
    var u = 0;


    if (document.getElementById('ContentPlaceHolder1_topMiddle') != null) {
        var allElts = document.getElementById('ContentPlaceHolder1_topMiddle').getElementsByTagName('input');
        for (i = 0; i < allElts.length; i++) {
            var elt = allElts[i];
            if (elt.type == "checkbox") {
                if (elt.checked == true) {
                    u = 1;

                    k = elt.id.replace('ContentPlaceHolder1_Repeaterlistview_add_to_compare_list_', 'ContentPlaceHolder1_Repeaterlistview_hdnYourPrice_');


                    if (document.getElementById(k) != null) {
                        allprice = allprice + document.getElementById(k).value + ",";
                    }

                    k2 = elt.id.replace('ContentPlaceHolder1_Repeaterlistview_add_to_compare_list_', 'ContentPlaceHolder1_Repeaterlistview_hdnProductId_');
                    if (document.getElementById(k2) != null) {
                        allpid = allpid + document.getElementById(k2).value + ",";
                        allqty = allqty + "1" + ",";
                    }


                    // }

                }
            }
        }


        if (document.getElementById('ContentPlaceHolder1_hdnSelectedPID') != null) {
            document.getElementById('ContentPlaceHolder1_hdnSelectedPID').value = allpid;
        }

        if (document.getElementById('ContentPlaceHolder1_hdnSelectedPrice') != null) {
            document.getElementById('ContentPlaceHolder1_hdnSelectedPrice').value = allprice;
        }
        if (document.getElementById('ContentPlaceHolder1_hdnSelectedQty') != null) {
            document.getElementById('ContentPlaceHolder1_hdnSelectedQty').value = allqty;
        }
        if (u == 1) {
            if (document.getElementById('ContentPlaceHolder1_btnAddtocart') != null) {
                document.getElementById('ContentPlaceHolder1_btnAddtocart').style.display = '';

            }
            if (document.getElementById('ContentPlaceHolder1_btnaddtocartbottom') != null) {
                document.getElementById('ContentPlaceHolder1_btnaddtocartbottom').style.display = '';

            }



        }
        else {
            if (document.getElementById('ContentPlaceHolder1_btnAddtocart') != null) {
                document.getElementById('ContentPlaceHolder1_btnAddtocart').style.display = 'none';
            }

            if (document.getElementById('ContentPlaceHolder1_btnaddtocartbottom') != null) {
                document.getElementById('ContentPlaceHolder1_btnaddtocartbottom').style.display = 'none';

            }
        }

    }

}





function GatherSwatch() {
    var allpid = ",";
    var allprice = ",";
    var allqty = ",";
    var k = "";
    var k2 = "";
    var u = 0;


    if (document.getElementById('ContentPlaceHolder1_topMiddle') != null) {
        var allElts = document.getElementById('ContentPlaceHolder1_topMiddle').getElementsByTagName('input');
        for (i = 0; i < allElts.length; i++) {
            var elt = allElts[i];
            if (elt.type == "checkbox") {
                if (elt.checked == true) {

                    k = elt.id.replace('ContentPlaceHolder1_RepProduct_add_to_compare_', 'ContentPlaceHolder1_RepProduct_hdnYourPrice_');
                    u = 1;

                    if (document.getElementById(k) != null) {
                        allprice = allprice + document.getElementById(k).value + ",";
                    }

                    k2 = elt.id.replace('ContentPlaceHolder1_RepProduct_add_to_compare_', 'ContentPlaceHolder1_RepProduct_hdnProductId_');
                    if (document.getElementById(k2) != null) {
                        allpid = allpid + document.getElementById(k2).value + ",";
                        allqty = allqty + "1" + ",";
                    }


                    // }

                }
            }
        }


        //alert(allprice);
        if (document.getElementById('ContentPlaceHolder1_hdnSelectedPID') != null) {
            document.getElementById('ContentPlaceHolder1_hdnSelectedPID').value = allpid;
        }

        if (document.getElementById('ContentPlaceHolder1_hdnSelectedPrice') != null) {
            document.getElementById('ContentPlaceHolder1_hdnSelectedPrice').value = allprice;
        }
        if (document.getElementById('ContentPlaceHolder1_hdnSelectedQty') != null) {
            document.getElementById('ContentPlaceHolder1_hdnSelectedQty').value = allqty;
        }
        if (u == 1) {
            if (document.getElementById('ContentPlaceHolder1_btnAddtocart') != null) {
                document.getElementById('ContentPlaceHolder1_btnAddtocart').style.display = '';

            }
            if (document.getElementById('ContentPlaceHolder1_btnaddtocartbottom') != null) {
                document.getElementById('ContentPlaceHolder1_btnaddtocartbottom').style.display = '';

            }



        }
        else {
            if (document.getElementById('ContentPlaceHolder1_btnAddtocart') != null) {
                document.getElementById('ContentPlaceHolder1_btnAddtocart').style.display = 'none';
            }

            if (document.getElementById('ContentPlaceHolder1_btnaddtocartbottom') != null) {
                document.getElementById('ContentPlaceHolder1_btnaddtocartbottom').style.display = 'none';

            }
        }


    }

}
function ShowSwatchMessage() {

    //document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
    //document.getElementById('frmdisplay').height = '225px';
    //document.getElementById('frmdisplay').width = '565px';
    //document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:225px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    //document.getElementById('popupContact').style.width = '565px';
    //document.getElementById('popupContact').style.height = '225px';
    //window.scrollTo(0, 0);
    ////document.getElementById('acloseinve').onclick = function () { disablePopup(); }


    //$.ajax(
    //            {
    //                type: "POST",
    //                url: "/TestMail.aspx/GeLimitMessage",
    //                data: "{PId: 1}",
    //                contentType: "application/json; charset=utf-8",
    //                dataType: "json",
    //                async: "true",
    //                cache: "false",
    //                success: function (msg) {

    //                    var root = window.location.protocol + '//' + window.location.host;

    //                    document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css?6566" rel="stylesheet" type="text/css" />' + msg.d;
    //                    // $('#diestimatedate').attr('style', 'display:block;');

    //                },
    //                Error: function (x, e) {
    //                }
    //            });

    ////centerPopup();
    ////loadPopup();
    document.getElementById('afreeswatchmsg').click();


}
function ShowInventoryMessageNew(result) {

    // document.getElementById('header-part').style.zIndex = -1;

    //document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
    //document.getElementById('frmdisplay').height = '130px';
    //document.getElementById('frmdisplay').width = '565px';
    //document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:130px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    //document.getElementById('popupContact').style.width = '565px';
    //document.getElementById('popupContact').style.height = '130px';
    //window.scrollTo(0, 0);
    document.getElementById("hdnhtmlall").value = result;
    //$('#ainventorymsg').attr('href', '/inventorymessage.aspx');
    document.getElementById('ainventorymsg').click();

    //var root = window.location.protocol + '//' + window.location.host;
    //document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + result;
    //centerPopup();



    //loadPopup();

}
function urlencode(str) {
    return escape(str).replace('+', '%2B').replace('*', '%2A').replace('/', '%2F').replace('@', '%40');
}
function ShowInventoryMessage(result) {

    // document.getElementById('header-part').style.zIndex = -1;

    //document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
    //document.getElementById('frmdisplay').height = '130px';
    //document.getElementById('frmdisplay').width = '565px';
    //document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:130px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    //document.getElementById('popupContact').style.width = '565px';
    //document.getElementById('popupContact').style.height = '130px';
    //window.scrollTo(0, 0);


    //var root = window.location.protocol + '//' + window.location.host;
    //document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + result;
    //centerPopup();

    //loadPopup();

    //$('#ainventorymsg').removeAttr('href');
    document.getElementById("hdnhtmlall").value = result;
    //$('#ainventorymsg').attr('href', '/inventorymessage.aspx');
    document.getElementById('ainventorymsg').click();



}
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
            document.getElementById("ContentPlaceHolder1_hdnallpages").value = '2';
            return true;
        }
    }
    document.getElementById("ContentPlaceHolder1_hdnallpages").value = '2';
    return true;
}

function CheckSelection() {
    document.getElementById("ContentPlaceHolder1_hdnallpages").value = '1';
    document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();

}

function ColorSelection(clrvalue) {


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
    //  alert(document.getElementById(id));
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

    var productcount = document.getElementById('ContentPlaceHolder1_productcount');
    var totalproductstodisplay = productcount.value;

    if (totalproductstodisplay == 0) { return; }
    var checkdiv = document.getElementById('ContentPlaceHolder1_divcount');
    var temp = document.getElementById('ContentPlaceHolder1_hdncnt');
    var Take; var Skip;
    Skip = parseInt(temp.value) + 1;
    Take = parseInt(temp.value) + 12;
    if (totalproductstodisplay < Skip) { return; }
    $('#divPostsLoader').html('<img src="/images/220.gif">');
    var catid = document.getElementById('hdncatids').value;

    var view = document.getElementById('view1');
    var v = view.value;
    var ddlsortby = document.getElementById('ContentPlaceHolder1_ddlTopPrice').selectedIndex;

    var divcount = document.getElementById('ContentPlaceHolder1_divcount');
    var newcount; newcount = parseInt(divcount.value) + 1;
    if (totalproductstodisplay <= Take && flagin == 0) {
        // alert('last');
        $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).load("/scrolling.aspx?catid=" + catid + "&skip=" + Skip + "&take=" + Take + "&view=" + v + "&sortby=" + ddlsortby, function (response, status, xhr) {
            if (status == "error") {
                var msg = "Sorry but there was an error: ";
                $("#error").html(msg + xhr.status + " " + xhr.statusText);
            }
            if (status == "success") { $('#divPostsLoader').empty(); }
        });
        // $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).after("<div id='ContentPlaceHolder1_Div" + newcount + "' class='fp-main-bg-scroll'></div>");

        flagin = 1;
    }
    else {
        $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).load("/scrolling.aspx?catid=" + catid + "&skip=" + Skip + "&take=" + Take + "&view=" + v + "&sortby=" + ddlsortby, function (response, status, xhr) {
            if (status == "error") {
                var msg = "Sorry but there was an error: ";
                $("#error").html(msg + xhr.status + " " + xhr.statusText);
            }
            if (status == "success") { $('#divPostsLoader').empty(); }
        });
        $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).after("<div id='ContentPlaceHolder1_Div" + newcount + "' class='fp-main-bg-scroll'></div>");
    }
    var newdivcount = document.getElementById('ContentPlaceHolder1_divcount');
    newdivcount.value = newcount;
    var e = document.getElementById('ContentPlaceHolder1_hdncnt');
    e.value = Take;
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

function newPopup(url) {
    popupWindow = window.open(url, 'popUpWindow', 'height=900,width=1024,top=10,resizable=no,scrollbars=yes,toolbar=no,menubar=no,location=no,directories=no,status=yes')
}
function DeleteProductData(i, id) {

    if (i == -1 && id == -1) {


        var allElts = document.getElementById('ContentPlaceHolder1_topMiddle').getElementsByTagName('input');
        for (i = 0; i < allElts.length; i++) {
            var elt = allElts[i];
            if (elt.type == "checkbox") {
                if (elt.checked == true) {
                    elt.checked = false;
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