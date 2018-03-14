// JScript File

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

/* Start - All fields of Page Validation function */
var isShippingValidate = true;

function ValidatePageUser() {

    // For User Name and Password validation
    if (document.getElementById("ContentPlaceHolder1_txtUsername") != null && document.getElementById("ContentPlaceHolder1_txtUsername").value.replace(/^\s+|\s+$/g, '') == '') {
        alert("Please enter Email.");
        document.getElementById("ContentPlaceHolder1_txtUsername").value = '';
        document.getElementById("ContentPlaceHolder1_txtUsername").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtUsername').offset().top - 250 }, 'slow');
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtUsername") != null && document.getElementById("ContentPlaceHolder1_txtUsername").value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtUsername").value.replace(/^\s+|\s+$/g, ''))) {
        alert("Please enter valid Email.");
        document.getElementById("ContentPlaceHolder1_txtUsername").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtUsername').offset().top - 250 }, 'slow');
        return false;
    }

}
function ValidatePage() {


    // For User Name and Password validation
    if (document.getElementById("ContentPlaceHolder1_txtUsername") != null && document.getElementById("ContentPlaceHolder1_txtUsername").value.replace(/^\s+|\s+$/g, '') == '') {
        alert("Please enter Email.");
        document.getElementById("ContentPlaceHolder1_txtUsername").value = '';
        document.getElementById("ContentPlaceHolder1_txtUsername").focus();

        
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtUsername').offset().top - 250 }, 'slow');
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtUsername") != null && document.getElementById("ContentPlaceHolder1_txtUsername").value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtUsername").value.replace(/^\s+|\s+$/g, ''))) {
        alert("Please enter valid Email.");
        document.getElementById("ContentPlaceHolder1_txtUsername").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtUsername').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtpassword") != null && document.getElementById("ContentPlaceHolder1_txtpassword").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Password.");
        document.getElementById("ContentPlaceHolder1_txtpassword").value = '';
        document.getElementById("ContentPlaceHolder1_txtpassword").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtpassword').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtpassword") != null && document.getElementById("ContentPlaceHolder1_txtpassword").value.replace(/^\s+|\s+$/g, '').length < 6) {

        alert("Password must be at least 6 characters long.");
        document.getElementById("ContentPlaceHolder1_txtpassword").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtpassword').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtconfirmpassword") != null && document.getElementById("ContentPlaceHolder1_txtconfirmpassword").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Confirm Password.");
        document.getElementById("ContentPlaceHolder1_txtconfirmpassword").value = '';
        document.getElementById("ContentPlaceHolder1_txtconfirmpassword").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtconfirmpassword').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtconfirmpassword") != null && document.getElementById("ContentPlaceHolder1_txtpassword") != null && document.getElementById("ContentPlaceHolder1_txtpassword").value.replace(/^\s+|\s+$/g, '') != document.getElementById("ContentPlaceHolder1_txtconfirmpassword").value.replace(/^\s+|\s+$/g, '')) {

        alert("Confirm Password must be match with Password.");
        document.getElementById("ContentPlaceHolder1_txtconfirmpassword").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtconfirmpassword').offset().top - 250 }, 'slow');
        return false;
    }

    // For Billing Details validation

    else if (document.getElementById("ContentPlaceHolder1_txtBillFirstname") != null && document.getElementById("ContentPlaceHolder1_txtBillFirstname").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Billing First Name.");
        document.getElementById("ContentPlaceHolder1_txtBillFirstname").value = '';
        document.getElementById("ContentPlaceHolder1_txtBillFirstname").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillFirstname').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillLastname") != null && document.getElementById("ContentPlaceHolder1_txtBillLastname").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Billing Last Name.");
        document.getElementById("ContentPlaceHolder1_txtBillLastname").value = '';
        document.getElementById("ContentPlaceHolder1_txtBillLastname").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillLastname').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBilladdressLine1") != null && document.getElementById("ContentPlaceHolder1_txtBilladdressLine1").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Billing Address Line 1.");
        document.getElementById("ContentPlaceHolder1_txtBilladdressLine1").value = '';
        document.getElementById("ContentPlaceHolder1_txtBilladdressLine1").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBilladdressLine1').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillCity") != null && document.getElementById("ContentPlaceHolder1_txtBillCity").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Billing City.");
        document.getElementById("ContentPlaceHolder1_txtBillCity").value = '';
        document.getElementById("ContentPlaceHolder1_txtBillCity").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillCity').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillcountry") != null && document.getElementById("ContentPlaceHolder1_ddlBillcountry").selectedIndex == 0) {

        alert("Please select Billing Country.");
        document.getElementById("ContentPlaceHolder1_ddlBillcountry").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlBillcountry').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex == 0) {

        alert("Please select Billing State.");
        document.getElementById("ContentPlaceHolder1_ddlBillstate").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlBillstate').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate").options[document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex].value == '-11') {

        if (document.getElementById("ContentPlaceHolder1_txtBillingOtherState") != null && document.getElementById("ContentPlaceHolder1_txtBillingOtherState").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Other State.");
            document.getElementById("ContentPlaceHolder1_txtBillingOtherState").value = '';
            document.getElementById("ContentPlaceHolder1_txtBillingOtherState").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillingOtherState').offset().top - 250 }, 'slow');
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtBillZipCode") != null && document.getElementById("ContentPlaceHolder1_txtBillZipCode").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Billing Zip Code.");
            document.getElementById("ContentPlaceHolder1_txtBillZipCode").value = '';
            document.getElementById("ContentPlaceHolder1_txtBillZipCode").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillZipCode').offset().top - 250 }, 'slow');
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtBillphone") != null && document.getElementById("ContentPlaceHolder1_txtBillphone").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Billing Phone.");
            document.getElementById("ContentPlaceHolder1_txtBillphone").value = '';
            document.getElementById("ContentPlaceHolder1_txtBillphone").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillphone').offset().top - 250 }, 'slow');
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Billing Email.");
            document.getElementById("ContentPlaceHolder1_txtBillEmail").value = '';
            document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top - 250 }, 'slow');
            return false;
        }

        else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtBillEmail").value.replace(/^\s+|\s+$/g, ''))) {

            alert("Please enter valid Billing Email.");
            document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top - 250 }, 'slow');
            return false;
        }

        else if (document.getElementById("ContentPlaceHolder1_UseShippingAddress") != null && document.getElementById("ContentPlaceHolder1_UseShippingAddress").checked == true) {

            if (ValidateShippingFields()) {

                if (document.getElementById("ContentPlaceHolder1_txtCodeshow") != null && document.getElementById("ContentPlaceHolder1_txtCodeshow").value.replace(/^\s+|\s+$/g, '') == '') {

                    alert("Please enter Code.");
                    document.getElementById("ContentPlaceHolder1_txtCodeshow").value = '';
                    document.getElementById("ContentPlaceHolder1_txtCodeshow").focus();
                    $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtCodeshow').offset().top - 250 }, 'slow');
                    return false;
                }
                else {
                    return true;
                }
                return true;

            } else {
                return false;
            }
            if (isShippingValidate == false) {
                return false;
            }

        }

        else if (document.getElementById("ContentPlaceHolder1_txtCodeshow") != null && document.getElementById("ContentPlaceHolder1_txtCodeshow").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Code.");
            document.getElementById("ContentPlaceHolder1_txtCodeshow").value = '';
            document.getElementById("ContentPlaceHolder1_txtCodeshow").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtCodeshow').offset().top - 250 }, 'slow');
            return false;
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillZipCode") != null && document.getElementById("ContentPlaceHolder1_txtBillZipCode").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Billing Zip Code.");
        document.getElementById("ContentPlaceHolder1_txtBillZipCode").value = '';
        document.getElementById("ContentPlaceHolder1_txtBillZipCode").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillZipCode').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillphone") != null && document.getElementById("ContentPlaceHolder1_txtBillphone").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Billing Phone.");
        document.getElementById("ContentPlaceHolder1_txtBillphone").value = '';
        document.getElementById("ContentPlaceHolder1_txtBillphone").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillphone').offset().top - 250 }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Billing Email.");
        document.getElementById("ContentPlaceHolder1_txtBillEmail").value = '';
        document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top - 250 }, 'slow');
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtBillEmail").value.replace(/^\s+|\s+$/g, ''))) {

        alert("Please enter valid Billing Email.");
        document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top - 250 }, 'slow');
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_UseShippingAddress") != null && document.getElementById("ContentPlaceHolder1_UseShippingAddress").checked == true) {

        if (ValidateShippingFields()) {

            if (document.getElementById("ContentPlaceHolder1_txtCodeshow") != null && document.getElementById("ContentPlaceHolder1_txtCodeshow").value.replace(/^\s+|\s+$/g, '') == '') {

                alert("Please enter Code.");
                document.getElementById("ContentPlaceHolder1_txtCodeshow").value = '';
                document.getElementById("ContentPlaceHolder1_txtCodeshow").focus();
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtCodeshow').offset().top - 250 }, 'slow');
                return false;
            }
            else {
                return true;
            }
            return true;

        } else {
            return false;
        }
        if (isShippingValidate == false) {
            return false;
        }

    }

    else if (document.getElementById("ContentPlaceHolder1_txtCodeshow") != null && document.getElementById("ContentPlaceHolder1_txtCodeshow").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Code.");
        document.getElementById("ContentPlaceHolder1_txtCodeshow").value = '';
        document.getElementById("ContentPlaceHolder1_txtCodeshow").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtCodeshow').offset().top - 250 }, 'slow');
        return false;
    }
    return true;
}

