// JScript File

/* Start - General Function for restriction */

var testresults
function checkemail1(str) {
    str = str.replace(/^\s+|\s+$/g, "");
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
function ValidatePage() {
    
    
    // For User Name and Password validation

    if (document.getElementById("ContentPlaceHolder1_txtEmail") != null && document.getElementById("ContentPlaceHolder1_txtEmail").value == '') {
       
        jAlert('Please enter User Name.', 'Message', 'ContentPlaceHolder1_txtEmail');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtEmail').offset().top }, 'slow');
        return false;
    }


    else if (document.getElementById("ContentPlaceHolder1_txtEmail") != null && document.getElementById("ContentPlaceHolder1_txtEmail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtEmail").value)) {
       
        jAlert('Please enter valid Email.', 'Message', 'ContentPlaceHolder1_txtEmail');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtEmail').offset().top }, 'slow');
        return false;
    }

   
    else if (document.getElementById("ContentPlaceHolder1_txtAltEmail") != null && document.getElementById("ContentPlaceHolder1_txtAltEmail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtAltEmail").value)) {

        jAlert('Please enter valid Alternate Email.', 'Message', 'ContentPlaceHolder1_txtAltEmail');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtAltEmail').offset().top }, 'slow');
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtAltEmail") != null && document.getElementById("ContentPlaceHolder1_txtEmail") != null && document.getElementById("ContentPlaceHolder1_txtEmail").value.replace(/^\s+|\s+$/g, '') == document.getElementById("ContentPlaceHolder1_txtAltEmail").value.replace(/^\s+|\s+$/g, '')) {

        jAlert('Email and Alternate Email should not be same.', 'Message', 'ContentPlaceHolder1_txtAltEmail');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtAltEmail').offset().top }, 'slow');
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_lblpassword") != null && document.getElementById("ContentPlaceHolder1_lblpassword").innerHTML.toString().toLowerCase() == 'password is not generated') {

        jAlert('Please re-generate password.', 'Message', 'ContentPlaceHolder1_lblpassword');
        // $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_lblpassword').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtFirstName") != null && document.getElementById("ContentPlaceHolder1_txtFirstName").value == '') {

        jAlert('Please enter First Name.', 'Message', 'ContentPlaceHolder1_txtFirstName');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtFirstName').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtLastName") != null && document.getElementById("ContentPlaceHolder1_txtLastName").value == '') {

        jAlert('Please enter Last Name.', 'Message', 'ContentPlaceHolder1_txtLastName');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtLastName').offset().top }, 'slow');
        return false;
    }

    //For Coupon code
    else if (document.getElementById("ContentPlaceHolder1_txtcouponcode") != null && document.getElementById("ContentPlaceHolder1_txtcouponcode").value != '') {

        if (document.getElementById("ContentPlaceHolder1_txtFromDate") != null && document.getElementById("ContentPlaceHolder1_txtFromDate").value != '' && document.getElementById("ContentPlaceHolder1_txtToDate") != null && document.getElementById("ContentPlaceHolder1_txtToDate").value == '') {
            jAlert('please enter to date', 'Message', 'ContentPlaceHolder1_txtToDate');
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtFromDate") != null && document.getElementById("ContentPlaceHolder1_txtFromDate").value == '' && document.getElementById("ContentPlaceHolder1_txtToDate") != null && document.getElementById("ContentPlaceHolder1_txtToDate").value != '') {
            jAlert('please enter from date', 'Message', 'ContentPlaceHolder1_txtFromDate');
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtFromDate") != null && document.getElementById("ContentPlaceHolder1_txtFromDate").value != '' && document.getElementById("ContentPlaceHolder1_txtToDate") != null && document.getElementById("ContentPlaceHolder1_txtToDate").value != '') {

            var fromDate = document.getElementById("ContentPlaceHolder1_txtFromDate").value;
            var toDate = document.getElementById("ContentPlaceHolder1_txtToDate").value;
            if (Date.parse(fromDate) > Date.parse(toDate)) {
                jAlert('please enter To date grater than or equal to from date .', 'Message', 'ContentPlaceHolder1_txtToDate');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtToDate').offset().top }, 'slow');
                return false;
            }
        }
        if (document.getElementById("ContentPlaceHolder1_txtdiscountpercent") != null && document.getElementById("ContentPlaceHolder1_txtdiscountpercent").value == '') {
            jAlert('please enter discount', 'Message', 'ContentPlaceHolder1_txtdiscountpercent');
            return false;
        }
    }

    // For Billing Details validation

    else if (document.getElementById("ContentPlaceHolder1_txtBillFirstname") != null && document.getElementById("ContentPlaceHolder1_txtBillFirstname").value == '') {

        jAlert('Please enter Billing First Name.', 'Message', 'ContentPlaceHolder1_txtBillFirstname');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillFirstname').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillLastname") != null && document.getElementById("ContentPlaceHolder1_txtBillLastname").value == '') {

        jAlert('Please enter Billing Last Name.', 'Message', 'ContentPlaceHolder1_txtBillLastname');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillLastname').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBilladdressLine1") != null && document.getElementById("ContentPlaceHolder1_txtBilladdressLine1").value == '') {

        jAlert('Please enter Billing Address Line 1.', 'Message', 'ContentPlaceHolder1_txtBilladdressLine1');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBilladdressLine1').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillCity") != null && document.getElementById("ContentPlaceHolder1_txtBillCity").value == '') {

        jAlert('Please enter Billing City.', 'Message', 'ContentPlaceHolder1_txtBillCity');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillCity').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillcountry") != null && document.getElementById("ContentPlaceHolder1_ddlBillcountry").selectedIndex == 0) {

        jAlert('Please select Billing Country.', 'Message', 'ContentPlaceHolder1_ddlBillcountry');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlBillcountry').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex == 0) {

        jAlert('Please select Billing State.', 'Message', 'ContentPlaceHolder1_ddlBillstate');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlBillstate').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate").options[document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex].value == '-11') {
        if (document.getElementById("ContentPlaceHolder1_txtBillingOtherState") != null && document.getElementById("ContentPlaceHolder1_txtBillingOtherState").value == '') {
            jAlert('Please enter Other State.', 'Message', 'ContentPlaceHolder1_txtBillingOtherState');
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillingOtherState').offset().top }, 'slow');
            return false;
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillZipCode") != null && document.getElementById("ContentPlaceHolder1_txtBillZipCode").value == '') {

        jAlert('Please enter Billing Zip Code.', 'Message', 'ContentPlaceHolder1_txtBillZipCode');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillZipCode').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillphone") != null && document.getElementById("ContentPlaceHolder1_txtBillphone").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert('Please enter Billing Phone.', 'Message', 'ContentPlaceHolder1_txtBillphone');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillphone').offset().top }, 'slow');
        return false;
    }
    else if (!(document.getElementById('ContentPlaceHolder1_txtBillphone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
        jAlert('<center>Please enter Valid Billing Phone. \n eg. 123-456-7890</center>', 'Message', 'ContentPlaceHolder1_txtBillphone');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillphone').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value == '') {

        jAlert('Please enter Billing Email.', 'Message', 'ContentPlaceHolder1_txtBillEmail');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top }, 'slow');
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtBillEmail").value)) {

        jAlert('Please enter valid Billing Email.', 'Message', 'ContentPlaceHolder1_txtBillEmail');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBillEmail').offset().top }, 'slow');
        return false;
    }

    // For Shipping Details validation

    else if (document.getElementById("ContentPlaceHolder1_txtShipFirstname") != null && document.getElementById("ContentPlaceHolder1_txtShipFirstname").value == '') {

        jAlert('Please enter Shipping First Name.', 'Message', 'ContentPlaceHolder1_txtShipFirstname');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipFirstname').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipLastname") != null && document.getElementById("ContentPlaceHolder1_txtShipLastname").value == '') {
        jAlert('Please enter Shipping Last Name.', 'Message', 'ContentPlaceHolder1_txtShipLastname');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipLastname').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipAddressLine1") != null && document.getElementById("ContentPlaceHolder1_txtShipAddressLine1").value == '') {

        jAlert('Please enter Shipping Address Line 1.', 'Message', 'ContentPlaceHolder1_txtShipAddressLine1');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipAddressLine1').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipCity") != null && document.getElementById("ContentPlaceHolder1_txtShipCity").value == '') {

        jAlert('Please enter Shipping City.', 'Message', 'ContentPlaceHolder1_txtShipCity');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipCity').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipCounry") != null && document.getElementById("ContentPlaceHolder1_ddlShipCounry").selectedIndex == 0) {

        jAlert('Please select Shipping Country.', 'Message', 'ContentPlaceHolder1_ddlShipCounry');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlShipCounry').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null && document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex == 0) {

        jAlert('Please select Shipping State.', 'Message', 'ContentPlaceHolder1_ddlShipState');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlShipState').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null && document.getElementById("ContentPlaceHolder1_ddlShipState").options[document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex].value == '-11') {

        if (document.getElementById("ContentPlaceHolder1_txtShippingOtherState") != null && document.getElementById("ContentPlaceHolder1_txtShippingOtherState").value == '') {
            jAlert('Please enter Other State.', 'Message', 'ContentPlaceHolder1_txtShippingOtherState');
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShippingOtherState').offset().top }, 'slow');
            return false;
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipZipCode") != null && document.getElementById("ContentPlaceHolder1_txtShipZipCode").value == '') {

        jAlert('Please enter Shipping Zip Code.', 'Message', 'ContentPlaceHolder1_txtShipZipCode');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipZipCode').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipPhone") != null && document.getElementById("ContentPlaceHolder1_txtShipPhone").value == '') {

        jAlert('Please enter Shipping Phone.', 'Message', 'ContentPlaceHolder1_txtShipPhone');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipPhone').offset().top }, 'slow');
        return false;
    }
    else if (!(document.getElementById('ContentPlaceHolder1_txtShipPhone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
        jAlert('<center>Please enter Valid Shipping Phone. \n eg. 123-456-7890 </center>', 'Message', 'ContentPlaceHolder1_txtShipPhone');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipPhone').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value == '') {

        jAlert('Please enter Shipping Email.', 'Message', 'ContentPlaceHolder1_txtShipEmailAddress');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top }, 'slow');
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value)) {

        jAlert('Please enter valid Shipping Email.', 'Message', 'ContentPlaceHolder1_txtShipEmailAddress');
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtShipEmailAddress').offset().top }, 'slow');
        return false;
    }
    return true;
}

