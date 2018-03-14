function ShowModelForCompareProduct(id) {
    //document.getElementById('header-part').style.zIndex = -1;
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '550px';
    document.getElementById('frmdisplay1').width = '1080px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:1080px;height:550px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '1080px';
    document.getElementById('popupContact1').style.height = '550px';
    window.scrollTo(0, 0);
    document.getElementById('frmdisplay1').src = id;
    centerPopup1();
    loadPopup1();
}