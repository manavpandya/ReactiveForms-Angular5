function ppClose() {
    $("#fancybox-skin").fadeOut();
    $(".fancybox-overlay").fadeOut();
}


function hideshowdiv(id, id2) {
    for (var i = 1; i < 90; i++) {
        if (document.getElementById('divsubcat' + i) != null && i != id) {
            document.getElementById('divsubcat' + i).style.display = 'none';
        }
        else if (document.getElementById('divsubcat' + i) != null && i == id) {
            document.getElementById('divsubcat' + i).style.display = '';
        }
    }

    var alllianchor = document.getElementById('menu_detail' + id2).getElementsByTagName('a');

    for (var k = 0; k < alllianchor.length; k++) {
        var alllianchor1 = alllianchor[k];
        alllianchor1.removeAttribute("style");
        alllianchor1.setAttribute('style', 'color:#000000');
    }

    var allli = document.getElementById('menu_detail' + id2).getElementsByTagName('li');
    for (var j = 0; j < allli.length; j++) {
        if (allli[j].id.toString() == 'lisub' + id.toString()) {
            var litag = allli[j];
            litag.className = 'liselectedhover';
            var alll = litag.getElementsByTagName('a');
            var tttt = alll[0];

            tttt.style.color = '#ffffff';

        }
        else {
            var litag = allli[j];
            litag.removeAttribute("class");
            litag.style.backgroundColor = '#fff';
            litag.className = '';
            var alll = litag.getElementsByTagName('a');
            var tttt = alll[0];
            tttt.style.color = '#000000 !important;';
            tttt.style.backgroundColor = '#fff';

        }
    }
}


function ShowModelSearch() {

    document.getElementById("diviframecontactus").style.display = 'none';
    document.getElementById("diviframesearch").style.display = 'block';

    if (document.getElementById("diviframesearch") != null) { document.getElementById("diviframesearch").innerHTML = ''; }
    var link = "/SearchControl.aspx"
    var iframe = document.createElement('iframe');
    iframe.frameBorder = 0;
    iframe.width = "730px";
    iframe.height = "560px";
    iframe.id = "frmsearch";
    iframe.scrolling = "no";
    iframe.frameborder = "0"

    document.getElementById("diviframesearch").appendChild(iframe);
    document.getElementById("diviframesearch").style.display = 'block';
    document.getElementById("diviframecontactus").style.display = 'none';
    iframe.src = "/SearchControl.aspx";


    document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:730px;height:560px;border:solid 1px #7d7d7d;");
    document.getElementById('popupContactpricequote1').style.width = '730px';
    document.getElementById('popupContactpricequote1').style.height = '560px';



    window.scrollTo(0, 0);
    centerPopupmaster(); loadPopupmaster();



}

function ShowModelSearchContactus() {


    if (document.getElementById("diviframecontactus") != null) { $('#diviframecontactus').html(''); }
    var link = "/Contactuspopup.aspx"
    var iframe = document.createElement('iframe');
    iframe.frameBorder = 0;
    iframe.width = "1440px";
    iframe.height = "296px";
    iframe.id = "frmContactus";
    iframe.scrolling = "no";
    iframe.frameborder = "0"

    document.getElementById("diviframecontactus").appendChild(iframe);
    document.getElementById("diviframecontactus").style.display = 'block';
    document.getElementById("diviframesearch").style.display = 'none';



    document.getElementById('popupContactpricequote1').removeAttribute("style");
    document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 2000001; top: 0px; padding: 0px;width:600px;height:680px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    document.getElementById('popupContactpricequote1').style.width = '600px';
    document.getElementById('popupContactpricequote1').style.height = '680px';
    document.getElementById('frmContactus').height = '680px';
    document.getElementById('frmContactus').width = '625px';
    document.getElementById("frmContactus").contentWindow.document.body.innerHTML = '';
    document.getElementById("frmContactus").contentWindow.document.location.href = "/Contactuspopup.aspx";




    window.scrollTo(0, 0);
    centerPopupmaster(); loadPopupmaster();



}


