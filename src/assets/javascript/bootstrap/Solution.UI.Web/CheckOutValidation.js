
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
function isNumberKeyCard(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

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

function ValidationNotLogin() {

    if (CreateAccORLoginValidation()) {
        if (ValidatePage()) {
            if (ShippingValidation()) {
                if (checkcreditcard()) {

            if (document.getElementById('ContentPlaceHolder1_chkreturnpolicy') != null && document.getElementById('ContentPlaceHolder1_chkreturnpolicy').checked == false) {
                alert('Please select terms and condition.');
                return false;
            }
            else { Loader(); return true; }
        }
        else { return false; }
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
    return true;
}
function ValidationLoginuser() {

    if (ValidatePage()) {
        if (ShippingValidation()) {
            if (checkcreditcard()) {

                if (document.getElementById('ContentPlaceHolder1_chkreturnpolicy') != null && document.getElementById('ContentPlaceHolder1_chkreturnpolicy').checked == false) {
                    alert('Please select terms and condition.');
                    return false;
                }
                else { Loader(); return true; }
            }
            else { return false; }
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }

    return true;
}
function ValidationLoginuserOtherPayment() {

    if (ValidatePage()) {
        if (ShippingValidation()) {

             

                if (document.getElementById('ContentPlaceHolder1_chkreturnpolicy') != null && document.getElementById('ContentPlaceHolder1_chkreturnpolicy').checked == false) {
                    alert('Please select terms and condition.');
                    return false;
                }
                else { Loader(); return true; }
             
             
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
    Loader();
    return true;
}
function ValidationNotLoginOtherPayment() {
 if (CreateAccORLoginValidation()) {
    if (ValidatePage()) {

        if (ShippingValidation()) {
             

                if (document.getElementById('ContentPlaceHolder1_chkreturnpolicy') != null && document.getElementById('ContentPlaceHolder1_chkreturnpolicy').checked == false) {
                    alert('Please select terms and condition.');
                    return false;
                }
                else { Loader(); return true; }
             

            
        }
        else {
            return false;
        }

    }
    else {
        return false;
    }
}
    else {
        return false;
    }
    Loader();
    return true;
}
function ShippingValidation() {
    var falg = false;
    if (document.getElementById('ContentPlaceHolder1_rdoShippingMethod') != null) {


        var radioButtons = document.getElementById("ContentPlaceHolder1_rdoShippingMethod").getElementsByTagName('input');
        for (var x = 0; x < radioButtons.length; x++) {
            if (radioButtons[x].checked) {
                falg = true;
                break;
            }
        }

        if (falg) {
            return true;
        }
        else {
            alert('Please Select Shipping Method');
            document.getElementById('ContentPlaceHolder1_rdoShippingMethod').focus();
            return false;
        }
    }
    return true;
}

function CreateAccORLoginValidation() {
    if (document.getElementById('ContentPlaceHolder1_txtusername') != null && (document.getElementById('ContentPlaceHolder1_txtusername').value).replace(/^\s*\s*$/g, '') == '') {
        alert("Please enter Contact Email.");
        if (document.getElementById('ContentPlaceHolder1_chkCreateNewAccount') != null && document.getElementById('ContentPlaceHolder1_chkCreateNewAccount').checked == true)
        {
            document.getElementById('ContentPlaceHolder1_txtCreateEmail').focus();
            
        }
        else if (document.getElementById('ContentPlaceHolder1_chkCreateNewAccountGuest') != null && document.getElementById('ContentPlaceHolder1_chkCreateNewAccountGuest').checked == true) {
            
            document.getElementById('ContentPlaceHolder1_txtGuestemail').focus();
        }
        else {
            document.getElementById('ContentPlaceHolder1_txtusername').focus();
        }
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtusername').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtusername').value)) {
        alert('Please enter valid Contact Email.');


        if (document.getElementById('ContentPlaceHolder1_chkCreateNewAccount') != null && document.getElementById('ContentPlaceHolder1_chkCreateNewAccount').checked == true) {
           

            document.getElementById('ContentPlaceHolder1_txtCreateEmail').value = '';
            document.getElementById('ContentPlaceHolder1_txtCreateEmail').focus();
        }
        else if (document.getElementById('ContentPlaceHolder1_chkCreateNewAccountGuest') != null && document.getElementById('ContentPlaceHolder1_chkCreateNewAccountGuest').checked == true) {
            document.getElementById('ContentPlaceHolder1_txtGuestemail').value = '';
            document.getElementById('ContentPlaceHolder1_txtGuestemail').focus();
        }
        else {
            document.getElementById('ContentPlaceHolder1_txtusername').value = '';
            document.getElementById('ContentPlaceHolder1_txtusername').focus();
        }

       
        return false;
    }
    else {
        if (document.getElementById('ContentPlaceHolder1_chkCreateNewAccount').checked) {
            if (document.getElementById("ContentPlaceHolder1_txtCreateNewPassword") != null && document.getElementById("ContentPlaceHolder1_txtCreateNewPassword").value.replace(/^\s+|\s+$/g, '') == '') {
                alert("Please enter Password.");
                document.getElementById("ContentPlaceHolder1_txtCreateNewPassword").value = '';
                document.getElementById("ContentPlaceHolder1_txtCreateNewPassword").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtCreateNewPassword") != null && document.getElementById("ContentPlaceHolder1_txtCreateNewPassword").value.replace(/^\s+|\s+$/g, '').length < 6) {
                alert("Password must be at least 6 characters long.");
                document.getElementById("ContentPlaceHolder1_txtCreateNewPassword").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtConfirmPassWord") != null && document.getElementById("ContentPlaceHolder1_txtConfirmPassWord").value.replace(/^\s+|\s+$/g, '') == '') {
                alert("Please enter Confirm Password.");
                document.getElementById("ContentPlaceHolder1_txtConfirmPassWord").value = '';
                document.getElementById("ContentPlaceHolder1_txtConfirmPassWord").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtConfirmPassWord") != null && document.getElementById("ContentPlaceHolder1_txtConfirmPassWord") != null && document.getElementById("ContentPlaceHolder1_txtCreateNewPassword").value.replace(/^\s+|\s+$/g, '') != document.getElementById("ContentPlaceHolder1_txtConfirmPassWord").value.replace(/^\s+|\s+$/g, '')) {
                alert("Confirm Password must be match with Password.");
                document.getElementById("ContentPlaceHolder1_txtConfirmPassWord").focus();
                return false;
            }
        }
        if (document.getElementById('ContentPlaceHolder1_chkReturningAcHolder').checked) {
            if (document.getElementById("ContentPlaceHolder1_txtpassword") != null && document.getElementById("ContentPlaceHolder1_txtpassword").value.replace(/^\s+|\s+$/g, '') == '') {
                alert("Please enter Sign In Password.");
                document.getElementById("ContentPlaceHolder1_txtpassword").value = '';
                document.getElementById("ContentPlaceHolder1_txtpassword").focus();
                return false;
            }
        }
    }
    return true;
}

function ValidatePage() {


    // For User Name and Password validation

    if (document.getElementById("ContentPlaceHolder1_txtBillFirstname") != null && document.getElementById("ContentPlaceHolder1_txtBillFirstname").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Billing First Name.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillFirstname').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtBillFirstname").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillLastname") != null && document.getElementById("ContentPlaceHolder1_txtBillLastname").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Billing Last Name.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillLastname').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtBillLastname").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillAddressLine1") != null && document.getElementById("ContentPlaceHolder1_txtBillAddressLine1").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Billing Address Line 1.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillAddressLine1').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtBillAddressLine1").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillCity") != null && document.getElementById("ContentPlaceHolder1_txtBillCity").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Billing City.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillCity').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtBillCity").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillcountry") != null && document.getElementById("ContentPlaceHolder1_ddlBillcountry").selectedIndex == 0) {

        alert("Please Select Billing Country.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlBillcountry').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_ddlBillcountry").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex == 0) {

        alert("Please Select Billing State.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlBillstate').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_ddlBillstate").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate").options[document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex].value == '-11') {

        if (document.getElementById("ContentPlaceHolder1_txtBillother") != null && document.getElementById("ContentPlaceHolder1_txtBillother").value.replace(/^\s+|\s+$/g, "") == '') {

            alert("Please Enter Other State.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillother').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtBillother").focus();
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtBillZipCode") != null && document.getElementById("ContentPlaceHolder1_txtBillZipCode").value.replace(/^\s+|\s+$/g, "") == '') {

            alert("Please Enter Billing Zip Code.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillZipCode').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtBillZipCode").focus();
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtBillphone") != null && document.getElementById("ContentPlaceHolder1_txtBillphone").value.replace(/^\s+|\s+$/g, "") == '') {

            alert("Please Enter Billing Phone.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillphone').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtBillphone").focus();
            return false;
        }
        else if (!(document.getElementById('ContentPlaceHolder1_txtBillphone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
            alert("Please enter valid Billing Phone.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillphone').offset().top }, 'slow');
            document.getElementById('ContentPlaceHolder1_txtBillphone').focus();
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value.replace(/^\s+|\s+$/g, "") == '') {

            alert("Please Enter Billing Email.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
            return false;
        }

        else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtBillEmail").value)) {

            alert("Please Enter valid Billing Email.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
            return false;
        }


        else if (document.getElementById("ContentPlaceHolder1_UseShippingAddress") != null && document.getElementById("ContentPlaceHolder1_UseShippingAddress").checked == true) {

            if (ValidateShippingFields()) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillZipCode") != null && document.getElementById("ContentPlaceHolder1_txtBillZipCode").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Billing Zip Code.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillZipCode').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtBillZipCode").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillphone") != null && document.getElementById("ContentPlaceHolder1_txtBillphone").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Billing Phone.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillphone').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtBillphone").focus();
        return false;
    }
    else if (!(document.getElementById('ContentPlaceHolder1_txtBillphone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
        alert("Please enter valid Billing Phone.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillphone').offset().top }, 'slow');
        document.getElementById('ContentPlaceHolder1_txtBillphone').focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Billing Email.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtBillEmail").value)) {

        alert("Please Enter valid Billing Email.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtBillEmail").focus();
        return false;
    }


    else if (document.getElementById("ContentPlaceHolder1_UseShippingAddress") != null && document.getElementById("ContentPlaceHolder1_UseShippingAddress").checked == true) {

        if (ValidateShippingFields()) {
            return true;
        }
        else {
            return false;
        }
    }



    return true;
}

/* End - All fields of Page Validation function */


function ValidateShippingFields() {

    // For Shipping Details validation

    if (document.getElementById("ContentPlaceHolder1_txtShipFirstName") != null && document.getElementById("ContentPlaceHolder1_txtShipFirstName").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Shipping First Name.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipFirstName').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtShipFirstName").focus();

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipLastName") != null && document.getElementById("ContentPlaceHolder1_txtShipLastName").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Shipping Last Name.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipLastName').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtShipLastName").focus();

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtshipAddressLine1") != null && document.getElementById("ContentPlaceHolder1_txtshipAddressLine1").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Shipping Address Line 1.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtshipAddressLine1').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtshipAddressLine1").focus();

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipCity") != null && document.getElementById("ContentPlaceHolder1_txtShipCity").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Shipping City.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipCity').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtShipCity").focus();

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipCounry") != null && document.getElementById("ContentPlaceHolder1_ddlShipCounry").selectedIndex == 0) {

        alert("Please Select Shipping Country.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlShipCounry').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_ddlShipCounry").focus();

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null && document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex == 0) {

        alert("Please Select Shipping State.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlShipState').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_ddlShipState").focus();

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null && document.getElementById("ContentPlaceHolder1_ddlShipState").options[document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex].value == '-11') {

        if (document.getElementById("ContentPlaceHolder1_txtShipOther") != null && document.getElementById("ContentPlaceHolder1_txtShipOther").value.replace(/^\s+|\s+$/g, "") == '') {

            alert("Please Enter Other State.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipOther').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtShipOther").focus();

            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtShipZipCode") != null && document.getElementById("ContentPlaceHolder1_txtShipZipCode").value.replace(/^\s+|\s+$/g, "") == '') {

            alert("Please Enter Shipping Zip Code.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipZipCode').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtShipZipCode").focus();

            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtShipPhone") != null && document.getElementById("ContentPlaceHolder1_txtShipPhone").value.replace(/^\s+|\s+$/g, "") == '') {

            alert("Please Enter Shipping Phone.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipPhone').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtShipPhone").focus();

            return false;
        }
        else if (!(document.getElementById('ContentPlaceHolder1_txtShipPhone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
            alert("Please enter valid Shipping Phone.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipPhone').offset().top }, 'slow');
            document.getElementById('ContentPlaceHolder1_txtShipPhone').focus();
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value == '') {

            alert("Please Enter Shipping Email.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").focus();

            return false;
        }

        else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value)) {

            alert("Please Enter valid Shipping Email.");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top }, 'slow');
            document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").focus();

            return false;
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipZipCode") != null && document.getElementById("ContentPlaceHolder1_txtShipZipCode").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Shipping Zip Code.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipZipCode').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtShipZipCode").focus();

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipPhone") != null && document.getElementById("ContentPlaceHolder1_txtShipPhone").value.replace(/^\s+|\s+$/g, "") == '') {

        alert("Please Enter Shipping Phone.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipPhone').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtShipPhone").focus();

        return false;
    }
    else if (!(document.getElementById('ContentPlaceHolder1_txtShipPhone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
        alert("Please enter valid Shipping Phone.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipPhone').offset().top }, 'slow');
        document.getElementById('ContentPlaceHolder1_txtShipPhone').focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value == '') {

        alert("Please Enter Shipping Email.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").focus();

        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value)) {

        alert("Please Enter valid Shipping Email.");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top }, 'slow');
        document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").focus();

        return false;
    }
    return true;

}


/* Start - For Show and Hide Shipping Address Panel*/
function SetBillingShippingVisible(IsVisible) {

    if (document.getElementById("ContentPlaceHolder1_UseShippingAddress") != null && document.getElementById("ContentPlaceHolder1_UseShippingAddress").checked == true) {
        if (document.getElementById("ContentPlaceHolder1_pnlShippingDetails") != null) {
            document.getElementById("ContentPlaceHolder1_pnlShippingDetails").style.display = '';
        }

    }

    else {
        if (document.getElementById("ContentPlaceHolder1_pnlShippingDetails") != null) {
          //  MakeBlank();
          //  document.getElementById("ContentPlaceHolder1_pnlShippingDetails").style.display = 'none';

        }
    }
}
/* End - For Show and Hide Shipping Address Panel*/

/* Start - For Make All fields of Shipping Address Blank */

function MakeBlank() {

    if (document.getElementById("ContentPlaceHolder1_txtShipFirstName") != null) { document.getElementById("ContentPlaceHolder1_txtShipFirstName").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipLastName") != null) { document.getElementById("ContentPlaceHolder1_txtShipLastName").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtshipAddressLine1") != null) { document.getElementById("ContentPlaceHolder1_txtshipAddressLine1").value = ''; }
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
    if (IsVisible && document.getElementById("DIVBillingOther") != null) {
        document.getElementById('DIVBillingOther').style.display = 'block';
    }
    else {
        if (document.getElementById("DIVBillingOther") != null) {
            document.getElementById('DIVBillingOther').style.display = 'none';
        }
        if (document.getElementById('ContentPlaceHolder1_txtBillOther') != null) { document.getElementById('ContentPlaceHolder1_txtBillOther').value = ''; }


    }
}
function SetShippingOtherVisible(IsVisible) {
    if (IsVisible && document.getElementById("DIVShippingOther") != null) {
        document.getElementById('DIVShippingOther').style.display = 'block';
    }
    else {
        if (document.getElementById("DIVShippingOther") != null) {
            document.getElementById('DIVShippingOther').style.display = 'none';
        }
        if (document.getElementById('ContentPlaceHolder1_txtShipOther') != null) { document.getElementById('ContentPlaceHolder1_txtShipOther').value = ''; }


    }
}
function MakeBillingOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlBillstate') && document.getElementById('ContentPlaceHolder1_ddlBillstate').options[document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex].value == '-11')
        SetBillingOtherVisible(true);
    else
        SetBillingOtherVisible(false);
}
function MakeShippingOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlShipState') && document.getElementById('ContentPlaceHolder1_ddlShipState').options[document.getElementById('ContentPlaceHolder1_ddlShipState').selectedIndex].value == '-11')
        SetShippingOtherVisible(true);
    else
        SetShippingOtherVisible(false);
}

