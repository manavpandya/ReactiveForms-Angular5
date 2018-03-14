
//for Poup
function MM_openBrWindow(theURL, winName, features) {
    if (theURL.search(/clicktoenlarge.aspx/i) == 0)
        return; window.open(theURL, winName, features);
}

function OpenCenterWindow(pid, wi, he) {
    var w = wi;
    var h = he;
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}

//for Bookmark
function bookmarksite(title, url) {
    if (window.sidebar)
        window.sidebar.addPanel(title, url, ""); else if (window.opera && window.print) { var elem = document.createElement('a'); elem.setAttribute('href', url); elem.setAttribute('title', title); elem.setAttribute('rel', 'sidebar'); elem.click(); }
    else if (document.all)
        window.external.AddFavorite(url, title); else
        alert("Press Ctl + D To Bookmark this Page.");
}
function InsertProductOrderMultiSwatchAddtocart(Pid, eleclicked) {
    var Names = ""; var Values = "";
    //    if (document.getElementById('txtSwatchqty')) {
    //        if (document.getElementById('txtSwatchqty').value <= 0 || document.getElementById('txtSwatchqty').value == '' || isNaN(document.getElementById('txtSwatchqty').value))
    //        { jAlert("Please enter valid digits only !!!", "Message", "txtSwatchqty"); document.getElementById('txtSwatchqty').value = '1'; document.getElementById('txtSwatchqty').focus(); return false; }
    //    }
    Alltext = '';
    if (window.parent.document.getElementById("divMiniCart")) {
        Alltext = window.parent.document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked);
    resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid;
    var allElts; var i; var hdnprice = "";
    //     if (document.getElementById('txtSwatchqty'))
    //    { quantity = document.getElementById('txtSwatchqty').value; }
    //    if (document.getElementById('txtQty'))
    //    { quantity = document.getElementById('txtQty').value; }

    if (document.getElementById('ContentPlaceHolder1_hdnSelectedPID'))

    { ProductID = document.getElementById('ContentPlaceHolder1_hdnSelectedPID').value; }
    if (document.getElementById('ContentPlaceHolder1_hdnSelectedPrice'))
    { hdnprice = document.getElementById('ContentPlaceHolder1_hdnSelectedPrice').value; }
    if (document.getElementById('ContentPlaceHolder1_hdnSelectedQty'))
    { quantity = document.getElementById('ContentPlaceHolder1_hdnSelectedQty').value; }

    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    var hdnva = eleclicked.replace('_aorderswatch_', '_hdnswatchprice_');

    //if (document.getElementById(hdnva))
    //{ hdnprice = document.getElementById(hdnva).value; }
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=InsertMultiSwatch&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + ProductID + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProductQuickSwatch_All; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    return true;
}

function HandleResponseforInsertProductQuickSwatch_All() {
    if (XmlHttp.readyState == 4) {
        var maxlength = 0;
        if (document.getElementById('ContentPlaceHolder1_hdnswatchmaxlength')) {
            maxlength = document.getElementById('ContentPlaceHolder1_hdnswatchmaxlength').value;
        }
        if (XmlHttp.status == 200) {
            var result = XmlHttp.responseText; if (result.toLowerCase().indexOf('not sufficient inventory') != -1) {
                //alert("Not enough Inventory..."); 

                var tt = '';
                if (result.toLowerCase().indexOf('###inv###') != -1) {
                    tt = result.substring(result.toLowerCase().indexOf('###inv###') + 9, result.toLowerCase().indexOf('###inv1###') - result.toLowerCase().indexOf('###inv###'));
                    //alert(tt);
                    //alert(result.toLowerCase().indexOf('###inv###'));
                    //alert(result.toLowerCase().indexOf('###inv1###'));
                    //alert(result.toLowerCase().indexOf('###inv1###') - result.toLowerCase().indexOf('###inv###'));
                    ShowInventoryMessageNew(tt);

                    result = result.substring(result.toLowerCase().indexOf('###cart###') + 10).replace('###cart1###', '');
                    document.getElementById("divCart").innerHTML = result; if (document.getElementById('divMiniCart'))
                    { showLayer('divMiniCart'); }
                    SetTotalQuantity(); if (jQuery().scrollTo != null) { jQuery().scrollTo({ top: '0px', left: '00px' }, 1000); } else { window.scrollTo(0, 0); showMiniCart(); } if (eleForTransfer != null)
                        //SetTotalQuantity(); jQuery().scrollTo({ top: '0px', left: '00px' }, 1000); if (eleForTransfer != null)
                    { jQuery(eleForTransfer).show("transfer", options, 1000, showMiniCart); eleForTransfer = null; }
                    //alert(Alltext);
                }

                else if (result.toLowerCase().indexOf('shraddha') != -1) {

                    ShowInventoryMessageNew(result);
                }
                else {

                    ShowInventoryMessage(result);
                }

                //if (Alltext != '') { window.parent.document.getElementById("divMiniCart").innerHTML = Alltext; } if (document.getElementById("txtQty"))
                //    document.getElementById("txtQty").focus();



                return false;
            }

            window.parent.document.getElementById("divCart").innerHTML = ""; window.parent.document.getElementById("divCart").innerHTML = result;

            if (window.parent.document.getElementById('divMiniCart')) {
                // showLayerQuick('divMiniCart'); 
                //alert('You have added order swatch successfully. \n Max. limit(' + maxlength + ')');


                ShowSwatchMessage();
            }

            SetTotalQuantityquick();
            //showMiniCartQuick(); 
            // window.parent.scrollTo(0, 0); 
            //window.parent.disablePopup();

            if (eleForTransfer != null)
            { eleForTransfer = null; }
            else {
                setTimeout("showMiniCartQuick()", 1000);
                //alert('You have added        order swatch successfully. \n Order Swatch Max. limit is ' + maxlength + '');
                //document.getElementById('divswatchinfo').innerHTML=
                ShowSwatchMessage();

            }
        }
        else { alert("There was a problem retrieving data from the server."); }
    }
}
//for newsletter
//function ValidNewSletter() {
//    var element; if (document.getElementById('txtSubscriber'))
//    { element = document.getElementById('txtSubscriber'); }

//    var elementZip; if (document.getElementById('txtNewsZipCode'))
//    { elementZip = document.getElementById('txtNewsZipCode'); }
//    if (document.getElementById('txtSubscriber'))
//    { element = document.getElementById('txtSubscriber'); }
//    if (element.value == '') {
//        alert('Please enter your E-mail Address.'); if (document.getElementById('txtSubscriber'))
//        { document.getElementById('txtSubscriber').focus(); }
//        if (document.getElementById('txtSubscriber'))
//        { document.getElementById('txtSubscriber').focus(); }
//        return false;
//    }
//    else if (element.value.toString().toLowerCase() == 'enter your e-mail address') {
//        alert('Please enter your E-Mail Address.'); if (document.getElementById('txtSubscriber'))
//        { document.getElementById('txtSubscriber').focus(); }
//        if (document.getElementById('txtSubscriber'))
//        { document.getElementById('txtSubscriber').focus(); }
//        return false;
//    }


//    else {
//        var testresults; var str = element.value; var filter = /^.+@.+\..{2,3}$/; if (filter.test(str)) {
//            if (elementZip.value == '') {
//                alert('Please enter zip code.'); if (document.getElementById('txtNewsZipCode'))
//                { document.getElementById('txtNewsZipCode').focus(); }
//                if (document.getElementById('txtNewsZipCode'))
//                { document.getElementById('txtNewsZipCode').focus(); }
//                return false;
//            }
//            else if (elementZip.value.toString().toLowerCase() == 'enter zip') {
//                alert('Please enter your zip code.'); if (document.getElementById('txtNewsZipCode'))
//                { document.getElementById('txtNewsZipCode').focus(); }
//                if (document.getElementById('txtNewsZipCode'))
//                { document.getElementById('txtNewsZipCode').focus(); }
//                return false;
//            }
//            else { return true; }
//        }
//        else {
//            alert("Please enter valid E-Mail Address.")
//            if (document.getElementById('txtSubscriber'))
//            { document.getElementById('txtSubscriber').focus(); }
//            if (document.getElementById('txtSubscriber'))
//            { document.getElementById('txtSubscriber').focus(); }
//            return false;
//        }
//    }
//    if (elementZip.value == '') {
//        alert('Please enter zip code.'); if (document.getElementById('txtNewsZipCode'))
//        { document.getElementById('txtNewsZipCode').focus(); }
//        if (document.getElementById('txtNewsZipCode'))
//        { document.getElementById('txtNewsZipCode').focus(); }
//        return false;
//    }
//    else if (elementZip.value.toString().toLowerCase() == 'enter zip') {
//        alert('Please enter your zip code.'); if (document.getElementById('txtNewsZipCode'))
//        { document.getElementById('txtNewsZipCode').focus(); }
//        if (document.getElementById('txtNewsZipCode'))
//        { document.getElementById('txtNewsZipCode').focus(); }
//        return false;
//    }
//}
//function clear_text() {
//    if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value.toString().toLowerCase() == 'enter your e-mail address')
//    { document.getElementById('txtSubscriber').value = ""; }
//    if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value.toString().toLowerCase() == 'enter your e-mail address')
//    { document.getElementById('txtSubscriber').value = ""; }
//    return false;
//}
//function clear_NewsLetter(myControl) {
//    if (myControl && myControl.value.toString().toLowerCase() == 'enter your e-mail address')
//        myControl.value = "";
//}
//function ChangeNewsLetter(myControl) {
//    if (myControl != null && myControl.value == '')
//        myControl.value = "Enter your E-Mail Address";
//}
//function Change() {
//    if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
//        document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
//        document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; return false;
//}


