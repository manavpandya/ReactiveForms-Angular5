function checkFields() {

    if (document.getElementById('ContentPlaceHolder1_ddlcountry') != null && document.getElementById('ContentPlaceHolder1_ddlcountry').selectedIndex == 0) {
        jAlert('Please select Country.', 'Required Information', 'ContentPlaceHolder1_ddlcountry');

        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtZipCode') != null && document.getElementById('ContentPlaceHolder1_txtZipCode').value.replace(/^\s+|\s+$/g, "") == '') {
        jAlert('Please enter Zip Code.', 'Required Information', 'ContentPlaceHolder1_txtZipCode');

        return false;
    }

    document.getElementById("prepage").style.display = 'block';
    return true;
}

function getprice() {
    if (window.parent.document.getElementById('subtotal') != null) {
        if (parseFloat(window.parent.document.getElementById('subtotal').innerHTML.replace('$', '').replace(/,/g, '')) > parseFloat(0)) {
            document.getElementById('hdnprice').value = window.parent.document.getElementById('subtotal').innerHTML.toString().replace('$', '').replace(/,/g, '');
        }
        else {
            if (window.parent.document.getElementById('spnYourPrice') != null) {
                document.getElementById('hdnprice').value = window.parent.document.getElementById('spnYourPrice').innerHTML.toString().replace('$', '').replace(/,/g, '');
            }
            else if (window.parent.document.getElementById('spnRegularPrice') != null) {
                document.getElementById('hdnprice').value = window.parent.document.getElementById('spnRegularPrice').innerHTML.toString().replace('$', '').replace(/,/g, '');
            }
        }
    }
    else {
        if (window.parent.document.getElementById('spnYourPrice') != null) {
            document.getElementById('hdnprice').value = window.parent.document.getElementById('spnYourPrice').innerHTML.toString().replace('$', '').replace(/,/g, '');
        }
        else if (window.opener.document.getElementById('spnRegularPrice') != null) {
            document.getElementById('hdnprice').value = window.parent.document.getElementById('spnRegularPrice').innerHTML.toString().replace('$', '').replace(/,/g, '');
        }
    }

}

function isNumberKeyCard(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}