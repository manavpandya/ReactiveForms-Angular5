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

function checkfields() {

    if (document.getElementById('ContentPlaceHolder1_txtName') != null && document.getElementById('ContentPlaceHolder1_txtName').value == '') {
        alert('Please enter Name.');
        document.getElementById('ContentPlaceHolder1_txtName').focus();
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
    if (document.getElementById('ContentPlaceHolder1_txtLink') != null && document.getElementById('ContentPlaceHolder1_txtLink').value == '') {
        alert("Please enter Link to Inappropriate Content.");
        document.getElementById('ContentPlaceHolder1_txtLink').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtMessage') != null && document.getElementById('ContentPlaceHolder1_txtMessage').value == '') {
        alert("Please enter Message.");
        document.getElementById('ContentPlaceHolder1_txtMessage').focus();
        return false;
    }

    document.getElementById("prepage").style.display = '';
    return true;
}

function clearfields() {
    document.getElementById('ContentPlaceHolder1_txtName').value = '';
    document.getElementById('ContentPlaceHolder1_txtEmail').value = '';
    document.getElementById('ContentPlaceHolder1_txtLink').value = '';
    document.getElementById('ContentPlaceHolder1_txtMessage').value = '';
    document.getElementById('ContentPlaceHolder1_txtName').focus();
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