//function clear_Zip(myControl) {
//    if (myControl && myControl.value.toString().toLowerCase() == 'enter zip')
//        myControl.value = "";
//}
//function ChangeZip(myControl) {
//    if (myControl != null && myControl.value == '')
//        myControl.value = "Enter Zip";
//}


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
    else if (element.value.toString().toLowerCase() == 'enter your e-mail address') {
        alert('Please enter your E-Mail Address.'); if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        return false;
    }
    else {
        var testresults; var str = element.value; var filter = /^.+@.+\..{2,3}$/; if (filter.test(str))
        {   return true;  }
        else {
            alert("Please enter valid E-Mail Address.")
            if (document.getElementById('txtSubscriber'))
            { document.getElementById('txtSubscriber').focus(); }
            if (document.getElementById('txtSubscriber'))
            { document.getElementById('txtSubscriber').focus(); }
            return false;
        }
    }
    return false;
}
function clear_text() {
    if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value.toString().toLowerCase() == "enter your e-mail address")
    { document.getElementById('txtSubscriber').value = ""; }
    if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value.toString().toLowerCase() == "enter your e-mail address")
    { document.getElementById('txtSubscriber').value = ""; }
    return false;
}
function clear_NewsLetter(myControl) {
    if (myControl && myControl.value.toString().toLowerCase() == "enter your e-mail address")
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
    if (myControl && myControl.value.toString().toLowerCase() == "enter search here") {

        myControl.value = "";
        //myControl.removeAttribute("class");
        myControl.setAttribute('style','color:#393939 !important;');
        
    }
}
function ChangeSearch(myControl) {
    if (myControl != null && myControl.value == '') {
        myControl.value = "Enter Search Here";
        myControl.removeAttribute('style');
        
    }
}