/* End - All fields of Page Validation function */


/* Start - Make Same Functions*/

//For Make Same Billing Address
function MakeSameBillingAddress() {

    if (document.getElementById("ContentPlaceHolder1_txtFirstName") != null && document.getElementById("ContentPlaceHolder1_txtBillFirstname") != null) {
        document.getElementById("ContentPlaceHolder1_txtBillFirstname").value = document.getElementById("ContentPlaceHolder1_txtFirstName").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtLastName") != null && document.getElementById("ContentPlaceHolder1_txtBillLastname") != null) {
        document.getElementById("ContentPlaceHolder1_txtBillLastname").value = document.getElementById("ContentPlaceHolder1_txtLastName").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtEmail") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail") != null) {
        document.getElementById("ContentPlaceHolder1_txtBillEmail").value = document.getElementById("ContentPlaceHolder1_txtEmail").value;
    }
}

//For Make Same Shipping Address
function MakeSameShippingAddress() {

    if (document.getElementById("ContentPlaceHolder1_txtShipFirstname") != null && document.getElementById("ContentPlaceHolder1_txtBillFirstname") != null) {
        document.getElementById("ContentPlaceHolder1_txtShipFirstname").value = document.getElementById("ContentPlaceHolder1_txtBillFirstname").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtShipLastname") != null && document.getElementById("ContentPlaceHolder1_txtBillLastname") != null) {
        document.getElementById("ContentPlaceHolder1_txtShipLastname").value = document.getElementById("ContentPlaceHolder1_txtBillLastname").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtShippingCompany") != null && document.getElementById("ContentPlaceHolder1_txtBillingCompany") != null) {
        document.getElementById("ContentPlaceHolder1_txtShippingCompany").value = document.getElementById("ContentPlaceHolder1_txtBillingCompany").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtShipAddressLine1") != null && document.getElementById("ContentPlaceHolder1_txtBilladdressLine1") != null) {
        document.getElementById("ContentPlaceHolder1_txtShipAddressLine1").value = document.getElementById("ContentPlaceHolder1_txtBilladdressLine1").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtshipAddressLine2") != null && document.getElementById("ContentPlaceHolder1_txtBillAddressLine2") != null) {
        document.getElementById("ContentPlaceHolder1_txtshipAddressLine2").value = document.getElementById("ContentPlaceHolder1_txtBillAddressLine2").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtShipSuite") != null && document.getElementById("ContentPlaceHolder1_txtBillsuite") != null) {
        document.getElementById("ContentPlaceHolder1_txtShipSuite").value = document.getElementById("ContentPlaceHolder1_txtBillsuite").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtShipCity") != null && document.getElementById("ContentPlaceHolder1_txtBillCity") != null) {
        document.getElementById("ContentPlaceHolder1_txtShipCity").value = document.getElementById("ContentPlaceHolder1_txtBillCity").value;
    }
    if (document.getElementById("ContentPlaceHolder1_ddlShipCounry") != null && document.getElementById("ContentPlaceHolder1_ddlBillcountry") != null) {
        document.getElementById("ContentPlaceHolder1_ddlShipCounry").selectedIndex = document.getElementById("ContentPlaceHolder1_ddlBillcountry").selectedIndex;
    }
    if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null && document.getElementById("ContentPlaceHolder1_ddlBillstate") != null) {
        if (document.getElementById("ContentPlaceHolder1_ddlBillstate").options[document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex].value == '-11') {
            if (document.getElementById("ContentPlaceHolder1_ddlShipCounry") != null && document.getElementById("ContentPlaceHolder1_ddlBillcountry") != null) {
                SetShippingOtherVisible(false);
                document.getElementById("ContentPlaceHolder1_txtShippingOtherState").value = document.getElementById("ContentPlaceHolder1_txtBillingOtherState").value;
            }
        }
        else {
            document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex = document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex;
        }
    }
    if (document.getElementById("ContentPlaceHolder1_txtShipZipCode") != null && document.getElementById("ContentPlaceHolder1_txtBillZipCode") != null) {
        document.getElementById("ContentPlaceHolder1_txtShipZipCode").value = document.getElementById("ContentPlaceHolder1_txtBillZipCode").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtShipPhone") != null && document.getElementById("ContentPlaceHolder1_txtBillphone") != null) {
        document.getElementById("ContentPlaceHolder1_txtShipPhone").value = document.getElementById("ContentPlaceHolder1_txtBillphone").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtShippingFax") != null && document.getElementById("ContentPlaceHolder1_txtBillingFax") != null) {
        document.getElementById("ContentPlaceHolder1_txtShippingFax").value = document.getElementById("ContentPlaceHolder1_txtBillingFax").value;
    }
    if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtBillEmail") != null) {
        document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value = document.getElementById("ContentPlaceHolder1_txtBillEmail").value;
    }
}

