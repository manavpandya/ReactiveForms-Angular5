<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ItemPageRating.aspx.cs" Inherits="Solution.UI.Web.ItemPageRating" %>

<%@ Register Src="~/Controls/Dealoftheday.ascx" TagName="Dealoftheday" TagPrefix="ucdeal" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            width: 75%;
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
            width: 35%;
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
            width: 27%;
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
            float: right;
            margin: 0;
            padding: 0;
            line-height: 40px;
        }
    </style>
    <script type="text/javascript">


        function ShowModelForPleatGuidequote() {
            //centerPopup1pricequote();
            document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '';
            document.getElementById('Iframepricequote').height = '600px';
            document.getElementById('Iframepricequote').width = '970px';
            document.getElementById('popupContactpricequote').setAttribute("style", "z-index: 2000001; top: 0px; padding: 0px;width:970px;height:600px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContactpricequote').style.width = '970px';
            document.getElementById('popupContactpricequote').style.height = '600px';
            window.scrollTo(0, 0);
            centerPopup1pricequote(); loadPopup1pricequote(); //load popup here 
            var root = window.location.protocol + '//' + window.location.host;
            document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('divPleatGuide').innerHTML;
        }

        function variantDetailpricequote(divid) {
            //  centerPopup1();
            //document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '';
            document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '';
            document.getElementById('Iframepricequote').height = '500px';
            document.getElementById('Iframepricequote').width = '517px';
            document.getElementById('popupContactpricequote').setAttribute("style", "z-index: 2000001; top: 0px; padding: 0px;width:517px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContactpricequote').style.width = '517px';
            document.getElementById('popupContactpricequote').style.height = '500px';

            window.scrollTo(0, 0);
            centerPopup1pricequote(); loadPopup1pricequote(); //load popup here 
            var root = window.location.protocol + '//' + window.location.host;
            document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById(divid).innerHTML;
        }
        function ShowModelForPriceQuote() {
            // document.getElementById('header-part').style.zIndex = -1;

            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay').height = '260px';
            document.getElementById('frmdisplay').width = '550px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:550px;height:260px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact').style.width = '550px';
            document.getElementById('popupContact').style.height = '260px';
            window.scrollTo(0, 0);

            var root = window.location.protocol + '//' + window.location.host;
            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('ContentPlaceHolder1_divPriceQuote').innerHTML.toString().replace(/ContentPlaceHolder1_/g, '');
            //document.getElementById('frmdisplay1').src = id;
            centerPopup();
            loadPopup();
        }


        function ReviewCount(RevId, mode) {
            if (RevId > 0) {
                if (document.getElementById('ContentPlaceHolder1_hdnReviewType') != null) {
                    document.getElementById('ContentPlaceHolder1_hdnReviewType').value = mode;
                    document.getElementById('ContentPlaceHolder1_hdnReviewId').value = RevId;
                    document.getElementById('ContentPlaceHolder1_btnReviewCount').click();
                }
            }
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
        function tabdisplaycartpolicy(id, id1) {

            if (document.getElementById('lishippingtime') != null) {
                document.getElementById('lishippingtime').className = '';
            }
            if (document.getElementById('divshippingtime') != null) {
                document.getElementById('divshippingtime').style.display = 'none';
            }

            if (document.getElementById('lireturnpolicy') != null) {
                document.getElementById('lireturnpolicy').className = '';
            }
            if (document.getElementById('divreturnpolicy') != null) {
                document.getElementById('divreturnpolicy').style.display = 'none';
            }


            if (document.getElementById(id) != null) {
                document.getElementById(id).className = 'tabberactive';
            }
            if (document.getElementById(id1) != null) {
                document.getElementById(id1).style.display = '';
            }



        }
        function tabdisplaycart(id, id1) {

            if (document.getElementById('ContentPlaceHolder1_liswatch') != null) {
                document.getElementById('ContentPlaceHolder1_liswatch').className = '';
            }
            if (document.getElementById('ContentPlaceHolder1_divswatch') != null) {
                document.getElementById('ContentPlaceHolder1_divswatch').style.display = 'none';
            }

            if (document.getElementById('ContentPlaceHolder1_licustom') != null) {
                document.getElementById('ContentPlaceHolder1_licustom').className = '';
            }
            if (document.getElementById('ContentPlaceHolder1_divcustom') != null) {
                document.getElementById('ContentPlaceHolder1_divcustom').style.display = 'none';
            }

            if (document.getElementById('ContentPlaceHolder1_liready') != null) {
                document.getElementById('ContentPlaceHolder1_liready').className = '';
            }
            if (document.getElementById('ContentPlaceHolder1_divready') != null) {
                document.getElementById('ContentPlaceHolder1_divready').style.display = 'none';
            }

            if (document.getElementById(id) != null) {
                document.getElementById(id).className = 'tabberactive';
            }
            if (document.getElementById(id1) != null) {
                document.getElementById(id1).style.display = '';
            }



        }
        //        var $j = jQuery.noConflict();
        //        $j(document).ready(function ($) {
        //            var imagename = '';
        //            if (document.getElementById('ContentPlaceHolder1_imgMain')) {
        //                imagename = document.getElementById('ContentPlaceHolder1_imgMain').src;
        //            }
        //            imagename = imagename.replace('medium', 'large');


        //            $('#ContentPlaceHolder1_imgMain').addimagezoom({
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

        function priceQuote() {
            var ifr = document.getElementById("frmdisplay");

            if ((ifr.contentWindow.document.getElementById('ddlHeaderDesign').options[ifr.contentWindow.document.getElementById('ddlHeaderDesign').selectedIndex]).text == 'Select One') {
                alert('Please select Header.');
                ifr.contentWindow.document.getElementById('ddlHeaderDesign').focus();
                return false;
            }
            if ((ifr.contentWindow.document.getElementById('ddlFinishedWidth').options[ifr.contentWindow.document.getElementById('ddlFinishedWidth').selectedIndex]).text == 'Width') {

                alert('Please select Width.');
                ifr.contentWindow.document.getElementById('ddlFinishedWidth').focus();
                return false;
            }
            if ((ifr.contentWindow.document.getElementById('ddlFinishedLength').options[ifr.contentWindow.document.getElementById('ddlFinishedLength').selectedIndex]).text == 'Length') {

                alert('Please select Length.');
                ifr.contentWindow.document.getElementById('ddlFinishedLength').focus();
                return false;
            }
            if ((ifr.contentWindow.document.getElementById('ddlOptionType').options[ifr.contentWindow.document.getElementById('ddlOptionType').selectedIndex]).text == 'Options') {

                alert('Please select Options.');
                ifr.contentWindow.document.getElementById('ddlOptionType').focus();
                return false;
            }
            if ((ifr.contentWindow.document.getElementById('ddlQuantity').options[ifr.contentWindow.document.getElementById('ddlQuantity').selectedIndex]).text == 'Quantity') {

                alert('Please select Quantity.');
                ifr.contentWindow.document.getElementById('ddlQuantity').focus();
                return false;
            }

            document.getElementById('ContentPlaceHolder1_hdnheaderqoute').value = (ifr.contentWindow.document.getElementById('ddlHeaderDesign').options[ifr.contentWindow.document.getElementById('ddlHeaderDesign').selectedIndex]).value;
            document.getElementById('ContentPlaceHolder1_hdnwidthqoute').value = (ifr.contentWindow.document.getElementById('ddlFinishedWidth').options[ifr.contentWindow.document.getElementById('ddlFinishedWidth').selectedIndex]).value;
            document.getElementById('ContentPlaceHolder1_hdnlengthqoute').value = (ifr.contentWindow.document.getElementById('ddlFinishedLength').options[ifr.contentWindow.document.getElementById('ddlFinishedLength').selectedIndex]).value;
            document.getElementById('ContentPlaceHolder1_hdnoptionhqoute').value = (ifr.contentWindow.document.getElementById('ddlOptionType').options[ifr.contentWindow.document.getElementById('ddlOptionType').selectedIndex]).value;
            document.getElementById('ContentPlaceHolder1_hdnquantityqoute').value = (ifr.contentWindow.document.getElementById('ddlQuantity').options[ifr.contentWindow.document.getElementById('ddlQuantity').selectedIndex]).value;

            disablePopup();
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
            var price = document.getElementById('ContentPlaceHolder1_hdnActual').value;
            var saleprice = document.getElementById('ContentPlaceHolder1_hdnprice').value;
            if (parseFloat(saleprice) == parseFloat(0)) {
                saleprice = price;
            }
            var vprice = 0;
            var isselected = 0;
            if (document.getElementById('diestimatedate') != null) {
                $('#diestimatedate').html('');
               
            }

            if (document.getElementById('divVariant')) {
                var allselect = document.getElementById('divVariant').getElementsByTagName('select');

                for (var iS = 0; iS < allselect.length; iS++) {
                    var eltSelect = allselect[iS];
                    if (eltSelect.selectedIndex != 0) {
                        if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('($') > -1) {

                            var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('($') + 2);
                            vtemp = vtemp.replace(/\)/g, '');
                            vprice = parseFloat(vprice) + parseFloat(vtemp);
                        }

                    }
                    else {
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
            //saleprice = parseFloat(saleprice) + parseFloat(vprice);
            //price = parseFloat(price) + parseFloat(vprice);
            if (document.getElementById('ContentPlaceHolder1_divRegularPrice') != null) {
                document.getElementById('ContentPlaceHolder1_divRegularPrice').innerHTML = '<tt>MSRP :</tt> <span>$' + price.toFixed(2) + '</span>';
            }
            if (document.getElementById('ContentPlaceHolder1_divYourPrice') != null) {
                var SalePriceTag;
                if (document.getElementById('ContentPlaceHolder1_hdnSalePriceTag') != null && document.getElementById('ContentPlaceHolder1_hdnSalePriceTag'.value) != '') {
                    var SalePriceTag = ' <span>' + document.getElementById('ContentPlaceHolder1_hdnSalePriceTag').value + '</span>';
                }
                document.getElementById('ContentPlaceHolder1_divYourPrice').innerHTML = '<tt>Your Price :</tt> <strong>$' + saleprice.toFixed(2) + '</strong>' + SalePriceTag;
            }

            if (document.getElementById('ContentPlaceHolder1_divYouSave') != null) {
                var diffprice = parseFloat(price) - parseFloat(saleprice);
                var diffpercentage = (parseFloat(diffprice) * parseFloat(100)) / parseFloat(price)
                document.getElementById('ContentPlaceHolder1_divYouSave').innerHTML = '<tt>You Save :</tt> <span style="color:#B92127;">$' + diffprice.toFixed(2) + '<span style="padding-left: 0px;color:#B92127;"> (' + diffpercentage.toFixed(2) + '%)</span></span>&nbsp;';
            }
            if (isselected == 0) {
                var qid = document.getElementById('ContentPlaceHolder1_txtQty').value;
                var pid = '<%=Request["PID"] %>';
                var Names = ""; var Values = "";
                if (document.getElementById('divVariant')) {
                    var allselect = document.getElementById('divVariant').getElementsByTagName('select');

                    for (var iS = 0; iS < allselect.length; iS++) {
                        var eltSelect = allselect[iS];
                        if (eltSelect.selectedIndex == 0) {

                        }
                        else {
                            var nametext = eltSelect.id.replace('Selectvariant-', 'divvariantname-');
                            Names = Names + document.getElementById(nametext).innerHTML + ',';
                            Values = Values + eltSelect.options[eltSelect.selectedIndex].text + ',';


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
                                    $('#diestimatedate').html('<span class="readymade-detail-left" style="width:100% !important;color:#B92127;">' + msg.d + '</span>');
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
             var vidtt=  document.getElementById(dropid).options[document.getElementById(dropid).selectedIndex].text;
             if (vidtt.toString().toLowerCase().indexOf('pole pocket') > -1) {
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
                               else {
                                   PriceChangeondropdown();
             }


        }

        //        function sendData(id) {
        //            if (document.getElementById(id).options[document.getElementById(id).selectedIndex].text.toLowerCase().indexOf("custom") > -1) {
        //                document.getElementById('ContentPlaceHolder1_txtWidth').value = '';
        //                document.getElementById('ContentPlaceHolder1_txtLength').value = '';
        //                document.getElementById('ContentPlaceHolder1_txtWidth').onchange = function () { ChangeCustomprice(id); }
        //                document.getElementById('ContentPlaceHolder1_txtLength').onchange = function () { ChangeCustomprice(id); }
        //                document.getElementById('ContentPlaceHolder1_txtQty').onchange = function () { ChangeCustomprice(id); }
        //                document.getElementById('PCustom').style.display = '';
        //            }
        //            else {

        //                document.getElementById('ContentPlaceHolder1_txtWidth').value = '';
        //                document.getElementById('ContentPlaceHolder1_txtLength').value = '';
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
            if (document.getElementById('ContentPlaceHolder1_hdnSalePriceTag') != null && document.getElementById('ContentPlaceHolder1_hdnSalePriceTag'.value) != '') {
                var SalePriceTag = ' <span>' + document.getElementById('ContentPlaceHolder1_hdnSalePriceTag').value + '</span>';
            }
            if (document.getElementById('divmeadetomeasure') != null) {

                $('#divmeadetomeasure').html('');
                //$('#divmeadetomeasure').attr('style', 'display:none;');
            }
            if (document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex != 0) {
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


            }
            else {
                if (document.getElementById('divcustomprice') != null) {
                    $('#divcustomprice').html(document.getElementById('hdnpricetemp').value);
                }



            }

            if (document.getElementById('divcustomprice') != null) {
                $('#divcustomprice').attr('style', 'background: none;');
            }
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

                                    $('#divmeadetomeasure').html('<span class="readymade-detail-left" style="width:100% !important;color:#B92127;">' + msg.d + '</span>');
                                    $('#divmeadetomeasure').attr('style', 'display:block;');

                                }
                            },
                            Error: function (x, e) {
                            }
                        });
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
    <%--  <script src="/js/JQueryStoreIndex.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script src="/js/featuredimagezoomer.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="/js/jquery.jcarousel.pack_itempage.js"></script>--%>
    <%-- <script type="text/javascript">

        jQuery(document).ready(function () {
            jQuery('#mycarousel').jcarousel({
                vertical: true,
                scroll: 2
            });
        });
    </script>--%>
    <script type="text/javascript" src="/js/jquery.jcarousel.pack.js"></script>
    <script type="text/javascript" src="/js/gallery.js"></script>
    <script type="text/javascript">
        var $k = jQuery.noConflict();
        $k(document).ready(function () {
            $k('#mycarousel').jcarousel({
                vertical: true,
                scroll: 2
            });
        });
    </script>
    <%-- <script src="/js/tabber.js" type="text/javascript"> </script>--%>
    <%--New Added--%>
    <script src="/js/jquery-1.3.2.js" type="text/javascript"> </script>
    <script type="text/javascript" src="/js/jquery-alerts.js"></script>
    <script src="/js/popup.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/css/jquery.alerts.css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/js/featuredimagezoomer.js" type="text/javascript"></script>
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

            });

            //            if (document.getElementById('ContentPlaceHolder1_hdnIsShowImageZoomer').value == "true" && document.getElementById('ContentPlaceHolder1_imgMain').src.indexOf('image_not_available') <= -1) {
            //                var imagename = '';
            //                if (document.getElementById('ContentPlaceHolder1_imgMain')) {
            //                    imagename = document.getElementById('ContentPlaceHolder1_imgMain').src;
            //                }
            //                imagename = imagename.replace('medium', 'large');


            //                $j('#ContentPlaceHolder1_imgMain').addimagezoom({
            //                    zoomrange: [3, 10],
            //                    magnifiersize: [300, 300],
            //                    magnifierpos: 'right',
            //                    cursorshade: true,
            //                    largeimage: imagename //<-- No comma after last option!
            //                });
            //                $j("#Button2").click(function () {
            //                    var imagename = document.getElementById('ContentPlaceHolder1_imgMain').src;
            //                    imagename = imagename.toLowerCase().replace('medium', 'large');

            //                    $j('#ContentPlaceHolder1_imgMain').addimagezoom({
            //                        zoomrange: [3, 10],
            //                        magnifiersize: [300, 300],
            //                        magnifierpos: 'right',
            //                        cursorshade: true,
            //                        largeimage: imagename + '?54666'
            //                    })
            //                });
            //                //$('#loader_img1').show(); $('#ContentPlaceHolder1_imgMain').hide(); $('#ContentPlaceHolder1_imgMain').load(function () { $('#loader_img1').hide(); $('#ContentPlaceHolder1_imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });
            //            }

        });
    </script>
    <%--<link href="css/popup_gallery.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        function ShowModelHelp(id) {

            // document.getElementById('header-part').style.zIndex = -1;

            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay').height = '650px';
            document.getElementById('frmdisplay').width = '710px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:517px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

            document.getElementById('popupContact').style.width = '710px';
            document.getElementById('popupContact').style.height = '650px';
            window.scrollTo(0, 0);
            document.getElementById('btnreadmore').click();
            //document.getElementById('frmdisplay').src = '/MoreImages.aspx?PID=' + id;
            var imgnm = document.getElementById('ContentPlaceHolder1_imgMain').src;
            document.getElementById('frmdisplay').src = '/MoreImages.aspx?PID=' + id + '&img=' + imgnm;

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
            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + result;
            centerPopup();
            loadPopup();
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
            document.getElementById('frmdisplay1').height = '500px';
            document.getElementById('frmdisplay1').width = '517px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:517px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '517px';
            document.getElementById('popupContact1').style.height = '500px';
             
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
           // document.getElementById('header-part').style.zIndex = -1;
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

        function ShowModelForInappropriateRating(id) {
           // document.getElementById('header-part').style.zIndex = -1;
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

         function ShowModelForPriceMatch(id) {
           // document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '650px';
            document.getElementById('frmdisplay1').width = '600px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:600px;height:650px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '600px';
            document.getElementById('popupContact1').style.height = '650px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }

        function clearcomment() {
            if (document.getElementById('ContentPlaceHolder1_txtname') != null) { document.getElementById('ContentPlaceHolder1_txtname').value = 'Enter your name'; }
            if (document.getElementById('ContentPlaceHolder1_txtEmail') != null) { document.getElementById('ContentPlaceHolder1_txtEmail').value = 'Enter your email address'; }
            if (document.getElementById('ContentPlaceHolder1_txtcomment') != null) { document.getElementById('ContentPlaceHolder1_txtcomment').value = 'Enter your comment'; }
            if (document.getElementById('ContentPlaceHolder1_ddlrating') != null) {
                document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex = 0;
                ratingImage();
            }
        }

        function onKeyPressBlockNumbers1(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key != 46) {
                if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 0) {
                    return key;
                }
            }
            if (key == 13) {
                if (document.getElementById('ContentPlaceHolder1_btnAddToCart') != null) {
                    document.getElementById('ContentPlaceHolder1_btnAddToCart').click();
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
                if (document.getElementById('ContentPlaceHolder1_btnAddToCart') != null) {
                    document.getElementById('ContentPlaceHolder1_btnAddToCart').click();
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

            if ((document.getElementById("ContentPlaceHolder1_txtQty").value == '') || (document.getElementById("ContentPlaceHolder1_txtQty").value <= 0) || isNaN(document.getElementById('ContentPlaceHolder1_txtQty').value)) {
                jAlert("Please enter valid digits only !!!", "Message", "ContentPlaceHolder1_txtQty");
                document.getElementById("ContentPlaceHolder1_txtQty").value = 1; return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_divOptionalAccessories")) {
                var allElts = document.getElementById("ContentPlaceHolder1_gvOptionalAcc").getElementsByTagName('INPUT');
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

            else if (document.getElementById('ContentPlaceHolder1_divParent') != null && document.getElementById('ddlIsFreeEngraving').selectedIndex == 0) {
                var allDiv = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('div');
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
                    document.getElementById("ContentPlaceHolder1_hdnQuantity").value = TotQty;
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
                    var allDiv = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('div');
                    var i = 0;
                    for (i = 0; i < allDiv.length; i++) {
                        var divid = allDiv[i];
                        if (divid.id.indexOf('divmain') > -1) {
                            divid.style.display = '';
                            document.getElementById('ContentPlaceHolder1_divQuantity').style.display = 'none';
                        }
                    }
                    document.getElementById('ddlIsFreeEngraving').selectedIndex = 0;
                }
                else {
                    var allDiv = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('div');
                    var i = 0;
                    for (i = 0; i < allDiv.length; i++) {
                        var divid = allDiv[i];
                        if (divid.id.indexOf('divmain') > -1) {
                            divid.style.display = 'none';
                            document.getElementById('ContentPlaceHolder1_divQuantity').style.display = '';
                        }
                    }
                    document.getElementById('ddlIsFreeEngraving').selectedIndex = 1;
                }
            }
        }

        function checkTest() {
            if (document.getElementById('hdnEngraving') != null && document.getElementById('hdnEngraving').value != '') {
                var tt = document.getElementById('ContentPlaceHolder1_divParent').innerHTML;
                var allInput = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('*');

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
                document.getElementById('ContentPlaceHolder1_divParent').innerHTML = tt + newDiv;
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

            if (document.getElementById('ContentPlaceHolder1_divParent') != null && document.getElementById('ddlIsFreeEngraving').selectedIndex == 0) {
                var allDiv = document.getElementById('ContentPlaceHolder1_divParent').getElementsByTagName('div');

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

            document.getElementById('ContentPlaceHolder1_hdnEngravName').value = strname;
            document.getElementById('ContentPlaceHolder1_hdnEngravvalue').value = strvalue;
            document.getElementById('ContentPlaceHolder1_hdnEngravQty').value = strQty;
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
                document.getElementById('ContentPlaceHolder1_imgMain').style.display = 'none';
                if (document.getElementById('ContentPlaceHolder1_divzioom') != null) {
                    document.getElementById('ContentPlaceHolder1_divzioom').style.display = 'none';

                }
                document.getElementById('divvedeo').style.display = '';
                document.getElementById('videoid').src = '/images/pause.png';
            }
            else {
                document.getElementById('Hidden1').value = '1';
                document.getElementById('ContentPlaceHolder1_imgMain').style.display = '';
                if (document.getElementById('ContentPlaceHolder1_divzioom') != null) {
                    document.getElementById('ContentPlaceHolder1_divzioom').style.display = '';
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
    </style>
    <script type="text/javascript">
<!--
        function MM_openBrWindow(theURL, winName, features) { //v2.0
            window.open(theURL, winName, features);
        }
//-->
    </script>
    <script type="text/javascript">
        function ratingImage() {
            var indx = document.getElementById("<%=ddlrating.ClientID %>").selectedIndex;
            if (indx == 0) {
                document.getElementById("img1").src = '/images/star-form1.jpg';
                document.getElementById("img2").src = '/images/star-form1.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';
            }
            else if (indx == 1) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form1.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 2) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 3) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 4) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 5) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form.jpg';
                document.getElementById("img5").src = '/images/star-form.jpg';

            }
        }

        //        function CheckExits() {

        //            if (document.getElementById('ContentPlaceHolder1_txtname').value.replace(/^\s+|\s+$/g, "") == '') {
        //                jAlert('Please Enter Name.', 'Message', 'ContentPlaceHolder1_txtname');
        //                document.getElementById('ContentPlaceHolder1_txtname').focus();
        //                return false;
        //            }
        //            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") == '') {
        //                jAlert('Please Enter Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');

        //                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        //                return false;
        //            }
        //            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, ""))) {

        //                jAlert('Please Enter Valid Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');
        //                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
        //                return false;
        //            }
        //            if (document.getElementById('ContentPlaceHolder1_txtcomment').value.replace(/^\s+|\s+$/g, "") == '') {

        //                jAlert('Please Enter Comment', 'Message', 'ContentPlaceHolder1_txtcomment');
        //                document.getElementById('ContentPlaceHolder1_txtcomment').focus();
        //                return false;
        //            }
        //            if (document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex == 0) {

        //                jAlert('Please Select Rating.', 'Message', 'ContentPlaceHolder1_ddlrating');
        //                document.getElementById('ContentPlaceHolder1_ddlrating').focus();
        //                return false;
        //            }
        //            document.getElementById('prepage').style.display = '';
        //            if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        //                $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
        //            }
        //            return true;
        //        }
        function CheckExits() {

            if (document.getElementById('ContentPlaceHolder1_txtname').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Name.', 'Message', 'ContentPlaceHolder1_txtname');
                //document.getElementById('ContentPlaceHolder1_txtname').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtname').value.toString().toLowerCase() == 'enter your name') {
                jAlert('Please Enter Name.', 'Message', 'ContentPlaceHolder1_txtname');
                //document.getElementById('ContentPlaceHolder1_txtname').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');
                //document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.toString().toLowerCase() == 'enter your email address') {
                jAlert('Please Enter Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');
                //document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }

            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, ""))) {
                jAlert('Please Enter Valid Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');
                //document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtcomment').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Comment', 'Message', 'ContentPlaceHolder1_txtcomment');
                // document.getElementById('ContentPlaceHolder1_txtcomment').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtcomment').value.toString().toLowerCase() == 'enter your comment') {
                jAlert('Please Enter Comment.', 'Message', 'ContentPlaceHolder1_txtcomment');
                //document.getElementById('ContentPlaceHolder1_txtcomment').focus();
                return false;
            }

            if (document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex == 0) {
                jAlert('Please Select Rating.', 'Message', 'ContentPlaceHolder1_ddlrating');
                //document.getElementById('ContentPlaceHolder1_ddlrating').focus();
                return false;
            }
            document.getElementById('prepage').style.display = '';
            if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
                $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
            }
            return true;
        }
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
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div class="static-main">
        <div class="review-rating-main">
            <div class="review-rating-main-title">
                <a id="aback" runat="server" title="Back">
                    <img src="/images/back.jpg" alt="" title="" class="img-left"></a><h3>
                        <asp:Literal ID="ltrProductName" runat="server"></asp:Literal></h3>
            </div>
            <div class="review-rating-bg">
                <div class="review-left">
                    <div class="review-left-row1" id="boxContainer">
                        <div class="review-pro">
                            <div class="img-center" id="divMainImageDiv" runat="server">
                                <span></span>
                                <%--<asp:Literal ID="ltrMainImage" runat="server"></asp:Literal>--%>
                                <img src="/images/item-pro1.png" alt="" id="imgMain" runat="server" title="" style="vertical-align: middle;" />
                                <asp:Literal ID="lblTag" runat="server"></asp:Literal>
                                <asp:Literal ID="lblFreeEngravingImage" runat="server" Visible="false"></asp:Literal>
                                <input type="hidden" id="lblFreeEngraving" runat="server" visible="false" value='<%#Eval("IsFreeEngraving") %>' />
                                <div style="position: relative; margin-top: 50px; margin-left: 11px; display: none;"
                                    id="divvedeo">
                                    <asp:Literal ID="LtVedioParam" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="item-right">
                    <div id="item-tb-left2">
                        <div class="item-tb-left" id="divtaball">
                            <div class="item-tb-left2-tab">
                                <div class="tabing" id="tabber1-holder">
                                    <ul class="tabbernav">
                                        <li class="tabberactive" id="liready" runat="server" onclick="tabdisplaycart('ContentPlaceHolder1_liready','ContentPlaceHolder1_divready');">
                                            <a href="javascript:void(null);" title="READY MADE" id="areadymade" runat="server">READY
                                                MADE</a></li><li onclick="tabdisplaycart('ContentPlaceHolder1_licustom','ContentPlaceHolder1_divcustom');"
                                                    id="licustom" runat="server"><a href="javascript:void(null);" title="MADE TO ORDER"
                                                        id="amadetomeasure" runat="server">MADE TO ORDER</a></li><li onclick="tabdisplaycart('ContentPlaceHolder1_liswatch','ContentPlaceHolder1_divswatch');"
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
                                                                      <p id="diestimatedate" style="display:none;">
                                                             
                                                                      </p>
                                                    <div class="price-qty">
                                                        <div id="divQuantity" runat="server" style="float: left; width: 24%; padding: 10px 0 0 0;">
                                                            <span>Quantity :</span>
                                                            <asp:TextBox ID="txtQty" runat="server" MaxLength="4" TabIndex="0" Text="1" CssClass="price-qty-input"
                                                                onkeypress="return onKeyPressBlockNumbers1(event);" onkeyup="PriceChangeondropdown();"></asp:TextBox></div>
                                                        <div class="price-detail-right" id="divAddCart" runat="server" style="width: 74%;">
                                                            <%-- <a href="shopping-cart.html" title="Add To Cart">
                                                        <img src="images/item-add-to-cart.jpg" alt="Add To Cart" title="Add To Cart"></a>--%>
                                                            <asp:UpdatePanel runat="server" ID="uppanelAddtocart">
                                                                <ContentTemplate>
                                                                    <input type="hidden" runat="server" id="hdnQuantity" value="0" />
                                                                    <asp:ImageButton ID="btnAddToCart" runat="server" ToolTip="ADD TO CART" ImageUrl="/images/item-add-to-cart.jpg"
                                                                        OnClick="btnAddToCart_Click" OnClientClick="AddTocartForEngra(); return checkquantity();" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
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
                                                </div>
                                                <div class="item-prt1" style="display: none;">
                                                    <div class="discounts-box">
                                                        <asp:Literal ID="ltquantitydiscount" runat="server"></asp:Literal>
                                                        <asp:Literal ID="ltQtyDiscountHideen" runat="server"></asp:Literal>
                                                    </div>
                                                </div>
                                                <div class="details-row" id="divDeal1" runat="server" visible="false" style="padding: 0 0 10px 0;">
                                                    <ucdeal:Dealoftheday ID="Dealofthedaylbl" runat="server" />
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
                                                        <span> </span>
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
                                                            [ + ]</a> <a href="javascript:void(0);" style="display: none;"
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
                                                        Optional    sories</div>
                                                    <div id="OAData" style="padding-top: 3px">
                                                        <asp:GridView ID="gvOptionalAcc" CssClass="table-none" runat="server" AutoGenerateColumns="false"
                                                            CellPadding="0" CellSpacing="0" GridLines="None" AllowPaging="false" EmptyDataRowStyle-VerticalAlign="Middle"
                                                            EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-Font-Size="11px"
                                                            EmptyDataRowStyle-Wrap="true" EmptyDataText="No Record(s) Found." OnRowDataBound="gvOptionalAcc_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Add?">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ckhOASelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" Width="7%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Qty">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtOAQty" CssClass="quntity-box" onkeypress="return onKeyPressBlockNumbersOnly(event);"
                                                                            runat="server" Text="1"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgOAItem" runat="server" Width="80" ImageUrl='<%# GetMicroImage(Convert.ToString(Eval("ImageName")))%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Center" Width="7%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Product Detail">
                                                                    <ItemTemplate>
                                                                        <table border="0" cellpadding="0" cellspacing="0" style="color: #4B4B4B;">
                                                                            <tr>
                                                                                <td style="line-height: 15px;">
                                                                                    <b style="color: #4B4B4B;">Item Name :
                                                                                        <asp:Label ID="lblOAItemName" runat="server" Text='<%# Convert.ToString(Eval("Name")) %>'></asp:Label></b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="line-height: 5px;">
                                                                                    Price :<asp:Label ID="lblOARegularPrice" runat="server" Text='<%# Convert.ToDecimal(Eval("Price")).ToString("C2") %>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="line-height: 5px;">
                                                                                    <asp:Label ID="lblSalePriceTitle" runat="server" Text="Sale Price :"></asp:Label><asp:Label
                                                                                        ID="lblOASalePrice" ForeColor="Red" runat="server" Text='<%# Convert.ToDecimal(Eval("SalePrice")).ToString("C2")%>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:HiddenField ID="hdnOARegularPrice" runat="server" Value='<%# Eval("Price") %>' />
                                                                        <asp:HiddenField ID="hdnOASalePrice" runat="server" Value='<%# Eval("SalePrice") %>' />
                                                                        <asp:HiddenField ID="hdnOAProductID" runat="server" Value='<%# Eval("ProductID") %>' />
                                                                        <asp:HiddenField ID="hdnOASKU" runat="server" Value='<%# Eval("SKU") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <HeaderStyle HorizontalAlign="Left" Width="64%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
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
                                <div class="tabbertab" id="divcustom" runat="server" style="display: none;">
                                    <div>
                                        <asp:Literal ID="ltmadevariant" runat="server"></asp:Literal>
                                        <div class="readymade-detail">
                                            <div class="readymade-detail-pt1">
                                                <div class="readymade-detail-left">
                                                    Select Header:</div>
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
                                            <div class="readymade-detail-pt1">
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
                                            <div class="readymade-detail-pt1">
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
                                            <div class="readymade-detail-pt1">
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
                                            <div class="readymade-detail-pt1">
                                                <div class="readymade-detail-left">
                                                    Quantity (Panels):</div>
                                                <div class="readymade-detail-right">
                                                    <asp:DropDownList ID="dlcustomqty" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                        <asp:ListItem Value="">Quantity</asp:ListItem>
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
                                              <div class="readymade-detail-pt1" style="display:none;" id="divmeadetomeasure">
                                                             
                                                        </div>
                                            <div class="readymade-detail-pt1">
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
                                                <div class="order-swatch-item-right-1" style="width: 180px;">
                                                    <div class="order-swatch-item-rightrow-1">
                                                        <asp:Literal ID="ltSwatchPrice" runat="server"></asp:Literal>
                                                        <input type="hidden" id="hdnswatchprice" runat="server" value="0" />
                                                    </div>
                                                </div>
                                                <div class="order-swatch-item-rightrow-2" style="width: 120px;">
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
                                 <div class="price-quote-small" id="divpricequtoteformadetomeasure" runat="server">
                                                <a href="javascript:void(0);" onclick="ShowModelForPriceQuote();" title="Need Design Help">
                                                    <img src="/images/size-title.png" /></a>
                                            </div>
                                     <div class="readymade-detail" id="divPriceQuote" runat="server" style="display:none;">
                                   
                                        <div class="price-quote">
                                            <div class="price-quote-small">
                                                <img src="/images//size-title.png"></div>
                                            <div class="price-quote-small">
                                                <div class="price-select-box" style="width: 100% !important; padding-bottom: 5px; padding-top:5px;">
                                                     <div style="width: 20%;float:left;"> <span >Select Header :</span> </div> 
                                                       <div style="width: 80%;float:left;"> 
                                                
                                                    <div class="price-select-box-row" style="width: 80%;float:left;">
                                                        <asp:DropDownList ID="ddlHeaderDesign" runat="server" CssClass="option11"  style="width: 150px;">
                                                            <asp:ListItem Selected="True" Value="">Select One</asp:ListItem>
                                                            <asp:ListItem Value="Pole Pocket">Pole Pocket</asp:ListItem>
                                                            <asp:ListItem Value="French">French</asp:ListItem>
                                                            <asp:ListItem Value="Parisian">Parisian</asp:ListItem>
                                                            <asp:ListItem Value="Inverted">Inverted</asp:ListItem>
                                                            <asp:ListItem Value="Goblet">Goblet</asp:ListItem>
                                                            <asp:ListItem Value="Grommet">Grommet</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<a title="Help" target="_blank" href="pleat-guide.html">--%>
                                                        <a href="javascript:void(0);" onclick="javascript:window.parent.ShowModelForPleatGuidequote();">
                                                            <img width="16" height="16" class="img-left" title="Help" alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div class="price-select-box" style="width: 100% !important; padding-bottom: 5px; padding-top:5px;">
                                                     <div style="width: 20%;float:left;"> <span >Select Width :</span> </div> 
                                                       <div style="width: 80%;float:left;"> 
                                                    <div class="price-select-box-row" style="width: 80%;float:left;">
                                                        <asp:DropDownList ID="ddlFinishedWidth" runat="server" CssClass="option11"  style="width: 150px;">
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
                                                        <a title="" onclick="window.parent.variantDetailpricequote('divMakeOrderWidth');" href="javascript:void(0);">
                                                            <img width="16" height="16" style="padding: 2px 0 0 2px;" class="img-left" title="Help"
                                                                alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div class="price-select-width" style="width: 100% !important; padding-bottom: 5px; padding-top:5px;">
                                                    <div style="width: 20%;float:left;"> <span >Select Length :</span> </div> 
                                                    <div style="width: 80%;float:left;"> 
                                                    <div class="price-select-box-row" style="width: 80%;float:left;">
                                                        <asp:DropDownList ID="ddlFinishedLength" runat="server" CssClass="option11"  style="width: 150px;">
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
                                                        <a title="" onclick="window.parent.variantDetailpricequote('divMakeOrderLength');" href="javascript:void(0);">
                                                            <img width="16" height="16" style="padding: 2px 0 0 2px;" class="img-left" title="Help"
                                                                alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div class="price-select-option" style="width: 100% !important; padding-bottom: 5px; padding-top:5px;">
                                                     <div style="width: 20%;float:left;"> <span >Select Options :</span> </div> 
                                                 <div style="width: 80%;float:left;"> 
                                                    <div class="price-select-option-row" style="width: 80%;float:left;">
                                                        <asp:DropDownList ID="ddlOptionType" runat="server" CssClass="option2" style="width:150px !important;margin-right:0px !important;">
                                                            <asp:ListItem Value="" Selected="True">Options</asp:ListItem>
                                                            <asp:ListItem Value="Lined">Lined</asp:ListItem>
                                                            <asp:ListItem Value="Lined &amp; Interlined">Lined &amp; Interlined</asp:ListItem>
                                                            <asp:ListItem Value="Blackout Lining">Blackout Lining</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <a title="" onclick="window.parent.variantDetailpricequote('divMakeOrderOptions');" href="javascript:void(0);">
                                                            <img width="16" height="16" style="padding: 2px 0 0 2px;" class="img-left" title="Help"
                                                                alt="Help" src="/images/help-icon.png"></a>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div class="price-select-option" style="width: 100% !important; padding-bottom: 5px; padding-top:5px;">
                                                 <div style="width: 20%;float:left;"> <span >Quantity (Panels) :</span> </div>
                                                  <div style="width: 80%;float:left;">                                                    
                                                    <div class="price-select-option-row" style="width: 80%;float:left;">
                                                        <asp:DropDownList ID="ddlQuantity" runat="server" CssClass="option11" style="width:150px;">
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
                                                      <%--  <a title="" onclick="window.parent.variantDetail('divMakeOrderQuantity');" href="javascript:void(0);">
                                                            <img width="16" height="16" style="padding: 2px 0 0 2px; float: left;" title="Help"
                                                                alt="Help" src="/images/help-icon.png"></a>--%>
                                                    </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="submit-request" style="float:left !important;padding-left: 104px;">
                                                <a href="javascript:void(0);" onclick="return window.parent.priceQuote();" title="SUBMIT REQUEST">
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
        </div>
        <div class="item-left-row5" id="divImage">
            <asp:UpdatePanel ID="UpanlReviewCount" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="display: none;">
                        <asp:Button ID="btnReviewCount" OnClick="btnReviewCount_Click" runat="server" Text="review" /></div>
                    <span style="margin-bottom: 10px; padding-bottom: 10px; padding-top: 10px;">Last Comments<a
                        href="javascript:void(0);" onclick="$('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_divProductReview').offset().top }, 'slow'); "
                        style="float: right; color: #B92127; font-size: 14px; padding-right: 47%;">Write
                        a Review</a></span><div class="comment-sort-by" id="divReviewSort" runat="server">
                            <asp:DropDownList ID="ddlSortReviewCnt" runat="server" AutoPostBack="true" Width="50%"
                                OnSelectedIndexChanged="ddlSortReviewCnt_SelectedIndexChanged" CssClass="option">
                                <asp:ListItem Text="Helpfulness - High to Low" Value="high"></asp:ListItem>
                                <asp:ListItem Text="Helpfulness - Low to High" Value="low"></asp:ListItem>
                            </asp:DropDownList>
                            <span>Sort Reviews By :</span>
                        </div>
                    <asp:Literal ID="ltreviewDetail" runat="server"></asp:Literal>
                    <div class="comment-detail-title">
                    </div>
                    <div class="review-main">
                        <div class="review-paging">
                            <p>
                                <strong></strong>
                            </p>
                        </div>
                        <p class="write-review-link">
                        </p>
                        <div class="comment-sort-by" id="divReviewSort1" runat="server">
                            <asp:DropDownList ID="ddlSortReviewCnt1" runat="server" AutoPostBack="true" Width="50%"
                                OnSelectedIndexChanged="ddlSortReviewCnt1_SelectedIndexChanged" CssClass="option">
                                <asp:ListItem Text="Helpfulness - High to Low" Value="high"></asp:ListItem>
                                <asp:ListItem Text="Helpfulness - Low to High" Value="low"></asp:ListItem>
                            </asp:DropDownList>
                            <span>Sort Reviews By :</span>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divProductReview" runat="server" class="item-left-row5" style="border-top: 1px solid #DDDDDD;">
            <span>WRITE A REVIEW</span>
            <div class="review-detail-bg" id="divPostComment" runat="server" style="width: 50%;">
                <div class="slidingDivImage">
                </div>
                <div class="item-row" id="divPostReview">
                    <div class="big-title">
                        <p style="display: none;">
                            <a class="show_hideImage" onclick="ShowHideDiv(document.getElementById('review-box').value);">
                                View all Review for This Product</a></p>
                    </div>
                </div>
                <div id="ContentPlaceHolder1_upProductReview">
                    <div id="ContentPlaceHolder1_pnlpostComment" onkeypress="javascript:return WebForm_FireDefaultButton(event,
    'ContentPlaceHolder1_btnsubmit')">
                        <asp:UpdatePanel ID="upProductReview" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlpostComment" runat="server" DefaultButton="btnsubmit">
                                    <p>
                                        <asp:TextBox ID="txtname" Text="Enter your name" runat="server" onblur="if(this.value==''){this.value='Enter your name'};"
                                            onfocus="if(this.value=='Enter your name'){this.value=''};" CssClass="textbox"></asp:TextBox>
                                    </p>
                                    <p>
                                        <asp:TextBox ID="txtEmail" Text="Enter your email address" runat="server" onblur="if(this.value==''){this.value='Enter your email address'};"
                                            onfocus="if(this.value=='Enter your email address'){this.value=''};" CssClass="textbox"></asp:TextBox>
                                    </p>
                                    <p>
                                        <asp:TextBox ID="txtcomment" runat="server" Text="Enter your comment" onblur="if(this.value==''){this.value='Enter your comment'};"
                                            onfocus="if(this.value=='Enter your comment'){this.value=''};" CssClass="textarea"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </p>
                                    <div class="review-row1">
                                        <p class="review-rating" style="width: 32%;">
                                            <font style="float: left; font-size: 14px;">Rating :</font>
                                            <asp:DropDownList ID="ddlrating" runat="server" CssClass="select-box" Width="69px"
                                                Style="margin-left: 2px; float: left;" AutoPostBack="false" onchange="ratingImage();">
                                                <asp:ListItem Value="0" Text="Rating"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                            </asp:DropDownList>
                                            <img src="/images/star-form1.jpg" id="img1" style="vertical-align: middle" />
                                            <img src="/images/star-form1.jpg" id="img2" style="vertical-align: middle" />
                                            <img src="/images/star-form1.jpg" id="img3" style="vertical-align: middle" />
                                            <img src="/images/star-form1.jpg" id="img4" style="vertical-align: middle" />
                                            <img src="/images/star-form1.jpg" id="img5" style="vertical-align: middle" />
                                        </p>
                                        <a onclick="clearcomment();" href="javascript:void(0);" title="RESET" class="item-button">
                                            RESET </a>
                                        <asp:Button Style="cursor: pointer;" CssClass="item-button active" ToolTip="SUBMIT"
                                            Text="SUBMIT" ID="btnsubmit" runat="server" OnClientClick="return CheckExits();"
                                            OnClick="btnsubmit_Click" />
                                        <%--    <asp:ImageButton  CssClass="item-button active" OnClientClick="return CheckExits();"
                                                    runat="server" ToolTip="Submit"  ImageUrl="/images/submit-button.jpg" OnClick="btnsubmit_Click" />--%>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <%-- <Triggers> <asp:AsyncPostBackTrigger
    ControlID="btnsubmit" /> </Triggers>--%>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--commented--%>
    <div id="divOldCode" runat="server" visible="false">
        <div class="item-top">
            <div class="breadcrumbs" style="width: 67% !important;">
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
            <div class="f-like">
                <span>Like Us For A Chance To Win A $50 HPD Coupon.</span>
                <iframe src="http://www.facebook.com/plugins/like.php?app_id=166725596724792&amp;href=http://<%=Request.Url.Authority %><%=Request.ApplicationPath.TrimEnd('/') %><%=Request.RawUrl  %>&amp;send=false&amp;layout=button_count&amp;width=75