function ValidSearch() {
    var myControl;

    if (document.getElementById('txtSearch')) {
        myControl = document.getElementById('txtSearch');
    }



    if (myControl.value == '' || myControl.value.toString().toLowerCase() == 'enter search here') {
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

function onKeyPressBlockNumbers(e) {
    var key = window.event ? window.event.keyCode : e.which;

    if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0) {
        return key;
    }

    var keychar = String.fromCharCode(key);

    var reg = /\d/;
    if (window.event)
        return event.returnValue = reg.test(keychar);
    else
        return reg.test(keychar);
}

// Search Validation

function ValidSearchByPage() {
    var myControl;
    if (document.getElementById('txtSearch'))
    { myControl = document.getElementById('txtSearch'); }

    if (myControl.value == '' || myControl.value == 'Enter Keyword...') {
        alert("Please enter something to search");
        if (document.getElementById('txtSearch'))
        { document.getElementById('txtSearch').focus(); }
        return false;
    }

    var str = myControl.value.replace(/^\s+|\s+$/g, '');
    while (str.substring(str.length - 1, str.length) == ' ') // check white space from end
    {
        str = str.substring(0, str.length - 1);
    }

    if (str.length < 3)
    { alert("Please enter at least 3 characters to search"); myControl.focus(); return false; }
    location.href = '/search.aspx?SearchTerm=' + urlencode(myControl.value.replace(/^\s+|\s+$/g, ''));
    return false;
}

function urlencode(str) {
    return escape(str).replace('+', '%2B').replace('*', '%2A').replace('/', '%2F').replace('@', '%40');
}



/*For Mini Cart*/
var TotalProductAvail = '0'; var resetCartHover = true; var Alltext = ''; var Custid; var XmlHttp; var timevar; var options = { to: "#divShoppingCart", className: 'ui-effects-transfer' }; var eleForTransfer = null; var CartVisible = false; var fadingOut = false; function hideLayer(elementId) {
    CartVisible = false; if (document.getElementById(elementId) != null)
    { document.getElementById(elementId).style.display = "none"; }
}
function hideLayerQuick(elementId) {
    CartVisible = false; if (window.parent.document.getElementById(elementId) != null)
    { window.parent.document.getElementById(elementId).style.display = "none"; }
}
function showLayer(elementId) {
    if (document.getElementById(elementId) != null)
    { document.getElementById(elementId).style.display = "block"; }
}
function showLayerQuick(elementId) {
    if (window.parent.document.getElementById(elementId) != null)
    { window.parent.document.getElementById(elementId).style.display = "block"; }
}
function AddCart(Items, eleclicked) {
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    resetHover(); hideMiniCart(); var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?RandomStr=" + randomnumber + "&Mode=Add&CustID=" + Custid;
    var p = document.getElementById('tt').innerHTML;
    if (Items > 1)
        document.getElementById('cartlink').innerHTML = "<p><span class='navQty'> (" + Items + " items)</span><span class='navTotal'> " + p + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon'>"; else
        document.getElementById('cartlink').innerHTML = "<p><span class='navQty'> (" + Items + " item)</span><span class='navTotal'> " + p + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon'>"; document.getElementById('hiddenTotalItems').value = Items; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponse; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null); return true;
}
function CreateXmlHttp() {
    XmlHttp = null; try
{ XmlHttp = new ActiveXObject("Msxml2.XMLHTTP"); }
    catch (e) {
        try
{ XmlHttp = new ActiveXObject("Microsoft.XMLHTTP"); }
        catch (e1)
{ XmlHttp = null; }
    }
    if (!XmlHttp && typeof XMLHttpRequest != "undefined")
    { XmlHttp = new XMLHttpRequest(); }
}
function HandleResponse() {
    if (XmlHttp.readyState == 4) {
        if (document.getElementById('pnlUpdate'))
        { document.getElementById('pnlUpdate').style.display = "none"; }
        if (XmlHttp.status == 200) {
            document.getElementById("divCart").innerHTML = XmlHttp.responseText; if (document.getElementById('divMiniCart'))
            { showLayer('divMiniCart'); }
            jQuery().scrollTo({ top: '0px', left: '20px' }, 1000); if (eleForTransfer != null)
            { jQuery(eleForTransfer).show("transfer", options, 1000, showMiniCart); eleForTransfer = null; }
            else
                setTimeout("showMiniCart()", 1000);
        }
        else
        { alert("There was a problem retrieving data from the server."); }
    }
}
function HandleResponseQuick() {
    if (XmlHttp.readyState == 4) {
        if (document.getElementById('pnlUpdate'))
        { document.getElementById('pnlUpdate').style.display = "none"; }
        if (XmlHttp.status == 200) {
            window.parent.document.getElementById("divCart").innerHTML = XmlHttp.responseText; if (window.parent.document.getElementById('divMiniCart')) { //showLayer('divMiniCart'); 
            }
            jQuery().scrollTo({ top: '0px', left: '20px' }, 1000); if (eleForTransfer != null)
            { jQuery(eleForTransfer).show("transfer", options, 1000, showMiniCart); eleForTransfer = null; }
            else
                setTimeout("showMiniCart()", 1000);
        }
        else
        { alert("There was a problem retrieving data from the server."); }
    }
}
function ShowHideCart() {
    if (window.location.pathname.toLowerCase().indexOf('/addtocart.aspx') != -1 || window.location.pathname.toLowerCase().indexOf('/checkoutcommon.aspx') != -1 || window.location.pathname.toLowerCase().indexOf('/order.aspx') != -1)
    { hideLayer('divMiniCart'); return; }
    if (document.getElementById('hiddenTotalItems').value == 0.00)
    { hideLayer('divMiniCart'); }
    else
    { showLayer('divMiniCart'); showMiniCart(); }
}
function RemoveProduct(ProductDetails) {
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Delete&RandomNum=" + randomnumber + "&CustID=" + Custid + "&Products=" + ProductDetails; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforUpdateRemove; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null); return true;
}
function UpdateProduct() {
    Alltext = '';


    if (document.getElementById("divMiniCart")) {
        Alltext = document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    var allElts; allElts = document.getElementById("divMiniCart").getElementsByTagName("input"); var i; var ProductDetails = ""; for (i = 0; i < allElts.length; i++) {
        var elt = allElts[i]; if (elt.type == "text" && elt.id.toLowerCase().indexOf('txtqty-') != -1) {
            var price = "0";


            ProductDetails = ProductDetails + elt.id + "-" + elt.value + "-" + price + "*"; if (isNaN(elt.value) || elt.value <= 0) {

                elt.value = 1; alert("Please enter valid Quantity in Cart."); if (document.getElementById('pnlUpdate'))
                { document.getElementById('pnlUpdate').style.display = "none"; }
                elt.focus(); return false;
            }
        }
    }
    ProductDetails = ProductDetails.replace(/\+/g, '%2B').replace(/\&/g, '%26').replace(/\?/g, '%3F').replace(/&#39;/g, "'");
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Update&RandomNum=" + randomnumber + "&CustID=" + Custid + "&Products=" + ProductDetails; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforUpdateRemove; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null); return true;
}
function HandleResponseforUpdateRemove() {
    if (XmlHttp.readyState == 4) {
        if (document.getElementById('pnlUpdate'))
        { document.getElementById('pnlUpdate').style.display = "none"; }
        if (XmlHttp.status == 200) {
            document.getElementById("divCart").innerHTML = ""; 
            document.getElementById("divCart").innerHTML = XmlHttp.responseText; if (document.getElementById("divCart").innerHTML.toLowerCase().indexOf('not sufficient inventory') != -1) {
                //alert('Not enough Inventory...');
                ChkInventoryUpdate(document.getElementById("divCart").innerHTML);
            if (Alltext != '') { document.getElementById("divMiniCart").innerHTML = Alltext; } return false; }
            if (document.getElementById('divMiniCart'))
            { showLayer('divMiniCart'); }
            SetTotalQuantity();
            //            jQuery().scrollTo({ top: '0px', left: '00px' }, 1000); if (eleForTransfer != null)
            //            { jQuery(eleForTransfer).show("transfer", options, 1000, showMiniCart); eleForTransfer = null; }
            //            else
            setTimeout("showMiniCart()", 1000);
        }
        else
        { alert("There was a problem retrieving data from the server."); }
    }
}
function SetCustomer(CustomerID)
{ Custid = CustomerID; document.getElementById('hiddenCustID').value = CustomerID; }
function SetTotalQuantity() {
    //    var allElts; allElts = document.getElementById("divMiniCart").getElementsByTagName("input"); var i; var Total = 0; for (i = 0; i < allElts.length; i++) {
    //        var elt = allElts[i]; if (elt.type == "text" && elt.id.toLowerCase().indexOf('txtqty-') != -1)
    //        { Total = parseInt(Total) + parseInt(elt.value); }
    //    }
    //    var p = document.getElementById('tt').innerHTML;

    Total = 0;
    var allElts; allElts = document.getElementById("divMiniCart").getElementsByTagName("input"); var i; var Total = 0; for (i = 0; i < allElts.length; i++) {
        var elt = allElts[i]; if (elt.type == "text" && (elt.id.toLowerCase().indexOf('txtqty-') != -1 || elt.id.toLowerCase().indexOf('txtqtychild-') != -1))
        { Total = parseInt(Total) + parseInt(elt.value); }
    }

    var p = '$0.00';
    if (document.getElementById('tt') == null) { }
    else {
        p = document.getElementById('tt').innerHTML;
    }
    if (Total.toString().length <= 1) {
        Total = "0" + Total;
    }
    document.getElementById('hiddenTotalItems').value = Total; if (Total > 1)
        document.getElementById('cartlink').innerHTML = "<p><span class='navQty'> (" + Total + " items)</span><span class='navTotal'> " + p + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon'>"; else
        document.getElementById('cartlink').innerHTML = "<p><span class='navQty'> (" + Total + " item)</span><span class='navTotal'> " + p + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon'>"; if (Total == 0) {
        if (document.getElementById('divMiniCart'))
            hideLayer('divMiniCart');
    }
}
function SetTotalQuantityquick() {
    //    var allElts; allElts = document.getElementById("divMiniCart").getElementsByTagName("input"); var i; var Total = 0; for (i = 0; i < allElts.length; i++) {
    //        var elt = allElts[i]; if (elt.type == "text" && elt.id.toLowerCase().indexOf('txtqty-') != -1)
    //        { Total = parseInt(Total) + parseInt(elt.value); }
    //    }
    //    var p = document.getElementById('tt').innerHTML;

    Total = 0;
    var allElts; allElts = window.parent.document.getElementById("divMiniCart").getElementsByTagName("input"); var i; var Total = 0; for (i = 0; i < allElts.length; i++) {
        var elt = allElts[i]; if (elt.type == "text" && (elt.id.toLowerCase().indexOf('txtqty-') != -1 || elt.id.toLowerCase().indexOf('txtqtychild-') != -1))
        { Total = parseInt(Total) + parseInt(elt.value); }
    }

    var p = '$0.00';
    if (window.parent.document.getElementById('tt') == null) { }
    else {
        p = window.parent.document.getElementById('tt').innerHTML;
    }
    if (Total.toString().length <= 1) {
        Total = "0" + Total;
    }
    window.parent.document.getElementById('hiddenTotalItems').value = Total; if (Total > 1)
        window.parent.document.getElementById('cartlink').innerHTML = "<p><span class='navQty'> (" + Total + " items)</span><span class='navTotal'> " + p + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon'>"; else
        window.parent.document.getElementById('cartlink').innerHTML = "<p><span class='navQty'> (" + Total + " item)</span><span class='navTotal'> " + p + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon'>"; if (Total == 0) {
        if (window.parent.document.getElementById('divMiniCart'))
            hideLayerQuick('divMiniCart');
    }
}
function CheckQty(e) {
    var key = window.event ? window.event.keyCode : e.which; if (key == 13)
    { UpdateProduct(); return false; }
    if (key == 8 || key == 9 || key == 189 || key == 0)
    { return key; }
    var keychar = String.fromCharCode(key); var reg = /\d/; if (window.event)
        return event.returnValue = reg.test(keychar); else
        return reg.test(keychar);
}
var TotalProductAvail = '0'; var resetCartHover = true; var Custid; var XmlHttp; var timevar; var options = { to: "#divShoppingCart", className: 'ui-effects-transfer' }; var eleForTransfer = null; var CartVisible = false; var fadingOut = false; function hideLayer(elementId) {
    CartVisible = false; if (document.getElementById(elementId) != null)
    { document.getElementById(elementId).style.display = "none"; }
}
function getPageScrollForPopup() {
    var xScroll, yScroll; if (self.pageYOffset) { yScroll = self.pageYOffset; xScroll = self.pageXOffset; } else if (document.documentElement && document.documentElement.scrollTop) { yScroll = document.documentElement.scrollTop; xScroll = document.documentElement.scrollLeft; } else if (document.body) { yScroll = document.body.scrollTop; xScroll = document.body.scrollLeft; }
    arrayPageScrollForPopup = new Array(xScroll, yScroll)
    return arrayPageScrollForPopup;
}
function getPageSizeForPopup() {
    var xScroll, yScroll; if (window.innerHeight && window.scrollMaxY) { xScroll = window.innerWidth + window.scrollMaxX; yScroll = window.innerHeight + window.scrollMaxY; } else if (document.body.scrollHeight > document.body.offsetHeight) { xScroll = document.body.scrollWidth; yScroll = document.body.scrollHeight; } else { xScroll = document.body.offsetWidth; yScroll = document.body.offsetHeight; }
    var windowWidth, windowHeight; if (self.innerHeight) {
        if (document.documentElement.clientWidth) { windowWidth = document.documentElement.clientWidth; } else { windowWidth = self.innerWidth; }
        windowHeight = self.innerHeight;
    } else if (document.documentElement && document.documentElement.clientHeight) { windowWidth = document.documentElement.clientWidth; windowHeight = document.documentElement.clientHeight; } else if (document.body) { windowWidth = document.body.clientWidth; windowHeight = document.body.clientHeight; }
    if (yScroll < windowHeight) { pageHeight = windowHeight; } else { pageHeight = yScroll; }
    if (xScroll < windowWidth) { pageWidth = xScroll; } else { pageWidth = windowWidth; }
    arrayPageSize = new Array(pageWidth, pageHeight, windowWidth, windowHeight)
    return arrayPageSize;
}
function InsertProductMultiple(eleclicked) {


    var Names = ""; var Values = "";
    Alltext = '';
    if (document.getElementById("divMiniCart")) {
        Alltext = document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); resetHover(); hideMiniCart(); var quantity = ""; var ProductID = ""; var hdnprice = ""; var iCount = 0; var ids = "";

    if (document.getElementById('divyouMayalso') != null) {
        var allrelatedproduct = document.getElementById('divyouMayalso').getElementsByTagName('INPUT');
        for (iP = 0; iP < allrelatedproduct.length; iP++) {
            var elt = allrelatedproduct[iP];
            if (ids == "") {
                if (elt.type == 'text' && elt.id.indexOf('txtmayQty') > -1) {
                    ids = elt.id;
                }
            }
            if (elt.type == 'text' && parseInt(elt.value) > parseInt(0)) {
                ProductID = ProductID + elt.id.replace('txtmayQty-', '') + ',';
                var priceid = elt.id.replace('txtmayQty', 'hdnmayprice')
                hdnprice = hdnprice + ',' + document.getElementById(priceid).value;
                quantity = quantity + ',' + elt.value;
                iCount++;

            }

        }
    }

    if (iCount > 0) {
        if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
            $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
        }

        Values = urlencode(Values);
        Names = urlencode(Names);
        var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=InsertMulti&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + ProductID + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertMultiProduct; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
        if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
            $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._startTimer();
        }
        return true;
    }
    else {
        jAlert("Please Enter Valid Quantity. !!!", "Message", ids);
        return false;
    }
    return true;
}


