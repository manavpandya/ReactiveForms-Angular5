function ShowSwatchMessage() {

    var iframe = document.createElement('iframe');
    iframe.frameBorder = 0;
    iframe.width = "565px";
    iframe.height = "225px";
    iframe.id = "frmsearchswatch";
    iframe.scrolling = "no";
    iframe.frameborder = "0"

    document.getElementById("diviframesearch").innerHTML = '';
    document.getElementById("diviframesearch").appendChild(iframe);
    document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:225px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    document.getElementById('popupContactpricequote1').style.width = '565px';
    document.getElementById('popupContactpricequote1').style.height = '225px';
    window.scrollTo(0, 0);


    $.ajax(
                {
                    type: "POST",
                    url: "/TestMail.aspx/GeLimitMessage",
                    data: "{PId: 1}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: "true",
                    cache: "false",
                    success: function (msg) {

                        var root = window.location.protocol + '//' + window.location.host;

                        iframe.contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css?5656" rel="stylesheet" type="text/css" />' + msg.d.replace('disablePopup();', 'disablePopupmaster();');


                    },
                    Error: function (x, e) {
                    }
                });
    document.getElementById("diviframesearch").style.display = 'block';
    centerPopupmaster(); loadPopupmaster();


}
function ShowInventoryMessage(result) {

    document.getElementById("diviframesearch").innerHTML = '';


    document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:130px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    document.getElementById('popupContactpricequote1').style.width = '565px';
    document.getElementById('popupContactpricequote1').style.height = '130px';
    window.scrollTo(0, 0);

    var root = window.location.protocol + '//' + window.location.host;

    document.getElementById("diviframesearch").style.width = '565px';
    document.getElementById("diviframesearch").style.height = '130px';
    document.getElementById("diviframesearch").style.backgroundColor = '#ffffff';
    document.getElementById("diviframesearch").innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + result.replace('100%', '565px').replace('class="description_box_border"', 'class="description_box_border" style="color:#000;" ');
    document.getElementById("diviframesearch").style.display = 'block';

    centerPopupmaster(); loadPopupmaster();


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
    var catid = document.getElementById("ContentPlaceHolder1_hdncatid").value;
    var view = document.getElementById('view1');
    var v = view.value;
    var ddlsortby = document.getElementById('ContentPlaceHolder1_ddlTopPrice').selectedIndex;

    var divcount = document.getElementById('ContentPlaceHolder1_divcount');
    var newcount; newcount = parseInt(divcount.value) + 1;
    if (totalproductstodisplay <= Take && flagin == 0) {

        $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).load("/scrolling.aspx?catid=" + catid + "&skip=" + Skip + "&take=" + Take + "&view=" + v + "&sortby=" + ddlsortby, function (response, status, xhr) {
            if (status == "error") {
                var msg = "Sorry but there was an error: ";
                $("#error").html(msg + xhr.status + " " + xhr.statusText);
            }
            if (status == "success") { $('#divPostsLoader').empty(); }
        });


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