&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font&amp;height=21" scrolling="no"
                    frameborder="0" style="border: medium none; overflow: hidden; float: right; width: 82px;
                    height: 21px; margin-top: 2px; margin-right: 4px;" allowtransparency="true">
                </iframe>
            </div>
        </div>
        <div class="item-main">
            <div class="item-bg">
                <div class="item-right">
                    <div class="item-main-title">
                        <h1>
                        </h1>
                        <div class="rating">
                            <font id="divCustomerRating" runat="server">
                                <asp:Literal ID="ltRating" runat="server"></asp:Literal>
                            </font>
                            <p>
                                <asp:Literal ID="ltAvarageRate" runat="server"></asp:Literal>
                                <a href="javascript:void(0);" onclick="$('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_divProductReview').offset().top }, 'slow'); ">
                                    Write a Review</a></p>
                        </div>
                    </div>
                    <div class="item-left-row1" id="boxContainer">
                        <div class="more-images" id="hideMoreImages" runat="server" visible="false">
                            <ul>
                                <asp:Literal ID="ltBindMoreImage" runat="Server"></asp:Literal>
                            </ul>
                        </div>
                        <div class="item-pro">
                            <p class="item-pro-title" id="divNextBack" runat="server">
                                <%--   <div runat="server" id="IPreImage">--%>
                                <%-- <a href="#" title="Previous" class="item-prev" onclick="javascript:document.getElementById('prepage').style.display = '';"
                                id="ImgPrevious" runat="server">Previous </a>--%>
                                <a href="javascript:void(0);" style="margin-left: 15px;" class="item-prev" onmouseout="document.getElementById('ContentPlaceHolder1_imgPrev').style.display = 'none';"
                                    onmouseover="document.getElementById('ContentPlaceHolder1_imgPrev').style.display = '';"
                                    onclick="chkHeight();" id="ImgPrevious" runat="server">Previous<br />
                                    <img style="border: 1px solid #eee; width: 100px; height: 120px; position: absolute;
                                        display: none;" id="imgPrev" runat="server" />
                                </a>
                                <img id="imgNextImage" runat="server" />
                                <%-- </div>
                            <div id="INextImage" runat="server">--%>
                                <a href="javascript:void(0);" onclick="chkHeight();" id="ImgNext" onmouseout="document.getElementById('ContentPlaceHolder1_imgNextImage').style.display = 'none';"
                                    onmouseover="document.getElementById('ContentPlaceHolder1_imgNextImage').style.display = '';"
                                    class="item-next" runat="server">Next
                                    <br />
                                    <%-- <img style="border: 1px solid #eee; width: 100px; height: 100px; position: absolute;
                                    display: none;" id="imgNextImage" runat="server" />--%>
                                </a>
                                <%--</div>--%>
                            </p>
                            <div class="zoom" id="divzioom" runat="server">
                                <a href="javascript:void(0);" onclick="ShowModelHelp(<%=Request.QueryString["PID"]%>);"
                                    title="Zoom">Zoom</a> <a href="javascript:void(0);" onclick="ShowModelHelp(<%=Request.QueryString["PID"]%>);"
                                        title="Zoom">
                                        <img src="/images/zoom-icon.jpg" alt="" title=""></a>
                            </div>
                            <div class="img-right" id="popUpDivnew" style="margin-top: 10px; display: none;"
                                runat="server">
                                <a href="javascript:void(0);" onclick="ShowImagevideo();" id="anchrvideo">
                                    <img width="20" height="20" src="/images/play.png" id="videoid" border="0" />
                                </a>
                            </div>
                        </div>
                    </div>
                    <div class="item-right-row1" id="divdiscription" runat="server">
                        <div class="item-right-row1-title">
                            Product Description</div>
                        <div class="item-right-row1-bg">
                            <asp:Literal ID="ltrTabs" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="item-right-row1">
                        <div class="item-right-row1-title">
                            Product Features</div>
                        <div class="item-right-row1-bg">
                            <span>Item Code:
                                <asp:Literal ID="litModelNumber" runat="server"></asp:Literal></span>
                            <asp:Literal ID="ltfeature" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="item-right-row1" id="divProductProperty" runat="server">
                        <div class="item-right-row1-title" id="divproperty" style="cursor: pointer;">
                            <span style="float: left;">Product Properties</span><span style="flot: right;"><img
                                src="/images/expand.gif" alt="Expand" title="Expand" id="imgPro" style="float: right;
                                margin: 0;"></span></div>
                        <div class="item-right-row1-bg" id="divproperty1" style="display: none;">
                            <div class="properties-main">
                                <div class="properties1" id="divLightControl" runat="server" visible="false">
                                    <div class="item-detail-row31">
                                        <div class="item-detail-row3-left">
                                            Light Control :</div>
                                        <div class="item-detail-row3-righ1">
                                            <img id="imgLightControl" runat="server" alt="Light Control" title="Light Control">
                                        </div>
                                    </div>
                                    <div class="item-detail-row31" id="divPrivacy" runat="server" visible="false">
                                        <div class="item-detail-row3-left">
                                            Privacy :</div>
                                        <div class="item-detail-row3-righ1">
                                            <img id="imgPrivacy" runat="server" alt="Privacy" title="Privacy">
                                        </div>
                                    </div>
                                    <div style="border: 0;" class="item-detail-row31" id="divEfficiency" runat="server"
                                        visible="false">
                                        <div class="item-detail-row3-left">
                                            Efficiency :</div>
                                        <div class="item-detail-row3-righ1">
                                            <img id="imgEfficiency" runat="server" alt="Efficiency" title="Efficiency">
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
                    <div class="item-right-row2">
                    </div>
                    <div class="item-right-row4" id="divYoumay" runat="server">
                        <div class="item-right-row4-title">
                            You May Also Like
                        </div>
                        <div class="bs-main">
                            <div class="jcarousel-skin-tango1">
                                <ul id="mycarousel8">
                                    <asp:Literal ID="ltrYoumay" runat="server"></asp:Literal>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="item-right-row5" id="divOptioanlAccesories" runat="server">
                        <div class="item-right-row5-title">
                            Recommended Hardware & Accessories:</div>
                        <div class="bs-main">
                            <div class="jcarousel-skin-tango1">
                                <ul id="mycarousel9">
                                    <asp:Literal ID="ltrOptionalAccesories" runat="server"></asp:Literal>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="item-left" id="boxContainer-left" style="padding-top: 815px;">
                    <div class="item-right-row2" id="divSocialLinks" runat="server">
                        <div class="item-right-row2-title">
                            <div style="margin-left: 106px;">
                                <%-- <a href="#">
                            <img src="/images/f-like-icon.png" alt="" title=""></a>--%>
                                <div class="img-left" style="margin: 0 8px 0 0; width: 73px;">
                                    <iframe src="http://www.facebook.com/plugins/like.php?app_id=166725596724792&amp;href=http://<%=Request.Url.Authority %><%=Request.ApplicationPath.TrimEnd('/') %><%=Request.RawUrl  %>&amp;send=false&amp;layout=button_count&amp;width=75