function HandleResponseforInsertMultiProduct() {
    if (XmlHttp.readyState == 4) {
        if (document.getElementById('pnlUpdate'))
        { document.getElementById('pnlUpdate').style.display = "none"; }
        if (XmlHttp.status == 200) {
            var result = XmlHttp.responseText; if (result.toLowerCase().indexOf('not sufficient inventory') != -1) {
                alert("Not enough Inventory..."); if (Alltext != '') { document.getElementById("divMiniCart").innerHTML = Alltext; } if (document.getElementById(ids))
                    document.getElementById(ids).focus(); return false;
            }
            document.getElementById("divCart").innerHTML = ""; document.getElementById("divCart").innerHTML = result; if (document.getElementById('divMiniCart'))
            { showLayer('divMiniCart'); }
            SetTotalQuantity(); jQuery().scrollTo({ top: '0px', left: '00px' }, 1000); if (eleForTransfer != null)
            { jQuery(eleForTransfer).show("transfer", options, 1000, showMiniCart); eleForTransfer = null; }
            else
                setTimeout("showMiniCart()", 1000);
        }
        else
        { alert("There was a problem retrieving data from the server."); }
    }
}

function InsertProductWishlist(Pid, eleclicked) {
    var Names = ""; var Values = ""; var vprice = 0;
    if (document.getElementById('divVariant')) {
        var allselect = document.getElementById('divVariant').getElementsByTagName('select');

        for (var iS = 0; iS < allselect.length; iS++) {
            var eltSelect = allselect[iS];
            if (eltSelect.selectedIndex == 0) {
                jAlert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!", "Message", eltSelect.id);
                //alert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!");
                return false;
            }
            else {
                var nametext = eltSelect.id.replace('Selectvariant-', 'divvariantname-');
                Names = Names + document.getElementById(nametext).innerHTML.replace(/,/g, ' ') + ',';
                Values = Values + eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ') + ',';

                if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('(+$') > -1) {
                    var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('(+$') + 3);
                    vtemp = vtemp.replace(/\)/g, '');
                    vprice = parseFloat(vprice) + parseFloat(vtemp);
                    document.getElementById('ContentPlaceHolder1_hdnVariantValueId').value = Values;
                    document.getElementById('ContentPlaceHolder1_hdnVariantNameId').value = Names;
                    document.getElementById('ContentPlaceHolder1_hdnVariantPrice').value = vprice;
                }
            }
        }
    }

    if (document.getElementById('ContentPlaceHolder1_txtQty')) {
        if (document.getElementById('ContentPlaceHolder1_txtQty').value <= 0 || document.getElementById('ContentPlaceHolder1_txtQty').value == '' || isNaN(document.getElementById('ContentPlaceHolder1_txtQty').value))
        { jAlert("Please enter valid digits only !!!", "Message", "ContentPlaceHolder1_txtQty"); document.getElementById('ContentPlaceHolder1_txtQty').value = '1'; document.getElementById('ContentPlaceHolder1_txtQty').focus(); return false; }
    }
    if (document.getElementById('OAData') != null) {
        var allrelatedproduct = document.getElementById('OAData').getElementsByTagName('INPUT');

        for (iP = 0; iP < allrelatedproduct.length; iP++) {
            var elt = allrelatedproduct[iP];
            if (elt.type == 'checkbox' && elt.checked == true) {
                var chkIds = elt.id.replace('ckhOASelect', 'txtOAQty');
                if (document.getElementById(chkIds)) {
                    if (document.getElementById(chkIds).value <= 0 || document.getElementById(chkIds).value == '' || isNaN(document.getElementById(chkIds).value))
                    { jAlert("Please enter valid digits only !!!", "Message", chkIds); document.getElementById(chkIds).value = '1'; document.getElementById(chkIds).focus(); return false; }

                }
            }
        }
    }
    return true;
}

