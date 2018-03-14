function isNumberKeyCard(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function ValidateOneClickPage() {


    if (document.getElementById('ContentPlaceHolder1_ddlBillingAddress') != null && (document.getElementById('ContentPlaceHolder1_ddlBillingAddress').selectedIndex == 0 || document.getElementById('ContentPlaceHolder1_ddlBillingAddress').options[document.getElementById('ContentPlaceHolder1_ddlBillingAddress').selectedIndex].value == '-12')) {
        alert('Please select Billing Address.');
        document.getElementById('ContentPlaceHolder1_ddlBillingAddress').focus();
        return false;
    }

    if (document.getElementById('ContentPlaceHolder1_ddlShippingAddress') != null && (document.getElementById('ContentPlaceHolder1_ddlShippingAddress').selectedIndex == 0 || document.getElementById('ContentPlaceHolder1_ddlShippingAddress').options[document.getElementById('ContentPlaceHolder1_ddlShippingAddress').selectedIndex].value == '-12')) {
        alert('Please select Shipping Address.');
        document.getElementById('ContentPlaceHolder1_ddlShippingAddress').focus();
        return false;
    }

//    if (document.getElementById('ContentPlaceHolder1_ddlPaymentMethod') != null && (document.getElementById('ContentPlaceHolder1_ddlPaymentMethod').selectedIndex == 0 || document.getElementById('ContentPlaceHolder1_ddlPaymentMethod').options[document.getElementById('ContentPlaceHolder1_ddlPaymentMethod').selectedIndex].value == '-12')) {
//        alert('Please select  Payment Method.');
//        document.getElementById('ContentPlaceHolder1_ddlPaymentMethod').focus();
//        return false;
//    }
    return true;
}

function onMouseOverCVC() {
    document.images["CVCImage"].src = "/images/cvv.gif";
    document.images["CVCImage"].style.display = 'block';
}

function onMouseOutCVC() {
    document.images["CVCImage"].src = "";
    document.images["CVCImage"].style.display = 'none';
}

function ValidateCreditCard() {

    var CardType = '';
    CardType = document.getElementById('ContentPlaceHolder1_ddlCardType').options[document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex].text;

    if (document.getElementById('ContentPlaceHolder1_txtcardName') != null && document.getElementById('ContentPlaceHolder1_txtcardName').value == '') {
        alert('Please Enter Name of Card.');
        document.getElementById('ContentPlaceHolder1_txtcardName').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null && document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex == 0) {
        alert('Please select Card Type.');
        document.getElementById('ContentPlaceHolder1_ddlCardType').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && document.getElementById('ContentPlaceHolder1_txtCardNumber').value == '') {
        alert('Please Enter Card Number.');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && /^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_txtCardNumber').value) == false) {
        alert('Please Enter valid Numeric Card Number');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && (CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_txtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.length != 15) {
        alert('Credit Card Number must be 15 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.length != 16) {
        alert('Credit Card Number must be 16 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardVarificationCode') != null && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value == '') {
        alert('Please Enter Card Verification Code.');
        document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardVarificationCode') != null && /^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value) == false) {
        alert('Please enter valid Numeric Card Verification Code');
        document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').focus();
        return false;
    }

    else if (document.getElementById('ContentPlaceHolder1_txtCardVarificationCode') != null && (CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value.length != 4 && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value != '') {
        alert('Card Verification Code must be 4 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtCardVarificationCode') != null && CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value.length != 3 && document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').value != '') {
        alert('Card Verification Code must be 3 digit long');
        document.getElementById('ContentPlaceHolder1_txtCardVarificationCode').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlmonth') != null && document.getElementById('ContentPlaceHolder1_ddlmonth').selectedIndex == 0) {
        alert('Please select Month.');
        document.getElementById('ContentPlaceHolder1_ddlmonth').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlyear') != null && document.getElementById('ContentPlaceHolder1_ddlyear').selectedIndex == 0) {
        alert('Please select Year');
        document.getElementById('ContentPlaceHolder1_ddlyear').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_ddlmonth') != null && document.getElementById('ContentPlaceHolder1_ddlmonth').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_ddlyear') != null && document.getElementById('ContentPlaceHolder1_ddlyear').selectedIndex != 0) {

        var objDate = new Date();
        var year = document.getElementById('ContentPlaceHolder1_ddlyear').options[document.getElementById('ContentPlaceHolder1_ddlyear').selectedIndex].value;
        var month = document.getElementById('ContentPlaceHolder1_ddlmonth').options[document.getElementById('ContentPlaceHolder1_ddlmonth').selectedIndex].value;
        if ((year > objDate.getFullYear()) || (year == objDate.getFullYear() && month >= objDate.getMonth())) {
            return true;
        }
        else {
            alert('Please Enter Valid Expiration Date.');
            return false;
        }

    }
    return true;
}