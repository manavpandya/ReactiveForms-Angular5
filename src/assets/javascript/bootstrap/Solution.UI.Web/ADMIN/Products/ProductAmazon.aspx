<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductAmazon.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductAmazon" %>

<%@ Register Src="~/ADMIN/Controls/ProductHead.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script src="../JS/ProductValidation.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
    function SetHammingValue() {
            if (document.getElementById('ContentPlaceHolder1_chkIsHamming') != null && document.getElementById('ContentPlaceHolder1_chkIsHamming').checked == true) {
                document.getElementById('ContentPlaceHolder1_tdHamminglbl').style.display = '';
                document.getElementById('ContentPlaceHolder1_tdHammingQty').style.display = '';
            }
            else {
                document.getElementById('ContentPlaceHolder1_tdHamminglbl').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_tdHammingQty').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_txtHammingQty').value = '0';
            }
        }

        function btnCheck(inv, btid) {
            var r = document.getElementById(inv).value;
            // alert('sam' +'---- '+r);
            if (r <= 0) {
                alert('Enter Valid Quantity!');
            }
        }
          function OpenMoreImagesPopup() {
            var popupurl = "MoreImagesUpload.aspx?StoreID=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>";
            window.open(popupurl, "MoreIamgesPopup", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800,height=550,left=250,top=80");
        }
        $(document).ready(function () {

            $(".slidingDivWarehouse").show();
            $(".show_hideWarehouse").show();

            $('.show_hideWarehouse').click(function () {
                $(".slidingDivWarehouse").slideToggle();
            });
        });

        function ShowHideButton(id, number) {
            var imgsrc = document.getElementById(id).src;

            if (imgsrc.toLowerCase().indexOf('minimize.png') > -1) {
                document.getElementById(id).src = imgsrc.toLowerCase().replace('minimize.png', 'expand.gif');
                document.getElementById(id).title = 'Show';
                document.getElementById(id).alt = 'Show';
                document.getElementById(id).ClassName = 'close';
                document.getElementById(number).style.paddingTop = "8px";
                document.getElementById(number).style.border = "none";
                document.getElementById(number).style.paddingBottom = "8px";
            }
            else if (imgsrc.toLowerCase().indexOf('expand.gif') > -1) {
                document.getElementById(id).src = imgsrc.toLowerCase().replace('expand.gif', 'minimize.png');
                document.getElementById(id).title = 'Minimize';
                document.getElementById(id).alt = 'Minimize';
                document.getElementById(id).ClassName = 'minimize';
                document.getElementById(number).style.paddingTop = "0px";
                document.getElementById(number).style.border = "none";
                document.getElementById(number).style.paddingBottom = "0px";
            }
        }
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
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }

        function openCenteredCrossSaleWindow1(mode) {
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            if (StoreID.value != "0") {
                var width = 700;
                var height = 500;
                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));

                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('DropShipperPopUp.aspx?StoreID=' + StoreID + '&mode=' + mode, "Mywindow", windowFeatures);
                return false;
            }
            else {
                jAlert('Select Store!', 'Message', StoreID);
                return false;
            }
        }
        function openCenteredCrossSaleWindow2(mode) {
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            if (StoreID.value != "0") {
                var width = 700;
                var height = 500;
                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));

                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('AssemblerProductPopUp.aspx?StoreID=' + StoreID + '&mode=' + mode, "Mywindow", windowFeatures);
                return false;
            }
            else {
                jAlert('Select Store!', 'Message', StoreID);
                return false;
            }
        }

        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = document.getElementsByTagName("input");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
            }
        }

        function ValidateAmazonPage() {

            if ((document.getElementById('ContentPlaceHolder1_txtTitle').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Product Title', 'Message', 'ContentPlaceHolder1_txtTitle');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtTitle').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtSKU').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter SKU', 'Message', 'ContentPlaceHolder1_txtSKU');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSKU').offset().top }, 'slow');
                return false;
            }
//            if (document.getElementById("ContentPlaceHolder1_ddlproductidtype") != null && document.getElementById("ContentPlaceHolder1_ddlproductidtype").selectedIndex == 0) {
//                jAlert('Please Select Product ID Type', 'Message', 'ContentPlaceHolder1_ddlproductidtype');
//                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlproductidtype').offset().top }, 'slow');
//                return false;
//            }
            if ((document.getElementById('ContentPlaceHolder1_txtStandardProductID').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter UPC', 'Message', 'ContentPlaceHolder1_txtStandardProductID');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtStandardProductID').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtStandardProductID').value).replace(/^\s*\s*$/g, '').length >12 || (document.getElementById('ContentPlaceHolder1_txtStandardProductID').value).replace(/^\s*\s*$/g, '').length < 12 ) {
                jAlert('UPC must be 12 digit long', 'Message', 'ContentPlaceHolder1_txtStandardProductID');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtStandardProductID').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtItemPrice').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Item Price', 'Message', 'ContentPlaceHolder1_txtItemPrice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtItemPrice').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtSalesPrice').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Sales Price', 'Message', 'ContentPlaceHolder1_txtSalesPrice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSalesPrice').offset().top }, 'slow');
                return false;
            }
            var sprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtSalesPrice').value).replace(/^\s*\s*$/g, '')).toFixed(2);
            sprice = sprice;
            var itemprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtItemPrice').value).replace(/^\s*\s*$/g, '')).toFixed(2);
            itemprice = itemprice;
            var ourPrice = parseFloat((document.getElementById('ContentPlaceHolder1_txtOurPrice').value)).toFixed(2);
            ourPrice = ourPrice;

            if (parseFloat(sprice) > parseFloat(itemprice)) {
                jAlert('Sales Price Should be less than or equal Item Price', 'Message', 'ContentPlaceHolder1_txtSalesPrice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSalesPrice').offset().top }, 'slow');
                return false;
            }
            if (parseFloat(sprice) > parseFloat(0) && parseFloat(sprice) < parseFloat(ourPrice)) {
                jAlert('Our Cost should be less than Sale Price', 'Message', 'ContentPlaceHolder1_txtOurPrice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtOurPrice').offset().top }, 'slow');
                return false;
            }
            else if (parseFloat(itemprice) < parseFloat(ourPrice)) {
                jAlert('Our Cost should be less than Price', 'Message', 'ContentPlaceHolder1_txtOurPrice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtOurPrice').offset().top }, 'slow');
                return false;
            }

            if ((document.getElementById('ContentPlaceHolder1_txtInventory').value).replace(/^\s*\s*$/g, '') == '') {
                if (document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_0') != null) {
                    jAlert('Please Enter Inventory from Warehouse', 'Message', 'ContentPlaceHolder1_grdWarehouse_txtInventory_0');
                    $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_grdWarehouse_txtInventory_0').offset().top }, 'slow');
                }
                else {
                    jAlert('Please Enter Inventory from Warehouse', 'Message', 'ContentPlaceHolder1_txtInventory');
                    $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtInventory').offset().top }, 'slow');
                }
                return false;
            }
             if ((document.getElementById('ContentPlaceHolder1_txtInventory').value).replace(/^\s*\s*$/g, '') != '') {
                var InvVlaue = checkCount();
                if (InvVlaue == false) { $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_grdWarehouse').offset().top }, 'slow'); return false; }        
            }
            if ((document.getElementById('ContentPlaceHolder1_txtWeight').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Weight', 'Message', 'ContentPlaceHolder1_txtWeight');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtWeight').offset().top }, 'slow');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_ddlManufacture').selectedIndex == 0) {
                jAlert('Please Select Manufacturer', 'Message', 'ContentPlaceHolder1_ddlManufacture');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlManufacture').offset().top }, 'slow');
                return false;
            }
//            if ((document.getElementById('ContentPlaceHolder1_txtMFRPartNumber').value).replace(/^\s*\s*$/g, '') == '') {
//                jAlert('Please Enter MFR Part Number', 'Message', 'ContentPlaceHolder1_txtMFRPartNumber');
//                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtMFRPartNumber').offset().top }, 'slow');
//                return false;
//            }
           if ((document.getElementById('ContentPlaceHolder1_txtItemType').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Material', 'Message', 'ContentPlaceHolder1_txtItemType');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtItemType').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtBrand').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Brand', 'Message', 'ContentPlaceHolder1_txtBrand');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtBrand').offset().top }, 'slow');
                return false;
            }
            if (document.getElementById("ContentPlaceHolder1_ddlFulfillment") != null && document.getElementById("ContentPlaceHolder1_ddlFulfillment").selectedIndex == 0) {
                jAlert('Please Select Fulfillment', 'Message', 'ContentPlaceHolder1_ddlFulfillment');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlFulfillment').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtCurrency').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Currency', 'Message', 'ContentPlaceHolder1_txtCurrency');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtCurrency').offset().top }, 'slow');
                return false;
            }

//            if (document.getElementById("ContentPlaceHolder1_chkIsHamming") != null && document.getElementById("ContentPlaceHolder1_chkIsHamming").checked == true && document.getElementById("ContentPlaceHolder1_txtHammingQty") != null) {
//                if (document.getElementById("ContentPlaceHolder1_txtHammingQty").value != '') {
//                    var Inventory = parseInt((document.getElementById('ContentPlaceHolder1_txtInventory').value)).toFixed(2);
//                    Inventory = Inventory;
//                    var HammingQty = parseInt((document.getElementById('ContentPlaceHolder1_txtHammingQty').value)).toFixed(2);
//                    HammingQty = HammingQty;

