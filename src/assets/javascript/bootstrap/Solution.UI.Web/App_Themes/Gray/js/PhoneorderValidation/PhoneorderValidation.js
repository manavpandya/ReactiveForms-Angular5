
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
    if (ValidatePage()) {

        if (ShippingValidation()) {
            return checkcreditcard();
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

    if (ShippingValidation()) {
        return checkcreditcard();
    }
    else {
        return false;
    }

    return true;
}
function ValidationLoginuserOtherPayment() {

    if (ShippingValidation()) {
        if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
        return true;
    }
    else {
        return false;
    }
    if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
    return true;
}
function ValidationNotLoginOtherPayment() {
    if (ValidatePage()) {

        if (ShippingValidation()) {
            if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
            return true;
        }
        else {
            return false;
        }

    }
    else {
        return false;
    }
    if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
    return true;
}
function ShippingValidation() {
    var falg = false;
    if (document.getElementById('ContentPlaceHolder1_ddlShippingMethod') != null) {

        //        if (document.getElementById("ContentPlaceHolder1_ddlShippingMethod").options.length > 0) {
        //            return true;
        //        }
        //        else {

        //            jAlert('Please Select Shipping Method','Required Information','ContentPlaceHolder1_ddlShippingMethod');
        //            document.getElementById('ContentPlaceHolder1_ddlShippingMethod').focus();
        //            return false;
        //        }
        return true;
    }
    return true;
}

function ValidatePage() {


    // For User Name and Password validation

    if (document.getElementById("ContentPlaceHolder1_TxtEmail") != null && document.getElementById("ContentPlaceHolder1_TxtEmail").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Billing Email.", "Required Information", "ContentPlaceHolder1_TxtEmail");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_TxtEmail').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_TxtEmail") != null && document.getElementById("ContentPlaceHolder1_TxtEmail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_TxtEmail").value)) {

        jAlert("Please Enter valid Billing Email.", "Required Information", "ContentPlaceHolder1_TxtEmail");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_TxtEmail').offset().top }, 'slow');
        return false;
    }


    if (document.getElementById("ContentPlaceHolder1_txtB_FName") != null && document.getElementById("ContentPlaceHolder1_txtB_FName").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Billing First Name.", "Required Information", "ContentPlaceHolder1_txtB_FName");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_FName').offset().top }, 'slow');
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtB_LName") != null && document.getElementById("ContentPlaceHolder1_txtB_LName").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Billing Last Name.", "Required Information", "ContentPlaceHolder1_txtB_LName");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_LName').offset().top }, 'slow');

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtB_Add1") != null && document.getElementById("ContentPlaceHolder1_txtB_Add1").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Billing Address Line 1.", "Required Information", "ContentPlaceHolder1_txtB_Add1");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_Add1').offset().top }, 'slow');

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtB_City") != null && document.getElementById("ContentPlaceHolder1_txtB_City").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Billing City.", "Required Information", "ContentPlaceHolder1_txtB_City");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_City').offset().top }, 'slow');

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlB_Country") != null && document.getElementById("ContentPlaceHolder1_ddlB_Country").selectedIndex == 0) {

        jAlert("Please Select Billing Country.", "Required Information", "ContentPlaceHolder1_ddlB_Country");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlB_Country').offset().top }, 'slow');

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlB_State") != null && document.getElementById("ContentPlaceHolder1_ddlB_State").selectedIndex == 0) {

        jAlert("Please Select Billing State.", "Required Information", "ContentPlaceHolder1_ddlB_State");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlB_State').offset().top }, 'slow');

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlB_State") != null && document.getElementById("ContentPlaceHolder1_ddlB_State").options[document.getElementById("ContentPlaceHolder1_ddlB_State").selectedIndex].value == '-11') {

        if (document.getElementById("ContentPlaceHolder1_txtB_OtherState") != null && document.getElementById("ContentPlaceHolder1_txtB_OtherState").value.replace(/^\s+|\s+$/g, "") == '') {

            jAlert("Please Enter Other State.", "Required Information", "ContentPlaceHolder1_txtB_OtherState");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_OtherState').offset().top }, 'slow');

            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtB_Zip") != null && document.getElementById("ContentPlaceHolder1_txtB_Zip").value.replace(/^\s+|\s+$/g, "") == '') {

            jAlert("Please Enter Billing Zip Code.", "Required Information", "ContentPlaceHolder1_txtB_Zip");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_Zip').offset().top }, 'slow');

            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtB_Phone") != null && document.getElementById("ContentPlaceHolder1_txtB_Phone").value.replace(/^\s+|\s+$/g, "") == '') {

            jAlert("Please Enter Billing Phone.", "Required Information", "ContentPlaceHolder1_txtB_Phone");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_Phone').offset().top }, 'slow');

            return false;
        }
        else if (!(document.getElementById('ContentPlaceHolder1_txtB_Phone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
            jAlert("Please enter valid Billing Phone.", "Required Information", "ContentPlaceHolder1_txtB_Phone");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_Phone').offset().top }, 'slow');

            return false;
        }

        else if (document.getElementById("ContentPlaceHolder1_chkAddress") != null && document.getElementById("ContentPlaceHolder1_chkAddress").checked == false) {

            if (ValidateShippingFields()) {
                return true;
            }
            else {
                return false;
            }

        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtB_Zip") != null && document.getElementById("ContentPlaceHolder1_txtB_Zip").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Billing Zip Code.", "Required Information", "ContentPlaceHolder1_txtB_Zip");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_Zip').offset().top }, 'slow');

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtB_Phone") != null && document.getElementById("ContentPlaceHolder1_txtB_Phone").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Billing Phone.", "Required Information", "ContentPlaceHolder1_txtB_Phone");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_Phone').offset().top }, 'slow');

        return false;
    }
    else if (!(document.getElementById('ContentPlaceHolder1_txtB_Phone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
        jAlert("Please enter valid Billing Phone.", "Required Information", "ContentPlaceHolder1_txtB_Phone");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtB_Phone').offset().top }, 'slow');

        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_chkAddress") != null && document.getElementById("ContentPlaceHolder1_chkAddress").checked == false) {

        if (ValidateShippingFields()) {
            return true;
        }
        else {
            return false;
        }
        return false;
    }



    return true;
}

/* End - All fields of Page Validation function */


function ValidateShippingFields() {

    // For Shipping Details validation

    if (document.getElementById("ContentPlaceHolder1_txtS_FName") != null && document.getElementById("ContentPlaceHolder1_txtS_FName").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Shipping First Name.", "Required Information", "ContentPlaceHolder1_txtS_FName");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_FName').offset().top }, 'slow');


        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtS_LNAme") != null && document.getElementById("ContentPlaceHolder1_txtS_LNAme").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Shipping Last Name.", "Required Information", "ContentPlaceHolder1_txtS_LNAme");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_LNAme').offset().top }, 'slow');


        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtS_Add1") != null && document.getElementById("ContentPlaceHolder1_txtS_Add1").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Shipping Address Line 1.", "Required Information", "ContentPlaceHolder1_txtS_Add1");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_Add1').offset().top }, 'slow');


        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtS_City") != null && document.getElementById("ContentPlaceHolder1_txtS_City").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Shipping City.", "Required Information", "ContentPlaceHolder1_txtS_City");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_City').offset().top }, 'slow');


        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlS_Country") != null && document.getElementById("ContentPlaceHolder1_ddlS_Country").selectedIndex == 0) {

        jAlert("Please Select Shipping Country.", "Required Information", "ContentPlaceHolder1_ddlS_Country");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlS_Country').offset().top }, 'slow');


        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlS_State") != null && document.getElementById("ContentPlaceHolder1_ddlS_State").selectedIndex == 0) {

        jAlert("Please Select Shipping State.", "Required Information", "ContentPlaceHolder1_ddlS_State");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlS_State').offset().top }, 'slow');


        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlS_State") != null && document.getElementById("ContentPlaceHolder1_ddlS_State").options[document.getElementById("ContentPlaceHolder1_ddlS_State").selectedIndex].value == '-11') {


        if (document.getElementById("ContentPlaceHolder1_txtS_OtherState") != null && document.getElementById("ContentPlaceHolder1_txtS_OtherState").value.replace(/^\s+|\s+$/g, "") == '') {

            jAlert("Please Enter Other State.", "Required Information", "ContentPlaceHolder1_txtS_OtherState");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_OtherState').offset().top }, 'slow');
            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtS_Zip") != null && document.getElementById("ContentPlaceHolder1_txtS_Zip").value.replace(/^\s+|\s+$/g, "") == '') {

            jAlert("Please Enter Shipping Zip Code.", "Required Information", "ContentPlaceHolder1_txtS_Zip");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_Zip').offset().top }, 'slow');

            return false;
        }
        else if (document.getElementById("ContentPlaceHolder1_txtS_Phone") != null && document.getElementById("ContentPlaceHolder1_txtS_Phone").value.replace(/^\s+|\s+$/g, "") == '') {

            jAlert("Please Enter Shipping Phone.", "Required Information", "ContentPlaceHolder1_txtS_Phone");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_Phone').offset().top }, 'slow');

            return false;
        }
        else if (!(document.getElementById('ContentPlaceHolder1_txtS_Phone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
            jAlert("Please enter valid Shipping Phone.", "Required Information", "ContentPlaceHolder1_txtS_Phone");
            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_Phone').offset().top }, 'slow');

            return false;
        }


    }
    else if (document.getElementById("ContentPlaceHolder1_txtS_Zip") != null && document.getElementById("ContentPlaceHolder1_txtS_Zip").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Shipping Zip Code.", "Required Information", "ContentPlaceHolder1_txtS_Zip");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_Zip').offset().top }, 'slow');

        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtS_Phone") != null && document.getElementById("ContentPlaceHolder1_txtS_Phone").value.replace(/^\s+|\s+$/g, "") == '') {

        jAlert("Please Enter Shipping Phone.", "Required Information", "ContentPlaceHolder1_txtS_Phone");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_Phone').offset().top }, 'slow');

        return false;
    }
    else if (!(document.getElementById('ContentPlaceHolder1_txtS_Phone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
        jAlert("Please enter valid Shipping Phone.", "Required Information", "ContentPlaceHolder1_txtS_Phone");
        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtS_Phone').offset().top }, 'slow');

        return false;
    }

    return true;

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
        if (document.getElementById('ContentPlaceHolder1_txtB_OtherState') != null) { document.getElementById('ContentPlaceHolder1_txtB_OtherState').value = ''; }


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
        if (document.getElementById('ContentPlaceHolder1_txtS_OtherState') != null) { document.getElementById('ContentPlaceHolder1_txtS_OtherState').value = ''; }


    }
}
function MakeBillingOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlB_State') && document.getElementById('ContentPlaceHolder1_ddlB_State').options[document.getElementById('ContentPlaceHolder1_ddlB_State').selectedIndex].value == '-11')
        SetBillingOtherVisible(true);
    else
        SetBillingOtherVisible(false);
}
function MakeShippingOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlS_State') && document.getElementById('ContentPlaceHolder1_ddlS_State').options[document.getElementById('ContentPlaceHolder1_ddlS_State').selectedIndex].value == '-11')
        SetShippingOtherVisible(true);
    else
        SetShippingOtherVisible(false);
}

