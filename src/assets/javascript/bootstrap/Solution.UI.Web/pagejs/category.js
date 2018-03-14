function ShowModelQuick(id) {

    document.getElementById('header').style.zIndex = -1;
    document.getElementById("frmquickview").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmquickview').height = '425px';
    document.getElementById('frmquickview').width = '750px';
    document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:750px;height:425px;border:solid 1px #7d7d7d;");
    document.getElementById('popupContact').style.width = '750px';
    document.getElementById('popupContact').style.height = '425px';
    window.scrollTo(0, 0);
    document.getElementById('btnreadmore').click();
    document.getElementById('frmquickview').src = '/QuickView.aspx?PID=' + id;

}
function adtocart(price, id) {
    if (document.getElementById('prepage') != null) {
        document.getElementById('prepage').style.display = '';
    }
    document.getElementById("ContentPlaceHolder1_hdnPrice").value = price;
    document.getElementById("ContentPlaceHolder1_hdnproductId").value = id;

}