&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font&amp;height=21" scrolling="no" frameborder="0" style="border: none;
                                        overflow: hidden; width: 82px; height: 21px;" allowtransparency="true"></iframe>
                                </div>
                               
                                <div class="img-left" style="margin: 0 8px 0 0; width: 80px;">
                                    <a href="http://twitter.com/share" class="twitter-share-button" data-count="horizontal"
                                        data-via="lbenitez">Tweet</a>
                                    <script src="/js/widgets.js" type="text/javascript"></script>
                                </div>
                                <div class="img-left" style="margin: 0 8px 0 0; width: 58px;">
                                    <g:plusone size="medium"></g:plusone>
                                    <script type="text/javascript">
                                        (function () {
                                            var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                                            po.src = 'https://apis.google.com/js/plusone.js';
                                            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                                        })();
                                    </script>
                                </div>
                                <div class="img-left" style="margin: 0 8px 0 0; width: 105px;">
                                    <asp:Literal ID="ltrPinIt" runat="server"></asp:Literal>
                                    <script type="text/javascript" src="http://assets.pinterest.com/js/pinit.js"></script>
                                    <script type="text/javascript" src="http://s7.addthis.com/js/152/addthis_widget.js"></script>
                                    <a id="lnkBookMark" runat="server" style="margin-left: 10px;" onclick="return addthis_sendto()"
                                        onmouseout="addthis_close()" onmouseover="return addthis_open(this, '', '[URL]', '[TITLE]')"
                                        href="http://www.addthis.com/bookmark.php" linkindex="21">
                                        <img src="/images/share.png" /></a>
                                </div>
                                <div class="img-left" style="margin: 0 8px 0 0; width: 105px;">
                                    <a href="http://www.houzz.com/pro/exclusivefabrics/half-price-drapes">
                                        <img src="http://www.houzz.com/res/1697/pic/badge86_25_2.png?v=1697" alt="Remodeling and Home Design"
                                            height="20" border="0" /></a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="divBookmarkLinks" runat="server" class="item-left-row2">
                        <a id="lnkPriceMatch" runat="server" title="PRICE MATCH" href="javascript:void(0);">
                            Price Match</a>
                        <label id="lblafterpricematch" runat="server">
                            |</label>
                        <a href="/faq" target="_blank" title="FAQ">FAQ</a>|<a href="/measuringguide.aspx"
                            target="_blank" title="Measuring Guide">Measuring Guide</a>|<a href="javascript:void(0);"
                                title="Design Guide">Design Guide</a> <a visible="false" id="lnkPrintThisPage" runat="server"
                                    href="javascript:void(0);" title="PRINT THIS PAGE">Print this Page</a><label id="lblafterprintpage"
                                        runat="server"></label>
                        <asp:LinkButton ID="btnWishList" runat="server" OnClick="btnWishList_Click" OnClientClick="return checkquantity();"
                            ToolTip="ADD TO WISHLIST" Visible="false">ADD TO WISHLIST</asp:LinkButton><label
                                id="lblafterwishlist" runat="server" visible="false"></label>
                        <a id="lnkTellFriend" runat="server" title="EMAIL A FRIEND" visible="false">Email a
                            Friend</a><label id="lblaftertellfriend" runat="server"></label>
                    </div>
                    <%--Here is Shipping Time Tabs--%>
                    <asp:Literal ID="ltrShippingReturnPolicy" runat="server"></asp:Literal>
                    <div class="item-left-row6" id="divrecently" runat="server">
                        <div class="item-left-row6-title">
                            Recently Viewed</div>
                        <div class="bs-main">
                            <div class="jcarousel-skin-tango1">
                                <ul id="mycarousel10">
                                    <asp:Literal ID="ltrRecently" runat="server"></asp:Literal>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="item-scroll" id="divColorOptionForFabric" runat="server">
                <span style="border-bottom: 1px solid #DDDDDD;">Color Option</span>
                <%--  <div id="Div2" class="content">
                <div class="images_container">
                    <asp:Literal ID="ltrColorOption" runat="server"></asp:Literal>
                </div>
            </div>--%>
                <div class="fp-row1">
                    <asp:Repeater ID="RepColorOption" runat="server" OnItemDataBound="RepColorOption_ItemDataBound">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                            <div class="fp-boxColor" id="Catbox" runat="server">
                                <div class="fp-displayColor" id="CatDisplay" runat="server">
                                    <div class="fp-box-divColor">
                                        <div class="img-center">
                                            <%--  <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                            title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                            <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                <asp:Image ImageUrl='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                    ID="imgName" runat="server" Height="168px" /><img src="/images/imgloader.gif" height="168px"
                                                        alt="" id='<%# "loader_imgcolor" + Convert.ToString(Container.ItemIndex +1) %>'
                                                        style="text-align: center; vertical-align: middle;" /></a>
                                        </div>
                                    </div>
                                    <h2 class="fp-box-h2-Color" style="height: 35px;">
                                        <%-- <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                        <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                            <%# SetNameForColor(Convert.ToString(Eval("Name")))%>
                                        </a>
                                </div>
                            </div>
                            <asp:Literal ID="ltBottom" runat="server"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul></FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
    <%--commented--%>
    <div class="right-sub-section" style="display: none;">
        <h1>
            Best Sellers</h1>
        <div class="best-sellers-box">
            <asp:Repeater ID="rptBestSeller" runat="server" OnItemDataBound="rptBestSeller_ItemDataBound">
                <ItemTemplate>
                    <div id="ProboxBestSeller" runat="server" class="best-sellers-box">
                        <div id="proDisplay" runat="server">
                            <div class="img-center">
                                <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%#
    Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx">
                                    <img src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' width="134"
                                        height="112" alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>" /></a></div>
                            <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName")
    %>' />
                            <asp:Literal ID="lblTagImage" runat="server" Visible="false"></asp:Literal>
                            <asp:Label ID="lblTagImageName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                            <h2 style="height: 50px;">
                                <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%#
    Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx" title="<%#
    SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                    <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                            <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                            <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                            <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                            <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                            <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                            <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                            <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("RegularPrice")),
    2)%>'></asp:Label>
                            <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%#
    Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                            <p>
                                <a href="javascript:void(0);" id="abestLink" runat="server" title="VIEW MORE">
                                    <img src="/images/view_more.jpg" alt="VIEW MORE" /></a></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="item-content" style="z-index: 99; display: none;">
        <div class="item-content-left">
            <div class="item-main">
                <div class="item-left-button">
                    <asp:ImageButton ID="btnDownload" Visible="false" runat="server" alt="DownLoad PDF"
                        title="DownLoad PDF" ImageUrl="/images/download-pdf.png" OnClick="btnDownload_Click" />
                </div>
            </div>
        </div>
        <div class="item-content-right">
            <div class="left-menu-title">
                <img src="/images/left-menu-title-top.png" width="212" height="4" class="img-left">
                <div class="my-cart-title">
                    <img src="/images/my-cart-icon.png" alt="My Cart" title="My Cart" />
                    <h2>
                        MY CART</h2>
                </div>
                <img src="/images/left-menu-title-bottom-1.png" width="212" height="4" class="img-left">
            </div>
            <div class="my-cart-box-main">
                <div class="my-cart-box">
                    <p>
                        Sub Total (<asp:Literal ID="lttotalitems" runat="server"></asp:Literal>)</p>
                    <p>
                        <strong id="subtotal">
                            <asp:Literal ID="ltsubtotal" runat="server"></asp:Literal></strong></p>
                    <div class="my-cart-box-sub" style="width: 178px; padding-left: 16px; padding-right: 16px;">
                        <asp:ImageButton ID="btnViewDetail" runat="server" ToolTip="View Detail" ImageUrl="/images/view-detail.png"
                            OnClick="btnViewDetail_Click" />
                        <asp:ImageButton ID="btnCheckout" runat="server" ToolTip="Checkout" ImageUrl="/images/checkout.png"
                            OnClick="btnCheckout_Click" />
                    </div>
                    <div class="my-cart-box-sub" style="padding-left: 36px; padding-right: 36px; width: 138px;">
                        <a href="javascript:void(0);" title="Estimated
    shipping" onclick="ShowModelHelpShipping('/ShippingCalculation.aspx?ProductId=<%=Request.QueryString["PID"]
    %>');">
                            <img title="Estimated shipping" alt="Estimated shipping" src="/images/estimated-shipping.png"></a>
                    </div>
                </div>
                <img src="/images/my-cart-box-bottom.png" class="img-left" />
            </div>
            <asp:Literal ID="ltrBox" runat="server"></asp:Literal>
        </div>
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
        <img src="/images/video.png" title="Show
    Video" border="0" width="16px" height="16px" style="vertical-align: middle; margin-right: 3px" /><a
        id="button" tabindex="4" href="javascript:void(0);" onclick="popup('popUpDiv')"
        style='color: #628e2c; font-weight: bold; text-decoration: underline; font-size: 12px;
        font-family: Arial,Helvetica,sans-serif;'> Show Video</a>
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
            ToolTip="Close" OnClientClick="return
    false;"></asp:ImageButton>
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
        <input type="hidden" id="hdnheaderqoute" runat="server" value="" />
        <input type="hidden" id="hdnwidthqoute" runat="server" value="" />
        <input type="hidden" id="hdnlengthqoute" runat="server" value="" />
        <input type="hidden" id="hdnoptionhqoute" runat="server" value="" />
         <input type="hidden" id="hdnquantityqoute" runat="server" value="" />
          <input type="hidden" value="" id="hdnReviewId" runat="server" />
                <input type="hidden" value="" id="hdnReviewType" runat="server" />
        <asp:ImageButton ID="btnMultiPleAddtocart" runat="server" ToolTip="ADD
    TO CART" Width="151" Height="40" Style="margin: 0px;" ImageUrl="/images/item-add-to-cart.png"
            OnClick="btnMultiPleAddtocart_Click" />
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <div style="border: 1px solid
    #ccc;">
            <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                <tr>
                    <td>
                        <div style="background: none repeat scroll 0 0 rgba(0,
    0,0, 0.9) !important; border: 1px solid #ccc; width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px;
                            -moz-border-radius: 10px; border-radius: 10px;">
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

    <div id="popupContactpricequote" style="z-index: 2000001; top: 30px; padding: 0px; width: 750px;display:none;">
        <div style='float: right; background-color: transparent; right: -15px; top: -18px;
            position: absolute;'>
            <a href="javascript:void(0);" onclick="javascript:disablePopuppricequote();" title="">
                <img src="/images/popupclose.png" alt="" /></a>
        </div>
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <iframe id="Iframepricequote" frameborder="0" height="650" width="580" scrolling="auto">
                        </iframe>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div id="divtempimage" runat="server" style="display: none;">
    </div>
    <div style="display: none;">
    <div id="divmeasuring">
                <asp:Literal ID="ltmeasuredata" runat="server"></asp:Literal>
            </div>
             <div id="divPleatGuide">
                <asp:Literal ID="ltrPleatGuide" runat="server"></asp:Literal>
            </div>
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
    <%--  <script type="text/javascript"> var countries = new ddtabcontent("countrytabs")
    countries.setpersist(true) countries.setselectedClassTarget("link") //"link" or
    "linkparent" countries.init() </script>--%>
    <%--<script type="text/javascript" src="/js/script.js"></script>--%>
    <script type="text/javascript" src="/js/script.js"></script>
    <script src="/js/JQueryStoreIndex.js" type="text/javascript"></script>
</asp:Content>