function showfreeshipping() {

    var iframe = document.createElement('iframe');
    iframe.frameBorder = 0;
    iframe.width = "1440px";
    iframe.height = "296px";
    iframe.id = "frmfreeshipp";
    iframe.scrolling = "auto";
    iframe.frameborder = "0"


    if (document.getElementById("diviframesearch") != null) { $("#diviframesearch").html(''); }
    document.getElementById("diviframesearch").appendChild(iframe);

    document.getElementById("diviframesearch").style.display = 'block';
    document.getElementById("diviframecontactus").style.display = 'none';


    document.getElementById('popupContactpricequote1').removeAttribute("style");
    document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 2000001; top: 0px; padding: 0px;width:800px;height:680px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    document.getElementById('popupContactpricequote1').style.width = '800px';
    document.getElementById('popupContactpricequote1').style.height = '680px';
    document.getElementById('frmfreeshipp').height = '680px';
    document.getElementById('frmfreeshipp').width = '800px';
    document.getElementById("frmfreeshipp").contentWindow.document.open();
    document.getElementById("frmfreeshipp").contentWindow.document.close();
    document.getElementById("frmfreeshipp").contentWindow.document.body.innerHTML = '';
    document.getElementById("frmfreeshipp").contentWindow.document.body.innerHTML = '<link href="/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('divfreehtml').innerHTML;

    window.scrollTo(0, 0);
    centerPopupmaster(); loadPopupmaster();


}


function showtradeshipping() {


    if (document.getElementById("diviframesearch") != null) { $("#diviframesearch").html(''); }
    var iframe = document.createElement('iframe');
    iframe.frameBorder = 0;
    iframe.width = "600px";
    iframe.height = "680px";
    iframe.id = "frmtrade";
    iframe.scrolling = "auto";
    iframe.frameborder = "0"

    document.getElementById("diviframesearch").appendChild(iframe);

    document.getElementById("diviframesearch").style.display = 'block';
    document.getElementById("diviframecontactus").style.display = 'none';


    document.getElementById('popupContactpricequote1').removeAttribute("style");
    document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 2000001; top: 0px; padding: 0px;width:600px;height:680px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");


    document.getElementById('popupContactpricequote1').style.width = '600px';
    document.getElementById('popupContactpricequote1').style.height = '680px';
    document.getElementById('frmtrade').height = '680px';
    document.getElementById('frmtrade').width = '600px';



    document.getElementById("frmtrade").contentWindow.document.open();
    document.getElementById("frmtrade").contentWindow.document.close();
    document.getElementById("frmtrade").contentWindow.document.body.innerHTML = '';
    document.getElementById("frmtrade").contentWindow.document.body.innerHTML = '<link href="/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('divtradehtml').innerHTML;
    window.scrollTo(0, 0);
    centerPopupmaster(); loadPopupmaster();
}

function tabheaderhide(id, id1) {

    if (document.getElementById(id) != null) {

        document.getElementById(id).className = 'active';
    }
    if (document.getElementById(id1) != null) {

        document.getElementById(id1).className = '';
    }

}
function OpenCenterWindow(pid, wi, he) {
    var w = wi;
    var h = he;
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}

function ChkInventoryUpdate(result) {
    //if (document.getElementById('divInventoryCart') != null) {
    //    document.getElementById('divInventoryCart').innerHTML = result;
    //    document.getElementById('divInventoryCart').style.display = '';
    //}
    document.getElementById("hdnhtmlall").value = result;
    //$('#ainventorymsg').attr('href', '/inventorymessage.aspx');
    document.getElementById('ainventorymsg').click();
}


function setupRotator() {
    setInterval('textRotate()', 5000);
}


function textRotate() {

    var irota = document.getElementById('hdnrotator').value;
    var alltext = document.getElementById('rotatebannertextall').getElementsByTagName('div');

    if ((parseInt(irota) + 1) > alltext.length) {
        $('#rotatebannertext').fadeOut();
        irota = 0;
        if (document.getElementById('divheder' + irota.toString()) != null) {
            document.getElementById('rotatebannertext').innerHTML = document.getElementById('divheder' + irota.toString()).innerHTML;
            $('#rotatebannertext').fadeIn();
        }
        irota = parseInt(irota) + 1;
        document.getElementById('hdnrotator').value = irota;

    }
    else {
        $('#rotatebannertext').fadeOut();

        if (document.getElementById('divheder' + irota.toString()) != null) {


            document.getElementById('rotatebannertext').innerHTML = document.getElementById('divheder' + irota.toString()).innerHTML;
            $('#rotatebannertext').fadeIn();
        }

        irota = parseInt(irota) + 1;
        document.getElementById('hdnrotator').value = irota;


    }

}


(function () {
    var bs = document.createElement('script');
    bs.type = 'text/javascript';
    bs.async = true;
    bs.src = ('https:' == document.location.protocol ? 'https' : 'http') + '://d2so4705rl485y.cloudfront.net/widgets/tracker/tracker.js';
    var s = document.getElementsByTagName('script')[0];
    s.parentNode.insertBefore(bs, s);
})();


(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

ga('create', 'UA-2756708-1', 'halfpricedrapes.com');
ga('require', 'displayfeatures');
ga('send', 'pageview');
