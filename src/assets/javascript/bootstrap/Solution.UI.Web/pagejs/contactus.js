function ReloadCapthca() {
    document.getElementById('ContentPlaceHolder1_txtShowcode').value = '';

    var chars = "0123456789";
    var string_length = 8;
    var randomstring = '';
    for (var i = 0; i < string_length; i++) {
        var rnum = Math.floor(Math.random() * chars.length);
        randomstring += chars.substring(rnum, rnum + 1);
    }
    document.getElementById('imgcapcha').src = '/JpegImage.aspx?id=' + randomstring;
    document.getElementById('ContentPlaceHolder1_txtShowcode').focus();
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

    else if ((document.getElementById('ContentPlaceHolder1_txtinformation').value).replace(/^\s*\s*$/g, '') == '') {
        flag = false;
        name = 'Message';
        document.getElementById('ContentPlaceHolder1_txtinformation').focus();
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