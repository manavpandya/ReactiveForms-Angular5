function CopyBillOther() {

    if (document.getElementById("ContentPlaceHolder1_chkcopy") != null && document.getElementById("ContentPlaceHolder1_chkcopy").checked == true) {
        if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null) {
            var itext = document.getElementById("ContentPlaceHolder1_ddlBillstate").options[document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex].text;
            if (itext.toLowerCase() == 'other') {
                document.getElementById('DIVShippingOther').style.display = 'block';

            }
        }
    }
}
function SameAsBilling() {
    if (document.getElementById('ContentPlaceHolder1_chkcopy').checked) {
        MakeSameother();
    }
    else {
        MakeBlankother();
    }
}
function copyfrombill(idfrom, idto) {
    if (document.getElementById('ContentPlaceHolder1_chkcopy') != null && document.getElementById('ContentPlaceHolder1_chkcopy').checked == true) { document.getElementById(idto).value = document.getElementById(idfrom).value; }
}
function hidetable()
{
    if (document.getElementById('ContentPlaceHolder1_pnlShippingDetails') != null) { document.getElementById('ContentPlaceHolder1_pnlShippingDetails').style.display = 'none'; }
}
function MakeSameother() {


  if (document.getElementById('ContentPlaceHolder1_pnlShippingDetails') != null)
        document.getElementById('ContentPlaceHolder1_pnlShippingDetails').style.display = 'none';

    if (document.getElementById('ContentPlaceHolder1_txtShipFirstName') != null)
        document.getElementById('ContentPlaceHolder1_txtShipFirstName').value = document.getElementById('ContentPlaceHolder1_txtBillFirstname').value;

    if (document.getElementById('ContentPlaceHolder1_txtShipLastName') != null)
        document.getElementById('ContentPlaceHolder1_txtShipLastName').value = document.getElementById('ContentPlaceHolder1_txtBillLastname').value;

    if (document.getElementById('ContentPlaceHolder1_txtshipAddressLine1') != null)
        document.getElementById('ContentPlaceHolder1_txtshipAddressLine1').value = document.getElementById('ContentPlaceHolder1_txtBillAddressLine1').value;

    if (document.getElementById('ContentPlaceHolder1_txtshipAddressLine2') != null)
        document.getElementById('ContentPlaceHolder1_txtshipAddressLine2').value = document.getElementById('ContentPlaceHolder1_txtBillAddressLine2').value;

    if (document.getElementById('ContentPlaceHolder1_txtShipSuite') != null)
        document.getElementById('ContentPlaceHolder1_txtShipSuite').value = document.getElementById('ContentPlaceHolder1_txtBillSuite').value;

    if (document.getElementById('ContentPlaceHolder1_txtShipCity') != null)
        document.getElementById('ContentPlaceHolder1_txtShipCity').value = document.getElementById('ContentPlaceHolder1_txtBillCity').value;


    if (document.getElementById('ContentPlaceHolder1_ddlShipState'))
        document.getElementById('ContentPlaceHolder1_tempbillid').value = document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex;


    if (document.getElementById('ContentPlaceHolder1_txtShipZipCode') != null)
        document.getElementById('ContentPlaceHolder1_txtShipZipCode').value = document.getElementById('ContentPlaceHolder1_txtBillZipCode').value;

    if (document.getElementById('ContentPlaceHolder1_txtShipPhone') != null)
        document.getElementById('ContentPlaceHolder1_txtShipPhone').value = document.getElementById('ContentPlaceHolder1_txtBillphone').value;

    if (document.getElementById('ContentPlaceHolder1_txtShipEmailAddress') != null)
        document.getElementById('ContentPlaceHolder1_txtShipEmailAddress').value = document.getElementById('ContentPlaceHolder1_txtBillEmail').value;

    try {
        if (document.getElementById('ContentPlaceHolder1_ddlShipState'))
            document.getElementById('ContentPlaceHolder1_ddlShipState').selectedIndex = document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex;
        if ((document.getElementById('ContentPlaceHolder1_ddlBillstate').options[document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex]).text == 'Other') {
            document.getElementById('DIVShippingOther').style.display = 'block';
            document.getElementById('ContentPlaceHolder1_txtShipOther').value = document.getElementById('ContentPlaceHolder1_txtBillother').value;
        }
    }
    catch (err) {
    }

    if (document.getElementById('ContentPlaceHolder1_ddlShipCounry') != null) {
        document.getElementById('ContentPlaceHolder1_ddlShipCounry').selectedIndex = document.getElementById('ContentPlaceHolder1_ddlBillcountry').selectedIndex;
        __doPostBack('ctl00$ContentPlaceHolder1$ddlShipCounry', '');
    }



}
function chkHeightcredit() {
    var windowHeight = 0;
   // windowHeight = $(document).height(); //window.innerHeight;
    var hh = $('#ContentPlaceHolder1_trPaymentMethods').innerHeight();
    hh = hh + 45;
    document.getElementById('prepagecredit').style.height = hh + 'px';
    document.getElementById('prepagecredit').style.display = '';
}

