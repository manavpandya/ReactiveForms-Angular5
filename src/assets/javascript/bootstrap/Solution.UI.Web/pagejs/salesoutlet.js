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

function unselectcheckbox(chkelement) {
    var allElts = document.getElementById('divPrice').getElementsByTagName('INPUT');
    var i;
    for (i = 0; i < allElts.length; i++) {
        var elt = allElts[i];
        if (elt.type == "checkbox") {
            if (elt.name != chkelement) {
                elt.checked = false;
            }
        }
    }
    CheckSelection();
}

//for newsletter
function ValidNewSletter() {
    var element; if (document.getElementById('txtSubscriber'))
    { element = document.getElementById('txtSubscriber'); }
    if (document.getElementById('txtSubscriber'))
    { element = document.getElementById('txtSubscriber'); }
    if (element.value == '') {
        alert('Please enter your E-mail Address.'); if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        return false;
    }
    else if (element.value == 'Enter your E-Mail Address') {
        alert('Please enter your E-Mail Address.'); if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        if (document.getElementById('txtSubscriber'))
        { document.getElementById('txtSubscriber').focus(); }
        return false;
    }
    else {
        var testresults; var str = element.value; var filter = /^.+@.+\..{2,3}$/; if (filter.test(str))
        { return true; }
        else {
            alert("Please enter valid E-Mail Address.")
            if (document.getElementById('txtSubscriber'))
            { document.getElementById('txtSubscriber').focus(); }
            if (document.getElementById('txtSubscriber'))
            { document.getElementById('txtSubscriber').focus(); }
            return false;
        }
    }
}
function clear_text() {
    if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value == "Enter your E-Mail Address")
    { document.getElementById('txtSubscriber').value = ""; }
    if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value == "Enter your E-Mail Address")
    { document.getElementById('txtSubscriber').value = ""; }
    return false;
}
function clear_NewsLetter(myControl) {
    if (myControl && myControl.value == "Enter your E-Mail Address")
        myControl.value = "";
}
function ChangeNewsLetter(myControl) {
    if (myControl != null && myControl.value == '')
        myControl.value = "Enter your E-Mail Address";
}
function Change() {
    if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
        document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
            document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; return false;
}


//For Search
function clear_Search(myControl) {
    if (myControl && myControl.value == "Search by Keyword")
        myControl.value = "";
}
function ChangeSearch(myControl) {
    if (myControl != null && myControl.value == '')
        myControl.value = "Search by Keyword";
}


function ValidSearch() {
    var myControl;

    if (document.getElementById('txtSearch')) {
        myControl = document.getElementById('txtSearch');
    }



    if (myControl.value == '' || myControl.value == 'Search by Keyword') {
        alert("Please enter something to search");

        if (document.getElementById('txtSearch')) {
            document.getElementById('txtSearch').focus();
        }


        return false;
    }

    if (myControl.value.length < 3) {
        alert("Please enter at least 3 characters to search");
        myControl.focus();
        return false;
    }
    return true;
}

function ShowModelQuick(id) {

    document.getElementById("frmdisplayquick").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplayquick').height = '500px';
    document.getElementById('frmdisplayquick').width = '1000px';
    document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:1000px;height:500px;position:absolute;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact').style.width = '1000px';
    document.getElementById('popupContact').style.height = '500px';

    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id;
    centerPopup();
    loadPopup();


}