function InsertProductCustom(Pid, eleclicked) {

    var Names = ""; var Values = ""; var vprice = 0;

    if (document.getElementById('divVariantcustom')) {
        var allselect = document.getElementById('divVariantcustom').getElementsByTagName('select');

        for (var iS = 0; iS < allselect.length; iS++) {
            var eltSelect = allselect[iS];
            if (eltSelect.selectedIndex == 0) {
                jAlert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!", "Message", eltSelect.id);
                //alert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!");
                //var nametext = eltSelect.id.replace('Selectvariant-', '');
               
                return false;
            }
            else {
                var nametext = eltSelect.id.replace('Selectvariantcustom-', 'divvariantnamecustom-');
                Names = Names + document.getElementById(nametext).innerHTML.replace(/,/g, ' ') + ',';
                Values = Values + eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').replace(/\+/g, '%2B') + ',';
                if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('(+$') > -1) {
                    var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('(+$') + 3);
                    vtemp = vtemp.replace(/\)/g, '');
                    vprice = parseFloat(vprice) + parseFloat(vtemp);

                }

            }
        }
    }
    if (document.getElementById('divselvaluecustom-1') != null) {
        document.getElementById('divselvaluecustom-1').innerHTML = document.getElementById('ContentPlaceHolder1_ddlcustomstyle').options[document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex].text;
    }
    if (document.getElementById('divselvaluecustom-2') != null) {
        document.getElementById('divselvaluecustom-2').innerHTML = document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex].text;
    }
    if (document.getElementById('divselvaluecustom-3') != null) {
        document.getElementById('divselvaluecustom-3').innerHTML = document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex].text;
    }
    if (document.getElementById('divselvaluecustom-4') != null && document.getElementById('ContentPlaceHolder1_ddlcustomoptin') != null) {
        document.getElementById('divselvaluecustom-4').innerHTML = document.getElementById('ContentPlaceHolder1_ddlcustomoptin').options[document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex].text;
    }
    if (document.getElementById('divselvaluecustom-5') != null) {
        document.getElementById('divselvaluecustom-5').innerHTML = document.getElementById('ContentPlaceHolder1_dlcustomqty').options[document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex].text;
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex == 0) {

        jAlert("Please Select Header", "Message", 'ContentPlaceHolder1_ddlcustomstyle');
        varianttabhideshowcustom(1);
        if (document.getElementById('divselvaluecustom-1') != null) {
            document.getElementById('divselvaluecustom-1').innerHTML = '';
        }
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex == 0) {

        jAlert("Please Select Width", "Message", 'ContentPlaceHolder1_ddlcustomwidth');
        varianttabhideshowcustom(2);
        if (document.getElementById('divselvaluecustom-2') != null) {
            document.getElementById('divselvaluecustom-2').innerHTML = '';
        }

        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex == 0) {

        jAlert("Please Select Length", "Message", 'ContentPlaceHolder1_ddlcustomlength');
        varianttabhideshowcustom(2);
        if (document.getElementById('divselvaluecustom-3') != null) {
            document.getElementById('divselvaluecustom-3').innerHTML = '';
        }
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlcustomoptin') != null && document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex == 0) {

        jAlert("Please Select Options", "Message", 'ContentPlaceHolder1_ddlcustomoptin');
        varianttabhideshowcustom(3);
        if (document.getElementById('divselvaluecustom-4') != null) {
            document.getElementById('divselvaluecustom-4').innerHTML = '';
        }
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex == 0) {

        jAlert("Please Select Quantity", "Message", 'ContentPlaceHolder1_dlcustomqty');
        varianttabhideshowcustom(4);
        if (document.getElementById('divselvaluecustom-5') != null) {
            document.getElementById('divselvaluecustom-5').innerHTML = '';
        }
        return false;
    }
    else {
        Names = Names + document.getElementById('ContentPlaceHolder1_ddlcustomstyle').options[0].text.replace(/,/g, ' ') + ',';
        Values = Values + document.getElementById('ContentPlaceHolder1_ddlcustomstyle').options[document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex].value.replace(/,/g, ' ') + ',';

        Names = Names + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[0].text.replace(/,/g, ' ') + ',';
        Values = Values + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex].value.replace(/,/g, ' ') + ',';

        Names = Names + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[0].text.replace(/,/g, ' ') + ',';
        Values = Values + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex].value.replace(/,/g, ' ') + ',';
        if (document.getElementById('ContentPlaceHolder1_ddlcustomoptin') != null) {


            Names = Names + document.getElementById('ContentPlaceHolder1_ddlcustomoptin').options[0].text.replace(/,/g, ' ') + ',';
            Values = Values + document.getElementById('ContentPlaceHolder1_ddlcustomoptin').options[document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex].value.replace(/,/g, ' ') + ',';
        }


    }




    Alltext = '';
    if (document.getElementById("divMiniCart")) {
        Alltext = document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid; if (document.getElementById('ContentPlaceHolder1_dlcustomqty'))
    { quantity = document.getElementById('ContentPlaceHolder1_dlcustomqty').options[document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex].value; }
    if (document.getElementById('txtQty'))
    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    if (document.getElementById('ContentPlaceHolder1_hdnpricecustomcart'))
    { hdnprice = document.getElementById('ContentPlaceHolder1_hdnpricecustomcart').value; }
    hdnprice = parseFloat(hdnprice) + parseFloat(vprice);
    hdnprice = parseFloat(hdnprice) / parseFloat(quantity);

    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
    }

    Values = urlencode(Values);
    Names = urlencode(Names);
    var randomnumber = Math.floor(Math.random() * 10000); 
    var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + Pid + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values + "&ProductType=2"; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProduct; XmlHttp.open("GET",  requestUrl, true); XmlHttp.send(null);
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._startTimer();
    }
    return true;

}
function InsertProductCustomQuick(Pid, eleclicked) {

    var Names = ""; var Values = ""; var vprice = 0;

    if (document.getElementById('divVariantcustom')) {
        var allselect = document.getElementById('divVariantcustom').getElementsByTagName('select');

        for (var iS = 0; iS < allselect.length; iS++) {
            var eltSelect = allselect[iS];
            if (eltSelect.selectedIndex == 0) {
                jAlert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!", "Message", eltSelect.id);
                //alert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!");
                return false;
            }
            else {
                var nametext = eltSelect.id.replace('Selectvariantcustom-', 'divvariantnamecustom-');
                Names = Names + document.getElementById(nametext).innerHTML.replace(/,/g, ' ') + ',';
                Values = Values + eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').replace(/\+/g, '%2B') + ',';
                if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('(+$') > -1) {
                    var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('(+$') + 3);
                    vtemp = vtemp.replace(/\)/g, '');
                    vprice = parseFloat(vprice) + parseFloat(vtemp);

                }

            }
        }
    }

    if (document.getElementById('ddlcustomstyle').selectedIndex == 0) {

        jAlert("Please Select Header", "Message", 'ddlcustomstyle');

        return false;
    }
    else if (document.getElementById('ddlcustomwidth').selectedIndex == 0) {

        jAlert("Please Select Width", "Message", 'ddlcustomwidth');

        return false;
    }
    else if (document.getElementById('ddlcustomlength').selectedIndex == 0) {

        jAlert("Please Select Length", "Message", 'ddlcustomlength');

        return false;
    }
    else if (document.getElementById('ddlcustomoptin') !=null && document.getElementById('ddlcustomoptin').selectedIndex == 0) {

        jAlert("Please Select Options", "Message", 'ddlcustomoptin');

        return false;
    }
    else if (document.getElementById('dlcustomqty').selectedIndex == 0) {

        jAlert("Please Select Quantity", "Message", 'dlcustomqty');

        return false;
    }
    else {
        Names = Names + document.getElementById('ddlcustomstyle').options[0].text.replace(/,/g, ' ') + ',';
        Values = Values + document.getElementById('ddlcustomstyle').options[document.getElementById('ddlcustomstyle').selectedIndex].value.replace(/,/g, ' ') + ',';

        Names = Names + document.getElementById('ddlcustomwidth').options[0].text.replace(/,/g, ' ') + ',';
        Values = Values + document.getElementById('ddlcustomwidth').options[document.getElementById('ddlcustomwidth').selectedIndex].value.replace(/,/g, ' ') + ',';

        Names = Names + document.getElementById('ddlcustomlength').options[0].text.replace(/,/g, ' ') + ',';
        Values = Values + document.getElementById('ddlcustomlength').options[document.getElementById('ddlcustomlength').selectedIndex].value.replace(/,/g, ' ') + ',';
        if (document.getElementById('ddlcustomoptin') != null) {


            Names = Names + document.getElementById('ddlcustomoptin').options[0].text.replace(/,/g, ' ') + ',';
            Values = Values + document.getElementById('ddlcustomoptin').options[document.getElementById('ddlcustomoptin').selectedIndex].value.replace(/,/g, ' ') + ',';
        }


    }




    Alltext = '';
    if (window.parent.document.getElementById("divMiniCart")) {
        Alltext = window.parent.document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid; if (document.getElementById('dlcustomqty'))
    { quantity = document.getElementById('dlcustomqty').options[document.getElementById('dlcustomqty').selectedIndex].value; }
    //    if (document.getElementById('txtQty'))
    //    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    if (document.getElementById('hdnpricecustomcart'))
    { hdnprice = document.getElementById('hdnpricecustomcart').value; }
    hdnprice = parseFloat(hdnprice) + parseFloat(vprice);
    hdnprice = parseFloat(hdnprice) / parseFloat(quantity);

    Values = urlencode(Values);
    Names = urlencode(Names);
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + Pid + "&Quantity=" + quantity + "&VariantNames=" + Names + "&ProductType=2&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProductQuick; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    return true;
}
function InsertProductSwatch(Pid, eleclicked) {


    var Names = ""; var Values = "";
    if (document.getElementById('ContentPlaceHolder1_txtSwatchqty')) {
        if (document.getElementById('ContentPlaceHolder1_txtSwatchqty').value <= 0 || document.getElementById('ContentPlaceHolder1_txtSwatchqty').value == '' || isNaN(document.getElementById('ContentPlaceHolder1_txtSwatchqty').value))
        { jAlert("Please enter valid digits only !!!", "Message", "ContentPlaceHolder1_txtSwatchqty"); document.getElementById('ContentPlaceHolder1_txtSwatchqty').value = '1'; document.getElementById('ContentPlaceHolder1_txtSwatchqty').focus(); return false; }
    }

    Alltext = '';
    if (document.getElementById("divMiniCart")) {
        Alltext = document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid; if (document.getElementById('ContentPlaceHolder1_txtSwatchqty'))
    { quantity = document.getElementById('ContentPlaceHolder1_txtSwatchqty').value; }
    if (document.getElementById('txtQty'))
    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    if (document.getElementById('ContentPlaceHolder1_hdnswatchprice')) {
        hdnprice = document.getElementById('ContentPlaceHolder1_hdnswatchprice').value;        
     }
    
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
    }

    Values = urlencode(Values);
    Names = urlencode(Names);
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + Pid + "&Quantity=" + quantity + "&VariantNames=" + Names + "&ProductType=2&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProductQuickSwatch; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._startTimer();
    }
    return true;

}
function InsertProductOrderSwatchAddtocartfree(Pid, eleclicked) {
    var Names = ""; var Values = "";
    //    if (document.getElementById('txtSwatchqty')) {
    //        if (document.getElementById('txtSwatchqty').value <= 0 || document.getElementById('txtSwatchqty').value == '' || isNaN(document.getElementById('txtSwatchqty').value))
    //        { jAlert("Please enter valid digits only !!!", "Message", "txtSwatchqty"); document.getElementById('txtSwatchqty').value = '1'; document.getElementById('txtSwatchqty').focus(); return false; }
    //    }
    Alltext = '';
    if (window.parent.document.getElementById("divMiniCart")) {
        Alltext = window.parent.document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked);
    resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid;

    //     if (document.getElementById('txtSwatchqty'))
    //    { quantity = document.getElementById('txtSwatchqty').value; }
    //    if (document.getElementById('txtQty'))
    //    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    var hdnva = eleclicked.replace('_imgAddToCart_', '_hdnswatchprice_');


    
    Values = urlencode(Values);
    Names = urlencode(Names);
    if (document.getElementById(hdnva))
    { hdnprice = document.getElementById(hdnva).value; }
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + Pid + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProductQuickSwatch; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    
    return true;
}

