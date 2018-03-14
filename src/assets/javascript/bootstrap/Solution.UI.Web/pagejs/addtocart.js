function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        alert('Enter Valid Digit Only!');
        return false;
    }

    return true;
}
function validation(textId) {
    var a = document.getElementById(textId).value.replace(/^\s+|\s+$/g, "");
    if (a == "") {
        alert('Enter Promo Code!');
        document.getElementById(textId).focus();
        return false;
    }
    Loader();
    document.getElementById('ContentPlaceHolder1_btnApply').click();
    return false;
}
function Shipvalidation() {
    if (document.getElementById('ContentPlaceHolder1_ddlcountry') != null && document.getElementById('ContentPlaceHolder1_ddlcountry').selectedIndex == 0) {
        alert('Please Select Country.');
        document.getElementById('ContentPlaceHolder1_ddlcountry').focus();
        return false;

    }
    else if (document.getElementById('ContentPlaceHolder1_txtZipCode') != null && document.getElementById('ContentPlaceHolder1_txtZipCode').value.replace(/^\s+|\s+$/g, "") == '') {
        alert('Please Enter ZipCode.');
        document.getElementById('ContentPlaceHolder1_txtZipCode').focus();
        return false;
    }

    Loader();


    return true;
}

function chkHeight() {
    var windowHeight = 0;
    windowHeight = $(document).height(); //window.innerHeight;

    document.getElementById('prepage').style.height = windowHeight + 'px';
    document.getElementById('prepage').style.display = '';
}

function Loader() {

    chkHeight();
}