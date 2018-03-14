function checkfields() {

    if (document.getElementById('ContentPlaceHolder1_txtOldPassword').value == '') {
        alert('Please enter Old Password.');
        document.getElementById('ContentPlaceHolder1_txtOldPassword').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtpassword').value == '') {
        alert('Please enter New Password.');
        document.getElementById('ContentPlaceHolder1_txtpassword').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtpassword').value.length < 6) {
        alert('Password length must be at least 6 character long.');
        document.getElementById('ContentPlaceHolder1_txtpassword').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtconfirmpassword').value == '') {
        alert('Please enter Confirm Password.');
        document.getElementById('ContentPlaceHolder1_txtconfirmpassword').focus();
        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtpassword').value != document.getElementById('ContentPlaceHolder1_txtconfirmpassword').value) {
        alert('Confirm Password must be match with Password.');
        document.getElementById('ContentPlaceHolder1_txtconfirmpassword').focus();
        return false;
    }
    return true;
}