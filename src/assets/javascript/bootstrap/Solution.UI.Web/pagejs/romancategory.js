
function variantlink(lnk, vid) {
    document.getElementById('ContentPlaceHolder1_hdnLink').value = lnk;
    document.getElementById('ContentPlaceHolder1_hdnVariant').value = vid;
    __doPostBack('ctl00$ContentPlaceHolder1$btnSave', '');
}