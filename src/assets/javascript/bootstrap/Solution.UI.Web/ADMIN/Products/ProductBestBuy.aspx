<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductBestBuy.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductBestBuy" %>

<%@ Register Src="~/ADMIN/Controls/ProductHead.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script src="../JS/ProductBuyValidation.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function selecttab(obj) {
            if (obj.id == "mitem1") {
                document.getElementById("tab1").style.display = "block";
                document.getElementById("tab2").style.display = "none";
                document.getElementById("mitem1").src = "/App_Themes/<%=Page.Theme %>/images/required_hover.jpg";
                document.getElementById("mitem2").src = "/App_Themes/<%=Page.Theme %>/images/desired_regular.jpg";

            }
            else if (obj.id == "mitem2") {

                document.getElementById("tab2").style.display = "block";
                document.getElementById("tab1").style.display = "none";
                document.getElementById("mitem1").src = "/App_Themes/<%=Page.Theme %>/images/required_regular.jpg";
                document.getElementById("mitem2").src = "/App_Themes/<%=Page.Theme %>/images/desired_hover.jpg";
            }
        }

        function selecttabbydefault() {

            document.getElementById("tab1").style.display = "block";
            document.getElementById("tab2").style.display = "none";
            document.getElementById("mitem1").src = "/App_Themes/<%=Page.Theme %>/images/required_hover.jpg";
            document.getElementById("mitem2").src = "/App_Themes/<%=Page.Theme %>/images/desired_regular.jpg";
            document.getElementById("ctl00_ContentPlaceHolder1_ImageButton1").click();
            return false;
        }
    </script>
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
            <tr>
                <td class="border-td">
                    <table class="content-table" cellspacing="0" cellpadding="0" border="0" bgcolor="#FFFFFF"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="border-td-sub">
                                    <table class="add-product" cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tbody>
                                            <tr>
                                                <th>
                                                    <div class="main-title-left">
                                                        <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                        <h2 style="padding-top: 3px;">
                                                            <asp:Label ID="lblHeader" runat="server" Text="Add Product"></asp:Label></h2>
                                                    </div>
                                                    <div class="main-title-right">
                                                        <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                            <img id="imgMainDiv" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                    </div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="middle" colspan="3" width="100%">
                                                    <div class="tab_box2">
                                                        <uc1:Header runat="server" ID="head"></uc1:Header>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%" style="float: left; margin-top: 15px;
                                                        border: 1px solid #DFDFDF; border-bottom: 0px;">
                                                        <tr id="trmenu" runat="server" class="altrow">
                                                            <td>
                                                                <img style="cursor: pointer;" onclick="selecttab(this)" src="/App_Themes/<%=Page.Theme %>/images/required_hover.jpg"
                                                                    id="mitem1" name="mitem1" alt="Required Field" />
                                                                <img style="cursor: pointer;" onclick="selecttab(this)" src="/App_Themes/<%=Page.Theme %>/images/desired_regular.jpg"
                                                                    id="mitem2" name="mitem2" alt="Optional Field" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="center">
                                                    <div id="tab1">
                                                        <table cellpadding="0" cellspacing="0" height="400" width="100%">
                                                            <tr valign="top">
                                                                <td id="tdMainDiv">
                                                                    <div id="divMain" class="slidingDivMainDiv">
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                            <tr class="altrow">
                                                                                <td align="center">
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                                                    <span style="float: right"><span class="star" align="right">*</span>Required Fields</span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="even-row">
                                                                                <td colspan="2">
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">*</span>Seller ID :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtSellerID" Text="11111111" runat="server" class="order-textfield"
                                                                                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                <a style="color: rgb(51,51,51); text-decoration: underline;" href="javascript:void(0);"
                                                                                                    onclick="document.getElementById('ContentPlaceHolder1_txtSellerID').value='11111111';return false;">
                                                                                                    Reset Seller ID</a>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">*</span>Gtin :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtGtin" runat="server" AutoCompleteType="Disabled" MaxLength="100"
                                                                                                    class="order-textfield"></asp:TextBox>
                                                                                                <asp:LinkButton ID="lnkRandomNo" Style="color: rgb(51,51,51); text-decoration: underline;"
                                                                                                    runat="server" OnClick="lnkRandomNo_Click">Generate Random Gtin</asp:LinkButton>&nbsp;
                                                                                                <span style="color: rgb(51,51,51);">&nbsp;(i.e. UPC or EAN)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">&nbsp</span>Isbn :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtISBN" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">*</span> Manufacturer Name :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="ddlManufacture" runat="server" class="product-type">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">*</span>Manufacture Part No. :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtManufactrPartNo" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">&nbsp</span>Asin :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtAsin" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">*</span>SKU :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtSKU" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">*</span>Serial No :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtSerialNo" runat="server" class="order-textfield" MaxLength="10"
                                                                                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">*</span>Title :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtTitle" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">*</span>Weight :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtWeight" runat="server" class="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">*</span>Msrp :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtMsrp" runat="server" class="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">&nbsp</span>Listing Price:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtListingPrice" runat="server" class="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">&nbsp</span>Keywords :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtBuyKeywords" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">&nbsp</span>Product Set ID :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtProductSetID" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">&nbsp</span>Availability :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtAvailability" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">*</span>Inventory :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:TextBox ID="txtInventory" runat="server" class="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                <%--&nbsp;<img alt="Low Level" title="Low Level" src="../Images/bullet_red.png"
                                                                                                    id="ImgToggelInventory" runat="server" visible="false" style="vertical-align: middle;" />--%>
                                                                                                &nbsp;<span style="font-size: 28px; vertical-align: middle; color: Red; cursor: default;"
                                                                                                    title="Low Level" id="ImgToggelInventory" runat="server" visible="false">• <font
                                                                                                        style="font-size: 13px; vertical-align: middle; padding-bottom: 5px;">Low Level</font></span>&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">&nbsp</span>Published :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:CheckBox ID="chkPublished" runat="server" Checked="true" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow">
                                                                                            <td width="20%">
                                                                                                <span class="star">&nbsp</span>DisContinue :
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <asp:RadioButton ID="RBtnContiYes" runat="server" Text=" Yes" GroupName="DiscontinueSKU" />&nbsp;<asp:RadioButton
                                                                                                    ID="RBtnContNo" Checked="true" runat="server" Text=" No" GroupName="DiscontinueSKU" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                <span class="star">&nbsp</span>Display Order:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtDisplayOrder" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="oddrow" id="trDimension" runat="server">
                                                                                            <td align="left" height="30" style="width: 202px" valign="top">
                                                                                                &nbsp; Dimensions :
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <span style="padding-left: 1px;">Width</span>
                                                                                                <asp:TextBox ID="txtwidth" runat="server" Width="60px"></asp:TextBox>
                                                                                                <span>&nbsp;x&nbsp;Height</span>
                                                                                                <asp:TextBox ID="txtheight" runat="server" Width="60px"></asp:TextBox>
                                                                                                <span>&nbsp;x&nbsp;Length</span>
                                                                                                <asp:TextBox ID="txtlength" runat="server" Width="60px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trlistingId" runat="server" class="altrow">
                                                                                            <td align="left" style="width: 224px; height: 24px;" valign="top">
                                                                                                &nbsp; Listing Id :
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblListing" runat="server" Text="N/A"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 50%" valign="top">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <th>
                                                                                                <div class="main-title-left">
                                                                                                    <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                    <h2>
                                                                                                        Description</h2>
                                                                                                </div>
                                                                                                <div class="main-title-right">
                                                                                                    <a title="close" href="javascript:void(0);" class="show_hideSEO" onclick="return ShowHideButton('ImgSEO','tdSEO');">
                                                                                                        <img id="Img1" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a><a
                                                                                                            title="Minimize" href="#"></a>
                                                                                                </div>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td id="tdProductDescription">
                                                                                                <div id="tab-container" class="slidingDivSEO">
                                                                                                    <ul class="menu">
                                                                                                        <li class="active" id="ordernotes1" onclick='$("#ordernotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.order-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                            Detail Description</li>
                                                                                                        <li id="privatenotes1" onclick='$("#ordernotes1").removeClass("active");$("#privatenotes1").addClass("active"); $("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.private-notes").fadeIn();$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                            Warranty</li>
                                                                                                        <li id="giftnotes1" onclick='$("#giftnotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.gift-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                            Features</li>
                                                                                                    </ul>
                                                                                                    <span class="clear"></span>
                                                                                                    <div class="tab-content-2 order-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <div class="ckeditor-table" style="width: 100%">
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
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 private-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtWarranty" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <span class="clear"></span>
                                                                                                    <div class="tab-content-2 gift-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <div class="ckeditor-table" style="width: 100%">
                                                                                                                <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtFeatures" Rows="10"
                                                                                                                    Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                                                <script type="text/javascript">
                                                                                                                    CKEDITOR.replace('<%= txtFeatures.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
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
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="width: 50%" valign="top">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <th colspan="2">
                                                                                                <div class="main-title-left">
                                                                                                    <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
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
                                                                                                        <li class="active" id="liSEOPageTitle1" onclick='$("#liSEOPageTitle1").addClass("active");$("#liSEOKeywords1").removeClass("active");$("#liSEODesc1").removeClass("active");$("#liImageTooltip1").removeClass("active");$("div.liSEOPageTitle").fadeIn();$("div.liSEOKeywords").css("display", "none");$("div.liSEODesc").css("display", "none");$("div.liImageTooltip").css("display", "none");'>
                                                                                                            Page Title</li>
                                                                                                        <li id="liSEOKeywords1" onclick='$("#liSEOKeywords1").addClass("active");$("#liSEOPageTitle1").removeClass("active");$("#liSEODesc1").removeClass("active");$("#liImageTooltip1").removeClass("active");$("div.liSEOKeywords").fadeIn();$("div.liSEOPageTitle").css("display", "none");$("div.liSEODesc").css("display", "none");$("div.liImageTooltip").css("display", "none");'>
                                                                                                            Keywords</li>
                                                                                                        <li id="liSEODesc1" onclick='$("#liSEODesc1").addClass("active");$("#liSEOPageTitle1").removeClass("active");$("#liSEOKeywords1").removeClass("active");$("#liImageTooltip1").removeClass("active");$("div.liSEODesc").fadeIn();$("div.liSEOKeywords").css("display", "none");$("div.liSEOPageTitle").css("display", "none");$("div.liImageTooltip").css("display", "none");'>
                                                                                                            Description</li>
                                                                                                        <li id="liImageTooltip1" onclick='$("#liImageTooltip1").addClass("active");$("#liSEOPageTitle1").removeClass("active");$("#liSEOKeywords1").removeClass("active");$("#liSEODesc1").removeClass("active");$("div.liImageTooltip").fadeIn();$("div.liSEOKeywords").css("display", "none");$("div.liSEOPageTitle").css("display", "none");$("div.liSEODesc").css("display", "none");'>
                                                                                                            Image Tooltip</li>
                                                                                                    </ul>
                                                                                                    <span class="clear"></span>
                                                                                                    <div class="tab-content-2 liSEOPageTitle" style="display: block">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Height="90px"
                                                                                                                Width="100%" Columns="6" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox></div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 liSEOKeywords">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSEKeywords" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 liSEODesc">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSEDesc" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox></div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 liImageTooltip">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtSEOImageTooltip" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox></div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <th colspan="2">
                                                                                                <div class="main-title-left">
                                                                                                    <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                    <h2>
                                                                                                        Category</h2>
                                                                                                </div>
                                                                                                <div class="main-title-right">
                                                                                                    <a title="close" href="javascript:void(0);" class="show_hideSEO" onclick="return ShowHideButton('ImgSEO','tdSEO');">
                                                                                                        <img id="Img2" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a><a
                                                                                                            title="Minimize" href="#"></a>
                                                                                                </div>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                Main Category :
                                                                                                <asp:TextBox ID="txtMainCategory" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                Category :
                                                                                                <asp:DropDownList ID="ddlCategory" runat="server" Style="margin-left: 29px;" AutoPostBack="false">
                                                                                                    <asp:ListItem Value="0">Select Category</asp:ListItem>
                                                                                                    <asp:ListItem Value="1013">Consumer Electronics</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <th>
                                                                                                    <div class="main-title-left">
                                                                                                        <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
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
                                                                                                                                    background: #f5f5f5; color: #000000;" />
                                                                                                                            </td>
                                                                                                                            <td width="9%">
                                                                                                                                <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click" />
                                                                                                                            </td>
                                                                                                                            <td width="64%">
                                                                                                                                <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete" />
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
                                                                                                <td align="center">
                                                                                                    <div id="Div1">
                                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                                            <tr>
                                                                                                                <td id="td2">
                                                                                                                    <div id="div2" class="slidingDivMainDiv">
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                                                                            <tr class="altrow">
                                                                                                                                <td align="center">
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr class="altrow">
                                                                                                                                <td colspan="2">
                                                                                                                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                                                        <tbody>
                                                                                                                                            <tr>
                                                                                                                                                <th>
                                                                                                                                                    <div class="main-title-left">
                                                                                                                                                        <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                                                        <h2>
                                                                                                                                                            Fields</h2>
                                                                                                                                                    </div>
                                                                                                                                                    <div class="main-title-right">
                                                                                                                                                        <a href="javascript:void(0);" class="show_hideImage" title="Close" onclick="return ShowHideButton('ImgImages','tdImages');">
                                                                                                                                                            <img id="Img3" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                                                                title="Minimize" alt="Minimize"></a></div>
                                                                                                                                                </th>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <div class="slidingDivImage">
                                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                            <tr class="oddrow">
                                                                                                                                                                <td width="20%" valign="top">
                                                                                                                                                                    Location :
                                                                                                                                                                </td>
                                                                                                                                                                <td valign="middle">
                                                                                                                                                                    <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                            <tr class="altrow">
                                                                                                                                                                <td width="20%" valign="top">
                                                                                                                                                                    Tag Name :
                                                                                                                                                                </td>
                                                                                                                                                                <td valign="middle">
                                                                                                                                                                    <asp:TextBox ID="txtTagName" runat="server"></asp:TextBox>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                            <tr class="oddrow">
                                                                                                                                                                <td width="20%" valign="top">
                                                                                                                                                                    Condition :
                                                                                                                                                                </td>
                                                                                                                                                                <td valign="middle">
                                                                                                                                                                    <asp:DropDownList ID="ddlCondition" runat="server" Width="120px">
                                                                                                                                                                        <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                                                                                                                                                        <asp:ListItem Text="Refurbished" Value="Refurbished"></asp:ListItem>
                                                                                                                                                                    </asp:DropDownList>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                            <tr class="altrow">
                                                                                                                                                                <td>
                                                                                                                                                                    IsShipSeparately :
                                                                                                                                                                </td>
                                                                                                                                                                <td>
                                                                                                                                                                    <asp:CheckBox ID="chkIsShipSeparate" runat="server" />
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
                                                                                                                    </div>
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
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="tab2" style="display: none;">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr valign="top">
                                                                <td style="width: 100%">
                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                        <tr class="altrow">
                                                                            <td width="20%">
                                                                                ShippingRateExpedited :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtShippingRateExpedited" runat="server" MaxLength="50" class="order-textfield"
                                                                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="even-row">
                                                                            <td width="20%">
                                                                                ShippingRateStandard :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtShippingRateStandard" runat="server" MaxLength="50" class="order-textfield"
                                                                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow">
                                                                            <td>
                                                                                OfferExpeditedShipping :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtOfferExpeditedShipping" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="even-row">
                                                                            <td width="20%">
                                                                                Assembly Level :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtAssemblyLevel" runat="server" MaxLength="50" class="order-textfield"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow">
                                                                            <td>
                                                                                Board Type :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBoardType" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="even-row">
                                                                            <td>
                                                                                Brightness:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBrightness" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow">
                                                                            <td>
                                                                                Card Type:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCardType" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="even-row">
                                                                            <td>
                                                                                Closure:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtClosure" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow">
                                                                            <td>
                                                                                Dispenser Type:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtDispenser" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="even-row">
                                                                            <td>
                                                                                Easel Type:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtEaselType" runat="server"></asp:TextBox>
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
                                                                        <br />
                                                                        <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return ValidatePage();" />
                                                                        &nbsp;&nbsp;
                                                                        <asp:ImageButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
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
        </table>
    </div>
</asp:Content>
