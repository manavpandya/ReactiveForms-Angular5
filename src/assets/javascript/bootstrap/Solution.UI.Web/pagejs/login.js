function ForLoginPopupFacebook() {
    var url = window.location.pathname;
    var myPageName = url.substring(url.lastIndexOf('/') + 1);
    if (myPageName == '')
        myPageName = 'index.aspx';
    var FacebookUrl = 'Facebook.aspx?RquestPageName=' + myPageName;
    window.location.href = FacebookUrl;
}

function ForLoginGoogle() {
    var url = window.location.pathname;
    var myPageName = url.substring(url.lastIndexOf('/') + 1);
    if (myPageName == '')
        myPageName = 'index.aspx';
    var GoogleUrl = 'GoogleLogin.aspx?googleauth=true&RquestPageName=' + myPageName;
    window.location.href = GoogleUrl;
}

function ForLoginTwitter() {
    var url = window.location.pathname;
    var myPageName = url.substring(url.lastIndexOf('/') + 1);
    if (myPageName == '')
        myPageName = 'index.aspx';
    var TwitterUrl = 'Twitter.aspx?RquestPageName=' + myPageName;
    window.location.href = TwitterUrl;
}

function checkfieldsforlogin() {


    if (document.getElementById('ContentPlaceHolder1_txtusername').value.replace(/^\s+|\s+$/g, '') == '') {
        alert('Please enter email address.');
        document.getElementById('ContentPlaceHolder1_txtusername').value = '';
        document.getElementById('ContentPlaceHolder1_txtusername').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtusername').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtusername').value)) {
        alert('Please enter valid email address.');
        document.getElementById('ContentPlaceHolder1_txtusername').value = '';
        document.getElementById('ContentPlaceHolder1_txtusername').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtpassword').value.replace(/^\s+|\s+$/g, '') == '') {
        alert('Please enter password.');
        document.getElementById('ContentPlaceHolder1_txtpassword').value = '';
        document.getElementById('ContentPlaceHolder1_txtpassword').focus();
        return false;
    }
    return true;
}

function checkfieldsforForgotpwd() {

    if (document.getElementById('ContentPlaceHolder1_txtEmail').value == '') {
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