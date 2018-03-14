/* Start - General Function for restriction */

var testresults
function checkemail1(str) {
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
    if (filter.test(str))
        testresults = true
    else {
        testresults = false
    }
    return (testresults)
}

function onKeyPressPhone(e) {


    var key = window.event ? window.event.keyCode : e.which;

    if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45 || key == 88 || key == 17 || key == 120) {
        return key;
    }

    var keychar = String.fromCharCode(key);

    var reg = /\d/;
    if (window.event)
        return event.returnValue = reg.test(keychar);
    else
        return reg.test(keychar);

}

function onKeyPressBlockNumbers(e) {

    var key = window.event ? window.event.keyCode : e.which;

    if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 88 || key == 17 || key == 120) {
        return key;
    }

    var keychar = String.fromCharCode(key);

    var reg = /\d/;
    if (window.event)
        return event.returnValue = reg.test(keychar);
    else
        return reg.test(keychar);


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

    if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 104)
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
function isNumberKeyCard(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function isNumberKey(event) {
    var retval = false;
    var charCode = (event.keyCode) ? event.keyCode : event.which;
    if (charCode > 31 && (charCode < 33 || charCode > 64) && (charCode < 91 || charCode > 96) && (charCode < 123 || charCode > 126))
        retval = true;
    if (charCode == 8 || (charCode > 35 && charCode < 41) || charCode == 46 || charCode == 9)
        retval = true;
    if (navigator.appName.indexOf('Microsoft') != -1)
        window.event.returnValue = retval;
    return retval;
}

/* End -  General Function for restriction */

var isBillingValidate = true;
function ValidatePage() {

    var CardType = '';
    CardType = document.getElementById('ContentPlaceHolder1_ddlCardType').options[document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex].text;

    if (document.getElementById('ContentPlaceHolder1_txtcardName') != null && document.getElementById('ContentPlaceHolder1_txtcardName').value == '') {
        alert('Please Enter Name of Card.');
        document.getElementById('ContentPlaceHolder1_txtcardName').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null && document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex == 0) {
        alert('Please select Card Type.');
        document.getElementById('ContentPlaceHolder1_ddlCardType').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && document.getElementById('ContentPlaceHolder1_txtCardNumber').value == '') {
        alert('Please Enter Card Number.');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && /^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_txtCardNumber').value) == false) {
        alert('Please Enter valid Numeric Card Number');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && (CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_txtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.length != 15) {
        alert('Credit Card Number must be 15 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.length != 16) {
        alert('Credit Card Number must be 16 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardVarificationCode') != null && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value == '') {
        alert('Please Enter Card Verification Code.');
        document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardVarificationCode') != null && /^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value) == false) {
        alert('Please enter valid Numeric Card Verification Code');
        document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardVarificationCode') != null && (CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value.length != 4 && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value != '') {
        alert('Card Verification Code must be 4 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtCardVarificationCode') != null && CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value.length != 3 && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value != '') {
        alert('Card Verification Code must be 3 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlmonth') != null && document.getElementById('ContentPlaceHolder1_ddlmonth').selectedIndex == 0) {
        alert('Please select Month.');
        document.getElementById('ContentPlaceHolder1_ddlmonth').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlyear') != null && document.getElementById('ContentPlaceHolder1_ddlyear').selectedIndex == 0) {
        alert('Please select Year');
        document.getElementById('ContentPlaceHolder1_ddlyear').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_ddlmonth') != null && document.getElementById('ContentPlaceHolder1_ddlmonth').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_ddlyear') != null && document.getElementById('ContentPlaceHolder1_ddlyear').selectedIndex != 0) {
        var objDate = new Date();
        var year = document.getElementById('ContentPlaceHolder1_ddlyear').options[document.getElementById('ContentPlaceHolder1_ddlyear').selectedIndex].value;
        var month = document.getElementById('ContentPlaceHolder1_ddlmonth').options[document.getElementById('ContentPlaceHolder1_ddlmonth').selectedIndex].value;
        if ((year > objDate.getFullYear()) || (year == objDate.getFullYear() && month >= objDate.getMonth() + 1)) {

            if (document.getElementById('ContentPlaceHolder1_ddlBillingAddress') != null && document.getElementById('ContentPlaceHolder1_ddlBillingAddress').selectedIndex == 0) {
                alert('Please select Billing Address');
                document.getElementById('ContentPlaceHolder1_ddlBillingAddress').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_ddlBillingAddress') != null && (document.getElementById('ContentPlaceHolder1_ddlBillingAddress').options[document.getElementById('ContentPlaceHolder1_ddlBillingAddress').selectedIndex]).value == '-12') {
                alert('Please select proper Billing Address');
                document.getElementById('ContentPlaceHolder1_ddlBillingAddress').focus();
                return false;
            }

        }
        else {
            alert('Please Enter Valid Expiration Date.');
            return false;
        }
    }

}


function ValidateBillingFields() {

    if (document.getElementById("ContentPlaceHolder1_txtBillFirstname") != null && document.getElementById("ContentPlaceHolder1_txtBillFirstname").value == '') {

        alert("Please enter Billing First Name.");
        document.getElementById("ContentPlaceHolder1_txtBillFirstname").focus();
        isBillingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillLastname") != null && document.getElementById("ContentPlaceHolder1_txtBillLastname").value == '') {

        alert("Please enter Billing Last Name.");
        document.getElementById("ContentPlaceHolder1_txtBillLastname").focus();
        isBillingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBilladdressLine1") != null && document.getElementById("ContentPlaceHolder1_txtBilladdressLine1").value == '') {

        alert("Please enter Billing Address Line 1.");
        document.getElementById("ContentPlaceHolder1_txtBilladdressLine1").focus();
        isBillingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillCity") != null && document.getElementById("ContentPlaceHolder1_txtBillCity").value == '') {

        alert("Please enter Billing City.");
        document.getElementById("ContentPlaceHolder1_txtBillCity").focus();
        isBillingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillcountry") != null && document.getElementById("ContentPlaceHolder1_ddlBillcountry").selectedIndex == 0) {

        alert("Please select Billing Country.");
        document.getElementById("ContentPlaceHolder1_ddlBillcountry").focus();
        isBillingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex == 0) {

        alert("Please select Billing State.");
        document.getElementById("ContentPlaceHolder1_ddlBillstate").focus();
        isBillingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate").options[document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex].value == '-11') {

        if (document.getElementById("ContentPlaceHolder1_txtBillingOtherState") != null && document.getElementById("ContentPlaceHolder1_txtBillingOtherState").value == '') {
            alert("Please enter Other State.");
            document.getElementById("ContentPlaceHolder1_txtBillingOtherState").focus();
            isBillingValidate = false;
            return false;
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillZipCode") != null && document.getElementById("ContentPlaceHolder1_txtBillZipCode").value == '') {

        alert("Please enter Billing Zip Code.");
        document.getElementById("ContentPlaceHolder1_txtBillZipCode").focus();
        isBillingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillphone") != null && document.getElementById("ContentPlaceHolder1_txtBillphone").value == '') {

        alert("Please enter Billing Phone.");
        document.getElementById("ContentPlaceHolder1_txtBillphone").focus();
        isBillingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value == '') {

        alert("Please enter Billing Email.");
        document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
        isBillingValidate = false;
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtBillEmail").value)) {

        alert("Please enter valid Billing Email.");
        document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
        isBillingValidate = false;
        return false;
    }
}

function MakeBillingOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlBillstate') != null && document.getElementById('ContentPlaceHolder1_ddlBillstate').options[document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex].value == '-11')
        SetBillingOtherVisible(true);
    else
        SetBillingOtherVisible(false);
}

function SetBillingOtherVisible(IsVisible) {
    if (IsVisible) {
        if (document.getElementById('DIVBillingOther') != null) { document.getElementById('DIVBillingOther').style.display = 'block'; }
    }
    else {
        if (document.getElementById('ContentPlaceHolder1_txtBillingOtherState') != null) { document.getElementById('ContentPlaceHolder1_txtBillingOtherState').value = ''; }
        if (document.getElementById('DIVBillingOther') != null) { document.getElementById('DIVBillingOther').style.display = 'none'; }

    }
}


function onMouseOverCVC() {
    document.images["CVCImage"].src = "/images/cvv.gif";
    document.images["CVCImage"].style.display = 'block';
}

function onMouseOutCVC() {
    document.images["CVCImage"].src = "";
    document.images["CVCImage"].style.display = 'none';
}