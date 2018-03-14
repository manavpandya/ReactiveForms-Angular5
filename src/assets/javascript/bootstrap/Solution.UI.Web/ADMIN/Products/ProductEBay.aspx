<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductEBay.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductEBay" %>

<%@ Register Src="~/ADMIN/Controls/ProductHead.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script src="../JS/ProducteBayValidation.js" type="text/javascript"></script>
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
    </style>
    <script type="text/javascript">
        var myWindow;
        function openCenteredCrossSaleWindow(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('ProductSku.aspx?StoreID=' + StoreID + '&clientid=' + x, "subWind", windowFeatures);
        }

        function iframeAutoheight(iframe) {
            var height = iframe.contentWindow.document.body.scrollHeight;

            iframe.height = height + 30;
        }
        function iframeAutoheightById(iframe, height1) {
            //var height = document.getElementById(iframe).contentWindow.document.body.scrollHeight;
            //document.getElementById(iframe).src = document.getElementById(iframe).src;
            //var hgt = $(window).height();
            height1 = parseInt(height1) - parseInt(100);
            $('#' + iframe).css('height', height1.toString() + 'px');

            //document.getElementById(iframe).height = height + 30;
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
    </script>
    <script type="text/javascript">
        function ValidateImageUpload() {
            if (document.getElementById("ContentPlaceHolder1_fuProductIcon") != null && document.getElementById("ContentPlaceHolder1_fuProductIcon").value == '') {
                jAlert("Please Select an Image to Upload.", "Message")
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

        function SetTotalWeight() {

            var totalWeight = 0;
            var Weight = 0;
            for (var i = 0; i < 20; i++) {
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
        function OpenProductOption() {
            var productid = '<%=Request["Id"] %>'
            if (productid != '' && productid != null) {
                window.location.href = 'ProductVariant.aspx?StoreId=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>';
            }
        }

        function Tabdisplay(id) {
            //document.getElementById('ContentPlaceHolder1_hdnTabid').value = id;
            for (var i = 1; i < 15; i++) {

                var divid = "divtab" + i.toString()
                var liid = "lie" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('lie' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }

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
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                <h2 style="padding-top: 3px;">
                                                    <asp:Label runat="server" ID="lblHeader" Text="Add Product"></asp:Label>
                                                </h2>
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
                                        <td id="tdMainDiv">
                                            <div id="divMain" class="slidingDivMainDiv">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                    <tr class="altrow">
                                                        <td align="center">
                                                            <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row">
                                                        <td>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="oddrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">*</span>Product&nbsp;Name:
                                                                    </td>
                                                                    <td style="width: 36%">
                                                                        <asp:TextBox ID="txtProductName" runat="server" MaxLength="500" class="order-textfield"
                                                                            Width="500px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td>
                                                                        <span class="star">*</span>SKU:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSKU" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                    <td width="" rowspan="10" align="left">
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
                                                                                        <a href="javascript:void(0);" class="show_hideWarehouse" onclick="return ShowHideButton('ImgeBayWarehouses','tdeBayWarehouses');">
                                                                                            <img id="ImgeBayWarehouses" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"
                                                                                                class="minimize" title="Minimize" alt="Minimize" /></a>
                                                                                    </div>
                                                                                </th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td id="tdeBayWarehouses" align="left">
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
                                                                                                                        onblur="SetTotalWeight();" MaxLength="5" onkeyup="SetTotalWeight();"></asp:TextBox>
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
                                                                <tr class="oddrow">
                                                                    <td>
                                                                        <span class="star">*</span>Weight:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtWeight" runat="server" Width="80px" class="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                        Ex (12.52)
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td>
                                                                        <span class="star">*</span>Price:
                                                                    </td>
                                                                    <td>
                                                                        $<asp:TextBox ID="txtPrice" runat="server" class="order-textfield" Width="80px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                        Ex (12.00)
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td>
                                                                        <span class="star">&nbsp</span>Sale Price:
                                                                    </td>
                                                                    <td>
                                                                        $<asp:TextBox ID="txtSalePrice" runat="server" class="order-textfield" Width="80px"
                                                                            onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                        Ex (8.62) &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td>
                                                                        <span class="star">*</span>Inventory:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtInventory" runat="server" class="status-textfield" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td>
                                                                        <span class="star">&nbsp</span>Published :
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkPublished" runat="server" Text=" " Checked="true" Width="150px" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">*</span>ebay Category :
                                                                    </td>
                                                                    <td style="">
                                                                        <asp:DropDownList ID="ddlebayCategory" CssClass="order-list" runat="server" Width="500px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">*</span>ebay StoreCategory :
                                                                    </td>
                                                                    <td style="">
                                                                        <asp:DropDownList ID="ddlebayStorecategory" CssClass="order-list" runat="server"
                                                                            Width="500px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">&nbsp;</span>Brand :
                                                                    </td>
                                                                    <td style="">
                                                                        <asp:TextBox ID="txtBrand" runat="server" MaxLength="500" class="order-textfield"
                                                                            Width="180px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">&nbsp;</span>Material :
                                                                    </td>
                                                                    <td style="">
                                                                        <asp:TextBox ID="txtmaterial" runat="server" MaxLength="500" class="order-textfield"
                                                                            Width="180px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">&nbsp;</span>Model :
                                                                    </td>
                                                                    <td style="">
                                                                        <asp:TextBox ID="txtModel" runat="server" MaxLength="500" class="order-textfield"
                                                                            Width="180px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">&nbsp;</span>Color :
                                                                    </td>
                                                                    <td style="">
                                                                         
                                                                        <asp:DropDownList ID="ddlColormap" runat="server" AutoPostBack="false" CssClass="product-type">
                                                                            <asp:ListItem Value="">- Select -</asp:ListItem>
                                                                            <asp:ListItem Value="Beige"> Beige</asp:ListItem>
                                                                            <asp:ListItem Value="Black"> Black</asp:ListItem>
                                                                            <asp:ListItem Value="Blue"> Blue</asp:ListItem>
                                                                            <asp:ListItem Value="Brown"> Brown</asp:ListItem>
                                                                            <asp:ListItem Value="Clear"> Clear</asp:ListItem>
                                                                            <asp:ListItem Value="Gold"> Gold</asp:ListItem>
                                                                            <asp:ListItem Value="Green"> Green</asp:ListItem>
                                                                            <asp:ListItem Value="Grey"> Grey</asp:ListItem>
                                                                            <asp:ListItem Value="Ivory"> Ivory</asp:ListItem>
                                                                            <asp:ListItem Value="Multi"> Multi</asp:ListItem>
                                                                            <asp:ListItem Value="Orange"> Orange</asp:ListItem>
                                                                            <asp:ListItem Value="Pink"> Pink</asp:ListItem>
                                                                            <asp:ListItem Value="Purple"> Purple</asp:ListItem>
                                                                            <asp:ListItem Value="Red"> Red</asp:ListItem>
                                                                            <asp:ListItem Value="Silver"> Silver</asp:ListItem>
                                                                            <asp:ListItem Value="White"> White</asp:ListItem>
                                                                            <asp:ListItem Value="Yellow"> Yellow</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">&nbsp;</span>Type :
                                                                    </td>
                                                                    <td style="">
                                                                        <asp:TextBox ID="txtType" runat="server" MaxLength="500" class="order-textfield"
                                                                            Width="180px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td style="width: 15%">
                                                                        <span class="star">&nbsp</span>ebay Listing Type :
                                                                    </td>
                                                                    <td align="left" style="height: 24px;">
                                                                        <asp:RadioButton ID="RBtnStore" Checked="true" runat="server" Text=" Store" GroupName="ebayListingType" />&nbsp;
                                                                        <asp:RadioButton ID="RBtnAuction" runat="server" Text=" Auction" GroupName="ebayListingType" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td align="left" style="width: 224px; height: 24px;" valign="top">
                                                                        <span class="star">&nbsp</span>ebay ProductID :
                                                                    </td>
                                                                    <td align="left" style="height: 24px">
                                                                        <asp:Label ID="lblebayProductID" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td align="left" style="width: 224px; height: 24px;" valign="top">
                                                                        <span class="star">*</span>ebay Listing Days :
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <asp:TextBox ID="TxtebayListingDays" runat="server" Text="30" CssClass="order-textfield"
                                                                            Width="75px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td align="left" style="width: 224px; height: 24px;" valign="top">
                                                                        <span class="star">&nbsp</span>ebay LastUpdated :
                                                                    </td>
                                                                    <td align="left" style="height: 24px" colspan="3">
                                                                        <asp:Label ID="lblebayLastUpdated" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td align="left" style="width: 224px; height: 24px;" valign="top">
                                                                        <span class="star">&nbsp</span>ebay ExpireDate :
                                                                    </td>
                                                                    <td align="left" style="height: 24px" colspan="3">
                                                                        <asp:Label ID="lblebayExpireDate" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 224px; height: 24px;" valign="top">
                                                                        <span class="star">&nbsp</span>Is Free Shipping :
                                                                    </td>
                                                                    <td align="left" style="height: 24px" colspan="3">
                                                                        <asp:CheckBox ID="chkfreeshipping" runat="server" />
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
                                                                                        <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                        <h2>
                                                                                            Description/Features</h2>
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
                                                                                            <li id="lie1" onclick="Tabdisplay(1);" class="active">Description</li>
                                                                                            <li id="lie2" onclick="Tabdisplay(2);" class="">Features</li>
                                                                                        </ul>
                                                                                        <span class="clear"></span>
                                                                                        <div id="divtab1" class="tab-content product-Description">
                                                                                            <div class="tab-content-3">
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                <tr>
                                                                                                                    <td class="ckeditor-table">
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
                                                                                        <div id="divtab2" class="tab-content product-Description" style="display: none;">
                                                                                            <div class="tab-content-3">
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                <tr>
                                                                                                                    <td class="ckeditor-table">
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
                                                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a;
                                                                                                                                background: #f5f5f5; color: #000000;" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClientClick="return ValidateImageUpload();"
                                                                                                                                OnClick="btnUpload_Click" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                OnClick="btnDelete_Click" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                                <td valign="bottom" id="trUploadFiles" runat="server">
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
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr id="trProductVariant" runat="server">
                                                                <td>
                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                                        <tr>
                                                                            <th colspan="6">
                                                                                <div class="main-title-left">
                                                                                    <img class="img-left" title="Additional Info" alt="Additional Info" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                    <h2>
                                                                                        Product Option</h2>
                                                                                </div>
                                                                                <div class="main-title-right">
                                                                                    <a href="javascript:void(0);" class="show_hideDesc" onclick="return ShowHideButton('ImgPVariant','tdPVariant');">
                                                                                        <img id="ImgPVariant" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a>
                                                                                </div>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" id="tdPVariant" colspan="6">
                                                                                <div id="div5" class="slidingDivDesc">
                                                                                    <iframe id="ifrmProductVariant" runat="server" frameborder="0" marginheight="0" marginwidth="0"
                                                                                        width="100%" scrolling="no"></iframe>
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
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <br />
                    <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return ValidatePage();"
                        OnClick="btnSave_Click" />
                    &nbsp;&nbsp;
                    <asp:ImageButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
                </td>
            </tr>
            <tr>
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
