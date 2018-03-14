<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddToCart.aspx.cs" Inherits="Solution.UI.Web.AddToCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <div class="breadcrumbs">
        <a href="/" title="Home">Home </a>> <span>Shopping Cart</span></div>
    <div class="content-main">
        <div class="static-title">
            <span>Shopping Cart</span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="margin-bottom: 10px;">
                    <tbody>
                        <tr style="height: 5px; vertical-align: middle;">
                            <td colspan="3" align="center">
                                <asp:Label ID="lblInverror" runat="server" ForeColor="Red"></asp:Label><br />
                                <asp:Label ID="lblFreeShippningMsg" runat="server" Font-Bold="false" ForeColor="Red"
                                    Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Repeater ID="RptCartItems" runat="server" OnItemDataBound="RptCartItems_ItemDataBound"
                                    OnItemCommand="RptCartItems_ItemCommand">
                                    <HeaderTemplate>
                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table">
                                            <tbody>
                                                <tr>
                                                    <th width="20%">
                                                        Image
                                                    </th>
                                                    <th width="34%" id="thproduct" runat="server">
                                                        Product
                                                    </th>
                                                    <th width="10%" id="thsku" runat="server">
                                                        SKU
                                                    </th>
                                                    <th width="10%">
                                                        Price
                                                    </th>
                                                    <th id="thdisprice" runat="server" visible="false" width="8%">
                                                        Discount Price
                                                    </th>
                                                    <th width="9%" id="thqty" runat="server">
                                                        Quantity
                                                    </th>
                                                    <th width="12%">
                                                        Sub Total
                                                    </th>
                                                </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr valign="top">
                                            <td style="border-top: none;">
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
                                            <td width="200px;" align="left" style="border-top: none;" id="tdproduct" runat="server">
                                                <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("ProductId") %>' />
                                                <%-- <a href="/<%#Eval("mainCategory")+"/"+ Eval("Sename")+"-"+ Eval("ProductId") +".aspx" %>">
                                            <%#Eval("Name")%></a>--%>
                                                <%-- <a id="lnkProductName" runat="server" href="">--%>
                                                <a id="lnkProductName" runat="server" visible="false" href="">
                                                    <%#Eval("Name")%></a>
                                                <asp:Label ID="lblFreeProductName" Visible="false" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                <br />
                                                <asp:Literal ID="ltrlVariane" runat="server"></asp:Literal>&nbsp;<asp:LinkButton ID="lbtndelete" runat="server" ToolTip="Remove" Text="[Remove]" OnClientClick="javascript:if(confirm('Are you sure want to delete this item?')){Loader();return true;}else{return false;};"
                                                    CommandName="del" CommandArgument='<%#Eval("CustomCartID") %>'></asp:LinkButton>
                                            </td>
                                            <td width="92px" style="border-top: none;" id="tdSku" runat="server">
                                                <%#Eval("SKU")%>
                                            </td>
                                            <td align="right" style="border-top: none;">
                                                $<asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price")), 2)%>'></asp:Label>
                                            </td>
                                            <td id="tdDiscountprice" runat="server" width="8%" align="right" style="border-top: none;
                                                padding-left: 0px; padding-right: 10px;" visible="false">
                                                $<asp:Label ID="lblDiscountprice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Discountprice")), 2)%>'></asp:Label>
                                            </td>
                                            <td align="center" style="border-top: none;" id="tdqty" runat="server">
                                                <asp:TextBox ID="txtQty" runat="server" MaxLength="4" Text='<%#Eval("Qty") %>' CssClass="wish-list-quantity"
                                                    Style="text-align: center; width: 40px;"></asp:TextBox>
                                                
                                            </td>
                                            <td align="right" style="border-top: none;">
                                                <asp:Literal ID="ltrSubTotal" runat="server"></asp:Literal>
                                                <asp:Label ID="lblNettotal" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "IndiSubTotal")), 2)%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tr style="display:none">
                                            <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                id="tdShipping" runat="server">
                                                Shipping :
                                            </td>
                                            <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">
                                                $<asp:Label ID="lblShippingcost" runat="server" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trCustomlevelDiscount" runat="server" visible="false" style="display:none">
                                            <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                id="tdCustomlevelDiscount" runat="server">
                                                Customer Level Discount :
                                            </td>
                                            <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">
                                                $<asp:Label ID="lblCustomlevel" runat="server" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trQuantitylDiscount" runat="server" visible="false" style="display:none">
                                            <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                id="tdQuantityDiscount" runat="server">
                                                Quantity Discount :
                                            </td>
                                            <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">
                                                $<asp:Label ID="lblQuantityDiscount" runat="server" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trGiftCertiDiscount" runat="server" visible="false" style="display:none">
                                            <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                id="tdGiftCardAppliedDiscount" runat="server">
                                                Gift Card Applied Discount :
                                            </td>
                                            <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">
                                                $<asp:Label ID="lblGiftCertiDiscount" runat="server" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trGiftCardRemBal" runat="server" visible="false" style="display:none">
                                            <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                id="tdGiftCardRemainingBalance" runat="server">
                                                Gift Card Remaining Balance :
                                            </td>
                                            <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">
                                                $<asp:Label ID="lblGiftCardRemBal" runat="server" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display:none">
                                            <td colspan="5" align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;"
                                                id="tdDiscount" runat="server">
                                                Discount :
                                            </td>
                                            <td align="right" style="border-top: none; padding-left: 0px; padding-right: 10px;">
                                                $<asp:Label ID="lblDiscount" runat="server" Text="0.00"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>

                                            </td>
                                            <td>
                                                 
                                    <table cellspacing="0" cellpadding="0" border="0"  width="100%" class="table-border-none">
                                        <tbody>
                                          
                                            <tr>
                                               <td align="left" width="15%">&nbsp;Coupon&nbsp;Code&nbsp;:</td>
                                                <td valign="top" width="10%">
                                                    <asp:TextBox ID="txtPromoCode" runat="server" CssClass="promo-code-texfild"></asp:TextBox>
                                                </td>
                                                <td width="7%" >
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
                            <td colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                         <td align="left" style="display:none;">
                                              <asp:ImageButton ID="btnApply" runat="server" alt="APPLY" title="APPLY" ImageUrl="/images/apply.png"
                                                        OnClick="btnApply_Click"   />

                                         </td>
                                        <td align="right">
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
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="trPromocode" runat="server">
                            <td colspan="3">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="trCalculateShippingHeader" runat="server">
                            <td width="48%" class="td-broder" valign="top" style="border-bottom: none;">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="border-none">
                                    <tr>
                                        <th align="left" style="font-family: Arial,Helvetica,sans-serif;">
                                            Calculate Shipping
                                        </th>
                                    </tr>
                                </table>
                            </td>
                            <td width="4%">
                                &nbsp;
                            </td>
                            <td width="48%" class="td-broder" valign="top" style="border-bottom: none;">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="border-none">
                                    <tr>
                                        <th align="left" style="font-family: Arial,Helvetica,sans-serif;">
                                            Proceed to Secure Checkout
                                        </th>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trShopping" runat="server">
                            <td width="48%" class="td-broder" valign="top">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSubmit">
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="border-none">
                                        <tbody>
                                            <tr>
                                                <td align="right" colspan="3">
                                                    <span class="required-red">*</span>Required Fields
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span class="required-red">*</span>Country
                                                </td>
                                                <td align="left" style="padding: 0pt;">
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlcountry" runat="server" CssClass="option1" Style="width: 212px;">
                                                        <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span class="required-red">*</span>Zip Code
                                                </td>
                                                <td align="left" style="padding: 0pt;">
                                                    :
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtZipCode" runat="server" CssClass="promo-code-texfild" MaxLength="15"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:ImageButton ID="btnSubmit" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit.png"
                                                        OnClientClick="return Shipvalidation();" OnClick="btnSubmit_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="left">
                                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                                    <asp:RadioButtonList ID="rdoShipping" runat="server" AutoPostBack="true" onchange="javascript:document.getElementById('prepage').style.display = '';chkHeight();"
                                                        OnSelectedIndexChanged="rdoShipping_SelectedIndexChanged" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td width="4%">
                                &nbsp;
                            </td>
                            <td width="48%" class="td-broder" valign="middle">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="border-none">
                                    <tbody>
                                        <tr>
                                            <td height="80" colspan="2" align="center" valign="middle" style="padding-top: 20px;
                                                padding-bottom: 20px">
                                                <asp:ImageButton ID="btnproceedtocheckout" runat="server" CssClass="checkout-buttons"
                                                    alt="PROCEED TO SECURE CHECKOUT" title="PROCEED TO SECURE CHECKOUT" ImageUrl="/images/proceed-to-secure-checkout.png"
                                                    OnClick="btnproceedtocheckout_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--  <td width="50%" height="54" align="center" valign="top">
                                                <span id="ss_img_wrapper_115-55_image_en"><a href="http://www.alphassl.com/ssl-certificates/wildcard-ssl.html"
                                                    target="_blank" title="SSL Certificates">
                                                    <img alt="Wildcard SSL Certificates" border="0" id="ss_img" src="//seal.alphassl.com/SiteSeal/images/alpha_noscript_115-55_en.gif"
                                                        title="SSL Certificate" /></a></span><script type="text/javascript" src="//seal.alphassl.com/SiteSeal/alpha_image_115-55_en.js"></script>
                                             
                                            </td>--%>
                                            <%-- <td width="50%" align="center" valign="top">--%>
                                            <td colspan="2" align="center" valign="middle" style="padding-bottom: 20px">
                                                <img title="" alt="" src="/images/card-new-addtocart.png" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <%--  <tr id="trAdditionalInfo" runat="server">
                            <td colspan="3">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none">
                                    <tbody>
                                        <tr>
                                            <th>
                                                Additional Info
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <ul>
                                                    <li><a href="/shippingpolicy" title="shipping information">shipping information</a></li>
                                                    <li><a href="#" title="billing information">billing information</a></li>
                                                    <li><a href="#" title="gift cards / services">gift cards / services</a></li>
                                                    <li><a href="#" title="sales tax">sales tax</a></li>
                                                    <li><a href="/returnpolicy" title="returns &amp; exchanges">returns &amp; exchanges</a></li>
                                                    <li><a href="/contactus.aspx" title="contact us">contact us</a></li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>--%>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <%--   <div class="best-sellers-title" style="font-size: 18px" >
                            Cross Sell Products</div>--%>
    <asp:Repeater ID="rptCrossSellProducts" runat="server" Visible="false">
        <ItemTemplate>
            <div id="ProboxBestSeller" runat="server" class="bs-box">
                <div id="proDisplay" runat="server">
                    <div onclick="bs-dispaly">
                        <div>
                            <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx">
                                <img src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' width="168"
                                    alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                    title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>" /></a></div>
                    </div>
                    <h2>
                        <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                            title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                            <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                    <p>
                        <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                            title="more details">more details</a></p>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="right-sub-section" style="min-height: 322px; display: none;">
        <h1>
            Best Sellers</h1>
        <div class="best-sellers-box">
            <div class="img-center">
                <asp:Literal ID="ltrImage" runat="server"></asp:Literal>
            </div>
            <asp:Literal ID="lblTagImage" runat="server"></asp:Literal>
            <h2>
                <asp:Literal ID="ltrname" runat="server"></asp:Literal>
            </h2>
            <p>
                <asp:Literal ID="ltrregular" runat="server" Visible="false"></asp:Literal>
                <asp:Literal ID="ltryourprice" runat="server"></asp:Literal>
            </p>
            <p>
                <asp:Literal ID="ltrAddtocart" runat="server"></asp:Literal>
            </p>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('Enter Valid Digit Only!');
                return false;
            }

            return true;
        } 
    </script>
    <script language="javascript" type="text/javascript">
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
        function Shipvalidation() {
            if (document.getElementById('<%=ddlcountry.ClientID %>') != null && document.getElementById('<%=ddlcountry.ClientID %>').selectedIndex == 0) {
                alert('Please Select Country.');
                document.getElementById('<%=ddlcountry.ClientID %>').focus();
                return false;

            }
            else if (document.getElementById('<%=txtZipCode.ClientID %>') != null && document.getElementById('<%=txtZipCode.ClientID %>').value.replace(/^\s+|\s+$/g, "") == '') {
                alert('Please Enter ZipCode.');
                document.getElementById('<%=txtZipCode.ClientID %>').focus();
                return false;
            }

            Loader();
            

            return true;
        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }

        function Loader() {

            chkHeight();
        }
        
    </script>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
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
    <div class="banners" style="margin-top: 5px; display: none;">
        <div class="banner-box">
            <img src="/images/your-order.png" alt="" title="" width="209" height="43" />
            <p>
                Ask Away! that's what
                <br />
                we're here for!</p>
            <a href="/contactus.aspx">
                <img src="/images/contact-form.png" width="105" height="30" alt="Contact Us" title="Contact Us">
            </a>
        </div>
        <div class="banner-box">
            <img src="/images/questions.png" alt="" title="" width="209" height="43" />
            <ul>
                <li><a href="/shippingpolicy" title="Shipping Information">Shipping Policy</a></li>
                <li><a href="/returnpolicy" title="Return Policy">Return Policy</li>
                <li><a href="/warranty" title="International Orders">Warranty</a></li>
            </ul>
        </div>
        <div class="banner-box">
            <img src="/images/guaranteed.png" alt="" title="" width="209" height="43" />
            <p>
                Authorized dealer for every brand we sell. Outstanding customer service. Dedicated
                knowledgeable staff. Worry free return policy.</p>
        </div>
        <div class="banner-box-last">
            <img src="/images/guarantee.png" alt="" title="" width="209" height="43" />
            <p>
                All information provided is neither shared nor sold to any third party.</p>
        </div>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hdnSubTotalofProduct" runat="server" value="" />
    </div>
</asp:Content>
