function ReloadCapthca() {
    document.getElementById('ContentPlaceHolder1_txtCodeshow').value = '';

    var chars = "0123456789";
    var string_length = 8;
    var randomstring = '';
    for (var i = 0; i < string_length; i++) {
        var rnum = Math.floor(Math.random() * chars.length);
        randomstring += chars.substring(rnum, rnum + 1);
    }

    document.getElementById('imgcapcha').src = '/JpegImage.aspx?id=' + randomstring;
    document.getElementById('ContentPlaceHolder1_txtCodeshow').focus();
}

function ValidatePage() {
    if ((document.getElementById('ContentPlaceHolder1_txtOrderNumber').value).replace(/^\s*\s*$/g, '') == '') {
        alert('Please Enter Order Number');
        document.getElementById('ContentPlaceHolder1_txtOrderNumber').focus();
        return false;
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtEmail').value).replace(/^\s*\s*$/g, '') == '') {
        alert('Please Enter E-Mail Address');
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        return false;
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtEmail').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value)) {
        alert('Please Enter valid E-Mail Address');
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        return false;
    }
    else if ((document.getElementById('ContentPlaceHolder1_txtCodeshow').value).replace(/^\s*\s*$/g, '') == '') {
        alert('Please Enter Code Shown');
        document.getElementById('ContentPlaceHolder1_txtCodeshow').focus();
        return false;
    }
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