//                    if (parseInt(HammingQty) > parseInt(Inventory)) {
//                        jAlert('Hemming Quantity should be less than Inventory', 'Message', 'ContentPlaceHolder1_txtHammingQty');
//                        $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtHammingQty').offset().top }, 'slow');
//                        return false;
//                    }
//             }
//        else {
//            jAlert('Please Enter Hemming Percentage', 'Message', 'ContentPlaceHolder1_txtHammingQty');
//            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtHammingQty').offset().top }, 'slow');
//            return false;
//        }
//    }

//            if (document.getElementById("ContentPlaceHolder1_ddlProductType") != null && document.getElementById("ContentPlaceHolder1_ddlProductType").selectedIndex == 0) {
//                jAlert('Please Select Product Type', 'Message', 'ContentPlaceHolder1_ddlProductType');
//                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlProductType').offset().top }, 'slow');
//                return false;
//            }
            return true;
        }
        //        function PrintBarcode() {
        //            if (document.getElementById('divBarcodePrint')) {
        //                var pri = document.getElementById("ifmcontentstoprint").contentWindow;
        //                pri.document.open();
        //                var contentAll = document.getElementById("divBarcodePrint").innerHTML;
        //                pri.document.write(contentAll);
        //                pri.document.close();
        //                pri.focus();
        //                pri.print();
        //            }
        //            return false;
        //        }

              function checkCount() {
                var myform = document.forms[0];
                var len = myform.elements.length;
                var count = 0;
                for (var i = 0; i < len; i++) {
                    if (myform.elements[i].type == 'radio') {
                        if (myform.elements[i].checked == true) {
                            count += 1;
                        }
                    }
                }
                if (count == 0) {
                    window.parent.jAlert('Select at least one Warehouse!', 'Message');
                    return false;
                }
                else {
                    return true;
                }
            }


        function PrintBarcode() {
            if (document.getElementById('divBarcodePrint')) {
                var BrowserName = navigator.appName.toString();
                if (BrowserName.toString().toLowerCase().indexOf('internet explorer') > -1) {
                    w = window.open('', 'msg', 'directories=no, location=no, menubar=no, status=yes,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=10,Width=10,left=0,top=0,visible=false,alwaysLowered=yes');
                    w.document.write(document.getElementById("divBarcodePrint").innerHTML);
                    w.document.close();
                    w.print();
                    w.close();
                }
                else {
                    var pri = document.getElementById("ifmcontentstoprint").contentWindow;
                    pri.document.open();
                    var contentAll = document.getElementById("divBarcodePrint").innerHTML;
                    pri.document.write(contentAll);
                    pri.document.close();
                    pri.focus();
                    pri.print();
                }
            }
            return false;
        }
    </script>
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
        function keyRestrictForInventory(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
                return true;
            return false;
        }

        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }

        function SetTotalWeight() {

            var totalWeight = 0;
            var Weight = 0;
            for (var i = 0; i < 20; i++) {
                if (document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_' + i)) {
                    if (document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_' + i).value == '') {
                        Weight = 0;
                    }
                    else {
                        Weight = parseInt(document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_' + i).value);
                    }
                    totalWeight += Weight;
                }
            }
            document.getElementById('ContentPlaceHolder1_grdWarehouse_lblTotal').innerHTML = totalWeight;
            document.getElementById('ContentPlaceHolder1_txtInventory').value = totalWeight;
        }
    </script>
    <style type="text/css">
        .slidingDiv
        {
            height: 300px;
            padding: 20px;
            margin-top: 10px;
        }
        .show_hide
        {
            display: block;
        }
         .divfloatingcss
        {
            bottom: 0;
            right: 0;
            position: fixed;
            width: 14%;
            margin-right: 43%;
            background-image: url("/Admin/images/title-bg-trans.png");
              -webkit-border-top-left-radius: 15px;
-webkit-border-top-right-radius: 15px;
-moz-border-radius-topleft: 15px;
-moz-border-radius-topright: 15px;
border-top-left-radius: 15px;
border-top-right-radius: 15px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function selecttab(obj) {
            if (obj.id == "mitem1") {
                document.getElementById("tab1").style.display = "block";

                document.getElementById("tab2").style.display = "none";
                document.getElementById("tab3").style.display = "none";

                document.getElementById("mitem1").src = "/App_Themes/<%=Page.Theme %>/images/required_hover.jpg";
                document.getElementById("mitem2").src = "/App_Themes/<%=Page.Theme %>/images/desired_regular.jpg";
                document.getElementById("mitem3").src = "/App_Themes/<%=Page.Theme %>/images/optional_regular.jpg";
            }
            else if (obj.id == "mitem2") {

                document.getElementById("tab2").style.display = "block";

                document.getElementById("tab1").style.display = "none";
                document.getElementById("tab3").style.display = "none";
                document.getElementById("mitem1").src = "/App_Themes/<%=Page.Theme %>/images/required_regular.jpg";
                document.getElementById("mitem2").src = "/App_Themes/<%=Page.Theme %>/images/desired_hover.jpg";
                document.getElementById("mitem3").src = "/App_Themes/<%=Page.Theme %>/images/optional_regular.jpg";
            }
            else if (obj.id == "mitem3") {

                document.getElementById("tab3").style.display = "block";

                document.getElementById("tab1").style.display = "none";
                document.getElementById("tab2").style.display = "none";
                document.getElementById("mitem1").src = "/App_Themes/<%=Page.Theme %>/images/required_regular.jpg";
                document.getElementById("mitem2").src = "/App_Themes/<%=Page.Theme %>/images/desired_regular.jpg";
                document.getElementById("mitem3").src = "/App_Themes/<%=Page.Theme %>/images/optional_hover.jpg";

            }

        }

        function selecttabbydefault() {

            document.getElementById("tab1").style.display = "block";
            document.getElementById("tab2").style.display = "none";
            document.getElementById("tab3").style.display = "none";
            document.getElementById("mitem1").src = "/App_Themes/<%=Page.Theme %>/images/required_hover.jpg";
            document.getElementById("mitem2").src = "/App_Themes/<%=Page.Theme %>/images/desired_regular.jpg";
            document.getElementById("mitem3").src = "/App_Themes/<%=Page.Theme %>/images/optional_regular.jpg";

            document.getElementById("ctl00_ContentPlaceHolder1_ImageButton1").click();
            return false;
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            $(".slidingDiv").show();
            $(".show_hide").show();

            $('.show_hide').click(function () {
                $(".slidingDiv").slideToggle();
            });

        });

        $(document).ready(function () {

            $(".slidingDivImage").show();
            $(".show_hideImage").show();

            $('.show_hideImage').click(function () {
                $(".slidingDivImage").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivSEO").show();
            $(".show_hideSEO").show();

            $('.show_hideSEO').click(function () {
                $(".slidingDivSEO").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivCategory").show();
            $(".show_hideCategory").show();

            $('.show_hideCategory').click(function () {
                $(".slidingDivCategory").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivDesc").show();
            $(".show_hideDesc").show();

            $('.show_hideDesc').click(function () {
                $(".slidingDivDesc").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivMainDiv").show();
            $(".show_hideMainDiv").show();

            $('.show_hideMainDiv').click(function () {
                $(".slidingDivMainDiv").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivProductDesc").show();
            $(".show_hideProductDesc").show();

            $('.show_hideProductDesc').click(function () {
                $(".slidingDivProductDesc").slideToggle();
            });
        });
        
    </script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_txtLaunchDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtReleaseDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtSaleStartDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtSaleEndDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtRebateStartDate1').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtRebateEndDate1').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtRebateStartDate2').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtRebateEndDate2').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtRestockDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        
    </script>
    <script type="text/javascript">
        var currmenu = 1;
        function tab(tab_num) {
            var tab_name = '';
            try {

                for (i = 1; i <= 8; i++) {
                    if (i == tab_num) {
                        if (i < 6) {
                            document.getElementById("tab1").style.display = "block";
                        }
                        else {
                            tab_name = "tab" + i;
                            document.getElementById(tab_name).style.display = "block";
                        }

                        var imgname = 'm' + i;
                        document.getElementById(imgname).src = "../images/" + imgname + "-hover.jpg";
                        currmenu = i;
                    }
                    else {
                        var imgname = 'm' + i;
                        document.getElementById(imgname).src = "../images/" + imgname + ".jpg";
                        if (tab_num > 5 && i > 5) {
                            tab_name = "tab" + i;
                            document.getElementById(tab_name).style.display = "none";
                            document.getElementById("tab1").style.display = "none";
                        }
                    }
                }
            }
            catch (err) {
                //alert(err.message + tab_name);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sm1" runat="server">
    </asp:ScriptManager>
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
    </div>
    <div class="content-row2">
        <table cellspacing="0" cellpadding="0" border="0" width="100%">
            <tbody>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table class="content-table" cellspacing="0" cellpadding="0" border="0" bgcolor="#FFFFFF"
                            width="100%">
                            <tbody>
                                <tr>
                                    <td class="border-td-sub">
                                        <table class="add-product" cellspacing="0" cellpadding="0" border="0" width="100%">
                                            <tbody>
                                                <tr style="width: 1126px;">
                                                    <th>
                                                        <div class="main-title-left">
                                                            <img class="img-left" title="Add/Edit Product" alt="Add/Edit Product" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                            <h2 style="padding-top: 3px;">
                                                                Add/Edit Amazon Product</h2>
                                                        </div>
                                                        <div class="main-title-right">
                                                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="ShowHideButton('imgMainDiv','tdMainDiv');">
                                                                <img id="imgMainDiv" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="tab_box2">
                                                            <uc1:Header runat="server" ID="head"></uc1:Header>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trAddNewProduct" visible="false">
                                                    <td align="center" class="border" valign="middle" style="width: 202px; text-align: center;
                                                        line-height: 30px; background-color: white;" colspan="2">
                                                        <asp:Label ID="pronotavailmsg" runat="server"></asp:Label>
                                                        <asp:ImageButton ID="btnCloneNewProduct" runat="server" ImageUrl="../images/add.jpg"
                                                            OnClick="btnCloneNewProduct_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0" id="tableproduct" runat="server"
                                                            width="100%">
                                                            <tr>
                                                                <td align="left">
                                                                    <table cellpadding="0" cellspacing="0" width="100%" style="float: left; margin-top: 15px;
                                                                        border: 1px solid #DFDFDF; border-bottom: 0px;">
                                                                        <tr id="trmenu" runat="server" class="altrow">
                                                                            <td>
                                                                                <img style="cursor: pointer;" onclick="selecttab(this)" src="/App_Themes/<%=Page.Theme %>/images/required_hover.jpg"
                                                                                    id="mitem1" name="mitem1" alt="" />
                                                                                <img style="cursor: pointer;" onclick="selecttab(this)" src="/App_Themes/<%=Page.Theme %>/images/desired_regular.jpg"
                                                                                    id="mitem2" name="mitem2" alt="" />
                                                                                <img style="cursor: pointer; display: none;" onclick="selecttab(this)" src="/App_Themes/<%=Page.Theme %>/images/optional_regular.jpg"
                                                                                    id="mitem3" name="mitem3" alt="" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <span id="msgid" runat="server" style="cursor: default; margin-left: 40%; text-align: center;
                                                                        color: Red; font-weight: bold;" visible="false">Your product dose not clone until
                                                                        click on save button</span>
                                                                    <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                                    <span style="float: right; margin: 10px;"><span class="star" align="right">*</span>Required
                                                                        Fields</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="center">
                                                                    <div id="tab1" style="float: left; width: 100%;">
                                                                        <table cellpadding="0" cellspacing="0" height="400" width="100%">
                                                                            <tr valign="top">
                                                                                <td id="tdMainDiv">
                                                                                    <div id="divMain" class="slidingDivMainDiv">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                                            <tr class="even-row">
                                                                                                <td>
                                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                                        <tr class="oddrow">
                                                                                                            <td width="12%" align="left">
                                                                                                                <span class="star">*</span>Title:
                                                                                                            </td>
                                                                                                            <td colspan="4" align="left">
                                                                                                                <asp:TextBox ID="txtTitle" Width="750px" runat="server" MaxLength="400" class="order-textfield"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow">
                                                                                                            <td width="12%" align="left">
                                                                                                                <span class="star">*</span>SKU:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:TextBox ID="txtSKU" runat="server" AutoCompleteType="Disabled" MaxLength="100"
                                                                                                                    class="order-textfield"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td width="58%" colspan="3" rowspan="4" align="left">
                                                                                                                <table width="500px" border="0" cellpadding="0" cellspacing="0" style="border: 1px solid #BCC0C1;">
                                                                                                                    <tr>
                                                                                                                        <th>
                                                                                                                            <div class="main-title-left">
                                                                                                                                <img class="img-left" title="Warehouses" alt="Warehouses" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                                                                <h2>
                                                                                                                                    Warehouses
                                                                                                                                </h2>
                                                                                                                            </div>
                                                                                                                            <div class="main-title-right">
                                                                                                                                <a href="javascript:void(0);" class="show_hideWarehouse" onclick="ShowHideButton('ImgAmazonWarehouses','tdAmazonWarehouses');">
                                                                                                                                    <img id="ImgAmazonWarehouses" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"
                                                                                                                                        class="minimize" title="Minimize" alt="Minimize" /></a>
                                                                                                                            </div>
                                                                                                                        </th>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td id="tdAmazonWarehouses" align="left">
                                                                                                                            <div id="div3" class="slidingDivWarehouse">
                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                    <tr>
                                                                                                                                        <td>
                                                                                                                                            <asp:GridView ID="grdWarehouse" AutoGenerateColumns="false" runat="server" BorderStyle="Solid"
                                                                                                                                                BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                                                                                                                Width="100%" DataKeyNames="WareHouseID" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                                                                                                                HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="false" PagerSettings-Mode="NumericFirstLast"
                                                                                                                                                AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True"
                                                                                                                                                EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                                                                                ViewStateMode="Enabled" ShowHeaderWhenEmpty="true" OnRowDataBound="grdWarehouse_RowDataBound"
                                                                                                                                                ShowFooter="True">
                                                                                                                                                <Columns>
                                                                                                                                                    <asp:TemplateField>
                                                                                                                                                        <HeaderTemplate>
                                                                                                                                                            Preferred&nbsp;Location</HeaderTemplate>
                                                                                                                                                        <ItemTemplate>
                                                                                                                                                            <asp:RadioButton ID="rdowarehouse" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)"
                                                                                                                                                                GroupName="WarehouseSelect" />
                                                                                                                                                            <asp:Label ID="lblPreferredLocation" runat="server" Visible="false" Text='<%#Bind("PreferredLocation") %>'></asp:Label>
                                                                                                                                                        </ItemTemplate>
                                                                                                                                                        <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                                                                                                                                                        <FooterTemplate>
                                                                                                                                                            &nbsp;
                                                                                                                                                        </FooterTemplate>
                                                                                                                                                        <FooterStyle HorizontalAlign="Right" CssClass="footerBorder" />
                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                    <asp:TemplateField HeaderText="Warehouse Name">
                                                                                                                                                        <ItemTemplate>
                                                                                                                                                            <asp:Label ID="lblWarehouse" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                                                                                                                                        </ItemTemplate>
                                                                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="80%"></HeaderStyle>
                                                                                                                                                        <ItemStyle HorizontalAlign="Left" Width="80%"></ItemStyle>
                                                                                                                                                        <FooterTemplate>
                                                                                                                                                            <b>Total Inventory:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                                                                                                                                                        </FooterTemplate>
                                                                                                                                                        <FooterStyle HorizontalAlign="Right" CssClass="footerBorder" />
                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                    <asp:TemplateField HeaderText="Inventory">
                                                                                                                                                        <ItemTemplate>
                                                                                                                                                            <asp:TextBox ID="txtInventory" CssClass="order-textfield" Style="width: 50px; text-align: center;"
                                                                                                                                                                runat="server" Text='<%#Bind("Inventory") %>' onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');SetTotalWeight();return ret;"
                                                                                                                                                                onblur="SetTotalWeight();" MaxLength="5" onkeyup="SetTotalWeight();"></asp:TextBox>
                                                                                                                                                        </ItemTemplate>
                                                                                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                                                                        <FooterTemplate>
                                                                                                                                                            <asp:Label ID="lblTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                                                                        </FooterTemplate>
                                                                                                                                                        <FooterStyle HorizontalAlign="Center" Font-Bold="true" CssClass="footerBorderinventory" />
                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                    <asp:TemplateField HeaderText="WarehouseID" Visible="false">
                                                                                                                                                        <ItemTemplate>
                                                                                                                                                            <asp:Label ID="lblWarehouseID" runat="server" Text='<%#Bind("WarehouseID") %>'></asp:Label>
                                                                                                                                                        </ItemTemplate>
                                                                                                                                                    </asp:TemplateField>
                                                                                                                                                </Columns>
                                                                                                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" BackColor="White" />
                                                                                                                                                <AlternatingRowStyle CssClass="altrow" BackColor="#FBFBFB" />
                                                                                                                                            </asp:GridView>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow">
                                                                                                            <td width="12%" align="left">
                                                                                                                <span class="star">*</span>Product ID Type :
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:DropDownList ID="ddlproductidtype" runat="server" CssClass="order-list" Width="120px">
                                                                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                                                                    <asp:ListItem Value="UPC" Selected="True">UPC</asp:ListItem>
                                                                                                                    <asp:ListItem Value="EAN">EAN</asp:ListItem>
                                                                                                                    <asp:ListItem Value="GTIN">GTIN</asp:ListItem>
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow">
                                                                                                            <td width="12%" align="left">
                                                                                                                <span class="star">*</span>Standard&nbsp;Product&nbsp;ID(UPC):
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <%-- <asp:TextBox ID="txtStandardProductID" runat="server" MaxLength="20" class="order-textfield"
                                                                                                                    onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>--%>
                                                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtStandardProductID" runat="server" MaxLength="12" class="order-textfield"
                                                                                                                                onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                        <td style="display: none">
                                                                                                                            <asp:LinkButton ID="lbtngetUPC" runat="server" OnClick="lbtngetUPC_Click" Visible="false">Get UPC</asp:LinkButton>&nbsp;
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="trBarcode" runat="server" visible="false">
                                                                                                            <td valign="top" style="padding-top: 32px;" align="left">
                                                                                                                Product Barcode :
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <a style="padding-left: 174px" href="javascript:void(0);" onclick="return PrintBarcode();">
                                                                                                                    Print Barcode</a>
                                                                                                                <br />
                                                                                                                <div id="divBarcodePrint">
                                                                                                                    <img alt="" id="imgOrderBarcode" runat="server" />
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow">
                                                                                                            <td width="12%" align="left">
                                                                                                                <span class="star">*</span>ItemPrice:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                $<asp:TextBox ID="txtItemPrice" runat="server" Style="width: 100px" MaxLength="8"
                                                                                                                    class="order-textfield" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                                                                Ex. (12.52)
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">*</span>SalesPrice:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                $<asp:TextBox ID="txtSalesPrice" runat="server" Style="width: 100px" MaxLength="8"
                                                                                                                    class="order-textfield" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                                                                Ex. (10.50)
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">&nbsp;</span>Our Cost:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                $<asp:TextBox ID="txtOurPrice" runat="server" Style="width: 100px" MaxLength="8"
                                                                                                                    CssClass="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                                Ex. (9.50)
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">*</span>Inventory:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:UpdatePanel runat="server" ID="inv">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:TextBox ID="txtInventory" runat="server" Style="width: 100px" class="order-textfield"></asp:TextBox></ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                                <%--&nbsp;<img alt="Low Level" title="Low Level"
                                                                                                                        src="../Images/bullet_red.png" id="ImgToggelInventory" runat="server" visible="false"
                                                                                                                        style="vertical-align: middle;" />--%>
                                                                                                                &nbsp;<span style="font-size: 28px; vertical-align: middle; color: Red; cursor: default;"
                                                                                                                    title="Low Level" id="ImgToggelInventory" runat="server" visible="false">• <font
                                                                                                                        style="font-size: 13px; vertical-align: middle; padding-bottom: 5px;">Low Level</font></span>
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow">
                                                                                                            <td width="12%" align="left">
                                                                                                                <span class="star">*</span>Weight:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:TextBox ID="txtWeight" runat="server" Style="width: 100px" MaxLength="10" class="order-textfield"
                                                                                                                    onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                                                                lbs. Ex. (12.52)
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow">
                                                                                                            <td width="12%" align="left">
                                                                                                                <span class="star">*</span>Manufacturer:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:DropDownList ID="ddlManufacture" runat="server" class="product-type">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow" style="display: none;">
                                                                                                            <td align="left">
                                                                                                                <span class="star">*</span>MFRPartNumber:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:TextBox ID="txtMFRPartNumber" runat="server" MaxLength="40" class="order-textfield"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <%--<tr class="oddrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">*</span>ProductType:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:DropDownList ID="ddlProductType" runat="server" CssClass="order-list">
                                                                                                                    <%-- <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                                                                        <asp:ListItem Value="BagCase">BagCase</asp:ListItem>
                                                                                                        <asp:ListItem Value="Binocular">Binocular</asp:ListItem>
                                                                                                        <asp:ListItem Value="BlankMedia">BlankMedia</asp:ListItem>
                                                                                                        <asp:ListItem Value="Camcorder">Camcorder</asp:ListItem>
                                                                                                        <asp:ListItem Value="Cleaner">Cleaner</asp:ListItem>
                                                                                                        <asp:ListItem Value="Darkroom">Darkroom</asp:ListItem>
                                                                                                        <asp:ListItem Value="DigitalCamera">DigitalCamera</asp:ListItem>
                                                                                                        <asp:ListItem Value="Film">Film</asp:ListItem>
                                                                                                        <asp:ListItem Value="FilmCamera">FilmCamera</asp:ListItem>
                                                                                                        <asp:ListItem Value="Filter">Filter</asp:ListItem>
                                                                                                        <asp:ListItem Value="Flash">Flash</asp:ListItem>
                                                                                                        <asp:ListItem Value="Lens">Lens</asp:ListItem>
                                                                                                        <asp:ListItem Value="LensAccessory">LensAccessory</asp:ListItem>
                                                                                                        <asp:ListItem Value="LightMeter">LightMeter</asp:ListItem>
                                                                                                        <asp:ListItem Value="Lighting">Lighting</asp:ListItem>
                                                                                                        <asp:ListItem Value="Microscope">Microscope</asp:ListItem>
                                                                                                        <asp:ListItem Value="OtherAccessory">OtherAccessory</asp:ListItem>
                                                                                                        <asp:ListItem Value="PhotoPaper">PhotoPaper</asp:ListItem>
                                                                                                        <asp:ListItem Value="PhotoStudio">PhotoStudio</asp:ListItem>
                                                                                                        <asp:ListItem Value="PowerSupply">PowerSupply</asp:ListItem>
                                                                                                        <asp:ListItem Value="Projection">Projection</asp:ListItem>
                                                                                                        <asp:ListItem Value="SurveillanceSystem">SurveillanceSystem</asp:ListItem>
                                                                                                        <asp:ListItem Value="Telescope">Telescope</asp:ListItem>
                                                                                                        <asp:ListItem Value="TripodStand">TripodStand</asp:ListItem>
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>--%>
                                                                                                        <tr class="altrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">*</span>Material:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:DropDownList ID="txtItemType" runat="server" AutoPostBack="false" CssClass="product-type">
                                                                                                                    <asp:ListItem Value="">- Select -</asp:ListItem>
                                                                                                                     <asp:ListItem Value="100% Cotton"> 100% Cotton</asp:ListItem>
                                                                                                                     <asp:ListItem Value="100% Polyester"> 100% Polyester</asp:ListItem>
                                                                                                                    <asp:ListItem Value="18/10 Steel"> 18/10 Steel</asp:ListItem>
                                                                                                                    <asp:ListItem Value="18/8 Steel"> 18/8 Steel</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Acrylic"> Acrylic</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Aluminum"> Aluminum</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Bamboo"> Bamboo</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Beechwood"> Beechwood</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Birch"> Birch</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Bone China"> Bone China</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Brass"> Brass</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Bronze"> Bronze</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Carbon"> Carbon</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Cast Iron"> Cast Iron</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Cedar"> Cedar</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Ceramic"> Ceramic</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Cherrywood"> Cherrywood</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Chrome"> Chrome</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Chromium Steel"> Chromium Steel</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Clay"> Clay</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Copper"> Copper</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Cork"> Cork</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Cotton"> Cotton</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Crystal"> Crystal</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Earthenware"> Earthenware</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Elmwood"> Elmwood</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Fabric"> Fabric</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Gilded Gold"> Gilded Gold</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Glass"> Glass</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Hard-Anodized Aluminum"> Hard-Anodized Aluminum</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Hardwood"> Hardwood</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Iron"> Iron</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Leaded Crystal"> Leaded Crystal </asp:ListItem>
                                                                                                                     <asp:ListItem Value="Linen & Polester Blend"> Linen & Polester Blend </asp:ListItem>
                                                                                                                    

                                                                                                                    <asp:ListItem Value="Leather"> Leather</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Mahogany"> Mahogany</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Maple"> Maple</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Marble"> Marble</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Melamine"> Melamine</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Multi-Ply"> Multi-Ply</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Neoprene"> Neoprene</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Nickel"> Nickel</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Non-Leaded Crystal"> Non-Leaded Crystal </asp:ListItem>
                                                                                                                    <asp:ListItem Value="Nonstick"> Nonstick</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Nylon"> Nylon</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Oak"> Oak</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Olive Wood"> Olive Wood</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Paper"> Paper</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Pewter"> Pewter</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Pine"> Pine</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Plastic"> Plastic</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Platinum"> Platinum</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Polycarbonate"> Polycarbonate</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Polyester"> Polyester</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Porcelain"> Porcelain</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Porcelain Bone China"> Porcelain Bone China</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Rattan & Wicker"> Rattan & Wicker</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Recylced"> Recylced </asp:ListItem>
                                                                                                                    <asp:ListItem Value="Rosewood"> Rosewood</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Rubber"> Rubber</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Silicone"> Silicone</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Silver"> Silver</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Silver-Plated"> Silver-Plated</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Solid Gold"> Solid Gold</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Stainless Steel"> Stainless Steel</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Steel"> Steel</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Stone"> Stone</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Stoneware"> Stoneware</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Synthetic"> Synthetic</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Teak"> Teak</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Terracotta"> Terracotta</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Tin"> Tin</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Tritan"> Tritan</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Walnut"> Walnut</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Zinc"> Zinc</asp:ListItem>
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">&nbsp;</span>Color Map:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:DropDownList ID="ddlColormap" runat="server" AutoPostBack="false" CssClass="product-type">
                                                                                                                    <asp:ListItem Value="">- Select -</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Aqua"> Aqua</asp:ListItem>
                                                                                                                    
                                                                                                                    <asp:ListItem Value="Beige"> Beige</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Black"> Black</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Blue"> Blue</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Brown"> Brown</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Cream"> Cream</asp:ListItem>
                                                                                                                     <asp:ListItem Value="Ecru"> Ecru</asp:ListItem>
                                                                                                                     <asp:ListItem Value="Eggshell"> Eggshell</asp:ListItem>
                                                                                                                      
                                                                                                                    <asp:ListItem Value="Clear"> Clear</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Gold"> Gold</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Green"> Green</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Grey"> Grey</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Ivory"> Ivory</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Multi"> Multi</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Natural"> Natural</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Navy"> Navy</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Oatmeal"> Oatmeal</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Off White"> Off White</asp:ListItem>
                                                                                                                    
                                                                                                                    <asp:ListItem Value="Orange"> Orange</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Other"> Other</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Pink"> Pink</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Purple"> Purple</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Red"> Red</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Rust"> Rust</asp:ListItem>
                                                                                                                     <asp:ListItem Value="Sand"> Sand</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Silver"> Silver</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Stone"> Stone</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Taupe"> Taupe</asp:ListItem>
                                                                                                                    <asp:ListItem Value="White"> White</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Yellow"> Yellow</asp:ListItem>
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">&nbsp;</span>Design:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:DropDownList ID="ddlDesign" runat="server" AutoPostBack="false" CssClass="product-type">
                                                                                                                    <asp:ListItem Value="">- Select -</asp:ListItem>
                                                                                                                    <asp:ListItem Value="Animal Print">Animal Print</asp:ListItem>
<asp:ListItem Value="Art Deco">Art Deco</asp:ListItem>
<asp:ListItem Value="Asian">Asian</asp:ListItem>
<asp:ListItem Value="Banded">Banded</asp:ListItem>
<asp:ListItem Value="Childrens">Childrens</asp:ListItem>
<asp:ListItem Value="Contemporary">Contemporary</asp:ListItem>
<asp:ListItem Value="Country">Country</asp:ListItem>
<asp:ListItem Value="Crystal">Crystal</asp:ListItem>
<asp:ListItem Value="Early American">Early American</asp:ListItem>
<asp:ListItem Value="Floral">Floral</asp:ListItem>
<asp:ListItem Value="Geometric">Geometric</asp:ListItem>
<asp:ListItem Value="Mission">Mission</asp:ListItem>
<asp:ListItem Value="Natural Fiber">Natural Fiber</asp:ListItem>
<asp:ListItem Value="Novelty/Themed">Novelty/Themed</asp:ListItem>
<asp:ListItem Value="Old World">Old World</asp:ListItem>
<asp:ListItem Value="Pattern">Pattern</asp:ListItem>
<asp:ListItem Value="Patterned">Patterned</asp:ListItem>
<asp:ListItem Value="Retro">Retro</asp:ListItem>
<asp:ListItem Value="Rustic/Lodge">Rustic/Lodge</asp:ListItem>
<asp:ListItem Value="Seasonal">Seasonal</asp:ListItem>
<asp:ListItem Value="Shag/Flokati">Shag/Flokati</asp:ListItem>
<asp:ListItem Value="Solid">Solid</asp:ListItem>
<asp:ListItem Value="Southwestern">Southwestern</asp:ListItem>
<asp:ListItem Value="Special">Special</asp:ListItem>
<asp:ListItem Value="Sports">Sports</asp:ListItem>
<asp:ListItem Value="Sports/Collegiate">Sports/Collegiate</asp:ListItem>
<asp:ListItem Value="Sports/Professional">Sports/Professional</asp:ListItem>
<asp:ListItem Value="Striped">Striped</asp:ListItem>
<asp:ListItem Value="Tiffany">Tiffany</asp:ListItem>
<asp:ListItem Value="Traditional">Traditional</asp:ListItem>
<asp:ListItem Value="Transitional">Transitional</asp:ListItem>
<asp:ListItem Value="Tropical">Tropical</asp:ListItem>
<asp:ListItem Value="Two-toned">Two-toned</asp:ListItem>
<asp:ListItem Value="Victorian">Victorian</asp:ListItem>
<asp:ListItem Value="Whimsical">Whimsical</asp:ListItem>


                                                                                                                    
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow">
                                                                                                            <td width="15%" align="left">
                                                                                                                <span class="star">*</span>Brand:
                                                                                                            </td>
                                                                                                            <td width="85%" align="left">
                                                                                                                <asp:TextBox ID="txtBrand" runat="server" MaxLength="50" CssClass="order-textfield"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">*</span>Fulfillment&nbsp;Center&nbsp;ID&nbsp;:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:DropDownList ID="ddlFulfillment" runat="server" CssClass="order-list" Width="120px">
                                                                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                                                                    <asp:ListItem Value="AMAZON_NA" Selected="True">AMAZON_NA</asp:ListItem>
                                                                                                                    <asp:ListItem Value="DEFAULT">DEFAULT</asp:ListItem>
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow">
                                                                                                            <td align="left">
                                                                                                                <span class="star">*</span>Currency:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:TextBox ID="txtCurrency" CssClass="order-textfield" runat="server" MaxLength="50"
                                                                                                                    Text="USD"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="altrow" align="left">
                                                                                                                <span class="star">&nbsp;&nbsp;</span>Status:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:CheckBox ID="chkPublished" runat="server" Checked="true" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="display:none;">
                                                                                                            <td class="oddrow" align="left">
                                                                                                                <span class="star">&nbsp;</span> Is Hemming:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                               <table>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                           <asp:CheckBox ID="chkIsHamming" runat="server" onchange="return SetHammingValue();" TabIndex="16" />
                                                                                                                        </td>
                                                                                                                        <td id="tdHamminglbl" runat="server" style="display:none;">
                                                                                                                           &nbsp;<span class="star">*</span>Hemming(%)
                                                                                                                        </td>
                                                                                                                        <td id="tdHammingQty" runat="server" style="display:none;">
                                                                                                                            <asp:TextBox ID="txtHammingQty" CssClass="order-textfield" Style="width: 50px; text-align: center;"
                                                                                                                                              runat="server" onkeypress="var ret=keyRestrictForInventory(event,'0123456789.');return ret;"
                                                                                                                                              MaxLength="5" TabIndex="7" ></asp:TextBox>
                                                                                                                         </td>
                                                                                                                      </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="altrow">
                                                                                                            <td class="oddrow" align="left">
                                                                                                                <span class="star">&nbsp;&nbsp;</span>Is Discontinue:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:CheckBox ID="chkIsDiscontinue" runat="server" Checked="false" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow" style="display: none;">
                                                                                                            <td width="12%">
                                                                                                                <span class="star"></span>Stock/Dropship:
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:UpdateProgress ID="upgrProductTypeDelivery" runat="server" AssociatedUpdatePanelID="upProductTypeDelivery">
                                                                                                                    <ProgressTemplate>
                                                                                                                        <div style="position: relative;">
                                                                                                                            <div style="position: absolute; bottom: -27px; left: 40%;">
                                                                                                                                <img alt="" src="../images/ProductLoader.gif" />
                                                                                                                                <b>Loading ... ... Please wait!</b>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </ProgressTemplate>
                                                                                                                </asp:UpdateProgress>
                                                                                                                <asp:UpdatePanel ID="upProductTypeDelivery" runat="server" UpdateMode="Always">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:DropDownList ID="ddlProductTypeDelivery" AutoPostBack="true" runat="server"
                                                                                                                            class="product-type" OnSelectedIndexChanged="ddlProductTypeDelivery_SelectedIndexChanged">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                            <td width="12%">
                                                                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                                                                    <ContentTemplate>
                                                                                                                        <div id="divvendor" runat="server" visible="false">
                                                                                                                            <span class="star"></span>Drop Shipper/Vendor :
                                                                                                                        </div>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                            <td width="12%">
                                                                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:DropDownList ID="ddlvendor" runat="server" class="product-type" AutoPostBack="true"
                                                                                                                            Visible="false">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="oddrow" style="display: none;">
                                                                                                            <td width="12%">
                                                                                                                <span class="star">*</span>Product&nbsp;Type:
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:UpdateProgress ID="upgrProductType" runat="server" AssociatedUpdatePanelID="upProductType">
                                                                                                                    <ProgressTemplate>
                                                                                                                        <div style="position: relative;">
                                                                                                                            <div style="position: absolute; bottom: -27px; left: 40%;">
                                                                                                                                <img alt="" src="../images/ProductLoader.gif" />
                                                                                                                                <b>Loading ... ... Please wait!</b>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </ProgressTemplate>
                                                                                                                </asp:UpdateProgress>
                                                                                                                <asp:UpdatePanel ID="upProductType" runat="server" UpdateMode="Always">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:DropDownList ID="ddlProductType" AutoPostBack="true" runat="server" class="product-type"
                                                                                                                            OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="display: none;">
                                                                                                            <td>
                                                                                                                <%-- <asp:ImageButton ID="ibtnFeaturecategory" runat="server" OnClientClick="openCenteredCrossSaleWindow1('ContentPlaceHolder1_hdnvendorAllSku');" />--%>
                                                                                                            </td>
                                                                                                            <td colspan="3">
                                                                                                                <div style="display: none;">
                                                                                                                    <asp:Button ID="btnvendorlist" runat="server" Text="Savesadfasd" OnClick="btnvendorlist_click" /></div>
                                                                                                                <asp:UpdatePanel ID="updategrdDropShip" runat="server">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:HiddenField ID="hdnvendorAllSku" runat="server" />
                                                                                                                        <asp:HiddenField ID="hdnProductALLSku" runat="server" />
                                                                                                                        <asp:GridView ID="grdDropShip" runat="server" AutoGenerateColumns="false" OnRowDataBound="grdDropShip_RowDataBound"
                                                                                                                            BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                                                                                            GridLines="None" HeaderStyle-ForeColor="White" ShowFooter="true" Width="80%"
                                                                                                                            OnRowCommand="grdDropShip_RowCommand" OnRowCancelingEdit="cancelRecord" OnRowEditing="editRecord">
                                                                                                                            <EmptyDataTemplate>
                                                                                                                                <table width="100%" cellpadding="2" cellspacing="1" style="background-color: White;">
                                                                                                                                    <tr>
                                                                                                                                        <th style="color: White">
                                                                                                                                            Dropshipper Name
                                                                                                                                        </th>
                                                                                                                                        <th style="color: White">
                                                                                                                                            Dropshipper Product Name
                                                                                                                                        </th>
                                                                                                                                        <th style="color: White">
                                                                                                                                            SKU
                                                                                                                                        </th>
                                                                                                                                        <th style="color: White">
                                                                                                                                            Priority
                                                                                                                                        </th>
                                                                                                                                        <th style="color: White">
                                                                                                                                            Operations
                                                                                                                                        </th>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td colspan="5" align="right">
                                                                                                                                            <asp:ImageButton ID="ibtnFeaturecategory" runat="server" OnClientClick="return openCenteredCrossSaleWindow1('ContentPlaceHolder1_hdnvendorAllSku');" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </EmptyDataTemplate>
                                                                                                                            <Columns>
                                                                                                                                <asp:TemplateField HeaderText="VendorID" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <%--<asp:HiddenField ID="hdnVendorSKUID" runat="server" Value= '<%# Convert.ToInt32( DataBinder.Eval(Container.DataItem,"VendorSKUID")) %>'/>--%>
                                                                                                                                        <asp:Label ID="lblVendorSKUID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VendorSKUID") %>'
                                                                                                                                            Visible="false"></asp:Label>
                                                                                                                                        <asp:Label ID="lblVendorID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VendorID") %>'
                                                                                                                                            Visible="false"></asp:Label>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="Dropshipper Name" HeaderStyle-HorizontalAlign="Left">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblDropshipperName" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="Dropshipper Product Name" HeaderStyle-HorizontalAlign="Left">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblProductName" runat="server" Text='<%#Bind("ProductName") %>'></asp:Label>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="SKU" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblVendorSKU" runat="server" Text='<%#Bind("VendorSKU") %>'></asp:Label>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="Priority" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblPriority" runat="server" Text='<%#Bind("Priority") %>'> </asp:Label>
                                                                                                                                        <%--<asp:TextBox ID="txtPriority" runat="server" Text="0" Width="20px"></asp:TextBox>--%>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <EditItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtPriority" runat="server" Visible="True" Width="40px" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                            Text='<%#Bind("Priority") %>' MaxLength="2" Style="text-align: center;"></asp:TextBox>
                                                                                                                                    </EditItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="Operations">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                                                                                            CommandArgument='<%# Eval("VendorSKUID") %>' ImageUrl="~/App_Themes/Gray/images/edit-icon.png">
                                                                                                                                        </asp:ImageButton>
                                                                                                                                        <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="del"
                                                                                                                                            CommandArgument='<%# Eval("VendorSKUID") %>' message='<%# Eval("VendorSKUID") %>'
                                                                                                                                            OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){return true;}else{return false;}"
                                                                                                                                            ImageUrl="~/App_Themes/Gray/images/delete-icon.png"></asp:ImageButton>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <EditItemTemplate>
                                                                                                                                        <asp:ImageButton ID="btnSave" runat="server" Visible="true" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VendorSKUID") %>'
                                                                                                                                            CommandName="Save" AlternateText="Save" ImageUrl="~/App_Themes/Gray/images/save.png" />
                                                                                                                                        <asp:ImageButton ID="btnCancel" runat="server" Visible="true" CommandName="Cancel"
                                                                                                                                            ImageUrl="~/App_Themes/Gray/images/CloseIcon.png" Height="16px" Width="16px"
                                                                                                                                            AlternateText="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VendorSKUID") %>' />
                                                                                                                                    </EditItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:ImageButton ID="ibtnFeaturecategory" runat="server" OnClientClick="return openCenteredCrossSaleWindow1('ContentPlaceHolder1_hdnvendorAllSku');" />
                                                                                                                                    </FooterTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                            </Columns>
                                                                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" BackColor="White" />
                                                                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                                        </asp:GridView>
                                                                                                                        <asp:GridView ID="grdAssembler" runat="server" AutoGenerateColumns="false" Visible="false"
                                                                                                                            OnRowDataBound="grdAssembler_RowDataBound" BorderStyle="Solid" BorderColor="#E7E7E7"
                                                                                                                            BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None" HeaderStyle-ForeColor="White"
                                                                                                                            ShowFooter="true" Width="60%" OnRowCommand="grdAssembler_RowCommand" OnRowCancelingEdit="cancelProduct"
                                                                                                                            OnRowEditing="editProduct">
                                                                                                                            <EmptyDataTemplate>
                                                                                                                                <table width="100%" cellpadding="2" cellspacing="1" style="background-color: White;">
                                                                                                                                    <tr>
                                                                                                                                        <th style="color: White">
                                                                                                                                            Product Name
                                                                                                                                        </th>
                                                                                                                                        <th style="color: White">
                                                                                                                                            SKU
                                                                                                                                        </th>
                                                                                                                                        <th style="color: White">
                                                                                                                                            Quantity
                                                                                                                                        </th>
                                                                                                                                        <th style="color: White">
                                                                                                                                            Operations
                                                                                                                                        </th>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td colspan="4" align="right">
                                                                                                                                            <asp:ImageButton ID="ibtnFeatureProduct" runat="server" OnClientClick="return openCenteredCrossSaleWindow2('ContentPlaceHolder1_hdnProductALLSku');" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </EmptyDataTemplate>
                                                                                                                            <Columns>
                                                                                                                                <asp:TemplateField HeaderText="VendorID" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <%--<asp:HiddenField ID="hdnVendorSKUID" runat="server" Value= '<%# Convert.ToInt32( DataBinder.Eval(Container.DataItem,"VendorSKUID")) %>'/>--%>
                                                                                                                                        <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                                                                                                                            Visible="false"></asp:Label>
                                                                                                                                        <%--  <asp:Label ID="lblVendorID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VendorID") %>'
                                                                                                                Visible="false"></asp:Label>--%>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="Product Name" HeaderStyle-HorizontalAlign="Left">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblProductName" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="SKU" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblProductSKU" runat="server" Text='<%#Bind("SKU") %>'></asp:Label>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>'> </asp:Label>
                                                                                                                                        <%--<asp:TextBox ID="txtPriority" runat="server" Text="0" Width="20px"></asp:TextBox>--%>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <EditItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtQuantity" runat="server" Visible="True" Width="40px" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                            Text='<%#Bind("Quantity") %>' MaxLength="5" Style="text-align: center;"></asp:TextBox>
                                                                                                                                    </EditItemTemplate>
                                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="Operations">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                                                                                            CommandArgument='<%# Eval("ProductID") %>' ImageUrl="~/App_Themes/Gray/images/edit-icon.png">
                                                                                                                                        </asp:ImageButton>
                                                                                                                                        <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="del"
                                                                                                                                            CommandArgument='<%# Eval("ProductID") %>' message='<%# Eval("ProductID") %>'
                                                                                                                                            OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){return true;}else{return false;}"
                                                                                                                                            ImageUrl="~/App_Themes/Gray/images/delete-icon.png"></asp:ImageButton>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <EditItemTemplate>
                                                                                                                                        <asp:ImageButton ID="btnSave" runat="server" Visible="true" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                                                                                                                            CommandName="Save" AlternateText="Save" ImageUrl="~/App_Themes/Gray/images/save.png" />
                                                                                                                                        <asp:ImageButton ID="btnCancel" runat="server" Visible="true" CommandName="Cancel"
                                                                                                                                            ImageUrl="~/App_Themes/Gray/images/CloseIcon.png" Height="16px" Width="16px"
                                                                                                                                            AlternateText="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                                                                                                                    </EditItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:ImageButton ID="ibtnFeatureProduct" runat="server" OnClientClick="return openCenteredCrossSaleWindow2('ContentPlaceHolder1_hdnProductALLSku');" />
                                                                                                                                    </FooterTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                            </Columns>
                                                                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" BackColor="White" />
                                                                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                                        </asp:GridView>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td width="60%" valign="top">
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <th>
                                                                                                                            <div class="main-title-left">
                                                                                                                                <img class="img-left" title="Description" alt="Description" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                                                                <h2>
                                                                                                                                    Description</h2>
                                                                                                                            </div>
                                                                                                                            <div class="main-title-right">
                                                                                                                                <a href="javascript:void(0);" class="show_hideProductDesc" onclick="return ShowHideButton('imgDescription','tdDescription');">
                                                                                                                                    <img id="imgDescription" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                                        title="Minimize" alt="Minimize" /></a>
                                                                                                                            </div>
                                                                                                                        </th>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td id="tdDescription">
                                                                                                                            <div id="tab-container-1" class="slidingDivProductDesc">
                                                                                                                                <ul class="menu">
                                                                                                                                    <li id="product-Description" class="active">Detail Description</li>
                                                                                                                                </ul>
                                                                                                                                <span class="clear"></span>
                                                                                                                                <div class="tab-content product-Description">
                                                                                                                                    <div class="tab-content-3">
                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                            <tr style="display: none;">
                                                                                                                                                <td>
                                                                                                                                                    <div id="divDescription" runat="Server" visible="false">
                                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
                                                                                                                                                            <tr>
                                                                                                                                                                <td width="144px">
                                                                                                                                                                    <asp:DropDownList ID="ddlDescription" runat="server" CssClass="order-list" AutoPostBack="True"
                                                                                                                                                                        OnSelectedIndexChanged="ddlDescription_SelectedIndexChanged" Width="142px">
                                                                                                                                                                        <asp:ListItem Text="Description Tab : 01" Value="1"></asp:ListItem>
                                                                                                                                                                        <asp:ListItem Text="Description Tab : 02" Value="2"></asp:ListItem>
                                                                                                                                                                        <asp:ListItem Text="Description Tab : 03" Value="3"></asp:ListItem>
                                                                                                                                                                        <asp:ListItem Text="Description Tab : 04" Value="4"></asp:ListItem>
                                                                                                                                                                        <asp:ListItem Text="Description Tab : 05" Value="5"></asp:ListItem>
                                                                                                                                                                    </asp:DropDownList>
                                                                                                                                                                </td>
                                                                                                                                                                <td style="text-align: left">
                                                                                                                                                                    Tab Title:
                                                                                                                                                                    <asp:TextBox ID="txtTitleDesc" runat="server" class="order-textfield" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;
                                                                                                                                                                    <asp:LinkButton ID="lnkSaveDesc" runat="server" Font-Bold="true" Font-Size="12px"
                                                                                                                                                                        Font-Underline="true" Text="Save Description">Save Description</asp:LinkButton>
                                                                                                                                                                </td>
                                                                                                                                                                <td width="20%" align="left">
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                            <tr style="display: none;">
                                                                                                                                                                <td>
                                                                                                                                                                    <span>Is Tabbing Display</span>
                                                                                                                                                                    <asp:CheckBox ID="chkIsTabbingDisplay" Checked="true" runat="server" />
                                                                                                                                                                </td>
                                                                                                                                                                <td>
                                                                                                                                                                </td>
                                                                                                                                                                <td>
                                                                                                                                                                    &nbsp;
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                        </table>
                                                                                                                                                    </div>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                                        <tr>
                                                                                                                                                            <td class="ckeditor-table">
                                                                                                                                                                <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                                                                                                    Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                                                                                                <script type="text/javascript">
                                                                                                                                                                    CKEDITOR.replace('<%= txtDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                                                                                                                                    CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                                                                                                        if (ev.data.name == 'image') {
                                                                                                                                                                            var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                            btn.hidden = false;
                                                                                                                                                                            btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                        }
                                                                                                                                                                        if (ev.data.name == 'link') {
                                                                                                                                                                            var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                            btn.hidden = false;
                                                                                                                                                                            btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                        }
                                                                                                                                                                    });
                                                                                                                                                                </script>
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    &nbsp;
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </table>
                                                                                                                                    </div>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <th>
                                                                                                                            <div class="main-title-left">
                                                                                                                                <img class="img-left" title="Images" alt="Images" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                                <h2>
                                                                                                                                    Images</h2>
                                                                                                                            </div>
                                                                                                                            <div class="main-title-right">
                                                                                                                                <a href="javascript:void(0);" class="show_hideImage" title="Close" onclick="return ShowHideButton('ImgImages','tdImages');">
                                                                                                                                    <img id="ImgImages" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                                        title="Minimize" alt="Minimize"></a></div>
                                                                                                                        </th>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td id="tdImages">
                                                                                                                            <div id="divImage" class="slidingDivImage">
                                                                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                                    <tr>
                                                                                                                                        <td valign="middle">
                                                                                                                                            <img alt="Upload" id="ImgLarge" runat="server" width="150" style="margin-bottom: 5px;
                                                                                                                                                border: 1px solid darkgray" /><br />
                                                                                                                                        </td>
                                                                                                                                        <td valign="bottom" style="padding: 0px 2px;">
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td>
                                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                <tr>
                                                                                                                                                    <td width="10%">
                                                                                                                                                        <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a;
                                                                                                                                                            background: #f5f5f5; color: #000000;" />
                                                                                                                                                    </td>
                                                                                                                                                    <td width="9%">
                                                                                                                                                        <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click1" />
                                                                                                                                                    </td>
                                                                                                                                                    <td width="64%">
                                                                                                                                                        <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                            OnClick="btnDelete_Click" />
                                                                                                                                                    </td>
                                                                                                                                                    <td align="left">
                                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" id="tduploadMoreImages"
                                                                                                                                                            runat="server" visible="false">
                                                                                                                                                            <tr>
                                                                                                                                                                <td>
                                                                                                                                                                    <a target="content" class="list_lin" style="cursor: pointer;" onclick="JavaScript:OpenMoreImagesPopup();">
                                                                                                                                                                        <img src="/App_Themes/<%=Page.Theme %>/images/Upload-More-Images.png" title="Upload More Images " />
                                                                                                                                                                    </a>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                        </table>
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>
                                                                                                                                        </td>
                                                                                                                                        <td id="tduploadPdf" runat="server" visible="false">
                                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                <tr>
                                                                                                                                                    <td width="10%">
                                                                                                                                                    </td>
                                                                                                                                                    <td width="9%">
                                                                                                                                                    </td>
                                                                                                                                                    <td width="64%">
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td width="40%" valign="top">
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="display: none;">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                                                                                <tr>
                                                                                                                    <th>
                                                                                                                        <div class="main-title-left">
                                                                                                                            <img class="img-left" title="Categories" alt="Categories" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                            <h2>
                                                                                                                                Categories
                                                                                                                            </h2>
                                                                                                                        </div>
                                                                                                                        <div class="main-title-right">
                                                                                                                            <a href="javascript:void(0);" class="show_hideCategory" onclick="return ShowHideButton('ImgCategories','tdCategories');">
                                                                                                                                <img id="ImgCategories" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                                    title="Minimize" alt="Minimize"></a>
                                                                                                                        </div>
                                                                                                                    </th>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td id="tdCategories">
                                                                                                                        <div id="div1" class="slidingDivCategory">
                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                <tr style="display: none;">
                                                                                                                                    <td width="18%">
                                                                                                                                        Main&nbsp;Category:
                                                                                                                                    </td>
                                                                                                                                    <td width="82%">
                                                                                                                                        <asp:TextBox ID="txtMaincategory" runat="server" CssClass="order-textfield" MaxLength="100"
                                                                                                                                            Height="19px" Width="290px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td width="18%" valign="top" style="padding: 10px;">
                                                                                                                                        <span class="star">&nbsp;</span>Select&nbsp;Category:
                                                                                                                                    </td>
                                                                                                                                    <td width="82%" align="left" class="list_table_cell_link " id="TDCategory" runat="server"
                                                                                                                                        style="height: 163px; padding-top: 3px" valign="top">
                                                                                                                                        <div id="divTrvCategories">
                                                                                                                                            <asp:TreeView ID="trvCategories" runat="server" ShowCheckBoxes="Leaf" ForeColor="#212121"
                                                                                                                                                Font-Size="12px" Width="304px" PopulateNodesFromClient="True" ShowLines="true">
                                                                                                                                            </asp:TreeView>
                                                                                                                                            <input id="treeCatID" runat="server" type="hidden" value="0" />
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="tab2" style="display: none;">
                                                                        <table cellpadding="0" cellspacing="0" height="400" width="100%">
                                                                            <tr valign="top">
                                                                                <td style="width: 100%">
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                                        <tr class="even-row" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Model Name:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtModelName" runat="server" MaxLength="50" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Model Number:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtModelNumber" runat="server" MaxLength="50" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Merchant Catalog Number:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtMerchantCatalogNumber" runat="server" MaxLength="40" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%" align="left">
                                                                                                Bullet Point1:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtBulletPoint1" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%" align="left">
                                                                                                Bullet Point2:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtBulletPoint2" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%" align="left">
                                                                                                Bullet Point3:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtBulletPoint3" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%" align="left">
                                                                                                Bullet Point4:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtBulletPoint4" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%" align="left">
                                                                                                Bullet Point5:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtBulletPoint5" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Search Terms1:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtPlatinumKeywords1" runat="server" MaxLength="50" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Search Terms2:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtPlatinumKeywords2" runat="server" MaxLength="50" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Search Terms3:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtPlatinumKeywords3" runat="server" MaxLength="50" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Search Terms4:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtPlatinumKeywords4" runat="server" MaxLength="50" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Search Terms5:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtPlatinumKeywords5" runat="server" MaxLength="50" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Product Description:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtProductDescription" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Keyword1:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtKeyword1" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Keyword2:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtKeyword2" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Keyword3:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtKeyword3" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Main Image URL:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtMainImageURL" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Condition Type:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:DropDownList ID="ddlConditionType" runat="server" CssClass="order-list" Width="120px">
                                                                                                    <asp:ListItem Text="Select Option" Value=""></asp:ListItem>
                                                                                                    <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Refurbished" Value="Refurbished"></asp:ListItem>
                                                                                                    <asp:ListItem Text="UsedLikeNew" Value="UsedLikeNew"></asp:ListItem>
                                                                                                    <asp:ListItem Text="UsedVeryGood" Value="UsedVeryGood"></asp:ListItem>
                                                                                                    <asp:ListItem Text="UsedGood" Value="UsedGood"></asp:ListItem>
                                                                                                    <asp:ListItem Text="UsedAcceptable" Value="UsedAcceptable"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                Condition Note:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtConditionNote" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow" style="display: none;">
                                                                                            <td width="15%" align="left">
                                                                                                shipping weight unit of measure:
                                                                                            </td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:DropDownList ID="ddlShippingWeightUnitOfMeasure" runat="server" CssClass="order-list"
                                                                                                    Width="120px">
                                                                                                    <asp:ListItem Text="Select Option" Value=""></asp:ListItem>
                                                                                                    <asp:ListItem Text="GR" Value="GR"></asp:ListItem>
                                                                                                    <asp:ListItem Text="KG" Value="KG"></asp:ListItem>
                                                                                                    <asp:ListItem Text="OZ" Value="OZ"></asp:ListItem>
                                                                                                    <asp:ListItem Text="LB" Value="LB"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="tab3" style="display: none;">
                                                                        <table cellpadding="0" cellspacing="0" height="400px" width="100%">
                                                                            <tr valign="top">
                                                                                <td style="width: 100%">
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Other Image URL1:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtOtherImageUrl1" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Other Image URL2:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtOtherImageUrl2" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Other Image URL3:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtOtherImageUrl3" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Other Image URL4:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtOtherImageUrl4" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Other Image URL5:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtOtherImageUrl5" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Other Image URL6:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtOtherImageUrl6" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Other Image URL7:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtOtherImageUrl7" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Other Image URL8:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtOtherImageUrl8" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                RelationshipType:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRelationshipType" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Prop65:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:CheckBox ID="chkProp65" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                CPSIA Warning1:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:DropDownList ID="ddlCPSIAWarning1" runat="server" CssClass="order-list" Width="228px">
                                                                                                    <asp:ListItem Text="Select Option" Value="0"></asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard balloon">choking hazard balloon</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard contains a marble">choking hazard contains a marble</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard contains small ball">choking hazard contains small ball</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard is a marble">choking hazard is a marble</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard is a small ball">choking hazard is a small ball</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard small parts">choking hazard small parts</asp:ListItem>
                                                                                                    <asp:ListItem Value="no warning applicable">no warning applicable</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                CPSIA Warning2:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:DropDownList ID="ddlCPSIAWarning2" runat="server" CssClass="order-list" Width="228px">
                                                                                                    <asp:ListItem Text="Select Option" Value="0"></asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard balloon">choking hazard balloon</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard contains a marble">choking hazard contains a marble</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard contains small ball">choking hazard contains small ball</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard is a marble">choking hazard is a marble</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard is a small ball">choking hazard is a small ball</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard small parts">choking hazard small parts</asp:ListItem>
                                                                                                    <asp:ListItem Value="no warning applicable">no warning applicable</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                CPSIA Warning3:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:DropDownList ID="ddlCPSIAWarning3" runat="server" CssClass="order-list" Width="228px">
                                                                                                    <asp:ListItem Text="Select Option" Value="0"></asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard balloon">choking hazard balloon</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard contains a marble">choking hazard contains a marble</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard contains small ball">choking hazard contains small ball</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard is a marble">choking hazard is a marble</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard is a small ball">choking hazard is a small ball</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard small parts">choking hazard small parts</asp:ListItem>
                                                                                                    <asp:ListItem Value="no warning applicable">no warning applicable</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                CPSIA Warning4:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:DropDownList ID="ddlCPSIAWarning4" runat="server" CssClass="order-list" Width="228px">
                                                                                                    <asp:ListItem Text="Select Option" Value="0"></asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard balloon">choking hazard balloon</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard contains a marble">choking hazard contains a marble</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard contains small ball">choking hazard contains small ball</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard is a marble">choking hazard is a marble</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard is a small ball">choking hazard is a small ball</asp:ListItem>
                                                                                                    <asp:ListItem Value="choking hazard small parts">choking hazard small parts</asp:ListItem>
                                                                                                    <asp:ListItem Value="no warning applicable">no warning applicable</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                CPSIA Warning Description:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtCPSIAWarningDescription" runat="server" MaxLength="250" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Parent SKU:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtParentSKU" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Product Tax Code:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtProductTaxCode" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Launch Date:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtLaunchDate" runat="server" MaxLength="12" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Release Date:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtReleaseDate" runat="server" MaxLength="12" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                MAP:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtMAP" runat="server" MaxLength="18" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                MSRP:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtMSRP" runat="server" MaxLength="18" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Sale Start Date:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSaleStartDate" runat="server" MaxLength="12" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Sale End Date:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSaleEndDate" runat="server" MaxLength="12" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Rebate Start Date1:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRebateStartDate1" runat="server" MaxLength="12" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Rebate End Date1:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRebateEndDate1" runat="server" MaxLength="12" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Rebate Message1:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRebateMessage1" runat="server" MaxLength="250" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Rebate Name1:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRebateName1" runat="server" MaxLength="40" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Rebate Start Date2:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRebateStartDate2" runat="server" MaxLength="12" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Rebate End Date2:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRebateEndDate2" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                RebateMessage2:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRebateMessage2" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Rebate Name2:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRebateName2" runat="server" MaxLength="40" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Max Order Quantity:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtMaxOrderQuantity" runat="server" MaxLength="18" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Lead time To Ship:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtLeadtimeToShip" runat="server" MaxLength="18" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Restock Date:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtRestockDate" runat="server" MaxLength="12" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Items Included:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtItemsIncluded" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Max Aggregate Ship Quantity:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtMaxAggregateShipQuantity" runat="server" MaxLength="18" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Is Gift Wrap Available:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:CheckBox ID="chkIsGiftWrapAvailable" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Is Gift Message Available:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:CheckBox ID="chkIsGiftMessageAvailable" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Is Discontinued By Manufacturer:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:CheckBox ID="chkIsDiscontinuedByManufacturer" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                Registered Parameter:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:DropDownList ID="ddlRegisteredParameter" runat="server" CssClass="order-list"
                                                                                                    Width="112px">
                                                                                                    <asp:ListItem Text="Select Option" Value="0"></asp:ListItem>
                                                                                                    <asp:ListItem Text="PrivateLabel" Value="PrivateLabel"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Specialized" Value="Specialized"></asp:ListItem>
                                                                                                    <asp:ListItem Text="NonConsumer" Value="NonConsumer"></asp:ListItem>
                                                                                                    <asp:ListItem Text="PreConfigured" Value="PreConfigured"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                Update Delete:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:DropDownList ID="ddlUpdateDelete" runat="server" CssClass="order-list" Width="112px">
                                                                                                    <asp:ListItem Text="Select Option" Value="0"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Update" Value="Update"></asp:ListItem>
                                                                                                    <asp:ListItem Text="PartialUpdate" Value="PartialUpdate"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Delete" Value="Delete"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                PoS_ForUseWith:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtPoSForUseWith" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                PoS_BatteryChemicalType:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtPoSBatteryChemicalType" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                PoS_CameraPowerSupplyType:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtPoSCameraPowerSupplyType" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                SS_SurveillanceSystemType:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSSSurveillanceSystemType" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                SS_CameraType:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSSCameraType" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                SS_Features1:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSSFeatures1" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                SS_Features2:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSSFeatures2" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                SS_Features3:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSSFeatures3" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                SS_Features4:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSSFeatures4" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td width="15%">
                                                                                                SS_Features5:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSSFeatures5" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td width="15%">
                                                                                                SS_Camera Accessories:
                                                                                            </td>
                                                                                            <td width="85%">
                                                                                                <asp:TextBox ID="txtSSCameraAccessories" runat="server" MaxLength="100" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div style="width: 100%; float: left;">
                                                                        <table align="center" border="0" cellpadding="0" cellspacing="0" class="table" width="100%">
                                                                            <tbody>
                                                                                <tr align="center">
                                                                                    <td align="center">
                                                                                     <div id="divfloating" class="divfloatingcss" style="width:300px;">
                                                        <div style="margin-bottom: 1px;margin-top: 3px;">    
                                                                                         
                                                                                        <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return ValidateAmazonPage();"
                                                                                            OnClick="btnSave_Click" />
                                                                                        &nbsp;&nbsp;
                                                                                        <asp:ImageButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
                                                                                        </div>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img height="10" width="1" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint"></iframe>
    </div>
</asp:Content>