/* End - Make Same Functions*/


function SameAsBillingOption() {
    if (document.getElementById("ContentPlaceHolder1_chkSameAsBilling") != null && document.getElementById("ContentPlaceHolder1_chkSameAsBilling").checked == true) {
        MakeSameShippingAddress();
    }
    else {
        MakeBlank();
    }
}

function SameAsOption() {
    if (document.getElementById("ContentPlaceHolder1_chkSameAsAbove") != null && document.getElementById("ContentPlaceHolder1_chkSameAsAbove").checked == true) {
        MakeSameBillingAddress();
    }
    else {
        MakeBlankSecurity();
    }
}





/* Start - For Show and Hide Shipping Address Panel*/
function SetBillingShippingVisible(IsVisible) {

    if (IsVisible && document.getElementById("ContentPlaceHolder1_pnlShippingDetails") != null) {
        document.getElementById("ContentPlaceHolder1_pnlShippingDetails").style.display = 'block';


    }
    else if (!IsVisible && document.getElementById("ContentPlaceHolder1_pnlShippingDetails") != null) {
        MakeBlank();
        document.getElementById("ContentPlaceHolder1_pnlShippingDetails").style.display = 'none';
    }
}
/* End - For Show and Hide Shipping Address Panel*/

/* Start - For Make All fields of Shipping Address Blank */

