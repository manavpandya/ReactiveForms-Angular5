function ValidatePage() {

    var sprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtListingPrice').value)).toFixed(2);
    sprice = sprice;

    var itemprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtMsrp').value)).toFixed(2);
    itemprice = itemprice;

    var ourPrice = parseFloat((document.getElementById('ContentPlaceHolder1_txtOurPrice').value)).toFixed(2);
    ourPrice = ourPrice;

    if ((document.getElementById('ContentPlaceHolder1_txtSellerID').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter SellerID', 'Message', 'ContentPlaceHolder1_txtSellerID');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSellerID').offset().top }, 'slow');
        return false;
    }

    if ((document.getElementById('ContentPlaceHolder1_txtGtin').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Gtin', 'Message', 'ContentPlaceHolder1_txtGtin');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtGtin').offset().top }, 'slow');
        return false;
    }

    if (document.getElementById('ContentPlaceHolder1_ddlManufacture').selectedIndex== 0) {
        jAlert('Please Select Manufacture Name', 'Message', 'ContentPlaceHolder1_ddlManufacture');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlManufacture').offset().top }, 'slow');
        return false;
    }

    if ((document.getElementById('ContentPlaceHolder1_txtManufactrPartNo').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Manufacture Part No', 'Message', 'ContentPlaceHolder1_txtManufactrPartNo');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtManufactrPartNo').offset().top }, 'slow');
        return false;
    }

    if ((document.getElementById('ContentPlaceHolder1_txtSKU').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter SKU', 'Message', 'ContentPlaceHolder1_txtSKU');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSKU').offset().top }, 'slow');
        return false;
    }

    if ((document.getElementById('ContentPlaceHolder1_txtSerialNo').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Serial No', 'Message', 'ContentPlaceHolder1_txtSerialNo');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSerialNo').offset().top }, 'slow');
        return false;
    }

    if ((document.getElementById('ContentPlaceHolder1_txtTitle').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Title', 'Message', 'ContentPlaceHolder1_txtTitle');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtTitle').offset().top }, 'slow');
        return false;
    }

    if ((document.getElementById('ContentPlaceHolder1_txtWeight').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Weight', 'Message', 'ContentPlaceHolder1_txtWeight');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtWeight').offset().top }, 'slow');
        return false;
    }

    if ((document.getElementById('ContentPlaceHolder1_txtMsrp').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Msrp', 'Message', 'ContentPlaceHolder1_txtMsrp');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtMsrp').offset().top }, 'slow');
        return false;
    }
    if (parseFloat(itemprice) < parseFloat(sprice)) {
        jAlert('Listing Price should be less than Msrp Price', 'Message', 'ContentPlaceHolder1_txtListingPrice');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtListingPrice').offset().top }, 'slow');
        return false;
    }
    if (parseFloat(sprice) > parseFloat(0) && parseFloat(sprice) < parseFloat(ourPrice)) {
        jAlert('Our Cost should be less than Sale Price', 'Message', 'ContentPlaceHolder1_txtOurPrice');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtOurPrice').offset().top }, 'slow');
        return false;
    }
    else if (parseFloat(itemprice) < parseFloat(ourPrice)) {
        jAlert('Our Cost should be less than Price', 'Message', 'ContentPlaceHolder1_txtOurPrice');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtOurPrice').offset().top }, 'slow');
        return false;
    }
    if ((document.getElementById('ContentPlaceHolder1_txtInventory').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Inventory', 'Message', 'ContentPlaceHolder1_txtInventory');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtInventory').offset().top }, 'slow');
        return false;
    }
    if (document.getElementById("ContentPlaceHolder1_ddlProductTypeDelivery") != null && document.getElementById("ContentPlaceHolder1_ddlProductTypeDelivery").selectedIndex == 0) {
        jAlert('Please Select Product Delivery Type', 'Message', 'ContentPlaceHolder1_ddlProductTypeDelivery');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlProductTypeDelivery').offset().top }, 'slow');
        return false;
    }
    if (document.getElementById("ContentPlaceHolder1_ddlProductType") != null && document.getElementById("ContentPlaceHolder1_ddlProductType").selectedIndex == 0) {
        jAlert('Please Select Product Type', 'Message', 'ContentPlaceHolder1_ddlProductType');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlProductType').offset().top }, 'slow');
        return false;
    }

//    if (document.getElementById('ContentPlaceHolder1_ddlCategory').selectedIndex == 0) {
//        jAlert('Please Select at least one Category', 'Message', 'ContentPlaceHolder1_ddlCategory');
//        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlCategory').offset().top }, 'slow');
//        return false;
//    }
    return true;
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