function InsertProductOrderSwatchAddtocart(Pid, eleclicked) {
    var Names = ""; var Values = "";
    //    if (document.getElementById('txtSwatchqty')) {
    //        if (document.getElementById('txtSwatchqty').value <= 0 || document.getElementById('txtSwatchqty').value == '' || isNaN(document.getElementById('txtSwatchqty').value))
    //        { jAlert("Please enter valid digits only !!!", "Message", "txtSwatchqty"); document.getElementById('txtSwatchqty').value = '1'; document.getElementById('txtSwatchqty').focus(); return false; }
    //    }
    Alltext = '';
    if (window.parent.document.getElementById("divMiniCart")) {
        Alltext = window.parent.document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked);
    resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid;

    //     if (document.getElementById('txtSwatchqty'))
    //    { quantity = document.getElementById('txtSwatchqty').value; }
    //    if (document.getElementById('txtQty'))
    //    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    var hdnva = eleclicked.replace('_aorderswatch_', '_hdnswatchprice_');
    
    

    Values = urlencode(Values);
    Names = urlencode(Names);
    if (document.getElementById(hdnva))
    { hdnprice = document.getElementById(hdnva).value; }
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + Pid + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProductQuickSwatch; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    return true;
}


function InsertProductSwatchQuick(Pid, eleclicked) {


    var Names = ""; var Values = "";
    if (document.getElementById('txtSwatchqty')) {
        if (document.getElementById('txtSwatchqty').value <= 0 || document.getElementById('txtSwatchqty').value == '' || isNaN(document.getElementById('txtSwatchqty').value))
        { jAlert("Please enter valid digits only !!!", "Message", "txtSwatchqty"); document.getElementById('txtSwatchqty').value = '1'; document.getElementById('txtSwatchqty').focus(); return false; }
    }

    Alltext = '';
    if (window.parent.document.getElementById("divMiniCart")) {
        Alltext = window.parent.document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid; if (document.getElementById('txtSwatchqty'))
    { quantity = document.getElementById('txtSwatchqty').value; }
    else if (document.getElementById('txtQty'))
    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    if (document.getElementById('hdnswatchprice'))
    { hdnprice = document.getElementById('hdnswatchprice').value; }

    Values = urlencode(Values);
    Names = urlencode(Names);
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + Pid + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProductQuickSwatch; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    return true;
}
function InsertProductQuickview(Pid, eleclicked) {


    var Names = ""; var Values = ""; var vprice = 0;
    if (document.getElementById('divVariant')) {
        var allselect = document.getElementById('divVariant').getElementsByTagName('select');

        for (var iS = 0; iS < allselect.length; iS++) {
            var eltSelect = allselect[iS];
            if (eltSelect.selectedIndex == 0) {
                jAlert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!", "Message", eltSelect.id);
                //alert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!");
                return false;
            }
            else {
                var nametext = eltSelect.id.replace('Selectvariant-', 'divvariantname-');
                Names = Names + document.getElementById(nametext).innerHTML.replace(/,/g, ' ') + ',';
                Values = Values + eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').replace(/\+/g, '%2B') + ',';

                if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('($') > -1) {

                    var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('($') + 2);

                    vtemp = vtemp.replace(/\)/g, '');
                    vprice = parseFloat(vprice) + parseFloat(vtemp);
                }
            }
        }
    }

    if (document.getElementById('txtQty')) {
        if (document.getElementById('txtQty').value <= 0 || document.getElementById('txtQty').value == '' || isNaN(document.getElementById('txtQty').value))
        { jAlert("Please enter valid digits only !!!", "Message", "txtQty"); document.getElementById('txtQty').value = '1'; document.getElementById('txtQty').focus(); return false; }
    }


    Alltext = '';
    if (window.parent.document.getElementById("divMiniCart")) {
        Alltext = window.parent.document.getElementById("divMiniCart").innerHTML;
    }


    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); var quantity = 1; var ProductID = Pid; if (document.getElementById('txtQty'))
    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('txtQty'))
    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    if (document.getElementById('hdnprice'))
    { hdnprice = document.getElementById('hdnprice').value; }

    if (parseFloat(vprice) > 0) {
        hdnprice = parseFloat(vprice);
    }
    else {
        //hdnprice = parseFloat(hdnprice) + parseFloat(vprice);
        hdnprice = parseFloat(hdnprice);
    }

    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
    }
    Values = urlencode(Values);
    Names = urlencode(Names);
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + ProductID + "&Quantity=" + quantity + "&VariantNames=" + Names + "&ProductType=1&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProductQuick; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._startTimer();
    }
    return true;

}
function InsertProduct(Pid, eleclicked) {


    var Names = ""; var Values = ""; var vprice = 0;
    if (document.getElementById('divVariant')) {
        var allselect = document.getElementById('divVariant').getElementsByTagName('select');

        for (var iS = 0; iS < allselect.length; iS++) {
            var eltSelect = allselect[iS];
            if (eltSelect.selectedIndex == 0) {
                jAlert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!", "Message", eltSelect.id);
                //alert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!");

                var nametext = eltSelect.id.replace('Selectvariant-', '');
                
                varianttabhideshow(nametext);
                return false;
            }
            else {
                var nametext = eltSelect.id.replace('Selectvariant-', 'divvariantname-');
                Names = Names + document.getElementById(nametext).innerHTML.replace(/,/g, ' ') + ',';
                Values = Values + eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').replace(/\+/g, '%2B') + ',';

                if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('($') > -1) {

                    var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('($') + 2);

                    vtemp = vtemp.replace(/\)/g, '');
                    vprice = parseFloat(vprice) + parseFloat(vtemp);
                }
            }
        }
    }

    if (document.getElementById('ContentPlaceHolder1_txtQty')) {
        if (document.getElementById('ContentPlaceHolder1_txtQty').value <= 0 || document.getElementById('ContentPlaceHolder1_txtQty').value == '' || isNaN(document.getElementById('ContentPlaceHolder1_txtQty').value))
        { jAlert("Please enter valid digits only !!!", "Message", "ContentPlaceHolder1_txtQty"); document.getElementById('ContentPlaceHolder1_txtQty').value = '0'; document.getElementById('ContentPlaceHolder1_txtQty').focus(); return false; }
    }

    Alltext = '';
    if (document.getElementById("divMiniCart")) {
        Alltext = document.getElementById("divMiniCart").innerHTML;
    }

    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid; if (document.getElementById('ContentPlaceHolder1_txtQty'))
    { quantity = document.getElementById('ContentPlaceHolder1_txtQty').value; }
    if (document.getElementById('txtQty'))
    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    if (document.getElementById('ContentPlaceHolder1_hdnprice'))
    { hdnprice = document.getElementById('ContentPlaceHolder1_hdnprice').value; }
    if (parseFloat(vprice) > 0) {
        hdnprice = parseFloat(vprice);
    }
    else {
        //hdnprice = parseFloat(hdnprice) + parseFloat(vprice);
        hdnprice = parseFloat(hdnprice);
    }

    if (document.getElementById('OAData') != null) {
        var allrelatedproduct = document.getElementById('OAData').getElementsByTagName('INPUT');

        for (iP = 0; iP < allrelatedproduct.length; iP++) {
            var elt = allrelatedproduct[iP];
            if (elt.type == 'checkbox' && elt.checked == true) {
                var chkIds = elt.id.replace('ckhOASelect', 'txtOAQty');
                if (document.getElementById(chkIds)) {
                    if (document.getElementById(chkIds).value <= 0 || document.getElementById(chkIds).value == '' || isNaN(document.getElementById(chkIds).value))
                    { jAlert("Please enter valid digits only !!!", "Message", chkIds); document.getElementById(chkIds).value = '1'; document.getElementById(chkIds).focus(); return false; }
                    var chkIdRegularPrice = elt.id.replace('ckhOASelect', 'hdnOARegularPrice');
                    var chkIdSalePrice = elt.id.replace('ckhOASelect', 'hdnOASalePrice');
                    var chkIdProductId = elt.id.replace('ckhOASelect', 'hdnOAProductID');
                    //                      
                    var sprice = parseFloat((document.getElementById(chkIdSalePrice).value).replace(/^\s*\s*$/g, '')).toFixed(2);
                    sprice = sprice;
                    var itemprice = parseFloat((document.getElementById(chkIdRegularPrice).value).replace(/^\s*\s*$/g, '')).toFixed(2);
                    itemprice = itemprice;
                    if (parseFloat(sprice) > parseFloat(itemprice)) {
                        hdnprice = hdnprice + ',' + itemprice;
                    }
                    else if (parseFloat(sprice) != parseFloat(0)) {
                        hdnprice = hdnprice + ',' + sprice;
                    }
                    else {
                        hdnprice = hdnprice + ',' + itemprice;
                    }

                    ProductID = ProductID + ',' + document.getElementById(chkIdProductId).value;
                    quantity = quantity + ',' + document.getElementById(chkIds).value;
                }
            }
        }
    }
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
    }

    Values = urlencode(Values);
    Names = urlencode(Names);
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + ProductID + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values + "&ProductType=1"; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProduct; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._startTimer();
    }
    return true;

}


