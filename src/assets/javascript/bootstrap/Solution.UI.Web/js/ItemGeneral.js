function ReviewCount(RevId, mode) {
    if (RevId > 0) {
        if (document.getElementById('ContentPlaceHolder1_hdnReviewType') != null) {
            document.getElementById('ContentPlaceHolder1_hdnReviewType').value = mode;
            document.getElementById('ContentPlaceHolder1_hdnReviewId').value = RevId;
            document.getElementById('ContentPlaceHolder1_btnReviewCount').click();
        }
    }
}

//        function displayall() {
//            if (document.getElementById('divtaball') != null) {
//                document.getElementById('divtaball').style.display = '';
//            }
//            if (document.getElementById('divshippingtab') != null) {
//                document.getElementById('divshippingtab').style.display = '';
//            }
//        }
//        window.onload = displayall;
function tabdisplaycartpolicy(id, id1) {

    if (document.getElementById('lishippingtime') != null) {
        document.getElementById('lishippingtime').className = '';
    }
    if (document.getElementById('divshippingtime') != null) {
        document.getElementById('divshippingtime').style.display = 'none';
    }

    if (document.getElementById('lireturnpolicy') != null) {
        document.getElementById('lireturnpolicy').className = '';
    }
    if (document.getElementById('divreturnpolicy') != null) {
        document.getElementById('divreturnpolicy').style.display = 'none';
    }


    if (document.getElementById(id) != null) {
        document.getElementById(id).className = 'tabberactive';
    }
    if (document.getElementById(id1) != null) {
        document.getElementById(id1).style.display = '';
    }



}
function tabdisplaycart(id, id1) {

    if (document.getElementById('ContentPlaceHolder1_liswatch') != null) {
        document.getElementById('ContentPlaceHolder1_liswatch').className = '';
    }
    if (document.getElementById('ContentPlaceHolder1_divswatch') != null) {
        document.getElementById('ContentPlaceHolder1_divswatch').style.display = 'none';
    }

    if (document.getElementById('ContentPlaceHolder1_licustom') != null) {
        document.getElementById('ContentPlaceHolder1_licustom').className = '';
    }
    if (document.getElementById('ContentPlaceHolder1_divcustom') != null) {
        document.getElementById('ContentPlaceHolder1_divcustom').style.display = 'none';
    }

    if (document.getElementById('ContentPlaceHolder1_liready') != null) {
        document.getElementById('ContentPlaceHolder1_liready').className = '';
    }
    if (document.getElementById('ContentPlaceHolder1_divready') != null) {
        document.getElementById('ContentPlaceHolder1_divready').style.display = 'none';
    }

    if (document.getElementById(id) != null) {
        document.getElementById(id).className = 'tabberactive';
    }
    if (document.getElementById(id1) != null) {
        document.getElementById(id1).style.display = '';
    }



}
//        var $j = jQuery.noConflict();
//        $j(document).ready(function ($) {
//            var imagename = '';
//            if (document.getElementById('ContentPlaceHolder1_imgMain')) {
//                imagename = document.getElementById('ContentPlaceHolder1_imgMain').src;
//            }
//            imagename = imagename.replace('medium', 'large');


//            $('#ContentPlaceHolder1_imgMain').addimagezoom({
//                zoomrange: [3, 10],
//                magnifiersize: [300, 300],
//                magnifierpos: 'right',
//                cursorshade: true,
//                largeimage: imagename //<-- No comma after last option!
//            })
////             $("#Button2").click(function () {
////                 var imagename = document.getElementById('imgMain').src;
////                 imagename = imagename.replace('medium', 'large');
////                 $('#imgMain').addimagezoom({
////                     zoomrange: [3, 10],
////                     magnifiersize: [300, 300],
////                     magnifierpos: 'right',
////                     cursorshade: true,
////                     largeimage: imagename
////                 })
////             }) 


//        })
// 






var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_beginRequest(beginRequest);

function beginRequest() {
    prm._scrollPosition = null;
}

function ShowModelHelpShipping(id) {
    //document.getElementById('header-part').style.zIndex = -1;
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '730px';
    document.getElementById('frmdisplay1').width = '620px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:620px;height:730px;");
    document.getElementById('popupContact1').style.width = '620px';
    document.getElementById('popupContact1').style.height = '730px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    document.getElementById('frmdisplay1').src = id;
}

