
//var popupStatus=0;function loadPopup(){if(popupStatus==0){$("#backgroundPopup").css({"opacity":"0.7"});$("#backgroundPopup").fadeIn("slow");$("#popupContact").fadeIn("slow");popupStatus=1;}}
//function loadPopup1(){if(popupStatus==0){$("#backgroundPopup").css({"opacity":"0.7"});$("#backgroundPopup").fadeIn("slow");$("#popupContact1").fadeIn("slow");popupStatus=1;}}
var popupStatus = 0;
var popupStatuspricequote = 0;
function centerPopup1pricequote() {
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var popupHeight1 = $("#popupContactpricequote").height();
    var popupWidth1 = $("#popupContactpricequote").width();
    $("#popupContactpricequote").css({
        "position": "fixed",
        "top": windowHeight / 2 - popupHeight1 / 2 + 5,
        "bottom": 5,
        "left": windowWidth / 2 - popupWidth1 / 2
    });
}

function loadPopup1pricequote() {
    if (popupStatuspricequote == 0) {
        $("#backgroundPopup").css({
            "opacity": "0.4"
        });
       // $("#backgroundPopup").fadeIn("slow");
        $("#popupContactpricequote").fadeIn("slow");
        popupStatuspricequote = 1;
    }
}


function disablePopuppricequote() {

    if (popupStatuspricequote == 1) {
         //$("#backgroundPopup").fadeOut("slow");

        $("#popupContactpricequote").fadeOut("slow");
        popupStatuspricequote = 0;
    }
}
function loadPopupmaster() {
    if (popupStatuspricequote == 0) {
        $("#backgroundPopup").css({
            "opacity": "0.4"
        });
        $("#backgroundPopup").fadeIn("slow");
        $("#popupContactpricequote1").fadeIn("slow");
        popupStatuspricequote = 1;
    }
}

function centerPopupmaster() {
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var popupHeight1 = $("#popupContactpricequote1").height();
    var popupWidth1 = $("#popupContactpricequote1").width();
    $("#popupContactpricequote1").css({
        "position": "fixed",
        "top": windowHeight / 2 - popupHeight1 / 2 + 5,
        "bottom": 5,
        "left": windowWidth / 2 - popupWidth1 / 2
    });
}
function disablePopupmaster() {

    if (popupStatuspricequote == 1) {
        $("#backgroundPopup").fadeOut("slow");

        $("#popupContactpricequote1").fadeOut("slow");
        popupStatuspricequote = 0;
    }
}

 function loadPopup() { if (popupStatus == 0) { $("#backgroundPopup").css({ "opacity": "0.4" }); $("#backgroundPopup").fadeIn("slow"); $("#popupContact").fadeIn("slow"); popupStatus = 1; } }
function loadPopup1() { if (popupStatus == 0) { $("#backgroundPopup").css({ "opacity": "0.4" }); $("#backgroundPopup").fadeIn("slow"); $("#popupContact1").fadeIn("slow"); popupStatus = 1; } }
function disablePopup(){if(popupStatus==1){$("#backgroundPopup").fadeOut("slow");$("#popupContact").fadeOut("slow");$("#popupContact1").fadeOut("slow");popupStatus=0;}}
function centerPopup(){var windowWidth=document.documentElement.clientWidth;var windowHeight=document.documentElement.clientHeight;var popupHeight=$("#popupContact").height();var popupWidth=$("#popupContact").width();$("#popupContact").css({"position":"fixed","top":windowHeight/2-popupHeight/2+5,"bottom":5,"left":windowWidth/2-popupWidth/2});}
function centerPopup1(){var windowWidth=document.documentElement.clientWidth;var windowHeight=document.documentElement.clientHeight;var popupHeight1=$("#popupContact1").height();var popupWidth1=$("#popupContact1").width();$("#popupContact1").css({"position":"fixed","top":windowHeight/2-popupHeight1/2+5,"bottom":5,"left":windowWidth/2-popupWidth1/2});}
$(document).ready(function(){$("#btnreadmore").click(function(){centerPopup();loadPopup();});$("#ContentPlaceHolder1_popupContactClose").click(function(){disablePopup();if(document.getElementById('header')){document.getElementById('header').style.zIndex=1000;}});$("#popupContactClose").click(function(){disablePopup();});$("#btnhelpdescri").click(function(){centerPopup1();loadPopup1();});$("#ContentPlaceHolder1_popupContactClose1").click(function(){disablePopup();if(document.getElementById('header')){document.getElementById('header').style.zIndex=1000;}});$("#backgroundPopup").click(function(){});$(document).keypress(function(e){if(e.keyCode==27&&popupStatus==1){disablePopup();}});});