<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs" Inherits="Solution.UI.Web.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="js/jquery.min.js" async></script>
    <%--    <script type="text/javascript" src="js/jquery.jcarousel.pack.js"></script>--%>
    <script type="text/javascript" src="js/gallery.js" async></script>
    <script type="text/javascript" src="js/script.js" async></script>
    <%-- <script type="text/javascript" src="js/contentslider.js" > </script>
    <script type="text/javascript" src="js/jquery-1.2.3.pack.js" > </script>
    <script type="text/javascript" src="js/iepngfix_tilebg.js" ></script>--%>
    <%--<style type="text/css">
        img, div, input
        {
            behavior: url("js/iepngfix.htc");
        }
    </style>--%>
    <style type="text/css">
        .opacity span {
            color: #464646 !important;
            font-size: 20px !important;
        }

        .opacity strong {
            color: #848484 !important;
            font-size: 36px !important;
        }

        .opacity label {
            color: #464646 !important;
            font-size: 20px !important;
        }
    </style>
    <link href="/css/bannerslider.css?nav=1232" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function IndexValidation() {
            if (document.getElementById("ContentPlaceHolder1_txtFrom") != null && document.getElementById("ContentPlaceHolder1_txtFrom").value == "") {
                alert("Please enter from price.");
                document.getElementById("ContentPlaceHolder1_txtFrom").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtTo") != null && document.getElementById("ContentPlaceHolder1_txtTo").value == "") {
                alert("Please enter to price.");
                document.getElementById("ContentPlaceHolder1_txtTo").focus();
                return false;
            }
            else {
                var fromPrice = parseFloat(document.getElementById("ContentPlaceHolder1_txtFrom").value);
                var toPrice = parseFloat(document.getElementById("ContentPlaceHolder1_txtTo").value);
                if (parseFloat(fromPrice) > parseFloat(toPrice)) {
                    alert("Please enter valid price range.");
                    document.getElementById("ContentPlaceHolder1_txtFrom").focus();
                    return false;
                }
                else {
                    return true;
                }
            }
            return true;
        }




        function CheckSelection() {
            document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
        }

        function ColorSelection(clrvalue) {
            document.getElementById("ContentPlaceHolder1_hdnColorSelection").value = clrvalue;
             
            $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GetUrl",
                            data: "{Colorname: '" + clrvalue + "',Patternname: '' }",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {
                                window.location.href = msg.d;
                            },
                            Error: function (x, e) {
                            }
                        });

           
           // document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
        }
        function CheckpatternSelection(clrvalue) {
            document.getElementById("ContentPlaceHolder1_hdnpattern").value = clrvalue;
            $.ajax(
                       {
                           type: "POST",
                           url: "/TestMail.aspx/GetUrl",
                           data: "{Colorname: '',Patternname: '" + clrvalue + "' }",
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           async: "true",
                           cache: "false",
                           success: function (msg) {
                               window.location.href = msg.d;
                           },
                           Error: function (x, e) {
                           }
                       });
            //document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
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

        function unselectcheckbox(chkelement) {
            var allElts = document.getElementById('divPrice').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.name != chkelement) {
                        elt.checked = false;
                    }
                }
            }
            CheckSelection();
        }

        function unselectcheckboxforCustom(chkelement) {
            var allElts = document.getElementById('divCustom').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.name != chkelement) {
                        elt.checked = false;
                    }
                }
            }
            CheckSelection();
        }

        function ShowModelQuick(id) {

            document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmquickview").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmquickview').height = '425px';
            document.getElementById('frmquickview').width = '750px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:750px;height:425px;border:solid 1px #7d7d7d;");
            document.getElementById('popupContact').style.width = '750px';
            document.getElementById('popupContact').style.height = '425px';
            window.scrollTo(0, 0);
            document.getElementById('btnreadmore').click();
            document.getElementById('frmquickview').src = '/QuickView.aspx?PID=' + id;

        }
        function adtocart(price, id) {
            if (document.getElementById('prepage') != null) {
                document.getElementById('prepage').style.display = '';
            }
            document.getElementById("ContentPlaceHolder1_hdnPrice").value = price;
            document.getElementById("ContentPlaceHolder1_hdnproductId").value = id;
            document.getElementById("ContentPlaceHolder1_btnAddtocart").click();
        }
    </script>
    <div class="featured-product-bg" visible="false">
        <div class="fp-title">
             
                <asp:Literal ID="ltFeatureproductTitle" runat="server" Visible="false"></asp:Literal>
            
        </div>
        <div class="fp-main">
            <div class="fp-row1">
                <asp:Repeater ID="rptFeaturedProduct" runat="server" OnItemDataBound="rptFeaturedProduct_ItemDataBound">
                    <HeaderTemplate>
                        <ul id="mycarousel" class="jcarousel-skin-tango">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <div class="fp-display" id="Probox" runat="server">
                                <div class="fp-box">
                                    <div class="fp-box-div">
                                        <div class="img-center">
                                            <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                                            <asp:Literal ID="lblTagImage" runat="server"></asp:Literal>
                                            <asp:Literal ID="lblFreeEngravingImage" runat="server" Visible="false"></asp:Literal>
                                            <input type="hidden" id="lblFreeEngraving" runat="server" visible="false" value='<%#Eval("IsFreeEngraving") %>' />
                                            <asp:Label ID="lblTagImageName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                                            <%--<span></span>--%>
                                            <%--  <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%# Eval("ProductID")%>.aspx">--%>
                                            <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>">
                                                <img alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                                    title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                                    id='<%# "imgFeaturedProduct" + Convert.ToString(Container.ItemIndex +1) %>' src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                    width="230" height="309" /><img src="/images/view-detail.png"
                                                        alt="View Detail" class="preview" width="150" height="30" title="View Detail"></a>
                                        </div>
                                    </div>
                                    <h2 class="fp-box-h2" style="height: 42px;">
                                        <%--   <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%# Eval("ProductID")%>.aspx"
                                            title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                        <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                            <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                                    <p class="fp-box-p">
                                        <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                                        <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                        <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                        <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                        <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                                        <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("RegularPrice")), 2)%>'></asp:Label>
                                        <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                                        <span><a visible="false" href="javascript:void(0);" id="aFeaturedLink" runat="server"
                                            title="View More">
                                            <%--<img src="/images/view_more.jpg" alt="View More" height="18" width="80" />--%></a></span>
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
    </div>
    <div class="small-banner" id="hidesmallBannernew" runat="server">
    </div>
    <div class="index-option-main">
        <div class="shop-by-color">
            <div class="index-option-title">
                <span>Shop by Color</span>
            </div>
             <asp:Literal ID="ltrColoroptions" runat="server"></asp:Literal>
            <%--<ul>
                <li><a href="javascript:void(0);" onclick="ColorSelection('Purple & Mauve');">
                    <img title="" alt="" src="images/black-white.png"></a><span><a href="#">Black &amp; White</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/white-iwory.png"></a><span><a href="#">White &amp; Iwory</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/brown-tan.png"></a><span><a href="#">Brown &amp; Tan</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/gold-rust.png"></a><span><a href="#">Gold &amp; Rust</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/blue-teal.png"></a><span><a href="#">Blue &amp; Teal</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/green-olive.png"></a><span><a href="#">Green &amp; Olive</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/pink-fuchsia.png"></a><span><a href="#">Pink &amp; Fuchsia</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/red-burgundy.png"></a><span><a href="#">Red &amp; Burgundy</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/purple-mauve.png"></a><span><a href="#">Purple &amp; Mauve</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/multi.png"></a><span><a href="#">Multi</a></span></li>
            </ul>--%>
        </div>
        <div class="shop-by-design">
            <div class="index-option-title">
                <span>Shop by Design</span>
            </div>
              <asp:Literal ID="ltrPatternoption" runat="server"></asp:Literal>
            <%--<ul>
                <li><a href="javascript:void(0);" onclick="CheckpatternSelection('Solids');">
                    <img title="" alt="" src="images/solids-design.png"></a><span><a href="#">Solids</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/stripes-design.png"></a><span><a href="#">Stripes</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/plaids-design.png"></a><span><a href="#">Plaids</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/geometric-design.png"></a><span><a href="#">Geometric</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/damask-toile-design.png"></a><span><a href="#">Damask &amp; Toile</a></span></li>
                <li><a href="#">
                    <img title="" alt="" src="images/embroidered-floral-design.png"></a><span><a href="#">Embroidered &amp; Floral</a></span></li>
            </ul>--%>
        </div>
    </div>
    <div class="option-pro-main" id="hideIndexOptionDiv" runat="server" style="display:none;">
        <div class="colors-box">
            <div class="option-probox-title">
                <span>Colors</span>
            </div>
            <asp:Literal ID="ltrColor" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Header</span>
            </div>
            <asp:Literal ID="ltrHeader" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Pattern</span>
            </div>
            <asp:Literal ID="ltrPattern" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Fabric</span>
            </div>
            <asp:Literal ID="ltrFabric" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Style</span>
            </div>
            <asp:Literal ID="ltrStyle" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Custom</span>
            </div>
            <div class="toggle1" id="divCustom">
                <ul id="mycarousel6" class="jcarousel-skin-tango2">
                    <li>
                        <ul class="option-pro">
                            <li class="pattern-pro-box">
                                <input type="checkbox" class="checkbox" name="chkCustom_Yes" value="Yes" onchange="unselectcheckboxforCustom('chkCustom_Yes');"
                                    onclick="unselectcheckboxforCustom('chkCustom_Yes');" />
                                <span>Yes</span></li>
                            <li class="pattern-pro-box">
                                <input type="checkbox" class="checkbox" name="chkCustom_No" value="No" onchange="unselectcheckboxforCustom('chkCustom_No');"
                                    onclick="unselectcheckboxforCustom('chkCustom_No');" />
                                <span>No</span> </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div class="price-box-main">
            <div class="option-probox-title">
                <span>Price</span>
            </div>
            <div class="toggle1" id="divPrice" style="height: 238px;">
                <div class="price-box-bg">
                    <ul>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_0" value="< 10" onchange="unselectcheckbox('chkPrice_0');" />
                            <span>Less than $10</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_1" value=">= 10 ~ <= 20" onchange="unselectcheckbox('chkPrice_1');"
                                onclick="unselectcheckbox('chkPrice_1');" />
                            <span>$10 to $20</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_2" value=">= 20 ~ <= 40" onchange="unselectcheckbox('chkPrice_2');"
                                onclick="unselectcheckbox('chkPrice_2');" />
                            <span>$20 to $40</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_3" value=">= 40 ~ <= 60" onchange="unselectcheckbox('chkPrice_3');"
                                onclick="unselectcheckbox('chkPrice_3');" />
                            <span>$40 to $60</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_4" value=">= 60 ~ <= 80" onchange="unselectcheckbox('chkPrice_4');"
                                onclick="unselectcheckbox('chkPrice_4');" />
                            <span>$60 to $80</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_5" value=">= 80 ~ <= 100"
                                onchange="unselectcheckbox('chkPrice_5');" onclick="unselectcheckbox('chkPrice_5');" />
                            <span>$80 to $100</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_6" value=">= 100 ~ <= 200"
                                onchange="unselectcheckbox('chkPrice_6');" onclick="unselectcheckbox('chkPrice_6');" />
                            <span>$100 to $200</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_7" value="> 200" onchange="unselectcheckbox('chkPrice_7');"
                                onclick="unselectcheckbox('chkPrice_7');" />
                            <span>Greater then $200</span></li>
                    </ul>
                    <div class="qty-box">
                        <p>
                            $<span>
                                <asp:TextBox ID="txtFrom" runat="server" MaxLength="3" Width="28px" CssClass="qty-input"
                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                            </span>to $<span>
                                <asp:TextBox ID="txtTo" runat="server" MaxLength="3" Width="28px" CssClass="qty-input"
                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                            </span>
                        </p>
                        <asp:ImageButton ID="btnIndexPriceGo" runat="server" ImageUrl="/images/go-button.jpg"
                            OnClientClick="return IndexValidation();" ToolTip="Go" OnClick="btnIndexPriceGo_Click" Style="margin-top: 10px !important;" /><asp:ImageButton
                                ID="btnIndexPriceGo1" runat="server" Style="display: none;" ImageUrl="/images/go-button.jpg"
                                ToolTip="Go" OnClick="btnIndexPriceGo1_Click" />
                    </div>
                </div>
            </div>
        </div>
        <a href="#" id="toggle1" style="display: none;">
            <img src="/images/option-arrow-up.png" height="31" width="28" alt="arrow" id="option-arrow"></a>
    </div>
    <div class="index-content" id="hideIndexBlogDiv" runat="server" visible="false">
        <%--<div class="index-banner-main1">
            <div class="index-banner">
                <a href="/designnotesblog" title="HPD’s Blog - Perfect Curtains & Drapes">
                    <img src="/images/blog-banner.jpg" alt="HPD’s Blog - Perfect Curtains & Drapes" title="HPD’s Blog - Perfect Curtains & Drapes" class="img-left"></a>
            </div>
            <div class="index-banner-bg">
                <h3>
                    <asp:Literal ID="ltrHeadBlogContent" runat="server"></asp:Literal></h3>
                <asp:Literal ID="ltrSubBlogContent" runat="server"></asp:Literal>
                <a href="http://blog.halfpricedrapes.com/" target="_blank" title="Read More">Read More</a>
            </div>
        </div>
        <div class="index-banner-main2">
            <div class="index-banner">
                <a href="/showyoursupport" title="Your Support before Ordering Curtains">
                    <img src="/images/support-banner.jpg" alt="Your Support before Ordering Curtains" title="Your Support before Ordering Curtains" class="img-left"></a>
            </div>
            <div class="index-banner-bg">
                <h3>
                    <asp:Literal ID="ltrHeadSupportContent" runat="server"></asp:Literal></h3>
                <asp:Literal ID="ltrSubSupportContent" runat="server"></asp:Literal>
                <a href="/showyoursupport" title="Read More">Read More</a>
            </div>
        </div>
        <div class="index-banner-main3">
            <div class="index-banner">
                <a href="/maycatalog" title="Half Price Drapes Catalog for Home Decor">
                    <img src="/images/catalog-banner.jpg" alt="Half Price Drapes Catalog for Home Decor" title="Half Price Drapes Catalog for Home Decor" class="img-left"></a>
            </div>
            <div class="index-banner-bg">
                <h3>
                    <asp:Literal ID="ltrHeadCatalogContent" runat="server"></asp:Literal></h3>
                <asp:Literal ID="ltrSubCatalogContent" runat="server"></asp:Literal>
                <a href="/maycatalog" title="Read More">Read More</a>
            </div>
        </div>--%>
        <asp:Literal ID="ltrHeadCatalogContent" runat="server"></asp:Literal>
    </div>


    <%--<script type="text/javascript" src="js/script.js"></script>--%>
    <%--commented old code--%>
    <div class="banner-right" id="dvBannerright" runat="server" style="display: none;">
    </div>
    <div class="small-banner-row" id="dvSmallbannerrow" runat="server" style="display: none;">
    </div>
    <%--    <div id="content-right">--%>
    <div id="content-main">
        <div class="best-seller" style="display: none;">
            <%--<img src="images/best_seller_top.jpg" alt="" width="685" height="12" title="" class="img-left" />--%>
            <div class="best-pro-area">
                <div class="best-seller-title">
                    <p>
                        Our Best Selling Products
                    </p>
                </div>
                <div class="best-pro-display">
                    <asp:Repeater ID="rptBestSeller" runat="server" OnItemDataBound="rptBestSeller_ItemDataBound">
                        <ItemTemplate>
                            <div id="ProboxBestSeller" runat="server" class="best-pro-box">
                                <div id="proDisplay" runat="server">
                                    <div class="img-center">
                                        <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx">
                                            <img src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' width="127"
                                                height="118" alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                                title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>" /></a>
                                    </div>
                                    <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                                    <asp:Literal ID="lblTagImage" runat="server" Visible="false"></asp:Literal>
                                    <asp:Label ID="lblTagImageName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                                    <h2 style="padding-top: 2px; padding-bottom: 2px; height: 42px;">
                                        <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                            title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                            <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                                    <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                                    <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                    <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                                    <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                    <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                    <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("RegularPrice")), 2)%>'></asp:Label>
                                    <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                                    <p>
                                        <span><a href="javascript:void(0);" id="abestLink" runat="server" title="View More">
                                            <img src="/images/view_more.jpg" alt="View More" height="18" width="80" /></a></span>
                                    </p>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <%--<img src="images/best_seller_bottom.jpg" alt="" width="685" height="12" title=""
                class="img-left" />--%>
        </div>
        <div class="future-content1" style="display: none;">
             
                <asp:Literal ID="ltbettertitle" runat="server"></asp:Literal>
            
            <p>
                <asp:Literal ID="ltbetterdescription" runat="server"></asp:Literal>
            </p>
        </div>
        <div class="future-content1" style="display: none;">
            
                <asp:Literal ID="ltaboutustitle" runat="server"></asp:Literal>
            
            <p>
                <asp:Literal ID="ltaboutusdescription" runat="server"></asp:Literal>
            </p>
        </div>
        <div id="hideFeaturedCategory" runat="server" visible="false">
            <div class="title-boxindex">
                <h1>FEATURED CATEGORIES</h1>
            </div>
            <div class="cat-mainIndex">
                <asp:Repeater ID="rptFeaturedCategory" Visible="false" runat="server" OnItemDataBound="rptFeaturedCategory_ItemDataBound">
                    <HeaderTemplate>
                        <div class="cat-rowindex">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="cat-box" id="Catbox" runat="server">
                            <div class="cat-display" id="catDisplay" runat="server">
                                <a href="<%# SetCategoryPath(Convert.ToString(Eval("SEName")),Convert.ToString(Eval("ParentSEName"))) %>">
                                    <img id="imgFeaturedCategory" src='<%# GetIconImageCategory(Convert.ToString(Eval("ImageName"))) %>'
                                        width="180" height="149" alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>" />
                                </a>
                            </div>
                            <h2 style="padding-top: 3px; padding-bottom: 3px; height: 30px;">
                                <a href="<%# SetCategoryPath(Convert.ToString(Eval("SEName")),Convert.ToString(Eval("ParentSEName"))) %>"
                                    title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                    <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <%-- </div>--%>
    <%-- <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;
        border: solid 1px #7d7d7d;">
        <table border="0" cellspacing="0" cellpadding="0" class="table_border">
            <tr>
                <td align="left">
                    <iframe id="frmquickview" frameborder="0" height="425" width="750" scrolling="auto">
                    </iframe>
                </td>
            </tr>
        </table>
    </div>--%>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div style="display: none;">
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <asp:ImageButton ID="popupContactClose" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
    </div>
    <%-- <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 1440px;
        background: #fff;">
          <div style='float: right; background-color: transparent; right: -15px; top: -18px;
            position: absolute;'>
              <a href="javascript:void(0);" onclick="javascript:disablePopup();" title="">
                  <img src="/images/popupclose.png" alt="" />
              </a>
          </div>
          <div style="background: none repeat scroll 0 0 white;width:100%">
              <table border="0" cellspacing="0" cellpadding="0" width="100%">
                  <tr>
                      <td align="left">
                          <div style="float:left;width:100%;" id="divcolorhtml">
                              
                          </div>
                      </td>
                  </tr>
              </table>
          </div>
      </div>--%>
    <div style="display: none;">
        <input type="hidden" id="hdnPrice" runat="server" />
        <input type="hidden" id="hdnproductId" runat="server" />
        <input type="hidden" id="hdnColorSelection" runat="server" value="" />
        <input type="hidden" id="hdnpattern" runat="server" value="" />
        <asp:Button ID="btnAddtocart" OnClick="btnAddToCart_Click" runat="server" />
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16px; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" height="48" width="48" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" src="//static.criteo.net/js/ld/ld.js" async="true"></script>
    <script type="text/javascript"> window.criteo_q = window.criteo_q || []; window.criteo_q.push({ event: "setAccount", account: 6853 }, { event: "setSiteType", type: "d" }, { event: "viewHome" }); </script>


</asp:Content>
