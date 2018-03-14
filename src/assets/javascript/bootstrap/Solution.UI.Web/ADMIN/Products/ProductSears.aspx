<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductSears.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductSears" %>

<%@ Register Src="~/ADMIN/Controls/ProductHead.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script src="../JS/ProductSearsValidation.js" type="text/javascript"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <%--<script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
   
 
     function OpenMoreImagesPopup() {
            var popupurl = "MoreImagesUpload.aspx?StoreID=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>";
            window.open(popupurl, "MoreIamgesPopup", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800,height=550,left=250,top=80");
        }
        function btnCheck(inv, btid) {
            var r = document.getElementById(inv).value;
            // alert('sam' +'---- '+r);
            if (r <= 0) {
                alert('Enter Valid Quantity!');
            }
        }

        function Checkfields() {

            var sprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtSalePrice').value)).toFixed(2);
            if(document.getElementById('ContentPlaceHolder1_txtSalePrice').value == '')
            {
            sprice =  parseFloat(0);
            }
            
            
           
            var itemprice = parseFloat((document.getElementById('ContentPlaceHolder1_txtPrice').value)).toFixed(2);
            itemprice = itemprice;
            var startDate;
            var endDate;
            var startDate1;
            var endDate1;
            var ourPrice = parseFloat((document.getElementById('ContentPlaceHolder1_txtOurPrice').value)).toFixed(2);
            ourPrice = ourPrice;
              if (document.getElementById("ContentPlaceHolder1_txtstartdate").value != '' && document.getElementById("ContentPlaceHolder1_txtenddate").value != '') {
              startDate = new Date($('#ContentPlaceHolder1_txtstartdate').val());
  endDate = new Date($('#ContentPlaceHolder1_txtenddate').val());
}
if ( document.getElementById("ContentPlaceHolder1_txtShippingStartDate").value != '' && document.getElementById("ContentPlaceHolder1_txtShippingEndDate").value != '')
{
startDate1 = new Date($('#ContentPlaceHolder1_txtShippingStartDate').val());
  endDate1 = new Date($('#ContentPlaceHolder1_txtShippingEndDate').val());
}

            var ReturnData = true;
            if (document.getElementById("ContentPlaceHolder1_txttitle").value == '') {
                jAlert('Please enter Title.', 'Message', 'ContentPlaceHolder1_txttitle');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_ddlManufacture").selectedIndex == 0) {
                jAlert('Please select Brand.', 'Message', 'ContentPlaceHolder1_ddlManufacture');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtModelNumber").value == '') {
                jAlert('Please enter Model Number.', 'Message', 'ContentPlaceHolder1_txtModelNumber');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtSKU").value == '') {
                jAlert('Please enter SKU.', 'Message', 'ContentPlaceHolder1_txtSKU');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtupc").value == '') {
                jAlert('Please enter UPC.', 'Message', 'ContentPlaceHolder1_txtupc');
                ReturnData = false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtupc').value != '' && document.getElementById('ContentPlaceHolder1_txtupc').value.length < 12) {
                jAlert('UPC Length must be grater than or equal to 12 digit.', 'Message', 'ContentPlaceHolder1_txtupc');
                ReturnData = false;
            }
            else if((parseFloat(sprice) > parseFloat(0)) && (document.getElementById("ContentPlaceHolder1_txtstartdate").value == '' || document.getElementById("ContentPlaceHolder1_txtenddate").value == ''))
            {
             jAlert('Please Enter sale price start date and End date', 'Message', 'ContentPlaceHolder1_txtstartdate');
                ReturnData = false;
            }
            else if ( document.getElementById("ContentPlaceHolder1_txtstartdate").value != '' && document.getElementById("ContentPlaceHolder1_txtenddate").value != '' && (startDate > endDate)) {
             
                jAlert('End Date should be greater than Start Date', 'Message', 'ContentPlaceHolder1_txtenddate');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtWeight").value == '') {
                jAlert('Please enter Weight.', 'Message', 'ContentPlaceHolder1_txtWeight');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtPrice").value == '') {
                jAlert('Please enter Price.', 'Message', 'ContentPlaceHolder1_txtPrice');
                ReturnData = false;
            }

            else if (parseFloat(itemprice) < parseFloat(sprice)) {

                jAlert('Sale Price should be less than Price', 'Message', 'ContentPlaceHolder1_txtSalePrice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSalePrice').offset().top }, 'slow');
                ReturnData = false;
            }

            else if (parseFloat(sprice) > parseFloat(0) && parseFloat(sprice) < parseFloat(ourPrice)) {
                jAlert('Our Cost should be less than Sale Price', 'Message', 'ContentPlaceHolder1_txtOurPrice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtOurPrice').offset().top }, 'slow');
                ReturnData = false;
            }
            else if (parseFloat(itemprice) < parseFloat(ourPrice)) {
                jAlert('Our Cost should be less than Price', 'Message', 'ContentPlaceHolder1_txtOurPrice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtOurPrice').offset().top }, 'slow');
                ReturnData = false;
            }

            else if (document.getElementById("ContentPlaceHolder1_txtInventory").value.replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please enter Inventory.', 'Message', 'ContentPlaceHolder1_txtInventory');
                ReturnData = false;
            }
             else if ( document.getElementById("ContentPlaceHolder1_txtShippingStartDate").value != '' && document.getElementById("ContentPlaceHolder1_txtShippingEndDate").value != '' && (startDate1 > endDate1)) {
                jAlert('Shipping End Date should be greater than Shipping Start Date', 'Message', 'ContentPlaceHolder1_txtShippingEndDate');
                ReturnData = false;
            }