function PriceChangeondropdown() {
    var price = document.getElementById('ContentPlaceHolder1_hdnActual').value;
    var saleprice = document.getElementById('ContentPlaceHolder1_hdnprice').value;
    if (parseFloat(saleprice) == parseFloat(0)) {
        saleprice = price;
    }
    var vprice = 0;
    if (document.getElementById('divVariant')) {
        var allselect = document.getElementById('divVariant').getElementsByTagName('select');

        for (var iS = 0; iS < allselect.length; iS++) {
            var eltSelect = allselect[iS];
            if (eltSelect.selectedIndex != 0) {
                if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('(+$') > -1) {

                    var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('(+$') + 3);
                    vtemp = vtemp.replace(/\)/g, '');
                    vprice = parseFloat(vprice) + parseFloat(vtemp);
                }
            }
        }
    }
    saleprice = parseFloat(saleprice) + parseFloat(vprice);
    price = parseFloat(price) + parseFloat(vprice);
    if (document.getElementById('ContentPlaceHolder1_divRegularPrice') != null) {
        document.getElementById('ContentPlaceHolder1_divRegularPrice').innerHTML = '<tt>Regular Starting Price :</tt> <span>$' + price.toFixed(2) + '</span>';
    }
    if (document.getElementById('ContentPlaceHolder1_divYourPrice') != null) {
        document.getElementById('ContentPlaceHolder1_divYourPrice').innerHTML = '<tt>Your Price :</tt> <strong>$' + saleprice.toFixed(2) + '</strong>';
    }

    if (document.getElementById('ContentPlaceHolder1_divYouSave') != null) {
        var diffprice = parseFloat(price) - parseFloat(saleprice);
        var diffpercentage = (parseFloat(diffprice) * parseFloat(100)) / parseFloat(price)
        document.getElementById('ContentPlaceHolder1_divYouSave').innerHTML = '<tt>You Save :</tt> <span style="color:#B92127;">$' + diffprice.toFixed(2) + '<span style="padding-left: 0px;color:#B92127;"> (' + diffpercentage.toFixed(2) + '%)</span></span>&nbsp;';
    }



}




function chkHeight() {

    if (document.getElementById('prepage')) {
        var windowHeight = 0;
        windowHeight = $(document).height(); //window.innerHeight;

        document.getElementById('prepage').style.height = windowHeight + 'px';
        document.getElementById('prepage').style.display = '';
    }
}

var $k = jQuery.noConflict();
$k(document).ready(function () {
    $k('#mycarousel').jcarousel({
        vertical: true,
        scroll: 2
    });
});

var $j = jQuery.noConflict();
$j(document).ready(function () {


    //            $j('#mycarousel').jcarousel({
    //                vertical: true,
    //                scroll: 2
    //            });


    $j("#divproperty").click(function () {
        $j('#divproperty1').slideToggle();
        if (document.getElementById('imgPro') != null) {
            if (document.getElementById('imgPro').src.toString().toLowerCase().indexOf('minimize.png') > -1) {
                $j('#imgPro').attr("src", document.getElementById('imgPro').src.replace('minimize.png', 'expand.gif'));
                $j('#imgPro').attr("title", 'Expand');
            }
            else {
                $j('#imgPro').attr("src", document.getElementById('imgPro').src.replace('expand.gif', 'minimize.png'));
                $j('#imgPro').attr("title", 'Collapse');
            }
        }

    });

    if (document.getElementById('ContentPlaceHolder1_hdnIsShowImageZoomer').value == "true" && document.getElementById('ContentPlaceHolder1_imgMain').src.indexOf('image_not_available') <= -1) {
        var imagename = '';
        if (document.getElementById('ContentPlaceHolder1_imgMain')) {
            imagename = document.getElementById('ContentPlaceHolder1_imgMain').src;
        }
        imagename = imagename.replace('medium', 'large');


        $j('#ContentPlaceHolder1_imgMain').addimagezoom({
            zoomrange: [3, 10],
            magnifiersize: [300, 300],
            magnifierpos: 'right',
            cursorshade: true,
            largeimage: imagename //<-- No comma after last option!
        });
        $j("#Button2").click(function () {
            var imagename = document.getElementById('ContentPlaceHolder1_imgMain').src;
            imagename = imagename.toLowerCase().replace('medium', 'large');

            $j('#ContentPlaceHolder1_imgMain').addimagezoom({
                zoomrange: [3, 10],
                magnifiersize: [300, 300],
                magnifierpos: 'right',
                cursorshade: true,
                largeimage: imagename + '?54666'
            })
        });
        //$('#loader_img1').show(); $('#ContentPlaceHolder1_imgMain').hide(); $('#ContentPlaceHolder1_imgMain').load(function () { $('#loader_img1').hide(); $('#ContentPlaceHolder1_imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });
    }

});

