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
function chkHeight() {

    var windowHeight = 0;
    windowHeight = $(document).height();
    document.getElementById('prepage').style.height = windowHeight + 'px';
    document.getElementById('prepage').style.display = '';
}

function checkfields() {

    if (document.getElementById('ContentPlaceHolder1_txtname') != null && document.getElementById('ContentPlaceHolder1_txtname').value == '') {
        alert('Please enter your name.');
        document.getElementById('ContentPlaceHolder1_txtname').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtEmail') != null && document.getElementById('ContentPlaceHolder1_txtEmail').value == '') {
        alert("Please enter your email");
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtEmail') != null && document.getElementById('ContentPlaceHolder1_txtEmail').value != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value)) {
        alert('Please enter valid email.');
        document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtReEmail') != null && document.getElementById('ContentPlaceHolder1_txtReEmail').value == '') {
        alert("Please enter re-enter email.");
        document.getElementById('ContentPlaceHolder1_txtReEmail').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtReEmail') != null && document.getElementById('ContentPlaceHolder1_txtReEmail').value != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtReEmail').value)) {
        alert('Please enter valid re-enter email.');
        document.getElementById('ContentPlaceHolder1_txtReEmail').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtEmail') != null && document.getElementById('ContentPlaceHolder1_txtReEmail') != null && document.getElementById('ContentPlaceHolder1_txtEmail').value != document.getElementById('ContentPlaceHolder1_txtReEmail').value) {
        alert('Re-enter email must be same with your email.');
        document.getElementById('ContentPlaceHolder1_txtReEmail').focus();
        return false;
    }

    if (document.getElementById('ContentPlaceHolder1_txtEmail1') != null && document.getElementById('ContentPlaceHolder1_txtEmail1').value == '') {
        alert("Please enter Email1");
        document.getElementById('ContentPlaceHolder1_txtEmail1').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtEmail1') != null && document.getElementById('ContentPlaceHolder1_txtEmail1').value != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail1').value)) {
        alert('Please enter valid Email1.');
        document.getElementById('ContentPlaceHolder1_txtEmail1').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtEmail') != null && document.getElementById('ContentPlaceHolder1_txtEmail1') != null && document.getElementById('ContentPlaceHolder1_txtEmail').value == document.getElementById('ContentPlaceHolder1_txtEmail1').value) {
        alert('Your email and Email1 can not be same.');
        document.getElementById('ContentPlaceHolder1_txtEmail1').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtEmail2') != null && document.getElementById('ContentPlaceHolder1_txtEmail2').value != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail2').value)) {
        alert('Please enter valid Email2.');
        document.getElementById('ContentPlaceHolder1_txtEmail2').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtEmail3') != null && document.getElementById('ContentPlaceHolder1_txtEmail3').value != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail3').value)) {
        alert('Please enter valid Email3.');
        document.getElementById('ContentPlaceHolder1_txtEmail3').focus();
        return false;
    }

    chkHeight();
    return true;
}

function clearfields() {
    if (document.getElementById('ContentPlaceHolder1_txtname') != null) { document.getElementById('ContentPlaceHolder1_txtname').value = ''; }
    if (document.getElementById('ContentPlaceHolder1_txtEmail') != null) { document.getElementById('ContentPlaceHolder1_txtEmail').value = ''; }
    if (document.getElementById('ContentPlaceHolder1_txtReEmail') != null) { document.getElementById('ContentPlaceHolder1_txtReEmail').value = ''; }
    if (document.getElementById('ContentPlaceHolder1_txtEmail1') != null) { document.getElementById('ContentPlaceHolder1_txtEmail1').value = ''; }
    if (document.getElementById('ContentPlaceHolder1_txtEmail2') != null) { document.getElementById('ContentPlaceHolder1_txtEmail2').value = ''; }
    if (document.getElementById('ContentPlaceHolder1_txtEmail3') != null) { document.getElementById('ContentPlaceHolder1_txtEmail3').value = ''; }
    if (document.getElementById('ContentPlaceHolder1_txtMessage') != null) { document.getElementById('ContentPlaceHolder1_txtMessage').value = 'I thought you would be interested in this item at HalfPriceDrapes Enjoy!'; }
    if (document.getElementById('ContentPlaceHolder1_txtname') != null) { document.getElementById('ContentPlaceHolder1_txtname').focus(); }
    return false;
}