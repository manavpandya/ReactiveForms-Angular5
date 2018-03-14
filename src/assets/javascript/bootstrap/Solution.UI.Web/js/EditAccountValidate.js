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
function ValidatePage() {


    // For Edit Account Details validation

    if (document.getElementById("ContentPlaceHolder1_txtFirstname") != null && document.getElementById("ContentPlaceHolder1_txtFirstname").value == '') {

        alert("Please enter First Name.");
        document.getElementById("ContentPlaceHolder1_txtFirstname").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtLastname") != null && document.getElementById("ContentPlaceHolder1_txtLastname").value == '') {

        alert("Please enter Last Name.");
        document.getElementById("ContentPlaceHolder1_txtLastname").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtaddressLine1") != null && document.getElementById("ContentPlaceHolder1_txtaddressLine1").value == '') {

        alert("Please enter Address Line 1.");
        document.getElementById("ContentPlaceHolder1_txtaddressLine1").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtCity") != null && document.getElementById("ContentPlaceHolder1_txtCity").value == '') {

        alert("Please enter City.");
        document.getElementById("ContentPlaceHolder1_txtCity").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlcountry") != null && document.getElementById("ContentPlaceHolder1_ddlcountry").selectedIndex == 0) {

        alert("Please select Country.");
        document.getElementById("ContentPlaceHolder1_ddlcountry").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlstate") != null && document.getElementById("ContentPlaceHolder1_ddlstate").selectedIndex == 0) {

        alert("Please select State.");
        document.getElementById("ContentPlaceHolder1_ddlstate").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_ddlstate") != null && document.getElementById("ContentPlaceHolder1_ddlstate").options[document.getElementById("ContentPlaceHolder1_ddlstate").selectedIndex].value == '-11') {

        if (document.getElementById("ContentPlaceHolder1_txtOtherState") != null && document.getElementById("ContentPlaceHolder1_txtOtherState").value == '') {

            alert("Please enter Other State.");
            document.getElementById("ContentPlaceHolder1_txtOtherState").focus();
            return false;
        }
    }
    else if (document.getElementById("ContentPlaceHolder1_txtZipCode") != null && document.getElementById("ContentPlaceHolder1_txtZipCode").value == '') {

        alert("Please enter Zip Code.");
        document.getElementById("ContentPlaceHolder1_txtZipCode").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtphone") != null && document.getElementById("ContentPlaceHolder1_txtphone").value == '') {

        alert("Please enter Phone.");
        document.getElementById("ContentPlaceHolder1_txtphone").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtEmail") != null && document.getElementById("ContentPlaceHolder1_txtEmail").value == '') {

        alert("Please enter Email.");
        document.getElementById("ContentPlaceHolder1_txtEmail").focus();
        return false;
    }

    else if (document.getElementById("ContentPlaceHolder1_txtEmail") != null && document.getElementById("ContentPlaceHolder1_txtEmail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtEmail").value)) {

        alert("Please enter valid Email.");
        document.getElementById("ContentPlaceHolder1_txtEmail").focus();
        return false;
    }
    return true;
}

/* End - All fields of Page Validation function */


/* Start -  Make Other State Field visible True or false according to condition*/
function SetOtherVisible(IsVisible) {
    if (IsVisible) {
        if (document.getElementById('DIVOther') != null) { document.getElementById('DIVOther').style.display = 'block'; }
    }
    else {

        if (document.getElementById('ctl00_ContentPlaceHolder1_txtOtherState') != null) { document.getElementById('ctl00_ContentPlaceHolder1_txtOtherState').value = ''; }
        if (document.getElementById('DIVOther') != null) { document.getElementById('DIVOther').style.display = 'none'; }

    }
}

function MakeOtherVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlstate') != null && document.getElementById('ContentPlaceHolder1_ddlstate').options[document.getElementById('ContentPlaceHolder1_ddlstate').selectedIndex].value == '-11')
        SetOtherVisible(true);
    else
        SetOtherVisible(false);
}


/* End -  Make Other State Field visible True or false according to condition*/

