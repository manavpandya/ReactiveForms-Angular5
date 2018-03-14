<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductBuy.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductBuy" %>

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
    <style type="text/css">
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
        
    </script>
    <script type="text/javascript">
        function btnCheck(inv, btid) {
            var r = document.getElementById(inv).value;
            // alert('sam' +'---- '+r);
            if (r <= 0) {
                alert('Enter Valid Quantity!');
            }
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
    </script>
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
            <tr id="trProductData" runat="server">
                <td class="border-td">
                    <table class="content-table" cellspacing="0" cellpadding="0" border="0" bgcolor="#FFFFFF"
                        width="100%">
                        <tbody>
                            <tr>
                                <td class="border-td-sub">
                                    <table class="add-product" cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tbody>
                                            <tr style="width: 1126px;">
                                                <th>
                                                    <div class="main-title-left">
                                                        <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                        <h2 style="padding-top: 3px;">
                                                            <asp:Label runat="server" ID="lblHeader" Text="Add Product"></asp:Label></h2>
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
                                                    <table border="0" cellpadding="0" cellspacing="0" id="tableproduct" runat="server"
                                                        width="100%">
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0" width="100%" style="float: left; margin-top: 15px;
                                                                    border: 1px solid #DFDFDF; border-bottom: 1px solid #DFDFDF;">
                                                                    <tr id="trmenu" runat="server" class="altrow">
                                                                        <td>
                                                                            <img style="cursor: pointer;" onclick="selecttab(this)" src="/App_Themes/<%=Page.Theme %>/images/required_hover.jpg"
                                                                                id="mitem1" name="mitem1" alt="Required Field" />
                                                                            <img style="cursor: pointer;" onclick="selecttab(this)" src="/App_Themes/<%=Page.Theme %>/images/desired_regular.jpg"
                                                                                id="mitem2" name="mitem2" alt="Optional Field" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <span id="msgid" runat="server" style="cursor: default; margin-left: 40%; text-align: center;
                                                                    color: Red; font-weight: bold;" visible="false">Your product dose not clone until
                                                                    click on save button</span>
                                                                <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="center">
                                                                <div id="tab1">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" id="table1" runat="server">
                                                                        <tr valign="top">
                                                                            <td>
                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                                    <tr class="even-row">
                                                                                        <td colspan="2" id="tdMainDiv">
                                                                                            <div id="divMain" class="slidingDivMainDiv">
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr class="altrow">
                                                                                                        <td style="height: 30px;" valign="middle">
                                                                                                        </td>
                                                                                                        <td align="right">
                                                                                                            <span class="star">*</span> Required Field
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>Seller ID :
                                                                                                        </td>
                                                                                                        <td width="87%">
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
                                                                                                        <%--<td>
                                                                                                            <asp:TextBox ID="txtGtin" runat="server" AutoCompleteType="Disabled" MaxLength="100"
                                                                                                                class="order-textfield"></asp:TextBox>
                                                                                                            <asp:LinkButton ID="lnkRandomNo" Style="color: rgb(51,51,51); text-decoration: underline;
                                                                                                                display: none;" runat="server" OnClick="lnkRandomNo_Click">Generate Random Gtin</asp:LinkButton>
                                                                                                            <span style="color: rgb(51,51,51);">(i.e. UPC or EAN)</span>
                                                                                                        </td>--%>
                                                                                                        <td>
                                                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtGtin" runat="server" AutoCompleteType="Disabled" MaxLength="100"
                                                                                                                            class="order-textfield"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:LinkButton ID="lnkRandomNo" Style="color: rgb(51,51,51); text-decoration: underline;
                                                                                                                            display: none;" runat="server" OnClick="lnkRandomNo_Click">Generate Random Gtin</asp:LinkButton>
                                                                                                                        <span style="color: rgb(51,51,51);">(i.e. UPC or EAN)</span>
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
                                                                                                        <td>
                                                                                                            <a style="padding-left: 174px" href="javascript:void(0);" onclick="return PrintBarcode();">
                                                                                                                Print Barcode</a>
                                                                                                            <br />
                                                                                                            <div id="divBarcodePrint">
                                                                                                                <img alt="" id="imgOrderBarcode" runat="server" />
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp</span>Isbn :
                                                                                                        </td>
                                                                                                        <td>
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
                                                                                                        <td>
                                                                                                            <span class="star">*</span>Manufacture Part No. :
                                                                                                        </td>
                                                                                                        <td>
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
                                                                                                        <td>
                                                                                                            <span class="star">*</span>SKU :
                                                                                                        </td>
                                                                                                        <td>
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
                                                                                                        <td>
                                                                                                            <span class="star">*</span>Title :
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtTitle" runat="server" Style="width: 500px;" class="order-textfield"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">*</span>Weight :
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtWeight" runat="server" Style="width: 80px;" class="order-textfield"
                                                                                                                onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                            lbs. Ex (12.52)
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td>
                                                                                                            <span class="star">*</span>Msrp :
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            $<asp:TextBox ID="txtMsrp" runat="server" class="order-textfield" Style="width: 80px;"
                                                                                                                onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                            Ex (12.00)
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp</span>Listing Price:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            $<asp:TextBox ID="txtListingPrice" runat="server" class="order-textfield" Style="width: 80px;"
                                                                                                                onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                            Ex (8.62)
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp</span>Our Cost:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            $<asp:TextBox ID="txtOurPrice" runat="server" class="order-textfield" Style="width: 80px;"
                                                                                                                onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                            Ex (7.62)
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow" style="display: none;">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp</span>Keywords :
                                                                                                        </td>
                                                                                                        <td>
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
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp</span>Availability :
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtAvailability" runat="server" class="order-textfield"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td>
                                                                                                            <span class="star">*</span>Inventory :
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:UpdatePanel runat="server" ID="inv">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:TextBox ID="txtInventory" runat="server" class="order-textfield" Style="width: 80px;"
                                                                                                                        onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox></ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                            <%-- &nbsp;<img alt="Low Level" title="Low Level" src="../Images/bullet_red.png" id="ImgToggelInventory"
                                                                                                                runat="server" visible="false" style="vertical-align: middle;" />--%>
                                                                                                            &nbsp;<span style="font-size: 28px; vertical-align: middle; color: Red; cursor: default;"
                                                                                                                title="Low Level" id="ImgToggelInventory" runat="server" visible="false">• <font
                                                                                                                    style="font-size: 13px; vertical-align: middle; padding-bottom: 5px;">Low Level</font></span>
                                                                                                            &nbsp;
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
                                                                                                    <tr class="altrow">
                                                                                                        <td class="oddrow" align="left">
                                                                                                            <span class="star">&nbsp;&nbsp;</span>Is Discontinue:
                                                                                                        </td>
                                                                                                        <td align="left">
                                                                                                            <asp:CheckBox ID="chkIsDiscontinue" runat="server" Checked="false" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>Stock/Dropship:
                                                                                                        </td>
                                                                                                        <td>
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
                                                                                                                                <asp:DropDownList ID="ddlProductTypeDelivery" AutoPostBack="true" runat="server"
                                                                                                                                    class="product-type" OnSelectedIndexChanged="ddlProductTypeDelivery_SelectedIndexChanged">
                                                                                                                                </asp:DropDownList>
                                                                                                                            </ContentTemplate>
                                                                                                                        </asp:UpdatePanel>
                                                                                                                    </td>
                                                                                                                    <td align="left">
                                                                                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                                                                            <ContentTemplate>
                                                                                                                                <div id="divvendor" runat="server" visible="false">
                                                                                                                                    <span class="star">*</span> Drop Shipper/Vendor :
                                                                                                                                </div>
                                                                                                                            </ContentTemplate>
                                                                                                                        </asp:UpdatePanel>
                                                                                                                    </td>
                                                                                                                    <td align="left">
                                                                                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                                                                            <ContentTemplate>
                                                                                                                                <asp:DropDownList ID="ddlvendor" runat="server" class="product-type" AutoPostBack="true"
                                                                                                                                    Visible="false">
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
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star"></span>Product&nbsp;Type:
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
                                                                                                                        OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged">
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
                                                                                                    <tr class="oddrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp</span>DisContinue :
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:RadioButton ID="RBtnContiYes" runat="server" Text=" Yes" GroupName="DiscontinueSKU" />&nbsp;<asp:RadioButton
                                                                                                                ID="RBtnContNo" Checked="true" runat="server" Text=" No" GroupName="DiscontinueSKU" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp</span>Display Order :
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtDisplayOrder" runat="server" Style="width: 80px;" class="order-textfield"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow" id="trDimension" runat="server">
                                                                                                        <td align="left" height="30" style="width: 202px" valign="top">
                                                                                                            &nbsp; Dimensions :
                                                                                                        </td>
                                                                                                        <td align="left">
                                                                                                            <span style="padding-left: 1px;">Width</span>
                                                                                                            <asp:TextBox ID="txtwidth" runat="server" Style="width: 80px;" class="order-textfield"
                                                                                                                onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                            <span>&nbsp;x&nbsp;Height</span>
                                                                                                            <asp:TextBox ID="txtheight" runat="server" Style="width: 80px;" class="order-textfield"
                                                                                                                onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                            <span>&nbsp;x&nbsp;Length</span>
                                                                                                            <asp:TextBox ID="txtlength" runat="server" Style="width: 80px;" class="order-textfield"
                                                                                                                onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
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
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 50%" valign="top">
                                                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                <tr>
                                                                                                    <th>
                                                                                                        <div class="main-title-left">
                                                                                                            <img class="img-left" title="Description" alt="Description" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                                            <h2>
                                                                                                                Description</h2>
                                                                                                        </div>
                                                                                                        <div class="main-title-right">
                                                                                                            <a href="javascript:void(0);" class="show_hideProductDesc" onclick="return ShowHideButton('imgDescription','tdProductDescription');">
                                                                                                                <img id="imgDescription" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                    title="Minimize" alt="Minimize" /></a>
                                                                                                        </div>
                                                                                                    </th>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td id="tdProductDescription">
                                                                                                        <div id="tab-container" class="slidingDivProductDesc">
                                                                                                            <ul class="menu">
                                                                                                                <li class="active" id="ordernotes1" onclick='$("#ordernotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.order-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                                    Detail Description</li>
                                                                                                                <%--<li id="privatenotes1" onclick='$("#ordernotes1").removeClass("active");$("#privatenotes1").addClass("active"); $("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.private-notes").fadeIn();$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                                        Warranty</li>
                                                                                                                    <li id="giftnotes1" onclick='$("#giftnotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.gift-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                                                        Features</li>--%>
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
                                                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                <tr>
                                                                                                    <th colspan="2">
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
                                                                                            </table>
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
                                                                                                                    <div id="div3" class="slidingDivCategory">
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr style="display: none;">
                                                                                                                                <td width="18%">
                                                                                                                                    <span class="star">&nbsp;&nbsp;</span> Main&nbsp;Category :
                                                                                                                                </td>
                                                                                                                                <td width="82%">
                                                                                                                                    <asp:TextBox ID="txtMainCategory" runat="server" CssClass="order-textfield" MaxLength="100"
                                                                                                                                        Height="19px" Width="290px"></asp:TextBox>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td width="18%" valign="top" style="padding: 10px;">
                                                                                                                                    <span class="star">&nbsp;</span>Select&nbsp;Category :
                                                                                                                                </td>
                                                                                                                                <td width="82%" align="left" class="list_table_cell_link " id="TDCategory" runat="server"
                                                                                                                                    style="height: 163px;" valign="top">
                                                                                                                                    <div id="divTrvCategories">
                                                                                                                                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="false" Width="292px"
                                                                                                                                            class="product-type">
                                                                                                                                            <asp:ListItem Value="0">Select Category</asp:ListItem>
                                                                                                                                            <asp:ListItem Value="1013">Consumer Electronics</asp:ListItem>
                                                                                                                                        </asp:DropDownList>
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
                                                                                    <tr>
                                                                                        <td colspan="2">
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
                                                                                                                        <td width="10%" valign="top">
                                                                                                                            Icon Image:
                                                                                                                        </td>
                                                                                                                        <td valign="middle" align="left">
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
                                                                                                                                    <td align="left">
                                                                                                                                        <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click" />
                                                                                                                                        &nbsp;<asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                            OnClick="btnDelete_Click" OnClientClick="if(confirm('Delete')){reture true;}else{reture false;}" />
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
                                                                                        <td align="center" colspan="2">
                                                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <th>
                                                                                                            <div class="main-title-left">
                                                                                                                <img class="img-left" title="Fields" alt="Fields" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                <h2>
                                                                                                                    Fields</h2>
                                                                                                            </div>
                                                                                                            <div class="main-title-right">
                                                                                                                <a href="javascript:void(0);" class="show_hideDesc" title="Close" onclick="return ShowHideButton('ImgDesc','tdDesc');">
                                                                                                                    <img id="ImgDesc" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                        title="Minimize" alt="Minimize"></a></div>
                                                                                                        </th>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td valign="top" id="tdDesc" colspan="6">
                                                                                                            <div id="div2" class="slidingDivDesc">
                                                                                                                <table width="80%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                    <tr class="oddrow">
                                                                                                                        <td width="12%" valign="top">
                                                                                                                            Location :
                                                                                                                        </td>
                                                                                                                        <td valign="middle">
                                                                                                                            <asp:TextBox ID="txtLocation" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="altrow">
                                                                                                                        <td valign="top">
                                                                                                                            Tag Name :
                                                                                                                        </td>
                                                                                                                        <td valign="middle">
                                                                                                                            <asp:TextBox ID="txtTagName" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="oddrow">
                                                                                                                        <td valign="top">
                                                                                                                            Condition :
                                                                                                                        </td>
                                                                                                                        <td valign="middle">
                                                                                                                            <asp:DropDownList ID="ddlCondition" runat="server" Width="120px" class="product-type">
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
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div id="tab2" style="display: none;">
                                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr valign="top">
                                                                            <td colspan="2" id="td1">
                                                                                <div id="divTab2" class="slidingDivMainDiv">
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                                        <tr class="altrow">
                                                                                            <td width="12%">
                                                                                                ShippingRateExpedited :
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtShippingRateExpedited" runat="server" MaxLength="50" class="order-textfield"
                                                                                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td>
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
                                                                                                <asp:TextBox ID="txtOfferExpeditedShipping" runat="server" class="order-textfield"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td>
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
                                                                                                <asp:TextBox ID="txtBoardType" class="order-textfield" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td>
                                                                                                Brightness:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtBrightness" class="order-textfield" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                Card Type:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtCardType" class="order-textfield" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td>
                                                                                                Closure:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtClosure" class="order-textfield" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="altrow">
                                                                                            <td>
                                                                                                Dispenser Type:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtDispenser" class="order-textfield" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td>
                                                                                                Easel Type:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtEaselType" class="order-textfield" runat="server"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
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
                                                    </table>
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
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint"></iframe>
    </div>
</asp:Content>
