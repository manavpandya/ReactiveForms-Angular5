
function ValidatePage() {
    var flag = false;
    var name;

    if (!document.getElementById('ContentPlaceHolder1_chkreturnpolicy').checked) {
        flag = false;
        name = 'read and understand the return policy';
        document.getElementById('ContentPlaceHolder1_chkreturnpolicy').focus();
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtOrderNumber').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Order Number';
        document.getElementById('ContentPlaceHolder1_txtOrderNumber').focus();
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
    else if ((document.getElementById('ContentPlaceHolder1_txtInvoiceDate').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Invoice Date';
        document.getElementById('ContentPlaceHolder1_txtInvoiceDate').focus();
    }
    else if (document.getElementById('ContentPlaceHolder1_txtItemReturned1') && (document.getElementById('ContentPlaceHolder1_txtItemReturned1').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Items to be Returned';
        document.getElementById('ContentPlaceHolder1_txtItemReturned1').focus();
    } else if (document.getElementById('ContentPlaceHolder1_txtItemCode1') && (document.getElementById('ContentPlaceHolder1_txtItemCode1').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Merchandise Code';
        document.getElementById('ContentPlaceHolder1_txtItemCode1').focus();
    } else if (document.getElementById('ContentPlaceHolder1_txtQuantity1') && (document.getElementById('ContentPlaceHolder1_txtQuantity1').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Quantity';
        document.getElementById('ContentPlaceHolder1_txtQuantity1').focus();
    }

    else if ((document.getElementById('ContentPlaceHolder1_txtCodeshow').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Code Shown.';
        document.getElementById('ContentPlaceHolder1_txtCodeshow').focus();
    }
    else {
        flag = true;
    }
    if (flag == false) {
        alert('Please enter ' + name);
    }
    return flag;
}
function MakeOthersVisible() {
    if ((document.getElementById('ContentPlaceHolder1_ddlB_State').options[document.getElementById('ContentPlaceHolder1_ddlB_State').selectedIndex]).text == 'Others') {
        document.getElementById('ContentPlaceHolder1_divOthers').style.display = '';
    }
    else {
        document.getElementById('ContentPlaceHolder1_divOthers').style.display = 'none';
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

    