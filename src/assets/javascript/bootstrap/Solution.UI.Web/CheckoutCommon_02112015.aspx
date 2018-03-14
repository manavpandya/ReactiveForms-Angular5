<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CheckoutCommon.aspx.cs" Inherits="Solution.UI.Web.CheckoutCommon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jquery-1.3.2.js" type="text/javascript" language="javascript"> </script>
    <script src="js/CheckOutValidation.js?6456" type="text/javascript" language="javascript"></script>

    <script type="text/javascript">
        window.onload = function () { enableTooltips('mainTable') };
    </script>

    <script type="text/javascript">

        //         function iframerealod(id, randomnum) {
        //document.getElementById('ContentPlaceHolder1_hdnhostedpaymentid').value = id;
        //            document.getElementById('ContentPlaceHolder1_btnPlaceOrder').style.display = 'none'; var hh = $('#iframecreditcard').innerHeight(); $('#prepageiframe').height(hh); $('#prepageiframe').show(); $('#iframecreditcard').load(function () { $('#prepageiframe').height(hh); $('#prepageiframe').hide(); if(document.getElementById('ContentPlaceHolder1_hdnremidplace').value == '1'){document.getElementById('ContentPlaceHolder1_btnPlaceOrder').style.display = 'none';} else {document.getElementById('ContentPlaceHolder1_btnPlaceOrder').style.display = '';} }).each(function () { if (this.complete) $(this).load(); });
        //            document.getElementById('iframecreditcard').src = 'https://connect.chargelogic.com/?HostedPaymentID=' + id;
        //            document.getElementById('prepage').style.display = 'none';


        //        }
    </script>
    <script type="text/javascript" src="/pagejs/checkoutcommon.js?434768655"></script>
    <link type="text/css" href="/pagecss/checkoutcommon.css" rel="stylesheet" />

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
                    <div class="checkbox-main">
                        <div class="checkbox-massage">
                            <asp:Label ID="lblInverror" runat="server"></asp:Label><br />
                            <asp:Label ID="lblFreeShippningMsg" runat="server"
                                Text=""></asp:Label>

                        </div>
                        <div class="checkbox-banner" id="checkbox-banner-img">
                            <script type="text/javascript" data-pp-pubid="544a9d5b91" data-pp-placementtype="728x90">
                                (function (d, t) {
                                    "use strict";
                                    var s = d.getElementsByTagName(t)[0], n = d.createElement(t);
                                    n.src = "https://paypal.adtag.where.com/merchant.js";
                                    s.parentNode.insertBefore(n, s);
                                }(document, "script"));
                            </script>
                        </div>

                        <div class="checkout-table" id="checkouttablecart" runat="server">

                            <table width="100%" cellspacing="0" cellpadding="0" border="0" id="tblcartmain" runat="server">
                                <tbody>

                                    <tr>
                                        <td colspan="3">
                                            <input type="hidden" id="hdnSubtotal" runat="server" value="0" />
                                            <asp:Repeater ID="RptCartItems" runat="server" OnItemDataBound="RptCartItems_ItemDataBound"
                                                OnItemCommand="RptCartItems_ItemCommand">
                                                <HeaderTemplate>
                                                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-pro">
                                                        <tbody>
                                                            <tr>

                                                                <th id="thproduct" runat="server">Product Details
                                                                </th>
                                                                <th width="10%" id="thsku" runat="server" style="display: none;">SKU
                                                                </th>
                                                                <th width="10%" class="td-pt1">Price
                                                                </th>
                                                                <th id="thdisprice" runat="server" visible="false" width="8%">Discount Price
                                                                </th>
                                                                <th width="9%" class="td-pt2" id="thqty" runat="server">Quantity
                                                                </th>
                                                                <th width="12%" class="td-pt3">Sub Total
                                                                </th>
                                                            </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr valign="top">

                                                        <td align="left" style="border-top: none; width: 54%;" id="tdproduct" runat="server">
                                                            <div class="table-pro-img">
                                                                <asp:Image ImageUrl='<%# GetIconImage(Convert.ToString(DataBinder.Eval(Container.DataItem, "ImageName")))%>'
                                                                    ID="imgName" runat="server" Width="150px" />
                                                                <asp:HiddenField ID="hdnVariantvalue" runat="server" Value='<%#Eval("VariantValues")%>' />
                                                                <asp:HiddenField ID="hdnVariantname" runat="server" Value='<%#Eval("VariantNames")%>' />
                                                                <asp:HiddenField ID="hdnCustomcartId" runat="server" Value='<%#Eval("CustomCartID")%>' />
                                                                <asp:HiddenField ID="hdnIndTotal" runat="server" Value='<%#Eval("IndiSubTotal")%>' />
                                                                <asp:HiddenField ID="hdnprice" runat="server" Value='<%#Eval("price")%>' />
                                                                <asp:HiddenField ID="hdnRelatedproductID" runat="server" Value='<%#Eval("RelatedproductID")%>' />
                                                                <asp:HiddenField ID="hdnswatchqty" runat="server" Value='0' />
                                                                <asp:HiddenField ID="hdnswatchtype" runat="server" Value='' />
                                                            </div>
                                                            <div class="table-pro-desc">
                                                                <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("ProductId")%>' />
                                                                <h2>
                                                                    <a id="lnkProductName" runat="server" visible="false" href="">
                                                                        <%#Eval("Name")%></a><asp:Label ID="lblFreeProductName" Visible="false" runat="server" Text='<%#Eval("Name")%>'></asp:Label></h2>
                                                                <span><strong>SKU :</strong></span><%#Eval("SKU")%><asp:Literal ID="ltrlVariane" runat="server"></asp:Literal><br />
                                                                <asp:LinkButton ID="lbtndelete" runat="server" ToolTip="Remove" Text="[Remove]" OnClientClick="javascript:if(confirm('Are you sure want to delete this item?')){Loader();return true;}else{return false;};"
                                                                    CommandName="del" CommandArgument='<%#Eval("CustomCartID")%>'></asp:LinkButton>
                                                            </div>
                                                        </td>
                                                        <td style="border-top: none; display: none;" id="tdSku" runat="server">
                                                            <%#Eval("SKU")%>
                                                        </td>
                                                        <td width="10%" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price")), 2)%>'></asp:Label>
                                                        </td>
                                                        <td id="tdDiscountprice" runat="server" width="8%" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                            visible="false">$<asp:Label ID="lblDiscountprice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Discountprice")), 2)%>'></asp:Label>
                                                        </td>
                                                        <td align="center" style="border-top: none; width: 9%;" id="tdqty" runat="server">
                                                            <asp:TextBox ID="txtQty" runat="server" MaxLength="4" Text='<%#Eval("Qty")%>' CssClass="wish-list-quantity"
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
                                                        <td colspan="3" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                            id="tdShipping" runat="server">Shipping :
                                                        </td>
                                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblShippingcost" runat="server" Text="0.00"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trCustomlevelDiscount" runat="server" visible="false" style="display: none;">
                                                        <td colspan="3" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                            id="tdCustomlevelDiscount" runat="server">Customer Level Discount :
                                                        </td>
                                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblCustomlevel" runat="server" Text="0.00"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trQuantitylDiscount" runat="server" visible="false" style="display: none;">
                                                        <td colspan="3" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                            id="tdQuantityDiscount" runat="server">Quantity Discount :
                                                        </td>
                                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblQuantityDiscount" runat="server" Text="0.00"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGiftCertiDiscount" runat="server" visible="false" style="display: none;">
                                                        <td colspan="3" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                            id="tdGiftCardAppliedDiscount" runat="server">Gift Card Applied Discount :
                                                        </td>
                                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblGiftCertiDiscount" runat="server" Text="0.00"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trGiftCardRemBal" runat="server" visible="false" style="display: none;">
                                                        <td colspan="3" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                            id="tdGiftCardRemainingBalance" runat="server">Gift Card Remaining Balance :
                                                        </td>
                                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblGiftCardRemBal" runat="server" Text="0.00"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <td colspan="3" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                            id="tdDiscount" runat="server">Discount :
                                                        </td>
                                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblDiscount" runat="server" Text="0.00"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <td></td>

                                                        <td colspan="2" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                            id="tdordertax" runat="server">Order Tax :
                                                        </td>
                                                        <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">$<asp:Label ID="lblordertax" runat="server" Text="0.00"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td align="left">
                                                            <div class="coupon-box">
                                                                <strong><span>Coupon Code</span></strong><asp:TextBox ID="txtPromoCode" runat="server" CssClass="code-input"></asp:TextBox>
                                                                <asp:ImageButton ID="btnApply1" runat="server" alt="APPLY" title="APPLY" ImageUrl="/images/apply.png"
                                                                    OnClientClick="return validation();" />

                                                            </div>
                                                        </td>
                                                        <td align="right" colspan="2" style="border-top: none; padding-left: 0px; padding-right: 10px;"
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
                                                    <td align="right">
                                                        <asp:ImageButton AlternateText="ADD ANOTHER ITEM" ID="btnAddAnotherItem" runat="server"
                                                            ToolTip="ADD ANOTHER ITEM" ImageUrl="/images/add-another-item-small.png" OnClick="btnAddAnotherItem_Click" />
                                                        <asp:ImageButton AlternateText="Save Your Cart" ID="btnClearCart" runat="server"
                                                            ToolTip="SAVE YOUR CART" ImageUrl="/images/save-your-cart.png" OnClientClick="javascript:document.getElementById('prepage').style.display = '';chkHeight(); "
                                                            OnClick="btnClearCart_Click" />
                                                        <asp:ImageButton AlternateText="Update Cart" ID="btnUpdateCart" runat="server" ToolTip="UPDATE CART"
                                                            ImageUrl="/images/update-cart.png" OnClick="btnUpdateCart_Click" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnupdatecoupon').value = '1'; document.getElementById('prepage').style.display = '';chkHeight();" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>



                                </tbody>
                            </table>




                        </div>


                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="new-login-main-pt" id="LoginTable" runat="server">
                <div class="new-login-main-pt-main" id="trCreatAcclogin" runat="server">
                    <div class="new-login-main-pt-main-left" id="divrdolist" runat="server">
                        <div class="new-login-main-pt-title-left">Terms and Conditions </div>
                        <div class="new-login-main-pt-title-left-text">
                            <div class="static_content checkout-scroll" id="termsdiv">
                                <asp:Literal ID="ltrreturnpolicy" runat="server"></asp:Literal>
                            </div>
                            <div class="checkout-checkbox" onclick="javascript:document.getElementById('ContentPlaceHolder1_chkaccept').click();">
                                <input id="chkaccept" type="checkbox" runat="server" onclick="javascript: document.getElementById('ContentPlaceHolder1_chkaccept').click();" />
                                <strong>I understand and agree.</strong>
                            </div>
                        </div>
                    </div>
                    <div class="new-login-main-pt-main-right">
                        <div class="new-login-main-pt-title-right">Checkout Options </div>
                        <div class="new-login-main-pt-title-right-text">
                            <div class="checkout-pt1">
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
                                                            <%=storePath.ToString()%></strong> account.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" height="10" align="left" style="border: medium none;">
                                                            <div class="login-row-1-passowrd">
                                                                <span>Email :</span>
                                                                <asp:TextBox ID="txtusername" runat="server" EnableViewState="false"
                                                                    CssClass="login-text"></asp:TextBox><br />
                                                                <br />
                                                            </div>
                                                            <div class="login-row-1-passowrd">
                                                                <span>Password:</span>
                                                                <asp:TextBox ID="txtpassword" runat="server" CssClass="login-text"
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
                                                    <td valign="top" height="10" colspan="2" align="left" style="border: medium none;">
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
                                                    <td align="right" style="padding-left: 6px;" colspan="2">
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
                                <asp:RadioButton ID="chkCreateNewAccount" GroupName="crtAcccount" runat="server" Font-Size="14px" Style="display: none;"
                                    onchange="ShowHideCreateAccDetails();" onclick="ShowHideCreateAccDetails();" />
                                <div class="checkout-pt-title">
                                    <span>I'm a new user</span>

                                    <asp:ImageButton ID="btnimgCreateNewAccount" runat="server" ImageUrl="/images/register-check-out.png" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_trReturningAccount').style.display='none';document.getElementById('termsdiv').className='static_content checkout-scroll';if(document.getElementById('ContentPlaceHolder1_chkaccept') != null && document.getElementById('ContentPlaceHolder1_chkaccept').checked == false) { alert('Please accept terms and conditions.'); return false;}else{return true;};" OnClick="btnimgCreateNewAccount_Click" />
                                    <p>Register to save time and track your orders.</p>
                                    <img id="" style="cursor: pointer; display: none;" src="/images/register-check-out.png" title="" onclick="javascript:document.getElementById('ContentPlaceHolder1_chkCreateNewAccount').click();" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <asp:UpdatePanel ID="Upadatelogin" runat="server">
                <ContentTemplate>


                    <div style="margin-top: 10px;">
                        <table width="100%" cellspacing="0" cellpadding="0" style="float: left;">
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
                                                                                        <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName")%>' />
                                                                                        <asp:Literal ID="lblTagImage" runat="server"></asp:Literal>

                                                                                        <input type="hidden" id="lblFreeEngraving" runat="server" visible="false" value='<%#Eval("IsFreeEngraving")%>' />
                                                                                        <asp:Label ID="lblTagImageName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "TagName")%>'></asp:Label>
                                                                                        <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>">
                                                                                            <img alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem, "Tooltip")))%>"
                                                                                                title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem, "Tooltip")))%>"
                                                                                                id='<%# "imgFeaturedProduct" + Convert.ToString(Container.ItemIndex + 1)%>' src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName")))%>'
                                                                                                width="230" height="309" /></a>
                                                                                    </div>
                                                                                </div>
                                                                                <h2 class="fp-box-h2" style="height: 42px;">
                                                                                    <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem, "Tooltip")))%>">
                                                                                        <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                                                                                <p class="fp-box-p">
                                                                                    <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                                                                                    <asp:Literal ID="ltrYourPrice" Visible="false" runat="server"></asp:Literal>
                                                                                    <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                                                                    <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                                                                    <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                                                                    <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>

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

                                    <table cellpadding="0" cellspacing="0" width="100%" class="table-pro">
                                        <tr>
                                            <th align="left" style="text-align: left; font-family: Arial,Helvetica,sans-serif; border-right: solid 1px #ddd !important;" colspan="2">Step 1 :Login Detail
                                                        <strong class="required-fields"><span class="required-red">*</span>Required Fields</strong>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="border-right: solid 1px #ddd !important;">
                                                <table cellspacing="0" cellpadding="0" border="0" class="table-none-border" style="width: 100% !important;">

                                                    <tr>
                                                        <td height="30px" align="left" style="padding-top: 5px; width: 17%; padding-bottom: 5px; font-size: 12px; font-weight: normal;" class="email-td"><span class="required-red">*</span>Email 
                                                        </td>

                                                        <td height="30" style="padding-top: 5px; width: 83%; padding-bottom: 5px; font-size: 12px; font-weight: normal; float: left;" class="email-td-2">

                                                            <asp:TextBox ID="txtCreateEmail" runat="server" CssClass="code-input"
                                                                Style="width: 200px; float: left; margin-left: 1px;" MaxLength="100"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td height="30px" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;" class="email-td-3"><span class="required-red">*</span>Create&nbsp;a&nbsp;password
                                                        </td>

                                                        <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;" class="email-td-4">



                                                            <asp:TextBox ID="txtCreateNewPassword" runat="server" TextMode="Password" CssClass="code-input"
                                                                Style="width: 200px; float: left;" MaxLength="100" OnTextChanged="txtCreateNewPassword_TextChanged"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="row1" id="trCreAccChangePass02" runat="server" style="display: none;">
                                                        <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;" class="email-td-3"><span class="required-red">*</span>Confirm&nbsp;password
                                                        </td>

                                                        <td height="30" align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal;" class="email-td-4">
                                                            <asp:TextBox ID="txtConfirmPassWord" runat="server" CssClass="code-input" TextMode="Password"
                                                                Style="width: 200px; float: left;" MaxLength="100" OnTextChanged="txtConfirmPassWord_TextChanged"></asp:TextBox>
                                                            <asp:ImageButton ID="btnNextCreateAcc" Style="float: right;" runat="server" AlternateText="Next" ImageUrl="/images/next-option.png" OnClientClick="return ValidatebtnNextCreateAcc();" />
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

                                    <table cellpadding="0" cellspacing="0" width="100%" class="table-pro">
                                        <tr>
                                            <th align="left" style="text-align: left;" colspan="3">Step 1 : Please Enter Contact Email
                                                        <strong class="required-fields"><span class="required-red">*</span>Required Fields</strong>
                                            </th>
                                        </tr>
                                        <tr>

                                            <td style="border-right: solid 1px #ddd !important;">
                                                <table cellspacing="0" cellpadding="0" border="0" class="table-none-border table-md" style="width: 100% !important;">

                                                    <tr>
                                                        <td align="left" style="width: 17%;" class="email-td-5"><span class="required-red">*</span>Email 
                                                        </td>

                                                        <td align="left" style="padding-top: 5px; padding-bottom: 5px; font-size: 12px; font-weight: normal; width: 83%;">



                                                            <asp:TextBox ID="txtGuestemail" runat="server" CssClass="code-input"
                                                                Style="width: 200px; float: left;" MaxLength="100"></asp:TextBox>
                                                            <asp:ImageButton ID="btnNextGuest" Style="float: right;" runat="server" ImageUrl="/images/next-option.png" OnClientClick="return ValidatebtnNextGuest();" AlternateText="Next" />
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
                                            <td>
                                                <div class="order-summary">
                                                    <asp:Literal ID="ltrShippingSummary" runat="server"></asp:Literal>
                                                </div>
                                                <div class="billing-address">
                                                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-pro"
                                                        id="tblBillAddEntry" runat="server">
                                                        <tr>
                                                            <th align="left" id="lblBilling" runat="server" colspan="2" style="text-align: left;"><span>Step 2: Billing Address</span>
                                                                <strong class="required-fields"><span class="required-red">*</span>Required Fields</strong>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-bottom: 10px !important; border-top: none;" colspan="3">
                                                                <table cellspacing="0" cellpadding="0" border="0" style="width: 100% !important;" class="table-none-border table-responsive">
                                                                    <tr style="height: 34px;">
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="required-red">*</span>First Name
                                                                        </td>

                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txtBillFirstname" runat="server" Style="width: 100%;" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillFirstname','ContentPlaceHolder1_txtShipFirstName');" CssClass="code-input"></asp:TextBox>
                                                                            <asp:CheckBox ID="UseShippingAddress" runat="server" onchange="javascript:SetBillingShippingVisible(true);"
                                                                                onclick="javascript:SetBillingShippingVisible(true);" AutoPostBack="false" Text="&nbsp;Ship to a different address"
                                                                                TextAlign="Right" Style="display: none;" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="required-red">*</span>Last Name
                                                                        </td>

                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txtBillLastname" Style="width: 100%;" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillLastname','ContentPlaceHolder1_txtShipLastName');" CssClass="code-input"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="required-red">*</span>Address&nbsp;Line&nbsp;1
                                                                        </td>

                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txtBillAddressLine1" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillAddressLine1','ContentPlaceHolder1_txtshipAddressLine1');" runat="server" CssClass="code-input"
                                                                                Style="width: 100%;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="required-red">&nbsp;</span>Address Line 2
                                                                        </td>

                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txtBillAddressLine2" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillAddressLine2','ContentPlaceHolder1_txtshipAddressLine2');" CssClass="code-input"
                                                                                Style="width: 100%;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                                                        </td>

                                                                        <td valign="top">

                                                                            <asp:TextBox ID="txtBillSuite" Style="width: 100%;" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillSuite','ContentPlaceHolder1_txtShipSuite');" CssClass="code-input"></asp:TextBox>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="middle">
                                                                            <span class="required-red">*</span>City
                                                                        </td>

                                                                        <td valign="top">

                                                                            <asp:TextBox ID="txtBillCity" Style="width: 100%;" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillCity','ContentPlaceHolder1_txtShipCity');" CssClass="code-input"></asp:TextBox>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="middle">
                                                                            <span class="required-red">*</span>Country
                                                                        </td>

                                                                        <td valign="middle">
                                                                            <asp:DropDownList ID="ddlBillcountry" runat="server" CssClass="select-box-pro" Style="width: 100%;"
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

                                                                        <td valign="middle" style="padding: 0px;">
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="padding: 0px;">
                                                                                        <asp:DropDownList ID="ddlBillstate" runat="server" CssClass="select-box-pro" Style="margin-left: -3px;"
                                                                                            onchange="MakeBillingOtherVisible();copyfrombill('ContentPlaceHolder1_ddlBillstate','ContentPlaceHolder1_ddlShipState');CopyBillOther();">

                                                                                            <asp:ListItem Text="Select State/Province " Value="Select State/Province ">Select State/Province </asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 100%;">
                                                                                        <div id="DIVBillingOther" style="display: none; padding-top: 7px">

                                                                                            <table style="width: 100%;">
                                                                                                <tr>
                                                                                                    <td style="width: 80%;">
                                                                                                        <span class="required-red">*</span>If Others, Specify&nbsp;</td>
                                                                                                    <td style="width: 50%;">
                                                                                                        <asp:TextBox ID="txtBillother"
                                                                                                            runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillother','ContentPlaceHolder1_txtShipOther');" CssClass="code-input"></asp:TextBox>
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
                                                                        <td valign="middle">
                                                                            <span class="required-red">*</span>Zip Code
                                                                        </td>

                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txtBillZipCode" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillZipCode','ContentPlaceHolder1_txtShipZipCode');" CssClass="code-input" onchange="javascript:document.getElementById('ContentPlaceHolder1_updateDiv').style.display = 'block';" OnTextChanged="txtBillZipCode_TextChanged" AutoPostBack="true"
                                                                                MaxLength="15" Style="width: 100%;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="middle">
                                                                            <span class="required-red">*</span>Phone
                                                                        </td>

                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txtBillphone" runat="server" onkeyup="copyfrombill('ContentPlaceHolder1_txtBillphone','ContentPlaceHolder1_txtShipPhone');" CssClass="code-input" MaxLength="20" Style="width: 90%;"></asp:TextBox>
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
                                                </div>
                                                <div class="shipping-address">

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td id="trbillrow" style="display: none;" runat="server" width="39%" align="right" valign="top">
                                                                <table cellspacing="0" cellpadding="0" border="0" class="table-pro" width="100%"
                                                                    id="tblShippAddressEntry" runat="server">


                                                                    <tr>
                                                                        <th align="left" style="text-align: left;" id="tdshipping" runat="server">
                                                                            <span>Step 3: Shipping Address</span> <strong class="required-fields"><span class="required-red">*</span>Required
                                                        Fields</strong>
                                                                        </th>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="border-bottom: none;">
                                                                            <span>
                                                                                <asp:CheckBox ID="chkcopy" runat="server" onchange="SameAsBilling();"
                                                                                    Text="&nbsp;Same as Billing Address"
                                                                                    TextAlign="Right" />
                                                                            </span>
                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td style="border-top: none;">
                                                                            <table cellpadding="0" border="0" width="100%" class="table-none-border table-responsive" id="pnlShippingDetails"
                                                                                runat="server">
                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="required-red">*</span>First Name
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:TextBox ID="txtShipFirstName" runat="server" Style="width: 100%;" CssClass="code-input"></asp:TextBox>

                                                                                    </td>
                                                                                </tr>


                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="required-red">*</span>Last Name
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:TextBox ID="txtShipLastName" runat="server" Style="width: 100%;" CssClass="code-input"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="required-red">*</span>Address&nbsp;Line&nbsp;1
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:TextBox ID="txtshipAddressLine1" runat="server" CssClass="code-input"
                                                                                            Style="width: 100%;"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="required-red">&nbsp;</span>Address Line 2
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:TextBox ID="txtshipAddressLine2" runat="server" CssClass="code-input"
                                                                                            Style="width: 100%;"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:TextBox ID="txtShipSuite" Style="width: 100%;" runat="server" CssClass="code-input"></asp:TextBox>

                                                                                    </td>
                                                                                </tr>

                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="required-red">*</span>City
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:TextBox ID="txtShipCity" Style="width: 100%;" runat="server" CssClass="code-input"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="required-red">*</span>Country
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlShipCounry" runat="server" CssClass="select-box-pro" Style="width: 100%;"
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

                                                                                    <td valign="middle" style="padding: 0px;">
                                                                                        <table style="margin-left: 2px;">
                                                                                            <tr>
                                                                                                <td style="padding: 0px;">
                                                                                                    <asp:DropDownList ID="ddlShipState" runat="server" CssClass="select-box-pro"
                                                                                                        Style="margin-left: -5px;" onchange="MakeShippingOtherVisible();ShowShipButton();">

                                                                                                        <asp:ListItem Text="Select State/Province " Value="Select State/Province ">Select State/Province </asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 100%;">
                                                                                                    <div id="DIVShippingOther" style="display: none; padding-top: 7px;">
                                                                                                        <table style="width: 100%">
                                                                                                            <tr>
                                                                                                                <td style="width: 80%;">
                                                                                                                    <span class="required-red">*</span>If Others, Specify&nbsp;</td>
                                                                                                                <td style="width: 50%;">
                                                                                                                    <asp:TextBox ID="txtShipOther" runat="server" CssClass="code-input"></asp:TextBox>
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
                                                                                    <td>
                                                                                        <span class="required-red">*</span>Zip Code
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:TextBox ID="txtShipZipCode" Style="width: 100%;" runat="server" CssClass="code-input"
                                                                                            MaxLength="15" onblur="ShowShipButton();" onchange="javascript:document.getElementById('ContentPlaceHolder1_updateDiv').style.display = 'block';" AutoPostBack="true" OnTextChanged="txtShipZipCode_TextChanged"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span class="required-red">*</span>Phone
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:TextBox ID="txtShipPhone" Style="width: 90%;" runat="server" CssClass="code-input" MaxLength="20"></asp:TextBox>
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
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ImageButton ID="btnNextAddress" Style="float: right;" runat="server" ImageUrl="/images/next-option.png" AlternateText="Next" OnClientClick="return ValidatebtnNextAddress();" />
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
                                            <td colspan="3">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trshipping" style="display: none;" runat="server">
                                            <td colspan="3">
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
                                                            <th align="left" style="text-align: left;" colspan="3" runat="server" id="lblshipping">Step 4: Shipping Method
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td width="60%" valign="top" align="left" colspan="2">



                                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                                                <asp:RadioButtonList ID="rdoShippingMethod" runat="server" AutoPostBack="true" onchange="Loader();"
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



                                                            </td>
                                                            <td valign="bottom" align="right">
                                                                <asp:ImageButton ID="btnNextShipping" runat="server" OnClientClick="return ValidatebtnNextShipping();" ImageUrl="/images/next-option.png" AlternateText="Next" /></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                       
                                        <tr id="trordernotes" runat="server" style="display: none;">
                                            <td colspan="3">&nbsp;
                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-pro">
                                                            <tbody>
                                                                <tr>
                                                                    <th style="text-align: left;" align="left">Order Notes (Optional)
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" colspan="4" style="width: 100%;">
                                                                        <asp:TextBox ID="txtOrderNotes" runat="server" TextMode="MultiLine" CssClass="order-text-box"
                                                                            cols="25" Rows="5" Width="98%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                      </td>
                                        </tr>
                                        <tr id="trPaymentMethods" runat="server" style="display: none;">
                                            <td colspan="3">
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-pro" style="margin-top:14px;">
                                                    <tbody>
                                                        <tr>
                                                            <th align="left" style="text-align: left;">
                                                                <span>
                                                                    <asp:Literal ID="ltshippingmethod" runat="server" Text="Step 5: Payment Method"></asp:Literal></span>
                                                                <strong class="required-fields"><span class="required-red">*</span>Required Fields</strong>
                                                                <asp:RadioButtonList ID="rdlPaymentType" Style="display: none;" runat="server" RepeatDirection="Horizontal" CssClass="btnredio" AutoPostBack="True" Width="215px" OnSelectedIndexChanged="rdlPaymentType_SelectedIndexChanged">
                                                                    <asp:ListItem Value="Creditcard">Creditcard</asp:ListItem>
                                                                    <asp:ListItem Value="PAYPALEXPRESS">Paypal</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <asp:Literal ID="ltrMethodName" runat="server" Visible="false"></asp:Literal>
                                                            </th>



                                                        </tr>
                                                        <tr>
                                                            <td valign="top" colspan="4" style="width: 100%;">
                                                                <asp:ImageButton ID="btncreditcard" runat="server" alt="CONTINUE CHECKOUT" title="CONTINUE CHECKOUT WITH CREDITCARD"
                                                                    ImageUrl="/images/place-order.png" OnClick="btncreditcard_Click" />
                                                                <asp:ImageButton ID="btnpaypal" runat="server" alt="CONTINUE CHECKOUT" title="CONTINUE CHECKOUT WITH PAYPAL"
                                                                    ImageUrl="/images/PaypalCheckout.gif" OnClick="btnpaypal_Click" />

                                                            </td>
                                                        </tr>

                                                        <tr style="display: none;">
                                                            <td align="left">
                                                                <table cellspacing="0" cellpadding="0" border="0" class="table-none-border payment-table" id="tblpayment" runat="server">


                                                                    <tr runat="server" id="trchargelogicdiv" style="display: none;">

                                                                        <td valign="top" colspan="4" style="width: 100%;">
                                                                            <div id="divcreditcardmsg" style="color: #ff0000"></div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trchargelogiciframe" style="display: none;">
                                                                        <td valign="top" colspan="4" style="width: 100%;"></td>
                                                                    </tr>
                                                                    <tr runat="server" id="trnameoncard">
                                                                        <td>
                                                                            <span class="required-red">*</span>Name on Card
                                                                        </td>

                                                                        <td>
                                                                            <asp:TextBox ID="txtNameOnCard" Style="width: 90%;" runat="server" CssClass="code-input"></asp:TextBox>
                                                                        </td>
                                                                        <td width="23%" rowspan="4" class="security-icon" align="center" style="padding: 0;" valign="top">
                                                                            <img src="/images/card-new.png" alt="" title="" />
                                                                            <table width="135" border="0" cellpadding="2" style="float: right;" cellspacing="0" title="Click to Verify - This site chose Symantec SSL for secure e-commerce and confidential communications.">
                                                                                <tr>
                                                                                    <td width="135" align="center" valign="top">
                                                                                        <script type="text/javascript" src="https://seal.verisign.com/getseal?host_name=www.halfpricedrapes.com&amp;size=L&amp;use_flash=YES&amp;use_transparent=YES&amp;lang=en"></script>
                                                                                        <br />
                                                                                        <a href="http://www.symantec.com/verisign/ssl-certificates" target="_blank" style="color: #000000; text-decoration: none; font: bold 7px verdana,sans-serif; letter-spacing: .5px; text-align: center; margin: 0px; padding: 0px;">ABOUT SSL CERTIFICATES</a></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trcardtype">
                                                                        <td>
                                                                            <span class="required-red">*</span>Card Type
                                                                        </td>

                                                                        <td colspan="2">
                                                                            <asp:DropDownList ID="ddlCardType" runat="server" CssClass="select-box-pro card-type" Style="width: 50%;">
                                                                                <asp:ListItem Text="Select Card Type" Value="Select Card Type">Select Card Type</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trcardnumber">
                                                                        <td>
                                                                            <span class="required-red">*</span>Card Number
                                                                        </td>

                                                                        <td colspan="2">
                                                                            <asp:TextBox ID="txtCardNumber" runat="server" MaxLength="16" CssClass="code-input card-number"
                                                                                onkeypress="return isNumberKeyCard(event)" Style="width: 40%; width: 50% !important;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trexpirationdate">
                                                                        <td>
                                                                            <span class="required-red">*</span>Expiration Date
                                                                        </td>

                                                                        <td colspan="2">
                                                                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="select-box-pro month-input" Style="width: 75px;">
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
                                                                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="select-box-pro year-input" Style="width: 70px;">
                                                                                <asp:ListItem Text="Year" Value="Year">Year</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trcvc">
                                                                        <td>
                                                                            <span class="required-red">*</span>Card&nbsp;security&nbsp;code&nbsp;(CSC)
                                                                        </td>

                                                                        <td colspan="2">
                                                                            <asp:TextBox ID="txtCSC" runat="server" CssClass="code-input" TextMode="Password" OnPreRender="txtCSC_PreRender" Style="width: 40px;"
                                                                                onkeypress="return isNumberKeyCard(event)" MaxLength="4"></asp:TextBox>
                                                                            <a href="javascript:void(0);" class="required-red display-none-2" onclick="javascript:document.getElementById('CVCImage').style.display=''; $('html, body').animate({ scrollTop: $('#footer-part').offset().top }, 'slow'); "
                                                                                title="What's this?">(What's this?)</a>
                                                                            <div id="CVCImage" style="position: absolute; display: none; z-index: 1; background-color: #fff; border: solid 1px #e0dfdf; padding-top: 5px; padding-bottom: 20px; padding-left: 20px; width: 495px; padding-right: 20px;">
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
                                            <td colspan="3">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" border="0" class="table-pro" width="100%">
                                                    <tbody>

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


                                        <tr>
                                            <td colspan="3">&nbsp;
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="3">
                                                <div class="checkout-box-buttons" id="divplaceorder" style="display: none;" runat="server">
                                                    <table style="float: left; width: 100%;">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:ImageButton ID="btnPlaceOrder" Style="display: none;" runat="server" alt="CONTINUE CHECKOUT" title="CONTINUE CHECKOUT"
                                                                    ImageUrl="/images/place-order.png" OnClick="btnPlaceOrder_Click" />
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

                                                </div>
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
                <input type="hidden" id="hdnNext" runat="server" value="0" />
                <asp:TextBox ID="txtCOnfirmationID" runat="server"></asp:TextBox>
                <asp:Button ID="btntempradio" runat="server" Text="t" OnClick="btntempradio_Click" />
                <input type="hidden" id="hdnischargelogic" runat="server" value="0" />
                <input type="hidden" id="hdnupdatecoupon" runat="server" value="0" />
                <input type="hidden" id="hdnhostedpaymentid" runat="server" value="" />
                <input type="hidden" id="hdnremidplace" runat="server" value="0" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%=strcretio%>
    <%=GTM %>
</asp:Content>
