<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="PhoneOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.PhoneOrder" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js?56"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/PhoneorderValidation.js?44444"></script>
    <%--<script src="https://connect.chargelogic.com/ChargeLogicConnectEmbed.js" type="text/javascript"></script>
    <script src="https://servername.chargelogic.com/ChargeLogicConnectEmbed.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <div id="popupContact" style="z-index: 1000001; width: 750px; height: 250px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="border: 1px solid #444; background-color: #fff; font-size: 12px;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif; font-weight: bold;">&nbsp;Search Customer
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
                                <iframe id="frmdisplayquick" frameborder="0" height="650" width="580" scrolling="auto"></iframe>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function iframerealod(id, randomnum) {
            document.getElementById('ContentPlaceHolder1_btnSave').style.display = 'none'; var hh = $('#iframecreditcard').innerHeight(); $('#prepageiframe').height(hh); $('#prepageiframe').show(); $('#iframecreditcard').load(function () { $('#prepageiframe').height(hh); $('#prepageiframe').hide(); document.getElementById('ContentPlaceHolder1_btnSave').style.display = ''; }).each(function () { if (this.complete) $(this).load(); });
           //  var hh = $('#iframecreditcard').innerHeight(); $('#prepageiframe').height(hh); $('#prepageiframe').show(); $('#iframecreditcard').load(function () { $('#prepageiframe').height(hh); $('#prepageiframe').hide();}).each(function () { if (this.complete) $(this).load(); });
            document.getElementById('iframecreditcard').src = 'https://connect.chargelogic.com/?HostedPaymentID=' + id;
            document.getElementById('prepage').style.display = 'none';
        }
        function confirmmsg(OrderNumber)
        {
            jConfirmok('Order Placed Successfully. \n Order #: ' + OrderNumber + ' \n Press ok button for order list', 'Confirmation');
        }
        function CheckCouponCode() {
            if ($('#ContentPlaceHolder1_txtCouponCode').val() != '')
            { return true; }
            else { jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode'); }
            return false;
        }
        function CheckStorecreaditCode() {
            if ($('#ContentPlaceHolder1_txtstorecreaditno').val() == '') {
                jAlert('Please Enter Valid number!', 'Message', 'ContentPlaceHolder1_txtstorecreaditno');
                return false;
            }
            //if ($('#ContentPlaceHolder1_txtstorecreaditAmount').val() == '') {
            //    jAlert('Please Enter Valid Amount!', 'Message', 'ContentPlaceHolder1_txtstorecreaditAmount');
            //    return false;
            //}
            return true;
        }
        function ChangeItemPrice(rowid, price) {
            // alert('test');
            // alert(rowid);
            // alert(price);
            // alert(document.getElementById(rowid).innerText);
            //  alert(document.getElementById(price).value);
            $.ajax(
                      {
                          type: "POST",
                          url: "/TestMail.aspx/ChangePhoneOrderPrice",
                          data: "{CustomCartID: " + document.getElementById(rowid).innerHTML + ",price: '" + document.getElementById(price).value + "' }",
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          async: "true",
                          cache: "false",
                          success: function (msg) {

                              // alert(msg.d);
                              document.getElementById('ContentPlaceHolder1_btnshoppingcartitems').click();


                          },
                          Error: function (x, e) {
                              alert(e.Error);
                          }
                      });


        }


        function ChangeDiscountPrice(rowid, price) {
          

            if (document.getElementById(price) != null && document.getElementById(price).value == '') {
               
                document.getElementById(price).value = 0;
               
            }
            $.ajax(
                      {
                          type: "POST",
                          url: "/TestMail.aspx/ChangePhoneOrderCouponPrice",
                          data: "{CustomCartID: " + document.getElementById(rowid).innerHTML + ",price: '" + document.getElementById(price).value + "' }",
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          async: "true",
                          cache: "false",
                          success: function (msg) {

                            
                              document.getElementById('ContentPlaceHolder1_btnshoppingcartitems').click();


                          },
                          Error: function (x, e) {
                              alert(e.Error);
                          }
                      });
        


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
            if (document.getElementById('ContentPlaceHolder1_txtstorecreaditAmount') != null && document.getElementById('ContentPlaceHolder1_txtstorecreaditAmount').value != '')
            {
                var applystorecredit = parseFloat(FinalSubTotal) - parseFloat(document.getElementById('ContentPlaceHolder1_txtstorecreaditAmount').value);
                if (parseFloat(applystorecredit) < parseFloat(0))
                {
                    applystorecredit = 0;
                }
                document.getElementById('ContentPlaceHolder1_txtBoxcaptureamount').value = applystorecredit.toFixed(2);
            }
            else {
                document.getElementById('ContentPlaceHolder1_txtBoxcaptureamount').value = parseFloat(FinalSubTotal).toFixed(2);
            }
            


        }

        function onchangediscount()
        {
            if (document.getElementById('ContentPlaceHolder1_btndiscount') != null) {
                document.getElementById('ContentPlaceHolder1_btndiscount').click();
            }
        }

        //        function fpercent(quantity, percet) {
        //            return 
        //        }

        function OpenInventoryForUpgradeSKU(ProductID, StoreID, ParentProductID, CustomerCartId) {

            var width = 800;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));



            //if(lblid!='')x=lblid;
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            //window.open('OrderProductSku.aspx?StoreID=' + StoreID + '&clientid=' + x + "&lblID=" + lblid + "&CustID=" + CustID + "&subWind", "Mywindow", windowFeatures);

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


            //if(lblid!='')x=lblid;
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;

            if (document.getElementById('ContentPlaceHolder1_hdnsearchlinksku') != null && document.getElementById('ContentPlaceHolder1_hdnsearchlinksku').value != '') {
                var searchlinksku = document.getElementById('ContentPlaceHolder1_hdnsearchlinksku').value;
                window.open('OrderProductSku.aspx?StoreID=' + StoreID + '&clientid=' + x + "&lblID=" + lblid + "&CustID=" + CustID + "&subWind" + "&searchlinksku=" + searchlinksku, "Mywindow", windowFeatures);
                document.getElementById('ContentPlaceHolder1_hdnsearchlinksku').value = '';
            }

            else {  window.open('OrderProductSku.aspx?StoreID=' + StoreID + '&clientid=' + x + "&lblID=" + lblid + "&CustID=" + CustID + "&subWind", "Mywindow", windowFeatures); }

        }
        function openCenteredCrossSaleWindowmarry(x, lblid) {
            //    if(lblid!='')createCookie('prskus',document.getElementById(lblid).innerHTML,1);
            //    else
            createCookie('prskus', document.getElementById(x).value, 1);
            var width = 1000;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = document.getElementById('ContentPlaceHolder1_ddlStore').value;
            var CustID = document.getElementById('ContentPlaceHolder1_HdnCustID').value;

            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;


            window.open('OrderProductSku.aspx?StoreID=' + StoreID + '&clientid=' + x + "&marrypro=1&lblID=" + lblid + "&CustID=" + CustID + "&subWind", "Mywindow", windowFeatures);
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
            document.getElementById('frmdisplayquick').src = '/Admin/Orders/CustomerPhoneOrder.aspx?StoreId=' + document.getElementById('ContentPlaceHolder1_ddlStore').options[document.getElementById('ContentPlaceHolder1_ddlStore').selectedIndex].value + '&CustId=' + document.getElementById('ContentPlaceHolder1_HdnCustID').value;
            centerPopup();
            loadPopup();

        }

        function Showcallrefbox(selectedvalue) {
            if (selectedvalue.toLowerCase() == "other") {
                document.getElementById('tdother').style.display = '';
            }
            else { document.getElementById('tdother').style.display = 'none'; }

        }


        function Showsearcklinksku() {
            document.getElementById('ContentPlaceHolder1_aRelated').click();

        }
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;
            </div>
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
                                                    <img class="img-left" title="Sale Order" alt="Sale Order" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Sale Order"></asp:Label></h2>
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
                                                                                        href="javascript:void(0);" onclick="ShowModelQuick();">Search Customer</a>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td style="width: 129px; text-align: left;">
                                                                                    <%-- <span style="color: #ff0033">*</span>--%> Customer Type :
                                                                                </td>
                                                                                <td style="text-align: left;">
                                                                                    <asp:DropDownList ID="ddlcustomertype" runat="server" Width="160" CssClass="product-type">
                                                                                        <%--<asp:ListItem Text="Select Customer Type" Value="0" Selected="true"></asp:ListItem>--%>
                                                                                        <asp:ListItem Text="Retails" Value="Retails"></asp:ListItem>
                                                                                        <asp:ListItem Text="Trade" Value="Trade"></asp:ListItem>
                                                                                    </asp:DropDownList>
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
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">&nbsp; Company :
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
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="top">&nbsp; Address2 :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtB_Add2" onkeyup="copyfrombill('ContentPlaceHolder1_txtB_Add2','ContentPlaceHolder1_txtS_Add2');"
                                                                                            runat="server" CssClass="order-textfield" Width="250px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow" valign="top">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">&nbsp; Suite :
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
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">&nbsp; Company :
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
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="top">&nbsp; Address2 :
                                                                                    </td>
                                                                                    <td align="left" style="height: 22px" valign="middle">
                                                                                        <asp:TextBox ID="txtS_Add2" runat="server" CssClass="order-textfield" Width="250px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td align="left" class="font-black01" style="height: 22px" valign="middle">&nbsp; Suite :
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
                                                                <tr class="oddrow">
                                                                    <td valign="top">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td valign="middle" align="left" style="height: 22px; width: 126px;" class="font-black01">Is Registered :</td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkisregister" runat="server" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border: solid 1px #e7e7e7;">
                                                                            <tr style="text-align: left;" class="altrow">
                                                                                <td style="width: 120px; text-align: left; background-color: #e7e7e7;">
                                                                                    <b>Payment &nbsp; Method : </b>
                                                                                </td>
                                                                                <td style="text-align: left; background-color: #e7e7e7;" align="left">
                                                                                    <asp:RadioButton ID="rdoCreditCard" runat="server"   GroupName="PaymentMethod"
                                                                                        Checked="true" Text=" Credit Card" Font-Bold="true" ValidationGroup="Payment"
                                                                                        OnCheckedChanged="rdoCreditCard_CheckedChanged" />
                                                                                    &nbsp;&nbsp;
                                                                                    <asp:RadioButton ID="rdoCheque" runat="server" AutoPostBack="true" GroupName="PaymentMethod"
                                                                                        Text=" Other" Font-Bold="true" ValidationGroup="Payment" OnCheckedChanged="rdoCheque_CheckedChanged" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <table>
                                                                            <tr>
                                                                                <td  style="width: 129px; text-align: left;">Captured Amount($):</td>
                                                                                <td  style="text-align: left;"> <asp:TextBox ID="txtBoxcaptureamount"  onkeypress='return onKeyPressBlockNumbers(event);'  runat="server" CssClass="order-textfield"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblCheque" style="border: solid 1px #e7e7e7;"
                                                                            visible="false" runat="server">
                                                                            <tr class="oddrow">
                                                                                <td style="width: 129px; text-align: left;">
                                                                                    <span style="color: #ff0033" id="span1">*</span>Reference Detail :
                                                                                </td>
                                                                                <td style="text-align: left; padding-left: 0px;">
                                                                                    <asp:TextBox ID="txtCheque" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblCreditCard"
                                                                            style="border: solid 1px #e7e7e7;" runat="server">
                                     <tr>
                                         <td colspan="2">
                                             <asp:Label ID="lblchargelogicError" runat="server" ForeColor="Red"></asp:Label>
                                         </td>
                                     </tr>
                                                                            <tr class="oddrow">
                                                                                <td style="width: 129px; text-align: left;">
                                                                                    <span style="color: #ff0033">*</span>Name on Card :
                                                                                </td>
                                                                                <td style="text-align: left; padding-left: 0px;">
                                                                                    <asp:TextBox ID="TxtNameOnCard" AutoCompleteType="None" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td style="text-align: left">
                                                                                    <span style="color: #ff0033">*</span>Card Type :
                                                                                </td>
                                                                                <td style="text-align: left; padding-left: 0px;">
                                                                                    <asp:DropDownList ID="ddlCardType" AutoCompleteType="None" runat="server" CssClass="product-type" Width="150">
                                                                                    </asp:DropDownList>
                                                                                    <%--<asp:TextBox ID="TxtCardType" runat="server" CssClass="order-textfield"></asp:TextBox>--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td style="width: 129px; text-align: left">
                                                                                    <span style="color: #ff0033">*</span>Card Number :
                                                                                </td>
                                                                                <td style="text-align: left; padding-left: 0px;">
                                                                                    <asp:TextBox ID="TxtCardNumber"    MaxLength="16" Width="120" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                    &nbsp;&nbsp;&nbsp;&nbsp; CVC :&nbsp;
                                                                                    <asp:TextBox ID="TxtCardVerificationCode" runat="server" TextMode="Password"  OnPreRender="TxtCardVerificationCode_PreRender" Width="40" MaxLength="4"
                                                                                        CssClass="order-textfield"></asp:TextBox>
                                                                                    <asp:Label ID="lblCardNumber" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td style="text-align: left">
                                                                                    <span style="color: #ff0033">*</span>Expiration Date :
                                                                                </td>
                                                                                <td style="text-align: left; padding-left: 0px;">
                                                                                    <asp:DropDownList ID="ddlMonth" Style="padding-left: 3px;" runat="server" CssClass="product-type"
                                                                                        Width="75px">
                                                                                        <asp:ListItem Value="0">Month</asp:ListItem>
                                                                                        <asp:ListItem Value="1">Jan(01)</asp:ListItem>
                                                                                        <asp:ListItem Value="2">Feb(02)</asp:ListItem>
                                                                                        <asp:ListItem Value="3">Mar(03)</asp:ListItem>
                                                                                        <asp:ListItem Value="4">Apr(04)</asp:ListItem>
                                                                                        <asp:ListItem Value="5">May(05)</asp:ListItem>
                                                                                        <asp:ListItem Value="6">June(06)</asp:ListItem>
                                                                                        <asp:ListItem Value="7">July(07)</asp:ListItem>
                                                                                        <asp:ListItem Value="8">Aug(08)</asp:ListItem>
                                                                                        <asp:ListItem Value="9">Sept(09)</asp:ListItem>
                                                                                        <asp:ListItem Value="10">Oct(10)</asp:ListItem>
                                                                                        <asp:ListItem Value="11">Nov(11)</asp:ListItem>
                                                                                        <asp:ListItem Value="12">Dec(12)</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="product-type" Width="69px"
                                                                                        Style="padding-left: 5px;">
                                                                                        <asp:ListItem Value="0">Year</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border: solid 1px #e7e7e7;">
                                                                            <tr class="altrow">
                                                                                <td colspan="2" style="text-align: left; background-color: #e7e7e7;">
                                                                                    <b>General Order Notes</b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow" style="display: none">
                                                                                <td style="width: 129px;">LastIPAddress :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TxtLastIPAddress" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="display: none">
                                                                                <td>Reference Order ID :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TxtReferenceOrderID" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr style="text-align: left;" class="oddrow">
                                                                                            <td style="width: 120px; text-align: left;">Notes :
                                                                                            </td>
                                                                                            <td style="text-align: left;" align="left" colspan="2">
                                                                                                <asp:TextBox ID="TxtNotes" TextMode="multiLine" Height="50" Width="325" CssClass="order-textfield"
                                                                                                    runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>

                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td colspan="2"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td style="width: 120px; text-align: left;">Call Reference :</td>

                                                                                            <td align="left" style="text-align: left; padding-right: 10px;">
                                                                                                <asp:DropDownList ID="ddlcallref" runat="server" CssClass="product-type" Width="100px" onchange="Showcallrefbox(this.value);">
                                                                                                    <asp:ListItem Text="Select " Value="0"> </asp:ListItem>
                                                                                                    <asp:ListItem Text="Google" Value="Google"> </asp:ListItem>
                                                                                                    <asp:ListItem Text="Bing" Value="Bing"> </asp:ListItem>
                                                                                                    <asp:ListItem Text="Advertise" Value="Advertise"> </asp:ListItem>
                                                                                                    <asp:ListItem Text="Other" Value="other"> </asp:ListItem>
                                                                                                </asp:DropDownList></td>

                                                                                            <td id="tdother" style="display: none">
                                                                                                <asp:TextBox ID="txtrefother" runat="server"></asp:TextBox></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>

                                                                </tr>

                                                                <tr>
                                                                    <td style="padding-top: 4px; text-align: left; padding-left: 4px;" colspan="2">
                                                                        <%--<asp:ImageButton ID="aRelated" runat="server" OnClientClick="if(ValidatePage()){chkHeight();return true;}else{return false;}"
                                                                            OnClick="aRelated_Click" ImageUrl="" />--%>
                                                                        <asp:ImageButton ID="aRelated" runat="server" OnClick="aRelated_Click" ImageUrl="" />
                                                                    </td>
                                                                </tr>
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
                                                                                    <span style="color: Red;">Your Shopping Cart is Empty.</span>
                                                                                </EmptyDataTemplate>
                                                                                <Columns>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                                            <%--<asp:Label ID="lblOrderedCustomCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'></asp:Label>--%>
                                                                                            <asp:Label ID="lblCustomerCartId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustomCartId") %>'></asp:Label>
                                                                                            <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                                                                            <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                                                                            <asp:Label ID="lblParentProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ParentProductID") %>'></asp:Label>
                                                                                           
                                                                                            <input type="hidden" id="hdnSubTotalGrid" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"SubTotal") %>' />
                                                                                            <input type="hidden" id="hdnWeightTotal" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"Weight") %>' />
                                                                                            <input type="hidden" id="hdnWeightTotal1" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"Weight1") %>' />
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
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="50%" Font-Size="12px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                                                                            <asp:Label ID="lblProductType" runat="server" Visible="false" Text='<%# Eval("ProductType")%>'></asp:Label>
                                                                                              <asp:Label ID="lblisProductType" runat="server" Visible="false" Text='<%# Eval("isProductType")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp; Items
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle Font-Size="12px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                      <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                                            
                                                                                        </ItemTemplate>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;SKU
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle Font-Size="12px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>'></asp:Label>
                                                                                            <asp:Label ID="lblSalePrice" runat="server" Text='<%#Eval("SalePrice")%>' Visible="false"></asp:Label>
                                                                                            <asp:TextBox ID="txtChangePrice" runat="server" Width="60px" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                           <a href="javascript:void(0);" id="btnSavePrice"   runat="server"><img id="imgimage" src="/App_Themes/Gray/images/save.png" alt="Save" /></a>
                                                                                            <div style="display:none;">
                                                                                              <asp:Label ID="lblCustomerCartChangePriceId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustomCartId") %>'></asp:Label>
                                                                                                </div>
                                                                                        </ItemTemplate>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Price($)
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                                        <ItemStyle HorizontalAlign="right" Font-Size="12px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDiscountPrice" runat="server" Text='<%# Eval("DiscountPercent")%>'
                                                                                                Visible="false"></asp:Label>
                                                                                            $<asp:Label ID="lblOrginalDiscountPrice" runat="server"></asp:Label>
                                                                                            <asp:TextBox ID="txtcouponprice" runat="server" Width="60px" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderTemplate>
                                                                                            Discount Price
                                                                                            <asp:Label ID="lblHeaderDiscount" runat="server"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="right" />
                                                                                        <ItemStyle HorizontalAlign="right" Font-Size="12px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Quantity
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                                        <ItemStyle HorizontalAlign="Center" Font-Size="12px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblIndiSubTotal" runat="server" Text='<%# String.Format("{0:C}", Convert.ToDecimal(Eval("IndiSubTotal")))%>'></asp:Label>
                                                                                            <asp:Label ID="lblSubTot" runat="server" Text='<%# Eval("IndiSubTotal")%>' Visible="false"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderTemplate>
                                                                                            Sub Total
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                                        <ItemStyle HorizontalAlign="right" Font-Size="12px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp; Notes
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtNotes" runat="server" CssClass="order-textfield" Height="40px"
                                                                                                Width="240px" TextMode="MultiLine" Text='<%# Eval("Notes")%>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="15%" Font-Size="12px" />
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
                                                                <tr>
                                                                    <td style="width: 50%"></td>
                                                                    <td valign="top" align="right" style="width: 50%">
                                                                        <div id="trOrderDetails" runat="server" style="display: none; width: 100%">
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border: solid 1px #e7e7e7;">
                                                                                <tr class="altrow">
                                                                                    <td colspan="2" style="text-align: left; background-color: #e7e7e7;">
                                                                                        <b>Order Details </b>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td style="text-align: left;">Shipping Method :
                                                                                    </td>
                                                                                    <td style="text-align: left;">
                                                                                        <asp:DropDownList ID="ddlShippingMethod" Style="padding-left: 3px; width: 250px;"
                                                                                            onchange="chkHeight();" AutoPostBack="true" runat="server" CssClass="product-type"
                                                                                            OnSelectedIndexChanged="ddlShippingMethod_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td style="text-align: left;">Coupon Code :
                                                                                    </td>
                                                                                    <td style="text-align: left;">

                                                                                        <table style="text-align: left;">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtCouponCode" Width="80" CssClass="order-textfield" runat="server"></asp:TextBox></td>

                                                                                                <td>
                                                                                                    <asp:ImageButton ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click"
                                                                                                        OnClientClick="return CheckCouponCode();" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>

                                                                                    </td>
                                                                                </tr>

                                                                                <tr class="altrow">
                                                                                  <td colspan="2">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td style="text-align: left;width:17.7%">Store Credit No:
                                                                                                </td>
                                                                                                <td style="text-align: left;width:18%"> 
                                                                                                    <asp:TextBox ID="txtstorecreaditno" Width="80"  CssClass="order-textfield" runat="server"></asp:TextBox></td>

                                                                                                <td style="text-align: left;width:18%">Store Credit Amount:
                                                                                                </td>
                                                                                                <td style="text-align: left;width:13%">
                                                                                                    <asp:TextBox ID="txtstorecreaditAmount" Width="80"  onkeypress='return onKeyPressBlockNumbers(event);' CssClass="order-textfield" runat="server"></asp:TextBox></td>

                                                                                                <td style="text-align: left;">
                                                                                                    <asp:ImageButton ID="btnStorecreaditapply" runat="server"   OnClick="btnStorecreaditapply_Click"
                                                                                                        OnClientClick="return CheckStorecreaditCode();" />
                                                                                                </td>
                                                                                           </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td style="text-align: left;">Sub Total :
                                                                                    </td>
                                                                                    <td style="text-align: left;">$<asp:Label ID="lblSubTotal" Text="0.00" runat="server"></asp:Label>
                                                                                        <asp:HiddenField ID="hfSubTotal" runat="Server" Value="0" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow" id="tddiscount" runat="server" style="display:none;">
                                                                                    <td style="text-align: left;">Discount :
                                                                                    </td>
                                                                                    <td style="text-align: left;">
                                                                                        <asp:TextBox ID="TxtDiscount" onkeypress='return onKeyPressBlockNumbers(event);' onchange="onchangediscount();"
                                                                                            onblur="ShoppingCartTotal();" Text="0.00" Width="50" CssClass="order-textfield"
                                                                                            runat="server"></asp:TextBox>
                                                                                        <input type="hidden" id="hdnIsSalesManager" runat="server" value="0" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td style="text-align: left;">Shipping Cost :
                                                                                    </td>
                                                                                    <td style="text-align: left;">
                                                                                        <asp:TextBox ID="TxtShippingCost" onkeypress='return onKeyPressBlockNumbers(event);'
                                                                                            onblur="ShoppingCartTotal();" Text="0.00" Width="50" CssClass="order-textfield"
                                                                                            runat="server"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr class="oddrow">
                                                                                    <td style="width: 129px; text-align: left;">Tax :
                                                                                    </td>
                                                                                    <td style="text-align: left;">
                                                                                        <asp:TextBox ID="TxtTax" Width="50" onkeypress='return onKeyPressBlockNumbers(event);'
                                                                                            onblur="ShoppingCartTotal();" Text="0.00" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                
                                                                                <tr class="oddrow" style="display: none;">
                                                                                    <td style="text-align: left;">Discount Notes :
                                                                                    </td>
                                                                                    <td style="text-align: left;">
                                                                                        <asp:DropDownList ID="ddldiscountnotes" Style="padding-left: 3px; width: 250px;"
                                                                                            onchange="chkHeight();" AutoPostBack="true" runat="server" CssClass="product-type">
                                                                                        </asp:DropDownList>
                                                                                    </td>

                                                                                </tr>
                                                                                <tr class="altrow">
                                                                                    <td style="text-align: left;">Total :
                                                                                    </td>
                                                                                    <td style="text-align: left;">$<asp:Label ID="lblTotal" Text="0.00" runat="server"></asp:Label>
                                                                                        <asp:HiddenField ID="hfTotal" Value="0" runat="Server" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td align="center" style="height: 30px" colspan="2">
                                                                        <asp:ImageButton ID="btnSave" runat="server" OnClientClick="if(ValidationNotLogin()){chkHeight();return true;}else{return false;};"
                                                                            OnClick="btnSave_Click" />

                                                                          &nbsp;
                                                                        <asp:ImageButton ID="btnincompleteorder" runat="server" OnClientClick="if(ValidationImcompleteOrder()){chkHeight();return true;}else{return false;};"
                                                                            OnClick="btnincompleteorder_Click" />

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
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Customer"
        ShowSummary="false" />
    <div style="display: none;">
        <input type="hidden" id="hdncountry" runat="server" value="" />
        <input type="hidden" id="hdnState" runat="server" value="" />
        <input type="hidden" id="hdnZipCode" runat="server" value="" />
        <input type="hidden" id="hdnTaxRate" runat="server" value="0" />
       <input type="hidden" id="hdnischargelogic" runat="server" value="0" />
          <asp:TextBox ID="txtCOnfirmationID" runat="server"></asp:TextBox>
        <input type="hidden" id="hdnOrdernumber" runat="server" value="0" />
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
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
        <asp:HiddenField ID="hdnisregestered" runat="server" Value="false" />
        <asp:HiddenField ID="hdnsearchlinksku" runat="server" />
        <asp:Button ID="btndiscount" runat="server" OnClick="btndiscount_Click" />
    </div>
    
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
</asp:Content>