function MakeBlank() {

    if (document.getElementById("ContentPlaceHolder1_txtShipFirstname") != null) { document.getElementById("ContentPlaceHolder1_txtShipFirstname").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipLastname") != null) { document.getElementById("ContentPlaceHolder1_txtShipLastname").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShippingCompany") != null) { document.getElementById("ContentPlaceHolder1_txtShippingCompany").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipAddressLine1") != null) { document.getElementById("ContentPlaceHolder1_txtShipAddressLine1").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtshipAddressLine2") != null) { document.getElementById("ContentPlaceHolder1_txtshipAddressLine2").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipSuite") != null) { document.getElementById("ContentPlaceHolder1_txtShipSuite").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipCity") != null) { document.getElementById("ContentPlaceHolder1_txtShipCity").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_ddlShipCounry") != null) { document.getElementById("ContentPlaceHolder1_ddlShipCounry").selectedIndex = 1; }
    if (document.getElementById("ContentPlaceHolder1_ddlShipState") != null) { document.getElementById("ContentPlaceHolder1_ddlShipState").selectedIndex = 0; }
    if (document.getElementById("ContentPlaceHolder1_txtShipZipCode") != null) { document.getElementById("ContentPlaceHolder1_txtShipZipCode").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipPhone") != null) { document.getElementById("ContentPlaceHolder1_txtShipPhone").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShippingFax") != null) { document.getElementById("ContentPlaceHolder1_txtShippingFax").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtShipEmailAddress") != null) { document.getElementById("ContentPlaceHolder1_txtShipEmailAddress").value = ''; }
    SetShippingOtherVisible(false);
}

function MakeBlankSecurity() {

    if (document.getElementById("ContentPlaceHolder1_txtBillFirstname") != null) { document.getElementById("ContentPlaceHolder1_txtBillFirstname").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtBillLastname") != null) { document.getElementById("ContentPlaceHolder1_txtBillLastname").value = ''; }
    if (document.getElementById("ContentPlaceHolder1_txtBillEmail") != null) { document.getElementById("ContentPlaceHolder1_txtBillEmail").value = ''; }

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
    if (document.getElementById('ContentPlaceHolder1_ddlBillstate') != null && document.getElementById('ContentPlaceHolder1_ddlBillstate').options[document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex].value == '-11') {

        SetBillingOtherVisible(true);
    }
    else {

        SetBillingOtherVisible(false);
    }
}
function MakeShippingOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlShipState') != null && document.getElementById('ContentPlaceHolder1_ddlShipState').options[document.getElementById('ContentPlaceHolder1_ddlShipState').selectedIndex].value == '-11')
        SetShippingOtherVisible(true);
    else
        SetShippingOtherVisible(false);
}

/* End -  Make Other State Field visible True or false according to condition*/