function checkcreditcard() {

    var CardType = '';
    var windowHeight = 0;

    if (document.getElementById('ContentPlaceHolder1_hdnischargelogic') != null && document.getElementById('ContentPlaceHolder1_hdnischargelogic').value=="1")

    {
        if (document.getElementById('ContentPlaceHolder1_rdoCreditCard') != null && document.getElementById('ContentPlaceHolder1_rdoCreditCard').checked == true) {

            if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems') != null && (parseInt(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems').rows.length) == 1)) {
                jAlert('Your Shopping Cart is Empty!', 'Sorry!');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtShippingCost') != null && document.getElementById('ContentPlaceHolder1_TxtShippingCost').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Shipping Cost.', 'Required Information', 'ContentPlaceHolder1_TxtShippingCost');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtTax') != null && document.getElementById('ContentPlaceHolder1_TxtTax').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Order Tax.', 'Required Information', 'ContentPlaceHolder1_TxtTax');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtDiscount') != null && document.getElementById('ContentPlaceHolder1_TxtDiscount').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Discount.', 'Required Information', 'ContentPlaceHolder1_TxtDiscount');
                return false;
            }
            alert("1");
            submitPayment('iframecreditcard', 'divcreditcardmsg');




            //            windowHeight = $(document).height();
            //            if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
            return false;

        }
        else
        {   if (document.getElementById('ContentPlaceHolder1_txtCheque') != null && document.getElementById('ContentPlaceHolder1_txtCheque').value.replace(/^\s+|\s+$/g, "") == '') {
            jAlert('Please Enter Reference details.', 'Required Information', 'ContentPlaceHolder1_txtCheque');
            return false;
        }
            if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems') != null && (parseInt(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems').rows.length) == 1)) {
                jAlert('Your Shopping Cart is Empty!', 'Sorry!');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtShippingCost') != null && document.getElementById('ContentPlaceHolder1_TxtShippingCost').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Shipping Cost.', 'Required Information', 'ContentPlaceHolder1_TxtShippingCost');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtTax') != null && document.getElementById('ContentPlaceHolder1_TxtTax').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Order Tax.', 'Required Information', 'ContentPlaceHolder1_TxtTax');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtDiscount') != null && document.getElementById('ContentPlaceHolder1_TxtDiscount').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Discount.', 'Required Information', 'ContentPlaceHolder1_TxtDiscount');
                return false;
            }


            //        windowHeight = $(document).height();
            //        if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
            return true;


        }
    }
    else
    {
        if (document.getElementById('ContentPlaceHolder1_rdoCreditCard') != null && document.getElementById('ContentPlaceHolder1_rdoCreditCard').checked == true) {
            if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null) {
                CardType = document.getElementById('ContentPlaceHolder1_ddlCardType').options[document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex].text;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtNameOnCard') != null && document.getElementById('ContentPlaceHolder1_TxtNameOnCard').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Name of Card.', 'Required Information', 'ContentPlaceHolder1_TxtNameOnCard');

                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null && document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex == 0) {
                jAlert('Please Select Card Type.', 'Required Information', 'ContentPlaceHolder1_ddlCardType');

                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_TxtCardNumber') != null && document.getElementById('ContentPlaceHolder1_TxtCardNumber').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Card Number.', 'Required Information', 'ContentPlaceHolder1_TxtCardNumber');

                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_TxtCardNumber') != null && (/^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_TxtCardNumber').value) == false && document.getElementById('ContentPlaceHolder1_TxtCardNumber').value.indexOf('*') < -1)) {
                jAlert('Please Enter valid Numeric Card Number', 'Required Information', 'ContentPlaceHolder1_TxtCardNumber');
                return false;
            }

            else if ((CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_TxtCardNumber') != null && document.getElementById('ContentPlaceHolder1_TxtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_TxtCardNumber').value.length != 15) {
                jAlert('Credit Card Number must be 15 digit long', 'Required Information', 'ContentPlaceHolder1_TxtCardNumber');
                return false;
            }

            else if (CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_TxtCardNumber') != null && document.getElementById('ContentPlaceHolder1_TxtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_TxtCardNumber').value.length != 16) {
                jAlert('Credit Card Number must be 16 digit long', 'Required Information', 'ContentPlaceHolder1_TxtCardNumber');
                return false;
            }

            else if (document.getElementById('ContentPlaceHolder1_ddlMonth') != null && document.getElementById('ContentPlaceHolder1_ddlMonth').selectedIndex == 0) {
                jAlert('Please Select Month.', 'Required Information', 'ContentPlaceHolder1_ddlMonth');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_ddlYear') != null && document.getElementById('ContentPlaceHolder1_ddlYear').selectedIndex == 0) {
                jAlert('Please Select Year', 'Required Information', 'ContentPlaceHolder1_ddlYear');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_ddlYear') != null && document.getElementById('ContentPlaceHolder1_ddlMonth') != null) {
                var objDate = new Date();
                var year = document.getElementById('ContentPlaceHolder1_ddlYear').options[document.getElementById('ContentPlaceHolder1_ddlYear').selectedIndex].value;
                var month = document.getElementById('ContentPlaceHolder1_ddlMonth').options[document.getElementById('ContentPlaceHolder1_ddlMonth').selectedIndex].value;

                if ((year > objDate.getFullYear()) || (year == objDate.getFullYear() && month >= objDate.getMonth() + 1)) {

                    //                if (document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode') != null && document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode').value.replace(/^\s+|\s+$/g, "") == '') {
                    //                    jAlert('Please Enter Card Verification Code.', 'Required Information', 'ContentPlaceHolder1_TxtCardVerificationCode');

                    //                    return false;
                    //                }
                    //                else 
                    if (document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode') != null && document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode').value.replace(/^\s+|\s+$/g, "") != '' && /^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode').value) == false) {
                        jAlert('Please enter valid Numeric Card Verification Code', 'Required Information', 'ContentPlaceHolder1_TxtCardVerificationCode');

                        return false;
                    }
                    else if ((CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode') != null && document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode').value.length != 4 && document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode').value != '') {
                        jAlert('Card Verification Code must be 4 digit long', 'Required Information', 'ContentPlaceHolder1_TxtCardVerificationCode');

                        return false;
                    }
                    else if (CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode') != null && document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode').value.length != 3 && document.getElementById('ContentPlaceHolder1_TxtCardVerificationCode').value != '') {
                        jAlert('Card Verification Code must be 3 digit long', 'Required Information', 'ContentPlaceHolder1_TxtCardVerificationCode');

                        return false;
                    }
                    if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems') != null && (parseInt(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems').rows.length) == 1)) {
                        jAlert('Your Shopping Cart is Empty!', 'Sorry!');
                        return false;
                    }
                    if (document.getElementById('ContentPlaceHolder1_TxtShippingCost') != null && document.getElementById('ContentPlaceHolder1_TxtShippingCost').value.replace(/^\s+|\s+$/g, "") == '') {
                        jAlert('Please Enter Shipping Cost.', 'Required Information', 'ContentPlaceHolder1_TxtShippingCost');
                        return false;
                    }
                    if (document.getElementById('ContentPlaceHolder1_TxtTax') != null && document.getElementById('ContentPlaceHolder1_TxtTax').value.replace(/^\s+|\s+$/g, "") == '') {
                        jAlert('Please Enter Order Tax.', 'Required Information', 'ContentPlaceHolder1_TxtTax');
                        return false;
                    }
                    if (document.getElementById('ContentPlaceHolder1_TxtDiscount') != null && document.getElementById('ContentPlaceHolder1_TxtDiscount').value.replace(/^\s+|\s+$/g, "") == '') {
                        jAlert('Please Enter Discount.', 'Required Information', 'ContentPlaceHolder1_TxtDiscount');
                        return false;
                    }

                    //                if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
                    return true;
                }
                else {
                    jAlert('Please Enter Valid Expiration Date.', 'Required Information', 'ContentPlaceHolder1_ddlYear');
                    return false;
                }
                if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems') != null && (parseInt(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems').rows.length) == 1)) {
                    jAlert('Your Shopping Cart is Empty!', 'Sorry!');
                    return false;
                }
                if (document.getElementById('ContentPlaceHolder1_TxtShippingCost') != null && document.getElementById('ContentPlaceHolder1_TxtShippingCost').value.replace(/^\s+|\s+$/g, "") == '') {
                    jAlert('Please Enter Shipping Cost.', 'Required Information', 'ContentPlaceHolder1_TxtShippingCost');
                    return false;
                }
                if (document.getElementById('ContentPlaceHolder1_TxtTax') != null && document.getElementById('ContentPlaceHolder1_TxtTax').value.replace(/^\s+|\s+$/g, "") == '') {
                    jAlert('Please Enter Order Tax.', 'Required Information', 'ContentPlaceHolder1_TxtTax');
                    return false;
                }
                if (document.getElementById('ContentPlaceHolder1_TxtDiscount') != null && document.getElementById('ContentPlaceHolder1_TxtDiscount').value.replace(/^\s+|\s+$/g, "") == '') {
                    jAlert('Please Enter Discount.', 'Required Information', 'ContentPlaceHolder1_TxtDiscount');
                    return false;
                }
            



                //            windowHeight = $(document).height();
                //            if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
                return true;
            }
        }
        else {
            if (document.getElementById('ContentPlaceHolder1_txtCheque') != null && document.getElementById('ContentPlaceHolder1_txtCheque').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Reference details.', 'Required Information', 'ContentPlaceHolder1_txtCheque');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems') != null && (parseInt(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems').rows.length) == 1)) {
                jAlert('Your Shopping Cart is Empty!', 'Sorry!');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtShippingCost') != null && document.getElementById('ContentPlaceHolder1_TxtShippingCost').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Shipping Cost.', 'Required Information', 'ContentPlaceHolder1_TxtShippingCost');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtTax') != null && document.getElementById('ContentPlaceHolder1_TxtTax').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Order Tax.', 'Required Information', 'ContentPlaceHolder1_TxtTax');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtDiscount') != null && document.getElementById('ContentPlaceHolder1_TxtDiscount').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Discount.', 'Required Information', 'ContentPlaceHolder1_TxtDiscount');
                return false;
            }
       

            //        windowHeight = $(document).height();
            //        if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
            return true;

        }
    }

    

    //    windowHeight = $(document).height();
    //    if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
    return true;
}

