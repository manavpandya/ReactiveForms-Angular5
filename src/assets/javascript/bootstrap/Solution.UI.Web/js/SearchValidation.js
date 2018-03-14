function CheckSearchValidation() {
    if (document.getElementById("ContentPlaceHolder1_txtSearch").value == '') {
        alert("Please enter something to Search");
        document.getElementById("ContentPlaceHolder1_txtSearch").focus();
        return false;
    }

    if (document.getElementById("ContentPlaceHolder1_txtSearch")) {
        var str = document.getElementById("ContentPlaceHolder1_txtSearch").value;
        while (str.substring(str.length - 1, str.length) == ' ') // check white space from end
        {
            str = str.substring(0, str.length - 1);
        }
    }
    if (str.length < 3) {
        alert("Please enter at least 3 characters to search");
        if (document.getElementById('ContentPlaceHolder1_txtSearch'))
        { document.getElementById('ContentPlaceHolder1_txtSearch').focus(); }
        return false;
    }
    return true;
}

function ShowHideDiv(val) {
    if (document.getElementById('ContentPlaceHolder1_lblAdvancedSearch')) {
        if (document.getElementById('ContentPlaceHolder1_lblAdvancedSearch').innerHTML.toLowerCase() == "advanced search") {
            document.getElementById('ContentPlaceHolder1_hdnsearch').value = "1";
            document.getElementById('ContentPlaceHolder1_lblAdvancedSearch').innerHTML = "Search Home";
            $(".slidingDivImage").slideToggle();
            $(".slidingDivImage").hide();
        }
        else {
            document.getElementById('ContentPlaceHolder1_hdnsearch').value = "0";
            document.getElementById('ContentPlaceHolder1_lblAdvancedSearch').innerHTML = "Advanced Search";
            $(".slidingDivImage").slideToggle();
            $(".slidingDivImage").show();
        }
    }
}