/* End - All fields of Page Validation function */


function ValidateShippingFields() {

    // For Shipping Details validation

    if (document.getElementById("ContentPlaceHolder1_txtShipFirstname") != null && document.getElementById("ContentPlaceHolder1_txtShipFirstname").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Shipping First Name.");
        document.getElementById("ContentPlaceHolder1_txtShipFirstname").value = '';
        document.getElementById("ContentPlaceHolder1_txtShipFirstname").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipFirstname').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipLastname") != null && document.getElementById("ContentPlaceHolder1_txtShipLastname").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Shipping Last Name.");
        document.getElementById("ContentPlaceHolder1_txtShipLastname").value = '';
        document.getElementById("ContentPlaceHolder1_txtShipLastname").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipLastname').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipAddressLine1") != null && document.getElementById("ContentPlaceHolder1_txtShipAddressLine1").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Shipping Address Line 1.");
        document.getElementById("ContentPlaceHolder1_txtShipAddressLine1").value = '';
        document.getElementById("ContentPlaceHolder1_txtShipAddressLine1").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipAddressLine1').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipCity") != null && document.getElementById("ContentPlaceHolder1_txtShipCity").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Shipping City.");
        document.getElementById("ContentPlaceHolder1_txtShipCity").value = '';
        document.getElementById("ContentPlaceHolder1_txtShipCity").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipCity').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipCounry") != null && document.getElementById("ContentPlaceHolder1_ddlShipCounry").selectedIndex == 0) {

        alert("Please select Shipping Country.");
        document.getElementById("ContentPlaceHolder1_ddlShipCounry").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlShipCounry').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null && document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex == 0) {

        alert("Please select Shipping State.");
        document.getElementById("ContentPlaceHolder1_ddlShipState").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlShipState').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null && document.getElementById("ContentPlaceHolder1_ddlShipState").options[document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex].value == '-11') {

        if (document.getElementById("ContentPlaceHolder1_txtShippingOtherState") != null && document.getElementById("ContentPlaceHolder1_txtShippingOtherState").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Other State.");
            document.getElementById("ContentPlaceHolder1_txtShippingOtherState").value = '';
            document.getElementById("ContentPlaceHolder1_txtShippingOtherState").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShippingOtherState').offset().top - 250 }, 'slow');
            isShippingValidate = false;
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtShipZipCode") != null && document.getElementById("ContentPlaceHolder1_txtShipZipCode").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Shipping Zip Code.");
            document.getElementById("ContentPlaceHolder1_txtShipZipCode").value = '';
            document.getElementById("ContentPlaceHolder1_txtShipZipCode").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipZipCode').offset().top - 250 }, 'slow');
            isShippingValidate = false;
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtShipPhone") != null && document.getElementById("ContentPlaceHolder1_txtShipPhone").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Shipping Phone.");
            document.getElementById("ContentPlaceHolder1_txtShipPhone").value = '';
            document.getElementById("ContentPlaceHolder1_txtShipPhone").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipPhone').offset().top - 250 }, 'slow');
            isShippingValidate = false;
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value.replace(/^\s+|\s+$/g, '') == '') {

            alert("Please enter Shipping Email.");
            document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value = '';
            document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top - 250 }, 'slow');
            isShippingValidate = false;
            return false;
        }

        else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value.replace(/^\s+|\s+$/g, ''))) {

            alert("Please enter valid Shipping Email.");
            document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").focus();
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top - 250 }, 'slow');
            isShippingValidate = false;
            return false;
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipZipCode") != null && document.getElementById("ContentPlaceHolder1_txtShipZipCode").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Shipping Zip Code.");
        document.getElementById("ContentPlaceHolder1_txtShipZipCode").value = '';
        document.getElementById("ContentPlaceHolder1_txtShipZipCode").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipZipCode').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipPhone") != null && document.getElementById("ContentPlaceHolder1_txtShipPhone").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Shipping Phone.");
        document.getElementById("ContentPlaceHolder1_txtShipPhone").value = '';
        document.getElementById("ContentPlaceHolder1_txtShipPhone").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipPhone').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value.replace(/^\s+|\s+$/g, '') == '') {

        alert("Please enter Shipping Email.");
        document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value = '';
        document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value.replace(/^\s+|\s+$/g, ''))) {

        alert("Please enter valid Shipping Email.");
        document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").focus();
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top - 250 }, 'slow');
        isShippingValidate = false;
        return false;
    }
    isShippingValidate = true;
    return true;
}