//            else if (document.getElementById("ContentPlaceHolder1_ddlProductTypeDelivery") != null && document.getElementById("ContentPlaceHolder1_ddlProductTypeDelivery").selectedIndex == 0) {
//                jAlert('Please Select Product Delivery Type', 'Message', 'ContentPlaceHolder1_ddlProductTypeDelivery');
//                ReturnData = false;
//            }
//            else if (document.getElementById("ContentPlaceHolder1_ddlProductType") != null && document.getElementById("ContentPlaceHolder1_ddlProductType").selectedIndex == 0) {
//                jAlert('Please Select Product Type', 'Message', 'ContentPlaceHolder1_ddlProductType');
//                ReturnData = false;
//            }
            else if (document.getElementById("ContentPlaceHolder1_txtSummary").value == '') {
                jAlert('Please enter Short Description.', 'Message', 'ContentPlaceHolder1_txtSummary');
                ReturnData = false;
            }
            //            else if (document.getElementById("ContentPlaceHolder1_txtMaincategory").value == '') {
            //                jAlert('Please enter Main Category.', 'Message', 'ContentPlaceHolder1_txtMaincategory');
            //                ReturnData = false;
            //            }
            else if (document.getElementById("ContentPlaceHolder1_txtwidth").value == '') {
                jAlert('Please enter Width.', 'Message', 'ContentPlaceHolder1_txtwidth');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtheight").value == '') {
                jAlert('Please enter Height.', 'Message', 'ContentPlaceHolder1_txtheight');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtlength").value == '') {
                jAlert('Please enter Length.', 'Message', 'ContentPlaceHolder1_txtlength');
                ReturnData = false;
            }

            else if (ClientValidate() == false) {
             jAlert('Please Select Category.', 'Message', 'ContentPlaceHolder1_trvCategories');
                return false;
            }
            return ReturnData;
        }
        $(function () {

            $('#ContentPlaceHolder1_txtstartdate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
            $('#ContentPlaceHolder1_txtenddate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
            $('#ContentPlaceHolder1_txtShippingStartDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
            $('#ContentPlaceHolder1_txtShippingEndDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function inputOnlyNumbers(evt) {
            var e = window.event || evt;
            var charCode = e.which || e.keyCode;
            if (charCode < 31 || (charCode > 47 && charCode < 58) || charCode == 46) {
                return true;
            }
            return false;
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

        function ClientValidate() {
            var listItemArray = document.getElementById('ContentPlaceHolder1_trvCategories').getElementsByTagName('INPUT');
            var isItemChecked = false;
            for (var i = 0; i < listItemArray.length; i++) {
                var listItem = listItemArray[i];
                if (listItem.checked) {
                    isItemChecked = true;
                }
            }
            if (isItemChecked == false) {
                //jAlert('Please select at least one Category', 'Message');
                return false;
            }
            else {
                return true;
            }
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

    </script>
    <script type="text/javascript">
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
    <style type="text/css">
        .chklistCat label
        {
            padding-left: 3px;
        }
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
        .footerBorder
        {
            border-top: 1px solid #DFDFDF;
            border-right: 1px solid #DFDFDF;
        }
        .footerBorderinventory
        {
            border-top: 1px solid #DFDFDF;
        }
    </style>
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

            $(".slidingDivWarehouse").show();
            $(".show_hideWarehouse").show();

            $('.show_hideWarehouse').click(function () {
                $(".slidingDivWarehouse").slideToggle();
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
    <asp:ScriptManager ID="sm1" runat="server">
    </asp:ScriptManager>
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <%-- <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/Images/add-product-icon.png" />
                                                <h2>
                                                    </h2>
                                            </div>
                                        </th>
                                    </tr>--%>
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add/Edit Product" alt="Add/Edit Product" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                <h2 style="padding-top: 3px;">
                                                    <asp:Label runat="server" Text="Add Product" ID="lblTitle"></asp:Label></h2>
                                            </div>
                                            <div class="main-title-right">
                                                <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                    <img id="imgMainDiv" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                            </div>
                                        </th>
                                    </tr>
                                    <%--  <tr>
                                        <td align="left" valign="middle" colspan="3" width="100%">
                                            <div class="tab_box2">
                                                <uc1:Header runat="server" ID="head"></uc1:Header>
                                            </div>
                                        </td>
                                    </tr>--%>
                                    <tr id="tr1" runat="server" style="background-color: #fff;">
                                        <td class="border-td">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                                                class="content-table">
                                                <tr>
                                                    <td class="border-td-sub" align="center">
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                            <tr>
                                                                <td align="left" valign="middle" colspan="3" width="100%">
                                                                    <div class="tab_box2">
                                                                        <uc1:Header runat="server" ID="Header1"></uc1:Header>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
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
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tableproduct"
                                                runat="server">
                                                <tr>
                                                    <td id="tdMainDiv">
                                                        <div id="divMain" class="slidingDivMainDiv">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                <tr class="altrow">
                                                                    <td align="center" colspan="6">
                                                                        <span id="msgid" runat="server" style="cursor: default; text-align: center; color: Red;
                                                                            font-weight: bold;" visible="false">Your product dose not clone until click on save
                                                                            button</span>
                                                                        <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td align="right" colspan="5">
                                                                        <span class="star">*</span> Required Field
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow" style="display: none">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Store Name :
                                                                    </td>
                                                                    <td style="width: 80%; height: 30px" colspan="3">
                                                                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="226px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">*</span> Title :
                                                                    </td>
                                                                    <td colspan="4" style="height: 30px">
                                                                        <asp:TextBox runat="server" ID="txttitle" TabIndex="1" MaxLength="400" Width="50%"
                                                                            CssClass="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">*</span> Brand :
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <asp:DropDownList ID="ddlManufacture" TabIndex="2" runat="server" CssClass="order-list"
                                                                            Width="228px" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="" rowspan="10" align="left" valign="top">
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
                                                                                        <a href="javascript:void(0);" class="show_hideWarehouse" onclick="return ShowHideButton('ImgeBayWarehouses','tdeBayWarehouses');">
                                                                                            <img id="ImgeBayWarehouses" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"
                                                                                                class="minimize" title="Minimize" alt="Minimize" /></a>
                                                                                    </div>
                                                                                </th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td id="tdeBayWarehouses" align="left">
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
                                                                                                                    <asp:TextBox ID="txtInventory" CssClass="order-textfield" TabIndex="3" Style="width: 50px;
                                                                                                                        text-align: center;" runat="server" Text='<%#Bind("Inventory") %>' onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');SetTotalWeight();return ret;"
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
                                                                <tr class="altrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">*</span> Model Number :
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="txtModelNumber" TabIndex="4" runat="server" CssClass="order-textfield"
                                                                            MaxLength="100"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">*</span> SKU :
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="txtSKU" TabIndex="5" runat="server" CssClass="order-textfield" AutoCompleteType="Disabled"
                                                                            MaxLength="100"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">*</span> UPC :
                                                                    </td>
                                                                    <%-- <td colspan="2" style="height: 30px">
                                                                        <asp:TextBox ID="txtupc" runat="server" CssClass="order-textfield" Width="270px"
                                                                            MaxLength="100"></asp:TextBox>
                                                                    </td>--%>
                                                                    <td colspan="2" style="height: 30px">
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtupc" TabIndex="6" runat="server" class="order-textfield" MaxLength="100"></asp:TextBox>
                                                                                </td>
                                                                                <td style="display: none">
                                                                                    <asp:LinkButton ID="lbtngetUPC" runat="server" OnClick="lbtngetUPC_Click" Visible="false">Get UPC</asp:LinkButton>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trBarcode" runat="server" visible="false">
                                                                    <td valign="top" style="padding-top: 32px;">
                                                                        Product Barcode :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <a style="padding-left: 174px" href="javascript:void(0);" onclick="return PrintBarcode();">
                                                                            Print Barcode</a>
                                                                        <br />
                                                                        <div id="divBarcodePrint">
                                                                            <img alt="" id="imgOrderBarcode" runat="server" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Start Date :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox runat="server" TabIndex="7" ID="txtstartdate" CssClass="order-textfield"
                                                                            Width="86px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>End Date :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox runat="server" ID="txtenddate" TabIndex="8" CssClass="order-textfield"
                                                                            Width="86px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">*</span> Weight :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtWeight" TabIndex="8" runat="server" CssClass="order-textfield"
                                                                            Width="86px" onkeypress="return keyRestrict(event,'0123456789');" MaxLength="50"></asp:TextBox>
                                                                        lbs. (Example : 1.30)
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">*</span> Price :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        $<asp:TextBox ID="txtPrice" TabIndex="9" runat="server" CssClass="order-textfield"
                                                                            Width="78px" onkeypress="return keyRestrict(event,'0123456789');" MaxLength="50"></asp:TextBox>
                                                                        (Example : 12.00)
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Sale Price :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        $<asp:TextBox ID="txtSalePrice" TabIndex="10" runat="server" CssClass="order-textfield"
                                                                            Width="78px" onkeypress="return keyRestrict(event,'0123456789');" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;</span>Our Cost:
                                                                    </td>
                                                                    <td colspan="4">
                                                                        $<asp:TextBox ID="txtOurPrice" TabIndex="11" runat="server" CssClass="order-textfield"
                                                                            Width="78px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">*</span> Inventory :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:UpdatePanel runat="server" ID="inv">
                                                                            <ContentTemplate>
                                                                                <asp:TextBox ID="txtInventory" runat="server" CssClass="order-textfield" Width="86px"
                                                                                    onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');SetTotalWeight();return ret;"
                                                                                    onblur="SetTotalWeight();" onkeyup="SetTotalWeight();" MaxLength="20"></asp:TextBox></ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                        <%--<img alt="Low Level" title="Low Level" src="../Images/bullet_red.png" id="ImgToggelInventory"
                                                                            runat="server" visible="false" style="vertical-align: middle;" />--%>
                                                                        &nbsp;<span style="font-size: 28px; vertical-align: middle; color: Red; cursor: default;"
                                                                            title="Low Level" id="ImgToggelInventory" runat="server" visible="false">• <font
                                                                                style="font-size: 13px; vertical-align: middle; padding-bottom: 5px;">Low Level</font></span>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Shipping Start Date :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox runat="server" ID="txtShippingStartDate" TabIndex="12" CssClass="order-textfield"
                                                                            Width="86px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Shipping End Date :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox runat="server" TabIndex="13" ID="txtShippingEndDate" CssClass="order-textfield"
                                                                            Width="86px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow" style="display: none">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Color :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtColor" TabIndex="14" runat="server" CssClass="order-textfield"
                                                                            Width="302px" Height="44px" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow" style="display: none">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Size :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtSize" TabIndex="15" runat="server" CssClass="order-textfield"
                                                                            Height="44px" TextMode="MultiLine" Width="302px" MaxLength="500"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Active :
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:CheckBox ID="chkPublished" TabIndex="16" runat="server" Text=" Published" Checked="true"
                                                                            Width="150px" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                                        <span class="star">&nbsp;&nbsp;</span>Is Discontinue:
                                                                    </td>
                                                                    <td align="left" colspan="4">
                                                                        <asp:CheckBox ID="chkIsDiscontinue" TabIndex="17" runat="server" Checked="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow" style="display: none;">
                                                                    <td width="12%">
                                                                        <span class="star">*</span>Stock/Dropship:
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
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
                                                                                            <asp:DropDownList ID="ddlProductTypeDelivery" TabIndex="18" AutoPostBack="true" runat="server"
                                                                                                class="product-type" Width="228px" OnSelectedIndexChanged="ddlProductTypeDelivery_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                                        <ContentTemplate>
                                                                                            <div id="divvendor" runat="server" visible="false">
                                                                                                <span class="star"></span>Drop Shipper/Vendor :
                                                                                            </div>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                                        <ContentTemplate>
                                                                                            <asp:DropDownList ID="ddlvendor" TabIndex="19" Width="228px" runat="server" class="product-type"
                                                                                                AutoPostBack="true" Visible="false">
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <%--<asp:UpdateProgress ID="upgrProductTypeDelivery" runat="server" AssociatedUpdatePanelID="upProductTypeDelivery">
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
                                                                                    class="product-type" OnSelectedIndexChanged="ddlProductTypeDelivery_SelectedIndexChanged"
                                                                                    TabIndex="5">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>--%>
                                                                    </td>
                                                                    <%--<td width="12%">
                                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                            <ContentTemplate>
                                                                                <div id="divvendor" runat="server" visible="false">
                                                                                    <span class="star">*</span> Drop Shipper/Vendor :
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td width="12%">
                                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="ddlvendor" runat="server" class="product-type" AutoPostBack="true"
                                                                                    TabIndex="5" Visible="false">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>--%>
                                                                </tr>
                                                                <tr class="oddrow" style="display: none;">
                                                                    <td width="12%">
                                                                        <span class="star">*</span>Product&nbsp;Type:
                                                                    </td>
                                                                    <td colspan="4">
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
                                                                                <asp:DropDownList ID="ddlProductType" TabIndex="20" Width="228px" AutoPostBack="true"
                                                                                    runat="server" class="product-type" OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged">
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
                                                                                    GridLines="None" HeaderStyle-ForeColor="White" ShowFooter="true" Width="70%"
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
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="even-row">
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
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                            <tr>
                                                                                                                                <td class="ckeditor-table">
                                                                                                                                    <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                                                                        TabIndex="21" Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7;
                                                                                                                                        background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
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
                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                    <tr>
                                                                                                                        <td width="20%" valign="top">
                                                                                                                            Main Image:
                                                                                                                        </td>
                                                                                                                        <td valign="middle">
                                                                                                                            <img alt="Upload" id="ImgLarge" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" /><br />
                                                                                                                            <%--<img alt="Upload" id="ImgLarge" src="/Resources/RedTag/Product/Medium/image_not_available.jpg"
                                                                                                    runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" /><br />--%>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            &nbsp;
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                <tr>
                                                                                                                                    <td width="10%">
                                                                                                                                        <asp:FileUpload ID="fuImage" runat="server" Width="220px" Style="border: 1px solid #1a1a1a;
                                                                                                                                            background: #f5f5f5; color: #000000;" TabIndex="23" />
                                                                                                                                    </td>
                                                                                                                                    <td width="9%">
                                                                                                                                        <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click"
                                                                                                                                            TabIndex="24" />
                                                                                                                                    </td>
                                                                                                                                    <td width="64%">
                                                                                                                                        <asp:ImageButton ID="btnDelete" runat="server" Visible="false" OnClientClick="return checkondelete();"
                                                                                                                                            AlternateText="Delete" OnClick="btnDelete_Click" />
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
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="border-td content-table">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <th>
                                                                                                <div class="main-title-left">
                                                                                                    <img class="img-left" title="SEO" alt="SEO" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                    <h2>
                                                                                                        SEO</h2>
                                                                                                </div>
                                                                                                <div class="main-title-right">
                                                                                                    <a title="close" href="javascript:void(0);" class="show_hideSEO" onclick="return ShowHideButton('ImgSEO1','tdSEO');">
                                                                                                        <img id="ImgSEO1" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a><a
                                                                                                            title="Minimize" href="#"></a>
                                                                                                </div>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td id="tdSEO">
                                                                                                <div id="tab-container" class="slidingDivSEO">
                                                                                                    <ul class="menu">
                                                                                                        <li class="active" id="ordernotes1" onclick='$("#ordernotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.order-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                            Page Title</li>
                                                                                                        <li id="privatenotes1" onclick='$("#ordernotes1").removeClass("active");$("#privatenotes1").addClass("active"); $("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.private-notes").fadeIn();$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                            Keywords</li>
                                                                                                        <li id="giftnotes1" onclick='$("#giftnotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.gift-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                            Description</li>
                                                                                                        <li id="myaccount1" onclick='$("#myaccount1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("div.my-account").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");'>
                                                                                                            Tooltip</li>
                                                                                                    </ul>
                                                                                                    <span class="clear"></span>
                                                                                                    <div class="tab-content-2 order-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="25"></asp:TextBox></div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 private-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSEKeyword" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="26"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 gift-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSEDescription" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="27"></asp:TextBox></div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 my-account">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox Height="19px" ID="txtToolTip" BorderStyle="None" runat="server" MaxLength="500"
                                                                                                                CssClass="status-textfield" Width="100%" TabIndex="28"></asp:TextBox></div>
                                                                                                    </div>
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
                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
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
                                                                                                                Height="19px" Width="400px"></asp:TextBox>
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
                                                                                                                    Font-Size="12px" Width="304px" PopulateNodesFromClient="True" ShowLines="true"
                                                                                                                    TabIndex="22">
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
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                        <tr>
                                                                            <th colspan="6">
                                                                                <div class="main-title-left">
                                                                                    <img class="img-left" title="Additional Info" alt="Additional Info" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                    <h2>
                                                                                        Additional Info</h2>
                                                                                </div>
                                                                                <div class="main-title-right">
                                                                                    <a href="javascript:void(0);" class="show_hideDesc" onclick="return ShowHideButton('ImgDesc','tdDesc');">
                                                                                        <img id="ImgDesc" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a>
                                                                                </div>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" id="tdDesc" colspan="6">
                                                                                <div id="div2" class="slidingDivDesc">
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr class="oddrow">
                                                                                            <td style="width: 202px; height: 30px;" valign="middle">
                                                                                                <span class="star">*</span> Short Description :
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                                <asp:TextBox TextMode="multiLine" TabIndex="29" ID="txtSummary" Height="101px" Width="410px"
                                                                                                    runat="server" CssClass="order-textfield" MaxLength="500"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td style="width: 224px; height: 24px;" valign="top">
                                                                                                <span class="star">&nbsp;&nbsp;</span>Display Order :
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                                <asp:TextBox ID="txtDisplayOrder" TabIndex="30" runat="server" CssClass="order-textfield"
                                                                                                    Width="73px" MaxLength="10"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td style="width: 202px; height: 30px;" valign="middle">
                                                                                                <span class="star">&nbsp;&nbsp;</span>Handling Fee :
                                                                                            </td>
                                                                                            <td colspan="2" style="height: 30px">
                                                                                                <asp:TextBox ID="txtSurCharge" TabIndex="31" runat="server" CssClass="order-textfield"
                                                                                                    Width="73px" onkeypress="return keyRestrict(event,'0123456789');" MaxLength="20"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td style="width: 202px; height: 30px;" valign="middle">
                                                                                                <span class="star">&nbsp;&nbsp;</span>Map Price Indicator :
                                                                                            </td>
                                                                                            <td colspan="2" style="height: 30px">
                                                                                                <asp:DropDownList ID="ddlMapPriceindicator" TabIndex="32" runat="server" CssClass="order-list">
                                                                                                    <asp:ListItem Text="Select One" Value=""></asp:ListItem>
                                                                                                    <asp:ListItem Text="Strict" Value="Strict"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Non-Strict" Value="Non-Strict"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td style="width: 202px; height: 30px;" valign="top">
                                                                                                <span class="star">*</span> Shipping Dimensions :
                                                                                            </td>
                                                                                            <td colspan="2" style="height: 30px">
                                                                                                <span style="padding-left: 1px;">W</span>
                                                                                                <asp:TextBox ID="txtwidth" runat="server" TabIndex="33" CssClass="order-textfield"
                                                                                                    Width="60px" onkeypress="return isNumberKey(event);" MaxLength="10"></asp:TextBox><span>&nbsp;x&nbsp;H</span>
                                                                                                <asp:TextBox ID="txtheight" runat="server" TabIndex="34" CssClass="order-textfield"
                                                                                                    Width="60px" onkeypress="return isNumberKey(event);" MaxLength="10"></asp:TextBox><span>&nbsp;x&nbsp;L</span>
                                                                                                <asp:TextBox ID="txtlength" runat="server" CssClass="order-textfield" Width="60px"
                                                                                                    onkeypress="return isNumberKey(event);" TabIndex="35" MaxLength="10"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td style="width: 202px; height: 30px;" valign="middle">
                                                                                                <span class="star">&nbsp;&nbsp;</span>Is Restricted :
                                                                                            </td>
                                                                                            <td colspan="2" style="height: 30px">
                                                                                                <asp:CheckBox ID="chkIsRestricted" TabIndex="36" runat="server" Width="150px" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td style="width: 202px; height: 30px;" valign="middle">
                                                                                                <span class="star">&nbsp;&nbsp;</span>Perishable :
                                                                                            </td>
                                                                                            <td colspan="2" style="height: 30px">
                                                                                                <asp:CheckBox ID="ChkPerishable" TabIndex="37" runat="server" Width="150px" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow" style="display: none">
                                                                                            <td style="width: 202px; height: 30px;" valign="middle">
                                                                                                <span class="star">&nbsp;&nbsp;</span>Requires Refrigeration :
                                                                                            </td>
                                                                                            <td colspan="2" style="height: 30px">
                                                                                                <asp:CheckBox ID="ChkRequiresRefrigeration" TabIndex="38" runat="server" Width="150px" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow" style="display: none">
                                                                                            <td style="width: 202px; height: 30px;" valign="middle">
                                                                                                <span class="star">&nbsp;&nbsp;</span>Requires Freezing :
                                                                                            </td>
                                                                                            <td colspan="2" style="height: 30px">
                                                                                                <asp:CheckBox ID="ChkRequiresFreezing" TabIndex="39" runat="server" Width="150px" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow" style="display: none">
                                                                                            <td style="width: 202px; height: 30px;" valign="middle">
                                                                                                <span class="star">&nbsp;&nbsp;</span>Contains Alcohol :
                                                                                            </td>
                                                                                            <td colspan="2" style="height: 30px">
                                                                                                <asp:CheckBox ID="ChkContainsAlcohol" TabIndex="40" runat="server" Width="150px" />
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
                                                <tr class="altrow">
                                                    <td>
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="width: 70%">
                                                                    <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                        CausesValidation="true" OnClientClick="return Checkfields();" TabIndex="41" OnClick="btnSave_Click" />
                                                                    &nbsp;<asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" TabIndex="42"
                                                                        ToolTip="Cancel" OnClick="btnCancel_Click" />
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
                </td>
            </tr>
            <tr>
                <td style="height: 10px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" alt="" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint"></iframe>
    </div>
</asp:Content>
