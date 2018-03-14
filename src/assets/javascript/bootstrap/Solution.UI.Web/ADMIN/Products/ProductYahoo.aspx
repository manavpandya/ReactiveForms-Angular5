<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductYahoo.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductYahoo" %>

<%@ Register Src="~/ADMIN/Controls/ProductHead.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script src="../JS/ProductYahooValidation.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
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
        function WriteYahooIds() {
            if (document.getElementById('ContentPlaceHolder1_txtProductName') != null && document.getElementById('ContentPlaceHolder1_txtSKU') != null && document.getElementById('ContentPlaceHolder1_txtYahooIDs') != null) {
                var PName = document.getElementById('ContentPlaceHolder1_txtProductName').value.toLowerCase();
                var Sku = document.getElementById('ContentPlaceHolder1_txtSKU').value.toLowerCase();
                var YahooIdUrl = PName + '-' + Sku;
                RemoveYahooSpecialChar(YahooIdUrl.replace(/\s+/g, '-').replace(/--/g, '-').replace(/--/g, '-'));
                document.getElementById('ContentPlaceHolder1_txtYahooIDs').value = YahooIdUrl;
                RemoveYahooSpecialChar();
            }
        }

        function RemoveYahooSpecialChar() {

            var yahoonew = document.getElementById('ContentPlaceHolder1_txtYahooIDs').value;
            yahoonew = yahoonew.replace(/\s+/g, '-').replace(/--/g, '-').replace(/--/g, '-');
            var test = yahoonew.replace(/[`~!@#$%^&*()_|+\=?;:'",.<>\{\}\[\]\\\/]/gi, '-');
            test = test.replace(/--/g, '-').replace(/--/g, '-').replace(/--/g, '-').replace(/--/g, '-').replace(/--/g, '-').replace(/--/g, '-');
            document.getElementById('ContentPlaceHolder1_txtYahooIDs').value = test.toLowerCase();
        }

        function checkondelete() {
            jConfirm('Are you sure want to delete this Image ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById("ContentPlaceHolder1_btndelImg").click();
                    return true;
                }
                else { return false; }
            });
            return false;
        }
        function btnCheck(inv, btid) {
            var r = document.getElementById(inv).value;
            // alert('sam' +'---- '+r);
            if (r <= 0) {
                alert('Enter Valid Quantity!');
            }
        }
    </script>
    <script type="text/javascript">
        function CloneVisible() {
            if (document.getElementById('ContentPlaceHolder1_trCloneProductMsg') != null) {
                document.getElementById('ContentPlaceHolder1_trCloneProductMsg').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_trProductData').style.display = '';
                document.getElementById('ContentPlaceHolder1_trProductData').style.display = 'block';
            }
        }
    </script>
    <script type="text/javascript">
        var myWindow;
        function openCenteredCrossSaleWindow(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('ProductSku.aspx?StoreID=' + StoreID + '&ProductID='+ ProductID + '&clientid=' + x, "subWind", windowFeatures);
        }


        function openCenteredCrossSaleWindowOptionalAcc(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('OptionalAccessoriesPopup.aspx?StoreID=' + StoreID +'&ProductID='+ ProductID + '&clientid=' + x, "subWind", windowFeatures);
        }

        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000)); 
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function OpenProductOption() {
            var productid='<%=Request["Id"] %>'
            if(productid !='' && productid !=null)
            {
              window.location.href='ProductVariant.aspx?StoreId=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>';
            }
        }

        function OpenMoreImagesPopup() {
            var popupurl = "MoreImagesUpload.aspx?StoreID=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>";
            window.open(popupurl, "MoreIamgesPopup", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800,height=550,left=250,top=80");
        }


        function SaleClerance()
        {
          if(document.getElementById('ContentPlaceHolder1_chkSaleclearance').checked ==true)
            {
             document.getElementById('ContentPlaceHolder1_txtSaleClearance').style.display='';
            }
            else{
            document.getElementById('ContentPlaceHolder1_txtSaleClearance').style.display='none';
            }
        }

         function DisContinue(tabNo) {
            try {
                if (tabNo == 1) {
                    document.getElementById('ContentPlaceHolder1_divDisContinueYes').style.display = '';
                }
                else {
                    document.getElementById('ContentPlaceHolder1_divDisContinueYes').style.display = 'none';
                    document.getElementById('ContentPlaceHolder1_txtContiSKUs').value = "";

                }
            }
            catch (err) { alert(err.message); }
        }

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

        function ChangeVendorSKU(id, controlname, controlmain) {
            document.getElementById("ContentPlaceHolder1_hdnid").value = id;
            document.getElementById("ContentPlaceHolder1_hdncontrol").value = controlname;
            document.getElementById("ContentPlaceHolder1_hdnmaincontrol").value = controlmain;
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
        
    </script>
    <script type="text/javascript">
        function keyRestrictEngraving(e, validchars) {
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
            var chkcnt = document.getElementById("ContentPlaceHolder1_grdWarehouse").getElementsByTagName("input");
            for (var i = 0; i < chkcnt.length; i++) {
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
    <script type="text/javascript">
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
    <asp:ScriptManager ID="sm1" runat="server">
    </asp:ScriptManager>
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
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
            <tr id="trProductOption" runat="server" style="display: none;" align="right">
                <td>
                    <a href="#" title="Product Option" onclick="return OpenProductOption();">
                        <img src="/App_Themes/<%=Page.Theme %>/images/product-options.gif" alt="Product Options"
                            title="Product Options" class="img-right" height="23" />
                    </a>
                </td>
            </tr>
            <tr id="trProductData" runat="server">
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add/Edit Product" alt="Add/Edit Product" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                <h2 style="padding-top: 3px;">
                                                    Add/Edit Product</h2>
                                            </div>
                                            <div class="main-title-right">
                                                <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                    <img id="imgMainDiv" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                            </div>
                                        </th>
                                    </tr>
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
                                                                <tr>
                                                                    <td align="center">
                                                                        <span id="msgid" runat="server" style="cursor: default; text-align: center; color: Red;
                                                                            font-weight: bold;" visible="false">Your product dose not clone until click on save
                                                                            button</span>
                                                                        <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr class="altrow">
                                                                                <td width="12%">
                                                                                    <span class="star">*</span>Product&nbsp;Name:
                                                                                </td>
                                                                                <td colspan="5">
                                                                                    <asp:TextBox ID="txtProductName" runat="server" MaxLength="500" Style="width: 800px;"
                                                                                        class="order-textfield" TabIndex="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td width="12%">
                                                                                    <span class="star">*</span>SKU:
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:TextBox ID="txtSKU" runat="server" MaxLength="500" class="order-textfield" TabIndex="2"></asp:TextBox>
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
                                                                                                                                    onblur="SetTotalWeight();" MaxLength="5" TabIndex="7" onkeyup="SetTotalWeight();"></asp:TextBox>
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
                                                                            <tr class="altrow" style="display: none">
                                                                                <td width="12%">
                                                                                    <span class="star">&nbsp;</span> Serial No:
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:TextBox ID="txtSerialNo" runat="server" class="order-textfield" onkeypress="return keyRestrictForOnlyNumeric(event,'0123456789');"
                                                                                        MaxLength="6" TabIndex="3"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="12%" valign="top">
                                                                                    <span class="star">*</span> YahooID :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="txtYahooIDs" runat="server" CssClass="status-textfield" onblur="RemoveYahooSpecialChar();"
                                                                                        TabIndex="2" Width="340px"></asp:TextBox><br />
                                                                                    <span class="star">(Please don't enter space and special character instead use '-')</span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td width="12%">
                                                                                    <span class="star">&nbsp;</span>UPC:
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtUPC" runat="server" MaxLength="100" class="order-textfield" TabIndex="3"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="display: none">
                                                                                                <asp:LinkButton ID="lbtngetUPC" runat="server" OnClick="lbtngetUPC_Click" Visible="false">Get UPC</asp:LinkButton>&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <%--<asp:TextBox ID="txtUPC" runat="server" MaxLength="100" class="order-textfield" TabIndex="4"></asp:TextBox>
                                                                                    &nbsp;
                                                                                    <asp:LinkButton ID="lnkUpcRandomNo" Visible="false" runat="server" ForeColor="#FF0000"
                                                                                        Style="text-decoration: underline;" OnClick="lnkUpcRandomNo_OnClick">Generate Random UPC</asp:LinkButton>--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trBarcode" runat="server" visible="false">
                                                                                <td valign="top" style="padding-top: 32px;">
                                                                                    Product Barcode :
                                                                                </td>
                                                                                <td>
                                                                                    <a style="padding-left: 174px" href="javascript:void(0);" onclick="return PrintBarcode();">
                                                                                        Print Barcode</a>
                                                                                    <br />
                                                                                    <div id="divBarcodePrint">
                                                                                        <img alt="" id="imgOrderBarcode" runat="server" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr class="altrow">
                                                                                <td width="12%">
                                                                                    <span class="star">*</span>Manufacturer:
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:DropDownList ID="ddlManufacture" runat="server" class="order-list" TabIndex="5">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow" style="display: none;">
                                                                                <td width="12%">
                                                                                    <span class="star">&nbsp;</span>Distributor:
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:DropDownList ID="ddlDistributor" runat="server" class="order-list" TabIndex="6">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow" style="display: none;">
                                                                                <td width="12%">
                                                                                    <span class="star">&nbsp;</span>Vendor:
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:CheckBoxList ID="chkmarryproduct" runat="server" CssClass="chklistCat" RepeatColumns="5"
                                                                                        RepeatDirection="Horizontal" TabIndex="7" Style="width: 600px;">
                                                                                    </asp:CheckBoxList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td width="12%">
                                                                                    <span class="star">&nbsp;</span>Availability:
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="txtAvailability" Width="150" MaxLength="500" TabIndex="8" runat="server"
                                                                                        class="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td>
                                                                                    <span class="star">*</span>Inventory:
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:UpdatePanel runat="server" ID="inv">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txtInventory" runat="server" TabIndex="9" class="status-textfield"
                                                                                                Width="150"></asp:TextBox></ContentTemplate>
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
                                                                                <td>
                                                                                    <span class="star">*</span>Weight:
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="txtWeight" runat="server" Width="80px" class="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                        TabIndex="10"></asp:TextBox>
                                                                                    lbs. Ex (12.52)
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td>
                                                                                    <span class="star">*</span>Price:
                                                                                </td>
                                                                                <td width="15%">
                                                                                    $<asp:TextBox ID="txtPrice" runat="server" class="order-textfield" Width="80px" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                        TabIndex="11"></asp:TextBox>
                                                                                    Ex (12.00)
                                                                                </td>
                                                                                <td width="15%" align="right">
                                                                                    Sale Price: $<asp:TextBox ID="txtSalePrice" runat="server" class="order-textfield"
                                                                                        Width="80px" onkeypress="return keyRestrict(event,'0123456789.');" TabIndex="12"></asp:TextBox>
                                                                                    Ex (8.62) &nbsp;&nbsp;
                                                                                    <asp:CompareValidator ID="cmpSalePrice" runat="server" ControlToCompare="txtPrice"
                                                                                        ControlToValidate="txtSalePrice" Display="Dynamic" ForeColor="Red" Font-Bold="true"
                                                                                        CssClass="error" ErrorMessage="Sale Price Should Be Less than Price" Operator="LessThanEqual"
                                                                                        SetFocusOnError="True" Type="Double" ValidationGroup="Product">Sale Price Should Be Less than Price</asp:CompareValidator>
                                                                                </td>
                                                                                <td align="left">
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td>
                                                                                    <span class="star">&nbsp;</span>Our Cost:
                                                                                </td>
                                                                                <td>
                                                                                    $<asp:TextBox ID="txtOurPrice" runat="server" CssClass="order-textfield" Width="80px"
                                                                                        onkeypress="return keyRestrict(event,'0123456789.');" TabIndex="13"></asp:TextBox>
                                                                                    Ex (7.62)
                                                                                </td>
                                                                                <td style="display: none;">
                                                                                    Sale-Clearance:
                                                                                </td>
                                                                                <td style="display: none;">
                                                                                    <asp:CheckBox ID="chkSaleclearance" runat="server" onchange="SaleClerance();" />
                                                                                    &nbsp;<asp:TextBox ID="txtSaleClearance" Style="display: none" runat="server" CssClass="order-textfield"
                                                                                        Width="80px" onkeypress="return keyRestrict(event,'0123456789.');" TabIndex="14"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow" style="display: none">
                                                                                <td>
                                                                                    <span class="star">&nbsp;</span> Display Order:
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="txtDisplayOrder" runat="server" class="status-textfield" MaxLength="4"
                                                                                        onkeypress="return keyRestrict(event,'0123456789');" TabIndex="13" Width="80"></asp:TextBox>
                                                                                </td>
                                                                                <td style="display: none;">
                                                                                    SurCharge:
                                                                                </td>
                                                                                <td style="display: none;">
                                                                                    <asp:TextBox ID="txtSurCharge" runat="server" class="order-textfield" Width="80px"
                                                                                        onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td>
                                                                                    <span class="star">&nbsp;</span> Status:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkPublished" runat="server" Checked="true" TabIndex="14" />
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkIsDiscontinue" runat="server" Text="Is Discontinue: " TextAlign="Left"
                                                                                        Checked="false" TabIndex="15" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td width="12%">
                                                                                    <span class="star">*</span>Stock/Dropship:
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
                                                                                                class="product-type" OnSelectedIndexChanged="ddlProductTypeDelivery_SelectedIndexChanged"
                                                                                                TabIndex="16">
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                                <td width="12%" colspan="2">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                                                    <ContentTemplate>
                                                                                                        <div id="divvendor" runat="server" visible="false">
                                                                                                            <span class="star"></span>Drop Shipper/Vendor :
                                                                                                        </div>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:DropDownList ID="ddlvendor" runat="server" class="product-type" AutoPostBack="true"
                                                                                                            TabIndex="17" Visible="false">
                                                                                                        </asp:DropDownList>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="12%">
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
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
                                                                                                OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged" TabIndex="18">
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
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
                                                                                                GridLines="None" HeaderStyle-ForeColor="White" ShowFooter="true" Width="60%"
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
                                                                            <tr class="altrow" style="display: none;">
                                                                                <td>
                                                                                    <span class="star">&nbsp;</span> DisContinue:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:RadioButton ID="RBtnContiYes" onclick="DisContinue(1);" runat="server" Text=" Yes"
                                                                                        GroupName="DiscontinueSKU" />&nbsp;<asp:RadioButton ID="RBtnContNo" Checked="true"
                                                                                            onclick="DisContinue(0);" runat="server" Text=" No" GroupName="DiscontinueSKU" />
                                                                                </td>
                                                                                <td>
                                                                                    <div style="display: none; width: 320px; float: left" id="divDisContinueYes" runat="server">
                                                                                        <asp:Label ID="lblContinueSKUs" runat="server">Is there Any SKUs</asp:Label>
                                                                                        <asp:TextBox ID="txtContiSKUs" Height="20px" runat="server" CssClass="status-textfield"
                                                                                            Width="120px" TextMode="MultiLine"></asp:TextBox>
                                                                                        <a id="a4" name="aRelated" onclick="openCenteredCrossSaleWindow('ContentPlaceHolder1_txtContiSKUs','');"
                                                                                            cssclass="sub_title" style="margin-right: 15px; font-weight: bold;">Select SKUs</a>
                                                                                    </div>
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td>
                                                                                    <span class="star">&nbsp;</span> Dimensions:
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    width&nbsp;&nbsp;
                                                                                    <asp:TextBox ID="txtWidth" runat="server" class="order-textfield" Width="80px" TabIndex="19"
                                                                                        onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                    &nbsp;&nbsp;&nbsp;x&nbsp;&nbsp;&nbsp;Height
                                                                                    <asp:TextBox ID="txtHeight" runat="server" class="order-textfield" Width="80px" TabIndex="20"
                                                                                        onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                    &nbsp;&nbsp;&nbsp;x&nbsp;&nbsp;&nbsp;Length
                                                                                    <asp:TextBox ID="txtLength" runat="server" class="order-textfield" Width="80px" TabIndex="21"
                                                                                        onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
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
                                                                                                        <div class="tab-content-3" style="width: 98%">
                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <div id="divDescription" runat="Server" visible="false">
                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
                                                                                                                                <tr>
                                                                                                                                    <td width="144px">
                                                                                                                                        <asp:DropDownList ID="ddlDescription" runat="server" class="product-type" AutoPostBack="True"
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
                                                                                                                                        <asp:TextBox ID="txtTitleDesc" runat="server" class="order-textfield" MaxLength="50"
                                                                                                                                            Width="175px"></asp:TextBox>&nbsp;&nbsp;
                                                                                                                                        <asp:LinkButton ID="lnkSaveDesc" runat="server" Font-Bold="true" Font-Size="12px"
                                                                                                                                            Font-Underline="true" Text="Save Description" OnClick="lnkSaveDesc_Click1">Save Description</asp:LinkButton>
                                                                                                                                    </td>
                                                                                                                                    <td align="left">
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
                                                                                                                                        TabIndex="22" Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7;
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
                                                                                                                            Icon Image:
                                                                                                                        </td>
                                                                                                                        <td valign="middle">
                                                                                                                            <img alt="Upload" id="ImgLarge" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" /><br />
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
                                                                                                                                        <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a;
                                                                                                                                            background: #f5f5f5; color: #000000;" TabIndex="25" />
                                                                                                                                    </td>
                                                                                                                                    <td width="9%">
                                                                                                                                        <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click"
                                                                                                                                            TabIndex="26" />
                                                                                                                                    </td>
                                                                                                                                    <td width="64%">
                                                                                                                                        <asp:ImageButton ID="btnDelete" runat="server" Visible="false" OnClientClick="return checkondelete();"
                                                                                                                                            AlternateText="Delete" OnClick="btnDelete_Click" />
                                                                                                                                        <div style="display: none;">
                                                                                                                                            <asp:ImageButton ID="btndelImg" runat="server" AlternateText="Delete" OnClick="btndelImg_Click"
                                                                                                                                                Style="width: 14px" />
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                        <td valign="bottom" id="trUploadFiles" runat="server">
                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                <tr>
                                                                                                                                    <td valign="bottom" style="padding: 0px 2px;">
                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                            <tr>
                                                                                                                                                <td>
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
                                                                                                    <a title="close" href="javascript:void(0);" class="show_hideSEO" onclick="return ShowHideButton('ImgSEO','tdSEO');">
                                                                                                        <img id="ImgSEO" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a><a
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
                                                                                                            Image Tooltip</li>
                                                                                                    </ul>
                                                                                                    <span class="clear"></span>
                                                                                                    <div class="tab-content-2 order-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="29"></asp:TextBox></div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 private-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSEKeyword" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="30"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 gift-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSEDescription" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="31"></asp:TextBox></div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 my-account">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox Height="19px" ID="txtToolTip" BorderStyle="None" runat="server" MaxLength="500"
                                                                                                                CssClass="status-textfield" Width="100%" TabIndex="32"></asp:TextBox></div>
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
                                                                                                                    Font-Size="12px" Width="304px" PopulateNodesFromClient="True" ShowLines="true"
                                                                                                                    TabIndex="23">
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
                                                                            <tr style="display: none;">
                                                                                <td width="15%" valign="top">
                                                                                    Optional Accessories:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtOptionalAccesories" TextMode="MultiLine" runat="server" CssClass="status-textfield"
                                                                                        Width="98%" TabIndex="8"></asp:TextBox>
                                                                                </td>
                                                                                <td width="14%" align="left" valign="bottom">
                                                                                    <a id="aOptAcc" name="aOptAcc" onclick="openCenteredCrossSaleWindowOptionalAcc('ContentPlaceHolder1_txtOptionalAccesories');"
                                                                                        style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select
                                                                                        Product(s) </a>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="15%" valign="top">
                                                                                    Product URL :
                                                                                </td>
                                                                                <td align="left" colspan="2">
                                                                                    <asp:TextBox ID="txtProductURL" runat="server" CssClass="status-textfield" TabIndex="33"
                                                                                        Width="400px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="display: none;">
                                                                                <td width="15%" valign="top">
                                                                                    Related Products:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtRelProducts" runat="server" CssClass="status-textfield" Width="450px"
                                                                                        Height="50px" TextMode="MultiLine" TabIndex="34"></asp:TextBox>
                                                                                    &nbsp; <a id="aRelated" name="aRelated" onclick="openCenteredCrossSaleWindow('ContentPlaceHolder1_txtRelProducts');"
                                                                                        style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="35">Select
                                                                                        Product(s) </a>
                                                                                </td>
                                                                                <td width="14%" align="left" valign="bottom">
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="display: none;">
                                                                                <td width="15%" valign="top">
                                                                                    Tag:
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:DropDownList ID="ddlTagName" class="order-list" Width="100px" runat="server"
                                                                                        TabIndex="36">
                                                                                        <asp:ListItem Text="Select One" Value=""></asp:ListItem>
                                                                                        <asp:ListItem Text="NewArrival" Value="NewArrival"></asp:ListItem>
                                                                                        <asp:ListItem Text="HotProduct" Value="HotProduct"></asp:ListItem>
                                                                                        <asp:ListItem Text="BestSeller" Value="BestSeller"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="display: none;">
                                                                                <td colspan="3">
                                                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 15%">
                                                                                                Satisfaction Guaranteed :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:CheckBox ID="chkSatisfactionGuaranteed" runat="Server" TabIndex="37" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                          <div id="divfloating" class="divfloatingcss" style="width:300px;">
                                                        <div style="margin-bottom: 1px;margin-top: 3px;"> 
                                                        <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return ValidatePage();"
                                                            OnClick="btnSave_Click" TabIndex="38" />
                                                        &nbsp;&nbsp;
                                                        <asp:ImageButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" TabIndex="39" />
                                                        </div>
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
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                </td>
            </tr>
        </table>
    </div>
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint"></iframe>
    </div>
    <!--start tab--->
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-1.2.6.min.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/tabs.js" type="text/javascript"></script>
    <!--end tab--->
</asp:Content>
