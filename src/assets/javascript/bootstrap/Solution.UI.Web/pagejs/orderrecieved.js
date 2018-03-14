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
function validationUser() {
    if (document.getElementById("ContentPlaceHolder1_txtEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtEmailAddress").value == '') {
        alert("Please Enter Email Address.");
        document.getElementById("ContentPlaceHolder1_txtEmailAddress").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtEmailAddress").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtEmailAddress").value)) {

        alert("Please Enter valid Email Address.");

        document.getElementById("ContentPlaceHolder1_txtEmailAddress").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtPassword") != null && document.getElementById("ContentPlaceHolder1_txtPassword").value == '') {

        alert("Please Enter Password.");
        document.getElementById("ContentPlaceHolder1_txtPassword").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtPassword") != null && document.getElementById("ContentPlaceHolder1_txtPassword").value.length < 6) {

        alert("Password must be at least 6 characters long.");

        document.getElementById("ContentPlaceHolder1_txtPassword").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtConfirmPassword") != null && document.getElementById("ContentPlaceHolder1_txtConfirmPassword").value == '') {

        alert("Please Enter Confirm Password.");

        document.getElementById("ContentPlaceHolder1_txtConfirmPassword").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtPassword").value != document.getElementById("ContentPlaceHolder1_txtConfirmPassword").value) {

        alert("Confirm Password must be match with Password.");

        document.getElementById("ContentPlaceHolder1_txtConfirmPassword").focus();
        return false;
    }
    if (document.getElementById("prepage") != null) { document.getElementById("prepage").style.display = ''; }

    return true;
}