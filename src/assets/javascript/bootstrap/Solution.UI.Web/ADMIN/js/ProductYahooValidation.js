function ValidatePage() {

    var sprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtSalePrice').value)).toFixed(2);
    sprice = sprice;

    var itemprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtPrice').value)).toFixed(2);
    itemprice = itemprice;

    var ourPrice = parseFloat((document.getElementById('ContentPlaceHolder1_txtOurPrice').value)).toFixed(2);
    ourPrice = ourPrice;

    if ((document.getElementById('ContentPlaceHolder1_txtProductName').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Product Name', 'Message', 'ContentPlaceHolder1_txtProductName');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtProductName').offset().top }, 'slow');
        return false;
    }
    if ((document.getElementById('ContentPlaceHolder1_txtSKU').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter SKU', 'Message', 'ContentPlaceHolder1_txtSKU');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSKU').offset().top }, 'slow');
        return false;
    }
    if ((document.getElementById('ContentPlaceHolder1_txtYahooIDs').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter YahooID', 'Message', 'ContentPlaceHolder1_txtYahooIDs');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtYahooIDs').offset().top }, 'slow');
        return false;
    }
//    if (document.getElementById("ContentPlaceHolder1_ddlProductTypeDelivery") != null && document.getElementById("ContentPlaceHolder1_ddlProductTypeDelivery").selectedIndex == 0) {
//        jAlert('Please Select Product Delivery Type', 'Message', 'ContentPlaceHolder1_ddlProductTypeDelivery');
//        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlProductTypeDelivery').offset().top }, 'slow');
//        return false;
//    }
//    if (document.getElementById("ContentPlaceHolder1_ddlProductType") != null && document.getElementById("ContentPlaceHolder1_ddlProductType").selectedIndex == 0) {
//        jAlert('Please Select Product Type', 'Message', 'ContentPlaceHolder1_ddlProductType');
//        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlProductType').offset().top }, 'slow');
//        return false;
//    }


//    if ((document.getElementById('ContentPlaceHolder1_txtUPC').value).replace(/^\s*\s*$/g, '') == '') {
//        jAlert('Please Enter UPC', 'Message', 'ContentPlaceHolder1_txtUPC');
//        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtUPC').offset().top }, 'slow');
//        return false;
//    }
    if (document.getElementById("ContentPlaceHolder1_ddlManufacture") != null && document.getElementById("ContentPlaceHolder1_ddlManufacture").selectedIndex == 0) {
        jAlert('Please Select Manufacturer', 'Message', 'ContentPlaceHolder1_ddlManufacture');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlManufacture').offset().top }, 'slow');
        return false;
    }
    if ((document.getElementById('ContentPlaceHolder1_txtInventory').value).replace(/^\s*\s*$/g, '') == '') {
        if (document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_0') != null) {
            jAlert('Please Enter Inventory from Warehouse', 'Message', 'ContentPlaceHolder1_grdWarehouse_txtInventory_0');
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_grdWarehouse_txtInventory_0').offset().top }, 'slow');
        }
        else {
            jAlert('Please Enter Inventory from Warehouse', 'Message', 'ContentPlaceHolder1_txtInventory');
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtInventory').offset().top }, 'slow');
        }
        return false;
    }
    if ((document.getElementById('ContentPlaceHolder1_txtWeight').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Weight', 'Message', 'ContentPlaceHolder1_txtWeight');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtWeight').offset().top }, 'slow');
        return false;
    }
    if ((document.getElementById('ContentPlaceHolder1_txtPrice').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Price', 'Message', 'ContentPlaceHolder1_txtPrice');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtPrice').offset().top }, 'slow');
        return false;
    }

    if (parseFloat(itemprice) < parseFloat(sprice)) {

        jAlert('Sale Price should be less than Price', 'Message', 'ContentPlaceHolder1_txtSalePrice');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSalePrice').offset().top }, 'slow');
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
    if (document.getElementById("ContentPlaceHolder1_chkIsFreeEngraving") != null && document.getElementById("ContentPlaceHolder1_chkIsFreeEngraving").checked == true && document.getElementById("tdEngravingSize") != null && document.getElementById("ContentPlaceHolder1_txtEngravingSize").value == '') {

        jAlert('Please Enter Engraving Size', 'Message', 'ContentPlaceHolder1_txtEngravingSize');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtEngravingSize').offset().top }, 'slow');
        return false;
    }
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
function keyRestrictForOnlyNumeric(e, validchars) {
    var key = '', keychar = '';
    key = getKeyCode(e);
    if (key == null) return true;
    keychar = String.fromCharCode(key);
    keychar = keychar.toLowerCase();
    validchars = validchars.toLowerCase();
    if (validchars.indexOf(keychar) != -1)
        return true;
    if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
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
