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
