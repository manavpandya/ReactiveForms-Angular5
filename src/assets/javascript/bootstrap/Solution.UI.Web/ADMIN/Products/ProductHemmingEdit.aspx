<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductHemmingEdit.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductHemmingEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script src="/Admin/js/jquery-onoff.js" type="text/javascript"></script>
    <script src="/Admin/js/jquery-switch-hemming.js" type="text/javascript"></script>
    <script type="text/javascript">
 function OpenCenterWindow(vendorid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(vendorid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
        function MakeCheckedall(flag, id) {
            var arrflag = flag.split(',');
            var arrid = id.split(',');
            if (arrflag.length == arrid.length)
                for (var i = 0; i < arrflag.length; i++) {
                    if (arrflag[i].toString() == "true") {
                        document.getElementById(arrid[i].toString()).checked = true;
                    }
                    else {
                        document.getElementById(arrid[i].toString()).checked = false;
                    }
                }
        }
  function CheckPass() {
            if ((document.getElementById('ContentPlaceHolder1_txtpassword').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Password', 'Message', 'ContentPlaceHolder1_txtpassword');
                return false;
            }
            return true;
        }

        function CheckState(id) {
            var i = "";
            if (document.getElementById(id).checked) {
                i = "on";
            }
            else {
                i = "off";
            }
            return i;
        }

        function Getstatusall(chkID, DivID, btnID, txtID) {
            var arrchk = chkID.split(',');
            var arrdiv = DivID.split(',');
            var arrtxt = txtID.split(',');
            if (arrchk.length == arrdiv.length)
                for (var i = 0; i < arrdiv.length; i++) {
                    var state = CheckState(arrchk[i]);
                    var $j = jQuery.noConflict();
                    $j('#' + arrdiv[i]).iphoneSwitch(state, { switch_on_container_path: '../images/iphone_switch_container_off.png' }, arrchk[i], btnID, arrtxt[i]);

                }
        }


    </script>
    <style type="text/css">
        body {
            font-family: Verdana, Geneva, sans-serif;
            font-size: 14px;
        }

        .left {
            float: none;
            width: 120px;
        }
    </style>
    <style type="text/css">
        .checklist-main th {
            background-color: #eee;
            line-height: 25px;
            border-top: solid 1px #999;
            border-bottom: solid 1px #999;
        }

        .checklist-main td {
            line-height: 25px;
        }

        .divfloatingcss {
            bottom: 0;
            right: 0;
            position: fixed;
            width: 10%;
            margin-right: 46%;
            background-image: url("/Admin/images/title-bg-trans.png");
            -webkit-border-top-left-radius: 15px;
            -webkit-border-top-right-radius: 15px;
            -moz-border-radius-topleft: 15px;
            -moz-border-radius-topright: 15px;
            border-top-left-radius: 15px;
            border-top-right-radius: 15px; /*-webkit-border-radius: 20px;
-moz-border-radius: 20px;
border-radius: 20px;*/
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#divfloating').attr("class", "divfloatingcss");
            $(window).scroll(function () {
                if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                    $('#divfloating').attr("class", "");
                }
                else {
                    $('#divfloating').attr("class", "divfloatingcss");
                }
            });
        });
    </script>

    <script type="text/javascript">



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
            if (window.event) return window.event.keyCode; else if (e) return e.which; else return null;
        }


        function AllowandlockQtyVariantChk(id) {
$('#' + id).attr("disabled", true);
            var chkbox = id.replace(/_grdoptionmainGroup_chkallowed_/ig, '_grdoptionmainGroup_grdvaluelisting_');
            var chkbox1 = id.replace(/_grdoptionmainGroup_chkallowed_/ig, '_grdoptionmainGroup_txtsafetyHandsw_');
             var allexistchk = document.getElementById('ContentPlaceHolder1_grdoptionmainGroup').getElementsByTagName('input');
            
            for (var i = 0; i < allexistchk.length; i++) {
                var elt1 = allexistchk[i];
                if (elt1.id.toString().toLowerCase().indexOf('_txtsafetyhandsw') > -1 && elt1.id.toString().toLowerCase() == chkbox1.toLowerCase()) {
                    AllowandlockQtyVariant(elt1.id);

                }
               AllowandlockQtyVariantswatch(id.replace(/_chkallowed_/ig, '_txtsafetyHandsw_'))
            }
$('#' + id).removeAttr("disabled");
            // var hpdsite = id.replace(/_chkallowed_/ig, '_txthpdwebsitesw_');
            // var saleschannelsite = id.replace(/_chkallowed_/ig, '_txtsaleschannelsw_');
            // var skuid = id.replace(/_chkallowed_/ig, '_ltSKU_');
            // var upcid = id.replace(/_chkallowed_/ig, '_txtUPC_');
            // alert(document.getElementById(skuid).innerHTML);
            // alert(document.getElementById(upcid).value);
            //$.ajax(
            //            {
            //                type: "POST",
            //                url: "/TestMail.aspx/gethpdsitevalue",
            //                data: "{SKU: '" + document.getElementById(skuid).innerHTML + "',UPC: '" + document.getElementById(upcid).value + "' }",
            //                contentType: "application/json; charset=utf-8",
            //                dataType: "json",
            //                async: "true",
            //                cache: "false",
            //                success: function (msg) {
            //                    alert(msg.d);
            //                    document.getElementById(hpdsite).value = msg.d;

            //                },
            //                Error: function (x, e) {
            //                }
            //            });
            //$.ajax(
            //            {
            //                type: "POST",
            //                url: "/TestMail.aspx/getsaleschannelvalue",
            //                data: "{SKU: '" + document.getElementById(skuid).innerHTML + "',UPC: '" + document.getElementById(upcid).value + "' }",
            //                contentType: "application/json; charset=utf-8",
            //                dataType: "json",
            //                async: "true",
            //                cache: "false",
            //                success: function (msg) {
            //                    document.getElementById(saleschannelsite).value = msg.d;
            //                },
            //                Error: function (x, e) {
            //                }
            //            });

        }
        function AllowandlockQtyVariantswatch(id) {
            var txtsafetyHand = id;
            var txttotalinventory = id.replace(/_txtsafetyHandsw/ig, '_hdnInventory');
            var txtwebsite = id.replace(/_txtsafetyHandsw/ig, '_txthpdwebsitesw');
            var txtsaleschannel = id.replace(/_txtsafetyHandsw/ig, '_txtsaleschannelsw');
            var txtsafetyHandvalue = document.getElementById(id).value;
            var chkallowed =  id.replace(/_txtsafetyHandsw/ig, '_chkallowed');


            var txtinventoryonHandvalue = document.getElementById(txttotalinventory).value;
            if (document.getElementById('ContentPlaceHolder1_chkonoff') != null && document.getElementById('ContentPlaceHolder1_chkonoff').checked == true && document.getElementById(chkallowed).checked == true) {
                var txtsaleschannelvalue = txtinventoryonHandvalue - txtsafetyHandvalue;



                if (parseFloat(txtsaleschannelvalue) > parseFloat(0)) {
                    document.getElementById(txtwebsite).value = txtsaleschannelvalue;
                    document.getElementById(txtsaleschannel).value = txtsaleschannelvalue;
                }
                else {
                    document.getElementById(txtwebsite).value = '0';
                    document.getElementById(txtsaleschannel).value = '0';
                }

            }
            else {
                var txtsaleschannelvalue = txtinventoryonHandvalue - txtsafetyHandvalue;
                var txthpdwebsitevalue = txtinventoryonHandvalue;

                if (parseFloat(txthpdwebsitevalue) > parseFloat(0)) {
                    
                    document.getElementById(txtwebsite).value = txthpdwebsitevalue;
                }
                else {
                    document.getElementById(txtwebsite).value = '0';
                }
                if (parseFloat(txtsaleschannelvalue) > parseFloat(0)) {
                    
                    document.getElementById(txtsaleschannel).value = txtsaleschannelvalue;
                }
                else {
                    document.getElementById(txtsaleschannel).value = '0';
                }


            }

        }
        function AllowandlockQtyVariant(id) {

            var txtsafetyHand = id;
            var chk = id;//.substring(0, id.indexOf('_txt'));
            var chkallowed = chk.replace(/_txtsafetyHandsw/ig, '_chkallowed');
            var hdnchkallowed = chk.replace(/_txtsafetyHandsw/ig, '_hdnchkallowed');
            var skuid = chk.replace(/_txtsafetyHandsw/ig, '_ltSKU');
            var txttotalinventory = id.replace(/_txtsafetyHandsw/ig, '_txttotalinventory');
            var txtsaleschannel = id.replace(/_txtsafetyHandsw/ig, '_txtsaleschannelsw');
            var txtsafetyHandvalue = document.getElementById(id).value;
            var txttotalinventoryvalue = document.getElementById(txttotalinventory).value;
            var txtsaleschannelvalue = document.getElementById(txtsaleschannel).value;
             
            //
            var txthpdwebsite = id.replace(/_txtsafetyHandsw/ig, '_txthpdwebsitesw');
            var txtinventoryonHand = id.replace(/_txtsafetyHandsw/ig, '_txtinventoryonHand');
            var txthpdwebsitevalue = document.getElementById(txthpdwebsite).value;
            var txtinventoryonHandvalue = document.getElementById(txtinventoryonHand).value;


            if (document.getElementById(txtsafetyHand) != null && document.getElementById(txtsafetyHand).value != "") {
                // alert(document.getElementById(txtsafetyHand));alert(document.getElementById(txtsafetyHand).value);
                if (document.getElementById(chkallowed) != null && document.getElementById(chkallowed).checked == true && document.getElementById('ContentPlaceHolder1_chkonoff') != null && document.getElementById('ContentPlaceHolder1_chkonoff').checked == true) {
                     
 if (document.getElementById("ContentPlaceHolder1_hdnallsku") != null)
                    {
                        if(document.getElementById("ContentPlaceHolder1_hdnallsku").value=='')
                        {
                           // document.getElementById("ContentPlaceHolder1_hdnallsku").value = ",";
                        }
                        if (document.getElementById(skuid) != null)
                        {
                            var s = document.getElementById(skuid).innerHTML;
                            //if (document.getElementById("ContentPlaceHolder1_hdnallsku").value.indexOf(',' + s + ',') <= -1) {
                               // document.getElementById("ContentPlaceHolder1_hdnallsku").value = document.getElementById("ContentPlaceHolder1_hdnallsku").value + s + ",";
//document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value = document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value + 'ON' + ",";
                            // }

                            document.getElementById("ContentPlaceHolder1_hdnallsku").value = s + ',' + document.getElementById("ContentPlaceHolder1_hdnallsku").value;
                            document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value = 'ON,' + document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value;
                            if (document.getElementById(hdnchkallowed).value == '1' || document.getElementById(hdnchkallowed).value.toString().toLowerCase() == 'true') {
                                document.getElementById("ContentPlaceHolder1_hdnonoffstatus").value = 'ON,' + document.getElementById("ContentPlaceHolder1_hdnonoffstatus").value;
                            }
                            else if (document.getElementById(hdnchkallowed).value == '' || document.getElementById(hdnchkallowed).value == '0' || document.getElementById(hdnchkallowed).value.toString().toLowerCase() == 'false') {
                                document.getElementById("ContentPlaceHolder1_hdnonoffstatus").value = 'OFF,' + document.getElementById("ContentPlaceHolder1_hdnonoffstatus").value;
                            }

                        }
                        
                        
                    }

                    txtsaleschannelvalue = txttotalinventoryvalue - txtsafetyHandvalue;
                    txthpdwebsitevalue = txtinventoryonHandvalue - txtsafetyHandvalue;
                    //document.getElementById(txtsaleschannel).value = txtsaleschannelvalue;
                    //document.getElementById(txthpdwebsite).value = txthpdwebsitevalue;

                    var Qty84Org = 0; var Qty96Org = 0; var Qty108Org = 0; var Qty120Org = 0;
                    var Qty84 = 0; var Qty96 = 0; var Qty108 = 0; var Qty120 = 0;
                    var Qty84Orgdiff = 0; var Qty96Orgdiff = 0; var Qty108Orgdiff = 0; var Qty120Orgdiff = 0;

                    var Qty84Orghw = 0; var Qty96Orghw = 0; var Qty108Orghw = 0; var Qty120Orghw = 0;
                    var Qty84hw = 0; var Qty96hw = 0; var Qty108hw = 0; var Qty120hw = 0;
                    var Qty84Orgdiffhw = 0; var Qty96Orgdiffhw = 0; var Qty108Orgdiffhw = 0; var Qty120Orgdiffhw = 0;

                     
                    var alldiv1 = id.substring(0, id.indexOf('_txt'));
                    var allexist1 = document.getElementById('ContentPlaceHolder1_grdoptionmainGroup').getElementsByClassName(document.getElementById(skuid).innerHTML);
                     
                    var tempQty84 = 0;
                    var tempQty96 = 0;
                    var tempQty108 = 0;
                    var tempQty120 = 0;
                    var StoreQty = 0;
                    for (var i = 0; i < allexist1.length; i++) {

                        var elt111 = allexist1[i];
                        var elt = allexist1[i];
                        var varrr = elt.id.toString();//.replace('_chkallowed_', '_ltSKU_');
                        
                        if (varrr.toString().toLowerCase().indexOf('_ltsku') > -1) {
                             
                            if (elt.innerHTML.toString().toLowerCase().indexOf('-84') > -1) {

                                var Qty84id1 = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                var total84id = varrr.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty84id = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total84value = 0; var safty84value = 0;
                                if (document.getElementById(total84id) != null && document.getElementById(total84id).value != "") { total84value = document.getElementById(total84id).value }
                                if (document.getElementById(safty84id) != null && document.getElementById(safty84id).value != "") { safty84value = document.getElementById(safty84id).value }
                                // if (parseFloat(total84value) >= parseFloat(safty84value)) {
                                Qty84Orgdiff = parseFloat(total84value) - parseFloat(safty84value);
                                //}
                                if (parseFloat(Qty84Orgdiff) < parseFloat(0)) {
                                    Qty84Orgdiff = 0;
                                }
                                StoreQty = parseFloat(total84value);
                                Qty84Org = document.getElementById(Qty84id1).value;


                                var Qty84id1hw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                var total84idhw = varrr.replace(/_ltSKU/ig, '_txtinventoryonHand');
                                var safty84idhw = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total84valuehw = 0; var safty84valuehw = 0;
                                if (document.getElementById(total84idhw) != null && document.getElementById(total84idhw).value != "") { total84valuehw = document.getElementById(total84idhw).value; }
                                if (document.getElementById(safty84idhw) != null && document.getElementById(safty84idhw).value != "") { safty84valuehw = document.getElementById(safty84idhw).value; }
                                // if (parseFloat(total84valuehw) >= parseFloat(safty84valuehw)) {
                                Qty84Orgdiffhw = parseFloat(total84valuehw) - parseFloat(safty84valuehw);
                                // }
                                if (parseFloat(Qty84Orgdiffhw) < parseFloat(0)) {
                                    Qty84Orgdiffhw = 0;
                                }
                                Qty84Orghw = document.getElementById(Qty84id1hw).value;
                                tempQty84 = parseFloat(StoreQty) - parseFloat(safty84value);

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-96') > -1) {
                                
                                var Qty96id1 = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                var total96id = varrr.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty96id = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total96value = 0; var safty96value = 0;
                                if (document.getElementById(total96id) != null && document.getElementById(total96id).value != "") { total96value = document.getElementById(total96id).value; }
                                if (document.getElementById(safty96id) != null && document.getElementById(safty96id).value != "") { safty96value = document.getElementById(safty96id).value; }
                                //  if (parseFloat(total96value) >= parseFloat(safty96value)) {
                                Qty96Orgdiff = parseFloat(total96value) - parseFloat(safty96value);
                                if (parseFloat(Qty96Orgdiff) < parseFloat(0)) {
                                    Qty96Orgdiff = 0;
                                }
                                //  }
                                Qty96Org = document.getElementById(Qty96id1).value;
                                StoreQty = parseFloat(total96value);

                                var Qty96id1hw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                var total96idhw = varrr.replace(/_ltSKU/ig, '_txtinventoryonHand');
                                var safty96idhw = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total96valuehw = 0; var safty96valuehw = 0;
                                if (document.getElementById(total96idhw) != null && document.getElementById(total96idhw).value != "") { total96valuehw = document.getElementById(total96idhw).value; }
                                if (document.getElementById(safty96idhw) != null && document.getElementById(safty96idhw).value != "") { safty96valuehw = document.getElementById(safty96idhw).value; }
                                //  if (parseFloat(total108value) >= parseFloat(safty108value)) {
                                Qty96Orgdiffhw = parseFloat(total96valuehw) - parseFloat(safty96valuehw);
                                if (parseFloat(Qty96Orgdiffhw) < parseFloat(0)) {
                                    Qty96Orgdiffhw = 0;
                                }
                                // }
                                Qty96Orghw = document.getElementById(Qty96id1hw).value;
                                tempQty96 = parseFloat(StoreQty) - parseFloat(safty96value);

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-108') > -1) {

                                var Qty108id1 = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                var total108id = varrr.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty108id = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total108value = 0; var safty108value = 0;
                                if (document.getElementById(total108id) != null && document.getElementById(total108id).value != "") { total108value = document.getElementById(total108id).value; }
                                if (document.getElementById(safty108id) != null && document.getElementById(safty108id).value != "") { safty108value = document.getElementById(safty108id).value; }
                                //if (parseFloat(total108value) >=  parseFloat(safty108value))
                                // {
                                Qty108Orgdiff = parseFloat(total108value) - parseFloat(safty108value);
                                if (parseFloat(Qty108Orgdiff) < parseFloat(0)) {
                                    Qty108Orgdiff = 0;
                                }
                                // }
                                Qty108Org = document.getElementById(Qty108id1).value;
                                StoreQty = parseFloat(total108value);
                                var Qty108id1hw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                var total108idhw = varrr.replace(/_ltSKU/ig, '_txtinventoryonHand');
                                var safty108idhw = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total108valuehw = 0; var safty108valuehw = 0;
                                if (document.getElementById(total108idhw) != null && document.getElementById(total108idhw).value != "") { total108valuehw = document.getElementById(total108idhw).value; }
                                if (document.getElementById(safty108idhw) != null && document.getElementById(safty108idhw).value != "") { safty108valuehw = document.getElementById(safty108idhw).value; }
                                // if (parseFloat(total108valuehw) >=  parseFloat(safty108valuehw))
                                // {
                                Qty108Orgdiffhw = parseFloat(total108valuehw) - parseFloat(safty108valuehw);
                                if (parseFloat(Qty108Orgdiffhw) < parseFloat(0)) {
                                    Qty108Orgdiffhw = 0;
                                }
                                // }
                                Qty108Orghw = document.getElementById(Qty108id1hw).value;
                                tempQty108 = parseFloat(StoreQty) - parseFloat(safty108value);

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-120') > -1) {

                                var Qty120id1 = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                var total120id = varrr.replace(/_ltSKU/ig, '_txttotalinventory');
                                var total120value = 0; var safty120value = 0;
                                if (document.getElementById(total120id) != null && document.getElementById(total120id).value != "") { total120value = document.getElementById(total120id).value; }
                                var safty120id = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                if (document.getElementById(safty120id) != null && document.getElementById(safty120id).value != "") { safty120value = document.getElementById(safty120id).value; }
                                // if (parseFloat(total120value) >=  parseFloat(safty120value))
                                // {
                                Qty120Orgdiff = parseFloat(total120value) - parseFloat(safty120value);
                                //}
                                Qty120Org = document.getElementById(Qty120id1).value;

                                var Qty120id1hw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                var total120idhw = varrr.replace(/_ltSKU/ig, '_txtinventoryonHand');
                                var safty120idhw = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total120valuehw = 0; var safty120valuehw = 0;
                                if (document.getElementById(total120idhw) != null && document.getElementById(total120idhw).value != "") { total120valuehw = document.getElementById(total120idhw).value; }
                                if (document.getElementById(safty120idhw) != null && document.getElementById(safty120idhw).value != "") { safty120valuehw = document.getElementById(safty120idhw).value; }
                                //if (parseFloat(total120valuehw) >=  parseFloat(safty120valuehw))
                                //{
                                Qty120Orgdiffhw = parseFloat(total120valuehw) - parseFloat(safty120valuehw);
                                //}

                                Qty120Orghw = document.getElementById(Qty120id1hw).value;

                            }


                        }
                    }

                    var Qty84id = ""; var Qty96id = ""; var Qty108id = ""; var Qty120id = "";
                    var Qty84idhw = ""; var Qty96idhw = ""; var Qty108idhw = ""; var Qty120idhw = "";
                    // alert(Qty84Org); alert(Qty96Org);alert(Qty108Org);alert(Qty120Org);

                    var alldiv = id;//.substring(0, id.indexOf('_txt'));
                   

                    var allexist = document.getElementById('ContentPlaceHolder1_grdoptionmainGroup').getElementsByClassName(document.getElementById(skuid).innerHTML);
                    for (var i = 0; i < allexist.length; i++) {
                        var elt111 = allexist[i];
                        var elt = allexist[i];// document.getElementById(elt111.id.toString().replace('_chkallowed_', '_ltSKU_'));
                        var varrr = elt.id.toString();
                        if (varrr.toString().toLowerCase().indexOf('_ltsku') > -1) {

                            if (elt.innerHTML.toString().toLowerCase().indexOf('-84') > -1) {

                                Qty84id = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');

                                if (parseFloat(Qty96Org) < parseFloat(0)) {
                                    Qty84 = parseFloat(Qty84Orgdiff);
                                }
                                else {
                                    Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96Org);
                                }

                                

                                if (parseFloat(Qty84) < parseFloat(0)) {
                                    if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                }



                                Qty84idhw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                if (parseFloat(Qty96Orghw) < parseFloat(0)) {
                                    Qty84hw = parseFloat(Qty84Orgdiffhw);
                                }
                                else {
                                    Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96Orghw);
                                }
                                
                                if (parseFloat(Qty84hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                }

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-96') > -1) {

                                Qty96id = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                if (parseFloat(Qty108Org) < parseFloat(0)) {
                                    Qty96 = parseFloat(Qty96Orgdiff);
                                }
                                else {
                                    Qty96 = parseFloat(Qty96Orgdiff) + parseFloat(Qty108Org);
                                }
                                if (parseFloat(Qty96) < parseFloat(0)) {
                                    Qty84 = parseFloat(Qty84Orgdiff);
                                }
                                else {
                                    Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);
                                }
                                //Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);

                                if (parseFloat(Qty84) < parseFloat(0)) {
                                    if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                }

                                if (parseFloat(Qty96) < parseFloat(0)) {
                                    if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }
                                }
                                //if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                //if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }

                                Qty96idhw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                if (parseFloat(Qty108Orghw) < parseFloat(0))
                                {
                                    Qty96hw = parseFloat(Qty96Orgdiffhw);
                                }
                                else {
                                    Qty96hw = parseFloat(Qty96Orgdiffhw) + parseFloat(Qty108Orghw);
                                }
                               
                                if (parseFloat(Qty96hw) < parseFloat(0)) {
                                    Qty84hw = parseFloat(Qty84Orgdiffhw);
                                }
                                else {
                                    Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96hw);
                                }


                                

                                if (parseFloat(Qty84hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                }
                                if (parseFloat(Qty96hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }
                                }

                                //if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                //if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-108') > -1) {

                                Qty108id = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                               // Qty108 = parseFloat(Qty108Orgdiff) + parseFloat(Qty120Org);
                               // Qty96 = parseFloat(Qty96Orgdiff) + parseFloat(Qty108);
                                //Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);

                                if (parseFloat(Qty120Org) < parseFloat(0)) {
                                    Qty108 = parseFloat(Qty108Orgdiff);
                                }
                                else {
                                    Qty108 = parseFloat(Qty108Orgdiff) + parseFloat(Qty120Org);
                                }
                                if (parseFloat(Qty108) < parseFloat(0)) {
                                    Qty96 = parseFloat(Qty96Orgdiff);
                                }
                                else {
                                    Qty96 = parseFloat(Qty96Orgdiff) + parseFloat(Qty108);
                                }

                                if (parseFloat(Qty96) < parseFloat(0)) {
                                    Qty84 = parseFloat(Qty84Orgdiff);
                                }
                                else {
                                    Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);
                                }


                                if (parseFloat(Qty84) < parseFloat(0)) {
                                    if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                }

                                if (parseFloat(Qty96) < parseFloat(0)) {
                                    if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }
                                }

                                if (parseFloat(Qty108) < parseFloat(0)) {
                                    if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = Qty108; }
                                }

                                //if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                //if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }
                                //if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = Qty108; }


                                Qty108idhw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                               // Qty108hw = parseFloat(Qty108Orgdiffhw) + parseFloat(Qty120Orghw);
                               // Qty96hw = parseFloat(Qty96Orgdiffhw) + parseFloat(Qty108hw);
                                //Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96hw);

                                if (parseFloat(Qty120Orghw) < parseFloat(0)) {
                                    Qty108hw = parseFloat(Qty108Orgdiffhw);
                                }
                                else {
                                    Qty108hw = parseFloat(Qty108Orgdiffhw) + parseFloat(Qty120Orghw);
                                }
                                if (parseFloat(Qty108hw) < parseFloat(0)) {
                                    Qty96hw = parseFloat(Qty96Orgdiffhw);
                                }
                                else {
                                    Qty96hw = parseFloat(Qty96Orgdiffhw) + parseFloat(Qty108hw);
                                }

                                if (parseFloat(Qty96hw) < parseFloat(0)) {
                                    Qty84hw = parseFloat(Qty84Orgdiffhw);
                                }
                                else {
                                    Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96hw);
                                }



                                if (parseFloat(Qty84hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                }
                                if (parseFloat(Qty96hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }
                                }
                                if (parseFloat(Qty108hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = Qty108hw; }
                                }

                                //if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                //if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }
                                //if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = Qty108hw; }

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-120') > -1) {

                                Qty120id = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                Qty120 = parseFloat(Qty120Orgdiff);
                                //Qty108 = parseFloat(Qty108Orgdiff) + parseFloat(Qty120);
                                //Qty96 = parseFloat(Qty96Orgdiff) + parseFloat(Qty108);
                                //Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);


                                if (parseFloat(Qty120) < parseFloat(0)) {
                                    Qty108 = parseFloat(Qty108Orgdiff) ;
                                }
                                else {
                                    Qty108 = parseFloat(Qty108Orgdiff) + parseFloat(Qty120);
                                }


                                if (parseFloat(Qty108) < parseFloat(0)) {
                                    Qty96 = parseFloat(Qty96Orgdiff);
                                }
                                else {
                                    Qty96 = parseFloat(Qty96Orgdiff) + parseFloat(Qty108);
                                }
                                if (parseFloat(Qty96) < parseFloat(0)) {
                                    Qty84 = parseFloat(Qty84Orgdiff);
                                }
                                else {
                                    Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);
                                }


                                if (parseFloat(Qty84) < parseFloat(0)) {
                                    if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                }

                                if (parseFloat(Qty96) < parseFloat(0)) {
                                    if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }
                                }
                                if (parseFloat(Qty108) < parseFloat(0)) {
                                    if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = Qty108; }
                                }
                                if (parseFloat(Qty120) < parseFloat(0)) {
                                    if (document.getElementById(Qty120id) != null) { document.getElementById(Qty120id).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty120id) != null) { document.getElementById(Qty120id).value = Qty120; }
                                }

                                // if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                // if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }
                                // if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = Qty108; }
                                // if (document.getElementById(Qty120id) != null) { document.getElementById(Qty120id).value = Qty120; }

                                Qty120idhw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                Qty120hw = parseFloat(Qty120Orgdiffhw);
                                //Qty108hw = parseFloat(Qty108Orgdiffhw) + parseFloat(Qty120hw);
                                //Qty96hw = parseFloat(Qty96Orgdiffhw) + parseFloat(Qty108hw);
                                //Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96hw);

                                if (parseFloat(Qty120hw) < parseFloat(0)) {
                                    Qty108hw = parseFloat(Qty108Orgdiffhw);
                                }
                                else {
                                    Qty108hw = parseFloat(Qty108Orgdiffhw) + parseFloat(Qty120hw);
                                }

                                if (parseFloat(Qty108hw) < parseFloat(0)) {
                                    Qty96hw = parseFloat(Qty96Orgdiffhw);
                                }
                                else {
                                    Qty96hw = parseFloat(Qty96Orgdiffhw) + parseFloat(Qty108hw);
                                }

                                if (parseFloat(Qty96hw) < parseFloat(0)) {
                                    Qty84hw = parseFloat(Qty84Orgdiffhw);
                                }
                                else {
                                    Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96hw);
                                }


                                if (parseFloat(Qty84hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                }
                                if (parseFloat(Qty96hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }
                                }
                                if (parseFloat(Qty108hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = Qty108hw; }
                                }
                                if (parseFloat(Qty120hw) < parseFloat(0)) {
                                    if (document.getElementById(Qty120idhw) != null) { document.getElementById(Qty120idhw).value = '0'; }
                                }
                                else {
                                    if (document.getElementById(Qty120idhw) != null) { document.getElementById(Qty120idhw).value = Qty120hw; }
                                }
                                //if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                //if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }
                                //if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = Qty108hw; }
                                //if (document.getElementById(Qty120idhw) != null) { document.getElementById(Qty120idhw).value = Qty120hw; }


                            }

                        }
                    }


                    if (tempQty84 < 0) {
                        if (Qty84 > (-(tempQty84))) {
                            Qty84 = Qty84 + tempQty84;
                            Qty84hw = Qty84;
                        }
                        else {
                            Qty84 = 0;
                            Qty84hw = 0;
                        }
                    }
                    else {
                        if (parseFloat(Qty84) <= parseFloat(0)) {
                            Qty84 = tempQty84;
                            Qty84hw = tempQty84;
                        }
                    }
                    if (tempQty96 < 0) {
                        if (Qty96 > (-(tempQty96))) {
                            Qty96 = Qty96 + tempQty96;
                            Qty96hw = Qty96;
                        }
                        else {
                            Qty96 = 0;
                            Qty96hw = 0;
                        }
                    }
                    else {
                        if (parseFloat(Qty96) <= parseFloat(0)) {
                            Qty96 = tempQty96;
                            Qty96hw = tempQty96;
                        }
                    }
                    if (tempQty108 < 0) {
                        if (Qty108 > (-(tempQty108))) {
                            Qty108 = Qty108 + tempQty108;
                            Qty108hw = Qty108;
                        }
                        else {
                            Qty108 = 0;
                            Qty108hw = 0;
                        }
                    }
                    else {
                        if (parseFloat(Qty108) <= parseFloat(0)) {
                            Qty108 = tempQty108;
                            Qty108hw = tempQty108;
                        }
                    }
                    if (parseFloat(Qty84) < parseFloat(0)) {
                        if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = '0'; }
                    }
                    else {
                        if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                    }

                    if (parseFloat(Qty96) < parseFloat(0)) {
                        if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = '0'; }
                    }
                    else {
                        if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }
                    }
                    if (parseFloat(Qty108) < parseFloat(0)) {
                        if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = '0'; }
                    }
                    else {
                        if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = Qty108; }
                    }

                    if (parseFloat(Qty84hw) < parseFloat(0)) {
                        if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = '0'; }
                    }
                    else {
                        if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                    }
                    if (parseFloat(Qty96hw) < parseFloat(0)) {
                        if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = '0'; }
                    }
                    else {
                        if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }
                    }
                    if (parseFloat(Qty108hw) < parseFloat(0)) {
                        if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = '0'; }
                    }
                    else {
                        if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = Qty108hw; }
                    }


                }
                else if (document.getElementById(chkallowed) != null && document.getElementById(chkallowed).checked == false) {
if (document.getElementById("ContentPlaceHolder1_hdnallsku") != null) {
                        if (document.getElementById("ContentPlaceHolder1_hdnallsku").value == '') {
                           // document.getElementById("ContentPlaceHolder1_hdnallsku").value = ",";
                        }
                        if (document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value == '') {
                            //document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value = ",";
                        }

                        if (document.getElementById(skuid) != null) {
                            var s = document.getElementById(skuid).innerHTML;

                          //  document.getElementById("ContentPlaceHolder1_hdnallsku").value = document.getElementById("ContentPlaceHolder1_hdnallsku").value + s + ",";
                           // document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value = document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value + 'OFF' + ",";

                            document.getElementById("ContentPlaceHolder1_hdnallsku").value = s + ',' + document.getElementById("ContentPlaceHolder1_hdnallsku").value;
                            document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value = 'OFF,' + document.getElementById("ContentPlaceHolder1_hdnhemmingstatus").value;
                            if (document.getElementById(hdnchkallowed) != null &&  (document.getElementById(hdnchkallowed).value == '1' || document.getElementById(hdnchkallowed).value.toString().toLowerCase() == 'true')) {
                                document.getElementById("ContentPlaceHolder1_hdnonoffstatus").value = 'ON,' + document.getElementById("ContentPlaceHolder1_hdnonoffstatus").value;
                            }
                            else if (document.getElementById(hdnchkallowed) != null && (document.getElementById(hdnchkallowed).value == '' || document.getElementById(hdnchkallowed).value == '0' || document.getElementById(hdnchkallowed).value.toString().toLowerCase() == 'false')) {
                                document.getElementById("ContentPlaceHolder1_hdnonoffstatus").value = 'OFF,' + document.getElementById("ContentPlaceHolder1_hdnonoffstatus").value;
                            }


                        }


                    }
                    
                    //if (document.getElementById('ContentPlaceHolder1_chkonoff') != null && document.getElementById('ContentPlaceHolder1_chkonoff').checked == true) {
                    //    txtsaleschannelvalue = txttotalinventoryvalue - txtsafetyHandvalue;
                    //    txthpdwebsitevalue = txtinventoryonHandvalue - txtsafetyHandvalue;
                    //}
                    //else {
                    //    txtsaleschannelvalue = txttotalinventoryvalue - txtsafetyHandvalue;
                    //    txthpdwebsitevalue = txtinventoryonHandvalue;
                    //}
                     



                    ////  txtsaleschannelvalue = txttotalinventoryvalue;
                    //// txthpdwebsitevalue = txtinventoryonHandvalue;

                    //if (parseFloat(txtsaleschannelvalue) < parseFloat(0)) {
                    //    document.getElementById(txtsaleschannel).value = '0';
                    //}
                    //else {
                    //    document.getElementById(txtsaleschannel).value = txtsaleschannelvalue;
                    //}
                    //if (parseFloat(txthpdwebsitevalue) < parseFloat(0)) {
                    //    document.getElementById(txthpdwebsite).value = '0';
                    //}
                    //else {
                    //    document.getElementById(txthpdwebsite).value = txthpdwebsitevalue;
                    //}


                    var allexist1 = document.getElementById('ContentPlaceHolder1_grdoptionmainGroup').getElementsByClassName(document.getElementById(skuid).innerHTML);

                    var tempQty84 = 0;
                    var tempQty96 = 0;
                    var tempQty108 = 0;
                    var tempQty120 = 0;
                    var StoreQty = 0;
                    for (var i = 0; i < allexist1.length; i++) {

                        var elt111 = allexist1[i];
                        var elt = allexist1[i];
                        var varrr = elt.id.toString();//.replace('_chkallowed_', '_ltSKU_');

                        if (varrr.toString().toLowerCase().indexOf('_ltsku') > -1) {

                            if (elt.innerHTML.toString().toLowerCase().indexOf('-84') > -1) {

                                var Qty84id1 = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                var Qty84idhw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                var total84id = varrr.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty84id = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total84value = 0; var safty84value = 0;
                                if (document.getElementById(total84id) != null && document.getElementById(total84id).value != "") { total84value = document.getElementById(total84id).value }
                                if (document.getElementById(safty84id) != null && document.getElementById(safty84id).value != "") { safty84value = document.getElementById(safty84id).value }
                                // if (parseFloat(total84value) >= parseFloat(safty84value)) {
                                Qty84Orgdiff = parseFloat(total84value) - parseFloat(safty84value);
                                //}
                                if (parseFloat(Qty84Orgdiff) < parseFloat(0)) {
                                    Qty84Orgdiff = 0;
                                }
                                document.getElementById(Qty84id1).value = Qty84Orgdiff;
                                document.getElementById(Qty84idhw).value = total84value;

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-96') > -1) {

                                var Qty96id1 = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                var Qty96idhw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                var total96id = varrr.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty96id = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total96value = 0; var safty96value = 0;
                                if (document.getElementById(total96id) != null && document.getElementById(total96id).value != "") { total96value = document.getElementById(total96id).value; }
                                if (document.getElementById(safty96id) != null && document.getElementById(safty96id).value != "") { safty96value = document.getElementById(safty96id).value; }
                                //  if (parseFloat(total96value) >= parseFloat(safty96value)) {
                                Qty96Orgdiff = parseFloat(total96value) - parseFloat(safty96value);
                                if (parseFloat(Qty96Orgdiff) < parseFloat(0)) {
                                    Qty96Orgdiff = 0;
                                }
                                //  }
                                document.getElementById(Qty96id1).value = Qty96Orgdiff;
                                document.getElementById(Qty96idhw).value = total96value;

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-108') > -1) {

                                var Qty108id1 = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                var Qty108idhw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                var total108id = varrr.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty108id = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                var total108value = 0; var safty108value = 0;
                                if (document.getElementById(total108id) != null && document.getElementById(total108id).value != "") { total108value = document.getElementById(total108id).value; }
                                if (document.getElementById(safty108id) != null && document.getElementById(safty108id).value != "") { safty108value = document.getElementById(safty108id).value; }
                                //if (parseFloat(total108value) >=  parseFloat(safty108value))
                                // {
                                Qty108Orgdiff = parseFloat(total108value) - parseFloat(safty108value);
                                if (parseFloat(Qty108Orgdiff) < parseFloat(0)) {
                                    Qty108Orgdiff = 0;
                                }
                                // }
                                document.getElementById(Qty108id1).value = Qty108Orgdiff;
                                document.getElementById(Qty108idhw).value = total108value;

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-120') > -1) {

                                var Qty120id1 = varrr.replace(/_ltSKU/ig, '_txtsaleschannelsw');
                                var Qty120idhw = varrr.replace(/_ltSKU/ig, '_txthpdwebsitesw');
                                var total120id = varrr.replace(/_ltSKU/ig, '_txttotalinventory');
                                var total120value = 0; var safty120value = 0;
                                if (document.getElementById(total120id) != null && document.getElementById(total120id).value != "") { total120value = document.getElementById(total120id).value; }
                                var safty120id = varrr.replace(/_ltSKU/ig, '_txtsafetyHandsw');
                                if (document.getElementById(safty120id) != null && document.getElementById(safty120id).value != "") { safty120value = document.getElementById(safty120id).value; }
                                // if (parseFloat(total120value) >=  parseFloat(safty120value))
                                // {
                                Qty120Orgdiff = parseFloat(total120value) - parseFloat(safty120value);
                                //}
                                if (parseFloat(Qty120Orgdiff) < parseFloat(0)) {
                                    Qty120Orgdiff = 0;
                                }
                                document.getElementById(Qty120id1).value = Qty120Orgdiff;
                                document.getElementById(Qty120idhw).value = total120value;

                            }


                        }
                    }







                }
            }


        }





    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divcontentrow1" runat="server" class="content-row1">
    </div>
    <div class="content-row2">
        <div style="background: url(/App_Themes/Gray/images/title-bg.jpg) repeat scroll left top #545454; border: 1px solid #999999; color: #000000; font-weight: normal; height: 26px; margin: 10px 0 0 0;">
            <div style="vertical-align: middle; float: left; margin: 5px 0 0 0;">
                &nbsp;Product Hemming
            </div>
            &nbsp;
        </div>
