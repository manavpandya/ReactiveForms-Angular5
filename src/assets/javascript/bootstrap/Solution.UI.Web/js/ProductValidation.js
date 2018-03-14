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
    if ((document.getElementById('ContentPlaceHolder1_txtProductURL').value).replace(/^\s*\s*$/g, '') == '') {
        jAlert('Please Enter Product URL', 'Message', 'ContentPlaceHolder1_txtProductURL');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtProductURL').offset().top }, 'slow');
        return false;
    }
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
    if ((document.getElementById('ContentPlaceHolder1_txtInventory').value).replace(/^\s*\s*$/g, '') != '') {
        var InvVlaue = checkCount();
        if (InvVlaue == false) { return false; }        
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

//if (document.getElementById("ContentPlaceHolder1_chkIsHamming") != null && document.getElementById("ContentPlaceHolder1_chkIsHamming").checked == true && document.getElementById("ContentPlaceHolder1_txtHammingQty") != null) {
        //if (document.getElementById("ContentPlaceHolder1_txtHammingQty").value != '') {
            //var Inventory = parseInt((document.getElementById('ContentPlaceHolder1_txtInventory').value)).toFixed(2);
            //Inventory = Inventory;
            //var HammingQty = parseInt((document.getElementById('ContentPlaceHolder1_txtHammingQty').value)).toFixed(2);
            //HammingQty = HammingQty;

            //if (parseInt(HammingQty) > parseInt(Inventory)) {
                //jAlert('Hemming Quantity should be less than Inventory', 'Message', 'ContentPlaceHolder1_txtHammingQty');
                //$('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtHammingQty').offset().top }, 'slow');
                //return false;
            //}
        //}
       // else {
           // jAlert('Please Enter Hemming %', 'Message', 'ContentPlaceHolder1_txtHammingQty');
           // $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtHammingQty').offset().top }, 'slow');
           // return false;
       // }
   // }
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
    if (document.getElementById("ContentPlaceHolder1_chkisordermade") != null && document.getElementById("ContentPlaceHolder1_chkismadetomeasure") != null && (document.getElementById("ContentPlaceHolder1_chkisordermade").checked == true || document.getElementById("ContentPlaceHolder1_chkismadetomeasure").checked == true)) {

        if (document.getElementById("ContentPlaceHolder1_ddlFabricType").selectedIndex == 0) {
            jAlert('Please Select Fabric Category.', 'Message', 'ContentPlaceHolder1_ddlFabricCode');
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlFabricCode').offset().top }, 'slow');
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_ddlFabricCode").selectedIndex == 0) {
            jAlert('Please Select Fabric Code.', 'Message', 'ContentPlaceHolder1_ddlFabricCode');
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlFabricCode').offset().top }, 'slow');
            return false;
        }
    }
    if (document.getElementById("ContentPlaceHolder1_chkIsFreeEngraving") != null && document.getElementById("ContentPlaceHolder1_chkIsFreeEngraving").checked == true && document.getElementById("tdEngravingSize") != null && document.getElementById("ContentPlaceHolder1_txtEngravingSize").value == '') {

        jAlert('Please Enter Engraving Size', 'Message', 'ContentPlaceHolder1_txtEngravingSize');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtEngravingSize').offset().top }, 'slow');
        return false;
    }
//    if (document.getElementById("ContentPlaceHolder1_ddlFabricType") != null && document.getElementById("ContentPlaceHolder1_ddlFabricType").selectedIndex != 0 && document.getElementById("ContentPlaceHolder1_ddlFabricCode").selectedIndex == 0) {
//        jAlert('Please Select Fabric Code', 'Message', 'ContentPlaceHolder1_ddlFabricCode');
//        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlFabricCode').offset().top }, 'slow');
//        return false;
    //    }

    if (document.getElementById("ContentPlaceHolder1_chkNewArrival") != null && document.getElementById("ContentPlaceHolder1_chkNewArrival").checked == true) {
        if (document.getElementById('ContentPlaceHolder1_txtNewArrivalFromDate').value == '') {
            jAlert('Please Enter New Arrival From Date.', 'Required Information', 'ContentPlaceHolder1_txtNewArrivalFromDate');
            Tabdisplay(6);
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_txtNewArrivalToDate').value == '') {
            jAlert('Please Enter New Arrival To Date.', 'Required Information', 'ContentPlaceHolder1_txtNewArrivalToDate');
            Tabdisplay(6);
            return false;
        }

        var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtNewArrivalFromDate').value);
        var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtNewArrivalToDate').value);
        if (startDate > endDate) {
            jAlert('Please Select Valid New Arrival Date.', 'Required Information', 'ContentPlaceHolder1_txtNewArrivalToDate');
            Tabdisplay(6);
            return false;
        }
    }

    if (document.getElementById("ContentPlaceHolder1_chkFeatured") != null && document.getElementById("ContentPlaceHolder1_chkFeatured").checked == true) {
        if (document.getElementById('ContentPlaceHolder1_txtIsFeaturedFromDate').value == '') {
            jAlert('Please Enter Featured From Date.', 'Required Information', 'ContentPlaceHolder1_txtIsFeaturedFromDate');
            Tabdisplay(6);
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_txtIsFeaturedToDate').value == '') {
            jAlert('Please Enter Featured To Date.', 'Required Information', 'ContentPlaceHolder1_txtIsFeaturedToDate');
            Tabdisplay(6);
            return false;
        }

        var startDate1 = new Date(document.getElementById('ContentPlaceHolder1_txtIsFeaturedFromDate').value);
        var endDate1 = new Date(document.getElementById('ContentPlaceHolder1_txtIsFeaturedToDate').value);
        if (startDate1 > endDate1) {
            jAlert('Please Select Valid Featured Date.', 'Required Information', 'ContentPlaceHolder1_txtIsFeaturedToDate');
            Tabdisplay(6);
            return false;
        }
    }


    if (document.getElementById('ContentPlaceHolder1_chkIsBestSeller') != null && document.getElementById('ContentPlaceHolder1_chkIsBestSeller').checked == true) {
        if (document.getElementById('ContentPlaceHolder1_txtBestSellerFromDate').value == '') {
            jAlert('Please Enter Best Seller From Date.', 'Required Information', 'ContentPlaceHolder1_txtBestSellerFromDate');
            Tabdisplay(6);
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_txtBestSellerToDate').value == '') {
            jAlert('Please Enter Best Seller To Date.', 'Required Information', 'ContentPlaceHolder1_txtBestSellerToDate');
            Tabdisplay(6);
            return false;
        }

        var startDateBest = new Date(document.getElementById('ContentPlaceHolder1_txtBestSellerFromDate').value);
        var endDateBest = new Date(document.getElementById('ContentPlaceHolder1_txtBestSellerToDate').value);
        if (startDateBest > endDateBest) {
            jAlert('Please Select Valid Best Seller Date.', 'Required Information', 'ContentPlaceHolder1_txtBestSellerToDate');
            Tabdisplay(6);
            return false;
        }
    }



    if (document.getElementById("ContentPlaceHolder1_chkIsFreeShipping") != null && document.getElementById("ContentPlaceHolder1_chkIsFreeShipping").checked == true) {
        if (document.getElementById('ContentPlaceHolder1_txtFreeShippingFromDate').value == '') {
            jAlert('Please Enter Free Shipping From Date.', 'Required Information', 'ContentPlaceHolder1_txtFreeShippingFromDate');
            Tabdisplay(6);
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_txtFreeShippingToDate').value == '') {
            jAlert('Please Enter Free Shipping To Date.', 'Required Information', 'ContentPlaceHolder1_txtFreeShippingToDate');
            Tabdisplay(6);
            return false;
        }

        var startDateFree = new Date(document.getElementById('ContentPlaceHolder1_txtFreeShippingFromDate').value);
        var endDateFree = new Date(document.getElementById('ContentPlaceHolder1_txtFreeShippingToDate').value);
        if (startDateFree > endDateFree) {
            jAlert('Please Select Valid Free Shipping Date.', 'Required Information', 'ContentPlaceHolder1_txtFreeShippingToDate');
            Tabdisplay(6);
            return false;
        }
    }
    Tabdisplay(document.getElementById('ContentPlaceHolder1_hdnTabid').value);
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

function keyRestrictforIntOnly(e, validchars) {
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

function checkCount() {
    var myform = document.forms[0];
    var len = myform.elements.length;
    var count = 0;
    for (var i = 0; i < len; i++) {
        if (myform.elements[i].type == 'radio') {
            if (myform.elements[i].checked == true) {
                count += 1;
            }
        }
    }
    if (count == 0) {
        window.parent.jAlert('Select at least one Warehouse Preferred Location!', 'Message');
        return false;
    }
    else {
        return true;
    }
}


