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

function keyRestrict(e, validchars) {

    var key = '', keychar = '';
    key = getKeyCode(e);
    if (key == null) return true;
    keychar = String.fromCharCode(key);
    keychar = keychar.toLowerCase();
    validchars = validchars.toLowerCase();
    if (validchars.indexOf(keychar) != -1)
        return true;
    if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
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

function checkEmailAd() {
    document.getElementById('popup_container').style.visibility = 'visible';
}
function checyes() {
    document.getElementById("ContentPlaceHolder1_hdnyes").value = '1';
    document.getElementById("ContentPlaceHolder1_Button2").click();
}