function ShowModelHelp(id) {

    // document.getElementById('header-part').style.zIndex = -1;

    document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay').height = '650px';
    document.getElementById('frmdisplay').width = '710px';
    document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:517px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    document.getElementById('popupContact').style.width = '710px';
    document.getElementById('popupContact').style.height = '650px';
    window.scrollTo(0, 0);
    document.getElementById('btnreadmore').click();
    document.getElementById('frmdisplay').src = '/MoreImages.aspx?PID=' + id;

}

function SelectVariantBypostback(strname,strvalue)
    {
    var variantname = strname.split(","); 
    var variantvalue = strvalue.split(","); 

    for (i = 0; i < variantname.length; i++) {
    if( variantvalue[i].toLowerCase().indexOf('select ') > -1)
    {
    if(document.getElementById(variantname[i]) != null)
    {
    document.getElementById(variantname[i]).value = '0';
    }
    }
    else
    {
    if(document.getElementById(variantname[i]) != null)
    {
   document.getElementById(variantname[i]).value = variantvalue[i];
   }
   }


    }

    }

    function variantDetail(divid) {
    centerPopup1();
            //document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '500px';
            document.getElementById('frmdisplay1').width = '517px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:517px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '517px';
            document.getElementById('popupContact1').style.height = '500px';
             
            document.getElementById('btnhelpdescri').click();
 
             document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML =  document.getElementById(divid).innerHTML;
        }

        function ShowModelHelpShipping(id) {
           // document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '620px';
            document.getElementById('frmdisplay1').width = '630px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:630px;height:620px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '630px';
            document.getElementById('popupContact1').style.height = '620px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }

         function ShowModelForNotification(id) {
           // document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '400px';
            document.getElementById('frmdisplay1').width = '610px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:610px;height:400px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '610px';
            document.getElementById('popupContact1').style.height = '400px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }

        function ShowModelForInappropriateRating(id) {
           // document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '400px';
            document.getElementById('frmdisplay1').width = '610px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:610px;height:400px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '610px';
            document.getElementById('popupContact1').style.height = '400px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }

         function ShowModelForPriceMatch(id) {
           // document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '650px';
            document.getElementById('frmdisplay1').width = '600px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:600px;height:650px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '600px';
            document.getElementById('popupContact1').style.height = '650px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }

        function clearcomment() {
            if (document.getElementById('ContentPlaceHolder1_txtname') != null) { document.getElementById('ContentPlaceHolder1_txtname').value = 'Enter your name'; }
            if (document.getElementById('ContentPlaceHolder1_txtEmail') != null) { document.getElementById('ContentPlaceHolder1_txtEmail').value = 'Enter your email address'; }
            if (document.getElementById('ContentPlaceHolder1_txtcomment') != null) { document.getElementById('ContentPlaceHolder1_txtcomment').value = 'Enter your comment'; }
            if (document.getElementById('ContentPlaceHolder1_ddlrating') != null) {
                document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex = 0;
                ratingImage();
            }
        }

        function onKeyPressBlockNumbers1(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key != 46) {
                if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 0) {
                    return key;
                }
            }
            if (key == 13) {
                if (document.getElementById('ContentPlaceHolder1_btnAddToCart') != null) {
                    document.getElementById('ContentPlaceHolder1_btnAddToCart').click();
                }
            }
            var keychar = String.fromCharCode(key);
            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
        function onKeyPressBlockNumbersOnly(e) {
            var key = window.event ? window.event.keyCode : e.which;
           
            if (key != 46) {
                if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 0) {
                    return key;
                }
            }
            if (key == 13) {
                if (document.getElementById('ContentPlaceHolder1_btnAddToCart') != null) {
                    document.getElementById('ContentPlaceHolder1_btnAddToCart').click();
                }
            }
            var keychar = String.fromCharCode(key);
            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
       


        function FreeEngraving() {
            if (document.getElementById('ddlIsFreeEngraving') != null) {
                if ((document.getElementById('ddlIsFreeEngraving').options[document.getElementById('ddlIsFreeEngraving').selectedIndex]).text == 'Yes') {
                    if (document.getElementById('hdnEngraving') != null && document.getElementById('hdnEngraving').value == '') {
                        document.getElementById('hdnEngraving').value = document.getElementById('divmain').innerHTML;
                    }
                    var allDiv = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('div');
                    var i = 0;
                    for (i = 0; i < allDiv.length; i++) {
                        var divid = allDiv[i];
                        if (divid.id.indexOf('divmain') > -1) {
                            divid.style.display = '';
                            document.getElementById('ContentPlaceHolder1_divQuantity').style.display = 'none';
                        }
                    }
                    document.getElementById('ddlIsFreeEngraving').selectedIndex = 0;
                }
                else {
                    var allDiv = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('div');
                    var i = 0;
                    for (i = 0; i < allDiv.length; i++) {
                        var divid = allDiv[i];
                        if (divid.id.indexOf('divmain') > -1) {
                            divid.style.display = 'none';
                            document.getElementById('ContentPlaceHolder1_divQuantity').style.display = '';
                        }
                    }
                    document.getElementById('ddlIsFreeEngraving').selectedIndex = 1;
                }
            }
        }

        function checkTest() {
            if (document.getElementById('hdnEngraving') != null && document.getElementById('hdnEngraving').value != '') {
                var tt = document.getElementById('ContentPlaceHolder1_divParent').innerHTML;
                var allInput = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('*');

                tt = tt.replace('style="display: none;" onclick="removeele', 'style="display: block;" onclick="removeele');
                tt = tt.replace('style="display: block;" onclick="checkTest();"', 'style="display: none;" onclick="checkTest();"');

                var NewdivName = 0;
                var TotcountNew = 0;
                var Totcount = 0;
                if (document.getElementById('hdnCountEngraving') != null) {
                    if (document.getElementById('hdnCountEngraving').value != '') {
                        Totcount = document.getElementById('hdnCountEngraving').value;
                        TotcountNew = parseInt(Totcount) + parseInt(1);
                        NewdivName = 'divmain' + TotcountNew;
                        document.getElementById('hdnCountEngraving').value = TotcountNew;
                    }
                    else
                    { document.getElementById('hdnCountEngraving').value = "1"; NewdivName = 'divmain1'; }
                }

                var newDiv = "<div id='" + NewdivName + "'>" + document.getElementById('hdnEngraving').value.replace('divmain', NewdivName).replace(/_kau_/g, TotcountNew) + "</div>";

                var j = 0;
                var str = "";
                var strvalue = "";
                for (j = 0; j < allInput.length; j++) {
                    var inttype = allInput[j];
                    if (inttype.type == 'text') {
                        str = str + inttype.id.toString() + ',';
                        strvalue = strvalue + inttype.value.toString() + ',';
                    }
                    else if (inttype.type == 'select-one') {
                        str = str + inttype.id.toString() + ',';
                        strvalue = strvalue + inttype.selectedIndex.toString() + ',';
                    }
                }
                document.getElementById('ContentPlaceHolder1_divParent').innerHTML = tt + newDiv;
                var n = str.split(",");
                var m = strvalue.split(",");
                var k = 0;
                for (k = 0; k < n.length; k++) {
                    var controlid = n[k];
                    if (controlid.indexOf('txt') > -1) {
                        document.getElementById(controlid).value = m[k];
                    }
                    else if (controlid.indexOf('select') > -1) {
                        document.getElementById(controlid).selectedIndex = m[k];
                    }
                }
                document.getElementById('ddlIsFreeEngraving').selectedIndex = 0;
            }
        }

        function removeelement(id) {
            document.getElementById(id).innerHTML = '';
        }

        function AddElement() {
        }

        function AddTocartForEngra() {
            var strname = "";
            var strvalue = "";
            var strQty = "";

            if (document.getElementById('ContentPlaceHolder1_divParent') != null && document.getElementById('ddlIsFreeEngraving').selectedIndex == 0) {
                var allDiv = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('div');

                var k = 0;
                for (k = 0; k < allDiv.length; k++) {
                    var controlid = allDiv[k];
                    if (controlid.id.indexOf('divmain') > -1) {

                        strname = strname + "divmain=";
                        strvalue = strvalue + "divmain=";
                        strQty = strQty + "divmain=";
                        var allDiv1 = controlid.getElementsByTagName('*');
                        var j = 0;

                        for (j = 0; j < allDiv1.length; j++) {
                            var controlid1 = allDiv1[j];

                            if (controlid1.id.indexOf('pname') > -1 && controlid1.type != 'hidden') {

                                var hdnid = "hdn" + controlid1.id;
                                strname = strname + document.getElementById(hdnid).value + ',';
                            }
                            if (controlid1.type == 'text' && controlid1.id.indexOf('txtQty') > -1) {
                                strQty = strQty + controlid1.value + ',';
                            }
                            else if (controlid1.type == 'text') {
                                strvalue = strvalue + controlid1.value + ',';
                            }
                            else if (controlid1.type == 'select-one') {
                                strvalue = strvalue + controlid1.options[controlid1.selectedIndex].text + ',';
                            }
                        }
                    }
                }
            }

            document.getElementById('ContentPlaceHolder1_hdnEngravName').value = strname;
            document.getElementById('ContentPlaceHolder1_hdnEngravvalue').value = strvalue;
            document.getElementById('ContentPlaceHolder1_hdnEngravQty').value = strQty;
        }
 function SetTimee() {

            setInterval("CheckTimer();", 1000);
            tabberAutomatic();
        }

         function moneyConvert(value) {
            var buf = "";
            var sBuf = "";
            var j = 0;
            value = String(value);
            if (value.indexOf(".") > 0) {
                buf = value.substring(0, value.indexOf("."));
            } else {
                buf = value;
            }
            if (buf.length % 3 != 0 && (buf.length / 3 - 1) > 0) {
                sBuf = buf.substring(0, buf.length % 3) + ",";
                buf = buf.substring(buf.length % 3);
            }
            j = buf.length;
            for (var i = 0; i < (j / 3 - 1); i++) {
                sBuf = sBuf + buf.substring(0, 3) + ",";
                buf = buf.substring(3);
            }
            sBuf = sBuf + buf;
            if (value.indexOf(".") > 0) {
                value = sBuf + value.substring(value.indexOf("."));
            }
            else {
                value = sBuf;
            }
            return value;
        }


        function checkDate(sender, args) {
        }
        function ShowImagevideo() {

            if (document.getElementById('videoid') != null && document.getElementById('videoid').src.indexOf('play') > -1) {

                document.getElementById('Hidden1').value = '0';

                if (document.getElementById('imgMain') != null) {

                    document.getElementById('Button1').click();

                }
                document.getElementById('ContentPlaceHolder1_imgMain').style.display = 'none';
                if (document.getElementById('ContentPlaceHolder1_divzioom') != null) {
                    document.getElementById('ContentPlaceHolder1_divzioom').style.display = 'none';

                }
                document.getElementById('divvedeo').style.display = '';
                document.getElementById('videoid').src = '/images/pause.png';
            }
            else {
                document.getElementById('Hidden1').value = '1';
                document.getElementById('ContentPlaceHolder1_imgMain').style.display = '';
                if (document.getElementById('ContentPlaceHolder1_divzioom') != null) {
                    document.getElementById('ContentPlaceHolder1_divzioom').style.display = '';
                }
                document.getElementById('divvedeo').style.display = 'none';
                document.getElementById('videoid').src = '/images/play.png';
                if (document.getElementById('imgMain') != null) {
                    document.getElementById('Button1').click();
                }
            }
        }
         function MM_openBrWindow(theURL, winName, features) { //v2.0
            window.open(theURL, winName, features);
        }

         function ratingImage() {
            var indx = document.getElementById("ContentPlaceHolder1_ddlrating").selectedIndex;
            if (indx == 0) {
                document.getElementById("img1").src = '/images/star-form1.jpg';
                document.getElementById("img2").src = '/images/star-form1.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';
            }
            else if (indx == 1) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form1.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 2) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 3) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 4) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 5) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form.jpg';
                document.getElementById("img5").src = '/images/star-form.jpg';

            }
        }

        
        function CheckExits() {

            if (document.getElementById('ContentPlaceHolder1_txtname').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Name.', 'Message', 'ContentPlaceHolder1_txtname');
                //document.getElementById('ContentPlaceHolder1_txtname').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtname').value.toString().toLowerCase() == 'enter your name') {
                jAlert('Please Enter Name.', 'Message', 'ContentPlaceHolder1_txtname');
                //document.getElementById('ContentPlaceHolder1_txtname').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');
                //document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.toString().toLowerCase() == 'enter your email address') {
                jAlert('Please Enter Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');
                //document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }

            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, ""))) {
                jAlert('Please Enter Valid Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');
                //document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtcomment').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Comment', 'Message', 'ContentPlaceHolder1_txtcomment');
                // document.getElementById('ContentPlaceHolder1_txtcomment').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtcomment').value.toString().toLowerCase() == 'enter your comment') {
                jAlert('Please Enter Comment.', 'Message', 'ContentPlaceHolder1_txtcomment');
                //document.getElementById('ContentPlaceHolder1_txtcomment').focus();
                return false;
            }

            if (document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex == 0) {
                jAlert('Please Select Rating.', 'Message', 'ContentPlaceHolder1_ddlrating');
                //document.getElementById('ContentPlaceHolder1_ddlrating').focus();
                return false;
            }
            document.getElementById('prepage').style.display = '';
            if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
                $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
            }
            return true;
        }
        var Emailresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                Emailresults = true
            else {
                Emailresults = false
            }
            return (Emailresults)
        }