function checkfieldsforOrderStatus() {


    if (document.getElementById('ContentPlaceHolder1_txtOrderNumber').value == '') {
        alert('Please enter Order Number.');
        document.getElementById('ContentPlaceHolder1_txtOrderNumber').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtEmail').value == '') {
        alert('Please enter email address.');
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtEmail').value != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value)) {
        alert('Please enter valid email address.');
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        return false;
    }
    return true;
}

function isNumberKeyOrderNumber(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))

        return false;


    return true;
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