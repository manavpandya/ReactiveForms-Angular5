<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuickView.aspx.cs" Inherits="Solution.UI.Web.QuickView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Carrois+Gothic|Telex|Oxygen' rel='stylesheet'
        type='text/css' />
    <link rel="stylesheet" type="text/css" href="/css/style.css" />
    <style>
        .product-properties-img-box
        {
            float: left;
            padding: 10px 5px 0;
        }
        .order-swatch-item
        {
            float: left;
            margin: 0 3%;
            padding: 0;
            width: 94%;
            border: 1px dashed #B92127;
            background: #f2f2f2;
        }
        .order-swatch-item-content
        {
            float: left;
            padding: 2%;
            width: 96%;
            margin: 0;
        }
        .order-swatch-item-left
        {
            float: left;
            width: 150px;
            margin: 0;
            padding: 0;
            text-align: center;
            border: 1px solid #d1cfcf;
            background: #FFF;
        }
        .order-swatch-item-right
        {
            float: right;
            width: 60%;
            margin: 0;
            padding: 0;
        }
        .order-swatch-item-right p
        {
            float: left;
            width: 100%;
            margin: 0;
            padding: 10px 0 0 0;
        }
        .order-swatch-item-right-1
        {
            float: left;
            width: 88%;
            margin: 0;
            padding: 0;
        }
        .order-swatch-item-rightrow-1
        {
            float: left;
            color: #848383;
            margin: 0;
            padding: 0;
            line-height: 40px;
            font-weight: bold;
            font-size: 12px;
            width: 100%;
        }
        .order-swatch-item-rightrow-1 span
        {
            float: left;
            line-height: 40px;
        }
        .order-swatch-item-rightrow-1 strong
        {
            font-size: 14px;
            font-weight: bold;
            color: #b92127;
            line-height: 40px;
            float: left;
            padding: 0 0 0 10px;
        }
        .order-swatch-item-rightrow-2
        {
            float: left;
            color: #848383;
            margin: 0;
            padding: 0;
            line-height: 40px;
            font-weight: bold;
            font-size: 12px;
            width: 50%;
        }
        .order-swatch-item-rightrow-2 span
        {
            float: left;
            line-height: 40px;
        }
        .order-swatch-item-rightrow-2 strong
        {
            font-size: 14px;
            font-weight: bold;
            color: #b92127;
            line-height: 40px;
            float: left;
            padding: 0 0 0 10px;
        }
        .order-swatch-input
        {
            border: 1px solid #d1cfcf;
            width: 50px;
            height: 24px;
            margin: 8px 0 0 10px;
        }
        .order-swatch-item-rightrow-3
        {
            float: left;
            margin-top: 5px;
            margin: 0;
            padding: 0;
            line-height: 40px;
        }
    </style>
    <script type="text/javascript">
        function selecteddropdownvalue(dropid, valid) {
            var ddlname = 'Selectvariant-' + dropid;
            var ddl = 'divSelectvariant-' + dropid;
            var allselect = document.getElementById(ddl).getElementsByTagName('div');
            for (var iS = 0; iS < allselect.length; iS++) {
                var eltSelect = allselect[iS];
                if (eltSelect.id.toString().indexOf("divflat-radio-" + dropid + "-" + valid + "") > -1) {
                    eltSelect.className = '';
                    eltSelect.className = 'iradio_flat-red checked';
                    //eltSelect.style.zIndex = '-1';
                }
                else if (eltSelect.id.toString().indexOf("divflat-radio-" + dropid + "-") > -1) {
                    eltSelect.className = '';
                    eltSelect.className = 'iradio_flat-red';
                    // eltSelect.style.zIndex = '100000';
                }
            }

            document.getElementById(ddlname).value = valid;
        }
        function varianttabhideshow(id) {

            var allselect = document.getElementById('divVariant').getElementsByTagName('div');

            for (var iS = 0; iS < allselect.length; iS++) {
                var eltSelect = allselect[iS];
                if (eltSelect.id.toString().indexOf('divcolspan-') > -1) {
                    if (eltSelect.id.toString().replace('divcolspan-', '') == id) {
                        if (document.getElementById('divSelectvariant-' + id).style.display == 'none') {
                            //document.getElementById('divSelectvariant-' + id).style.display = '';
                            $('#divSelectvariant-' + id).slideToggle();
                            eltSelect.className = 'readymade-detail-pt1-pro active';
                        }
                        else {
                            $('#divSelectvariant-' + id).slideToggle();
                            //document.getElementById('divSelectvariant-' + id).style.display = 'none';
                            eltSelect.className = 'readymade-detail-pt1-pro';
                        }


                    }
                    else {
                        eltSelect.className = 'readymade-detail-pt1-pro';
                        document.getElementById('divSelectvariant-' + eltSelect.id.toString().replace('divcolspan-', '')).style.display = 'none';
                    }
                }

            }

        }
        function Clearsess(urlnm,pid) {

            //if (window.event.clientY < 0 && (window.event.clientX > (document.documentElement.clientWidth - 5) || window.event.clientX < 0)) {
            var xmlhttp;

            if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera,

                //Safari

                xmlhttp = new XMLHttpRequest();

            } else {// code for IE6, IE5

                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");

            }
             
            xmlhttp.open("POST", "/Creaesession.aspx?pnm=" + urlnm + "&pid=" + pid, false);

            xmlhttp.send();
            window.parent.location.href = urlnm;


        }
        function ShowSwatchMessage() {

            // document.getElementById('header-part').style.zIndex = -1;

            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay').height = '225px';
            document.getElementById('frmdisplay').width = '565px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:225px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

            document.getElementById('popupContact').style.width = '565px';
            document.getElementById('popupContact').style.height = '225px';
            window.scrollTo(0, 0);


            $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GeLimitMessage",
                            data: "{PId: 1}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {
                                var root = window.location.protocol + '//' + window.location.host;
                                document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + msg.d;
                            },
                            Error: function (x, e) {
                            }
                        });

            centerPopup();
            loadPopup();
            

        }

        //        function displayall() {
        //            if (document.getElementById('divtaball') != null) {
        //                document.getElementById('divtaball').style.display = '';
        //            }
        //            if (document.getElementById('divshippingtab') != null) {
        //                document.getElementById('divshippingtab').style.display = '';
        //            }
        //        }
        //        window.onload = displayall;
        //        var $j = jQuery.noConflict();
        //        $j(document).ready(function ($) {
        //            var imagename = '';
        //            if (document.getElementById('imgMain')) {
        //                imagename = document.getElementById('imgMain').src;
        //            }
        //            imagename = imagename.replace('medium', 'large');


        //            $('#imgMain').addimagezoom({
        //                zoomrange: [3, 10],
        //                magnifiersize: [300, 300],
        //                magnifierpos: 'right',
        //                cursorshade: true,
        //                largeimage: imagename //<-- No comma after last option!
        //            })
        ////             $("#Button2").click(function () {
        ////                 var imagename = document.getElementById('imgMain').src;
        ////                 imagename = imagename.replace('medium', 'large');
        ////                 $('#imgMain').addimagezoom({
        ////                     zoomrange: [3, 10],
        ////                     magnifiersize: [300, 300],
        ////                     magnifierpos: 'right',
        ////                     cursorshade: true,
        ////                     largeimage: imagename
        ////                 })
        ////             }) 


        //        })
        // 

        function tabdisplaycart(id, id1) {

            if (document.getElementById('liswatch') != null) {
                document.getElementById('liswatch').className = '';
            }
            if (document.getElementById('divswatch') != null) {
                document.getElementById('divswatch').style.display = 'none';
            }

            if (document.getElementById('licustom') != null) {
                document.getElementById('licustom').className = '';
            }
            if (document.getElementById('divcustom') != null) {
                document.getElementById('divcustom').style.display = 'none';
            }

            if (document.getElementById('liready') != null) {
                document.getElementById('liready').className = '';
            }
            if (document.getElementById('divready') != null) {
                document.getElementById('divready').style.display = 'none';
            }

            if (document.getElementById(id) != null) {
                document.getElementById(id).className = 'tabberactive';
            }
            if (document.getElementById(id1) != null) {
                document.getElementById(id1).style.display = '';
            }



        }

        function priceQuote() {
            if ((document.getElementById('ddlHeaderDesign').options[document.getElementById('ddlHeaderDesign').selectedIndex]).text == 'Select One') {
                alert('Please select Style.');
                document.getElementById('ddlHeaderDesign').focus();
                return false;
            }
            if ((document.getElementById('ddlFinishedWidth').options[document.getElementById('ddlFinishedWidth').selectedIndex]).text == 'Width') {

                alert('Please select Width.');
                document.getElementById('ddlFinishedWidth').focus();
                return false;
            }
            if ((document.getElementById('ddlFinishedLength').options[document.getElementById('ddlFinishedLength').selectedIndex]).text == 'Length') {

                alert('Please select Length.');
                document.getElementById('ddlFinishedLength').focus();
                return false;
            }
            if ((document.getElementById('ddlOptionType').options[document.getElementById('ddlOptionType').selectedIndex]).text == 'Options') {

                alert('Please select Options.');
                document.getElementById('ddlOptionType').focus();
                return false;
            }
            if ((document.getElementById('ddlQuantity').options[document.getElementById('ddlQuantity').selectedIndex]).text == 'Quantity') {

                alert('Please select Quantity.');
                document.getElementById('ddlQuantity').focus();
                return false;
            }
            ShowModelHelpShipping('/CustomQuoteSize.aspx?ProductId=<%=Request.QueryString["PID"] %>');
            return true;

        }
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(beginRequest);

        function beginRequest() {
            prm._scrollPosition = null;
        }

        function ShowModelHelpShipping(id) {
            //document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '730px';
            document.getElementById('frmdisplay1').width = '620px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:620px;height:730px;");
            document.getElementById('popupContact1').style.width = '620px';
            document.getElementById('popupContact1').style.height = '730px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;


        }

        function PriceChangeondropdown() {
            var price = document.getElementById('hdnActual').value;
            var saleprice = document.getElementById('hdnprice').value;
            if (parseFloat(saleprice) == parseFloat(0)) {
                saleprice = price;
            }
            var vprice = 0;
            var isselected = 0;
            if (document.getElementById('diestimatedate') != null) {
                $('#diestimatedate').html('');
                //$('#diestimatedate').attr('style', 'display:none;');
            }
            if (document.getElementById('divVariant')) {
                var allselect = document.getElementById('divVariant').getElementsByTagName('select');

                for (var iS = 0; iS < allselect.length; iS++) {
                    var eltSelect = allselect[iS];
                    var valsel = eltSelect.id.replace('Selectvariant-', 'divselvalue-');
                    if (eltSelect.selectedIndex != 0) {
                        var valsel1 = eltSelect.id.replace('Selectvariant-', 'divcolspan-');
                        if (eltSelect.options[0].text.toLowerCase().indexOf('header design') > -1) {
                            document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + eltSelect.options[0].text.toLowerCase().replace('select ', '').replace('select', '') + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + eltSelect.options[eltSelect.selectedIndex].text + '</strong>';
                        }
                        else {
                            document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + eltSelect.options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + eltSelect.options[eltSelect.selectedIndex].text + '</strong>';
                        }

                        if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('($') > -1) {

                            var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('($') + 2);
                            vtemp = vtemp.replace(/\)/g, '');
                            vprice = parseFloat(vprice) + parseFloat(vtemp);
                        }
                    }
                    else {
                        if (document.getElementById(valsel) != null) {
                            document.getElementById(valsel).innerHTML = '';
                        }
                        isselected++;
                    }
                }
            }
            if (parseFloat(vprice) > 0) {
                saleprice = parseFloat(vprice)
            }
            else {
                saleprice = parseFloat(saleprice) + parseFloat(vprice);
            }

            price = parseFloat(price);
            if (document.getElementById('divRegularPrice') != null) {
                document.getElementById('divRegularPrice').innerHTML = '<tt>MSRP :</tt> <span>$' + price.toFixed(2) + '</span>';
            }
            if (document.getElementById('divYourPrice') != null) {
                //document.getElementById('divYourPrice').innerHTML = '<tt>Your Price :</tt> <strong>$' + saleprice.toFixed(2) + '</strong>';
                var SalePriceTag;
                if (document.getElementById('hdnSalePriceTag') != null && document.getElementById('hdnSalePriceTag'.value) != '') {
                    var SalePriceTag = ' <span>' + document.getElementById('hdnSalePriceTag').value + '</span>';
                }
                document.getElementById('divYourPrice').innerHTML = '<tt>Your Price :</tt> <strong>$' + saleprice.toFixed(2) + '</strong>' + SalePriceTag;
            }

            if (document.getElementById('divYouSave') != null) {
                var diffprice = parseFloat(price) - parseFloat(saleprice);
                var diffpercentage = (parseFloat(diffprice) * parseFloat(100)) / parseFloat(price)
                document.getElementById('divYouSave').innerHTML = '<tt>You Save :</tt> <span style="color:#B92127;">$' + diffprice.toFixed(2) + '<span style="padding-left: 0px;color:#B92127;"> (' + diffpercentage.toFixed(2) + '%)</span></span>&nbsp;';
            }

            if (isselected == 0) {
                var qid = document.getElementById('txtQty').value;
                var pid = '<%=Request["PID"] %>';
                var Names = ""; var Values = "";
                if (document.getElementById('divVariant')) {
                    var allselect = document.getElementById('divVariant').getElementsByTagName('select');

                    for (var iS = 0; iS < allselect.length; iS++) {
                        var eltSelect = allselect[iS];
                        if (eltSelect.selectedIndex == 0) {
                            if (document.getElementById(valsel) != null) {
                                document.getElementById(valsel).innerHTML = '';
                            }
                        }
                        else {
                            var nametext = eltSelect.id.replace('Selectvariant-', 'divvariantname-');
                            Names = Names + document.getElementById(nametext).innerHTML + ',';
                            Values = Values + eltSelect.options[eltSelect.selectedIndex].text + ',';
                            var valsel = eltSelect.id.replace('Selectvariant-', 'divselvalue-');
                            if (document.getElementById(valsel) != null) {
                                // document.getElementById(valsel).innerHTML = '"' + eltSelect.options[0].text + ':' + eltSelect.options[eltSelect.selectedIndex].text + '"';
                                var valsel1 = eltSelect.id.replace('Selectvariant-', 'divcolspan-');
                                document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + eltSelect.options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + eltSelect.options[eltSelect.selectedIndex].text + '</strong>';

                            }

                        }
                    }
                }
                $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GetDataAdminMessage",
                            data: "{ProductId: " + pid + ",ProductType: 1,Qty: " + qid + ",vValueid: '" + Values + "',vNameid: '" + Names + "' }",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {
                                if (document.getElementById('diestimatedate') != null) {
                                    $('#diestimatedate').html('<span class="readymade-detail-left" style="width:100% !important;">' + msg.d + '</span>');
                                    $('#diestimatedate').attr('style', 'display:block;');
                                }
                            },
                            Error: function (x, e) {
                            }
                        });
            }

        }


        function sendData(dropid, divid) {

            var pid = '<%=Request["PID"] %>';
            var vid = document.getElementById(dropid).options[document.getElementById(dropid).selectedIndex].value;

            $.ajax(
                                   {
                                       type: "POST",
                                       url: "/TestMail.aspx/Getvariantdata",
                                       data: "{ProductId: " + pid + ",variantvalueId: " + vid + "}",
                                       contentType: "application/json; charset=utf-8",
                                       dataType: "json",
                                       async: "true",
                                       cache: "false",

                                       success: function (msg) {
                                           //$("#myDiv:last").append(msg.d);
                                           if (document.getElementById(divid) != null) {
                                               $('#' + divid).html(msg.d);
                                               PriceChangeondropdown();

                                           }


                                       },

                                       Error: function (x, e) {

                                           PriceChangeondropdown();
                                       }
                                   });


        }

        //        function sendData(id) {
        //            if (document.getElementById(id).options[document.getElementById(id).selectedIndex].text.toLowerCase().indexOf("custom") > -1) {
        //                document.getElementById('txtWidth').value = '';
        //                document.getElementById('txtLength').value = '';
        //                document.getElementById('txtWidth').onchange = function () { ChangeCustomprice(id); }
        //                document.getElementById('txtLength').onchange = function () { ChangeCustomprice(id); }
        //                document.getElementById('txtQty').onchange = function () { ChangeCustomprice(id); }
        //                document.getElementById('PCustom').style.display = '';
        //            }
        //            else {

        //                document.getElementById('txtWidth').value = '';
        //                document.getElementById('txtLength').value = '';
        //                ChangeCustomprice(id);
        //                document.getElementById('PCustom').style.display = 'none';
        //            }

        //        }

        function ChangeCustomprice() {

            if (document.getElementById('divcustomprice') != null) {
                $('#divcustomprice').attr('style', 'background: url(/images/priceloading.gif) no-repeat scroll 0 0 transparent;');
            }
            if (document.getElementById('hdnpricetemp') != null && document.getElementById('hdnpricetemp').value == '') {
                if (document.getElementById('divcustomprice') != null) {
                    document.getElementById('hdnpricetemp').value = document.getElementById('divcustomprice').innerHTML;
                }

            }
            var SalePriceTag;
            if (document.getElementById('hdnSalePriceTag') != null && document.getElementById('hdnSalePriceTag'.value) != '') {
                var SalePriceTag = ' <span>' + document.getElementById('hdnSalePriceTag').value + '</span>';
            }
            if (document.getElementById('divmeadetomeasure') != null) {

                $('#divmeadetomeasure').html('');
                //$('#divmeadetomeasure').attr('style', 'display:none;');
            }
            if (document.getElementById('ddlcustomstyle').selectedIndex != 0 && document.getElementById('ddlcustomoptin').selectedIndex != 0 && document.getElementById('ddlcustomlength').selectedIndex != 0 && document.getElementById('ddlcustomwidth').selectedIndex != 0 && document.getElementById('dlcustomqty').selectedIndex != 0) {
                var pid = '<%=Request["PID"] %>';
                var sid = document.getElementById('<%=ddlcustomstyle.ClientID %>').options[document.getElementById('<%=ddlcustomstyle.ClientID %>').selectedIndex].value;
                var wid = document.getElementById('<%=ddlcustomwidth.ClientID %>').options[document.getElementById('<%=ddlcustomwidth.ClientID %>').selectedIndex].value;
                var lid = document.getElementById('<%=ddlcustomlength.ClientID %>').options[document.getElementById('<%=ddlcustomlength.ClientID %>').selectedIndex].value;
                var oid = document.getElementById('<%=ddlcustomoptin.ClientID %>').options[document.getElementById('<%=ddlcustomoptin.ClientID %>').selectedIndex].value;
                var qid = document.getElementById('<%=dlcustomqty.ClientID %>').options[document.getElementById('<%=dlcustomqty.ClientID %>').selectedIndex].value;
                var strMeasureValue = sid + ',' + wid + ',' + lid + ',' + oid + ',' + qid;
                var strMeasureName = 'Header,Width,Length,Options,Quantity (Panels)';

                $.ajax(
                                   {
                                       type: "POST",
                                       url: "/TestMail.aspx/GetData",
                                       data: "{ProductId: " + pid + ",Width: " + wid + ",Length: " + lid + ",Qty: " + qid + ",style: '" + sid + "',options: '" + oid + "' }",
                                       contentType: "application/json; charset=utf-8",
                                       dataType: "json",
                                       async: "true",
                                       cache: "false",

                                       success: function (msg) {
                                           //$("#myDiv:last").append(msg.d);
                                           if (document.getElementById('divcustomprice') != null) {
                                               $('#divcustomprice').html('<tt>Your Price :</tt> <strong>$' + msg.d + '</strong>' + SalePriceTag);
                                               document.getElementById('<%=hdnpricecustomcart.ClientID %>').value = msg.d;
                                           }


                                       },

                                       Error: function (x, e) {

                                       }
                                   });

                                   $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GetDataAdminMessage",
                            data: "{ProductId: " + pid + ",ProductType: 2,Qty: " + qid + ",vValueid: '" + strMeasureValue + "',vNameid: '" + strMeasureName + "' }",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {

                                
                                if (document.getElementById('divmeadetomeasure') != null) {

                                    $('#divmeadetomeasure').html('<span class="readymade-detail-left" style="width:100% !important;">' + msg.d + '</span>');
                                    $('#divmeadetomeasure').attr('style', 'display:block;');

                                }
                               
                            },
                            Error: function (x, e) {
                            }
                        });


            }
            else {
                if (document.getElementById('divcustomprice') != null) {
                    $('#divcustomprice').html(document.getElementById('hdnpricetemp').value);
                }



            }

            if (document.getElementById('divcustomprice') != null) {
                $('#divcustomprice').attr('style', 'background: none;');
            }
        }

    </script>
    <script type="text/javascript">
        function chkHeight() {

            if (document.getElementById('prepage')) {
                var windowHeight = 0;
                windowHeight = $(document).height(); //window.innerHeight;

                document.getElementById('prepage').style.height = windowHeight + 'px';
                document.getElementById('prepage').style.display = '';
            }
        }
    </script>
    <style type="text/css">
        #blanket
        {
            background-color: #111;
            opacity: 0.65;
            filter: alpha(opacity=65);
            position: absolute;
            z-index: 9001;
            top: 0px;
            left: 0px;
            width: 100%;
        }
        #popUpDiv
        {
            position: absolute;
            width: 320px;
            height: 390px;
            z-index: 9002;
        }
        #backgroundPopup
        {
            display: none;
            position: fixed;
            _position: absolute;
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background: #000000;
            border: 1px solid #cecece;
            z-index: 1;
        }
        #popupContact
        {
            display: none;
            position: fixed;
            _position: absolute;
            width: 269px;
            background: #FFFFFF;
            border: 2px solid #cecece;
            z-index: 2;
            padding: 12px;
            font-size: 13px;
        }
        #popupContact h1
        {
            text-align: left;
            color: #6FA5FD;
            font-size: 22px;
            font-weight: 700;
            border-bottom: 1px dotted #D3D3D3;
            padding-bottom: 2px;
            margin-bottom: 20px;
        }
        #popupContactClose
        {
            font-size: 14px;
            line-height: 14px;
            right: 6px;
            top: 4px;
            position: absolute;
            color: #6fa5fd;
            font-weight: 700;
            display: block;
        }
        #button
        {
            text-align: center;
            margin: 100px;
        }
        #btnreadmore
        {
            text-align: center;
            margin: 100px;
        }
        #btnhelpdescri
        {
            text-align: center;
            margin: 100px;
        }
        #popupContact1
        {
            display: none;
            _position: absolute;
            width: 750px;
            background: #FFFFFF;
            border: 2px solid #cecece;
            z-index: 2;
            padding: 12px;
            font-size: 13px;
        }
        #popupContact1 h1
        {
            text-align: left;
            color: #6FA5FD;
            font-size: 22px;
            font-weight: 700;
            border-bottom: 1px dotted #D3D3D3;
            padding-bottom: 2px;
            margin-bottom: 20px;
        }
        #popupContactClose1
        {
            font-size: 14px;
            line-height: 14px;
            right: 6px;
            top: 4px;
            position: absolute;
            color: #6fa5fd;
            font-weight: 700;
            display: block;
        }
    </style>
    <%--New Added--%>
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script src="/js/featuredimagezoomer.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/jquery.jcarousel.pack_itempage.js"></script>
    <script type="text/javascript">
        var $j = jQuery.noConflict();
        $j(document).ready(function () {


            //            $j('#mycarousel').jcarousel({
            //                vertical: true,
            //                scroll: 2
            //            });


            $j("#divproperty").click(function () {
                $j('#divproperty1').slideToggle();
                if (document.getElementById('imgPro') != null) {
                    if (document.getElementById('imgPro').src.toString().toLowerCase().indexOf('minimize.png') > -1) {
                        $j('#imgPro').attr("src", document.getElementById('imgPro').src.replace('minimize.png', 'expand.gif'));
                        $j('#imgPro').attr("title", 'Expand');
                    }
                    else {
                        $j('#imgPro').attr("src", document.getElementById('imgPro').src.replace('expand.gif', 'minimize.png'));
                        $j('#imgPro').attr("title", 'Collapse');
                    }
                }
                $('#divscroll').animate({ scrollTop: $('#divproperty1').offset().top }, 'slow');
            });

            $j("#divProductDescription").click(function () {
                $j('#divProductDescription1').slideToggle();
                if (document.getElementById('imgProductDescription') != null) {
                    if (document.getElementById('imgProductDescription').src.toString().toLowerCase().indexOf('minimize.png') > -1) {
                        $j('#imgProductDescription').attr("src", document.getElementById('imgProductDescription').src.replace('minimize.png', 'expand.gif'));
                        $j('#imgProductDescription').attr("title", 'Expand');
                    }
                    else {
                        $j('#imgProductDescription').attr("src", document.getElementById('imgProductDescription').src.replace('expand.gif', 'minimize.png'));
                        $j('#imgProductDescription').attr("title", 'Collapse');
                    }
                }
                $('#divscroll').animate({ scrollTop: $('#divProductDescription').offset().top - 70 }, 'slow');
            });


            $j("#divProductFeatures").click(function () {
                $j('#divProductFeatures1').slideToggle();
                if (document.getElementById('imgProductFeatures') != null) {
                    if (document.getElementById('imgProductFeatures').src.toString().toLowerCase().indexOf('minimize.png') > -1) {
                        $j('#imgProductFeatures').attr("src", document.getElementById('imgProductFeatures').src.replace('minimize.png', 'expand.gif'));
                        $j('#imgProductFeatures').attr("title", 'Expand');
                    }
                    else {
                        $j('#imgProductFeatures').attr("src", document.getElementById('imgProductFeatures').src.replace('expand.gif', 'minimize.png'));
                        $j('#imgProductFeatures').attr("title", 'Collapse');
                    }
                }
                $('#divscroll').animate({ scrollTop: $('#divProductFeatures').offset().top - 70 }, 'slow');

            });

            $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });


            //            if (document.getElementById('hdnIsShowImageZoomer').value == "true" && document.getElementById('imgMain').src.indexOf('image_not_available') <= -1) {
            //                var imagename = '';
            //                if (document.getElementById('imgMain')) {
            //                    imagename = document.getElementById('imgMain').src;
            //                }
            //                imagename = imagename.replace('medium', 'large');


            //                $j('#imgMain').addimagezoom({
            //                    zoomrange: [3, 10],
            //                    magnifiersize: [300, 300],
            //                    magnifierpos: 'right',
            //                    cursorshade: true,
            //                    largeimage: imagename //<-- No comma after last option!
            //                });
            //                $j("#Button2").click(function () {
            //                    var imagename = document.getElementById('imgMain').src;
            //                    imagename = imagename.toLowerCase().replace('medium', 'large');

            //                    $j('#imgMain').addimagezoom({
            //                        zoomrange: [3, 10],
            //                        magnifiersize: [300, 300],
            //                        magnifierpos: 'right',
            //                        cursorshade: true,
            //                        largeimage: imagename + '?54666'
            //                    })
            //                });
            //}

        });
    </script>
    <%--<script src="/js/tabber.js" type="text/javascript"> </script>--%>
    <%--New Added--%>
    <script src="/js/jquery-1.3.2.js" type="text/javascript"> </script>
    <script type="text/javascript" src="/js/jquery-alerts.js"></script>
    <script src="/js/popup.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/css/jquery.alerts.css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <%--<link href="css/popup_gallery.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        function ShowModelHelp(id) {

            //document.getElementById('header-part').style.zIndex = -1;

            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay').height = '650px';
            document.getElementById('frmdisplay').width = '710px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:517px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

            document.getElementById('popupContact').style.width = '710px';
            document.getElementById('popupContact').style.height = '650px';
            window.scrollTo(0, 0);
            document.getElementById('btnreadmore').click();
            document.getElementById('frmdisplay').src = '/MoreImages.aspx?PID=' + id;
        }

        function ShowInventoryMessage(result) {
            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay').height = '130px';
            document.getElementById('frmdisplay').width = '565px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:130px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

            document.getElementById('popupContact').style.width = '565px';
            document.getElementById('popupContact').style.height = '130px';
            window.scrollTo(0, 0);
            var root = window.location.protocol + '//' + window.location.host;
            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root +'/css/style.css" rel="stylesheet" type="text/css" />' + result;
            centerPopup();
            loadPopup();
        }
    </script>
    <script type="text/javascript">

    function SelectVariantBypostback(strname,strvalue)
    {
    var variantname = strname.split(","); 
    var variantvalue = strvalue.split(","); 

    for (i = 0; i < variantname.length; i++) {
    if( variantvalue[i].toLowerCase().indexOf('select ') > -1)
    {
    if(document.getElementById(variantname[i]) != null)
    {
    document.getElementById(variantname[i]).value = '0';
    }
    }
    else
    {
    if(document.getElementById(variantname[i]) != null)
    {
   document.getElementById(variantname[i]).value = variantvalue[i];
   }
   }


    }

    }

    function variantDetail(divid) {
    centerPopup1();
            //document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
             document.getElementById('frmdisplay1').height = '450px';
            document.getElementById('frmdisplay1').width = '517px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:517px;height:450px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
             document.getElementById('popupContact1').style.width = '517px';
             document.getElementById('popupContact1').style.height = '450px';
             
            document.getElementById('btnhelpdescri').click();
 
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML =  document.getElementById(divid).innerHTML;
        }

        function ShowModelHelpShipping(id) {
           // document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '620px';
            document.getElementById('frmdisplay1').width = '630px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:630px;height:620px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '630px';
            document.getElementById('popupContact1').style.height = '620px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }

         function ShowModelForNotification(id) {
            //document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '400px';
            document.getElementById('frmdisplay1').width = '610px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:610px;height:400px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '610px';
            document.getElementById('popupContact1').style.height = '400px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }

        
        function onKeyPressBlockNumbers1(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key != 46) {
                if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 0) {
                    return key;
                }
            }
            if (key == 13) {
                if (document.getElementById('btnAddToCart') != null) {
                    document.getElementById('btnAddToCart').click();
                }
            }
            var keychar = String.fromCharCode(key);
            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
        function onKeyPressBlockNumbersOnly(e) {
            var key = window.event ? window.event.keyCode : e.which;
           
            if (key != 46) {
                if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 0) {
                    return key;
                }
            }
            if (key == 13) {
                if (document.getElementById('btnAddToCart') != null) {
                    document.getElementById('btnAddToCart').click();
                }
            }
            var keychar = String.fromCharCode(key);
            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
        function checkquantity() {
                <%=strScriptVar %>

            if ((document.getElementById("txtQty").value == '') || (document.getElementById("txtQty").value <= 0) || isNaN(document.getElementById('txtQty').value)) {
                jAlert("Please enter valid digits only !!!", "Message", "txtQty");
                document.getElementById("txtQty").value = 1; return false;
            }
            else if (document.getElementById("divOptionalAccessories")) {
                var allElts = document.getElementById("gvOptionalAcc").getElementsByTagName('INPUT');
                var i;
                for (i = 0; i < allElts.length; i++) {
                    var elt = allElts[i];
                    if (elt.type == "checkbox" && elt.checked == true) {
                        var refQty = elt.id.replace('ckhOASelect', 'txtOAQty');

                        if (document.getElementById(refQty) != null && (document.getElementById(refQty).value == '' || document.getElementById(refQty).value == '0')) {
                            jAlert("Please enter valid Quantity.", "Message", refQty);
                            return false;
                        }
                    }
                }
            }

            else if (document.getElementById('divParent') != null && document.getElementById('ddlIsFreeEngraving').selectedIndex == 0) {
                var allDiv = document.getElementById('divParent').getElementsByTagName('div');
                var TotQty = 0;
                var k = 0;
                var check = false;
                for (k = 0; k < allDiv.length; k++) {
                    var controlid = allDiv[k];
                    if (controlid.id.indexOf('divmain') > -1) {
                        var allDiv1 = controlid.getElementsByTagName('*');
                        var j = 0;
                        for (j = 0; j < allDiv1.length; j++) {
                            var controlid1 = allDiv1[j];

                            if (controlid1.type == 'text' && controlid1.id.indexOf('txtQty') > -1 && (controlid1.value != '' && controlid1.value >= 0)) {
                                TotQty = parseInt(TotQty) + parseInt(controlid1.value);
                            }
                            if (controlid1.type == 'text' && controlid1.id.indexOf('txtQty') > -1 && (controlid1.value == '' || controlid1.value <= 0 || isNaN(controlid1.value))) {
                                var idtext = controlid1.id;
                                jAlert("Please enter valid digits only !!!", "Message", controlid1.id);
                                document.getElementById(controlid1.id).value = 1; return false;
                                check = true;
                                return false;
                            }
                            else if (controlid1.type == 'text' && controlid1.value == '') {
                                var idtext = controlid1.id.replace('txt1', 'hdnpname');
                                jAlert("Please enter " + document.getElementById(idtext).value + " !!!", "Message", controlid1.id);
                                check = true;
                                return false;
                            }

                            else if (controlid1.type == 'select-one' && controlid1.id.indexOf('select') > -1 && controlid1.selectedIndex == 0) {
                                var idtext = controlid1.id.replace('select', 'hdnpname111');
                                if (document.getElementById(idtext) != null) {
                                    jAlert("Please Select " + document.getElementById(idtext).value + " !!!", "Message", controlid1.id);
                                    check = true;
                                    return false;
                                }
                            }
                        }
                    }
                }
                if (check == false) {
                    document.getElementById("hdnQuantity").value = TotQty;
                    document.getElementById("prepage").style.display = ''; return true;
                }
            }
            else
            { document.getElementById("prepage").style.display = ''; return true; }
        }


        function FreeEngraving() {
            if (document.getElementById('ddlIsFreeEngraving') != null) {
                if ((document.getElementById('ddlIsFreeEngraving').options[document.getElementById('ddlIsFreeEngraving').selectedIndex]).text == 'Yes') {
                    if (document.getElementById('hdnEngraving') != null && document.getElementById('hdnEngraving').value == '') {
                        document.getElementById('hdnEngraving').value = document.getElementById('divmain').innerHTML;
                    }
                    var allDiv = document.getElementById('divParent').getElementsByTagName('div');
                    var i = 0;
                    for (i = 0; i < allDiv.length; i++) {
                        var divid = allDiv[i];
                        if (divid.id.indexOf('divmain') > -1) {
                            divid.style.display = '';
                            document.getElementById('divQuantity').style.display = 'none';
                        }
                    }
                    document.getElementById('ddlIsFreeEngraving').selectedIndex = 0;
                }
                else {
                    var allDiv = document.getElementById('divParent').getElementsByTagName('div');
                    var i = 0;
                    for (i = 0; i < allDiv.length; i++) {
                        var divid = allDiv[i];
                        if (divid.id.indexOf('divmain') > -1) {
                            divid.style.display = 'none';
                            document.getElementById('divQuantity').style.display = '';
                        }
                    }
                    document.getElementById('ddlIsFreeEngraving').selectedIndex = 1;
                }
            }
        }

        function checkTest() {
            if (document.getElementById('hdnEngraving') != null && document.getElementById('hdnEngraving').value != '') {
                var tt = document.getElementById('divParent').innerHTML;
                var allInput = document.getElementById('divParent').getElementsByTagName('*');

                tt = tt.replace('style="display: none;" onclick="removeele', 'style="display: block;" onclick="removeele');
                tt = tt.replace('style="display: block;" onclick="checkTest();"', 'style="display: none;" onclick="checkTest();"');

                var NewdivName = 0;
                var TotcountNew = 0;
                var Totcount = 0;
                if (document.getElementById('hdnCountEngraving') != null) {
                    if (document.getElementById('hdnCountEngraving').value != '') {
                        Totcount = document.getElementById('hdnCountEngraving').value;
                        TotcountNew = parseInt(Totcount) + parseInt(1);
                        NewdivName = 'divmain' + TotcountNew;
                        document.getElementById('hdnCountEngraving').value = TotcountNew;
                    }
                    else
                    { document.getElementById('hdnCountEngraving').value = "1"; NewdivName = 'divmain1'; }
                }

                var newDiv = "<div id='" + NewdivName + "'>" + document.getElementById('hdnEngraving').value.replace('divmain', NewdivName).replace(/_kau_/g, TotcountNew) + "</div>";

                var j = 0;
                var str = "";
                var strvalue = "";
                for (j = 0; j < allInput.length; j++) {
                    var inttype = allInput[j];
                    if (inttype.type == 'text') {
                        str = str + inttype.id.toString() + ',';
                        strvalue = strvalue + inttype.value.toString() + ',';
                    }
                    else if (inttype.type == 'select-one') {
                        str = str + inttype.id.toString() + ',';
                        strvalue = strvalue + inttype.selectedIndex.toString() + ',';
                    }
                }
                document.getElementById('divParent').innerHTML = tt + newDiv;
                var n = str.split(",");
                var m = strvalue.split(",");
                var k = 0;
                for (k = 0; k < n.length; k++) {
                    var controlid = n[k];
                    if (controlid.indexOf('txt') > -1) {
                        document.getElementById(controlid).value = m[k];
                    }
                    else if (controlid.indexOf('select') > -1) {
                        document.getElementById(controlid).selectedIndex = m[k];
                    }
                }
                document.getElementById('ddlIsFreeEngraving').selectedIndex = 0;
            }
        }

        function removeelement(id) {
            document.getElementById(id).innerHTML = '';
        }

        function AddElement() {
        }

        function AddTocartForEngra() {
            var strname = "";
            var strvalue = "";
            var strQty = "";

            if (document.getElementById('divParent') != null && document.getElementById('ddlIsFreeEngraving').selectedIndex == 0) {
                var allDiv = document.getElementById('divParent').getElementsByTagName('div');

                var k = 0;
                for (k = 0; k < allDiv.length; k++) {
                    var controlid = allDiv[k];
                    if (controlid.id.indexOf('divmain') > -1) {

                        strname = strname + "divmain=";
                        strvalue = strvalue + "divmain=";
                        strQty = strQty + "divmain=";
                        var allDiv1 = controlid.getElementsByTagName('*');
                        var j = 0;

                        for (j = 0; j < allDiv1.length; j++) {
                            var controlid1 = allDiv1[j];

                            if (controlid1.id.indexOf('pname') > -1 && controlid1.type != 'hidden') {

                                var hdnid = "hdn" + controlid1.id;
                                strname = strname + document.getElementById(hdnid).value + ',';
                            }
                            if (controlid1.type == 'text' && controlid1.id.indexOf('txtQty') > -1) {
                                strQty = strQty + controlid1.value + ',';
                            }
                            else if (controlid1.type == 'text') {
                                strvalue = strvalue + controlid1.value + ',';
                            }
                            else if (controlid1.type == 'select-one') {
                                strvalue = strvalue + controlid1.options[controlid1.selectedIndex].text + ',';
                            }
                        }
                    }
                }
            }

            document.getElementById('hdnEngravName').value = strname;
            document.getElementById('hdnEngravvalue').value = strvalue;
            document.getElementById('hdnEngravQty').value = strQty;
        }

    </script>
    <%--  <script src="/js/jquery-1.3.2.js" type="text/javascript"> </script>
    <script src="/js/featuredimagezoomer.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        //        jQuery(document).ready(function ($) {
        //            var imagename = '';
        //            if (document.getElementById('imgMain')) {
        //                imagename = document.getElementById('imgMain').src;
        //            }
        //            imagename = imagename.replace('medium', 'large');
        //            $('#imgMain').addimagezoom({
        //                zoomrange: [3, 10],
        //                magnifiersize: [300, 300],
        //                magnifierpos: 'right',
        //                cursorshade: true,
        //                largeimage: imagename //<-- No comma after last option!
        //            }),
        //             $("#Button2").click(function () {
        //                 var imagename = document.getElementById('imgMain').src;
        //                 imagename = imagename.replace('medium', 'large');
        //                 $('#imgMain').addimagezoom({
        //                     zoomrange: [3, 10],
        //                     magnifiersize: [300, 300],
        //                     magnifierpos: 'right',
        //                     cursorshade: true,
        //                     largeimage: imagename
        //                 })
        //             })
        //        })
        function SetTimee() {

            setInterval("CheckTimer();", 1000);
            tabberAutomatic();
        }
    </script>
    <script type="text/javascript">

        function moneyConvert(value) {
            var buf = "";
            var sBuf = "";
            var j = 0;
            value = String(value);
            if (value.indexOf(".") > 0) {
                buf = value.substring(0, value.indexOf("."));
            } else {
                buf = value;
            }
            if (buf.length % 3 != 0 && (buf.length / 3 - 1) > 0) {
                sBuf = buf.substring(0, buf.length % 3) + ",";
                buf = buf.substring(buf.length % 3);
            }
            j = buf.length;
            for (var i = 0; i < (j / 3 - 1); i++) {
                sBuf = sBuf + buf.substring(0, 3) + ",";
                buf = buf.substring(3);
            }
            sBuf = sBuf + buf;
            if (value.indexOf(".") > 0) {
                value = sBuf + value.substring(value.indexOf("."));
            }
            else {
                value = sBuf;
            }
            return value;
        }


        function checkDate(sender, args) {
        }
        function ShowImagevideo() {

            if (document.getElementById('videoid') != null && document.getElementById('videoid').src.indexOf('play') > -1) {

                document.getElementById('Hidden1').value = '0';

                if (document.getElementById('imgMain') != null) {

                    document.getElementById('Button1').click();

                }
                document.getElementById('imgMain').style.display = 'none';
                if (document.getElementById('divzioom') != null) {
                    document.getElementById('divzioom').style.display = 'none';

                }
                document.getElementById('divvedeo').style.display = '';
                document.getElementById('videoid').src = '/images/pause.png';
            }
            else {
                document.getElementById('Hidden1').value = '1';
                document.getElementById('imgMain').style.display = '';
                if (document.getElementById('divzioom') != null) {
                    document.getElementById('divzioom').style.display = '';
                }
                document.getElementById('divvedeo').style.display = 'none';
                document.getElementById('videoid').src = '/images/play.png';
                if (document.getElementById('imgMain') != null) {
                    document.getElementById('Button1').click();
                }
            }
        }
     
    </script>
    <script src="/js/General.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/iepngfix_tilebg.js"></script>
    <script type="text/javascript" src="/js/tabcontent.js"></script>
    <style type="text/css">
        img, div, input
        {
            behavior: url("/js/iepngfix.htc");
        }
        #example_6
        {
            position: relative; /* important */
            overflow: hidden; /* important */
            width: 100%; /* important */
            margin: 0;
            background: #fff;
        }
        
        #container_bd #example_6 ul li
        {
            display: block;
            float: left;
            margin: 0;
            padding: 0;
            border: none;
            background-color: #fff;
        }
        
        #container_bd #example_6 ul li img
        {
            display: block;
        }
        
        #example_6_frame
        {
            position: relative;
        }
        #container_bd #example_6_frame ul li
        {
            margin: 0 2px;
        }
        #container_bd #example_6_frame ul li img
        {
            border: 1px solid #d7d7d7;
            padding: 2px;
        }
        #container_bd #example_6_frame ul li img:hover
        {
            border: 1px solid #BA2126;
        }
    </style>
    <script type="text/javascript">
<!--
        function MM_openBrWindow(theURL, winName, features) { //v2.0
            window.open(theURL, winName, features);
        }
//-->
    </script>
    <script type="text/javascript">



        var Emailresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                Emailresults = true
            else {
                Emailresults = false
            }
            return (Emailresults)
        }
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="breadcrumbs" style="display: none;">
        <link href="/css/style.css" rel="stylesheet" type="text/css" />
        <asp:Literal ID="ltBreadcrmbs" runat="server"></asp:Literal>
        <input type="hidden" value="1" id="Hidden1" />
        <input type="hidden" value="" id="hdnEngraving" />
        <input type="hidden" value="1" id="hdnCountEngraving" />
        <input type="hidden" value="" id="hdnEngravName" runat="server" />
        <input type="hidden" value="" id="hdnEngravvalue" runat="server" />
        <input type="hidden" value="" id="hdnEngravprice" runat="server" />
        <input type="hidden" value="" id="hdnEngravQty" runat="server" />
    </div>
    <div class="item-main">
        <div class="item-main-title" style="border-bottom: 1px solid #DDDDDD; padding: 0px;">
            <h1>
                <asp:Literal ID="ltrProductName" runat="server"></asp:Literal></h1>
        </div>
        <div class="item-bg" style="border-bottom: none !important;">
            <div class="item-row1">
                <div class="item-left" style="width: 51%;">
                    <div class="item-left-row1" id="boxContainer" style="width: 80% !important; left: -95%;">
                        <div class="more-images" id="hideMoreImages" runat="server" visible="false" style="display: none;">
                            <div class="jcarousel-skin-tango3">
                                <ul id="mycarousel">
                                    <asp:Literal ID="ltBindMoreImage" runat="Server"></asp:Literal>
                                </ul>
                            </div>
                        </div>
                        <div class="item-pro">
                            <p class="item-pro-title" id="divNextBack" runat="server" style="display: none;">
                                <%--   <div runat="server" id="IPreImage">--%>
                                <%-- <a href="#" title="Previous" class="item-prev" onclick="javascript:document.getElementById('prepage').style.display = '';"
                                id="ImgPrevious" runat="server">Previous </a>--%>
                                <a href="#" style="margin-left: 15px;" title="Previous" class="item-prev" onmouseout="document.getElementById('imgPrev').style.display = 'none';"
                                    onmouseover="document.getElementById('imgPrev').style.display = '';" onclick="chkHeight();"
                                    id="ImgPrevious" runat="server">Previous<br />
                                    <img style="border: 1px solid #eee; width: 100px; height: 120px; position: absolute;
                                        display: none;" id="imgPrev" runat="server" />
                                </a>
                                <img id="imgNextImage" runat="server" />
                                <%-- </div>
                            <div id="INextImage" runat="server">--%>
                                <a href="#" title="Next" onclick="chkHeight();" id="ImgNext" onmouseout="document.getElementById('imgNextImage').style.display = 'none';"
                                    onmouseover="document.getElementById('imgNextImage').style.display = '';" class="item-next"
                                    runat="server">Next
                                    <br />
                                    <%-- <img style="border: 1px solid #eee; width: 100px; height: 100px; position: absolute;
                                    display: none;" id="imgNextImage" runat="server" />--%>
                                </a>
                                <%--</div>--%>
                            </p>
                            <div class="" style="text-align: center;" id="divMainImageDiv" runat="server">
                                <span></span>
                                <%--<asp:Literal ID="ltrMainImage" runat="server"></asp:Literal>--%>
                                <img src="/images/item-pro1.png" alt="" id="imgMain" runat="server" title="" style="vertical-align: middle;
                                    height: 309px;" />
                                <asp:Literal ID="lblTag" runat="server" Visible="false"></asp:Literal>
                                <asp:Literal ID="lblFreeEngravingImage" runat="server" Visible="false"></asp:Literal>
                                <input type="hidden" id="lblFreeEngraving" runat="server" visible="false" value='<%#Eval("IsFreeEngraving") %>' />
                                <div style="position: relative; margin-top: 50px; margin-left: 11px; display: none;"
                                    id="divvedeo">
                                    <asp:Literal ID="LtVedioParam" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="img-right" id="popUpDivnew" style="margin-top: 10px;" runat="server">
                                <a href="javascript:void(0);" onclick="ShowImagevideo();" id="anchrvideo">
                                    <img width="20" height="20" src="/images/play.png" id="videoid" border="0" />
                                </a>
                            </div>
                        </div>
                    </div>
                    <div id="container_bd" style="top: 420px; position: absolute; left: 0;">
                        <div id="example_6">
                            <div id="example_6_frame">
                                <ul>
                                    <asp:Literal ID="ltmoreimages" runat="server"></asp:Literal>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="item-left" id="boxContainer-left" style="padding: 0px !important;">
                </div>
                <div class="item-right" style="width: 48%;">
                    <div id="divscroll">
                        <div class="item-right-row1" id="divdiscription" runat="server">
                            <div class="item-right-row1-title" id="divProductDescription" style="cursor: pointer;">
                                <span style="float: left;">Product Description</span><span style="float: right;"><img
                                    src="/images/minimize.png" alt="Expand" title="Expand" id="imgProductDescription"
                                    style="float: right; margin: 0;"></span></div>
                            <div class="item-right-row1-bg" id="divProductDescription1">
                                <div style="max-height: 200px; overflow-y: auto !important;">
                                    <asp:Literal ID="ltrTabs" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="item-right-row1">
                            <div class="item-right-row1-title" id="divProductFeatures" style="cursor: pointer;">
                                <span style="float: left;">Product Features</span><span style="float: right;"><img
                                    src="/images/expand.gif" alt="Expand" title="Expand" id="imgProductFeatures"
                                    style="float: right; margin: 0;"></span></div>
                            <div class="item-right-row1-bg" id="divProductFeatures1" style="display: none;">
                                <div style="max-height: 200px; overflow-y: auto !important;">
                                    <span>Item Code:
                                        <asp:Literal ID="litModelNumber" runat="server"></asp:Literal></span>
                                    <asp:Literal ID="ltfeature" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="item-right-row1" id="divProductProperty" runat="server">
                            <div class="item-right-row1-title" id="divproperty" style="cursor: pointer;">
                                <span style="float: left;">Product Properties</span><span style="float: right;"><img
                                    src="/images/expand.gif" alt="Expand" title="Expand" id="imgPro" style="float: right;
                                    margin: 0;"></span></div>
                            <div class="item-right-row1-bg" id="divproperty1" style="display: none;">
                                <div class="properties-main" style="max-height: 200px; overflow-y: auto !important;
                                    width: 95% !important;">
                                    <div class="properties1" id="divLightControl" runat="server" visible="false" style="border: none;
                                        background: none; width: 211px;">
                                        <div class="item-detail-row31" style="border-bottom: none; width: 211px; margin-bottom: 5px;">
                                            <%-- <div class="item-detail-row3-left">
                                                Light Control :</div>--%>
                                            <div class="item-detail-row3-righ1">
                                                <img id="imgLightControl" runat="server" alt="Light Control" title="Light Control">
                                            </div>
                                        </div>
                                        <div class="item-detail-row31" id="divPrivacy" runat="server" visible="false" style="border-bottom: none;
                                            width: 211px; margin-bottom: 5px;">
                                            <%-- <div class="item-detail-row3-left">
                                                Privacy :</div>--%>
                                            <div class="item-detail-row3-righ1">
                                                <img id="imgPrivacy" runat="server" alt="Privacy" title="Privacy">
                                            </div>
                                        </div>
                                        <div style="border: 0; border-bottom: none; width: 211px;" class="item-detail-row31"
                                            id="divEfficiency" runat="server" visible="false">
                                            <%--<div class="item-detail-row3-left">
                                                Efficiency :</div>--%>
                                            <div class="item-detail-row3-righ1">
                                                <img id="imgEfficiency" runat="server" alt="Efficiency" title="Efficiency">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="item-right-row1" id="divColorOption" runat="server" visible="false">
                        <div class="item-right-row1-title">
                            Color Options:</div>
                        <div class="item-right-row1-bg">
                            <asp:Literal ID="ltrSecondoryColors" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="item-right-row3">
                        <div id="item-tb-left2">
                            <div class="item-tb-left" id="divtaball">
                                <div class="item-tb-left2-tab">
                                    <div class="tabing" id="tabber1-holder">
                                        <ul class="tabbernav">
                                            <li class="tabberactive" id="liready" runat="server" onclick="tabdisplaycart('liready','divready');">
                                                <a href="javascript:void(null);" title="READY MADE" id="areadymade" runat="server">READY
                                                    MADE</a></li><li onclick="tabdisplaycart('licustom','divcustom');" id="licustom"
                                                        runat="server"><a href="javascript:void(null);" title="MADE TO ORDER" id="amadetomeasure"
                                                            runat="server">MADE TO ORDER</a></li><li onclick="tabdisplaycart('liswatch','divswatch');"
                                                                id="liswatch" runat="server"><a href="javascript:void(null);" title="ORDER A SWATCH">
                                                                    ORDER A SWATCH</a></li></ul>
                                    </div>
                                </div>
                                <div id="content-right1" class="tabberlive">
                                    <div class="tabbertab" id="divready" runat="server">
                                        <div>
                                            <div class="readymade-detail">
                                                <asp:Literal ID="ltvariant" runat="server" Visible="false"></asp:Literal>
                                                <div class="price-detail">
                                                    <div class="price-detail-left" style="width: 100%;">
                                                        <p id="divRegularPrice" runat="server">
                                                            <%--  <tt>Regular Starting Price :</tt> <span>--%>
                                                            <asp:Literal ID="ltRegularPrice" runat="server"></asp:Literal><%--</span>--%>
                                                            <div id="spnRegularPrice" style="display: none">
                                                                <asp:Literal ID="ltRegularPriceforShippop" runat="server"></asp:Literal></div>
                                                            <input type="hidden" id="hdnpricetemp" value="" />
                                                            <input type="hidden" id="hdnpricecustomcart" runat="server" value="" />
                                                        </p>

                                                        <p id="divYourPrice" runat="server">
                                                            <tt>Your Price :</tt> <strong>
                                                                <asp:Literal ID="ltYourPrice" runat="server"></asp:Literal></strong>
                                                            <div id="spnYourPrice" style="display: none">
                                                                <asp:Literal ID="ltYourPriceforshiopop" runat="server"></asp:Literal>
                                                            </div>
                                                        </p>
                                                        <p id="divYouSave" runat="server" style="display: none;">
                                                            <tt>You Save :</tt> <span style="color: #B92127;">
                                                                <asp:Literal ID="ltYouSave" runat="server"></asp:Literal></span> &nbsp;
                                                            <%--<a style="vertical-align: middle;
                                                                display: none;" href="javascript:void(0);" onclick="OpenCenterWindow('/Pricematch.aspx?ProductId=<%=Request.QueryString["PID"] %>',700,700);">
                                                                <img title="Price Match" alt="Price Match" src="/images/price-match.png" style="vertical-align: middle;
                                                                    margin-left: 10px; margin-right: 10px;" />Price Match</a>--%></p>
                                                                     <p id="diestimatedate" style="display:none">
                                                                     </p>
                                                        <div class="price-qty">
                                                            <div id="divQuantity" runat="server" style="float: left; width: 24%; padding: 10px 0 0 0;">
                                                                <span>Quantity :</span>
                                                                <asp:TextBox ID="txtQty" runat="server" MaxLength="4" TabIndex="0" Text="1" CssClass="price-qty-input"
                                                                    onkeypress="return onKeyPressBlockNumbers1(event);" onkeyup="PriceChangeondropdown();"></asp:TextBox></div>
                                                            <%--   <div class="price-detail-right" id="divAddCart" runat="server" style="width: 74%;">--%>
                                                            <div id="divAddCart" runat="server" style="width: 100%;">
                                                                <%-- <a href="shopping-cart.html" title="Add To Cart">
                                                        <img src="images/item-add-to-cart.jpg" alt="Add To Cart" title="Add To Cart"></a>--%>
                                                                <%--   <asp:UpdatePanel runat="server" ID="uppanelAddtocart">
                                                                <ContentTemplate>--%>
                                                                <input type="hidden" runat="server" id="hdnQuantity" value="0" />
                                                                <asp:ImageButton ID="btnAddToCart" runat="server" ToolTip="ADD TO CART" ImageUrl="/images/item-add-to-cart.jpg"
                                                                    OnClick="btnAddToCart_Click" OnClientClick="AddTocartForEngra(); return checkquantity();" />
                                                                <%--  </ContentTemplate>
                                                            </asp:UpdatePanel>--%>
                                                                <div style="float: left; margin-top: 10px;">
                                                                    <asp:ImageButton ID="btnoutofStock" Style="cursor: default;" runat="server" ToolTip="OUT OF STOCK"
                                                                        OnClientClick="return false;" ImageUrl="/images/out-of-stock-item.png" Visible="false" />
                                                                    <%--   <p style="width: 120px; margin-top: 10px;" class="item-details1-right">--%>
                                                                    &nbsp; <a href="javascript:void(0);" visible="false" id="lnkAvailNotification" runat="server"
                                                                        title="CALL FOR AVAILABILITY">
                                                                        <img src="/images/call-for-avillabilty.png" alt="CALL FOR AVAILABILITY" title="CALL FOR AVAILABILITY"></a><%--</p>--%></div>
                                                            </div>
                                                        </div>
                                                        <div class="item-right-pt1" id="divtodayonly" style="height: 30px;" runat="server"
                                                            visible="false">
                                                            <div class="item-right-pt1-left">
                                                                <span style="color: #FE0000; font-size: 14px; font-weight: bold;">Today Only </span>
                                                            </div>
                                                            <div class="item-right-pt1-right" id="spnTodayPrice" runat="server">
                                                                :
                                                                <asp:Literal ID="ltTodayPrice" runat="server"></asp:Literal></div>
                                                        </div>
                                                        <div class="rating" id="divCustomerRating" runat="server" style="width: 100%; color: #848383;
                                                            font-size: 12px; float: left; text-align: left;">
                                                            <asp:Literal ID="ltRating" runat="server"></asp:Literal>
                                                        </div>
                                                    </div>
                                                    <div class="item-prt1" style="display: none;">
                                                        <div class="discounts-box">
                                                            <asp:Literal ID="ltquantitydiscount" runat="server"></asp:Literal>
                                                            <asp:Literal ID="ltQtyDiscountHideen" runat="server"></asp:Literal>
                                                        </div>
                                                    </div>
                                                    <div class="details-row" id="divDeal1" runat="server" visible="false" style="padding: 0 0 10px 0;">
                                                    </div>
                                                    <div class="item-right-pt1" id="divAvail" runat="server" visible="false">
                                                        <div class="item-right-pt1-left">
                                                            <a href="javascript:void(0);" title="In Stock" id="divInStock" runat="server" style="cursor: default;">
                                                                <span>IN STOCK</span></a> <a href="javascript:void(0);" title="In Stock" id="divOutStock"
                                                                    visible="false" runat="server" style="cursor: default;"><span>OUT OF STOCK</span></a>
                                                            <%--<asp:ImageButton ID="btnOutStock" runat="server" ToolTip="OUT OF STOCK" OnClientClick="return false;"
                                ImageUrl="/images/out of stock_Item.png" Visible="false" /></p>--%>
                                                        </div>
                                                        <div class="item-right-pt1-right">
                                                            : <a href="javascript:void(0);" title="Estimated Delivery Date" onclick="ShowModelHelpShipping('/ShippingCalculation.aspx?ProductId=<%=Request.QueryString["PID"] %>');">
                                                                Estimated Delivery Date</a></div>
                                                    </div>
                                                    <div class="item-details-box" style="margin-bottom: 10px; display: none; position: relative;
                                                        top: 0; left: 0; z-index: 10;" id="divParent" runat="server" visible="false">
                                                        <h1>
                                                            <span></span>
                                                            <select name="" class="combo-box" id="ddlIsFreeEngraving" onchange="FreeEngraving();">
                                                                <option>Yes </option>
                                                                <option selected="selected">No</option>
                                                            </select>
                                                        </h1>
                                                        <div id="divmain" style="display: none;">
                                                            <asp:Literal ID="ltrEngraving" runat="server"></asp:Literal>
                                                            <a href="#" class="font-style" title=""></a>
                                                            <p style="padding: 0px 0 0 5px;">
                                                                <strong>NOTE</strong></p>
                                                            <div class="giftwrapping-box">
                                                                <asp:Literal ID="ltrVariant" runat="server"></asp:Literal>
                                                                <div class="giftwrapping-box-row">
                                                                    <p>
                                                                        Quantity
                                                                    </p>
                                                                    <input type="text" name="txtQty1_kau_" id="txtQty1_kau_" maxlength="4" class="text-box"
                                                                        value="1" style="width: 40px; text-align: center; text-indent: 2px;" onkeypress="return onKeyPressBlockNumbers1(event);" />
                                                                </div>
                                                            </div>
                                                            <span class="item-link"><a href="javascript:void(0);" style="display: block;" onclick="checkTest();">
                                                                [ + ] </a> <a href="javascript:void(0);" style="display: none;"
                                                                    onclick="removeelement('divmain');">[ - ]</a></span>
                                                        </div>
                                                    </div>
                                                    <div class="item-right-pt1" id="divAttributes" style="width: 225px; padding-left: 5px;"
                                                        runat="server">
                                                    </div>
                                                    <div style="float: left; margin-bottom: 5px;" visible="false" id="divDeal" runat="server">
                                                        <img src="/images/banner_productdeal.png" alt="" title="" border="0" style="width: 780px;" />
                                                    </div>
                                                    <div class="item-right-pt1" visible="false" id="divOptionalAccessories" runat="server"
                                                        style="width: 100%">
                                                        <div id="OATitle" style="border: 1px solid rgb(218, 218, 218); background-color: rgb(238, 238, 238);
                                                            padding: 5px 2px 2px; height: 17px; color: #EA702F; font-weight: bold;">
                                                            Optional Accessories</div>
                                                        <div id="OAData" style="padding-top: 3px">
                                                        </div>
                                                    </div>
                                                    <asp:Literal ID="ltradySmallbanner" runat="server"></asp:Literal>
                                                </div>
                                            </div>
                                            <div class="readymade-detail" id="PCustom" style="display: none">
                                                <div class="readymade-detail-pt1">
                                                    <div class="readymade-detail-left">
                                                    </div>
                                                    <div class="readymade-detail-right">
                                                        <div class="readymade-detail-left" style="width: 55%;">
                                                            Width :&nbsp;&nbsp;
                                                            <asp:TextBox ID="txtWidth" runat="server" MaxLength="4" TabIndex="0" Text="1" CssClass="color_field"
                                                                onkeypress="return onKeyPressBlockNumbers1(event);"></asp:TextBox>&nbsp;&nbsp;Length
                                                            :&nbsp;&nbsp;
                                                            <asp:TextBox ID="txtLength" runat="server" MaxLength="4" TabIndex="0" Text="1" CssClass="color_field"
                                                                onkeypress="return onKeyPressBlockNumbers1(event);"></asp:TextBox></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tabbertab" id="divcustom" runat="server" style="width: 100% !important;
                                        display: none;">
                                        <h6>
                                            <a title="MADE TO ORDER">MADE TO ORDER</a></h6>
                                        <div>
                                            <asp:Literal ID="ltmadevariant" runat="server"></asp:Literal>
                                            <div class="readymade-detail">
                                                <div class="readymade-detail-pt1" style="width: 100% !important;">
                                                    <div class="readymade-detail-left">
                                                        Select Style:</div>
                                                    <div class="readymade-detail-right">
                                                        <asp:DropDownList ID="ddlcustomstyle" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                            <asp:ListItem Selected="True" Value="">Select One</asp:ListItem>
                                                            <asp:ListItem Value="Pole Pocket">Pole Pocket</asp:ListItem>
                                                            <asp:ListItem Value="French">French</asp:ListItem>
                                                            <asp:ListItem Value="Parisian">Parisian</asp:ListItem>
                                                            <asp:ListItem Value="Inverted">Inverted</asp:ListItem>
                                                            <asp:ListItem Value="Goblet">Goblet</asp:ListItem>
                                                            <asp:ListItem Value="Grommet">Grommet</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span><a title="Learn More" target="_blank" href="/pleatguide">Learn More</a><%--<a title="Learn More" onclick="variantDetail('divMakeOrderStyle');" href="javascript:void(0);">--%></span>
                                                    </div>
                                                </div>
                                                <div class="readymade-detail-pt1" style="width: 100% !important;">
                                                    <div class="readymade-detail-left">
                                                        Select Width:</div>
                                                    <div class="readymade-detail-right">
                                                        <asp:DropDownList ID="ddlcustomwidth" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                            <asp:ListItem Selected="True">Width</asp:ListItem>
                                                            <asp:ListItem Value="25">25"</asp:ListItem>
                                                            <asp:ListItem Value="26">26"</asp:ListItem>
                                                            <asp:ListItem Value="27">27"</asp:ListItem>
                                                            <asp:ListItem Value="28">28"</asp:ListItem>
                                                            <asp:ListItem Value="29">29"</asp:ListItem>
                                                            <asp:ListItem Value="30">30"</asp:ListItem>
                                                            <asp:ListItem Value="31">31"</asp:ListItem>
                                                            <asp:ListItem Value="32">32"</asp:ListItem>
                                                            <asp:ListItem Value="33">33"</asp:ListItem>
                                                            <asp:ListItem Value="34">34"</asp:ListItem>
                                                            <asp:ListItem Value="35">35"</asp:ListItem>
                                                            <asp:ListItem Value="36">36"</asp:ListItem>
                                                            <asp:ListItem Value="37">37"</asp:ListItem>
                                                            <asp:ListItem Value="38">38"</asp:ListItem>
                                                            <asp:ListItem Value="39">39"</asp:ListItem>
                                                            <asp:ListItem Value="40">40"</asp:ListItem>
                                                            <asp:ListItem Value="41">41"</asp:ListItem>
                                                            <asp:ListItem Value="42">42"</asp:ListItem>
                                                            <asp:ListItem Value="43">43"</asp:ListItem>
                                                            <asp:ListItem Value="44">44"</asp:ListItem>
                                                            <asp:ListItem Value="45">45"</asp:ListItem>
                                                            <asp:ListItem Value="46">46"</asp:ListItem>
                                                            <asp:ListItem Value="47">47"</asp:ListItem>
                                                            <asp:ListItem Value="48">48"</asp:ListItem>
                                                            <asp:ListItem Value="49">49"</asp:ListItem>
                                                            <asp:ListItem Value="50">50"</asp:ListItem>
                                                            <asp:ListItem Value="51">51"</asp:ListItem>
                                                            <asp:ListItem Value="52">52"</asp:ListItem>
                                                            <asp:ListItem Value="53">53"</asp:ListItem>
                                                            <asp:ListItem Value="54">54"</asp:ListItem>
                                                            <asp:ListItem Value="55">55"</asp:ListItem>
                                                            <asp:ListItem Value="56">56"</asp:ListItem>
                                                            <asp:ListItem Value="57">57"</asp:ListItem>
                                                            <asp:ListItem Value="58">58"</asp:ListItem>
                                                            <asp:ListItem Value="59">59"</asp:ListItem>
                                                            <asp:ListItem Value="60">60"</asp:ListItem>
                                                            <asp:ListItem Value="61">61"</asp:ListItem>
                                                            <asp:ListItem Value="62">62"</asp:ListItem>
                                                            <asp:ListItem Value="63">63"</asp:ListItem>
                                                            <asp:ListItem Value="64">64"</asp:ListItem>
                                                            <asp:ListItem Value="65">65"</asp:ListItem>
                                                            <asp:ListItem Value="66">66"</asp:ListItem>
                                                            <asp:ListItem Value="67">67"</asp:ListItem>
                                                            <asp:ListItem Value="68">68"</asp:ListItem>
                                                            <asp:ListItem Value="69">69"</asp:ListItem>
                                                            <asp:ListItem Value="70">70"</asp:ListItem>
                                                            <asp:ListItem Value="71">71"</asp:ListItem>
                                                            <asp:ListItem Value="72">72"</asp:ListItem>
                                                            <asp:ListItem Value="73">73"</asp:ListItem>
                                                            <asp:ListItem Value="74">74"</asp:ListItem>
                                                            <asp:ListItem Value="75">75"</asp:ListItem>
                                                            <asp:ListItem Value="76">76"</asp:ListItem>
                                                            <asp:ListItem Value="77">77"</asp:ListItem>
                                                            <asp:ListItem Value="78">78"</asp:ListItem>
                                                            <asp:ListItem Value="79">79"</asp:ListItem>
                                                            <asp:ListItem Value="80">80"</asp:ListItem>
                                                            <asp:ListItem Value="81">81"</asp:ListItem>
                                                            <asp:ListItem Value="82">82"</asp:ListItem>
                                                            <asp:ListItem Value="83">83"</asp:ListItem>
                                                            <asp:ListItem Value="84">84"</asp:ListItem>
                                                            <asp:ListItem Value="85">85"</asp:ListItem>
                                                            <asp:ListItem Value="86">86"</asp:ListItem>
                                                            <asp:ListItem Value="87">87"</asp:ListItem>
                                                            <asp:ListItem Value="88">88"</asp:ListItem>
                                                            <asp:ListItem Value="89">89"</asp:ListItem>
                                                            <asp:ListItem Value="90">90"</asp:ListItem>
                                                            <asp:ListItem Value="91">91"</asp:ListItem>
                                                            <asp:ListItem Value="92">92"</asp:ListItem>
                                                            <asp:ListItem Value="93">93"</asp:ListItem>
                                                            <asp:ListItem Value="94">94"</asp:ListItem>
                                                            <asp:ListItem Value="95">95"</asp:ListItem>
                                                            <asp:ListItem Value="96">96"</asp:ListItem>
                                                            <asp:ListItem Value="97">97"</asp:ListItem>
                                                            <asp:ListItem Value="98">98"</asp:ListItem>
                                                            <asp:ListItem Value="99">99"</asp:ListItem>
                                                            <asp:ListItem Value="100">100"</asp:ListItem>
                                                            <asp:ListItem Value="101">101"</asp:ListItem>
                                                            <asp:ListItem Value="102">102"</asp:ListItem>
                                                            <asp:ListItem Value="103">103"</asp:ListItem>
                                                            <asp:ListItem Value="104">104"</asp:ListItem>
                                                            <asp:ListItem Value="105">105"</asp:ListItem>
                                                            <asp:ListItem Value="106">106"</asp:ListItem>
                                                            <asp:ListItem Value="107">107"</asp:ListItem>
                                                            <asp:ListItem Value="108">108"</asp:ListItem>
                                                            <asp:ListItem Value="109">109"</asp:ListItem>
                                                            <asp:ListItem Value="110">110"</asp:ListItem>
                                                            <asp:ListItem Value="111">111"</asp:ListItem>
                                                            <asp:ListItem Value="112">112"</asp:ListItem>
                                                            <asp:ListItem Value="113">113"</asp:ListItem>
                                                            <asp:ListItem Value="114">114"</asp:ListItem>
                                                            <asp:ListItem Value="115">115"</asp:ListItem>
                                                            <asp:ListItem Value="116">116"</asp:ListItem>
                                                            <asp:ListItem Value="117">117"</asp:ListItem>
                                                            <asp:ListItem Value="118">118"</asp:ListItem>
                                                            <asp:ListItem Value="119">119"</asp:ListItem>
                                                            <asp:ListItem Value="120">120"</asp:ListItem>
                                                            <asp:ListItem Value="121">121"</asp:ListItem>
                                                            <asp:ListItem Value="122">122"</asp:ListItem>
                                                            <asp:ListItem Value="123">123"</asp:ListItem>
                                                            <asp:ListItem Value="124">124"</asp:ListItem>
                                                            <asp:ListItem Value="125">125"</asp:ListItem>
                                                            <asp:ListItem Value="126">126"</asp:ListItem>
                                                            <asp:ListItem Value="127">127"</asp:ListItem>
                                                            <asp:ListItem Value="128">128"</asp:ListItem>
                                                            <asp:ListItem Value="129">129"</asp:ListItem>
                                                            <asp:ListItem Value="130">130"</asp:ListItem>
                                                            <asp:ListItem Value="131">131"</asp:ListItem>
                                                            <asp:ListItem Value="132">132"</asp:ListItem>
                                                            <asp:ListItem Value="133">133"</asp:ListItem>
                                                            <asp:ListItem Value="134">134"</asp:ListItem>
                                                            <asp:ListItem Value="135">135"</asp:ListItem>
                                                            <asp:ListItem Value="136">136"</asp:ListItem>
                                                            <asp:ListItem Value="137">137"</asp:ListItem>
                                                            <asp:ListItem Value="138">138"</asp:ListItem>
                                                            <asp:ListItem Value="139">139"</asp:ListItem>
                                                            <asp:ListItem Value="140">140"</asp:ListItem>
                                                            <asp:ListItem Value="141">141"</asp:ListItem>
                                                            <asp:ListItem Value="142">142"</asp:ListItem>
                                                            <asp:ListItem Value="143">143"</asp:ListItem>
                                                            <asp:ListItem Value="144">144"</asp:ListItem>
                                                            <asp:ListItem Value="145">145"</asp:ListItem>
                                                            <asp:ListItem Value="146">146"</asp:ListItem>
                                                            <asp:ListItem Value="147">147"</asp:ListItem>
                                                            <asp:ListItem Value="148">148"</asp:ListItem>
                                                            <asp:ListItem Value="149">149"</asp:ListItem>
                                                            <asp:ListItem Value="150">150"</asp:ListItem>
                                                            <asp:ListItem Value="151">151"</asp:ListItem>
                                                            <asp:ListItem Value="152">152"</asp:ListItem>
                                                            <asp:ListItem Value="153">153"</asp:ListItem>
                                                            <asp:ListItem Value="154">154"</asp:ListItem>
                                                            <asp:ListItem Value="155">155"</asp:ListItem>
                                                            <asp:ListItem Value="156">156"</asp:ListItem>
                                                            <asp:ListItem Value="157">157"</asp:ListItem>
                                                            <asp:ListItem Value="158">158"</asp:ListItem>
                                                            <asp:ListItem Value="159">159"</asp:ListItem>
                                                            <asp:ListItem Value="160">160"</asp:ListItem>
                                                            <asp:ListItem Value="161">161"</asp:ListItem>
                                                            <asp:ListItem Value="162">162"</asp:ListItem>
                                                            <asp:ListItem Value="163">163"</asp:ListItem>
                                                            <asp:ListItem Value="164">164"</asp:ListItem>
                                                            <asp:ListItem Value="165">165"</asp:ListItem>
                                                            <asp:ListItem Value="166">166"</asp:ListItem>
                                                            <asp:ListItem Value="167">167"</asp:ListItem>
                                                            <asp:ListItem Value="168">168"</asp:ListItem>
                                                            <asp:ListItem Value="169">169"</asp:ListItem>
                                                            <asp:ListItem Value="170">170"</asp:ListItem>
                                                            <asp:ListItem Value="171">171"</asp:ListItem>
                                                            <asp:ListItem Value="172">172"</asp:ListItem>
                                                            <asp:ListItem Value="173">173"</asp:ListItem>
                                                            <asp:ListItem Value="174">174"</asp:ListItem>
                                                            <asp:ListItem Value="175">175"</asp:ListItem>
                                                            <asp:ListItem Value="176">176"</asp:ListItem>
                                                            <asp:ListItem Value="177">177"</asp:ListItem>
                                                            <asp:ListItem Value="178">178"</asp:ListItem>
                                                            <asp:ListItem Value="179">179"</asp:ListItem>
                                                            <asp:ListItem Value="180">180"</asp:ListItem>
                                                            <asp:ListItem Value="181">181"</asp:ListItem>
                                                            <asp:ListItem Value="182">182"</asp:ListItem>
                                                            <asp:ListItem Value="183">183"</asp:ListItem>
                                                            <asp:ListItem Value="184">184"</asp:ListItem>
                                                            <asp:ListItem Value="185">185"</asp:ListItem>
                                                            <asp:ListItem Value="186">186"</asp:ListItem>
                                                            <asp:ListItem Value="187">187"</asp:ListItem>
                                                            <asp:ListItem Value="188">188"</asp:ListItem>
                                                            <asp:ListItem Value="189">189"</asp:ListItem>
                                                            <asp:ListItem Value="190">190"</asp:ListItem>
                                                            <asp:ListItem Value="191">191"</asp:ListItem>
                                                            <asp:ListItem Value="192">192"</asp:ListItem>
                                                            <asp:ListItem Value="193">193"</asp:ListItem>
                                                            <asp:ListItem Value="194">194"</asp:ListItem>
                                                            <asp:ListItem Value="195">195"</asp:ListItem>
                                                            <asp:ListItem Value="196">196"</asp:ListItem>
                                                            <asp:ListItem Value="197">197"</asp:ListItem>
                                                            <asp:ListItem Value="198">198"</asp:ListItem>
                                                            <asp:ListItem Value="199">199"</asp:ListItem>
                                                            <asp:ListItem Value="200">200"</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span><a title="Learn More" onclick="variantDetail('divMakeOrderWidth');" href="javascript:void(0);">
                                                            Learn More</a></span>
                                                    </div>
                                                </div>
                                                <div class="readymade-detail-pt1" style="width: 100% !important;">
                                                    <div class="readymade-detail-left">
                                                        Select Length:</div>
                                                    <div class="readymade-detail-right">
                                                        <asp:DropDownList ID="ddlcustomlength" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                            <asp:ListItem Selected="True" Value="">Length</asp:ListItem>
                                                            <asp:ListItem Value="45">45"</asp:ListItem>
                                                            <asp:ListItem Value="46">46"</asp:ListItem>
                                                            <asp:ListItem Value="47">47"</asp:ListItem>
                                                            <asp:ListItem Value="48">48"</asp:ListItem>
                                                            <asp:ListItem Value="49">49"</asp:ListItem>
                                                            <asp:ListItem Value="50">50"</asp:ListItem>
                                                            <asp:ListItem Value="51">51"</asp:ListItem>
                                                            <asp:ListItem Value="52">52"</asp:ListItem>
                                                            <asp:ListItem Value="53">53"</asp:ListItem>
                                                            <asp:ListItem Value="54">54"</asp:ListItem>
                                                            <asp:ListItem Value="55">55"</asp:ListItem>
                                                            <asp:ListItem Value="56">56"</asp:ListItem>
                                                            <asp:ListItem Value="57">57"</asp:ListItem>
                                                            <asp:ListItem Value="58">58"</asp:ListItem>
                                                            <asp:ListItem Value="59">59"</asp:ListItem>
                                                            <asp:ListItem Value="60">60"</asp:ListItem>
                                                            <asp:ListItem Value="61">61"</asp:ListItem>
                                                            <asp:ListItem Value="62">62"</asp:ListItem>
                                                            <asp:ListItem Value="63">63"</asp:ListItem>
                                                            <asp:ListItem Value="64">64"</asp:ListItem>
                                                            <asp:ListItem Value="65">65"</asp:ListItem>
                                                            <asp:ListItem Value="66">66"</asp:ListItem>
                                                            <asp:ListItem Value="67">67"</asp:ListItem>
                                                            <asp:ListItem Value="68">68"</asp:ListItem>
                                                            <asp:ListItem Value="69">69"</asp:ListItem>
                                                            <asp:ListItem Value="70">70"</asp:ListItem>
                                                            <asp:ListItem Value="71">71"</asp:ListItem>
                                                            <asp:ListItem Value="72">72"</asp:ListItem>
                                                            <asp:ListItem Value="73">73"</asp:ListItem>
                                                            <asp:ListItem Value="74">74"</asp:ListItem>
                                                            <asp:ListItem Value="75">75"</asp:ListItem>
                                                            <asp:ListItem Value="76">76"</asp:ListItem>
                                                            <asp:ListItem Value="77">77"</asp:ListItem>
                                                            <asp:ListItem Value="78">78"</asp:ListItem>
                                                            <asp:ListItem Value="79">79"</asp:ListItem>
                                                            <asp:ListItem Value="80">80"</asp:ListItem>
                                                            <asp:ListItem Value="81">81"</asp:ListItem>
                                                            <asp:ListItem Value="82">82"</asp:ListItem>
                                                            <asp:ListItem Value="83">83"</asp:ListItem>
                                                            <asp:ListItem Value="84">84"</asp:ListItem>
                                                            <asp:ListItem Value="85">85"</asp:ListItem>
                                                            <asp:ListItem Value="86">86"</asp:ListItem>
                                                            <asp:ListItem Value="87">87"</asp:ListItem>
                                                            <asp:ListItem Value="88">88"</asp:ListItem>
                                                            <asp:ListItem Value="89">89"</asp:ListItem>
                                                            <asp:ListItem Value="90">90"</asp:ListItem>
                                                            <asp:ListItem Value="91">91"</asp:ListItem>
                                                            <asp:ListItem Value="92">92"</asp:ListItem>
                                                            <asp:ListItem Value="93">93"</asp:ListItem>
                                                            <asp:ListItem Value="94">94"</asp:ListItem>
                                                            <asp:ListItem Value="95">95"</asp:ListItem>
                                                            <asp:ListItem Value="96">96"</asp:ListItem>
                                                            <asp:ListItem Value="97">97"</asp:ListItem>
                                                            <asp:ListItem Value="98">98"</asp:ListItem>
                                                            <asp:ListItem Value="99">99"</asp:ListItem>
                                                            <asp:ListItem Value="100">100"</asp:ListItem>
                                                            <asp:ListItem Value="101">101"</asp:ListItem>
                                                            <asp:ListItem Value="102">102"</asp:ListItem>
                                                            <asp:ListItem Value="103">103"</asp:ListItem>
                                                            <asp:ListItem Value="104">104"</asp:ListItem>
                                                            <asp:ListItem Value="105">105"</asp:ListItem>
                                                            <asp:ListItem Value="106">106"</asp:ListItem>
                                                            <asp:ListItem Value="107">107"</asp:ListItem>
                                                            <asp:ListItem Value="108">108"</asp:ListItem>
                                                            <asp:ListItem Value="109">109"</asp:ListItem>
                                                            <asp:ListItem Value="110">110"</asp:ListItem>
                                                            <asp:ListItem Value="111">111"</asp:ListItem>
                                                            <asp:ListItem Value="112">112"</asp:ListItem>
                                                            <asp:ListItem Value="113">113"</asp:ListItem>
                                                            <asp:ListItem Value="114">114"</asp:ListItem>
                                                            <asp:ListItem Value="115">115"</asp:ListItem>
                                                            <asp:ListItem Value="116">116"</asp:ListItem>
                                                            <asp:ListItem Value="117">117"</asp:ListItem>
                                                            <asp:ListItem Value="118">118"</asp:ListItem>
                                                            <asp:ListItem Value="119">119"</asp:ListItem>
                                                            <asp:ListItem Value="120">120"</asp:ListItem>
                                                            <asp:ListItem Value="121">121"</asp:ListItem>
                                                            <asp:ListItem Value="122">122"</asp:ListItem>
                                                            <asp:ListItem Value="123">123"</asp:ListItem>
                                                            <asp:ListItem Value="124">124"</asp:ListItem>
                                                            <asp:ListItem Value="125">125"</asp:ListItem>
                                                            <asp:ListItem Value="126">126"</asp:ListItem>
                                                            <asp:ListItem Value="127">127"</asp:ListItem>
                                                            <asp:ListItem Value="128">128"</asp:ListItem>
                                                            <asp:ListItem Value="129">129"</asp:ListItem>
                                                            <asp:ListItem Value="130">130"</asp:ListItem>
                                                            <asp:ListItem Value="131">131"</asp:ListItem>
                                                            <asp:ListItem Value="132">132"</asp:ListItem>
                                                            <asp:ListItem Value="133">133"</asp:ListItem>
                                                            <asp:ListItem Value="134">134"</asp:ListItem>
                                                            <asp:ListItem Value="135">135"</asp:ListItem>
                                                            <asp:ListItem Value="136">136"</asp:ListItem>
                                                            <asp:ListItem Value="137">137"</asp:ListItem>
                                                            <asp:ListItem Value="138">138"</asp:ListItem>
                                                            <asp:ListItem Value="139">139"</asp:ListItem>
                                                            <asp:ListItem Value="140">140"</asp:ListItem>
                                                            <asp:ListItem Value="141">141"</asp:ListItem>
                                                            <asp:ListItem Value="142">142"</asp:ListItem>
                                                            <asp:ListItem Value="143">143"</asp:ListItem>
                                                            <asp:ListItem Value="144">144"</asp:ListItem>
                                                            <asp:ListItem Value="145">145"</asp:ListItem>
                                                            <asp:ListItem Value="146">146"</asp:ListItem>
                                                            <asp:ListItem Value="147">147"</asp:ListItem>
                                                            <asp:ListItem Value="148">148"</asp:ListItem>
                                                            <asp:ListItem Value="149">149"</asp:ListItem>
                                                            <asp:ListItem Value="150">150"</asp:ListItem>
                                                            <asp:ListItem Value="151">151"</asp:ListItem>
                                                            <asp:ListItem Value="152">152"</asp:ListItem>
                                                            <asp:ListItem Value="153">153"</asp:ListItem>
                                                            <asp:ListItem Value="154">154"</asp:ListItem>
                                                            <asp:ListItem Value="155">155"</asp:ListItem>
                                                            <asp:ListItem Value="156">156"</asp:ListItem>
                                                            <asp:ListItem Value="157">157"</asp:ListItem>
                                                            <asp:ListItem Value="158">158"</asp:ListItem>
                                                            <asp:ListItem Value="159">159"</asp:ListItem>
                                                            <asp:ListItem Value="160">160"</asp:ListItem>
                                                            <asp:ListItem Value="161">161"</asp:ListItem>
                                                            <asp:ListItem Value="162">162"</asp:ListItem>
                                                            <asp:ListItem Value="163">163"</asp:ListItem>
                                                            <asp:ListItem Value="164">164"</asp:ListItem>
                                                            <asp:ListItem Value="165">165"</asp:ListItem>
                                                            <asp:ListItem Value="166">166"</asp:ListItem>
                                                            <asp:ListItem Value="167">167"</asp:ListItem>
                                                            <asp:ListItem Value="168">168"</asp:ListItem>
                                                            <asp:ListItem Value="169">169"</asp:ListItem>
                                                            <asp:ListItem Value="170">170"</asp:ListItem>
                                                            <asp:ListItem Value="171">171"</asp:ListItem>
                                                            <asp:ListItem Value="172">172"</asp:ListItem>
                                                            <asp:ListItem Value="173">173"</asp:ListItem>
                                                            <asp:ListItem Value="174">174"</asp:ListItem>
                                                            <asp:ListItem Value="175">175"</asp:ListItem>
                                                            <asp:ListItem Value="176">176"</asp:ListItem>
                                                            <asp:ListItem Value="177">177"</asp:ListItem>
                                                            <asp:ListItem Value="178">178"</asp:ListItem>
                                                            <asp:ListItem Value="179">179"</asp:ListItem>
                                                            <asp:ListItem Value="180">180"</asp:ListItem>
                                                            <asp:ListItem Value="181">181"</asp:ListItem>
                                                            <asp:ListItem Value="182">182"</asp:ListItem>
                                                            <asp:ListItem Value="183">183"</asp:ListItem>
                                                            <asp:ListItem Value="184">184"</asp:ListItem>
                                                            <asp:ListItem Value="185">185"</asp:ListItem>
                                                            <asp:ListItem Value="186">186"</asp:ListItem>
                                                            <asp:ListItem Value="187">187"</asp:ListItem>
                                                            <asp:ListItem Value="188">188"</asp:ListItem>
                                                            <asp:ListItem Value="189">189"</asp:ListItem>
                                                            <asp:ListItem Value="190">190"</asp:ListItem>
                                                            <asp:ListItem Value="191">191"</asp:ListItem>
                                                            <asp:ListItem Value="192">192"</asp:ListItem>
                                                            <asp:ListItem Value="193">193"</asp:ListItem>
                                                            <asp:ListItem Value="194">194"</asp:ListItem>
                                                            <asp:ListItem Value="195">195"</asp:ListItem>
                                                            <asp:ListItem Value="196">196"</asp:ListItem>
                                                            <asp:ListItem Value="197">197"</asp:ListItem>
                                                            <asp:ListItem Value="198">198"</asp:ListItem>
                                                            <asp:ListItem Value="199">199"</asp:ListItem>
                                                            <asp:ListItem Value="200">200"</asp:ListItem>
                                                            <asp:ListItem Value="201">201"</asp:ListItem>
                                                            <asp:ListItem Value="202">202"</asp:ListItem>
                                                            <asp:ListItem Value="203">203"</asp:ListItem>
                                                            <asp:ListItem Value="204">204"</asp:ListItem>
                                                            <asp:ListItem Value="205">205"</asp:ListItem>
                                                            <asp:ListItem Value="206">206"</asp:ListItem>
                                                            <asp:ListItem Value="207">207"</asp:ListItem>
                                                            <asp:ListItem Value="208">208"</asp:ListItem>
                                                            <asp:ListItem Value="209">209"</asp:ListItem>
                                                            <asp:ListItem Value="210">210"</asp:ListItem>
                                                            <asp:ListItem Value="211">211"</asp:ListItem>
                                                            <asp:ListItem Value="212">212"</asp:ListItem>
                                                            <asp:ListItem Value="213">213"</asp:ListItem>
                                                            <asp:ListItem Value="214">214"</asp:ListItem>
                                                            <asp:ListItem Value="215">215"</asp:ListItem>
                                                            <asp:ListItem Value="216">216"</asp:ListItem>
                                                            <asp:ListItem Value="217">217"</asp:ListItem>
                                                            <asp:ListItem Value="218">218"</asp:ListItem>
                                                            <asp:ListItem Value="219">219"</asp:ListItem>
                                                            <asp:ListItem Value="220">220"</asp:ListItem>
                                                            <asp:ListItem Value="221">221"</asp:ListItem>
                                                            <asp:ListItem Value="222">222"</asp:ListItem>
                                                            <asp:ListItem Value="223">223"</asp:ListItem>
                                                            <asp:ListItem Value="224">224"</asp:ListItem>
                                                            <asp:ListItem Value="225">225"</asp:ListItem>
                                                            <asp:ListItem Value="226">226"</asp:ListItem>
                                                            <asp:ListItem Value="227">227"</asp:ListItem>
                                                            <asp:ListItem Value="228">228"</asp:ListItem>
                                                            <asp:ListItem Value="229">229"</asp:ListItem>
                                                            <asp:ListItem Value="230">230"</asp:ListItem>
                                                            <asp:ListItem Value="231">231"</asp:ListItem>
                                                            <asp:ListItem Value="232">232"</asp:ListItem>
                                                            <asp:ListItem Value="233">233"</asp:ListItem>
                                                            <asp:ListItem Value="234">234"</asp:ListItem>
                                                            <asp:ListItem Value="235">235"</asp:ListItem>
                                                            <asp:ListItem Value="236">236"</asp:ListItem>
                                                            <asp:ListItem Value="237">237"</asp:ListItem>
                                                            <asp:ListItem Value="238">238"</asp:ListItem>
                                                            <asp:ListItem Value="239">239"</asp:ListItem>
                                                            <asp:ListItem Value="240">240"</asp:ListItem>
                                                            <asp:ListItem Value="241">241"</asp:ListItem>
                                                            <asp:ListItem Value="242">242"</asp:ListItem>
                                                            <asp:ListItem Value="243">243"</asp:ListItem>
                                                            <asp:ListItem Value="244">244"</asp:ListItem>
                                                            <asp:ListItem Value="245">245"</asp:ListItem>
                                                            <asp:ListItem Value="246">246"</asp:ListItem>
                                                            <asp:ListItem Value="247">247"</asp:ListItem>
                                                            <asp:ListItem Value="248">248"</asp:ListItem>
                                                            <asp:ListItem Value="249">249"</asp:ListItem>
                                                            <asp:ListItem Value="250">250"</asp:ListItem>
                                                            <asp:ListItem Value="251">251"</asp:ListItem>
                                                            <asp:ListItem Value="252">252"</asp:ListItem>
                                                            <asp:ListItem Value="253">253"</asp:ListItem>
                                                            <asp:ListItem Value="254">254"</asp:ListItem>
                                                            <asp:ListItem Value="255">255"</asp:ListItem>
                                                            <asp:ListItem Value="256">256"</asp:ListItem>
                                                            <asp:ListItem Value="257">257"</asp:ListItem>
                                                            <asp:ListItem Value="258">258"</asp:ListItem>
                                                            <asp:ListItem Value="259">259"</asp:ListItem>
                                                            <asp:ListItem Value="260">260"</asp:ListItem>
                                                            <asp:ListItem Value="261">261"</asp:ListItem>
                                                            <asp:ListItem Value="262">262"</asp:ListItem>
                                                            <asp:ListItem Value="263">263"</asp:ListItem>
                                                            <asp:ListItem Value="264">264"</asp:ListItem>
                                                            <asp:ListItem Value="265">265"</asp:ListItem>
                                                            <asp:ListItem Value="266">266"</asp:ListItem>
                                                            <asp:ListItem Value="267">267"</asp:ListItem>
                                                            <asp:ListItem Value="268">268"</asp:ListItem>
                                                            <asp:ListItem Value="269">269"</asp:ListItem>
                                                            <asp:ListItem Value="270">270"</asp:ListItem>
                                                            <asp:ListItem Value="271">271"</asp:ListItem>
                                                            <asp:ListItem Value="272">272"</asp:ListItem>
                                                            <asp:ListItem Value="273">273"</asp:ListItem>
                                                            <asp:ListItem Value="274">274"</asp:ListItem>
                                                            <asp:ListItem Value="275">275"</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span><a title="Learn More" onclick="variantDetail('divMakeOrderLength');" href="javascript:void(0);">
                                                            Learn More</a></span>
                                                    </div>
                                                </div>
                                                <div class="readymade-detail-pt1" style="width: 100% !important;">
                                                    <div class="readymade-detail-left">
                                                        Select Options:</div>
                                                    <div class="readymade-detail-right">
                                                        <asp:DropDownList ID="ddlcustomoptin" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                            <asp:ListItem Value="" Selected="True">Options</asp:ListItem>
                                                            <asp:ListItem Value="Lined">Lined</asp:ListItem>
                                                            <asp:ListItem Value="Lined &amp; Interlined">Lined &amp; Interlined</asp:ListItem>
                                                            <asp:ListItem Value="Blackout Lining">Blackout Lining</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span><a title="Learn More" onclick="variantDetail('divMakeOrderOptions');" href="javascript:void(0);">
                                                            Learn More</a></span>
                                                    </div>
                                                </div>
                                                <div class="readymade-detail-pt1" style="width: 100% !important;">
                                                    <div class="readymade-detail-left">
                                                        Quantity (Panels):</div>
                                                    <div class="readymade-detail-right">
                                                        <asp:DropDownList ID="dlcustomqty" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                            <asp:ListItem Value="" >Quantity</asp:ListItem>
                                                            <asp:ListItem Value="1" Selected="True">1</asp:ListItem>
                                                            <asp:ListItem Value="2">2</asp:ListItem>
                                                            <asp:ListItem Value="3">3</asp:ListItem>
                                                            <asp:ListItem Value="4">4</asp:ListItem>
                                                            <asp:ListItem Value="5">5</asp:ListItem>
                                                            <asp:ListItem Value="6">6</asp:ListItem>
                                                            <asp:ListItem Value="7">7</asp:ListItem>
                                                            <asp:ListItem Value="8">8</asp:ListItem>
                                                            <asp:ListItem Value="9">9</asp:ListItem>
                                                            <asp:ListItem Value="10">10</asp:ListItem>
                                                            <asp:ListItem Value="11">11</asp:ListItem>
                                                            <asp:ListItem Value="12">12</asp:ListItem>
                                                            <asp:ListItem Value="13">13</asp:ListItem>
                                                            <asp:ListItem Value="14">14</asp:ListItem>
                                                            <asp:ListItem Value="15">15</asp:ListItem>
                                                            <asp:ListItem Value="16">16</asp:ListItem>
                                                            <asp:ListItem Value="17">17</asp:ListItem>
                                                            <asp:ListItem Value="18">18</asp:ListItem>
                                                            <asp:ListItem Value="19">19</asp:ListItem>
                                                            <asp:ListItem Value="20">20</asp:ListItem>
                                                        </asp:DropDownList>
                                                       <%-- <span><a title="Learn More" onclick="variantDetail('divMakeOrderQuantity');" href="javascript:void(0);">
                                                            Learn More</a></span>--%>
                                                    </div>
                                                </div>
                                                  <div class="readymade-detail-pt1" id="divmeadetomeasure" style="display:none;">
                                                  </div>
                                                 
                                                                 <%--    </p>--%>
                                                <div class="readymade-detail-pt1" style="width: 100% !important;">
                                                    <div class="price-detail-left" style="width: 380px;">
                                                        <p id="divcustomprice" style="width: 54%; margin-top: 14px;">
                                                            <asp:Literal ID="ltcustomPrice" runat="server"></asp:Literal>
                                                            <input type="hidden" id="hdncustomprice" runat="server" value="0" />
                                                        </p>
                                                        <asp:ImageButton CssClass="price-detail-right" ID="btnAddTocartMade" runat="server"
                                                            ToolTip="ADD TO CART" ImageUrl="/images/item-add-to-cart.jpg" />
                                                    </div>
                                                </div>
                                                <asp:Literal ID="ltmadeSmallbanner" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tabbertab" id="divswatch" runat="server" style="display: none;">
                                        <div class="order-swatch-item">
                                            <div class="order-swatch-item-content">
                                                <div class="order-swatch-item-left">
                                                    <asp:Literal ID="ltswatchSmallbanner" runat="server"></asp:Literal>
                                                    <a href="javascript:void(0);" id="aswatchurl" runat="server">
                                                        <img src="" id="imgswatchproduct" runat="server" /></a>
                                                </div>
                                                <div class="order-swatch-item-right">
                                                    <p>
                                                        <asp:Literal ID="ltOrderswatch" runat="server"></asp:Literal>
                                                    </p>
                                                    <div class="order-swatch-item-right-1">
                                                        <div class="order-swatch-item-rightrow-1">
                                                            <asp:Literal ID="ltSwatchPrice" runat="server"></asp:Literal>
                                                            <input type="hidden" id="hdnswatchprice" runat="server" value="0" />
                                                        </div>
                                                    </div>
                                                    <div class="order-swatch-item-rightrow-2">
                                                        <span>Quantity:</span>
                                                        <asp:TextBox ID="txtSwatchqty" CssClass="order-swatch-input" onkeypress="return onKeyPressBlockNumbersOnly(event);"
                                                            runat="server" Text="1" MaxLength="4" Style="text-align: center;"></asp:TextBox>
                                                    </div>
                                                    <div class="order-swatch-item-rightrow-3">
                                                        <asp:ImageButton ID="btnAddTocartSwatch" runat="server" ToolTip="ADD TO CART" ImageUrl="/images/item-add-to-cart.jpg" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="readymade-detail" id="divPriceQuote" runat="server" visible="false">
                                        <div class="price-quote">
                                            <div class="price-quote-small">
                                                <img src="/images//size-title.png"></div>
                                            <div class="price-quote-small">
                                                <div class="price-select-box">
                                                    <span>Select Style:</span>
                                                    <div class="price-select-box-row">
                                                        <asp:DropDownList ID="ddlHeaderDesign" runat="server" CssClass="option11">
                                                            <asp:ListItem Selected="True" Value="">Select One</asp:ListItem>
                                                            <asp:ListItem Value="Pole Pocket">Pole Pocket</asp:ListItem>
                                                            <asp:ListItem Value="French">French</asp:ListItem>
                                                            <asp:ListItem Value="Parisian">Parisian</asp:ListItem>
                                                            <asp:ListItem Value="Inverted">Inverted</asp:ListItem>
                                                            <asp:ListItem Value="Goblet">Goblet</asp:ListItem>
                                                            <asp:ListItem Value="Grommet">Grommet</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<a title="Help" target="_blank" href="pleat-guide.html">--%>
                                                        <a target="_blank" href="/pleatguide">
                                                            <img width="16" height="16" class="img-left" title="Help" alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                </div>
                                                <div class="price-select-width">
                                                    <span>Select Width:</span>
                                                    <div class="price-select-width-row">
                                                        <asp:DropDownList ID="ddlFinishedWidth" runat="server" CssClass="option11">
                                                            <asp:ListItem Selected="True">Width</asp:ListItem>
                                                            <asp:ListItem Value="25">25"</asp:ListItem>
                                                            <asp:ListItem Value="26">26"</asp:ListItem>
                                                            <asp:ListItem Value="27">27"</asp:ListItem>
                                                            <asp:ListItem Value="28">28"</asp:ListItem>
                                                            <asp:ListItem Value="29">29"</asp:ListItem>
                                                            <asp:ListItem Value="30">30"</asp:ListItem>
                                                            <asp:ListItem Value="31">31"</asp:ListItem>
                                                            <asp:ListItem Value="32">32"</asp:ListItem>
                                                            <asp:ListItem Value="33">33"</asp:ListItem>
                                                            <asp:ListItem Value="34">34"</asp:ListItem>
                                                            <asp:ListItem Value="35">35"</asp:ListItem>
                                                            <asp:ListItem Value="36">36"</asp:ListItem>
                                                            <asp:ListItem Value="37">37"</asp:ListItem>
                                                            <asp:ListItem Value="38">38"</asp:ListItem>
                                                            <asp:ListItem Value="39">39"</asp:ListItem>
                                                            <asp:ListItem Value="40">40"</asp:ListItem>
                                                            <asp:ListItem Value="41">41"</asp:ListItem>
                                                            <asp:ListItem Value="42">42"</asp:ListItem>
                                                            <asp:ListItem Value="43">43"</asp:ListItem>
                                                            <asp:ListItem Value="44">44"</asp:ListItem>
                                                            <asp:ListItem Value="45">45"</asp:ListItem>
                                                            <asp:ListItem Value="46">46"</asp:ListItem>
                                                            <asp:ListItem Value="47">47"</asp:ListItem>
                                                            <asp:ListItem Value="48">48"</asp:ListItem>
                                                            <asp:ListItem Value="49">49"</asp:ListItem>
                                                            <asp:ListItem Value="50">50"</asp:ListItem>
                                                            <asp:ListItem Value="51">51"</asp:ListItem>
                                                            <asp:ListItem Value="52">52"</asp:ListItem>
                                                            <asp:ListItem Value="53">53"</asp:ListItem>
                                                            <asp:ListItem Value="54">54"</asp:ListItem>
                                                            <asp:ListItem Value="55">55"</asp:ListItem>
                                                            <asp:ListItem Value="56">56"</asp:ListItem>
                                                            <asp:ListItem Value="57">57"</asp:ListItem>
                                                            <asp:ListItem Value="58">58"</asp:ListItem>
                                                            <asp:ListItem Value="59">59"</asp:ListItem>
                                                            <asp:ListItem Value="60">60"</asp:ListItem>
                                                            <asp:ListItem Value="61">61"</asp:ListItem>
                                                            <asp:ListItem Value="62">62"</asp:ListItem>
                                                            <asp:ListItem Value="63">63"</asp:ListItem>
                                                            <asp:ListItem Value="64">64"</asp:ListItem>
                                                            <asp:ListItem Value="65">65"</asp:ListItem>
                                                            <asp:ListItem Value="66">66"</asp:ListItem>
                                                            <asp:ListItem Value="67">67"</asp:ListItem>
                                                            <asp:ListItem Value="68">68"</asp:ListItem>
                                                            <asp:ListItem Value="69">69"</asp:ListItem>
                                                            <asp:ListItem Value="70">70"</asp:ListItem>
                                                            <asp:ListItem Value="71">71"</asp:ListItem>
                                                            <asp:ListItem Value="72">72"</asp:ListItem>
                                                            <asp:ListItem Value="73">73"</asp:ListItem>
                                                            <asp:ListItem Value="74">74"</asp:ListItem>
                                                            <asp:ListItem Value="75">75"</asp:ListItem>
                                                            <asp:ListItem Value="76">76"</asp:ListItem>
                                                            <asp:ListItem Value="77">77"</asp:ListItem>
                                                            <asp:ListItem Value="78">78"</asp:ListItem>
                                                            <asp:ListItem Value="79">79"</asp:ListItem>
                                                            <asp:ListItem Value="80">80"</asp:ListItem>
                                                            <asp:ListItem Value="81">81"</asp:ListItem>
                                                            <asp:ListItem Value="82">82"</asp:ListItem>
                                                            <asp:ListItem Value="83">83"</asp:ListItem>
                                                            <asp:ListItem Value="84">84"</asp:ListItem>
                                                            <asp:ListItem Value="85">85"</asp:ListItem>
                                                            <asp:ListItem Value="86">86"</asp:ListItem>
                                                            <asp:ListItem Value="87">87"</asp:ListItem>
                                                            <asp:ListItem Value="88">88"</asp:ListItem>
                                                            <asp:ListItem Value="89">89"</asp:ListItem>
                                                            <asp:ListItem Value="90">90"</asp:ListItem>
                                                            <asp:ListItem Value="91">91"</asp:ListItem>
                                                            <asp:ListItem Value="92">92"</asp:ListItem>
                                                            <asp:ListItem Value="93">93"</asp:ListItem>
                                                            <asp:ListItem Value="94">94"</asp:ListItem>
                                                            <asp:ListItem Value="95">95"</asp:ListItem>
                                                            <asp:ListItem Value="96">96"</asp:ListItem>
                                                            <asp:ListItem Value="97">97"</asp:ListItem>
                                                            <asp:ListItem Value="98">98"</asp:ListItem>
                                                            <asp:ListItem Value="99">99"</asp:ListItem>
                                                            <asp:ListItem Value="100">100"</asp:ListItem>
                                                            <asp:ListItem Value="101">101"</asp:ListItem>
                                                            <asp:ListItem Value="102">102"</asp:ListItem>
                                                            <asp:ListItem Value="103">103"</asp:ListItem>
                                                            <asp:ListItem Value="104">104"</asp:ListItem>
                                                            <asp:ListItem Value="105">105"</asp:ListItem>
                                                            <asp:ListItem Value="106">106"</asp:ListItem>
                                                            <asp:ListItem Value="107">107"</asp:ListItem>
                                                            <asp:ListItem Value="108">108"</asp:ListItem>
                                                            <asp:ListItem Value="109">109"</asp:ListItem>
                                                            <asp:ListItem Value="110">110"</asp:ListItem>
                                                            <asp:ListItem Value="111">111"</asp:ListItem>
                                                            <asp:ListItem Value="112">112"</asp:ListItem>
                                                            <asp:ListItem Value="113">113"</asp:ListItem>
                                                            <asp:ListItem Value="114">114"</asp:ListItem>
                                                            <asp:ListItem Value="115">115"</asp:ListItem>
                                                            <asp:ListItem Value="116">116"</asp:ListItem>
                                                            <asp:ListItem Value="117">117"</asp:ListItem>
                                                            <asp:ListItem Value="118">118"</asp:ListItem>
                                                            <asp:ListItem Value="119">119"</asp:ListItem>
                                                            <asp:ListItem Value="120">120"</asp:ListItem>
                                                            <asp:ListItem Value="121">121"</asp:ListItem>
                                                            <asp:ListItem Value="122">122"</asp:ListItem>
                                                            <asp:ListItem Value="123">123"</asp:ListItem>
                                                            <asp:ListItem Value="124">124"</asp:ListItem>
                                                            <asp:ListItem Value="125">125"</asp:ListItem>
                                                            <asp:ListItem Value="126">126"</asp:ListItem>
                                                            <asp:ListItem Value="127">127"</asp:ListItem>
                                                            <asp:ListItem Value="128">128"</asp:ListItem>
                                                            <asp:ListItem Value="129">129"</asp:ListItem>
                                                            <asp:ListItem Value="130">130"</asp:ListItem>
                                                            <asp:ListItem Value="131">131"</asp:ListItem>
                                                            <asp:ListItem Value="132">132"</asp:ListItem>
                                                            <asp:ListItem Value="133">133"</asp:ListItem>
                                                            <asp:ListItem Value="134">134"</asp:ListItem>
                                                            <asp:ListItem Value="135">135"</asp:ListItem>
                                                            <asp:ListItem Value="136">136"</asp:ListItem>
                                                            <asp:ListItem Value="137">137"</asp:ListItem>
                                                            <asp:ListItem Value="138">138"</asp:ListItem>
                                                            <asp:ListItem Value="139">139"</asp:ListItem>
                                                            <asp:ListItem Value="140">140"</asp:ListItem>
                                                            <asp:ListItem Value="141">141"</asp:ListItem>
                                                            <asp:ListItem Value="142">142"</asp:ListItem>
                                                            <asp:ListItem Value="143">143"</asp:ListItem>
                                                            <asp:ListItem Value="144">144"</asp:ListItem>
                                                            <asp:ListItem Value="145">145"</asp:ListItem>
                                                            <asp:ListItem Value="146">146"</asp:ListItem>
                                                            <asp:ListItem Value="147">147"</asp:ListItem>
                                                            <asp:ListItem Value="148">148"</asp:ListItem>
                                                            <asp:ListItem Value="149">149"</asp:ListItem>
                                                            <asp:ListItem Value="150">150"</asp:ListItem>
                                                            <asp:ListItem Value="151">151"</asp:ListItem>
                                                            <asp:ListItem Value="152">152"</asp:ListItem>
                                                            <asp:ListItem Value="153">153"</asp:ListItem>
                                                            <asp:ListItem Value="154">154"</asp:ListItem>
                                                            <asp:ListItem Value="155">155"</asp:ListItem>
                                                            <asp:ListItem Value="156">156"</asp:ListItem>
                                                            <asp:ListItem Value="157">157"</asp:ListItem>
                                                            <asp:ListItem Value="158">158"</asp:ListItem>
                                                            <asp:ListItem Value="159">159"</asp:ListItem>
                                                            <asp:ListItem Value="160">160"</asp:ListItem>
                                                            <asp:ListItem Value="161">161"</asp:ListItem>
                                                            <asp:ListItem Value="162">162"</asp:ListItem>
                                                            <asp:ListItem Value="163">163"</asp:ListItem>
                                                            <asp:ListItem Value="164">164"</asp:ListItem>
                                                            <asp:ListItem Value="165">165"</asp:ListItem>
                                                            <asp:ListItem Value="166">166"</asp:ListItem>
                                                            <asp:ListItem Value="167">167"</asp:ListItem>
                                                            <asp:ListItem Value="168">168"</asp:ListItem>
                                                            <asp:ListItem Value="169">169"</asp:ListItem>
                                                            <asp:ListItem Value="170">170"</asp:ListItem>
                                                            <asp:ListItem Value="171">171"</asp:ListItem>
                                                            <asp:ListItem Value="172">172"</asp:ListItem>
                                                            <asp:ListItem Value="173">173"</asp:ListItem>
                                                            <asp:ListItem Value="174">174"</asp:ListItem>
                                                            <asp:ListItem Value="175">175"</asp:ListItem>
                                                            <asp:ListItem Value="176">176"</asp:ListItem>
                                                            <asp:ListItem Value="177">177"</asp:ListItem>
                                                            <asp:ListItem Value="178">178"</asp:ListItem>
                                                            <asp:ListItem Value="179">179"</asp:ListItem>
                                                            <asp:ListItem Value="180">180"</asp:ListItem>
                                                            <asp:ListItem Value="181">181"</asp:ListItem>
                                                            <asp:ListItem Value="182">182"</asp:ListItem>
                                                            <asp:ListItem Value="183">183"</asp:ListItem>
                                                            <asp:ListItem Value="184">184"</asp:ListItem>
                                                            <asp:ListItem Value="185">185"</asp:ListItem>
                                                            <asp:ListItem Value="186">186"</asp:ListItem>
                                                            <asp:ListItem Value="187">187"</asp:ListItem>
                                                            <asp:ListItem Value="188">188"</asp:ListItem>
                                                            <asp:ListItem Value="189">189"</asp:ListItem>
                                                            <asp:ListItem Value="190">190"</asp:ListItem>
                                                            <asp:ListItem Value="191">191"</asp:ListItem>
                                                            <asp:ListItem Value="192">192"</asp:ListItem>
                                                            <asp:ListItem Value="193">193"</asp:ListItem>
                                                            <asp:ListItem Value="194">194"</asp:ListItem>
                                                            <asp:ListItem Value="195">195"</asp:ListItem>
                                                            <asp:ListItem Value="196">196"</asp:ListItem>
                                                            <asp:ListItem Value="197">197"</asp:ListItem>
                                                            <asp:ListItem Value="198">198"</asp:ListItem>
                                                            <asp:ListItem Value="199">199"</asp:ListItem>
                                                            <asp:ListItem Value="200">200"</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--  <a title="Help" href="http://site.halfpricedrapes.com/new-hpd/scripts/select-width.html"
                                                        id="wdth">--%>
                                                        <a title="" onclick="variantDetail('divMakeOrderWidth');" href="javascript:void(0);">
                                                            <img width="16" height="16" style="padding: 2px 0 0 2px;" class="img-left" title="Help"
                                                                alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                </div>
                                                <div class="price-select-width">
                                                    <span>Select Length:</span>
                                                    <div class="price-select-width-row">
                                                        <asp:DropDownList ID="ddlFinishedLength" runat="server" CssClass="option11">
                                                            <asp:ListItem Selected="True" Value="">Length</asp:ListItem>
                                                            <asp:ListItem Value="45">45"</asp:ListItem>
                                                            <asp:ListItem Value="46">46"</asp:ListItem>
                                                            <asp:ListItem Value="47">47"</asp:ListItem>
                                                            <asp:ListItem Value="48">48"</asp:ListItem>
                                                            <asp:ListItem Value="49">49"</asp:ListItem>
                                                            <asp:ListItem Value="50">50"</asp:ListItem>
                                                            <asp:ListItem Value="51">51"</asp:ListItem>
                                                            <asp:ListItem Value="52">52"</asp:ListItem>
                                                            <asp:ListItem Value="53">53"</asp:ListItem>
                                                            <asp:ListItem Value="54">54"</asp:ListItem>
                                                            <asp:ListItem Value="55">55"</asp:ListItem>
                                                            <asp:ListItem Value="56">56"</asp:ListItem>
                                                            <asp:ListItem Value="57">57"</asp:ListItem>
                                                            <asp:ListItem Value="58">58"</asp:ListItem>
                                                            <asp:ListItem Value="59">59"</asp:ListItem>
                                                            <asp:ListItem Value="60">60"</asp:ListItem>
                                                            <asp:ListItem Value="61">61"</asp:ListItem>
                                                            <asp:ListItem Value="62">62"</asp:ListItem>
                                                            <asp:ListItem Value="63">63"</asp:ListItem>
                                                            <asp:ListItem Value="64">64"</asp:ListItem>
                                                            <asp:ListItem Value="65">65"</asp:ListItem>
                                                            <asp:ListItem Value="66">66"</asp:ListItem>
                                                            <asp:ListItem Value="67">67"</asp:ListItem>
                                                            <asp:ListItem Value="68">68"</asp:ListItem>
                                                            <asp:ListItem Value="69">69"</asp:ListItem>
                                                            <asp:ListItem Value="70">70"</asp:ListItem>
                                                            <asp:ListItem Value="71">71"</asp:ListItem>
                                                            <asp:ListItem Value="72">72"</asp:ListItem>
                                                            <asp:ListItem Value="73">73"</asp:ListItem>
                                                            <asp:ListItem Value="74">74"</asp:ListItem>
                                                            <asp:ListItem Value="75">75"</asp:ListItem>
                                                            <asp:ListItem Value="76">76"</asp:ListItem>
                                                            <asp:ListItem Value="77">77"</asp:ListItem>
                                                            <asp:ListItem Value="78">78"</asp:ListItem>
                                                            <asp:ListItem Value="79">79"</asp:ListItem>
                                                            <asp:ListItem Value="80">80"</asp:ListItem>
                                                            <asp:ListItem Value="81">81"</asp:ListItem>
                                                            <asp:ListItem Value="82">82"</asp:ListItem>
                                                            <asp:ListItem Value="83">83"</asp:ListItem>
                                                            <asp:ListItem Value="84">84"</asp:ListItem>
                                                            <asp:ListItem Value="85">85"</asp:ListItem>
                                                            <asp:ListItem Value="86">86"</asp:ListItem>
                                                            <asp:ListItem Value="87">87"</asp:ListItem>
                                                            <asp:ListItem Value="88">88"</asp:ListItem>
                                                            <asp:ListItem Value="89">89"</asp:ListItem>
                                                            <asp:ListItem Value="90">90"</asp:ListItem>
                                                            <asp:ListItem Value="91">91"</asp:ListItem>
                                                            <asp:ListItem Value="92">92"</asp:ListItem>
                                                            <asp:ListItem Value="93">93"</asp:ListItem>
                                                            <asp:ListItem Value="94">94"</asp:ListItem>
                                                            <asp:ListItem Value="95">95"</asp:ListItem>
                                                            <asp:ListItem Value="96">96"</asp:ListItem>
                                                            <asp:ListItem Value="97">97"</asp:ListItem>
                                                            <asp:ListItem Value="98">98"</asp:ListItem>
                                                            <asp:ListItem Value="99">99"</asp:ListItem>
                                                            <asp:ListItem Value="100">100"</asp:ListItem>
                                                            <asp:ListItem Value="101">101"</asp:ListItem>
                                                            <asp:ListItem Value="102">102"</asp:ListItem>
                                                            <asp:ListItem Value="103">103"</asp:ListItem>
                                                            <asp:ListItem Value="104">104"</asp:ListItem>
                                                            <asp:ListItem Value="105">105"</asp:ListItem>
                                                            <asp:ListItem Value="106">106"</asp:ListItem>
                                                            <asp:ListItem Value="107">107"</asp:ListItem>
                                                            <asp:ListItem Value="108">108"</asp:ListItem>
                                                            <asp:ListItem Value="109">109"</asp:ListItem>
                                                            <asp:ListItem Value="110">110"</asp:ListItem>
                                                            <asp:ListItem Value="111">111"</asp:ListItem>
                                                            <asp:ListItem Value="112">112"</asp:ListItem>
                                                            <asp:ListItem Value="113">113"</asp:ListItem>
                                                            <asp:ListItem Value="114">114"</asp:ListItem>
                                                            <asp:ListItem Value="115">115"</asp:ListItem>
                                                            <asp:ListItem Value="116">116"</asp:ListItem>
                                                            <asp:ListItem Value="117">117"</asp:ListItem>
                                                            <asp:ListItem Value="118">118"</asp:ListItem>
                                                            <asp:ListItem Value="119">119"</asp:ListItem>
                                                            <asp:ListItem Value="120">120"</asp:ListItem>
                                                            <asp:ListItem Value="121">121"</asp:ListItem>
                                                            <asp:ListItem Value="122">122"</asp:ListItem>
                                                            <asp:ListItem Value="123">123"</asp:ListItem>
                                                            <asp:ListItem Value="124">124"</asp:ListItem>
                                                            <asp:ListItem Value="125">125"</asp:ListItem>
                                                            <asp:ListItem Value="126">126"</asp:ListItem>
                                                            <asp:ListItem Value="127">127"</asp:ListItem>
                                                            <asp:ListItem Value="128">128"</asp:ListItem>
                                                            <asp:ListItem Value="129">129"</asp:ListItem>
                                                            <asp:ListItem Value="130">130"</asp:ListItem>
                                                            <asp:ListItem Value="131">131"</asp:ListItem>
                                                            <asp:ListItem Value="132">132"</asp:ListItem>
                                                            <asp:ListItem Value="133">133"</asp:ListItem>
                                                            <asp:ListItem Value="134">134"</asp:ListItem>
                                                            <asp:ListItem Value="135">135"</asp:ListItem>
                                                            <asp:ListItem Value="136">136"</asp:ListItem>
                                                            <asp:ListItem Value="137">137"</asp:ListItem>
                                                            <asp:ListItem Value="138">138"</asp:ListItem>
                                                            <asp:ListItem Value="139">139"</asp:ListItem>
                                                            <asp:ListItem Value="140">140"</asp:ListItem>
                                                            <asp:ListItem Value="141">141"</asp:ListItem>
                                                            <asp:ListItem Value="142">142"</asp:ListItem>
                                                            <asp:ListItem Value="143">143"</asp:ListItem>
                                                            <asp:ListItem Value="144">144"</asp:ListItem>
                                                            <asp:ListItem Value="145">145"</asp:ListItem>
                                                            <asp:ListItem Value="146">146"</asp:ListItem>
                                                            <asp:ListItem Value="147">147"</asp:ListItem>
                                                            <asp:ListItem Value="148">148"</asp:ListItem>
                                                            <asp:ListItem Value="149">149"</asp:ListItem>
                                                            <asp:ListItem Value="150">150"</asp:ListItem>
                                                            <asp:ListItem Value="151">151"</asp:ListItem>
                                                            <asp:ListItem Value="152">152"</asp:ListItem>
                                                            <asp:ListItem Value="153">153"</asp:ListItem>
                                                            <asp:ListItem Value="154">154"</asp:ListItem>
                                                            <asp:ListItem Value="155">155"</asp:ListItem>
                                                            <asp:ListItem Value="156">156"</asp:ListItem>
                                                            <asp:ListItem Value="157">157"</asp:ListItem>
                                                            <asp:ListItem Value="158">158"</asp:ListItem>
                                                            <asp:ListItem Value="159">159"</asp:ListItem>
                                                            <asp:ListItem Value="160">160"</asp:ListItem>
                                                            <asp:ListItem Value="161">161"</asp:ListItem>
                                                            <asp:ListItem Value="162">162"</asp:ListItem>
                                                            <asp:ListItem Value="163">163"</asp:ListItem>
                                                            <asp:ListItem Value="164">164"</asp:ListItem>
                                                            <asp:ListItem Value="165">165"</asp:ListItem>
                                                            <asp:ListItem Value="166">166"</asp:ListItem>
                                                            <asp:ListItem Value="167">167"</asp:ListItem>
                                                            <asp:ListItem Value="168">168"</asp:ListItem>
                                                            <asp:ListItem Value="169">169"</asp:ListItem>
                                                            <asp:ListItem Value="170">170"</asp:ListItem>
                                                            <asp:ListItem Value="171">171"</asp:ListItem>
                                                            <asp:ListItem Value="172">172"</asp:ListItem>
                                                            <asp:ListItem Value="173">173"</asp:ListItem>
                                                            <asp:ListItem Value="174">174"</asp:ListItem>
                                                            <asp:ListItem Value="175">175"</asp:ListItem>
                                                            <asp:ListItem Value="176">176"</asp:ListItem>
                                                            <asp:ListItem Value="177">177"</asp:ListItem>
                                                            <asp:ListItem Value="178">178"</asp:ListItem>
                                                            <asp:ListItem Value="179">179"</asp:ListItem>
                                                            <asp:ListItem Value="180">180"</asp:ListItem>
                                                            <asp:ListItem Value="181">181"</asp:ListItem>
                                                            <asp:ListItem Value="182">182"</asp:ListItem>
                                                            <asp:ListItem Value="183">183"</asp:ListItem>
                                                            <asp:ListItem Value="184">184"</asp:ListItem>
                                                            <asp:ListItem Value="185">185"</asp:ListItem>
                                                            <asp:ListItem Value="186">186"</asp:ListItem>
                                                            <asp:ListItem Value="187">187"</asp:ListItem>
                                                            <asp:ListItem Value="188">188"</asp:ListItem>
                                                            <asp:ListItem Value="189">189"</asp:ListItem>
                                                            <asp:ListItem Value="190">190"</asp:ListItem>
                                                            <asp:ListItem Value="191">191"</asp:ListItem>
                                                            <asp:ListItem Value="192">192"</asp:ListItem>
                                                            <asp:ListItem Value="193">193"</asp:ListItem>
                                                            <asp:ListItem Value="194">194"</asp:ListItem>
                                                            <asp:ListItem Value="195">195"</asp:ListItem>
                                                            <asp:ListItem Value="196">196"</asp:ListItem>
                                                            <asp:ListItem Value="197">197"</asp:ListItem>
                                                            <asp:ListItem Value="198">198"</asp:ListItem>
                                                            <asp:ListItem Value="199">199"</asp:ListItem>
                                                            <asp:ListItem Value="200">200"</asp:ListItem>
                                                            <asp:ListItem Value="201">201"</asp:ListItem>
                                                            <asp:ListItem Value="202">202"</asp:ListItem>
                                                            <asp:ListItem Value="203">203"</asp:ListItem>
                                                            <asp:ListItem Value="204">204"</asp:ListItem>
                                                            <asp:ListItem Value="205">205"</asp:ListItem>
                                                            <asp:ListItem Value="206">206"</asp:ListItem>
                                                            <asp:ListItem Value="207">207"</asp:ListItem>
                                                            <asp:ListItem Value="208">208"</asp:ListItem>
                                                            <asp:ListItem Value="209">209"</asp:ListItem>
                                                            <asp:ListItem Value="210">210"</asp:ListItem>
                                                            <asp:ListItem Value="211">211"</asp:ListItem>
                                                            <asp:ListItem Value="212">212"</asp:ListItem>
                                                            <asp:ListItem Value="213">213"</asp:ListItem>
                                                            <asp:ListItem Value="214">214"</asp:ListItem>
                                                            <asp:ListItem Value="215">215"</asp:ListItem>
                                                            <asp:ListItem Value="216">216"</asp:ListItem>
                                                            <asp:ListItem Value="217">217"</asp:ListItem>
                                                            <asp:ListItem Value="218">218"</asp:ListItem>
                                                            <asp:ListItem Value="219">219"</asp:ListItem>
                                                            <asp:ListItem Value="220">220"</asp:ListItem>
                                                            <asp:ListItem Value="221">221"</asp:ListItem>
                                                            <asp:ListItem Value="222">222"</asp:ListItem>
                                                            <asp:ListItem Value="223">223"</asp:ListItem>
                                                            <asp:ListItem Value="224">224"</asp:ListItem>
                                                            <asp:ListItem Value="225">225"</asp:ListItem>
                                                            <asp:ListItem Value="226">226"</asp:ListItem>
                                                            <asp:ListItem Value="227">227"</asp:ListItem>
                                                            <asp:ListItem Value="228">228"</asp:ListItem>
                                                            <asp:ListItem Value="229">229"</asp:ListItem>
                                                            <asp:ListItem Value="230">230"</asp:ListItem>
                                                            <asp:ListItem Value="231">231"</asp:ListItem>
                                                            <asp:ListItem Value="232">232"</asp:ListItem>
                                                            <asp:ListItem Value="233">233"</asp:ListItem>
                                                            <asp:ListItem Value="234">234"</asp:ListItem>
                                                            <asp:ListItem Value="235">235"</asp:ListItem>
                                                            <asp:ListItem Value="236">236"</asp:ListItem>
                                                            <asp:ListItem Value="237">237"</asp:ListItem>
                                                            <asp:ListItem Value="238">238"</asp:ListItem>
                                                            <asp:ListItem Value="239">239"</asp:ListItem>
                                                            <asp:ListItem Value="240">240"</asp:ListItem>
                                                            <asp:ListItem Value="241">241"</asp:ListItem>
                                                            <asp:ListItem Value="242">242"</asp:ListItem>
                                                            <asp:ListItem Value="243">243"</asp:ListItem>
                                                            <asp:ListItem Value="244">244"</asp:ListItem>
                                                            <asp:ListItem Value="245">245"</asp:ListItem>
                                                            <asp:ListItem Value="246">246"</asp:ListItem>
                                                            <asp:ListItem Value="247">247"</asp:ListItem>
                                                            <asp:ListItem Value="248">248"</asp:ListItem>
                                                            <asp:ListItem Value="249">249"</asp:ListItem>
                                                            <asp:ListItem Value="250">250"</asp:ListItem>
                                                            <asp:ListItem Value="251">251"</asp:ListItem>
                                                            <asp:ListItem Value="252">252"</asp:ListItem>
                                                            <asp:ListItem Value="253">253"</asp:ListItem>
                                                            <asp:ListItem Value="254">254"</asp:ListItem>
                                                            <asp:ListItem Value="255">255"</asp:ListItem>
                                                            <asp:ListItem Value="256">256"</asp:ListItem>
                                                            <asp:ListItem Value="257">257"</asp:ListItem>
                                                            <asp:ListItem Value="258">258"</asp:ListItem>
                                                            <asp:ListItem Value="259">259"</asp:ListItem>
                                                            <asp:ListItem Value="260">260"</asp:ListItem>
                                                            <asp:ListItem Value="261">261"</asp:ListItem>
                                                            <asp:ListItem Value="262">262"</asp:ListItem>
                                                            <asp:ListItem Value="263">263"</asp:ListItem>
                                                            <asp:ListItem Value="264">264"</asp:ListItem>
                                                            <asp:ListItem Value="265">265"</asp:ListItem>
                                                            <asp:ListItem Value="266">266"</asp:ListItem>
                                                            <asp:ListItem Value="267">267"</asp:ListItem>
                                                            <asp:ListItem Value="268">268"</asp:ListItem>
                                                            <asp:ListItem Value="269">269"</asp:ListItem>
                                                            <asp:ListItem Value="270">270"</asp:ListItem>
                                                            <asp:ListItem Value="271">271"</asp:ListItem>
                                                            <asp:ListItem Value="272">272"</asp:ListItem>
                                                            <asp:ListItem Value="273">273"</asp:ListItem>
                                                            <asp:ListItem Value="274">274"</asp:ListItem>
                                                            <asp:ListItem Value="275">275"</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <a title="" onclick="variantDetail('divMakeOrderLength');" href="javascript:void(0);">
                                                            <img width="16" height="16" style="padding: 2px 0 0 2px;" class="img-left" title="Help"
                                                                alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                </div>
                                                <div class="price-select-option">
                                                    <span>Select Options:</span>
                                                    <div class="price-select-option-row">
                                                        <asp:DropDownList ID="ddlOptionType" runat="server" CssClass="option2">
                                                            <asp:ListItem Value="" Selected="True">Options</asp:ListItem>
                                                            <asp:ListItem Value="Lined">Lined</asp:ListItem>
                                                            <asp:ListItem Value="Lined &amp; Interlined">Lined &amp; Interlined</asp:ListItem>
                                                            <asp:ListItem Value="Blackout Lining">Blackout Lining</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <a title="" onclick="variantDetail('divMakeOrderOptions');" href="javascript:void(0);">
                                                            <img width="16" height="16" style="padding: 2px 0 0 2px;" class="img-left" title="Help"
                                                                alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                </div>
                                                <div class="price-select-option">
                                                    <span>Quantity (Panels):</span>
                                                    <div class="price-select-option-row">
                                                        <asp:DropDownList ID="ddlQuantity" runat="server" CssClass="option11">
                                                            <asp:ListItem Value="" Selected="True">Quantity</asp:ListItem>
                                                            <asp:ListItem Value="1">1</asp:ListItem>
                                                            <asp:ListItem Value="2">2</asp:ListItem>
                                                            <asp:ListItem Value="3">3</asp:ListItem>
                                                            <asp:ListItem Value="4">4</asp:ListItem>
                                                            <asp:ListItem Value="5">5</asp:ListItem>
                                                            <asp:ListItem Value="6">6</asp:ListItem>
                                                            <asp:ListItem Value="7">7</asp:ListItem>
                                                            <asp:ListItem Value="8">8</asp:ListItem>
                                                            <asp:ListItem Value="9">9</asp:ListItem>
                                                            <asp:ListItem Value="10">10</asp:ListItem>
                                                            <asp:ListItem Value="11">11</asp:ListItem>
                                                            <asp:ListItem Value="12">12</asp:ListItem>
                                                            <asp:ListItem Value="13">13</asp:ListItem>
                                                            <asp:ListItem Value="14">14</asp:ListItem>
                                                            <asp:ListItem Value="15">15</asp:ListItem>
                                                            <asp:ListItem Value="16">16</asp:ListItem>
                                                            <asp:ListItem Value="17">17</asp:ListItem>
                                                            <asp:ListItem Value="18">18</asp:ListItem>
                                                            <asp:ListItem Value="19">19</asp:ListItem>
                                                            <asp:ListItem Value="20">20</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <a title="" onclick="variantDetail('divMakeOrderQuantity');" href="javascript:void(0);">
                                                            <img width="16" height="16" style="padding: 2px 0 0 2px; float: left;" title="Help"
                                                                alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="submit-request">
                                                <a href="javascript:void(0);" onclick="return priceQuote();" title="SUBMIT REQUEST">
                                                    <img src="/images/submit-request.png" alt="SUBMIT REQUEST" title="SUBMIT REQUEST"></a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="readymade-detail" id="divFreeFabricSwatchBanner" runat="server" visible="false"
                                        style="padding-top: 15px;">
                                        <a id="lnkFabricSwatchBanner" runat="server" href="javascript:void(0);">
                                            <img id="imgFabricSwatchBanner" runat="server" /></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="display: none;">
                    <div id="divMakeOrderStyle">
                        <div style="float: right; width: 5%; margin: 0px; padding: 0px;">
                            <img src="/images/reset_search_all.png" onclick="window.parent.disablePopup();" /></div>
                        <asp:Literal ID="ltMakeOrderStyle" runat="server"></asp:Literal>
                    </div>
                    <div id="divMakeOrderWidth">
                        <%-- <div style="float: right; width: 5%; margin: 0px; padding: 0px;">
                    <img src="/images/reset_search_all.png" onclick="window.parent.disablePopup();" /></div>
                <div style="float: left; width: 95%; margin: 0px; padding: 0px;">
                    <asp:Literal ID="ltMakeOrderWidth" runat="server"></asp:Literal>
                </div>--%>
                        <div style="float: left; width: 100%; margin: 0px; padding: 0px;">
                            <div style="background: none repeat scroll 0 0 #F2F2F2; color: #D50000; font-family: Arial,Helvetica,sans-serif;
                                font-size: 13px; margin: 0 0 15px; padding: 5px 0;">
                                <h1 style="font-size: 13px; width: 474px; padding-left: 5px; margin-bottom: 8px;">
                                    Select Width:</h1>
                            </div>
                            <asp:Literal ID="ltMakeOrderWidth" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div id="divMakeOrderLength">
                        <%--  <div style="float: right; width: 5%; margin: 0px; padding: 0px;">
                    <img src="/images/reset_search_all.png" onclick="window.parent.disablePopup();" /></div>
                <div style="float: left; width: 95%; margin: 0px; padding: 0px;">
                    <asp:Literal ID="ltMakeOrderLength" runat="server"></asp:Literal>
                </div>--%>
                        <div style="float: left; width: 100%; margin: 0px; padding: 0px;">
                            <div style="background: none repeat scroll 0 0 #F2F2F2; color: #D50000; font-family: Arial,Helvetica,sans-serif;
                                font-size: 13px; margin: 0 0 15px; padding: 5px 0;">
                                <h1 style="font-size: 13px; width: 474px; padding-left: 5px; margin-bottom: 8px;">
                                    Select Length:</h1>
                            </div>
                            <asp:Literal ID="ltMakeOrderLength" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div id="divMakeOrderOptions">
                        <%--<div style="float: right; width: 5%; margin: 0px; padding: 0px;">
                    <img src="/images/reset_search_all.png" onclick="window.parent.disablePopup();" /></div>
                <div style="float: left; width: 95%; margin: 0px; padding: 0px;">
                    <asp:Literal ID="ltMakeOrderOptions" runat="server"></asp:Literal>
                </div>--%>
                        <div style="float: left; width: 100%; margin: 0px; padding: 0px;">
                            <div style="background: none repeat scroll 0 0 #F2F2F2; color: #D50000; font-family: Arial,Helvetica,sans-serif;
                                font-size: 13px; margin: 0 0 15px; padding: 5px 0;">
                                <h1 style="font-size: 13px; width: 474px; padding-left: 5px; margin-bottom: 8px;">
                                    Select Options:</h1>
                            </div>
                            <asp:Literal ID="ltMakeOrderOptions" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div id="divMakeOrderQuantity">
                        <%--<div style="float: right; width: 5%; margin: 0px; padding: 0px;">
                    <img src="/images/reset_search_all.png" onclick="window.parent.disablePopup();" /></div>
                <div style="float: left; width: 95%; margin: 0px; padding: 0px;">
                    <asp:Literal ID="ltMakeOrderQuantity" runat="server"></asp:Literal>
                </div>--%>
                        <div style="float: left; width: 100%; margin: 0px; padding: 0px;">
                            <div style="background: none repeat scroll 0 0 #F2F2F2; color: #D50000; font-family: Arial,Helvetica,sans-serif;
                                font-size: 13px; margin: 0 0 15px; padding: 5px 0;">
                                <h1 style="font-size: 13px; width: 474px; padding-left: 5px; margin-bottom: 8px;">
                                    Select Quantity:</h1>
                            </div>
                            <asp:Literal ID="ltMakeOrderQuantity" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
            <div class="item-row2">
            </div>
            <div class="item-row3">
                &nbsp;</div>
            <div class="item-right" style="width: 51%;">
            </div>
        </div>
    </div>
    <%--commented--%>
    <div class="item-content" style="z-index: 99; display: none;">
        <asp:Literal ID="lttotalitems" runat="server"></asp:Literal>
        <asp:Literal ID="ltsubtotal" runat="server"></asp:Literal>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div id="blanket" style="display: none;">
    </div>
    <div id="popUpDiv" style="display: none; position: absolute;">
        <div style="background-color: #cccccc; margin-top: 8px; padding-right: 3px; padding-top: 4px;">
            <a href="javascript:void(0)" onclick="popup('popUpDiv')" style="float: right; margin-right: 3px;">
                <img src="/images/cancel.png" id="closeimage" />
            </a>
        </div>
    </div>
    <div style="float: left; padding-left: 15px; padding-top: 5px" id="ltvideo" runat="server">
        <img src="/images/video.png" title="Show Video" border="0" width="16px" height="16px"
            style="vertical-align: middle; margin-right: 3px" /><a id="button" tabindex="4" href="javascript:void(0);"
                onclick="popup('popUpDiv')" style='color: #628e2c; font-weight: bold; text-decoration: underline;
                font-size: 12px; font-family: Arial,Helvetica,sans-serif;'> Show Video</a>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hdnVariantValueId" runat="server" value="0" />
        <input type="hidden" id="hdnVariantNameId" runat="server" value="0" />
        <input type="hidden" id="hdnVariantPrice" runat="server" value="0" />
        <input type="hidden" id="hdnIsShowImageZoomer" runat="server" value="0" />
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <input type="button" id="Button2" />
        <asp:ImageButton ID="popupContactClose" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
        <asp:ImageButton ID="popupContactClose1" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
        <input type="hidden" id="hdnprice" runat="server" value="0" />
        <input type="hidden" id="hdnsaleprice" runat="server" value="0" />
        <input type="hidden" id="hdnYousave" runat="server" value="0" />
        <input type="hidden" id="hdnActual" runat="server" value="0" />
        <input type="hidden" id="hdnSaleActual" runat="server" value="0" />
        <input type="hidden" id="hdnnames" runat="server" value="" />
        <input type="hidden" id="hdnValues" runat="server" value="" />
        <input type="hidden" id="hdnSalePriceTag" runat="server" value="" />
        <asp:ImageButton ID="btnMultiPleAddtocart" runat="server" ToolTip="ADD TO CART" Width="151"
            Height="40" Style="margin: 0px;" ImageUrl="/images/item-add-to-cart.png" OnClick="btnMultiPleAddtocart_Click" />
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); height: 100%;
        width: 100%; z-index: 1000; display: none;">
        <div style="border: 1px solid #ccc;">
            <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                <tr>
                    <td>
                        <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc;
                            width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px; -moz-border-radius: 10px;
                            border-radius: 10px;">
                            <center>
                                <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                <b style="color: #fff;">Loading ... ... Please wait!</b></center>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="popupContact1" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
        <div style='float: right; background-color: transparent; right: -15px; top: -18px;
            position: absolute;'>
            <a href="javascript:void(0);" onclick="javascript:disablePopup();" title="">
                <img src="/images/popupclose.png" alt="" /></a>
        </div>
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <iframe id="frmdisplay1" frameborder="0" height="650" width="580" scrolling="auto">
                        </iframe>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;
        background: #fff;">
        <div style='float: right; background-color: transparent; right: -15px; top: -18px;
            position: absolute;'>
            <a href="javascript:void(0);" onclick="javascript:disablePopup();" title="">
                <img src="/images/popupclose.png" alt="" /></a>
        </div>
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left">
                        <iframe id="frmdisplay" frameborder="0" height="450" scrolling="auto"></iframe>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divtempimage" runat="server" style="display: none;">
    </div>
    <script type="text/javascript">
        var countries = new ddtabcontent("countrytabs")
        countries.setpersist(true)
        countries.setselectedClassTarget("link") //"link" or "linkparent"
        countries.init()
    </script>
    <script src="/js/JQueryStoreIndex.js" type="text/javascript"></script>
    </form>
</body>
</html>