/* Start - For Show and Hide Shipping Address Panel*/
function SetBillingShippingVisible() {

    if (document.getElementById("ContentPlaceHolder1_UseShippingAddress") != null && document.getElementById("ContentPlaceHolder1_UseShippingAddress").checked == true) {
        if (document.getElementById("ContentPlaceHolder1_pnlShippingDetails") != null) {
            document.getElementById("ContentPlaceHolder1_pnlShippingDetails").style.display = 'block';
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_UseShippingAddress") != null && document.getElementById("ContentPlaceHolder1_UseShippingAddress").checked == false) {
        if (document.getElementById("ContentPlaceHolder1_pnlShippingDetails") != null) {
            MakeBlank();
            document.getElementById("ContentPlaceHolder1_pnlShippingDetails").style.display = 'none';
        }
    }
}
/* End - For Show and Hide Shipping Address Panel*/

/* Start - For Make All fields of Shipping Address Blank */

function MakeBlank() {

    if (document.getElementById("ContentPlaceHolder1_txtShipFirstname") != null) { document.getElementById("ContentPlaceHolder1_txtShipFirstname").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipLastname") != null) { document.getElementById("ContentPlaceHolder1_txtShipLastname").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipAddressLine1") != null) { document.getElementById("ContentPlaceHolder1_txtShipAddressLine1").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtshipAddressLine2") != null) { document.getElementById("ContentPlaceHolder1_txtshipAddressLine2").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipSuite") != null) { document.getElementById("ContentPlaceHolder1_txtShipSuite").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipCity") != null) { document.getElementById("ContentPlaceHolder1_txtShipCity").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_ddlShipCounry") != null) { document.getElementById("ContentPlaceHolder1_ddlShipCounry").selectedIndex = 1; }
    if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null) { document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex = 0; }
    if (document.getElementById("ContentPlaceHolder1_txtShipZipCode") != null) { document.getElementById("ContentPlaceHolder1_txtShipZipCode").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipPhone") != null) { document.getElementById("ContentPlaceHolder1_txtShipPhone").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null) { document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value = ''; }
    SetShippingOtherVisible(false);
}


