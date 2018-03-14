function chkURL(sel) {
    var str = "http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?";
    if (sel.match(str)) {
        return true;
    }
    else {
        return false;
    }
}
function trim(str) {
    return str.replace(/^\s+|\s+$/g, "");
}

function ValidateOnlineForm() {
    var flag = false;
    var name;
    var web = document.getElementById('ContentPlaceHolder1_txtWebSite');
    if ((document.getElementById('ContentPlaceHolder1_txtCompany').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Company Name';
        document.getElementById('ContentPlaceHolder1_txtCompany').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtStreet').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Street Name';
        document.getElementById('ContentPlaceHolder1_txtStreet').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtCity').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'City Name';
        document.getElementById('ContentPlaceHolder1_txtCity').focus();
    }

    else if ((document.getElementById('ContentPlaceHolder1_txtzipcode').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Zip Code';
        document.getElementById('ContentPlaceHolder1_txtzipcode').focus();
    }
    else if (trim(web.value) == "") {
        flag = false;
        name = 'Web site Address';
        web.focus();
    }
    else if (!chkURL(trim(web.value))) {
        flag = false;
        name = 'valid Web site Address';
        web.focus();
    }

    else if ((document.getElementById('ContentPlaceHolder1_txtM1FirstName').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'First Name';
        document.getElementById('ContentPlaceHolder1_txtM1FirstName').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtM1LastName').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Last Name';
        document.getElementById('ContentPlaceHolder1_txtM1LastName').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtM1Title').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Title';
        document.getElementById('ContentPlaceHolder1_txtM1Title').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtM1Email').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'E-Mail Address';
        document.getElementById('ContentPlaceHolder1_txtM1Email').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtM1Email').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtM1Email').value)) {
        flag = false;
        name = 'valid E-Mail Address';
        document.getElementById('ContentPlaceHolder1_txtM1Email').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtM1Phone').value) == '') {
        flag = false;
        name = 'Phone';
        document.getElementById('ContentPlaceHolder1_txtM1Phone').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtNoOfEmployee').value) == '') {
        flag = false;
        name = 'No Of Employees';
        document.getElementById('ContentPlaceHolder1_txtNoOfEmployee').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_ddlProjectType').options[document.getElementById('ContentPlaceHolder1_ddlProjectType').selectedIndex]).text == 'Project Type') {
        ValidationMsg = 'Please Project Type.';
        alert('Please select Project Type.');
        document.getElementById('ContentPlaceHolder1_ddlProjectType').focus();
        return false;
    }

    else {
        var windowHeight = 0;
        windowHeight = $(document).height(); //window.innerHeight;
        document.getElementById('prepage').style.height = windowHeight + 'px';
        document.getElementById('prepage').style.display = '';
        flag = true;
    }

    if (flag == false) {
        alert('Please enter ' + name + '.');
    }

    return flag;
}

function ValidatePage() {
    var flag = false;
    var name;
    if ((document.getElementById('ContentPlaceHolder1_txtname').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Name';
        document.getElementById('ContentPlaceHolder1_txtname').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtEmail').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'E-Mail Address';
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtEmail').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value)) {
        flag = false;
        name = 'valid E-Mail Address';
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_ddlcountry').options[document.getElementById('ContentPlaceHolder1_ddlcountry').selectedIndex]).text == 'Select Country') {
        ValidationMsg = 'Please Select Country.';
        alert('Please select Country.');
        document.getElementById('ContentPlaceHolder1_ddlcountry').focus();
        return false;
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtzipcode').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Zip Code';
        document.getElementById('ContentPlaceHolder1_txtzipcode').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtzipcode').value).replace(/^\s*\s*$/g, '') != '' && (document.getElementById('ContentPlaceHolder1_ddlcountry').options[document.getElementById('ContentPlaceHolder1_ddlcountry').selectedIndex]).text == 'United States' && (document.getElementById('ContentPlaceHolder1_txtzipcode').value.length != 5 || isNaN(document.getElementById('ContentPlaceHolder1_txtzipcode').value))) {
        flag = false;
        alert('Zip Code must be 5 digit long and Numeric.');
        document.getElementById('ContentPlaceHolder1_txtzipcode').focus();
        return false;
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtinformation').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Specific Question/Comment';
        document.getElementById('ContentPlaceHolder1_txtinformation').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtShowcode').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Code Shown';
        document.getElementById('ContentPlaceHolder1_txtShowcode').focus();
    }
    else {
        var windowHeight = 0;
        windowHeight = $(document).height(); //window.innerHeight;
        document.getElementById('prepage').style.height = windowHeight + 'px';
        document.getElementById('prepage').style.display = '';
        flag = true;
    }

    if (flag == false) {
        alert('Please enter ' + name + '.');
    }
    
    return flag;
}

/* Start -  Make Other State Field visible True or false according to condition*/

function SetOthersVisible(IsVisible) {
    if (IsVisible && document.getElementById('divOthers') != null) {
        document.getElementById('divOthers').style.display = 'block';
    }
    else {
        if (document.getElementById('ContentPlaceHolder1_txtother') != null) { document.getElementById('ContentPlaceHolder1_txtother').value = ''; }
        if (document.getElementById('divOthers') != null) { document.getElementById('divOthers').style.display = 'none'; }
    }
}

function MakeOthersVisible() {
    if (document.getElementById('ContentPlaceHolder1_ddlstate') != null && document.getElementById('ContentPlaceHolder1_ddlstate').options[document.getElementById('ContentPlaceHolder1_ddlstate').selectedIndex].value == '-11')
        SetOthersVisible(true);
    else
        SetOthersVisible(false);
}

/* End -  Make Other State Field visible True or false according to condition*/

function keyRestrict(e, validchars) {
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

function vitxt() {
    var val = document.getElementById("ContentPlaceHolder1_ddlstate").value;
    if (val == -11) {
        document.getElementById('ContentPlaceHolder1_divother').style.display = "";
    }
    else {
        document.getElementById('ContentPlaceHolder1_divother').style.display = "none";
        document.getElementById('ContentPlaceHolder1_txtother').value = '';
    }
}

function onKeyPressPhone(e) {
    var key = window.event ? window.event.keyCode : e.which;

    if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45) {
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

function checkemail1(str) {
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
    if (filter.test(str))
        testresults = true
    else {
        testresults = false
    }
    return (testresults)
}

function isNumberKey(event) {
    document.getElementById("ContentPlaceHolder1_txtname").value = document.getElementById("ContentPlaceHolder1_txtname").value.replace(/^\s+/, "");
    var retval = false;
    var charCode = (event.which) ? event.which : (window.event) ? window.event.keyCode : -1;
    if (charCode == -1 || charCode > 31 && (charCode < 33 || charCode > 64) && (charCode < 91 || charCode > 96) && (charCode < 123 || charCode > 126))
        retval = true;
    if (charCode == 8)
        retval = true;
    if (navigator.appName.indexOf('Microsoft') != -1)
        window.event.returnValue = retval;
    return retval;
}



