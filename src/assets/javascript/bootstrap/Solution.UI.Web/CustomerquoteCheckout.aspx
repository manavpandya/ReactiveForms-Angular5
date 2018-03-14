<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerquoteCheckout.aspx.cs" Inherits="Solution.UI.Web.CustomerquoteCheckout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jquery-1.3.2.js" type="text/javascript" language="javascript"> </script>
    <script src="js/CheckOutValidation.js" type="text/javascript" language="javascript"></script>
    <script type="text/javascript">
        window.onload = function () { enableTooltips('mainTable') };
    </script>

    <script type="text/javascript">
        function CopyBillOther() {

            if (document.getElementById("ContentPlaceHolder1_chkcopy") != null && document.getElementById("ContentPlaceHolder1_chkcopy").checked == true) {
                if (document.getElementById("ContentPlaceHolder1_ddlBillstate") != null) {
                    var itext = document.getElementById("ContentPlaceHolder1_ddlBillstate").options[document.getElementById("ContentPlaceHolder1_ddlBillstate").selectedIndex].text;
                    if (itext.toLowerCase() == 'other') {
                        document.getElementById('DIVShippingOther').style.display = 'block';

                    }
                }
            }
        }
        function SameAsBilling() {
            if (document.getElementById('ContentPlaceHolder1_chkcopy').checked) {
                MakeSameother();
            }
            else {
                MakeBlankother();
            }
        }
        function copyfrombill(idfrom, idto) {
            if (document.getElementById('ContentPlaceHolder1_chkcopy') != null && document.getElementById('ContentPlaceHolder1_chkcopy').checked == true) { document.getElementById(idto).value = document.getElementById(idfrom).value; }
        }

        function MakeSameother() {



            if (document.getElementById('ContentPlaceHolder1_txtShipFirstName') != null)
                document.getElementById('ContentPlaceHolder1_txtShipFirstName').value = document.getElementById('ContentPlaceHolder1_txtBillFirstname').value;

            if (document.getElementById('ContentPlaceHolder1_txtShipLastName') != null)
                document.getElementById('ContentPlaceHolder1_txtShipLastName').value = document.getElementById('ContentPlaceHolder1_txtBillLastname').value;

            if (document.getElementById('ContentPlaceHolder1_txtshipAddressLine1') != null)
                document.getElementById('ContentPlaceHolder1_txtshipAddressLine1').value = document.getElementById('ContentPlaceHolder1_txtBillAddressLine1').value;

            if (document.getElementById('ContentPlaceHolder1_txtshipAddressLine2') != null)
                document.getElementById('ContentPlaceHolder1_txtshipAddressLine2').value = document.getElementById('ContentPlaceHolder1_txtBillAddressLine2').value;

            if (document.getElementById('ContentPlaceHolder1_txtShipSuite') != null)
                document.getElementById('ContentPlaceHolder1_txtShipSuite').value = document.getElementById('ContentPlaceHolder1_txtBillSuite').value;

            if (document.getElementById('ContentPlaceHolder1_txtShipCity') != null)
                document.getElementById('ContentPlaceHolder1_txtShipCity').value = document.getElementById('ContentPlaceHolder1_txtBillCity').value;


            if (document.getElementById('ContentPlaceHolder1_ddlShipState'))
                document.getElementById('ContentPlaceHolder1_tempbillid').value = document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex;


            if (document.getElementById('ContentPlaceHolder1_txtShipZipCode') != null)
                document.getElementById('ContentPlaceHolder1_txtShipZipCode').value = document.getElementById('ContentPlaceHolder1_txtBillZipCode').value;

            if (document.getElementById('ContentPlaceHolder1_txtShipPhone') != null)
                document.getElementById('ContentPlaceHolder1_txtShipPhone').value = document.getElementById('ContentPlaceHolder1_txtBillphone').value;

            if (document.getElementById('ContentPlaceHolder1_txtShipEmailAddress') != null)
                document.getElementById('ContentPlaceHolder1_txtShipEmailAddress').value = document.getElementById('ContentPlaceHolder1_txtBillEmail').value;

            try {
                if (document.getElementById('ContentPlaceHolder1_ddlShipState'))
                    document.getElementById('ContentPlaceHolder1_ddlShipState').selectedIndex = document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex;
                if ((document.getElementById('ContentPlaceHolder1_ddlBillstate').options[document.getElementById('ContentPlaceHolder1_ddlBillstate').selectedIndex]).text == 'Other') {
                    document.getElementById('DIVShippingOther').style.display = 'block';
                    document.getElementById('ContentPlaceHolder1_txtShipOther').value = document.getElementById('ContentPlaceHolder1_txtBillother').value;
                }
            }
            catch (err) {
            }

            if (document.getElementById('ContentPlaceHolder1_ddlShipCounry') != null) {
                document.getElementById('ContentPlaceHolder1_ddlShipCounry').selectedIndex = document.getElementById('ContentPlaceHolder1_ddlBillcountry').selectedIndex;
                __doPostBack('ctl00$ContentPlaceHolder1$ddlShipCounry', '');
            }



        }
        function MakeBlankother() {

            document.getElementById('ContentPlaceHolder1_txtShipFirstName').value = "";
            document.getElementById('ContentPlaceHolder1_txtShipLastName').value = "";
            document.getElementById('ContentPlaceHolder1_txtshipAddressLine1').value = "";
            document.getElementById('ContentPlaceHolder1_txtshipAddressLine2').value = "";
            document.getElementById('ContentPlaceHolder1_txtShipSuite').value = "";
            document.getElementById('ContentPlaceHolder1_txtShipCity').value = "";
            document.getElementById('ContentPlaceHolder1_ddlShipCounry').options[document.getElementById('ContentPlaceHolder1_ddlShipCounry').selectedIndex].text == 'United States';
            document.getElementById('ContentPlaceHolder1_ddlShipState').selectedIndex = 0;

            if (document.getElementById('DIVBillingOther'))
                document.getElementById('DIVBillingOther').style.display = "none";
            document.getElementById('ContentPlaceHolder1_txtShipOther').value = "";
            document.getElementById('DIVBillingOther').style.display = "none";
            document.getElementById('ContentPlaceHolder1_txtShipZipCode').value = "";
            document.getElementById('ContentPlaceHolder1_txtShipPhone').value = "";
            document.getElementById('ContentPlaceHolder1_txtShipEmailAddress').value = "";
            document.getElementById('ContentPlaceHolder1_tempbillid').value = "";
        }



    </script>

    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('Enter Valid Digit Only!');
                return false;
            }

            return true;
        }

        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtCardNumber').bind('copy paste', function (e) {
                e.preventDefault();
            });

        });

        <%-- function validation() {
            var a = document.getElementById('<%=txtPromoCode.ClientID %>').value.replace(/^\s+|\s+$/g, "");
            if (a == "") {
                alert('Enter Promo Code!');
                document.getElementById('<%=txtPromoCode.ClientID %>').focus();
                return false;
            }
            Loader();
            return true;
        }--%>
        function validation(textId) {
            var a = document.getElementById(textId).value.replace(/^\s+|\s+$/g, "");
            if (a == "") {
                alert('Enter Promo Code!');
                document.getElementById(textId).focus();
                return false;
            }
            Loader();
            document.getElementById('<%=btnApply.ClientID%>').click();
            return false;
        }
    </script>
    <script type="text/javascript">
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }

        function Loader() {

            chkHeight();
        }
        function LoaderShipping() {

            var windowHeight = 0;
            windowHeight = $('#tblshippheight').innerHeight(); //window.innerHeight;

            document.getElementById('prepage3').style.height = windowHeight + 'px';
            document.getElementById('prepage3').style.display = '';
            //chkHeight();
        }

        function ShippingMethodGet() {
            document.getElementById('ContentPlaceHolder1_updateDiv').style.display = 'block';
            return true;
        }



    </script>
    <script type="text/javascript">
        function checkfieldsforlogin() {
            if (document.getElementById('ContentPlaceHolder1_chkaccept') != null && document.getElementById('ContentPlaceHolder1_chkaccept').checked == false) {
                alert('Please accept terms and conditions.');
                document.getElementById('ContentPlaceHolder1_chkaccept').focus();
                return false;
            }
            else if (document.getElementById('<%=txtusername.ClientID %>').value.replace(/^\s+|\s+$/g, '') == '') {
                alert('Please enter email address.');
                document.getElementById('<%=txtusername.ClientID %>').value = '';
                document.getElementById('<%=txtusername.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtusername.ClientID %>').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('<%=txtusername.ClientID %>').value)) {
                alert('Please enter valid email address.');
                document.getElementById('<%=txtusername.ClientID %>').value = '';
                document.getElementById('<%=txtusername.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtpassword.ClientID %>').value.replace(/^\s+|\s+$/g, '') == '') {
                alert('Please enter password.');
                document.getElementById('<%=txtpassword.ClientID %>').value = '';
                document.getElementById('<%=txtpassword.ClientID %>').focus();
                return false;
            }

    return true;
}