function Loadercredit() {

    chkHeightcredit();
}

function MakeBlankother() {

 if (document.getElementById('ContentPlaceHolder1_hdnischargelogic') != null && document.getElementById('ContentPlaceHolder1_hdnischargelogic').value == "1") {
        if (document.getElementById('ContentPlaceHolder1_hdnNext') != null) {
           
            document.getElementById('ContentPlaceHolder1_hdnNext').value = "1";
            
            document.getElementById("ContentPlaceHolder1_trshipping").style.display = "none";
            
            document.getElementById("ContentPlaceHolder1_trPaymentMethods").style.display = "none";
           document.getElementById("ContentPlaceHolder1_trordernotes").style.display = "none";
            document.getElementById("ContentPlaceHolder1_divplaceorder").style.display = "none";
           

            document.getElementById("ContentPlaceHolder1_btnNextAddress").style.display = '';
           
            document.getElementById("ContentPlaceHolder1_btnNextShipping").style.display = "none";
           
            
        }
    }

  if (document.getElementById('ContentPlaceHolder1_pnlShippingDetails') != null)
        document.getElementById('ContentPlaceHolder1_pnlShippingDetails').style.display = '';

    document.getElementById('ContentPlaceHolder1_txtShipFirstName').value = "";
    document.getElementById('ContentPlaceHolder1_txtShipLastName').value = "";
    document.getElementById('ContentPlaceHolder1_txtshipAddressLine1').value = "";
    document.getElementById('ContentPlaceHolder1_txtshipAddressLine2').value = "";
    document.getElementById('ContentPlaceHolder1_txtShipSuite').value = "";
    document.getElementById('ContentPlaceHolder1_txtShipCity').value = "";
    document.getElementById('ContentPlaceHolder1_ddlShipCounry').options[document.getElementById('ContentPlaceHolder1_ddlShipCounry').selectedIndex].text == 'United States';
    document.getElementById('ContentPlaceHolder1_ddlShipState').selectedIndex = 0;

    //if (document.getElementById('DIVBillingOther'))
        //document.getElementById('DIVBillingOther').style.display = "none";
    document.getElementById('ContentPlaceHolder1_txtShipOther').value = "";
    document.getElementById('DIVShippingOther').style.display = "none";
    document.getElementById('ContentPlaceHolder1_txtShipZipCode').value = "";
    document.getElementById('ContentPlaceHolder1_txtShipPhone').value = "";
    document.getElementById('ContentPlaceHolder1_txtShipEmailAddress').value = "";
    document.getElementById('ContentPlaceHolder1_tempbillid').value = "";
   

}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        alert('Enter Valid Digit Only!');
        return false;
    }

    return true;
}

$(document).ready(function () {
    $('#ContentPlaceHolder1_txtCardNumber').bind('copy paste', function (e) {
        e.preventDefault();
    });

});


function validation(textId) {
    var a = document.getElementById(textId).value.replace(/^\s+|\s+$/g, "");
    if (a == "") {
        alert('Enter Promo Code!');
        document.getElementById(textId).focus();
        return false;
    }
    Loader();
document.getElementById('ContentPlaceHolder1_hdnupdatecoupon').value = '1';
    document.getElementById('ContentPlaceHolder1_btnApply').click();
    return false;
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
function LoaderShipping() {

    var windowHeight = 0;
    windowHeight = $('#tblshippheight').innerHeight(); //window.innerHeight;

    document.getElementById('prepage3').style.height = windowHeight + 'px';
    document.getElementById('prepage3').style.display = '';
    //chkHeight();
}

function ShippingMethodGet() {
    document.getElementById('ContentPlaceHolder1_updateDiv').style.display = 'block';
    return true;
}

function checkfieldsforlogin() {
    if (document.getElementById('ContentPlaceHolder1_chkaccept') != null && document.getElementById('ContentPlaceHolder1_chkaccept').checked == false) {
        alert('Please accept terms and conditions.');
        document.getElementById('ContentPlaceHolder1_chkaccept').focus();
        return false;
    }
    else if (document.getElementById('ContentPlaceHolder1_txtusername').value.replace(/^\s+|\s+$/g, '') == '') {
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

function copyemail(fromemail, toemail, toemail1) {
    if (document.getElementById(fromemail) != null) {
        document.getElementById('ContentPlaceHolder1_txtusername').value = document.getElementById(fromemail).value;
        document.getElementById(toemail).value = document.getElementById(fromemail).value;
        document.getElementById(toemail1).value = document.getElementById(fromemail).value;
    }
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