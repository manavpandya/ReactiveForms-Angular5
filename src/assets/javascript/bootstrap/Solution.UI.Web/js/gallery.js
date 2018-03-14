function mycarousel_initCallback(carousel) {
    // Disable autoscrolling if the user clicks the prev or next button.
    carousel.buttonNext.bind('click', function () {
        carousel.startAuto(0);
    });

    carousel.buttonPrev.bind('click', function () {
        carousel.startAuto(0);
    });

    // Pause autoscrolling if the user moves with the cursor over the clip.
    carousel.clip.hover(function () {
        carousel.stopAuto();
    }, function () {
        carousel.startAuto();
    });
};

jQuery(document).ready(function () {
    jQuery('#mycarousel').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});

function mycarousel_initCallback(carousel) {
    // Disable autoscrolling if the user clicks the prev or next button.
    carousel.buttonNext.bind('click', function () {
        carousel.startAuto(0);
    });

    carousel.buttonPrev.bind('click', function () {
        carousel.startAuto(0);
    });

    // Pause autoscrolling if the user moves with the cursor over the clip.
    carousel.clip.hover(function () {
        carousel.stopAuto();
    }, function () {
        carousel.startAuto();
    });
};

jQuery(document).ready(function () {
    jQuery('#mycarousel1').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});
jQuery(document).ready(function () {
    jQuery('#mycarousel8').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});
jQuery(document).ready(function () {
    jQuery('#mycarousel9').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});
jQuery(document).ready(function () {
    jQuery('#mycarousel10').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});
function mycarousel_initCallback(carousel) {
    // Disable autoscrolling if the user clicks the prev or next button.
    carousel.buttonNext.bind('click', function () {
        carousel.startAuto(0);
    });

    carousel.buttonPrev.bind('click', function () {
        carousel.startAuto(0);
    });

    // Pause autoscrolling if the user moves with the cursor over the clip.
    carousel.clip.hover(function () {
        carousel.stopAuto();
    }, function () {
        carousel.startAuto();
    });
};

jQuery(document).ready(function () {
    jQuery('#mycarousel2').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});

jQuery(document).ready(function () {
    jQuery('#mycarousel3').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});

jQuery(document).ready(function () {
    jQuery('#mycarousel4').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});

jQuery(document).ready(function () {
    jQuery('#mycarousel5').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});

jQuery(document).ready(function () {
    jQuery('#mycarousel6').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});
jQuery(document).ready(function () {
    jQuery('#mycarousel7').jcarousel({
        scroll: 1,
        initCallback: mycarousel_initCallback
    });
});


$(document).ready(function () {
    $('A[rel="_blank"]').click(function () {
        window.open($(this).attr('href'));
        return false;
    });
});