function checkfieldsforForgotpwd() {

    if (document.getElementById('<%=txtEmail.ClientID %>').value == '') {
        alert('Please enter email address.');
        document.getElementById('<%=txtEmail.ClientID %>').focus();
        return false;
    }
    else if (document.getElementById('<%=txtEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail.ClientID %>').value)) {
        alert('Please enter valid email address.');
        document.getElementById('<%=txtEmail.ClientID %>').focus();
        return false;
    }
    return true;
}

function copyemail(fromemail, toemail, toemail1) {
    if (document.getElementById(fromemail) != null) {
        document.getElementById('ContentPlaceHolder1_txtusername').value = document.getElementById(fromemail).value;
        document.getElementById(toemail).value = document.getElementById(fromemail).value;
        document.getElementById(toemail1).value = document.getElementById(fromemail).value;
    }
}
    </script>
    <script type="text/javascript">
        var testresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                testresults = true
            else {
                testresults = false
            }
            return (testresults)
        }
    </script>
    <script type="text/javascript">
        //        var $j = jQuery.noConflict();
        //        $j(document).ready(function () {

        //            $j("#ContentPlaceHolder1_chkCreateNewAccount").click(function () {
        //                $j('#ContentPlaceHolder1_trCreAccChangePass01').slideToggle();
        //                $j('#ContentPlaceHolder1_trCreAccChangePass02').slideToggle();
        //                if (document.getElementById('ContentPlaceHolder1_chkCreateNewAccount').checked == true) {
        //                    $j('#ContentPlaceHolder1_trCreAccChangePass01').style.display = '';
        //                }
        //                else {
        //                    document.getElementById('ContentPlaceHolder1_trCreAccChangePass01').style.display = 'none';
        //                }
        //            });
        //        });
    </script>

    <style type="text/css">
        #ContentPlaceHolder1_rdoShippingMethod input {
            margin: 0 5px 0 0;
            float: left;
        }

        #ContentPlaceHolder1_rdlPaymentType input {
            margin: 2px 5px 0 0;
            float: left;
            font-weight: bold;
        }
    </style>
    <asp:ScriptManager ID="scrmanger" runat="server">
    </asp:ScriptManager>
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <span>Checkout</span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>Checkout</span>
        </div>
        <div class="static-content" id="mainTable">
             <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" id="tblcartmain" runat="server">
                <tbody>
                    <tr style="height: 5px; vertical-align: middle;">
                        <td colspan="3" align="center">
                            <asp:Label ID="lblInverror" runat="server" ForeColor="Red"></asp:Label><br />
                            <asp:Label ID="lblFreeShippningMsg" runat="server" Font-Bold="false" ForeColor="Red"
                                Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <script type="text/javascript" data-pp-pubid="544a9d5b91" data-pp-placementtype="728x90">
                                (function (d, t) {
                                    "use strict";
                                    var s = d.getElementsByTagName(t)[0], n = d.createElement(t);
                                    n.src = "https://paypal.adtag.where.com/merchant.js";
                                    s.parentNode.insertBefore(n, s);
                                }(document, "script"));
                     </script> 
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <input type="hidden" id="hdnSubtotal" runat="server" value="0" />
                            <asp:Repeater ID="RptCartItems" runat="server" OnItemDataBound="RptCartItems_ItemDataBound"
                                OnItemCommand="RptCartItems_ItemCommand">
                                <HeaderTemplate>
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table">
                                        <tbody>
                                            <tr>
                                                <th width="20%">Image
                                                </th>
                                                <th width="34%" id="thproduct" runat="server">Product
                                                </th>
                                                <th width="10%" id="thsku" runat="server">SKU
                                                </th>
                                                <th width="10%">Price
                                                </th>
                                                <th id="thdisprice" runat="server" visible="false" width="8%">Discount Price
                                                </th>
                                                <th width="9%" id="thqty" runat="server">Quantity
                                                </th>
                                                <th width="12%">Sub Total
                                                </th>
                                            </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr valign="top">
                                        <td style="border-top: none; width: 20%; text-align: center;">
                                            <asp:Image ImageUrl='<%# GetIconImage(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName"))) %>'
                                                ID="imgName" runat="server" Width="150px" />
                                            <asp:HiddenField ID="hdnVariantvalue" runat="server" Value='<%#Eval("VariantValues") %>' />
                                            <asp:HiddenField ID="hdnVariantname" runat="server" Value='<%#Eval("VariantNames") %>' />
                                            <asp:HiddenField ID="hdnCustomcartId" runat="server" Value='<%#Eval("CustomCartID") %>' />
                                            <asp:HiddenField ID="hdnIndTotal" runat="server" Value='<%#Eval("IndiSubTotal") %>' />
                                            <asp:HiddenField ID="hdnprice" runat="server" Value='<%#Eval("price") %>' />
                                            <asp:HiddenField ID="hdnRelatedproductID" runat="server" Value='<%#Eval("RelatedproductID") %>' />
                                            <asp:HiddenField ID="hdnswatchqty" runat="server" Value='0' />
                                            <asp:HiddenField ID="hdnswatchtype" runat="server" Value='' />
                                        </td>
                                        <td align="left" style="border-top: none; width: 34%;" id="tdproduct" runat="server">
                                            <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("ProductId") %>' />
                                            <%-- <a href="/<%#Eval("mainCategory")+"/"+ Eval("Sename")+"-"+ Eval("ProductId") +".aspx" %>">
                                            <%#Eval("Name")%></a>--%>
                                            <%-- <a id="lnkProductName" runat="server" href="">--%>
                                            <%#Eval("Name")%>
                                            <a id="lnkProductName" runat="server" visible="false" href="">
                                                <%#Eval("Name")%></a>
                                            <asp:Label ID="lblFreeProductName" Visible="false" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                            <br />
                                            <asp:Literal ID="ltrlVariane" runat="server"></asp:Literal>&nbsp;<br />
                                            <asp:LinkButton ID="lbtndelete" runat="server" ToolTip="Remove" Text="[Remove]" OnClientClick="javascript:if(confirm('Are you sure want to delete this item?')){Loader();return true;}else{return false;};"
                                                CommandName="del" Visible="false" CommandArgument='<%#Eval("CustomCartID") %>'></asp:LinkButton>
                                        </td>
                                        <td style="border-top: none; width: 15%;" id="tdSku" runat="server">
                                            <%#Eval("SKU")%>
                                        </td>
                                        <td width="10%" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price")), 2)%>'></asp:Label>
                                        </td>
                                        <td id="tdDiscountprice" runat="server" width="8%" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            visible="false">$<asp:Label ID="lblDiscountprice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Discountprice")), 2)%>'></asp:Label>
                                        </td>
                                        <td align="center" style="border-top: none; width: 9%;" id="tdqty" runat="server">
                                            <asp:TextBox ID="txtQty" runat="server" MaxLength="4" Text='<%#Eval("Qty") %>' CssClass="wish-list-quantity"
                                                Style="text-align: center; width: 40px; float: none;"></asp:TextBox>

                                        </td>
                                        <td align="right" style="border-top: none; width: 12%; padding-left: 0px; padding-right: 10px;">
                                            <asp:Literal ID="ltrSubTotal" runat="server"></asp:Literal>
                                            <asp:Label ID="lblNettotal" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "IndiSubTotal")), 2)%>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr style="display: none;">
                                        <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            id="tdShipping" runat="server">Shipping :
                                        </td>
                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblShippingcost" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trCustomlevelDiscount" runat="server" visible="false" style="display: none;">
                                        <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            id="tdCustomlevelDiscount" runat="server">Customer Level Discount :
                                        </td>
                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblCustomlevel" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trQuantitylDiscount" runat="server" visible="false" style="display: none;">
                                        <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            id="tdQuantityDiscount" runat="server">Quantity Discount :
                                        </td>
                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblQuantityDiscount" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trGiftCertiDiscount" runat="server" visible="false" style="display: none;">
                                        <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            id="tdGiftCardAppliedDiscount" runat="server">Gift Card Applied Discount :
                                        </td>
                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblGiftCertiDiscount" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trGiftCardRemBal" runat="server" visible="false" style="display: none;">
                                        <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            id="tdGiftCardRemainingBalance" runat="server">Gift Card Remaining Balance :
                                        </td>
                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblGiftCardRemBal" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            id="tdDiscount" runat="server">Discount :
                                        </td>
                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblDiscount" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="display:none;">
                                        <td></td>
                                        <td></td>
                                        <td colspan="3" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            id="tdordertax" runat="server">Order Tax :
                                        </td>
                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblordertax" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>

                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-border-none">
                                                <tbody>

                                                    <tr>
                                                        <td align="left" width="15%" style="font-weight:bold;font-size:14px;">&nbsp;Coupon&nbsp;Code&nbsp;:</td>
                                                        <td valign="top" width="10%">
                                                            <asp:TextBox ID="txtPromoCode" runat="server" CssClass="promo-code-texfild"></asp:TextBox>
                                                        </td>
                                                        <td width="7%">
                                                            <asp:ImageButton ID="btnApply1" runat="server" alt="APPLY" title="APPLY" ImageUrl="/images/apply.png"
                                                                OnClientClick="return validation();" />
                                                        </td>

                                                    </tr>
                                                </tbody>
                                            </table>

                                        </td>
                                        <td colspan="3" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                            id="tdTotal" runat="server">
                                            <strong>Sub Total : </strong>
                                        </td>
                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">
                                            <strong>$<asp:Label ID="lblSubtotal" runat="server"></asp:Label>
                                            </strong>
                                        </td>
                                    </tr>
                                    </tbody> </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr style="height: 3px;">
                        <td colspan="3"></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="contact-email">
                            <div>
                                <div class="checkout-right-box">
                                    <asp:Literal ID="ltrCart" runat="server"></asp:Literal>
                                    <asp:HiddenField ID="hdnCouponDiscount" runat="server" Value="0" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>

                                    <td align="left" id="trPromocode" runat="server" colspan="2" style="display: none;">


                                        <asp:ImageButton ID="btnApply" runat="server" alt="APPLY" title="APPLY" ImageUrl="/images/apply.png"
                                            OnClick="btnApply_Click" OnClientClick="return validation();" />

                                    </td>
                                    <td align="right" style="display:none;">
                                        <asp:ImageButton AlternateText="ADD ANOTHER ITEM" ID="btnAddAnotherItem" runat="server"
                                            ToolTip="ADD ANOTHER ITEM" ImageUrl="/images/add-another-item-small.png" OnClick="btnAddAnotherItem_Click" />
                                        <asp:ImageButton AlternateText="Save Your Cart" ID="btnClearCart" runat="server"
                                            ToolTip="SAVE YOUR CART" ImageUrl="/images/save-your-cart.png" OnClientClick="javascript:document.getElementById('prepage').style.display = '';chkHeight(); "
                                            OnClick="btnClearCart_Click" />
                                        <asp:ImageButton AlternateText="Update Cart" ID="btnUpdateCart" runat="server" ToolTip="UPDATE CART"
                                            ImageUrl="/images/update-cart.png" OnClick="btnUpdateCart_Click" OnClientClick="javascript:document.getElementById('prepage').style.display = '';chkHeight();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>


                  
                </tbody>
            </table>
                    </ContentTemplate>
                 </asp:UpdatePanel>
            <table id="LoginTable" runat="server" width="100%" cellspacing="0" cellpadding="0"
                border="0">
                <tbody>
                    <tr id="trCreatAcclogin" runat="server">
                        <td class="contact-email">
                            <div class="login-main-pt1" style="width: 100%;">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" class="table-none"
                                    style="border: 1px solid #D22D4F">


                                    <tr style="background: #F6F6F6;">
                                        <td height="30" align="left" class="" style="width: 70%; padding-top: 5px; padding-bottom: 5px; font-size: 13px; font-weight: bold; color: #D22D4F; background: #F6F6F6;">Terms and Conditions 
                                        </td>

                                        <td height="30" align="left" class="" style="padding-top: 5px; padding-bottom: 5px; font-size: 13px; font-weight: bold; color: #D22D4F; background: #F6F6F6;"
                                            colspan="2">Checkout Options
                                            <div style="display: none;">

                                                <img src="/images/help-icon.png" style="vertical-align: top; margin-top: 2px;" title="Special characters such as * &amp; ! are not allowed."
                                                    align="middle" />

                                            </div>
                                        </td>


                                    </tr>
                                    <tr id="divrdolist" runat="server">

                                        <td width="100%" height="30" align="left" colspan="2" style="border-right: 1px solid #D22D4F;">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="70%" valign="top" class="checkout-border">
                                                        <div class="static_content checkout-scroll" id="termsdiv">
                                                            <asp:Literal ID="ltrreturnpolicy" runat="server"></asp:Literal>

                                                        </div>
                                                        <div class="checkout-checkbox">
                                                            <input id="chkaccept" type="checkbox" runat="server" />
                                                            <strong>I understand and agree.</strong>
                                                        </div>
                                                    </td>
                                                    <td>&nbsp;&nbsp;</td>
                                                    <td style="width: 30%;" valign="top" align="left" colspan="3" class="checkout-border">
                                                        <div class="checkout-pt1">
                                                            <%--Text="&nbsp;Check here if you are a guest."--%>
                                                            <asp:RadioButton ID="chkCreateNewAccountGuest" GroupName="crtAcccount" Font-Size="14px" runat="server"
                                                                onchange="ShowHideGuestDetails();" onclick="ShowHideGuestDetails();" Style="display: none;" />
                                                            <div class="checkout-pt-title">
                                                                <span>Shop as guest</span>

                                                                <asp:ImageButton ID="btnimgCreateNewAccountGuest" runat="server" ImageUrl="/images/use-guest-checkout.png" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_trReturningAccount').style.display='none'; document.getElementById('termsdiv').className='static_content checkout-scroll'; if(document.getElementById('ContentPlaceHolder1_chkaccept') != null && document.getElementById('ContentPlaceHolder1_chkaccept').checked == false) { alert('Please accept terms and conditions.'); return false;}else{return true;};" OnClick="btnimgCreateNewAccountGuest_Click" />
                                                                <p>I do not wish to register or sign in.</p>
                                                                <img id="imgCreateNewAccountGuest" style="cursor: pointer; display: none;" src="/images/use-guest-checkout.png" title="" />

                                                            </div>
                                                        </div>

                                                        <div class="checkout-pt2">
                                                            <%--  Text="&nbsp;Check here if you are a returning account holder."--%>
                                                            <asp:RadioButton ID="chkReturningAcHolder" GroupName="crtAcccount" runat="server" Font-Size="14px"
                                                                onchange="ShowHideLoginDetails();" onclick="ShowHideLoginDetails();" Style="display: none;" />
                                                            <div class="checkout-pt-title">


                                                                <img id="imgReturningAcHolder" style="cursor: pointer;" src="/images/sign-in-check-out.png" title="" onclick="javascript:document.getElementById('termsdiv').className='static_content checkout-scroll scroll-auto';document.getElementById('ContentPlaceHolder1_chkReturningAcHolder').click();" />
                                                                <p>Save time checking out by signing in.</p>
                                                            </div>
                                                            <div id="trReturningAccount" class="checkout-pt-bg" runat="server" style="display: none;">
                                                                <div class="login-main-pt1" style="width: 100% !important;">
                                                                    <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin">
                                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" class="table-none">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <th align="left" style="text-align: left; font-family: Arial,Helvetica,sans-serif;">Returning Customers
                                                                                    </th>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" style="border: medium none;">Enter your E-Mail and Password below to sign into your <strong>
                                                                                        <%=storePath.ToString() %></strong> account.
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td valign="top" height="10" align="left" style="border: medium none;">
                                                                                        <div class="login-row-1-passowrd">
                                                                                            <span>Email :</span>
                                                                                            <asp:TextBox ID="txtusername" runat="server" Style="width: 200px;" EnableViewState="false"
                                                                                                CssClass="login-text"></asp:TextBox><br />
                                                                                            <br />
                                                                                        </div>
                                                                                        <div class="login-row-1-passowrd">
                                                                                            <span>Password:</span>
                                                                                            <asp:TextBox ID="txtpassword" runat="server" Width="175px" CssClass="login-text"
                                                                                                EnableViewState="false" TextMode="Password" OnTextChanged="txtPassword_TextChanged"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="login-row-1-buttons">
                                                                                            <asp:ImageButton ID="btnLogin" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit-order-place.png"
                                                                                                OnClick="btnLogin_Click" OnClientClick="return checkfieldsforlogin(); Loader();" />
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="padding-left: 6px;">
                                                                                        <asp:LinkButton ID="lkbForgetpwd" ToolTip="Forgot your password" runat="server" Text="Forgot your password?"
                                                                                            OnClick="lkbForgetpwd_Click">Forgot your password?</asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="pnlForgotPassword" runat="server" Visible="false" DefaultButton="btnSubmit">
                                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" class="table-none">
                                                                            <tr>
                                                                                <th colspan="2" style="font-family: Arial,Helvetica,sans-serif;">User Verification
                                                                                </th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" style="line-height: 20px; border: 0px;">You can recover your lost account information using the form below. Please enter
                                                your valid E-Mail Address (the one you used for registration),your account information
                                                will be mailed to you shortly.
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" height="10" align="left" style="border: medium none;">
                                                                                    <div class="login-row-1-email">
                                                                                        <span>E-Mail:</span>
                                                                                        <asp:TextBox ID="txtEmail" EnableViewState="false" runat="server" CssClass="login-field"></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="login-row-1-buttons" style="margin-left: 10px;">
                                                                                        <asp:ImageButton ID="btnSubmit" runat="server" alt="Submit" title="Submit" ImageUrl="~/images/submit.png"
                                                                                            OnClick="btnSubmit_Click" OnClientClick="return checkfieldsforForgotpwd();" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" style="padding-left: 6px;">
                                                                                    <asp:LinkButton ID="lnkReturnLogin" runat="server" OnClick="lnkReturnLogin_Click"
                                                                                        Text="Return to Login">Return to Login</asp:LinkButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="checkout-pt3">
                                                            <%--Text="&nbsp;Check here to create an account"--%>
                                                            <asp:RadioButton ID="chkCreateNewAccount" GroupName="crtAcccount" runat="server" Font-Size="14px" Style="display: none;"
                                                                onchange="ShowHideCreateAccDetails();" onclick="ShowHideCreateAccDetails();" />
                                                            <div class="checkout-pt-title">
                                                                <span>I'm a new user</span>

                                                                <asp:ImageButton ID="btnimgCreateNewAccount" runat="server" ImageUrl="/images/register-check-out.png" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_trReturningAccount').style.display='none';document.getElementById('termsdiv').className='static_content checkout-scroll';if(document.getElementById('ContentPlaceHolder1_chkaccept') != null && document.getElementById('ContentPlaceHolder1_chkaccept').checked == false) { alert('Please accept terms and conditions.'); return false;}else{return true;};" OnClick="btnimgCreateNewAccount_Click" />
                                                                <p>Register to save time and track your orders.</p>
                                                                <img id="" style="cursor: pointer; display: none;" src="/images/register-check-out.png" title="" onclick="javascript:document.getElementById('ContentPlaceHolder1_chkCreateNewAccount').click();" />

                                                            </div>

                                                        </div>

                                                    </td>

                                                </tr>

                                            </table>
                                        </td>

                                    </tr>
                                    <%--<tr id="trCreAccChangePass01" runat="server" style="display: none;">
                                        <td height="30px" align="right" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;">Create&nbsp;a&nbsp;password
                                        </td>
                                        <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;">
                                            <asp:TextBox ID="txtCreateNewPassword" runat="server" TextMode="Password" CssClass="login-text"
                                                Style="width: 200px; float: left;" MaxLength="100" OnTextChanged="txtCreateNewPassword_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="row1" id="trCreAccChangePass02" runat="server" style="display: none;">
                                        <td height="30" align="right" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;">Confirm&nbsp;password
                                        </td>
                                        <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;">
                                            <asp:TextBox ID="txtConfirmPassWord" runat="server" CssClass="login-text" TextMode="Password"
                                                Style="width: 200px; float: left;" MaxLength="100" OnTextChanged="txtConfirmPassWord_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                    <tr id="trReturningAcHolder" runat="server" style="display: none;">
                                        <td height="30" align="center" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: bold;">&nbsp;
                                        </td>
                                        <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-family: Arial,Helvetica,sans-serif; font-weight: bold;"></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:UpdatePanel ID="Upadatelogin" runat="server" >
                <ContentTemplate>
                    <div class="checkout-box">
                        <div class="checkout-content-left" style="width: 100%;">
                            <div class="checkout-content-box">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="">
                                    <tr id="trfeatureproduct" runat="server" style="display: none;">
                                        <td style="padding-bottom: 10px;">
                                            <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <th style="font-family: Arial,Helvetica,sans-serif;">Related Product(s)
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="fp-main">
                                                                <div class="fp-row1">
                                                                    <asp:Repeater ID="rptFeaturedProduct" runat="server" OnItemDataBound="rptFeaturedProduct_ItemDataBound">
                                                                        <HeaderTemplate>
                                                                            <ul id="mycarousel1111111" class="jcarousel-skin-tango">
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <li>
                                                                                <div class="fp-display" id="Probox" runat="server">
                                                                                    <div class="fp-box">
                                                                                        <div class="fp-box-div">
                                                                                            <div class="img-center">
                                                                                                <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                                                                                                <asp:Literal ID="lblTagImage" runat="server"></asp:Literal>

                                                                                                <input type="hidden" id="lblFreeEngraving" runat="server" visible="false" value='<%#Eval("IsFreeEngraving") %>' />
                                                                                                <asp:Label ID="lblTagImageName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                                                                                                <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>">
                                                                                                    <img alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                                                                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                                                                                        id='<%# "imgFeaturedProduct" + Convert.ToString(Container.ItemIndex +1) %>' src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                                                                        width="230" height="309" /></a>
                                                                                            </div>
                                                                                        </div>
                                                                                        <h2 class="fp-box-h2" style="height: 42px;">
                                                                                            <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                                                                <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                                                                                        <p class="fp-box-p">
                                                                                            <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                                                                                            <asp:Literal ID="ltrYourPrice" Visible="false" runat="server"></asp:Literal>
                                                                                            <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                                                                            <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                                                                            <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                                                                            <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                                                                                            <%--<asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("RegularPrice")), 2)%>'></asp:Label>--%>
                                                                                            <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                                                                                            <span><a visible="false" href="javascript:void(0);" id="aFeaturedLink" runat="server"
                                                                                                title="View More"></a></span>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </li>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </ul>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trCreAccChangePass01" runat="server" style="display: none;">
                                        <td style="padding: 0 0 10px 0;">

                                            <table cellpadding="0" cellspacing="0" width="100%" class="table-none">
                                                <tr>
                                                    <th align="left" style="text-align: left; font-family: Arial,Helvetica,sans-serif; border-right: solid 1px #ddd !important;" colspan="3">Login Detail
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td style="border-right: solid 1px #ddd !important;">
                                                        <table cellspacing="0" cellpadding="0" border="0" class="table-none-shipp" style="width: 50% !important;">

                                                            <tr>
                                                                <td height="30px" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal; width: 13%"><span class="required-red">*</span>Email 
                                                                </td>
                                                                <td style="width: 2%">:</td>
                                                                <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal; width: 44%;">



                                                                    <asp:TextBox ID="txtCreateEmail" runat="server" CssClass="login-text"
                                                                        Style="width: 200px; float: left;" MaxLength="100"></asp:TextBox>
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td height="30px" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;">Create&nbsp;a&nbsp;password
                                                                </td>
                                                                <td>:</td>
                                                                <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;">



                                                                    <asp:TextBox ID="txtCreateNewPassword" runat="server" TextMode="Password" CssClass="login-text"
                                                                        Style="width: 200px; float: left;" MaxLength="100" OnTextChanged="txtCreateNewPassword_TextChanged"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="row1" id="trCreAccChangePass02" runat="server" style="display: none;">
                                                                <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;">Confirm&nbsp;password
                                                                </td>
                                                                <td>:</td>
                                                                <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;">
                                                                    <asp:TextBox ID="txtConfirmPassWord" runat="server" CssClass="login-text" TextMode="Password"
                                                                        Style="width: 200px; float: left;" MaxLength="100" OnTextChanged="txtConfirmPassWord_TextChanged"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>

                                                </tr>

                                            </table>

                                        </td>

                                    </tr>
                                    <tr id="trguest" runat="server" style="display: none;">
                                        <td>

                                            <table cellpadding="0" cellspacing="0" width="100%" class="table-none">
                                                <tr>
                                                    <th align="left" style="text-align: left; font-family: Arial,Helvetica,sans-serif; border-right: solid 1px #ddd !important;" colspan="3">Contact Email
                                                    </th>
                                                </tr>
                                                <tr>

                                                    <td style="border-right: solid 1px #ddd !important;">
                                                        <table cellspacing="0" cellpadding="0" border="0" class="table-none-shipp" style="width: 50% !important;">

                                                            <tr>
                                                                <td height="30px" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal; width: 13%;"><span class="required-red">*</span>Email 
                                                                </td>
                                                                <td style="width: 2%">:</td>
                                                                <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal; width: 44%;">



                                                                    <asp:TextBox ID="txtGuestemail" runat="server" CssClass="login-text"
                                                                        Style="width: 200px; float: left;" MaxLength="100"></asp:TextBox>
                                                                </td>


                                                            </tr>
                                                        </table>

                                                    </td>



                                                </tr>

                                            </table>
                                            <br />
                                            <br />
                                        </td>

                                    </tr>
                                    <tr id="trcardnumdetail" style="display: none;" runat="server">
                                        <td>
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td width="40%" align="right" valign="top">
                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none"
                                                            id="tblBillAddEntry" runat="server">
                                                            <tr>
                                                                <th colspan="3" style="font-family: Arial,Helvetica,sans-serif;">Billing Address
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">&nbsp;
                                                                    <asp:CheckBox ID="UseShippingAddress" runat="server" onchange="javascript:SetBillingShippingVisible(true);"
                                                                        onclick="javascript:SetBillingShippingVisible(true);" AutoPostBack="false" Text="&nbsp;Ship to a different address"
                                                                        TextAlign="Right" Style="display: none;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="padding-bottom: 10px !important;;">
                                                                    
                                                                    <table cellspacing="0" cellpadding="0" border="0" class="table-none-shipp" style="width: 100% !important;">
                                                                        <tr>
                                                                            <td height="35" valign="middle" style="width: 13%;">
                                                                                <span class="required-red">*</span>First Name
                                                                            </td>
                                                                            <td valign="middle" align="center" style="width: 2%;">:
                                                                            </td>
                                                                            <td valign="middle" style="width: 44%;">
                                                                                <asp:TextBox ID="txtBillFirstname" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillFirstname','ContentPlaceHolder1_txtShipFirstName');" CssClass="checkout-text img-left"></asp:TextBox>
                                                                                <span class="img-right"><span class="required-red">*</span>Last Name &nbsp;&nbsp; :
                                                                &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtBillLastname" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillLastname','ContentPlaceHolder1_txtShipLastName');" CssClass="checkout-text"></asp:TextBox></span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="required-red">*</span>Address&nbsp;Line&nbsp;1
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtBillAddressLine1" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillAddressLine1','ContentPlaceHolder1_txtshipAddressLine1');" runat="server" CssClass="checkout-text-add"
                                                                                    Style="width: 97.5%;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="required-red">&nbsp;</span>Address Line 2
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtBillAddressLine2" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillAddressLine2','ContentPlaceHolder1_txtshipAddressLine2');" CssClass="checkout-text-add"
                                                                                    Style="width: 97.5%;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <span class="img-left">
                                                                                    <asp:TextBox ID="txtBillSuite" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillSuite','ContentPlaceHolder1_txtShipSuite');" CssClass="checkout-text"></asp:TextBox></span>
                                                                                <span class="img-right"><span class="required-red">*</span>City &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtBillCity" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillCity','ContentPlaceHolder1_txtShipCity');"  CssClass="checkout-text"></asp:TextBox></span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <span class="required-red">*</span>Country
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <asp:DropDownList ID="ddlBillcountry" runat="server" CssClass="select-box" Style="width: 185px;"
                                                                                    OnSelectedIndexChanged="ddlBillcountry_SelectedIndexChanged"
                                                                                    AutoPostBack="true">
                                                                                    <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <span class="required-red">*</span>State/Province
                                                                            </td>
                                                                            <td valign="top" align="right">:
                                                                            </td>
                                                                            <td valign="middle" style="padding: 0px;">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="padding: 0px;">
                                                                                            <asp:DropDownList ID="ddlBillstate" runat="server"  CssClass="select-box" Style="width: 185px; margin-left: 5px;"
                                                                                                onchange="MakeBillingOtherVisible();copyfrombill('ContentPlaceHolder1_ddlBillstate','ContentPlaceHolder1_ddlShipState');CopyBillOther();">

                                                                                                <asp:ListItem Text="Select State/Province " Value="Select State/Province ">Select State/Province </asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding: 0px;">
                                                                                            <div id="DIVBillingOther" style="display: none; padding-top: 7px">
                                                                                                <span class="required-red">*</span>If Others, Specify&nbsp;<asp:TextBox ID="txtBillother"
                                                                                                    runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillother','ContentPlaceHolder1_txtShipOther');" CssClass="checkout-text"></asp:TextBox>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <span class="required-red">*</span>Zip Code
                                                                            </td>
                                                                            <td>:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtBillZipCode" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillZipCode','ContentPlaceHolder1_txtShipZipCode');" CssClass="checkout-text" onchange="javascript:document.getElementById('ContentPlaceHolder1_updateDiv').style.display = 'block';" OnTextChanged="txtBillZipCode_TextChanged" AutoPostBack="true"
                                                                                    MaxLength="15"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <span class="required-red">*</span>Phone
                                                                            </td>
                                                                            <td>:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtBillphone" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillphone','ContentPlaceHolder1_txtShipPhone');" CssClass="checkout-text-phone" MaxLength="20"></asp:TextBox>
                                                                                <img src="/images/help.jpg" id="imgPhone" title="Your phone number is needed in case we need to contact you about your order like 123-456-7890."
                                                                                    class="img-left" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display: none;">
                                                                            <td valign="middle">
                                                                                <span class="required-red">*</span>E-Mail Address
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <asp:TextBox ID="txtBillEmail" runat="server" CssClass="checkout-text-phone"></asp:TextBox>
                                                                                <img src="/images/help.jpg" id="imgEmail" title="Your E-Mail address will never be sold or given to other companies."
                                                                                    class="img-left" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table cellspacing="0" cellpadding="0" border="0" width="99%" class="table-none"
                                                            id="tblShipAddress" runat="server" visible="false">
                                                            <tr>
                                                                <th colspan="3" style="font-family: Arial,Helvetica,sans-serif;">Shipping Address
                                                                </th>
                                                            </tr>

                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Literal ID="ltrShipAdd" runat="server"></asp:Literal>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td id="trbillrow" style="display: none;" runat="server" width="39%" align="right" valign="top">
                                                        <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%"
                                                            id="tblShippAddressEntry" runat="server">


                                                            <tr>
                                                                <th>
                                                                    <span class="img-left" style="font-family: Arial,Helvetica,sans-serif;">
                                                    Shipping Address</span> <strong class="required-fields"><span class="required-red">*</span>Required
                                                        Fields</strong>
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkcopy" runat="server" onchange="SameAsBilling();"
                                                                        Text="&nbsp;Same as Billing Address"
                                                                        TextAlign="Right" />

                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table cellpadding="0" border="0" width="100%" class="table-none-shipp" id="pnlShippingDetails"
                                                                        runat="server">
                                                                        <tr>
                                                                            <td valign="middle" style="width: 13%;">
                                                                                <span class="required-red">*</span>First Name
                                                                            </td>
                                                                            <td valign="middle" align="center" style="width: 2%;">:
                                                                            </td>
                                                                            <td valign="middle" style="width: 44%;">
                                                                                <asp:TextBox ID="txtShipFirstName" runat="server" CssClass="checkout-text img-left"></asp:TextBox>
                                                                                <span class="img-right"><span class="required-red">*</span>Last Name &nbsp;&nbsp; :
                                                                            &nbsp;&nbsp;
                                                                            <asp:TextBox ID="txtShipLastName" runat="server" CssClass="checkout-text"></asp:TextBox>
                                                                                </span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="required-red">*</span>Address&nbsp;Line&nbsp;1
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtshipAddressLine1" runat="server" CssClass="checkout-text-add"
                                                                                    Style="width: 97.5%;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="required-red">&nbsp;</span>Address Line 2
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtshipAddressLine2" runat="server" CssClass="checkout-text-add"
                                                                                    Style="width: 97.5%;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtShipSuite" runat="server" CssClass="checkout-text img-left"></asp:TextBox>
                                                                                <span class="img-right"><span class="required-red">*</span>City &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                            <asp:TextBox ID="txtShipCity" runat="server" CssClass="checkout-text"></asp:TextBox></span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <span class="required-red">*</span>Country
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <asp:DropDownList ID="ddlShipCounry" runat="server" CssClass="select-box" Style="width: 185px;"
                                                                                    OnSelectedIndexChanged="ddlShipCounry_SelectedIndexChanged"
                                                                                    AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                                <input type="hidden" runat="server" value="" id="tempbillid" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <span class="required-red">*</span>State/Province
                                                                            </td>
                                                                            <td valign="top" align="center">:
                                                                            </td>
                                                                            <td valign="middle" style="padding: 0px;">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="padding: 0px;">
                                                                                            <asp:DropDownList ID="ddlShipState" runat="server" CssClass="select-box"
                                                                                                Style="width: 185px; margin-left: 6px" onchange="MakeShippingOtherVisible();ShowShipButton();">

                                                                                                <asp:ListItem Text="Select State/Province " Value="Select State/Province ">Select State/Province </asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="padding: 0px;">
                                                                                            <div id="DIVShippingOther" style="display: none; padding-top: 7px;">
                                                                                                <span class="required-red">*</span>If Others, Specify&nbsp;
                                                                                        <asp:TextBox ID="txtShipOther" runat="server" CssClass="checkout-text"></asp:TextBox>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <span class="required-red">*</span>Zip Code
                                                                            </td>
                                                                            <td align="center">:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtShipZipCode" runat="server" CssClass="checkout-text"
                                                                                    MaxLength="15" onblur="ShowShipButton();" onchange="javascript:document.getElementById('ContentPlaceHolder1_updateDiv').style.display = 'block';" AutoPostBack="true" OnTextChanged="txtShipZipCode_TextChanged"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">
                                                                                <span class="required-red">*</span>Phone
                                                                            </td>
                                                                            <td align="center">:
                                                                            </td>
                                                                            <td valign="top">
                                                                                <asp:TextBox ID="txtShipPhone" runat="server" CssClass="checkout-text-phone" MaxLength="20"></asp:TextBox>
                                                                                <img src="/images/help.jpg" title="Your phone number is needed in case we need to contact you about your order like 123-456-7890."
                                                                                    class="img-left" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display: none;">
                                                                            <td valign="middle">
                                                                                <span class="required-red">*</span>E-Mail Address
                                                                            </td>
                                                                            <td valign="middle" align="center">:
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <asp:TextBox ID="txtShipEmailAddress" runat="server" CssClass="checkout-text-phone"></asp:TextBox>
                                                                                <img src="/images/help.jpg" title="Your E-Mail address will never be sold or given to other companies."
                                                                                    class="img-left" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table cellspacing="0" cellpadding="0" border="0" width="99%" class="table-none"
                                                            id="tblBillAddress" runat="server" visible="false">
                                                            <tr>
                                                                <th colspan="3" style="font-family: Arial,Helvetica,sans-serif;">
                                                                    <span class="img-left">Billing Address</span>
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle" colspan="4">
                                                                    <asp:Literal ID="ltrBillAdd" runat="server"></asp:Literal>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td width="20%" align="right" valign="top">
                                                        <asp:Literal ID="ltrShippingSummary" runat="server"></asp:Literal>

                                                    </td>

                                                </tr>

                                                <tr>
                                                    <td colspan="5">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <div id="prepage3" style="position: absolute; font-family: arial; font-size: 16; left: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 193px; width: 100%; z-index: 1000; display: none;">
                                                            <div style="border: 1px solid #ccc;">
                                                                <table width="100%" style="position: absolute; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                                                                    <tr>
                                                                        <td>
                                                                            <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc; width: 10%; height: 3%; padding: 5px; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">
                                                                                <center>
                                                                                    <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                                                                    <b style="color: #fff;">Loading ... ... Please wait!</b></center>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>

                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none" id="tblshippheight">
                                                            <tbody>
                                                                <tr>
                                                                    <th colspan="2" style="font-family: Arial,Helvetica,sans-serif;">Shipping Method
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td width="60%" valign="top" align="left">

                                                                       <%-- <asp:UpdatePanel runat="server">
                                                                            <ContentTemplate>--%>
                                                                                <%-- <script type="text/javascript" language="javascript">
                                                                                    Sys.Application.add_load(checkShippingdata);
                                                                                </script>--%>

                                                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                                                                <asp:RadioButtonList ID="rdoShippingMethod" runat="server" AutoPostBack="true"
                                                                                    CssClass="shippingradio" OnSelectedIndexChanged="rdoShippingMethod_SelectedIndexChanged"
                                                                                    Width="100%">
                                                                                </asp:RadioButtonList>
                                                                                <div style="float: left">
                                                                                    <asp:ImageButton ID="btnReloadShipping" runat="server" AlternateText="Get Additional Shipping Options" ImageUrl="/images/get-additional-shipping-options.png"
                                                                                        OnClientClick="return checkShippingdata();" OnClick="btnReloadShipping_Click" ToolTip="Get Additional Shipping Options" CausesValidation="False" />
                                                                                </div>
                                                                                <div style="width: 100%; text-align: center; height: 70px; display: none;" id="updateDiv"
                                                                                    runat="server">
                                                                                    <img id="loadImage" src="images/shippingloadre.gif" alt="Loading" /><br />
                                                                                    Loading shipping charges Please Wait..
                                                                                </div>


                                                                            <%--</ContentTemplate>
                                                                        </asp:UpdatePanel>--%>

                                                                    </td>
                                                                    <td valign="bottom" align="right"></td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr id="trPaymentMethods" runat="server">
                                                    <td colspan="5">
                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none">
                                                            <tbody>
                                                                <tr>
                                                                    <th colspan="5" align="left" style="font-family: Arial,Helvetica,sans-serif;">Payment Method :
                                                             <asp:RadioButtonList ID="rdlPaymentType" runat="server" RepeatDirection="Horizontal" onchange="Loader();" AutoPostBack="True" Width="200px" OnSelectedIndexChanged="rdlPaymentType_SelectedIndexChanged">
                                                                 <asp:ListItem Value="Creditcard" Selected="True">Creditcard</asp:ListItem>
                                                                 <asp:ListItem Value="PAYPALEXPRESS">Paypal</asp:ListItem>
                                                             </asp:RadioButtonList>
                                                                        <asp:Literal ID="ltrMethodName" runat="server" Visible="false"></asp:Literal>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="5" align="left">
                                                                        <table cellspacing="0" cellpadding="0" border="0" id="tblpayment" runat="server">
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <span class="required-red">*</span>Name on Card
                                                                                </td>
                                                                                <td>:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNameOnCard" runat="server" CssClass="checkout-card"></asp:TextBox>
                                                                                </td>
                                                                                <td width="23%" rowspan="2" align="center" style="padding: 0;">
                                                                                    <img src="/images/card-new.png" alt="" title="" />
                                                                                    <table width="135" border="0" cellpadding="2" style="float:right;" cellspacing="0" title="Click to Verify - This site chose Symantec SSL for secure e-commerce and confidential communications.">
                                                                                        <tr>
                                                                                            <td width="135" align="center" valign="top">
                                                                                                <script type="text/javascript" src="https://seal.verisign.com/getseal?host_name=www.halfpricedrapes.com&amp;size=L&amp;use_flash=YES&amp;use_transparent=YES&amp;lang=en"></script>
                                                                                                <br />
                                                                                                <a href="http://www.symantec.com/verisign/ssl-certificates" target="_blank" style="color: #000000; text-decoration: none; font: bold 7px verdana,sans-serif; letter-spacing: .5px; text-align: center; margin: 0px; padding: 0px;">ABOUT SSL CERTIFICATES</a></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <span class="required-red">*</span>Card Type
                                                                                </td>
                                                                                <td>:
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:DropDownList ID="ddlCardType" runat="server" CssClass="select-box" Style="width: 172px;">
                                                                                        <asp:ListItem Text="Select Card Type" Value="Select Card Type">Select Card Type</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="13%" valign="top">
                                                                                    <span class="required-red">*</span>Card Number
                                                                                </td>
                                                                                <td width="2%">:
                                                                                </td>
                                                                                <td width="44%" colspan="2">
                                                                                    <asp:TextBox ID="txtCardNumber" runat="server" MaxLength="16" CssClass="checkout-card"
                                                                                        onkeypress="return isNumberKeyCard(event)" Style="width: 43% !important;"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <span class="required-red">*</span>Expiration Date
                                                                                </td>
                                                                                <td>:
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="select-box" Style="width: 75px;">
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
                                                                                    <span style="float: left">&nbsp;</span>
                                                                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="select-box" Style="width: 70px;">
                                                                                        <asp:ListItem Text="Year" Value="Year">Year</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <span class="required-red">*</span>Card&nbsp;security&nbsp;code&nbsp;(CSC)
                                                                                </td>
                                                                                <td>:
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:TextBox ID="txtCSC" runat="server" CssClass="checkout-card" TextMode="Password" OnPreRender="txtCSC_PreRender" Style="width: 40px;"
                                                                                        onkeypress="return isNumberKeyCard(event)" MaxLength="4"></asp:TextBox>
                                                                                    <a href="javascript:void(0);" class="required-red" onclick="javascript:document.getElementById('CVCImage').style.display=''; $('html, body').animate({ scrollTop: $('#footer-part').offset().top }, 'slow'); "
                                                                                        title="What's this?">(What's this?)</a>
                                                                                    <div id="CVCImage" style="position: absolute; display: none; z-index: 1; background-color: #fff; border: solid 1px #e0dfdf; padding-top: 5px; padding-bottom: 20px; padding-left: 20px; width: 460px; padding-right: 20px;">
                                                                                        <div style="float: right;">
                                                                                            <a href="javascript:void(0);" onclick="javascript:document.getElementById('CVCImage').style.display='none';">Close</a>
                                                                                        </div>
                                                                                        <br />
                                                                                        <img style="z-index: 1;" src="/images/verificationnumber.gif" alt="" />
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
                                                <tr>
                                                    <td colspan="5">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                                                            <tbody>
                                                                <tr>
                                                                    <th style="font-family: Arial,Helvetica,sans-serif;">Order Notes (Optional)
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:TextBox ID="txtOrderNotes" runat="server" TextMode="MultiLine" CssClass="order-review-text-box"
                                                                            cols="25" Rows="5" Width="98%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td valign="top">

                                                                        <input id="chkreturnpolicy" type="checkbox" runat="server" checked="checked" />
                                                                        <a title="Terms and Conditions" href="/termandcondition.html" target="_blank">Please accept terms and condition.</a>

                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>


                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="checkout-box-buttons" id="divplaceorder" style="display: none;" runat="server">
                        <table style="float: left; width: 100%;">
                            <tr>
                                <td style="width: 720px">
                                    <asp:ImageButton ID="btnPlaceOrder" runat="server" alt="PLACE ORDER" title="PLACE ORDER"
                                        ImageUrl="/images/place-order.png" OnClick="btnPlaceOrder_Click" CssClass="img-right" />
                                </td>
                                <td style="width: 720px">
                                    <div class="img-right" style="float: right; margin-bottom: 5px;">
                                        <%--  <a href="contactus.aspx" title="Need Help With Your Order">
                                    <img src="/images/CheckoutIssue.png" alt="Need Help With Your Order" title="Need Help With Your Order" /></a>--%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                 
            </asp:UpdatePanel>
        </div>
    </div>
    <%--commented--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
                <div style="border: 1px solid #ccc;">
                    <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                        <tr>
                            <td>
                                <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc; width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">
                                    <center>
                                        <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                        <b style="color: #fff;">Loading ... ... Please wait!</b></center>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div style="display: none;">
                <input type="hidden" id="hdncountry" runat="server" value="" />
                <input type="hidden" id="hdnState" runat="server" value="" />
                <input type="hidden" id="hdnZipCode" runat="server" value="" />
                <input type="hidden" id="hdnSubTotalofProduct" runat="server" value="" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%=strcretio%>
</asp:Content>