function ValidationNotLogincc() {
    if (ValidatePage()) {

        if (ShippingValidation()) {
            return checkcreditcardnew();
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

function checkcreditcardnew() {

    var CardType = '';
    var windowHeight = 0;
    if (document.getElementById('ContentPlaceHolder1_rdoCreditCard') != null && document.getElementById('ContentPlaceHolder1_rdoCreditCard').checked == true) {

               if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems') != null && (parseInt(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems').rows.length) == 1)) {
            jAlert('Your Shopping Cart is Empty!', 'Sorry!');
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_TxtShippingCost') != null && document.getElementById('ContentPlaceHolder1_TxtShippingCost').value.replace(/^\s+|\s+$/g, "") == '') {
            jAlert('Please Enter Shipping Cost.', 'Required Information', 'ContentPlaceHolder1_TxtShippingCost');
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_TxtTax') != null && document.getElementById('ContentPlaceHolder1_TxtTax').value.replace(/^\s+|\s+$/g, "") == '') {
            jAlert('Please Enter Order Tax.', 'Required Information', 'ContentPlaceHolder1_TxtTax');
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_TxtDiscount') != null && document.getElementById('ContentPlaceHolder1_TxtDiscount').value.replace(/^\s+|\s+$/g, "") == '') {
            jAlert('Please Enter Discount.', 'Required Information', 'ContentPlaceHolder1_TxtDiscount');
            return false;
        }

            




        //            windowHeight = $(document).height();
        //            if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
        return true;
        
    }
    else {
        if (document.getElementById('ContentPlaceHolder1_txtCheque') != null && document.getElementById('ContentPlaceHolder1_txtCheque').value.replace(/^\s+|\s+$/g, "") == '') {
            jAlert('Please Enter Reference details.', 'Required Information', 'ContentPlaceHolder1_txtCheque');
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems') != null && (parseInt(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems').rows.length) == 1)) {
            jAlert('Your Shopping Cart is Empty!', 'Sorry!');
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_TxtShippingCost') != null && document.getElementById('ContentPlaceHolder1_TxtShippingCost').value.replace(/^\s+|\s+$/g, "") == '') {
            jAlert('Please Enter Shipping Cost.', 'Required Information', 'ContentPlaceHolder1_TxtShippingCost');
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_TxtTax') != null && document.getElementById('ContentPlaceHolder1_TxtTax').value.replace(/^\s+|\s+$/g, "") == '') {
            jAlert('Please Enter Order Tax.', 'Required Information', 'ContentPlaceHolder1_TxtTax');
            return false;
        }
        if (document.getElementById('ContentPlaceHolder1_TxtDiscount') != null && document.getElementById('ContentPlaceHolder1_TxtDiscount').value.replace(/^\s+|\s+$/g, "") == '') {
            jAlert('Please Enter Discount.', 'Required Information', 'ContentPlaceHolder1_TxtDiscount');
            return false;
        }


        //        windowHeight = $(document).height();
        //        if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
        return true;

    }

    //    windowHeight = $(document).height();
    //    if (document.getElementById('prepage') != null) { document.getElementById('prepage').style.display = ''; }
    return true;
}