function InsertProductRoman(Pid, eleclicked) {


    var Names = ""; var Values = ""; var vprice = 0;
    if (document.getElementById('divVariant')) {
        var allselect = document.getElementById('divVariant').getElementsByTagName('select');

        for (var iS = 0; iS < allselect.length; iS++) {
            var eltSelect = allselect[iS];
            if (eltSelect.selectedIndex == 0) {
                jAlert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!", "Message", eltSelect.id);
                //alert("Please " + eltSelect.options[eltSelect.selectedIndex].text + " !!!");
                var nametext = eltSelect.id.replace('Selectvariant-', '');

                varianttabhideshow(nametext);
                return false;
            }
            else {
                var nametext = eltSelect.id.replace('Selectvariant-', 'divvariantname-');
                if (document.getElementById(nametext) != null) {

                    if (document.getElementById(nametext).innerHTML.indexOf('<span>') > -1) {

                        Names = Names + document.getElementById(nametext).innerHTML.toString().substring(0, document.getElementById(nametext).innerHTML.toString().indexOf('<span>')).replace(/,/g, ' ') + ',';
                    }
                    else {
                        Names = Names + document.getElementById(nametext).innerHTML.replace(/,/g, ' ') + ',';
                    }
                    Values = Values + eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').replace(/\+/g, '%2B') + ',';
                    if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('(+$') > -1) {

                        var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('(+$') + 3);

                        vtemp = vtemp.replace(/\)/g, '');
                        vprice = parseFloat(vprice) + parseFloat(vtemp);
                    }
                }
            }
        }
    }

    if (document.getElementById('ContentPlaceHolder1_txtQty')) {
        if (document.getElementById('ContentPlaceHolder1_txtQty').value <= 0 || document.getElementById('ContentPlaceHolder1_txtQty').value == '' || isNaN(document.getElementById('ContentPlaceHolder1_txtQty').value))
        { jAlert("Please enter valid digits only !!!", "Message", "ContentPlaceHolder1_txtQty"); document.getElementById('ContentPlaceHolder1_txtQty').value = '1'; document.getElementById('ContentPlaceHolder1_txtQty').focus(); return false; }
    }
    Alltext = '';
    if (document.getElementById("divMiniCart")) {
        Alltext = document.getElementById("divMiniCart").innerHTML;
    }
    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid; if (document.getElementById('ContentPlaceHolder1_txtQty'))
    { quantity = document.getElementById('ContentPlaceHolder1_txtQty').value; }
    if (document.getElementById('txtQty'))
    { quantity = document.getElementById('txtQty').value; }
    if (document.getElementById('ProductID'))
    { ProductID = document.getElementById('ProductID').value; }
    var allElts; var i; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }

    if (document.getElementById('ContentPlaceHolder1_hdnprice'))
    { hdnprice = document.getElementById('ContentPlaceHolder1_hdnprice').value; }
    hdnprice = parseFloat(hdnprice) + parseFloat(vprice);

    if (document.getElementById('OAData') != null) {
        var allrelatedproduct = document.getElementById('OAData').getElementsByTagName('INPUT');

        for (iP = 0; iP < allrelatedproduct.length; iP++) {
            var elt = allrelatedproduct[iP];
            if (elt.type == 'checkbox' && elt.checked == true) {
                var chkIds = elt.id.replace('ckhOASelect', 'txtOAQty');
                if (document.getElementById(chkIds)) {
                    if (document.getElementById(chkIds).value <= 0 || document.getElementById(chkIds).value == '' || isNaN(document.getElementById(chkIds).value))
                    { jAlert("Please enter valid digits only !!!", "Message", chkIds); document.getElementById(chkIds).value = '1'; document.getElementById(chkIds).focus(); return false; }
                    var chkIdRegularPrice = elt.id.replace('ckhOASelect', 'hdnOARegularPrice');
                    var chkIdSalePrice = elt.id.replace('ckhOASelect', 'hdnOASalePrice');
                    var chkIdProductId = elt.id.replace('ckhOASelect', 'hdnOAProductID');
                    //                      
                    var sprice = parseFloat((document.getElementById(chkIdSalePrice).value).replace(/^\s*\s*$/g, '')).toFixed(2);
                    sprice = sprice;
                    var itemprice = parseFloat((document.getElementById(chkIdRegularPrice).value).replace(/^\s*\s*$/g, '')).toFixed(2);
                    itemprice = itemprice;
                    if (parseFloat(sprice) > parseFloat(itemprice)) {
                        hdnprice = hdnprice + ',' + itemprice;
                    }
                    else if (parseFloat(sprice) != parseFloat(0)) {
                        hdnprice = hdnprice + ',' + sprice;
                    }
                    else {
                        hdnprice = hdnprice + ',' + itemprice;
                    }
                    ProductID = ProductID + ',' + document.getElementById(chkIdProductId).value;
                    quantity = quantity + ',' + document.getElementById(chkIds).value;
                }
            }
        }
    }
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
    }
    Values = urlencode(Values);
    Names = urlencode(Names);
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + ProductID + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values + "&ProductType=3"; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProduct; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null);
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._startTimer();
    }
    return true;

}

function InsertProductSubcategory(Pid, eleclicked) {

    Alltext = '';
    if (document.getElementById("divMiniCart")) {
        Alltext = document.getElementById("divMiniCart").innerHTML;
    }

    arrayPageSizeForPopup = getPageSizeForPopup(); arrayPageScrollForPopup = getPageScrollForPopup(); var offsetY = arrayPageScrollForPopup[1] + (arrayPageSizeForPopup[3] / 2); var offsetX = (arrayPageSizeForPopup[2] / 2); if (document.getElementById('pnlUpdate'))
    { document.getElementById('pnlUpdate').style.left = offsetX + 'px'; document.getElementById('pnlUpdate').style.top = offsetY + 'px'; document.getElementById('pnlUpdate').style.display = "block"; }
    eleForTransfer = document.getElementById(eleclicked); resetHover(); hideMiniCart(); var quantity = 1; var ProductID = Pid;


    var allElts; var i; var Names = ""; var Values = ""; var hdnprice = "";
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }
    if (document.getElementById('hiddenCustID'))
    { Custid = document.getElementById('hiddenCustID').value; }


    var chkIdRegularPrice = eleclicked.replace('aProductlink', 'hdnRegularPrice');
    var chkIdSalePrice = eleclicked.replace('aProductlink', 'hdnYourPrice');

    var sprice = parseFloat((document.getElementById(chkIdSalePrice).value).replace(/^\s*\s*$/g, '')).toFixed(2);
    sprice = sprice;
    var itemprice = parseFloat((document.getElementById(chkIdRegularPrice).value).replace(/^\s*\s*$/g, '')).toFixed(2);
    itemprice = itemprice;
    if (parseFloat(sprice) > parseFloat(itemprice)) {
        hdnprice = itemprice;
    }
    else if (parseFloat(sprice) != parseFloat(0)) {
        hdnprice = sprice;
    }
    else {
        hdnprice = itemprice;
    }

    Values = urlencode(Values);
    Names = urlencode(Names);
    var randomnumber = Math.floor(Math.random() * 10000); var requestUrl = "/MiniCartCall.aspx?Mode=Insert&RandomNum=" + randomnumber + "&Price=" + hdnprice + "&ProdID=" + ProductID + "&Quantity=" + quantity + "&VariantNames=" + Names + "&VariantValues=" + Values; CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProduct; XmlHttp.open("GET", requestUrl, true); XmlHttp.send(null); return true;
}

