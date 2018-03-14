

$(function () {
    $('#ContentPlaceHolder1_txtstartdate').datetimepicker({
        showButtonPanel: true, ampm: false,
        showHour: false, showMinute: false, showSecond: false, showTime: false
    });
});

$(function () {
    $('#ContentPlaceHolder1_txtenddate').datetimepicker({
        showButtonPanel: true, ampm: false,
        showHour: false, showMinute: false, showSecond: false, showTime: false
    });
});

$(function () {
    $('#ContentPlaceHolder1_txtShippingStartDate').datetimepicker({
        showButtonPanel: true, ampm: false,
        showHour: false, showMinute: false, showSecond: false, showTime: false
    });
});

$(function () {
    $('#ContentPlaceHolder1_txtShippingEndDate').datetimepicker({
        showButtonPanel: true, ampm: false,
        showHour: false, showMinute: false, showSecond: false, showTime: false
    });
});

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
function inputOnlyNumbers(evt) {
    var e = window.event || evt;
    var charCode = e.which || e.keyCode;
    if (charCode < 31 || (charCode > 47 && charCode < 58) || charCode == 46) {
        return true;
    }
    return false;
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

function ClientValidate() {
    var listItemArray = document.getElementById('ContentPlaceHolder1_trvCategories').getElementsByTagName('INPUT');
    var isItemChecked = false;
    for (var i = 0; i < listItemArray.length; i++) {
        var listItem = listItemArray[i];
        if (listItem.checked) {
            isItemChecked = true;
        }
    }
    if (isItemChecked == false) {
        //jAlert('Please select at least one Category', 'Message');
        return true;
    }
    else {
        return true;
    }
}


function Checkfields() {
    var ReturnData = true;
    if (document.getElementById("ContentPlaceHolder1_txttitle").value == '') {
        jAlert('Please enter Title.', 'Message', 'ContentPlaceHolder1_txttitle');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlManufacture").selectedIndex == 0) {
        jAlert('Please select Brand.', 'Message', 'ContentPlaceHolder1_ddlManufacture');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtModelNumber").value == '') {
        jAlert('Please enter Model Number.', 'Message', 'ContentPlaceHolder1_txtModelNumber');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtSKU").value == '') {
        jAlert('Please enter SKU.', 'Message', 'ContentPlaceHolder1_txtSKU');
        ReturnData = false;
    }
    else if ((document.getElementById("ContentPlaceHolder1_txtenddate").value < document.getElementById("ContentPlaceHolder1_txtstartdate").value)) {
        jAlert('End Date should be greater than Start Date', 'Message', 'ContentPlaceHolder1_txtenddate');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtWeight").value == '') {
        jAlert('Please enter Weight.', 'Message', 'ContentPlaceHolder1_txtWeight');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtPrice").value == '') {
        jAlert('Please enter Price.', 'Message', 'ContentPlaceHolder1_txtPrice');
        ReturnData = false;
    }
    else if ((document.getElementById("ContentPlaceHolder1_txtPrice").value < document.getElementById("ContentPlaceHolder1_txtSalePrice").value)) {
        jAlert('Sale Price should be less than Price', 'Message', 'ContentPlaceHolder1_txtSalePrice');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtInventory").value == '') {
        jAlert('Please enter Inventory.', 'Message', 'ContentPlaceHolder1_txtInventory');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlProductTypeDelivery") != null && document.getElementById("ContentPlaceHolder1_ddlProductTypeDelivery").selectedIndex == 0) {

    alert('trtr');
        jAlert('Please Select Product Delivery Type', 'Message', 'ContentPlaceHolder1_ddlProductTypeDelivery');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlProductTypeDelivery').offset().top }, 'slow');
        return false;
    }
   else if (document.getElementById("ContentPlaceHolder1_ddlProductType") != null && document.getElementById("ContentPlaceHolder1_ddlProductType").selectedIndex == 0) {
        jAlert('Please Select Product Type', 'Message', 'ContentPlaceHolder1_ddlProductType');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlProductType').offset().top }, 'slow');
        return false;
    }

    else if ((document.getElementById("ContentPlaceHolder1_txtShippingEndDate").value < document.getElementById("ContentPlaceHolder1_txtShippingStartDate").value)) {
        jAlert('Shipping End Date should be greater than Shipping Start Date', 'Message', 'ContentPlaceHolder1_txtShippingEndDate');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtSummary").value == '') {
        jAlert('Please enter Short Description.', 'Message', 'ContentPlaceHolder1_txtSummary');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtMaincategory").value == '') {
        jAlert('Please enter Main Category.', 'Message', 'ContentPlaceHolder1_txtMaincategory');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtwidth").value == '') {
        jAlert('Please enter Width.', 'Message', 'ContentPlaceHolder1_txtwidth');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtheight").value == '') {
        jAlert('Please enter Height.', 'Message', 'ContentPlaceHolder1_txtheight');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtlength").value == '') {
        jAlert('Please enter Length.', 'Message', 'ContentPlaceHolder1_txtlength');
        ReturnData = false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtupc").value == '') {
        jAlert('Please enter UPC.', 'Message', 'ContentPlaceHolder1_txtupc');
        ReturnData = false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtupc').value != '' && document.getElementById('ContentPlaceHolder1_txtupc').value.length < 12) {
        jAlert('UPC Length must be grater than or equal to 12 digit.', 'Message', 'ContentPlaceHolder1_txtupc');
        return false;
    }
    else if (ClientValidate() == false) {
        return false;
    }
    return ReturnData;
}