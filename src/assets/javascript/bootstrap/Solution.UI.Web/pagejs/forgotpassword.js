function checkEmailAd() {
    document.getElementById('popup_container').style.visibility = 'visible';
}
function checyes() {
    document.getElementById("ContentPlaceHolder1_hdnyes").value = '1';
    document.getElementById("ContentPlaceHolder1_Button2").click();
}

function checkfields() {

    if (document.getElementById('ContentPlaceHolder1_txtshowcode').value == '') {
        alert('Please Enter Shown Code.');
        document.getElementById('ContentPlaceHolder1_txtshowcode').focus();
        return false;
    }
    return true;
}

function ReloadCapthca() {
    document.getElementById('ContentPlaceHolder1_txtshowcode').value = '';

    var chars = "0123456789";
    var string_length = 8;
    var randomstring = '';
    for (var i = 0; i < string_length; i++) {
        var rnum = Math.floor(Math.random() * chars.length);
        randomstring += chars.substring(rnum, rnum + 1);
    }

    document.getElementById('imgcapcha').src = '/JpegImage.aspx?id=' + randomstring;
    document.getElementById('ContentPlaceHolder1_txtshowcode').focus();
}