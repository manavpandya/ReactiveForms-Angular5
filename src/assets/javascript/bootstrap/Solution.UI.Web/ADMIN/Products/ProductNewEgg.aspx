<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductNewEgg.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductNewEgg" %>

<%@ Register Src="~/ADMIN/Controls/ProductHead.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        function Checkfields() {
            var ReturnData = true;
            if (document.getElementById("ContentPlaceHolder1_txtproductname").value == '') {
                jAlert('Please enter Product Name.', 'Message', 'ContentPlaceHolder1_txtproductname');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtSKU").value == '') {
                jAlert('Please enter SKU.', 'Message', 'ContentPlaceHolder1_txtSKU');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_ddlManufacture") != null && document.getElementById("ContentPlaceHolder1_ddlManufacture").selectedIndex == 0) {
                jAlert('Please Select Manufacturer', 'Message', 'ContentPlaceHolder1_ddlManufacture');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlManufacture').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtisbn").value == '') {
                jAlert('Please enter ISBN.', 'Message', 'ContentPlaceHolder1_txtisbn');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtUPC").value == '') {
                jAlert('Please enter UPC.', 'Message', 'ContentPlaceHolder1_txtUPC');
                ReturnData = false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtUPC').value != '' && document.getElementById('ContentPlaceHolder1_txtUPC').value.length < 12) {
                jAlert('UPC Length must be grater than or equal to 12 digit.', 'Message', 'ContentPlaceHolder1_txtUPC');
                //                alert('UPC Length must be grater than or equal to 12 digit.');
                //                document.getElementById('ContentPlaceHolder1_txtupc').focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtMaincategory").value == '') {
                jAlert('Please enter Main Category.', 'Message', 'ContentPlaceHolder1_txtMaincategory');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtWebsiteShortTitle").value == '') {
                jAlert('Please enter Website Short Title.', 'Message', 'ContentPlaceHolder1_txtWebsiteShortTitle');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtitemlength").value == '') {
                jAlert('Please enter Length.', 'Message', 'ContentPlaceHolder1_txtitemlength');
                ReturnData = false;
            }

            else if (document.getElementById("ContentPlaceHolder1_txtitemwidth").value == '') {
                jAlert('Please enter Width.', 'Message', 'ContentPlaceHolder1_txtitemwidth');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtitemheight").value == '') {
                jAlert('Please enter Height.', 'Message', 'ContentPlaceHolder1_txtitemheight');
                ReturnData = false;
            }

            else if (document.getElementById("ContentPlaceHolder1_txtItemWeight").value == '') {
                jAlert('Please enter Weight.', 'Message', 'ContentPlaceHolder1_txtItemWeight');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtPrice").value == '') {
                jAlert('Please enter Price.', 'Message', 'ContentPlaceHolder1_txtPrice');
                ReturnData = false;
            }
            else if ((document.getElementById("ContentPlaceHolder1_txtPrice").value < document.getElementById("ContentPlaceHolder1_txtsellingprice").value)) {
                jAlert('Sale Price should be less than Price', 'Message', 'ContentPlaceHolder1_txtsellingprice');
                ReturnData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtInventory").value == '') {
                jAlert('Please enter Inventory.', 'Message', 'ContentPlaceHolder1_txtInventory');
                ReturnData = false;
            }




            return ReturnData;
        }
    </script>
    <script type="text/javascript">
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/Images/add-product-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="Add Product" ID="lblTitle"></asp:Label></h2>
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
                                    <tr class="altrow">
                                        <td align="center">
                                            <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <span class="star">*</span> Required Field
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Store Name :
                                                    </td>
                                                    <td style="width: 80%; height: 30px" colspan="2">
                                                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="226px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Product Name :
                                                    </td>
                                                    <td colspan="2" style="height: 30px">
                                                        <asp:TextBox runat="server" ID="txtproductname" MaxLength="400" Width="410px" CssClass="order-textfield"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>SKU :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtSKU" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Seller Part# :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtsellerpartnumber" runat="server" CssClass="order-textfield" Width="410px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Manufacturer :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddlManufacture" runat="server" CssClass="order-list" Width="150px"
                                                            AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>ISBN :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtisbn" CssClass="order-textfield" Width="150px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>UPC :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtUPC" CssClass="order-textfield" Width="150px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 224px; height: 24px;" valign="middle">
                                                        <span class="star">*</span>Main Category :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtMaincategory" runat="server" CssClass="order-textfield" Width="302px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="top">
                                                        <span class="star">*</span>Category :
                                                    </td>
                                                    <td align="left" colspan="2" id="TD1" runat="server" style="height: 163px">
                                                        <div id="treeSelectedvalue">
                                                            <asp:TreeView ID="trvCategories" runat="server" ShowCheckBoxes="Leaf" Width="304px"
                                                                PopulateNodesFromClient="True" ShowLines="true">
                                                            </asp:TreeView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Manufacture Item URL :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtManufactureItemURL" runat="server" CssClass="order-textfield"
                                                            Width="302px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Related Seller Part # :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtRelatedSellerPartno" runat="server" CssClass="order-textfield"
                                                            Width="410px" Height="70px" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Website Short Title :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtWebsiteShortTitle" CssClass="order-textfield"
                                                            Width="302px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Website Long Title :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtWebsiteLongTitle" CssClass="order-textfield" Width="302px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="top">
                                                        <span class="star">&nbsp;&nbsp;</span>Product Description :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox TextMode="multiLine" ID="txtProductDescription" Rows="10" Columns="80"
                                                            runat="server"></asp:TextBox>
                                                        <script type="text/javascript">
                                                            CKEDITOR.replace('<%= txtProductDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                            CKEDITOR.on('dialogDefinition', function (ev) {
                                                                if (ev.data.name == 'image') {
                                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                                    btn.hidden = false;
                                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                }
                                                                if (ev.data.name == 'link') {
                                                                    btn = ev.data.definition.getContents('info').get('browse');
                                                                    btn.hidden = false;
                                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                }
                                                            });
                                                        </script>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Item Length :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtitemlength" CssClass="order-textfield" Width="100px"
                                                            onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Item Width :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtitemwidth" CssClass="order-textfield" Width="100px"
                                                            onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Item Height :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtitemheight" CssClass="order-textfield" Width="100px"
                                                            onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Item Weight :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtItemWeight" CssClass="order-textfield" Width="100px"
                                                            onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                        <span style="color: Gray">(Example : 1.30)</span>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Item Package :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddlitempackage" runat="server" CssClass="order-list" Width="150px"
                                                            AutoPostBack="false">
                                                            <asp:ListItem Value="Retail" Selected="true">Retail</asp:ListItem>
                                                            <asp:ListItem Value="OEM">OEM</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Shipping Restriction :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkShippingRestiction" runat="server" Width="150px" />
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 224px; height: 24px;" valign="top">
                                                        <span class="star">*</span>Price :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="order-textfield" Width="88px"
                                                            onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                        <span style="color: Gray">(Example : 12.00)</span>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 224px; height: 24px;" valign="top">
                                                        <span class="star">&nbsp;&nbsp;</span>Selling Price :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtsellingprice" runat="server" CssClass="order-textfield" Width="88px"
                                                            onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Shipping :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddlshipping" runat="server" CssClass="order-list" Width="100px"
                                                            AutoPostBack="false">
                                                            <asp:ListItem Value="0" Selected="True">Default</asp:ListItem>
                                                            <asp:ListItem Value="Free">Free</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">*</span>Inventory :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtInventory" runat="server" CssClass="order-textfield" Width="88px"
                                                            onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                        &nbsp;<span style="font-size: 28px; vertical-align: middle; color: Red; cursor: default;"
                                                            title="Low Level" id="ImgToggelInventory" runat="server" visible="false">• <font
                                                                style="font-size: 13px; vertical-align: middle; padding-bottom: 5px;">Low Level</font></span>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Activation Mark :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkActivationMark" runat="server" Width="150px" />
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Main Image :
                                                    </td>
                                                    <td colspan="2">
                                                        <img alt="Upload" id="ImgLarge" runat="server" width="150" style="margin-bottom: 5px;
                                                            border: 1px solid darkgray" /><br />
                                                        &nbsp;<asp:FileUpload ID="fuImage" runat="server" />
                                                        &nbsp;<asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" Style="vertical-align: bottom;"
                                                            OnClick="btnUpload_Click" />
                                                        <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete"
                                                            OnClick="btnDelete_Click" Style="vertical-align: bottom;" />
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Prop 65 :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkProp65" runat="server" Width="150px" />
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Prop 65 - Motherboard :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkProp65Motherboard" runat="server" Width="150px" />
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Age 18+ Verification :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkAgeVerification" runat="server" Width="150px" />
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Choking Hazard 1 :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddlchoking1" runat="server" CssClass="order-list" Width="150px"
                                                            AutoPostBack="false">
                                                            <asp:ListItem Value="Small parts" Selected="True">Small parts</asp:ListItem>
                                                            <asp:ListItem Value="Is a small ball">Is a small ball</asp:ListItem>
                                                            <asp:ListItem Value="Contains a small ball">Contains a small ball</asp:ListItem>
                                                            <asp:ListItem Value="Is a marble">Is a marble</asp:ListItem>
                                                            <asp:ListItem Value="Contains a marble">Contains a marble</asp:ListItem>
                                                            <asp:ListItem Value="Contains balloons">Contains balloons</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Choking Hazard 2 :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddlchoking2" runat="server" CssClass="order-list" Width="150px"
                                                            AutoPostBack="false">
                                                            <asp:ListItem Value="Small parts" Selected="True">Small parts</asp:ListItem>
                                                            <asp:ListItem Value="Is a small ball">Is a small ball</asp:ListItem>
                                                            <asp:ListItem Value="Contains a small ball">Contains a small ball</asp:ListItem>
                                                            <asp:ListItem Value="Is a marble">Is a marble</asp:ListItem>
                                                            <asp:ListItem Value="Contains a marble">Contains a marble</asp:ListItem>
                                                            <asp:ListItem Value="Contains balloons">Contains balloons</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Choking Hazard 3 :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddlchoking3" runat="server" CssClass="order-list" Width="150px"
                                                            AutoPostBack="false">
                                                            <asp:ListItem Value="Small parts" Selected="True">Small parts</asp:ListItem>
                                                            <asp:ListItem Value="Is a small ball">Is a small ball</asp:ListItem>
                                                            <asp:ListItem Value="Contains a small ball">Contains a small ball</asp:ListItem>
                                                            <asp:ListItem Value="Is a marble">Is a marble</asp:ListItem>
                                                            <asp:ListItem Value="Contains a marble">Contains a marble</asp:ListItem>
                                                            <asp:ListItem Value="Contains balloons">Contains balloons</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Choking Hazard 4 :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddlchoking4" runat="server" CssClass="order-list" Width="150px"
                                                            AutoPostBack="false">
                                                            <asp:ListItem Value="Small parts" Selected="True">Small parts</asp:ListItem>
                                                            <asp:ListItem Value="Is a small ball">Is a small ball</asp:ListItem>
                                                            <asp:ListItem Value="Contains a small ball">Contains a small ball</asp:ListItem>
                                                            <asp:ListItem Value="Is a marble">Is a marble</asp:ListItem>
                                                            <asp:ListItem Value="Contains a marble">Contains a marble</asp:ListItem>
                                                            <asp:ListItem Value="Contains balloons">Contains balloons</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="top">
                                                        <span class="star">&nbsp;&nbsp;</span>SE-Page Title :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Width="410px"
                                                            Height="94px" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="top">
                                                        <span class="star">&nbsp;&nbsp;</span>SE-Keywords :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtSEKeyword" runat="server" CssClass="order-textfield" Width="410px"
                                                            Columns="6" Height="86px" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 202px; height: 30px;" valign="top">
                                                        <span class="star">&nbsp;&nbsp;</span>SE-Description :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtSEDescription" runat="server" CssClass="order-textfield" Width="410px"
                                                            Columns="6" Height="94px" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 202px; height: 30px;" valign="middle">
                                                        <span class="star">&nbsp;&nbsp;</span>Active :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkPublished" runat="server" Text=" Published" Checked="true" Width="150px" />
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 224px; height: 24px;" valign="top">
                                                        <span class="star">&nbsp;&nbsp;</span>Display Order :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="order-textfield" Width="73px"
                                                            MaxLength="5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 70%">
                                                        <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClientClick="return Checkfields();" OnClick="btnSave_Click" CausesValidation="true" />
                                                        &nbsp;<asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            OnClick="btnCancel_Click" />
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
                <td style="height: 10px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" alt="" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
