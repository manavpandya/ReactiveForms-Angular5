/************************************************ 
*  jQuery iphoneSwitch plugin                   *
*                                               *
*  Author: Daniel LaBare                        *
*  Date:   2/4/2008                             *
************************************************/

jQuery.fn.iphoneSwitch = function (start_state, options, id, btnid, txtID) {


    var state = start_state == 'on' ? start_state : 'off';


    // define default settings
    var settings = {
        mouse_over: 'pointer',
        mouse_out: 'default',
        switch_on_container_path: '../images/iphone_switch_container_on.png',
        switch_off_container_path: '../images/iphone_switch_container_off.png',
        switch_path: '../images/iphone_switch.png',
        switch_height: 27,
        switch_width: 94
    };

    if (options) {
        jQuery.extend(settings, options);
    }

    // create the switch
    return this.each(function () {

        var container;
        var image;

        if (state == 'on') {
            document.getElementById(id).checked = true;
            if (document.getElementById(txtID) != null && document.getElementById(txtID).value != '') {
                document.getElementById(txtID).disabled = false;
            }
        }
        else {
            document.getElementById(id).checked = false;
            if (document.getElementById(txtID) != null && document.getElementById(txtID).value != '') {
                document.getElementById(txtID).disabled = true;
            }
        }
        if (document.getElementById(id).checked) {
            state == 'on';
        }
        else {
            state == 'off';
        }
         
        // make the container
        container = jQuery('<div class="iphone_switch_container" style="height:30px; width:' + settings.switch_width + 'px; position: relative; overflow: hidden"></div>');

        // make the switch image based on starting state
        image = jQuery('<img class="iphone_switch" style="height:' + settings.switch_height + 'px; width:' + settings.switch_width + 'px; background-image:url(' + settings.switch_path + '); background-repeat:none; background-position:' + (state == 'on' ? 0 : -53) + 'px" src="' + (state == 'on' ? settings.switch_on_container_path : settings.switch_off_container_path) + '" />');
        // insert into placeholder
        jQuery(this).html(jQuery(container).html(jQuery(image)));

        jQuery(this).mouseover(function () {
            jQuery(this).css("cursor", settings.mouse_over);
        });

        jQuery(this).mouseout(function () {
            jQuery(this).css("background", settings.mouse_out);
        });

        // click handling
        jQuery(this).click(function () {
            if (state == 'on') {
                if (confirm('Are you sure you want to turn off hemming ?')) {
                    jQuery(this).find('.iphone_switch').animate({ backgroundPosition: -53 }, "slow");
                    state = 'off';
                    document.getElementById(id).checked = false;
                    if (document.getElementById(txtID) != null && document.getElementById(txtID).value != '') {
                        document.getElementById(txtID).disabled = true;
                    }
                   
                    document.getElementById(btnid).click();
                }
            }
            else {
                if (confirm('Are you sure you want to turn on hemming ?')) {
                    jQuery(this).find('.iphone_switch').animate({ backgroundPosition: 0 }, "slow");
                    jQuery(this).find('.iphone_switch').attr('src', settings.switch_on_container_path);
                    state = 'on';
                    document.getElementById(id).checked = true;
                    if (document.getElementById(txtID) != null && document.getElementById(txtID).value != '') {
                        document.getElementById(txtID).disabled = false;
                    }
                    
                    document.getElementById(btnid).click();
                }
            }
           
        });
    });
};