/* Start -  Make Other State Field visible True or false according to condition*/
function SetBillingOtherVisible(IsVisible) {
    if (IsVisible && document.getElementById('DIVBillingOther') != null) {
        document.getElementById('DIVBillingOther').style.display = 'block';
    }
    else {
        if (document.getElementById('ContentPlaceHolder1_txtBillingOtherState') != null) { document.getElementById('ContentPlaceHolder1_txtBillingOtherState').value = ''; }
        if (document.getElementById('DIVBillingOther') != null) { document.getElementById('DIVBillingOther').style.display = 'none'; }

    }
}
function SetShippingOtherVisible(IsVisible) {
    if (IsVisible) {
        if (document.getElementById('DIVShippingOther') != null) { document.getElementById('DIVShippingOther').style.display = 'block'; }
    }
    else {
        if (document.getElementById('ContentPlaceHolder1_txtShippingOtherState') != null) { document.getElementById('ContentPlaceHolder1_txtShippingOtherState').value = ''; }
        if (document.getElementById('DIVShippingOther') != null) { document.getElementById('DIVShippingOther').style.display = 'none'; }
    }
}
function MakeBillingOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlBillstate') != null && document.getElementById('ContentPlaceHolder1_ddlBillstate').options[document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex].value == '-11')
        SetBillingOtherVisible(true);
    else
        SetBillingOtherVisible(false);
}
function MakeShippingOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlShipState') != null && document.getElementById('ContentPlaceHolder1_ddlShipState').options[document.getElementById('ContentPlaceHolder1_ddlShipState').selectedIndex].value == '-11')
        SetShippingOtherVisible(true);
    else
        SetShippingOtherVisible(false);
}

/* End -  Make Other State Field visible True or false according to condition*/

