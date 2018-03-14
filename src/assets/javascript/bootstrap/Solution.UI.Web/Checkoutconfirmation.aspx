<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Checkoutconfirmation.aspx.cs" Inherits="Solution.UI.Web.Checkoutconfirmation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HalfPriceDrapes.com</title>
    <meta name="Description" content="" />
    <meta name="Keywords" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <link href='http://fonts.googleapis.com/css?family=Roboto+Condensed:400,300,700|Roboto:400,300,700' rel='stylesheet' type='text/css' />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/menucss-new.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery-1.3.2.js" type="text/javascript" language="javascript"> </script>
    <script src="https://connect.chargelogic.com/ChargeLogicConnectEmbed.js" type="text/javascript"></script>
    <script src="https://servername.chargelogic.com/ChargeLogicConnectEmbed.js" type="text/javascript"></script>
    <script type="text/javascript">


        function iframerealod(id, randomnum) {

            document.getElementById('btnPlaceOrder').style.display = 'none'; var hh = $('#iframecreditcard').innerHeight(); $('#prepageiframe').height(hh); $('#prepageiframe').show(); $('#iframecreditcard').load(function () { $('#prepageiframe').height(hh); $('#prepageiframe').hide(); document.getElementById('btnPlaceOrder').style.display = ''; }).each(function () { if (this.complete) $(this).load(); });
            document.getElementById('iframecreditcard').src = 'https://connect.chargelogic.com/?HostedPaymentID=' + id;



        }

    </script>
    <script type="text/javascript">
        function btnplaceorderconfiramtion() {
            document.getElementById('btnPlaceOrder').style.display = 'none';
            submitPayment('iframecreditcard', 'divcreditcardmsg');
            setTimeout(function () { document.getElementById('btnPlaceOrder').style.display = ''; }, 5000);
            return false;
        }
        function reloadframe()
        {
            document.getElementById('btnPlaceOrder').style.display = 'none';
            document.getElementById('imgtempbutton').click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <div id="doc-width">
                <div style="left: 1;" id="header-part">
                    <div class="header-row2">
                        <div class="header-top-link">
                            <div class="header-row2-right-pt0" id="hidePhone">
                                <p>
                                    Call Toll Free: <span style="font-size: 20px; font-weight: bold; color: #FFFFFF !important;">
                                        <span class="skype_c2c_print_container notranslate">1-866-413-7273</span><span data-ismobile="false" data-isrtl="false" data-isfreecall="true" data-numbertocall="+18664137273" onclick="SkypeClick2Call.MenuInjectionHandler.makeCall(this, event)" onmouseout="SkypeClick2Call.MenuInjectionHandler.hideMenu(this, event)" onmouseover="SkypeClick2Call.MenuInjectionHandler.showMenu(this, event)" tabindex="-1" dir="ltr" class="skype_c2c_container notranslate" id="skype_c2c_container"><span skypeaction="skype_dropdown" dir="ltr" class="skype_c2c_highlighting_inactive_common"><span id="free_num_ui" class="skype_c2c_textarea_span"><img width="0" height="0" src="resource://skype_ff_extension-at-jetpack/skype_ff_extension/data/call_skype_logo.png" class="skype_c2c_logo_img"><span class="skype_c2c_text_span">1-866-413-7273</span><span class="skype_c2c_free_text_span">&nbsp;FREE</span></span></span></span></span> <span class="display-none" style="color: #393939; text-transform: none; font-size: 12px; padding-left: 5px;"><strong>Hours:</strong> Mon-Fri 8 am - 5 pm PST, Sat-Sun Closed</span>
                                </p>
                            </div>


                        </div>

                        <div class="logo">
                            <a title="Half Price Drapes" href="/index.aspx">
                                <img title="Half Price Drapes" alt="Half Price Drapes" src="/images/logo.png"></a>
                        </div>
                        <div class="header-row2-right">
                        </div>

                    </div>

                    <div id="header-row3">
                        <div class="top-menu">

                            <div class="menu-icon" id="menu_icon">
                                <a href="#" class="toggleMenu" style="display: none;">Menu</a>
                            </div>
                            <ul style="z-index: 11;" id="menu_show" class="header-nav">
                                <li class="link-1" id="menu_link"><a href="/signature-silk-curtains.html" onmouseover="hideshowdiv(1,1);">Signature Silk Curtains</a> </li>
                                <li class="link-2" id="menu_link"><a href="/pattern-faux-silk-curtains.html" onmouseover="hideshowdiv(13,2);">Pattern Faux Silk Curtains</a> </li>
                                <li class="link-3" id="menu_link"><a href="/solid-faux-silk-curtains.html" onmouseover="hideshowdiv(20,3);">Solid Faux Silk Curtains</a> </li>
                                <li class="link-4" id="menu_link"><a href="/velvet-curtains.html" onmouseover="hideshowdiv(27,4);">Velvet Curtains</a> </li>
                                <li class="link-5" id="menu_link"><a href="/blackout-curtains.html" onmouseover="hideshowdiv(33,5);">Blackout Curtains</a> </li>
                                <li class="link-6" id="menu_link"><a href="/cotton-linen-curtains.html" onmouseover="hideshowdiv(44,6);">Cotton &amp; Linen Curtains</a> </li>
                                <li class="link-7" id="menu_link"><a href="/sheer-curtains.html" onmouseover="hideshowdiv(54,7);">Sheer Curtains</a> </li>
                                <li class="link-8" id="menu_link"><a href="/customitempage.aspx" onmouseover="hideshowdiv(64,8);">Custom Drapes</a></li>
                                <li class="link-9" id="menu_link"><a href="/shades.html" onmouseover="hideshowdiv(64,9);">Shades</a> </li>
                                <li class="link-10" id="menu_link"><a href="/hardware-accessories.html" onmouseover="hideshowdiv(66,10);">Hardware &amp; Accessories</a> </li>
                            </ul>
                            <a href="/salesoutlet.html" class="qmparent link-11">
                                <img border="0" style="float: right;" title="Sales Outlet" alt="Sales Outlet" src="/images/sales-outlet.png"></a>

                        </div>
                    </div>

                    <div style="display: none;" class="header-banner">

                        <div id="rotatebannertext" class="header-banner">
                        </div>
                        <div style="display: none;" id="rotatebannertextall">
                        </div>

                    </div>
                </div>
                <div id="content-width">
                    <div class="static-title">
                        <span>Review Order</span>
                    </div>
                    <div style="float: left; width: 100%; padding-top: 2%;">
                        <table cellpadding="0" cellspacing="0" width="70%">
                            <tr>
                                <td style="text-align: left; width: 30%;" valign="top">

                                    <table cellspacing="0" cellpadding="0" width="100%" class="table-pro">
                                        <tr>
                                            <th align="left" colspan="3" style="text-align: left;">Billing Information
                                                         
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="3" style="text-align: left;">
                                                <asp:Literal ID="txtbilladdress" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                                <td style="text-align: left; width: 30%; padding-left: 1%;" valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" class="table-pro">
                                        <tr>
                                            <th align="left" colspan="3" style="text-align: left;">Shipping Information
                                                         
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="3" style="text-align: left;">
                                                <asp:Literal ID="txtshipaddress" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                                <td style="text-align: left; width: 30%; padding-left: 1%;" valign="top">
                                    <div class="checkout-table" id="checkouttablecart" runat="server" style="float: right; width: 100%; margin-top: 0 !important;margin-left: 1%;"></div>
                                </td>
                            </tr>
                        </table>
                    </div>


                    <div style="float: left; width: 100%; padding-top: 2%;">
                        <table cellspacing="0" cellpadding="0" width="100%" class="table-pro">
                            <tr>
                                <th align="left" colspan="3" style="text-align: left;">Payment Information
                                          <span style="float:right;display:none;"><a href="javascript:void(0);" onclick="reloadframe();">Reload</a></span>               
                                </th>
                            </tr>
                            <tr>

                                <td align="left" colspan="3" style="text-align: left;">

                                    <div style="float: left; width: 100%;">
                                        <div id="divcreditcardmsg" style="color: #ff0000"></div>
                                        <div id="prepageiframe" style="position: absolute; font-family: arial; font-size: 16px; left: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; width: 100%; z-index: 1000;overflow:hidden;">
                                            <div style="border: 1px solid #ccc;">
                                                <table width="100%" style="position: absolute; top: 50%; left: 40%; margin: -20px 0 0 -100px;">
                                                    <tr>
                                                        <td style="background: none !important;border:none;">
                                                            <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc; width: 22%; height: 3%; padding: 5px; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">
                                                                <center>
                                                                                    <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                                                                    <b style="color: #fff;">Please Wait – Loading Secure Payment Interface.</b></center>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <iframe height="250" scrolling="no" id="iframecreditcard" style="border: none; width: 100%"></iframe>
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </div>

                     <div style="text-align: center;padding-top: 27px !important;"  class="static-title">
                         
                        <asp:ImageButton ID="btnPlaceOrder" runat="server" alt="PLACE ORDER"   title="PLACE ORDER" OnClientClick="return btnplaceorderconfiramtion();"
                            ImageUrl="/images/place-order-confiramtion.png" />

                    </div>
                   <div style="display:none;">
                         <asp:ImageButton ID="imgtempbutton" runat="server" OnClick="imgtempbutton_Click"/>

                   </div>
                </div>
                <div id="footer-part">
                    <div class="footer-row2" id="divhalfpricedrapes"></div>
                    <div class="footer-row1">
                        <div class="social-media">
                            <span>Join the Conversation</span>
                            <div class="social-media-box"><a href="https://www.facebook.com/halfpricedrapes" target="_blank" class="facebook" title="Facebook">Facebook</a> <a href="https://twitter.com/halfpricedrapes" target="_blank" class="twitter" title="Twitter">Twitter</a> <a href="http://pinterest.com/halfpricedrapes/" target="_blank" class="pinterest" title="Pinterest">Pinterest</a><a href="https://plus.google.com/109269923332391670065/?rel=author" target="_blank" class="google-plus" title="Google Plus">Google Plus</a> </div>
                        </div>
                    </div>
                    <div class="footer-row3">
                        <div class="footer-quick-links">
                            <span>Quick Links</span>
                            <ul>
                                <li><a href="/measuringguide.html" title="Measuring Guide">Measuring Guide</a></li>
                                <li><a title="Pleat Guide" href="/pleatguide.html">Pleat Guide</a></li>
                                <li><a title="Installation &amp; Care" href="/draperyinstallationcare.html">Installation &amp; Care</a></li>
                            </ul>
                        </div>
                        <div class="footer-customer-service">
                            <span>Customer Service</span>
                            <address>
                                <p>
                                    1.866.413.7273<br />
                                    Mon - Fri between 8 am - 5 pm PST<br />
                                    Saturday &amp; Sunday Closed<br />
                                    Showroom 9AM- 4PM, <strong>Appointment Preferred</strong>
                                </p>
                            </address>
                        </div>
                        <div class="footer-row3-pt1">
                            <div class="payment-option">
                                <span>We Accept</span>
                                <img src="/images/spacer.png" alt="Visa" title="Visa" class="visa">
                                <img src="/images/spacer.png" alt="Master Card" title="Master Card" class="master-card">
                                <img src="http://www.halfpricedrapes.com/images/spacer.png" class="american-express" alt="American Express" title="American Express">
                                <img src="/images/spacer.png" alt="Paypal" title="Paypal" class="paypal">
                                <img src="/images/spacer.png" alt="Discover" title="Discover" class="discover" />
                            </div>
                            <div class="footer-shipping-banner">
                                <a href="javascript:void();" onclick="window.open('http://www.customerlobby.com/reviews/10057/half-price-drapes/' ,'ReviewPage', 'statusbar=no,menubar=no,toolbar=no,scrollbars=yes,resizable=yes,width=540, height=575,left=570,top=200,screenX=570,screenY=200'); return false;" target="_blank" title="Customer Review – HPD’s">
                                    <img src="/images/shipping-banner.png" alt="Customer Review – HPD’s" title="Customer Review – HPD’s" class="img-left" /></a>
                            </div>
                        </div>
                        <div class="footer-row3-pt2">
                            <div class="footer-shipping">
                                <div class="footer-shipping-left">
                                    <span>Shipping To</span> <a title="Shipping To" href="/internationalshipping">
                                        <img class="shippingto-icon" title="Shipping To" alt="Shipping To" src="/images/spacer.png"></a>
                                </div>
                            </div>
                            <div class="footer-row3-banner">
                                <a href="/fabricromancer.html">
                                    <img src="/images/footer-banner1.jpg" alt="Learn About Curtains &amp; Window Treatments" title="Learn About Curtains &amp; Window Treatments" class="img-left" /></a> <a href="/" target="_blank">
                                        <img src="/images/footer-banner2.jpg" alt="HalfPriceFabrics.com" title="HalfPriceFabrics.com" class="img-left" style="margin: 0;"></a>
                            </div>
                        </div>
                    </div>
                    <div class="footer-row4">
                        <div class="footer-row4-links">
                            <ul>
                                <li><a href="/aboutus.html" title="About Us">About Us</a>|</li>
                                <li><a href="/termandcondition.html" title="Terms and Conditions" rel="nofollow">Terms and Conditions</a>|</li>
                                <li><a href="/showroom.html" title="Showroom">Showroom</a>|</li>
                                <li><a href="http://blog.halfpricedrapes.com/" target="_blank" title="HPD Blog">HPD
              
              Blog</a>|</li>
                                <li><a href="/press.html" title="Press" rel="nofollow">Press</a>|</li>
                                <li><a href="/faq.html" title="FAQ" rel="nofollow">FAQ</a>|</li>
                                <li><a href="/shippingpolicy.html" title="Shipping Policy" rel="nofollow">Shipping Policy</a>|</li>
                                <li><a href="/privacypolicy.html" title="Privacy Policy" rel="nofollow">Privacy Policy</a>|</li>
                                <li><a title="Affiliate Program" href="/affiliateprogram.html" rel="nofollow">Affiliate Program</a>|</li>
                                <li><a href="/ind.html" title="Sitemap">Sitemap</a></li>
                            </ul>
                        </div>
                        <p>&copy; 2015 halfpricedrapes, All rights reserved. Designed and Developed by Kaushalam </p>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
