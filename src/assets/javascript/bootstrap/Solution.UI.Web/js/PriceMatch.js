function changeDiv(name) {

    var divweb = document.getElementById('knowWeb');
    var divstore = document.getElementById('knowStore');
    if (name == "Website") {
        divweb.style.display = "block";
        divstore.style.display = "none";
        if (document.getElementById('rbtweb') != null) { document.getElementById('rbtweb').checked = true; }
        if (document.getElementById('rbtstore') != null) { document.getElementById('rbtstore').checked = false; }
    }
    else if (name == "Retail Store") {
        divweb.style.display = "none";
        divstore.style.display = "block";
        if (document.getElementById('rbtweb') != null) { document.getElementById('rbtweb').checked = false; }
        if (document.getElementById('rbtstore') != null) { document.getElementById('rbtstore').checked = true; }
    }
}
    
function getPrice() {
    var totalprice = 0;
    var shipping = document.getElementById('txtShipping');
    var item = document.getElementById('hdnItemNo');
    var itemno = Number(trim(item.value));
    var price = document.getElementById('txtTotalPrice');

    for (var i = 1; i <= itemno; i++) {
        var amt = document.getElementById('txtPrice' + i);
        if (!chkNumber(trim(amt.value))) {
            alert("Please Enter Valid Price");
            amt.value = "";
            amt.focus();
            return false;
        }
        else {
            totalprice += Number(amt.value);
        }
    }
    if (trim(shipping.value) != "") {
        if (!chkNumber(trim(shipping.value))) {
            alert("Please Enter Valid Shipping Price");
            shipping.value = "";
            shipping.focus();
            return false;
        }
        else {
            totalprice += Number(shipping.value);
        }

    }
    price.value = totalprice;
}

function onSubmit() {
    var item = document.getElementById('hdnItemNo');
    var itemno = Number(trim(item.value));

    var i = 1;
    for (i = 1; i < itemno; i++) {
        
        var priceprevtemp = document.getElementById('txtPrice' + i);

         if (trim(priceprevtemp.value) == "") {
            alert("Please Fill Competitor's Price.");
            priceprevtemp.focus();
            return false;
        }
        else if (!chkNumber(trim(priceprevtemp.value))) {
            alert("Please Fill valid Competitor's Price.")
            priceprevtemp.focus();
            return false;
        }

    }

    
    var priceprev = document.getElementById('txtPrice' + itemno)
    
    if (trim(priceprev.value) == "") {
        alert("Please Fill Compititor's Price.");
        priceprev.focus();
        return false;
    }
    else {
        if (!chkNumber(trim(priceprev.value))) {
            alert("Please Fill Compititor's Price.")
            priceprev.focus();
            return false;
        }
    }
    getPrice();
    var rbtweb = document.getElementById('rbtweb');
    var rbtstore = document.getElementById('rbtstore');
    var web = document.getElementById('txtSweb');
    var store = document.getElementById('txtSstore');
    var Scity = document.getElementById('txtScity');
    var Sstate = document.getElementById('txtSstate');
    var Sphone = document.getElementById('txtSphone');
    var Name = document.getElementById('txtName');
    var Email = document.getElementById('txtEmail');
    var Phone = document.getElementById('txtPhone');
    var Zipcode = document.getElementById('txtZipcode');
    var Comment = document.getElementById('txtComment');
    if (rbtweb.checked) {
        if (trim(web.value) == "") {
            alert("Please Enter Website Address");
            web.focus();
            return false;
        }
        else {
            if (!chkURL(trim(web.value))) {
                alert("Please Enter Valid Website Address");
                web.focus();
                return false;
            }
        }
    }
    else {
        if (trim(store.value) == "") {
            alert("Please Enter Store Name");
            store.focus();
            return false;
        }
        if (trim(Scity.value) == "") {
            alert("Please Enter City of Store");
            Scity.focus();
            return false;
        }
        if (trim(Sstate.value) == "") {
            alert("Please Enter State of Store");
            Sstate.focus();
            return false;
        }
        if (trim(Sphone.value) != "") {
            if (!chkPhone(trim(Sphone.value))) {
                alert("Please Enter Valid Phone No of Store.");
                Sstate.focus();
                return false;
            }
        }
    }
    if (trim(Name.value) == "") {
        alert("Please Enter Your Name");
        Name.focus();
        return false;
    }
    if (trim(Email.value) == "") {
        alert("Please Enter Your Email-ID");
        Email.focus();
        return false;
    }
    else {
        if (!chkEmail(trim(Email.value))) {
            alert("Please Enter valid Email-Id");
            Email.focus();
            return false;
        }
    }
    if (trim(Phone.value) != "") {
        if (!chkPhone(trim(Phone.value))) {
            alert("Please Enter Your valid Phone No.");
            Phone.focus();
            return false;
        }
    }
    if (trim(Zipcode.value) == "") {
        alert("Please Enter Your Zipcode.");
        Zipcode.focus();
        return false;
    }
    else {
        if (!chkNumber(trim(Zipcode.value))) {
            alert("Please Enter Your Valid Zipcode.");
            Zipcode.focus();
            return false;
        }
    }
    if (Comment.value != "") {
        if (!chkHTML(trim(Comment.value))) {
            alert("Please dont use HTML Tag in Comment.");
            Comment.focus();
            return false;
        }
    }
}
function chkNumber(str) {
    var digits = "0123456789.";
    var temp;
    var flag = false;
    for (var i = 0; i < str.length; i++) {
        temp = str.substring(i, i + 1);
        if (digits.indexOf(temp) == -1) {
            return false;
        }
        else {
            if (temp == ".") {
                if (flag == false) {
                    flag = true;
                }
                else {
                    return false;
                }
            }
        }
    }
    return true;
}
function chkPhone(str) {
    var digits = "0123456789-";
    var temp;
    for (var i = 0; i < str.length; i++) {
        temp = str.substring(i, i + 1);
        if (digits.indexOf(temp) == -1) {
            return false;
        }
    }
    return true;
}
function chkEmail(sel) {
    var str = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
    if (sel.match(str)) {
        return true;
    }
    else {
        return false;
    }
}
function chkHTML(sel) {
    if (sel.match(/([\<])([^\>]{1,})*([\>])/i) == null) {
        return true;
    }
    else {
        return false;
    }
}
function chkURL(sel) {
    var str = "http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?";
    if (sel.match(str)) {
        return true;
    }
    else {
        return false;
    }
}
function trim(str) {
    return str.replace(/^\s+|\s+$/g, "");
}