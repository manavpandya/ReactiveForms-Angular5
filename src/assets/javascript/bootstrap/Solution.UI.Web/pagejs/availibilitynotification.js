function checkfields() {

    if (document.getElementById('ContentPlaceHolder1_txtFirstName') != null && document.getElementById('ContentPlaceHolder1_txtFirstName').value == '') {
        alert('Please enter First Name.');
        document.getElementById('ContentPlaceHolder1_txtFirstName').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtLastName') != null && document.getElementById('ContentPlaceHolder1_txtLastName').value == '') {
        alert("Please enter Last Name.");
        document.getElementById('ContentPlaceHolder1_txtLastName').focus();
        return false;
    }

    if (document.getElementById('ContentPlaceHolder1_txttelephone') != null && document.getElementById('ContentPlaceHolder1_txttelephone').value == '') {
        alert("Please enter Telephone Number.");
        document.getElementById('ContentPlaceHolder1_txttelephone').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtEmail') != null && document.getElementById('ContentPlaceHolder1_txtEmail').value == '') {
        alert("Please enter Email.");
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtEmail') != null && document.getElementById('ContentPlaceHolder1_txtEmail').value != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value)) {
        alert('Please enter valid Email.');
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        return false;
    }
    document.getElementById("prepage").style.display = '';
    return true;
}

function clearfields() {
    document.getElementById('ContentPlaceHolder1_txtFirstName').value = '';
    document.getElementById('ContentPlaceHolder1_txtLastName').value = '';
    document.getElementById('ContentPlaceHolder1_txttelephone').value = '';
    document.getElementById('ContentPlaceHolder1_txtEmail').value = '';
    document.getElementById('ContentPlaceHolder1_txtFirstName').focus();
    return false;
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