function checkcreditcard() {

    var CardType = '';
    if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null) {
        CardType = document.getElementById('ContentPlaceHolder1_ddlCardType').options[document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex].text;
    }
    if (document.getElementById('ContentPlaceHolder1_txtNameOnCard') != null && document.getElementById('ContentPlaceHolder1_txtNameOnCard').value.replace(/^\s+|\s+$/g, "") == '') {
        alert('Please Enter Name of Card.');
        document.getElementById('ContentPlaceHolder1_txtNameOnCard').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null && document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex == 0) {
        alert('Please Select Card Type.');
        document.getElementById('ContentPlaceHolder1_ddlCardType').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.replace(/^\s+|\s+$/g, "") == '') {
        alert('Please Enter Card Number.');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && (/^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_txtCardNumber').value) == false && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.indexOf('*') < -1)) {
        alert('Please Enter valid Numeric Card Number');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }

    else if ((CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && document.getElementById('ContentPlaceHolder1_txtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.length != 15) {
        alert('Credit Card Number must be 15 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }

    else if (CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && document.getElementById('ContentPlaceHolder1_txtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.length != 16) {
        alert('Credit Card Number must be 16 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }




    else if (document.getElementById('ContentPlaceHolder1_ddlMonth') != null && document.getElementById('ContentPlaceHolder1_ddlMonth').selectedIndex == 0) {
        alert('Please Select Month.');
        document.getElementById('ContentPlaceHolder1_ddlMonth').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlYear') != null && document.getElementById('ContentPlaceHolder1_ddlYear').selectedIndex == 0) {
        alert('Please Select Year');
        document.getElementById('ContentPlaceHolder1_ddlYear').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_ddlYear') != null && document.getElementById('ContentPlaceHolder1_ddlMonth') != null) {
        var objDate = new Date();
        var year = document.getElementById('ContentPlaceHolder1_ddlYear').options[document.getElementById('ContentPlaceHolder1_ddlYear').selectedIndex].value;
        var month = document.getElementById('ContentPlaceHolder1_ddlMonth').options[document.getElementById('ContentPlaceHolder1_ddlMonth').selectedIndex].value;

        if ((year > objDate.getFullYear()) || (year == objDate.getFullYear() && month >= objDate.getMonth() + 1)) {

            if (document.getElementById('ContentPlaceHolder1_txtCSC') != null && document.getElementById('ContentPlaceHolder1_txtCSC').value.replace(/^\s+|\s+$/g, "") == '') {
                alert('Please Enter Card Verification Code.');
                document.getElementById('ContentPlaceHolder1_txtCSC').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtCSC') != null && /^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_txtCSC').value) == false) {
                alert('Please enter valid Numeric Card Verification Code');
                document.getElementById('ContentPlaceHolder1_txtCSC').focus();
                return false;
            }
            else if ((CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_txtCSC') != null && document.getElementById('ContentPlaceHolder1_txtCSC').value.length != 4 && document.getElementById('ContentPlaceHolder1_txtCSC').value != '') {
                alert('Card Verification Code must be 4 digit long');
                document.getElementById('ContentPlaceHolder1_txtCSC').focus();
                return false;
            }
            else if (CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_txtCSC') != null && document.getElementById('ContentPlaceHolder1_txtCSC').value.length != 3 && document.getElementById('ContentPlaceHolder1_txtCSC').value != '') {
                alert('Card Verification Code must be 3 digit long');
                document.getElementById('ContentPlaceHolder1_txtCSC').focus();
                return false;
            }
            //Loader();
            return true;
        }
        else {
            alert('Please Enter Valid Expiration Date.');
            document.getElementById('ContentPlaceHolder1_ddlYear').focus();
            return false;
        }
        //Loader();
        return true;
    }

    //Loader();
    return true;
}

function onMouseOverCVC() {
    document.images["CVCImage"].src = "/images/cvv.gif";
    document.images["CVCImage"].style.display = 'block';
}

function onMouseOutCVC() {
    document.images["CVCImage"].src = "";
    document.images["CVCImage"].style.display = 'none';
}

function ShowHideCreateAccDetails() {
    if (document.getElementById('ContentPlaceHolder1_chkCreateNewAccount').checked == true) {
        document.getElementById('ContentPlaceHolder1_trCreAccChangePass01').style.display = '';
        document.getElementById('ContentPlaceHolder1_trCreAccChangePass02').style.display = '';
        document.getElementById('ContentPlaceHolder1_chkReturningAcHolder').checked = false;
        document.getElementById('ContentPlaceHolder1_trReturningAccount').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_trguest').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_txtpassword').value = '';
        document.getElementById('ContentPlaceHolder1_txtCreateEmail').focus();
       
    }
    else {
        document.getElementById('ContentPlaceHolder1_trCreAccChangePass01').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_trCreAccChangePass02').style.display = 'none';

        document.getElementById('ContentPlaceHolder1_txtCreateNewPassword').value = '';
        document.getElementById('ContentPlaceHolder1_txtConfirmPassWord').value = '';
        document.getElementById('ContentPlaceHolder1_trguest').style.display = 'none';
    }
    document.getElementById('ContentPlaceHolder1_trbillrow').style.display = '';
    document.getElementById('ContentPlaceHolder1_trcardnumdetail').style.display = '';
    document.getElementById('ContentPlaceHolder1_divplaceorder').style.display = '';
    if (document.getElementById('ContentPlaceHolder1_trfeatureproduct') != null) {
        document.getElementById('ContentPlaceHolder1_trfeatureproduct').style.display = '';
    }
    

}
function getemail(id)
{
    document.getElementById('ContentPlaceHolder1_txtusername').value = document.getElementById(id).value;
}

function ShowHideGuestDetails() {
    if (document.getElementById('ContentPlaceHolder1_chkCreateNewAccountGuest').checked == true) {
        document.getElementById('ContentPlaceHolder1_trCreAccChangePass01').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_trCreAccChangePass02').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_trReturningAccount').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_trbillrow').style.display = '';
        document.getElementById('ContentPlaceHolder1_trcardnumdetail').style.display = '';
        document.getElementById('ContentPlaceHolder1_divplaceorder').style.display = '';

        document.getElementById('ContentPlaceHolder1_trguest').style.display = '';
        document.getElementById('ContentPlaceHolder1_txtCreateNewPassword').value = '';
        document.getElementById('ContentPlaceHolder1_txtConfirmPassWord').value = '';
        document.getElementById('ContentPlaceHolder1_txtGuestemail').focus();
        if (document.getElementById('ContentPlaceHolder1_trfeatureproduct') != null) {
            document.getElementById('ContentPlaceHolder1_trfeatureproduct').style.display = '';
        }
    }
    
}
function ShowHideLoginDetails() {

    if (document.getElementById('ContentPlaceHolder1_chkReturningAcHolder').checked == true) {
        document.getElementById('ContentPlaceHolder1_trReturningAccount').style.display = '';
        document.getElementById('ContentPlaceHolder1_trguest').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_chkCreateNewAccount').checked = false;
        document.getElementById('ContentPlaceHolder1_txtCreateNewPassword').value = '';
        document.getElementById('ContentPlaceHolder1_txtConfirmPassWord').value = '';
        document.getElementById('ContentPlaceHolder1_trCreAccChangePass01').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_trCreAccChangePass02').style.display = 'none';
        
        document.getElementById('ContentPlaceHolder1_txtusername').focus();
    }
    else {
        document.getElementById('ContentPlaceHolder1_trReturningAccount').style.display = 'none';
        document.getElementById('ContentPlaceHolder1_trguest').style.display = 'none';
    }
    document.getElementById('ContentPlaceHolder1_trbillrow').style.display = 'none';
    document.getElementById('ContentPlaceHolder1_trcardnumdetail').style.display = 'none';
    document.getElementById('ContentPlaceHolder1_divplaceorder').style.display = 'none';
    if (document.getElementById('ContentPlaceHolder1_trfeatureproduct') != null) {
        document.getElementById('ContentPlaceHolder1_trfeatureproduct').style.display = 'none';
    }
}