function HandleResponseforInsertProduct() {
    if (XmlHttp.readyState == 4) {
        if (document.getElementById('pnlUpdate'))
        { document.getElementById('pnlUpdate').style.display = "none"; }
        if (XmlHttp.status == 200) {
            var result = XmlHttp.responseText;
             if (result.toLowerCase().indexOf('not sufficient inventory') != -1) {
                 //alert("Not enough Inventory..."); 
                ShowInventoryMessage(result); if (Alltext != '') { document.getElementById("divMiniCart").innerHTML = Alltext; } if (document.getElementById("txtQty"))
                    document.getElementById("txtQty").focus(); return false;
            }
            document.getElementById("divCart").innerHTML = ""; document.getElementById("divCart").innerHTML = result; if (document.getElementById('divMiniCart'))
            { showLayer('divMiniCart'); }
            SetTotalQuantity(); if (jQuery().scrollTo != null) { jQuery().scrollTo({ top: '0px', left: '00px' }, 1000); } else { window.scrollTo(0, 0); showMiniCart(); } if (eleForTransfer != null)
            //SetTotalQuantity(); jQuery().scrollTo({ top: '0px', left: '00px' }, 1000); if (eleForTransfer != null)
            { jQuery(eleForTransfer).show("transfer", options, 1000, showMiniCart); eleForTransfer = null; }
            else
                setTimeout("showMiniCart()", 1000);
        }
        else
        { alert("There was a problem retrieving data from the server."); }
    }
}

function HandleResponseforInsertProductQuickSwatch() {
    if (XmlHttp.readyState == 4) {
        var maxlength = 0;
        if (document.getElementById('ContentPlaceHolder1_hdnswatchmaxlength')) {
            maxlength = document.getElementById('ContentPlaceHolder1_hdnswatchmaxlength').value;
        }
        if (XmlHttp.status == 200) {
            var result = XmlHttp.responseText; if (result.toLowerCase().indexOf('not sufficient inventory') != -1) {
                //alert("Not enough Inventory..."); 
                ShowInventoryMessage(result);
                if (Alltext != '') { window.parent.document.getElementById("divMiniCart").innerHTML = Alltext; } if (document.getElementById("txtQty"))
                    document.getElementById("txtQty").focus(); return false;
            }
            window.parent.document.getElementById("divCart").innerHTML = ""; window.parent.document.getElementById("divCart").innerHTML = result; if (window.parent.document.getElementById('divMiniCart')) {
                // showLayerQuick('divMiniCart'); 
                //alert('You have added order swatch successfully. \n Max. limit(' + maxlength + ')');
                ShowSwatchMessage();
            }
            SetTotalQuantityquick();
            //showMiniCartQuick(); 
            // window.parent.scrollTo(0, 0); 
            //window.parent.disablePopup();

            if (eleForTransfer != null)
            { eleForTransfer = null; }
            else {
                setTimeout("showMiniCartQuick()", 1000);
                //alert('You have added        order swatch successfully. \n Order Swatch Max. limit is ' + maxlength + '');
                //document.getElementById('divswatchinfo').innerHTML=
                ShowSwatchMessage();

            }
        }
        else
        { alert("There was a problem retrieving data from the server."); }
    }
}

function HandleResponseforInsertProductQuick() {
    if (XmlHttp.readyState == 4) {

        if (XmlHttp.status == 200) {
            var result = XmlHttp.responseText; if (result.toLowerCase().indexOf('not sufficient inventory') != -1) {
                //alert("Not enough Inventory...");
                ShowInventoryMessage(result);
                 if (Alltext != '') { window.parent.document.getElementById("divMiniCart").innerHTML = Alltext; } if (document.getElementById("txtQty"))
                    document.getElementById("txtQty").focus(); return false;
            }
            window.parent.document.getElementById("divCart").innerHTML = ""; window.parent.document.getElementById("divCart").innerHTML = result; if (window.parent.document.getElementById('divMiniCart'))
            { showLayerQuick('divMiniCart'); }
            SetTotalQuantityquick(); showMiniCartQuick(); window.parent.scrollTo(0, 0); window.parent.disablePopup(); if (eleForTransfer != null)
            { eleForTransfer = null; }
            else
                setTimeout("showMiniCartQuick()", 1000);
        }
        else
        { alert("There was a problem retrieving data from the server."); }
    }
}

function onKeyPressBlockNumbers(e) {
    var key = window.event ? window.event.keyCode : e.which; if (key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0)
    { return key; }
    var keychar = String.fromCharCode(key); var reg = /\d/; if (window.event)
        return event.returnValue = reg.test(keychar); else
        return reg.test(keychar);
}
function CalculateSalePrice(CurrencySymbol) {
    try {
        var AttributePricesforItem = 0; var AttributePrices = 0; var i; var allElts; allElts = document.getElementsByTagName("select"); for (i = 0; i < allElts.length; i++) {
            var elt = allElts[i]; if (elt.type == 'select-one' && elt.id.toLowerCase().indexOf('selattr') != -1) {
                if (elt.options[elt.selectedIndex].text.toLowerCase().indexOf('select ') == -1)
                { var temp = "(" + CurrencySymbol; var TempNames = String(elt.options[elt.selectedIndex].text).split(temp); AttributePrices = AttributePrices + parseFloat(String(TempNames[1]).replace(')', '')); }
            }
            if (elt.type == 'select-one' && elt.id.toLowerCase().indexOf('selattribute') != -1) {
                if (elt.options[elt.selectedIndex].text.toLowerCase().indexOf('select ') == -1)
                { var temp = "(" + CurrencySymbol; var TempNames = String(elt.options[elt.selectedIndex].text).split(temp); AttributePricesforItem = AttributePricesforItem + parseFloat(String(TempNames[1]).replace(')', '')); }
            }
        }
        var SalePrice; if (document.getElementById("SalePrice"))
            SalePrice = roundNumber(String(parseFloat(document.getElementById("SalePrice").value.replace(CurrencySymbol, ''))), 2); if (document.getElementById("SalePriceforItem"))
            SalePrice = roundNumber(String(parseFloat(document.getElementById("SalePriceforItem").value.replace(CurrencySymbol, ''))), 2); var FinalAttributePrices; if (AttributePricesforItem == 0)
        { FinalAttributePrices = roundNumber(String(AttributePrices), 2); var FinalPrice = roundNumber(String(parseFloat(SalePrice) + parseFloat(FinalAttributePrices)), 2); document.getElementById("divSalePrice").innerHTML = CurrencySymbol + (FinalPrice); }
        else
        { FinalAttributePrices = roundNumber(String(AttributePricesforItem), 2); var FinalPrice = roundNumber(String(parseFloat(SalePrice) + parseFloat(FinalAttributePrices)), 2); document.getElementById("divSalePriceforItem").innerHTML = CurrencySymbol + (FinalPrice); }
    }
    catch (e)
{ }
}
function roundNumber(num, dec) { var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec); return result; }
function hideMiniCart() {
    if (resetCartHover == false)
        return; CartVisible = false; if (document.getElementById('CartLayer') != null)
    { fadingOut = true; jQuery(document.getElementById('CartLayer')).fadeOut('def', fadOutComplete); }
    clearTimeout(timevar);
}
function fadOutComplete()
{ fadingOut = false; }
function showMiniCart(optionalValue) {
    resetCartHover = false; if (CartVisible == true)
        return; if (fadingOut == true)
        return; optionalValue = optionalValue || 3000; clearTimeout(timevar); timevar = setTimeout("hideMiniCart()", optionalValue); if (document.getElementById('CartLayer') != null)
    { jQuery(document.getElementById('CartLayer')).fadeIn('def'); }
    CartVisible = true;
}
function showMiniCartQuick(optionalValue) {
    resetCartHover = false; if (CartVisible == true)
        return; if (fadingOut == true)
        return; optionalValue = optionalValue || 3000; clearTimeout(timevar); timevar = setTimeout("hideMiniCartQuick()", optionalValue); if (window.parent.document.getElementById('CartLayer') != null)
    { jQuery(window.parent.document.getElementById('CartLayer')).fadeIn('def'); }
    CartVisible = true;
}
function hideMiniCartQuick() {
    if (resetCartHover == false)
        return; CartVisible = false; if (window.parent.document.getElementById('CartLayer') != null)
    { fadingOut = true; jQuery(window.parent.document.getElementById('CartLayer')).fadeOut('def', fadOutComplete); }
    clearTimeout(timevar);
}
function resetHover()
{ resetCartHover = true; }
/*end*/