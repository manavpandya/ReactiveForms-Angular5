<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerQuote.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.CustomerQuote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/PhoneorderValidation.js"></script>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">




        function CheckCouponCode() {
            if ($('#ContentPlaceHolder1_txtCouponCode').val() != '')
            { return true; }
            else { jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode'); }
            return false;
        }

        $(document).ready(function () {
            //   return ShowHideButton('ImgProd', 'tdSelectedProd', 'divSelectedProd');
        });
        function loadpreview() {
           
            $.ajax({
                type: "GET",
                url: "CustomerQuote.aspx/PreiviewLoad",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: "true",
                cache: "false",
                success: function (msg) {

                },
                Error: function (x, e) {
                }
            });

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
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
                return true;
            return false;
        }
        function checkCount() {
            var totalRows = $("#<%=GVShoppingCartItems.ClientID %> tr").length;
            for (var i = 0; i < totalRows; i++) {
                if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_txtPrice_' + i)) {
                    var price = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_txtPrice_' + i).value;
                    var qty = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_txtQuantity_' + i).value;

                    if (price == "") {
                        jAlert("Please enter valid  Price!", "Message", "ContentPlaceHolder1_GVShoppingCartItems_txtPrice_" + i);
                        return false;
                    }
                    else if (qty == "" || qty <= 0) {
                        jAlert("Please enter valid  Quantity!", "Message", "ContentPlaceHolder1_GVShoppingCartItems_txtQuantity_" + i);
                        return false;
                    }
                }
            }
            return true;
        }

        function CalculateDiscount(rowid) {

            if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_txtPrice_' + rowid) != null && document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_txtQuantity_' + rowid) != null) {
                var price = parseFloat(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_txtPrice_' + rowid).value);
                var qty = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_txtQuantity_' + rowid).value;

                if (price > 0) {
                    if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_lblHeaderDiscount') != null && document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_lblHeaderDiscount').innerHTML != '') {
                        var DiscountPrice = parseFloat(document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_lblHeaderDiscount').innerHTML.replace('(', '').replace('%)', ''));

                        if (DiscountPrice >= 100) {
                            var Dispercent = ((price * DiscountPrice) / 100);
                            document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_lblOrginalDiscountPrice_' + rowid).innerHTML = parseFloat(Dispercent).toFixed(2);
                        }
                        else {
                            var Dispercent = ((price * DiscountPrice) / 100);
                            var finalDicPrice = price - Dispercent;
                            document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_lblOrginalDiscountPrice_' + rowid).innerHTML = parseFloat(finalDicPrice).toFixed(2);
                        }
                    }
                }
            }
        }

        function CalculateQuantity(rowid) {
            
            if (document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_lblQuantity_' + rowid) != null && document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_hdnProductID_' + rowid) != null) {

               
                var Qty = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_lblQuantity_' + rowid).value;
                var currentQty = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_txtQuantity_' + rowid).value;
                var pid = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_hdnProductID_' + rowid).value;
                var hdnVariantNames = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_hdnVariantNames_' + rowid).value;
                var hdnVariantvalues = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems_hdnVariantValues_' + rowid).value;
                var alllabl = document.getElementById('ContentPlaceHolder1_GVShoppingCartItems').getElementsByTagName('input');
                for (var i = 0; i < alllabl.length; i++) {
                    var cntrlid = alllabl[i];
                    if (cntrlid.id.toLowerCase().indexOf('hdnrelatedproductid') > -1) {
                        var rpid = cntrlid.value;
                       
                         

                        if (rpid == pid) {

                            var hdnVariantNameschild = document.getElementById(cntrlid.id.toString().replace('hdnRelatedproductID', 'hdnVariantNames')).value;
                            var hdnVariantvalueschild = document.getElementById(cntrlid.id.toString().replace('hdnRelatedproductID', 'hdnVariantValues')).value;
                            if (hdnVariantNames == hdnVariantNameschild && hdnVariantvalues == hdnVariantvalueschild) {

                                var rid = cntrlid.id.toString().replace('hdnRelatedproductID', 'lblQuantity');
                                var rtext = cntrlid.id.toString().replace('hdnRelatedproductID', 'txtQuantity');
                                
                                
                                //var rQty = document.getElementById(rid).value;
                                // rQty = parseInt(rQty) / parseInt(Qty);
                               // alert(document.getElementById(rid).value);
                                //rQty = parseInt(currentQty) 
                                document.getElementById(rtext).value = currentQty;
                                
                            }
                        }
                    }
                }

            }
        }

        function ShowHideButton(imgid, divid, divid1) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('expand.gif') > -1) {

                    if (ValidationNotLogin()) {
                        chkHeight();
                        document.getElementById(imgid).src = src.replace('expand.gif', 'minimize.png');
                        document.getElementById(imgid).title = 'Minimize';
                        document.getElementById(imgid).alt = 'Minimize';
                        document.getElementById(imgid).style.marginTop = "4px";
                        document.getElementById(imgid).className = 'minimize';
                        document.getElementById(divid).style.display = '';
                        if (document.getElementById(divid1)) {
                            document.getElementById(divid1).style.display = '';
                        }
                        document.getElementById('ContentPlaceHolder1_btnPreview').click();
                    }

                }
                else if (src.indexOf('minimize.png') > -1) {

                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    document.getElementById(divid).style.display = 'none';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = 'none';
                    }
                }
            }
        }
        function ShowHideButtonresetExpand(imgid, divid, divid1) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('minimize.png') > -1) {
                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    document.getElementById(divid).style.display = 'none';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = 'none';
                    }

                }
            }
        }
        function ShowHideButtonreset(imgid, divid, divid1) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('expand.gif') > -1) {
                    document.getElementById(imgid).src = src.replace('expand.gif', 'minimize.png');
                    document.getElementById(imgid).title = 'Minimize';
                    document.getElementById(imgid).alt = 'Minimize';
                    document.getElementById(imgid).style.marginTop = "4px";
                    document.getElementById(imgid).className = 'minimize';
                    document.getElementById(divid).style.display = '';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = '';
                    }
                }
                else if (src.indexOf('minimize.png') > -1) {

                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    document.getElementById(divid).style.display = 'none';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = 'none';
                    }

                }
            }
        }

        function ShowPreviewButton() {
            if (document.getElementById('ContentPlaceHolder1_rbMailList_1').checked == true) {

                document.getElementById('ContentPlaceHolder1_trPreview').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_trEmail').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_tablepreview').style.display = 'none';
                ShowHideButtonresetExpand('ImgProd', 'tdSelectedProd', 'divSelectedProd');
            }
            else {
                document.getElementById('ContentPlaceHolder1_trPreview').style.display = '';
                document.getElementById('ContentPlaceHolder1_tablepreview').style.display = '';
                ShowHideButtonresetExpand('ImgProd', 'tdSelectedProd', 'divSelectedProd');
            }
        }

        var myWindow;
        function onKeyPressBlockNumbers(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 0) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
        function ShoppingCartTotal() {
            var sCost = 0;
            var orderTax = 0;
            var orderDiscount = 0;
            var FinalSubTotal = 0;
            if (document.getElementById('ContentPlaceHolder1_TxtShippingCost') != null && document.getElementById('ContentPlaceHolder1_TxtShippingCost').value != '') {
                sCost = document.getElementById('ContentPlaceHolder1_TxtShippingCost').value;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtTax') != null && document.getElementById('ContentPlaceHolder1_TxtTax').value != '') {
                orderTax = document.getElementById('ContentPlaceHolder1_TxtTax').value;
            }
            if (document.getElementById('ContentPlaceHolder1_TxtDiscount') != null && document.getElementById('ContentPlaceHolder1_TxtDiscount').value != '') {
                if (document.getElementById('ContentPlaceHolder1_hdnIsSalesManager') != null && document.getElementById('ContentPlaceHolder1_hdnIsSalesManager').value == "1") {
                    orderDiscount = document.getElementById('ContentPlaceHolder1_TxtDiscount').value;
                    var SubTot = parseFloat(document.getElementById('ContentPlaceHolder1_lblSubTotal').innerHTML.replace("%", ""));
                    var Dispercent = (SubTot * 10) / 100;
                    if (parseFloat(orderDiscount) > parseFloat(Dispercent)) {
                        alert('Discount Should not be apply grater than 10% of SubTotal');
                        document.getElementById('ContentPlaceHolder1_TxtDiscount').value = "0";
                        document.getElementById('ContentPlaceHolder1_TxtDiscount').focus();
                        return;
                    }
                }
                else {
                    orderDiscount = document.getElementById('ContentPlaceHolder1_TxtDiscount').value;
                }
            }

            FinalSubTotal = parseFloat(parseFloat(document.getElementById('ContentPlaceHolder1_lblSubTotal').innerHTML) + parseFloat(sCost) + parseFloat(orderTax) - parseFloat(orderDiscount));
            if (parseFloat(FinalSubTotal.toString()) < 0) {
                FinalSubTotal = 0;
            }
            document.getElementById('ContentPlaceHolder1_lblTotal').innerHTML = parseFloat(FinalSubTotal).toFixed(2);
            document.getElementById('ContentPlaceHolder1_hfSubTotal').value = parseFloat(FinalSubTotal).toFixed(2);
            document.getElementById('ContentPlaceHolder1_hfTotal').value = document.getElementById('ContentPlaceHolder1_lblTotal').innerHTML;
        }

        //        function fpercent(quantity, percet) {
        //            return 
        //        }

        function OpenInventoryForUpgradeSKU(ProductID, StoreID, ParentProductID, CustomerCartId) {
            var width = 800;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('PhoneOrderVariantSearch.aspx?ProductId=' + ProductID + '&storeid=' + StoreID + '&parentProductID=' + ParentProductID + '&CustomerCartId=' + CustomerCartId, 'Mywindow', windowFeatures);
        }
        function openCenteredCrossSaleWindow(x, lblid) {
            createCookie('prskus', document.getElementById(x).value, 1);
            var width = 1000;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = document.getElementById('ContentPlaceHolder1_ddlStore').value;
            var CustID = document.getElementById('ContentPlaceHolder1_HdnCustID').value;
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;


            if (document.getElementById('ContentPlaceHolder1_hdnsearchlinksku') != null && document.getElementById('ContentPlaceHolder1_hdnsearchlinksku').value != '') {
                var searchlinksku = document.getElementById('ContentPlaceHolder1_hdnsearchlinksku').value;
                window.open('CustomerQuoteProductSku.aspx?StoreID=' + StoreID + '&clientid=' + x + "&lblID=" + lblid + "&CustID=" + CustID + "&subWind" + "&searchlinksku=" + searchlinksku, "Mywindow", windowFeatures);
                document.getElementById('ContentPlaceHolder1_hdnsearchlinksku').value = '';
            }
            else { window.open('CustomerQuoteProductSku.aspx?StoreID=' + StoreID + '&clientid=' + x + "&lblID=" + lblid + "&CustID=" + CustID + "&subWind", "Mywindow", windowFeatures); }

           
        }
        function openCenteredCrossSaleWindowRevise(x, lblid, CustomerQuoteId) {
            createCookie('prskus', document.getElementById(x).value, 1);
            var width = 1000;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = document.getElementById('ContentPlaceHolder1_ddlStore').value;
            var CustID = document.getElementById('ContentPlaceHolder1_HdnCustID').value;
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('CustomerQuoteProductSku.aspx?StoreID=' + StoreID + '&clientid=' + x + "&lblID=" + lblid + "&CustID=" + CustID + "&Mode=revise&CustomerQuoteId=" + CustomerQuoteId + "&subWind", "Mywindow", windowFeatures);
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
        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == variable) {
                    return pair[1];
                }
            }
        }


        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function copyfrombill(idfrom, idto) {
            if (document.getElementById('ContentPlaceHolder1_chkAddress').checked == true) { document.getElementById(idto).value = document.getElementById(idfrom).value; }
        }
        function ShowModelQuick() {
            disablePopup();
            document.getElementById("frmdisplayquick").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplayquick').height = '500px';
            document.getElementById('frmdisplayquick').width = '1000px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:1000px;height:530px;position:absolute;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact').style.width = '1000px';
            document.getElementById('popupContact').style.height = '530px';
            document.getElementById('frmdisplayquick').src = '/Admin/Orders/CustomerPhoneOrder.aspx?StoreId=' + document.getElementById('ContentPlaceHolder1_ddlStore').options[document.getElementById('ContentPlaceHolder1_ddlStore').selectedIndex].value + '&CustId=' + document.getElementById('ContentPlaceHolder1_HdnCustID').value + '&Quote=1';
            centerPopup();
            loadPopup();

        }
    </script>
    <div id="content-width">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" id="imgHeader" runat="server" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Customer Quote"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table" bgcolor="#ffffff">
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td align="right" class="new-order" colspan="2" style="width: 754px; height: 10px"
                                                                        valign="middle">
                                                                        <asp:Label ID="Label1" runat="server" CssClass="font-red"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trMsg" visible="false" runat="server">
                                                                    <td colspan="2">
                                                                        <center>
                                                                            <asp:Label ID="lblMsg" runat="server" ForeColor="red" CssClass="font-red" Text=""></asp:Label></center>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table width="100%" align="left" border="0" cellpadding="0" cellspacing="0" style="border: solid 1px #e7e7e7;">
                                                                            <tr class="oddrow">
                                                                                <td style="text-align: right; background-color: #e7e7e7;" colspan="2">
                                                                                    <span style="color: #ff0033">*</span> Required Fields
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td style="width: 129px; text-align: left;">
                                                                                    <span style="color: #ff0033">*</span>Store :
                                                                                </td>
                                                                                <td style="text-align: left;">
                                                                                    <asp:DropDownList ID="ddlStore" runat="server" Width="190" CssClass="product-type"
                                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td style="width: 129px; text-align: left;">
                                                                                    <span style="color: #ff0033">*</span>Email :
                                                                                </td>
                                                                                <td style="text-align: left;">
                                                                                    <asp:TextBox ID="TxtEmail" Width="300px" CssClass="order-textfield" runat="server"></asp:TextBox>&nbsp;<a
                                                                                        href="javascript:void(0);" onclick="ShowModelQuick();">Select Customer</a>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">
                                                                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSave">
                                                                            <table id="Table2" runat="server" align="left" border="0" style="border: solid 1px #e7e7e7;"
                                                                                cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr class="altrow">
                                                                                    <td colspan="2" style="background-color: #e7e7e7;">
                                                                                        <b>Billing Address </b>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td align="left" class="font-black01" style="width: 129px; height: 7px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>First Name :
                                                                                    </td>
                                                                                    <td align="left" style="height: 7px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_FName" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_FName','ContentPlaceHolder1_txtS_FName');"
                                                                                            runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>Last Name :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_LName" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_LName','ContentPlaceHolder1_txtS_LNAme');"
                                                                                            runat="server" CssClass="order-textfield"></asp:TextBox><span style="color: #ff0033">
                                                                                            </span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        &nbsp; Company :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_Company" MaxLength="100" runat="server" CssClass="order-textfield"
                                                                                            onkeyup="copyfrombill('ContentPlaceHolder1_txtB_Company','ContentPlaceHolder1_txtS_Company');"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow" valign="top">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="top">
                                                                                        <span style="color: #ff0033">*</span><span style="color: #ff0033"></span>Address1
                                                                                        :
                                                                                    </td>
                                                                                    <td align="left" style="color: #ff0033; height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_Add1" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_Add1','ContentPlaceHolder1_txtS_Add1');"
                                                                                            runat="server" Columns="6" CssClass="order-textfield" Rows="6" Width="250px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="top">
                                                                                        &nbsp; Address2 :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_Add2" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_Add2','ContentPlaceHolder1_txtS_Add2');"
                                                                                            runat="server" CssClass="order-textfield" Width="250px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow" valign="top">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        &nbsp; Suite :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_Suite" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_Suite','ContentPlaceHolder1_txtS_Suite');"
                                                                                            runat="server" CssClass="order-textfield"></asp:TextBox>&nbsp;
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>City :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_City" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_City','ContentPlaceHolder1_txtS_City');"
                                                                                            runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow" valign="top">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>Country :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:DropDownList ID="ddlB_Country" Style="padding-left: 3px;" runat="server" AutoPostBack="True"
                                                                                            onchange="chkHeight();" CssClass="product-type" Width="250" OnSelectedIndexChanged="ddlB_Country_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>State :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px;" valign="middle">
                                                                                        <asp:DropDownList ID="ddlB_State" Style="padding-left: 3px;" Width="250" runat="server"
                                                                                            CssClass="product-type" AutoPostBack="True" OnSelectedIndexChanged="ddlB_State_SelectedIndexChanged"
                                                                                            onchange="chkHeight();">
                                                                                        </asp:DropDownList>
                                                                                        &nbsp;
                                                                                        <div id="DIVBillingOther" style="display: none;">
                                                                                            <asp:Label ID="Label4" runat="server" CssClass="order-textfield" Height="30px" Text="If other, please specify"
                                                                                                Visible="False"></asp:Label><span class="list_table_cell_link">
                                                                                                    <br />
                                                                                                    <asp:TextBox ID="txtB_OtherState" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_OtherState','ContentPlaceHolder1_txtS_OtherState');"
                                                                                                        runat="server" CssClass="order-textfield" Width="120px" Visible="False"></asp:TextBox>
                                                                                                </span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow" valign="top">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>Zip :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_Zip" Width="65" MaxLength="9" runat="server" CssClass="order-textfield"
                                                                                            onkeyup="copyfrombill('ContentPlaceHolder1_txtB_Zip','ContentPlaceHolder1_txtS_Zip');"
                                                                                            onchange="chkHeight();" AutoPostBack="true" OnTextChanged="txtB_Zip_TextChanged"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>Phone :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_Phone" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_Phone','ContentPlaceHolder1_txtS_Phone');"
                                                                                            runat="server" CssClass="order-textfield" MaxLength="20"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                        <asp:Panel ID="pnlCustomer" runat="server" DefaultButton="btnSave">
                                                                            <table id="Table1" runat="server" align="left" border="0" style="border: solid 1px #e7e7e7;"
                                                                                cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr class="altrow">
                                                                                    <td colspan="2" style="background-color: #e7e7e7;">
                                                                                        <b>Shipping Address </b><b style="text-align: right; float: right;">
                                                                                            <asp:CheckBox ID="chkAddress" runat="server" Text="  Ship to my billing address"
                                                                                                AutoPostBack="true" onchange="chkHeight();" onclick="chkHeight();" OnCheckedChanged="chkAddress_CheckedChanged" />
                                                                                        </b>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td align="left" class="font-black01" style="height: 7px; width: 129px;" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>First Name :
                                                                                    </td>
                                                                                    <td align="left" style="height: 7px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_FName" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>Last Name :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_LNAme" runat="server" CssClass="order-textfield"></asp:TextBox><span
                                                                                            style="color: #ff0033"> </span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        &nbsp; Company :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_Company" MaxLength="100" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow" valign="top">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="top">
                                                                                        <span style="color: #ff0033">*</span><span style="color: #ff0033"></span>Address1
                                                                                        :
                                                                                    </td>
                                                                                    <td align="left" style="color: #ff0033; height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_Add1" runat="server" Columns="6" CssClass="order-textfield"
                                                                                            Rows="6" Width="250px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="top">
                                                                                        &nbsp; Address2 :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_Add2" runat="server" CssClass="order-textfield" Width="250px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        &nbsp; Suite :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_Suite" runat="server" CssClass="order-textfield"></asp:TextBox>&nbsp;
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>City :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_City" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>Country :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:DropDownList ID="ddlS_Country" Style="padding-left: 3px;" Width="250" runat="server"
                                                                                            onchange="chkHeight();" AutoPostBack="True" CssClass="product-type" OnSelectedIndexChanged="ddlS_Country_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>State :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:DropDownList ID="ddlS_State" Style="padding-left: 3px;" Width="250" runat="server"
                                                                                            onchange="chkHeight();" CssClass="product-type" AutoPostBack="True" OnSelectedIndexChanged="ddlS_State_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                        &nbsp;
                                                                                        <div id="DIVShippingOther" style="display: none;">
                                                                                            <asp:Label ID="lblS_OtherState" runat="server" CssClass="order-textfield" Height="30px"
                                                                                                Text="If other, please specify" Visible="False"></asp:Label><span class="list_table_cell_link">
                                                                                                    <br />
                                                                                                    <asp:TextBox ID="txtS_OtherState" runat="server" CssClass="order-textfield" Width="120px"
                                                                                                        Visible="false"></asp:TextBox>
                                                                                                </span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>Zip :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_Zip" Width="65" MaxLength="9" runat="server" CssClass="order-textfield"
                                                                                            onchange="chkHeight();" AutoPostBack="true" OnTextChanged="txtB_Zip_TextChanged"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">
                                                                                        <span style="color: #ff0033">*</span>Phone :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_Phone" runat="server" CssClass="order-textfield" MaxLength="20"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" colspan="2">
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border: solid 1px #e7e7e7;">
                                                                            <tr class="altrow">
                                                                                <td colspan="2" style="text-align: left; background-color: #e7e7e7;">
                                                                                    <b>General Information </b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <table id="Table3" runat="server" align="left" border="0" style="border: solid 1px #e7e7e7;"
                                                                                        cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr class="altrow">
                                                                                            <td align="left" class="font-black01" style="height: 7px; width: 129px;" valign="middle">
                                                                                                <span style="color: #ff0033">*</span>Send Mail :
                                                                                            </td>
                                                                                            <td align="left" style="height: 7px" valign="middle" colspan="2">
                                                                                                <asp:RadioButtonList ID="rbMailList" runat="server" RepeatDirection="Horizontal"
                                                                                                    onchange="ShowPreviewButton();">
                                                                                                    <asp:ListItem Value="0" Text=" HTML" Selected="true"></asp:ListItem>
                                                                                                    <asp:ListItem Value="1" Text=" PDF"></asp:ListItem>
                                                                                                </asp:RadioButtonList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td align="left" class="font-black01" style="height: 7px; width: 129px;" valign="middle">
                                                                                                <span style="color: #ff0033">*</span>General Note:
                                                                                            </td>
                                                                                            <td align="left" style="height: 7px" valign="middle" colspan="2">
                                                                                                <asp:TextBox ID="TxtNotes" TextMode="multiLine" Height="51" Width="325" CssClass="order-textfield"
                                                                                                    runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td align="left" class="font-black01" style="height: 7px; width: 129px;" valign="middle">
                                                                                                <span style="color: #ff0033">&nbsp;</span>Coupon Code:
                                                                                            </td>
                                                                                            <td align="left" style="height: 7px; width: 90px;" valign="middle">
                                                                                                <asp:TextBox ID="txtCouponCode" Width="80" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click"
                                                                                                    OnClientClick="return CheckCouponCode();" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td colspan="2">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="padding-top: 4px; text-align: left; padding-left: 4px;" colspan="2">
                                                                        <asp:ImageButton ID="aRelated" runat="server" OnClick="aRelated_Click" ImageUrl="" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding: 4px;" colspan="2">
                                                                                                <div id="divshoppingcartItems" class="table_border">
                                                                                                    <asp:GridView ID="GVShoppingCartItems" runat="server" CssClass="table-noneforOrder"
                                                                                                        AutoGenerateColumns="false" OnRowDataBound="GVShoppingCartItems_RowDataBound"
                                                                                                        Width="100%" CellPadding="0" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-Font-Bold="true"
                                                                                                        EmptyDataRowStyle-ForeColor="Red" EmptyDataText="Your Shopping Cart is Empty."
                                                                                                        CellSpacing="0" BorderStyle="Solid" GridLines="None" BorderWidth="1" BorderColor="#e7e7e7">
                                                                                                        <EmptyDataRowStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Red"></EmptyDataRowStyle>
                                                                                                        <EmptyDataTemplate>
                                                                                                            <span style="color: Red;">Your Shopping Cart is Empty.</span></EmptyDataTemplate>
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField Visible="false">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblCustomerQuoteItemID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustomerQuoteItemID") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                                                                     <asp:Label ID="lblRelatedproductID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"RelatedproductID") %>'></asp:Label>
                                                                                                               
                                                                                          <%--  <asp:Label ID="lblIndiSubTotal" runat="server" Visible="false" Text='<%# String.Format("{0:C}", Convert.ToDecimal(Eval("IndiSubTotal")))%>'></asp:Label>
                                                                                            <asp:Label ID="lblSubTot" runat="server" Text='<%# Eval("IndiSubTotal")%>' Visible="false"></asp:Label>
                                                                                            --%>
                                                                                            <asp:HiddenField ID="hdnswatchqty" runat="server" Value='0' />
                                                                                            <asp:HiddenField ID="hdnswatchtype" runat="server" Value='' />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    &nbsp; Product Name
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                                                                                                       <input type="hidden" ID="lblQuantity" runat="server" value ='<%# Eval("Quantity")%>' />
                                                                                                                   <input type="hidden" ID="hdnRelatedproductID" runat="server" value ='<%# Eval("RelatedproductID")%>' />
                                                                                                                   <input type="hidden" ID="hdnProductID" runat="server" value ='<%# Eval("ProductID")%>' />
                                                                                                                     <input id="hdnVariantNames" type="hidden" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></input>
                                                                                                                    <input id="hdnVariantValues" type="hidden" runat="server"  value='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></input>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle Width="50%" Font-Size="12px" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField Visible="false">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                                                                                                      <asp:Label ID="lblProductType" runat="server" Visible="false" Text='<%# Eval("IsProductType")%>'></asp:Label>
                                                                                              <asp:Label ID="lblisProductType" runat="server" Visible="false" Text='<%# Eval("isProductType")%>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderTemplate>
                                                                                                                    &nbsp; Items
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemStyle Font-Size="12px" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <%# Eval("SKU") %>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderTemplate>
                                                                                                                    &nbsp;SKU
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemStyle Font-Size="12px" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    $<asp:TextBox ID="txtPrice" runat="server" CssClass="order-textfield" Style="text-align: right;"
                                                                                                                        onkeypress="return keyRestrict(event,'0123456789.');" Width="60px" Text='<%#  Math.Round(Convert.ToDecimal(Eval("Price")),2)%> '></asp:TextBox>
                                                                                                                    <asp:Label ID="lblSwatchTot" runat="server" Visible="false"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderTemplate>
                                                                                                                    &nbsp;Price
                                                                                                                </HeaderTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                                                <ItemStyle HorizontalAlign="right" Font-Size="12px" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblDiscountPrice" runat="server" Text='<%# Eval("DiscountPercent")%>'
                                                                                                                        Visible="false"></asp:Label>
                                                                                                                    $<asp:Label ID="lblOrginalDiscountPrice" runat="server" Text='<%#  Math.Round(Convert.ToDecimal(Eval("DiscountPrice")),2)%> '></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderTemplate>
                                                                                                                    Discount Price
                                                                                                                    <asp:Label ID="lblHeaderDiscount" runat="server"></asp:Label>
                                                                                                                </HeaderTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                                                <ItemStyle HorizontalAlign="right" Font-Size="12px" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="order-textfield" Style="text-align: center;"
                                                                                                                        onkeypress="return onKeyPressBlockNumbers(event);" Width="60px" Text='<%# Eval("Quantity")%>'></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderTemplate>
                                                                                                                    &nbsp;Quantity
                                                                                                                </HeaderTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" Font-Size="12px" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    &nbsp; Notes
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtProductNotes" runat="server" CssClass="order-textfield" Height="40px"
                                                                                                                        Width="240px" TextMode="MultiLine" Text='<%# Eval("Notes")%>'></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle Width="12%" Font-Size="12px" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <AlternatingRowStyle CssClass="altrow" VerticalAlign="top" />
                                                                                                        <RowStyle BackColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px"
                                                                                                            Height="24px" HorizontalAlign="Left" />
                                                                                                    </asp:GridView>
                                                                                                    <asp:Literal ID="ltshoppingcartItems" runat="server"></asp:Literal>
                                                                                                </div>
                                                                                                <div style="display: none;">
                                                                                                    <asp:Button ID="btnshoppingcartitems" runat="server" Text="Savesadfasd" OnClientClick="chkHeight();"
                                                                                                        OnClick="btnshoppingcartitems_click" />
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trUpdateProduct" runat="server" visible="false">
                                                                                            <td style="width: 50%">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td valign="top" align="right" style="width: 50%">
                                                                                                <asp:ImageButton ID="btnUpdateProduct" runat="server" OnClick="btnUpdateProduct_Click"
                                                                                                    OnClientClick="return checkCount();" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <table width="100%" class="table">
                                                                                                    <tr id="tablepreview" style="display: none;" runat="server" class="altrow">
                                                                                                        <td colspan="2" style="text-align: left; background-color: #e7e7e7;">
                                                                                                            <div class="main-title-left" onclick="return ShowHideButton('ImgProd','tdSelectedProd','divSelectedProd');"
                                                                                                                style="cursor: pointer; color: #6A6A6A; font-family: Arial,Helvetica,sans-serif;
                                                                                                                font-size: 12px; background-color: #E7E7E7; text-align: left; height: 18px; padding-top: 7px;
                                                                                                                width: 95%; padding-left: 7px;">
                                                                                                                <b>Preview </b>
                                                                                                            </div>
                                                                                                            <div>
                                                                                                                <a href="javascript:void(0);" class="show_hideCateInfo" onclick="return ShowHideButton('ImgProd','tdSelectedProd','divSelectedProd');">
                                                                                                                    <img id="ImgProd" class="minimize" title="Show" alt="Show" src="/App_Themes/<%=Page.Theme %>/images/expand.gif"
                                                                                                                        style="cursor: pointer; padding-right: 10px; padding-top: 5px; float: right;" /></a>
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="2" id="tdSelectedProd">
                                                                                                            <table width="100%">
                                                                                                                <div id="divSelectedProd">
                                                                                                                    <tr id="trPreview" runat="server" style="display: none;">
                                                                                                                        <td align="left" style="height: 30px; display: none;" colspan="2">
                                                                                                                            <asp:ImageButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr id="trEmail" runat="server" style="display: none;">
                                                                                                                        <td align="center" style="height: 30px" colspan="2">
                                                                                                                            <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Width="400px" Height="450px"
                                                                                                                                CssClass="order-textfield"></asp:TextBox>
                                                                                                                            <script type="text/javascript">
                                                                                                                                CKEDITOR.replace('<%= txtBody.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400, width: 700 });
                                                                                                                                CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                                                                    if (ev.data.name == 'image') {
                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                        btn.hidden = false;
                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=580,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                    }
                                                                                                                                    if (ev.data.name == 'link') {
                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                        btn.hidden = false;
                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=420,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                    }
                                                                                                                                });
                                                                                                                            </script>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </div>
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
                                                                <tr class="oddrow">
                                                                    <td align="center" style="height: 30px" colspan="2">
                                                                        <asp:ImageButton ID="btnSave" runat="server" OnClientClick="if(ValidationNotLogin()){chkHeight();return true;}else{return false;};"
                                                                            OnClick="btnSave_Click" />
                                                                        &nbsp;
                                                                        <asp:ImageButton ID="BtnCancel" runat="server" CausesValidation="False" OnClick="BtnCancel_Click"
                                                                            ValidationGroup="Customer" />
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
    </div>
    <asp:HiddenField ID="HdnCustID" runat="server" Value="0" />
    <asp:HiddenField ID="HdnSubTotal" runat="server" Value="0" />
    <asp:HiddenField ID="HdnTotal" runat="server" Value="0" />
    <asp:HiddenField ID="hdnsearchlinksku" runat="server" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Customer"
        ShowSummary="false" />
    <div style="display: none;">
        <input type="hidden" id="hdncountry" runat="server" value="" />
        <input type="hidden" id="hdnState" runat="server" value="" />
        <input type="hidden" id="hdnZipCode" runat="server" value="" />
        <input type="hidden" id="hdnTaxRate" runat="server" value="0" />
        <input type="hidden" id="hdnIsReviseQuote" runat="server" value="0" />
        <asp:ImageButton ID="imgdefaulttemp" runat="server" OnClientClick="javascript:return false;" />

    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <asp:Label ID="lblSubTotal" Text="0.00" runat="server"></asp:Label>
        <asp:HiddenField ID="hfSubTotal" runat="Server" Value="0" />
        <asp:Label ID="lblTotal" Text="0.00" runat="server"></asp:Label>
        <asp:HiddenField ID="hfTotal" Value="0" runat="Server" />
    </div>
    <div id="popupContact" style="z-index: 1000001; width: 750px; height: 250px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="border: 1px solid #444;
            background-color: #fff; font-size: 12px;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;Select Customer
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupContactClose" Style="position: relative;" ImageUrl="~/images/cancel-icon.png"
                        runat="server" ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <iframe id="frmdisplayquick" frameborder="0" height="650" width="580" scrolling="auto">
                                </iframe>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div>
    </div>
</asp:Content>
