function ValidatePage() {
    var sprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtSalePrice').value)).toFixed(2);
    sprice = sprice;

    var itemprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtPrice').value)).toFixed(2);
    itemprice = itemprice;

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
    
    if ((document.getElementById('ContentPlaceHolder1_txtInventory').value).replace(/^\s*\s*$/g, '') == '') {
        if (document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_0') != null && document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_0').value == '') {
            jAlert('Please Enter Inventory from Warehouse', 'Message', 'ContentPlaceHolder1_grdWarehouse_txtInventory_0');
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_grdWarehouse_txtInventory_0').offset().top }, 'slow');
        }
        else {
            jAlert('Please Enter Inventory from Warehouse', 'Message', 'ContentPlaceHolder1_txtInventory');
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtInventory').offset().top }, 'slow');
        }
        return false;
    }
    //    if (document.getElementById("ContentPlaceHolder1_ddlebayCategory") != null && document.getElementById("ContentPlaceHolder1_ddlebayCategory").selectedIndex == 0) {
    //        jAlert('Please Select eBay Category', 'Message', 'ContentPlaceHolder1_ddlebayCategory');
    //        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlebayCategory').offset().top }, 'slow');
    //        return false;
    //    }
    //    if (document.getElementById("ContentPlaceHolder1_ddlebayStorecategory") != null && document.getElementById("ContentPlaceHolder1_ddlebayStorecategory").selectedIndex == 0) {
    //        jAlert('Please Select eBay Store Category', 'Message', 'ContentPlaceHolder1_ddlebayStorecategory');
    //        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlebayStorecategory').offset().top }, 'slow');
    //        return false;
    //    }
    if ((document.getElementById('ContentPlaceHolder1_TxtebayListingDays').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter eBay Listing Days', 'Message', 'ContentPlaceHolder1_TxtebayListingDays');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_TxtebayListingDays').offset().top }, 'slow');
        return false;
    }
    //    if ((document.getElementById('ContentPlaceHolder1_txtSalePrice').value) != '' &&
    //     ((document.getElementById('ContentPlaceHolder1_txtPrice').value) < (document.getElementById('ContentPlaceHolder1_txtSalePrice').value))) {
    //        jAlert('Sale Price can not be greater than Price', 'Message', 'ContentPlaceHolder1_txtSalePrice');
    //        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSalePrice').offset().top }, 'slow');
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