<div style="display: block;" id="password" runat="server" visible="true">
            <table border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                <tr>
                    <td valign="top" style="font-size: 11px;">
                        <span class="required-red"></span>Password
                    </td>
                    <td>:
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtpassword" runat="server" Style="font-size: 11px;" CssClass="order-textfield"
                            Width="143" TextMode="Password"></asp:TextBox>
                    </td>


                </tr>

                <tr class="oddrow">
                    <td colspan="2"></td>
                    <td align="left">
                        <asp:ImageButton ID="btnsubmit" OnClientClick="return CheckPass();"
                            OnClick="btnsubmit_Click" runat="server" ImageUrl="/App_Themes/Gray/images/submit.gif" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: right; width: 100%; line-height: 40px;" id="divsearch" runat="server" visible="false">
            <div style="float: left; margin-top: 10px;">
                <div style="float: left; vertical-align: top;">
                    <b>Globally Hemming&nbsp;&nbsp;</b>
                </div>
                <div style="float: left;" id="divshippingtime" runat="server">
                </div>
                <div style="display: none">
                    <asp:CheckBox ID="chkonoff" runat="server" />
                </div>


            </div>
            <div style="float: right;">
                Search By(SKU or UPC):
                <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"></asp:TextBox>&nbsp;<asp:Button
                    ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="if(document.getElementById('ContentPlaceHolder1_btnExport') != null){document.getElementById('ContentPlaceHolder1_btnExport').style.display='none';}" />&nbsp;<asp:Button ID="btnExport" runat="server" ToolTip="Export" Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border: none; cursor: pointer;"
                        OnClick="btnExport_Click" Visible="false" />&nbsp;<asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="if(document.getElementById('ContentPlaceHolder1_btnExport') != null){document.getElementById('ContentPlaceHolder1_btnExport').style.display='none';}" /> <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />
                                                                             <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" ValidationGroup="importfile" CausesValidation="true" 
                                                                />
  <asp:RegularExpressionValidator ID="revImage" ControlToValidate="uploadCSV" ValidationExpression="(.*?)\.(csv)$"
                                                            Text="Select csv File Only (Ex.: .csv)" runat="server"
                                                            ValidationGroup="importfile" Display="Dynamic" ForeColor="Red" />
                                               <asp:RequiredFieldValidator ID="reqfile" runat="server"  Text="Please select.csv file" ValidationGroup="importfile"  ControlToValidate="uploadCSV"  Display="Dynamic" ForeColor="Red" ></asp:RequiredFieldValidator>
                                               <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>&nbsp;<a onclick="OpenCenterWindow('/Admin/Products/viewhemminglog.aspx',900,500)" style="color:red;"
                                                                    href="javascript:void(0);"><img src="/App_Themes/Gray/images/viewlog.png" /></a>
            </div>
        </div>
        <div style="float: left; width: 100%;" id="divhemming" runat="server">
            <asp:GridView ID="grdoptionmainGroup" runat="server" GridLines="None" AutoGenerateColumns="false"
                CssClass="checklist-main border-right-all" AllowPaging="false" PageSize="40" BorderColor="#999"
                BorderWidth="1px" BorderStyle="Solid" ShowFooter="false" Width="100%" OnRowDataBound="grdoptionmainGroup_RowDataBound"
                OnPageIndexChanging="grdoptionmainGroup_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="&nbsp;SKU">
                        <ItemTemplate>
                            <asp:Label ID="ltSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                            <input type="hidden" id="hdnchkallowed" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"IsHamming") %>' />
                            <input type="hidden" id="hdnProductID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                            <input type="hidden" id="hdnProductInventoryID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ProductInventoryID") %>' />
                            <input type="hidden" id="hdnHemmingPercentage" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"HemmingPercentage") %>' />
                            <input type="hidden" id="hdntype" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ItemType") %>' />
                            <input type="hidden" id="hdnInventory" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>' />
                            <input type="hidden" id="hdnHemmingqty" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"HammingSafetyQty") %>' />
                            <input type="hidden" id="hdnBorder" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"border") %>' />
 <input type="hidden" id="hdncountinue" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"discountinue") %>' />
                            
                        </ItemTemplate>
                        <HeaderStyle Width="13%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="13%" />
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UPC">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUPC" onkeypress="return keyRestrict(event,'0123456789');" Style="width: 150px; text-align: left;"
                                runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UPC") %>'
                                CssClass="order-textfield"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle Width="14%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="14%" />
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inventory On Hand">
                        <ItemTemplate>
                            <asp:TextBox ID="txtinventoryonHand" onkeypress="return keyRestrict(event,'0123456789');"
                                Style="width: 80px; text-align: center;border:none !important;" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>'
                                CssClass="order-textfield"></asp:TextBox>
                            <asp:TextBox ID="txttotalinventory" Style="width: 80px; text-align: center; display: none;" runat="server"
                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="10%" />
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                   <%-- <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Location">
                        <ItemTemplate>
                            <asp:Label ID="ltlocation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="14%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="14%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="Livermore">
                        <ItemTemplate>
                            <asp:Label ID="ltlocation" runat="server" Text=''></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="7%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="7%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Atlanta">
                        <ItemTemplate>
                            <asp:Label ID="ltlocationatl" runat="server" Text=''></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="7%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="7%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Safety Lock Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtsafetyHandsw" onkeypress="return keyRestrict(event,'0123456789');"
                                Style="width: 80px; text-align: center;" runat="server" CssClass="order-textfield"
                                Text='<%# DataBinder.Eval(Container.DataItem,"HammingSafetyQty") %>' onkeyup="AllowandlockQtyVariantswatch(this.id)"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="10%" />
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Avilable For Channel Sales">
                        <ItemTemplate>
                            <asp:TextBox ID="txtsaleschannelsw" onkeypress="return keyRestrict(event,'0123456789');"
                                Style="width: 80px; text-align: center;border:none !important;" runat="server" CssClass="order-textfield"
                                Text='<%# DataBinder.Eval(Container.DataItem,"salechannel") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="10%" />
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Avilable for HPD">
                        <ItemTemplate>
                            <asp:TextBox ID="txthpdwebsitesw" onkeypress="return keyRestrict(event,'0123456789');"
                                Style="width: 80px; text-align: center;border:none !important;" runat="server" CssClass="order-textfield"
                                Text='<%# DataBinder.Eval(Container.DataItem,"Hpdsite") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="10%" />
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hemming Allowed">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkallowed" runat="server" onclick="AllowandlockQtyVariantChk(this.id);" />
                        </ItemTemplate>
                        <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="10%" />
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
<asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                             <asp:Label ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                        <ItemStyle Width="10%" />
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr style="display:none;">
                                <td colspan="100%">
                                    <div id="divchild<%# Eval("ProductID") %>" style="margin: 10px 0 10px 0; position: relative; left: 15px; overflow: auto; width: 98%;display:none;">
                                        <asp:GridView ID="grdvaluelisting" runat="server" GridLines="None" AutoGenerateColumns="false"
                                            CssClass="checklist-main border-right-all" AllowPaging="false" ShowHeader="True"
                                            ShowFooter="false" EmptyDataText="No Record Found(s)." EditRowStyle-HorizontalAlign="Center"
                                            BorderColor="#999999" BorderWidth="1px" BorderStyle="Solid" OnRowDataBound="grdvaluelisting_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="&nbsp;SKU">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ltSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                        <input type="hidden" id="hdnVariantValueID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' />
                                                        <input type="hidden" id="hdnHemmingPercentage" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"HemmingPercentage") %>' />
                                                        <input type="hidden" id="hdnAddiHemingQty" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"AddiHemingQty") %>' />
                                                        <input type="hidden" id="hdnProductVariantInventoryID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ProductVariantInventoryID") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="UPC">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtUPC" onkeypress="return keyRestrict(event,'0123456789');" Style="width: 150px; text-align: left;"
                                                            runat="server" CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"UPC") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inventory On Hand">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtinventoryonHand" onkeypress="return keyRestrict(event,'0123456789');"
                                                            Style="width: 80px; text-align: center;" runat="server" CssClass="order-textfield"
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:TextBox>

                                                        <asp:TextBox ID="txttotalinventory" Style="width: 80px; text-align: center; display: none;" runat="server"
                                                            CssClass="order-textfield" Text=''></asp:TextBox>

                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="&nbsp;Livermore">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ltlocation" runat="server" Text=''></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="&nbsp;Atlanta">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ltlocationatl" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="&nbsp;Atlanta&nbsp;Bulk">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ltlocationatlbulk" runat="server" Text=''></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Available For Sale (After 80% Global Safeyt Stock)" Visible="false">
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Safety Lock Qty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtsafetyHand" onkeypress="return keyRestrict(event,'0123456789');"
                                                            Style="width: 80px; text-align: center;" runat="server" CssClass="order-textfield"
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"AddiHemingQty") %>' onkeyup="AllowandlockQtyVariant(this.id)"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Avilable For Channel Sales">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtsaleschannel" onkeypress="return keyRestrict(event,'0123456789');"
                                                            Style="width: 80px; text-align: center;" runat="server" CssClass="order-textfield"
                                                            Text=''></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Avilable for HPD">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txthpdwebsite" onkeypress="return keyRestrict(event,'0123456789');"
                                                            Style="width: 80px; text-align: center;" runat="server" CssClass="order-textfield"
                                                            Text=''></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                    <ItemStyle Width="10%" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle HorizontalAlign="left" />
                <PagerSettings Mode="Numeric" Position="TopAndBottom" />
                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
            </asp:GridView>
        </div>
        <div style="float: right; width: 100%; height: 40px; padding-top: 5px;">
            <div id="divfloating" class="divfloatingcss" style="text-align: center; height: 25px; vertical-align: middle;">
                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" Visible="false" TabIndex="38" OnClientClick="javascript:document.getElementById('prepage').style.display = '';" />

            </div>
        </div>
        <div style="display: none;">

            <asp:ImageButton ID="btnSavetemp" runat="server" OnClick="btnSavetemp_Click" TabIndex="38" />
<input type="hidden" runat="server" id="hdnallsku" value="" />
<input type="hidden" runat="server" id="hdnhemmingstatus" value="" />
            <input type="hidden" runat="server" id="hdnonoffstatus" value="" />

        </div>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16px; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/Gray/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
