
var ww = document.body.clientWidth;

$(document).ready(function () {
    $(".nav li a").each(function () {
        if ($(this).next().length > 0) {
            $(this).addClass("parent");
        };
    })

    $(".toggleMenu").click(function (e) {
        e.preventDefault();
        $(this).toggleClass("active");
        $(".nav").toggle();
    });
    adjustMenu();
    $('#toggle1').click(function () {


        if ($("#option-arrow").attr('src') == "/images/option-arrow-up.png" || $("#option-arrow").attr('src') == "images/option-arrow-up.png") {

            $('.toggle1').css('display', 'none');
            }
            else {
                $('.toggle1').css('display', 'block');
            }
     
        
            //$('.toggle1').slideToggle('1100');

        


        var src = $("#option-arrow").attr('src') == "/images/option-arrow-up.png" ? "/images/option-arrow-down.png" : "/images/option-arrow-up.png";
        $("#option-arrow").attr('src', src);
        return false;

    });
    var itemWidth = $('#boxContainer').width();
    var availWidth = $('#boxContainer-left').width();
    var difference = availWidth + 25;
    var difference = "-" + difference;
    $('#boxContainer').css('left', difference + 'px');
})


$(window).resize(function () {
    var itemWidth = $('#boxContainer').width();
    var availWidth = $('#boxContainer-left').width();
    var difference = availWidth + 25;
    var difference = "-" + difference;
    $('#boxContainer').css('left', difference + 'px');
});
$(window).bind('resize orientationchange', function () {
    ww = document.body.clientWidth;
    adjustMenu();
});

var adjustMenu = function () {
    if (ww < 980) {
        $(".toggleMenu").css("display", "inline-block");
        if (!$(".toggleMenu").hasClass("active")) {
            $(".nav").hide();
        } else {
            $(".nav").show();
        }
        $(".nav li").unbind('mouseenter mouseleave');
        /*$(".nav li a.parent").unbind('click').bind('click', function(e) {
        // must be attached to anchor element to prevent bubbling
        e.preventDefault();
        $(this).parent("li").toggleClass("hover");
        });*/
    }
    else if (ww >= 981) {
        $(".toggleMenu").css("display", "none");
        $(".nav").show();
        $(".nav li").removeClass("hover");
        //	$(".nav li a").unbind('click');
        $(".nav li").unbind('mouseenter mouseleave').bind('mouseenter mouseleave', function () {
            // must be attached to li so that mouseleave is not triggered when hover over submenu
            $(this).toggleClass('hover');
        });
    }
}

