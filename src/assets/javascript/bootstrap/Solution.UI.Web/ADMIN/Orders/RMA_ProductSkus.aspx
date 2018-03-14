<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMA_ProductSkus.aspx.cs"
    Inherits="Solution.UI.Web.RMA_ProductSkus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Shopping Cart</title>
    <script type="text/javascript" language="javascript">

        var t;
        function selectAll(on) {

            var allElts = document.forms['aspnetForm'].elements;
            var i;

            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];

                if (elt.type == "checkbox") {
                    elt.checked = on;

                }
            }
        }

        function changeprice(id, spanid, hdnstatus, price, txtqty) {
            var originalPrice = 0;
            var idTotal = 0;
            var allkeys = document.getElementById(id).getElementsByTagName('*');
            var allprice = 0;
            var i = 0;
            var flg = false;
            for (i = 0; i < allkeys.length; i++) {
                var elt = allkeys[i];

                if (elt.type == "select-one") {
                    var iPrice = elt.options[elt.selectedIndex].text;
                    var ichk = iPrice.length;
                    if (elt.selectedIndex == 0) {
                        idTotal = 1;
                    }
                    if (iPrice.indexOf('($') > -1) {
                        iPrice = iPrice.toString().substring(iPrice.indexOf('($') + 2);
                        iPrice = iPrice.replace(')', '');
                        allprice += parseFloat(iPrice);
                        var totalprice = 0;
                    }
                }
            }
            if (idTotal == 0) {
                if (document.getElementById(hdnstatus)) { document.getElementById(hdnstatus).value = "2" };
            }
            var qty = 1;
            if (document.getElementById(txtqty) != null && document.getElementById(txtqty).value != '' && document.getElementById(txtqty).value != '0') {
                qty = document.getElementById(txtqty).value;
            }
            var pp = (parseFloat(price) + parseFloat(allprice)) * parseFloat(qty);
            if (document.getElementById(spanid)) { document.getElementById(spanid).innerHTML = pp.toFixed(2); }
        }
        function chkselect() {
            var allElts = document.forms['aspnetForm'].elements;
            var i;
            var Chktrue;
            Chktrue = 0;
            var CountQty = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;
                        var tQty = elt.id.toString().replace('chkSelect', 'TxtQty')
                        if (document.getElementById(tQty) != null && (document.getElementById(tQty).value == '' || document.getElementById(tQty).value == '0')) {
                            CountQty = CountQty + 1;
                        }
                    }
                }
            }

            if (Chktrue < 1) {
                jAlert("Please select at least one record...", "Required information");
                return false;
            }
            if (CountQty >= 1) {
                jAlert("Please enter valid Quantity...", "Required information");
                return false;
            }
            chkHeight();
            return true;
        }

        
    </script>
    <script language="javascript" type="text/javascript">
        var path;
        function OpenPopUp(e) {

            var offsetX = findPosX(e) + 90;
            var offsetY = findPosY(e);
            document.getElementById('zoom1-big').style.left = offsetX + 'px';
            document.getElementById('zoom1-big').style.top = offsetY + 'px';
            document.getElementById('zoom1-big').style.display = '';
            var temp = new Image();
            path = e.id;
            temp.src = e.id;
            //alert(temp.src);
            setTimeout(ShowImage, 1500);
        }
        function ShowImage() {
            document.getElementById('PopUpIconImage').src = path;
        }
        function ClosePopUp() {
            document.getElementById('zoom1-big').style.display = "none";
            document.getElementById('PopUpIconImage').src = "../../Client/images/ajax-loader.gif";
        }
        function findPosX(obj) {
            var curleft = 0;
            if (obj.offsetParent)
                while (1) {
                    curleft += obj.offsetLeft;
                    if (!obj.offsetParent)
                        break;
                    obj = obj.offsetParent;
                }
            else if (obj.x)
                curleft += obj.x;
            return curleft;
        }

        function findPosY(obj) {
            var curtop = 0;
            if (obj.offsetParent)
                while (1) {
                    curtop += obj.offsetTop;
                    if (!obj.offsetParent)
                        break;
                    obj = obj.offsetParent;
                }
            else if (obj.y)
                curtop += obj.y;
            return curtop;
        }
        function chkHeight() {
            
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
            
        }
        function ShowVariantdiv(id) {
            var allType = document.getElementById('divProList').getElementsByTagName('*');
            for (i = 0; i < allType.length; i++) {
                var elt = allType[i];
                if (elt.id.toString().indexOf('divvariant') > -1 && id == elt.id) {
                    elt.style.display = '';

                }
                else if (elt.id.toString().indexOf('divvariant') > -1) {
                    elt.style.display = 'none';
                }
            }
        }
        function HideVariantdiv(id) {
            if (document.getElementById(id)) {
                document.getElementById(id).style.display = 'none';
            }
        }
        function onKeyPressBlockNumbers(e) {

            var key = window.event ? window.event.keyCode : e.which;

            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
    </script>
    <style type="text/css">
        .content-table td
        {
            background-color: transparent !important;
        }
    </style>   
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" language="javascript">
    <%=strScriptVar %>
    </script>
    <form id="aspnetForm" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table border-td"
        style="padding: 2px;">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                        Search Product(s)
                    </div>
                    <div class="main-title-right">
                        <asp:HiddenField ID="hfskus" runat="server" />
                        <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                            <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                    </div>
                </th>
            </tr>
            <tr>
                <td class="border">
                    <table cellpadding="3" cellspacing="0" style="border: 1px solid gray; width: 100%;
                        margin-bottom: 13px;">
                        <tr runat="server" id="trSelectedproduct" visible="false">
                            <td colspan="2">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr style="background-color: #E5F0EF; height: 26px">
                                        <td style="color: #666666; background-color: #fff !important; font-size: 14px; font-family: Verdana,Arial,Helvetica,sans-serif;
                                            font-weight: bold; width: 30%; padding-left: 5px;">
                                            Selected Products:
                                        </td>
                                        <td style="color: #2A5F00; font-size: 11px; font-family: Verdana,Arial,Helvetica,sans-serif;
                                            width: 40%; text-align: center; display: none; background-color: #fff !important;">
                                            Total
                                            <asp:Label ID="lblselProductCount" runat="server" Text="0" Font-Size="11px" Font-Bold="true"></asp:Label>
                                            Products Selected Within
                                            <asp:Label ID="lblselCategoryCount" runat="server" Text="0" Font-Size="11px" Font-Bold="true"></asp:Label>
                                            Categories
                                        </td>
                                        <td align="right" style="padding-top: 5px; padding-right: 5px; background-color: #fff !important;">
                                            <asp:ImageButton ID="btnSubmit" OnClientClick="chkHeight();" runat="server" OnClick="btnSubmit_Click"
                                                Visible="false" />
                                            <asp:ImageButton ID="ImgColse2" OnClientClick="chkHeight();" runat="server" Style="border: 0px"
                                                OnClick="ClearShopping" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div style="border: 1px solid #C1C1C1; font-size: 11px; color: #323232; font-family: Verdana,Arial,Helvetica,sans-serif;">
                                    <div style="background-color: rgb(242, 242, 242); height: 20px; font-weight: bold;
                                        padding-top: 2px; width: 100%; border-bottom: 1px solid #C1C1C1;">
                                        <table cellpadding="0" cellspacing="0" id="divgridhead" runat="server" style="width: 100%">
                                            <tr>
                                                <td width="50%" style="padding-left: 5px;">
                                                    Product Name
                                                </td>
                                                <td width="12%">
                                                    Product Code&nbsp;
                                                </td>
                                                <td width="10%">
                                                    Quantity&nbsp;
                                                </td>
                                                <td width="10%">
                                                    Price&nbsp;
                                                </td>
                                                <td width="10%">
                                                    SubTotal&nbsp;
                                                </td>
                                                <td width="8%">
                                                    Remove&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divselected" runat="server" style="padding-left: 5px;">
                                        <asp:GridView ID="grdSelected" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                            CellPadding="0" CellSpacing="1" CssClass="list-table-border" GridLines="None"
                                            BorderWidth="0" ForeColor="#323232" ShowHeader="false" Width="98%" OnRowDataBound="grdSelected_RowDataBound"
                                            OnRowCommand="grdSelected_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                        <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblProductType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductType") %>'
                                                            Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="52%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQtyGrid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        $<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        $<asp:Label ID="lblGridPrice" runat="server" Text='<%# GetSubTotal(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Quantity"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btndel" runat="server" ImageUrl="~/Admin/images/btndel.gif"
                                                            OnClientClick="javascript:if(confirm('Are you sure , you want to delete this Product ?')){ chkHeight();}else{return false;}"
                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CustomCartID") %>' CommandName="delMe" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle BackColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px"
                                                CssClass="list_table_cell_link" Height="24px" HorizontalAlign="Left" />
                                        </asp:GridView>
                                    </div>
                                    <div style="width: 100%">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td style="width: 82%; padding-top: 10px; padding-bottom: 10px; text-align: right;">
                                                    <b>Total : </b>
                                                </td>
                                                <td style="width: 18%; padding-top: 10px; padding-bottom: 10px;">
                                                    &nbsp; <b>$<asp:Label ID="lblTotal" runat="server"></asp:Label></b>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="border" style="padding-bottom: 5px;">
                    <center>
                        <b>
                            <asp:Label ID="lbMsg" runat="server" Style="font-family: arial,helvetica; color: Red;
                                font-size: 13px;" Font-Size="X-Small"></asp:Label></b></center>
                </td>
            </tr>
            <tr>
                <td class="border">
                    <table cellpadding="3" cellspacing="0" style="border: 1px solid gray; width: 100%">
                        <tr>
                            <th>
                                <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                                    Search New Product(s)
                                </div>
                                <div class="main-title-right">
                                </div>
                            </th>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="2">
                            </td>
                        </tr>
                        <tr style="color: #666666; font-size: 12px; font-family: Verdana,Arial,Helvetica,sans-serif;">
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td width="377px" style="padding-bottom: 3px;">
                                            Search :&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox ID="txtSearch" runat="server" Style="width: 295px;" CssClass="order-textfield"
                                                Height="14px"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="if(document.getElementById('txtSearch').value.trim()==''){ alert('Please Enter Search Term.');document.getElementById('txtSearch').focus();return false;}else{chkHeight(); return true;}" />
                                            <asp:ImageButton ID="btnShowAll" runat="server" OnClick="btnShowAll_Click" OnClientClick="chkHeight();" />
                                        </td>
                                        <td style="color: #666666; font-size: 12px; font-family: Verdana,Arial,Helvetica,sans-serif;"
                                            align="right">
                                            <asp:ImageButton ID="btnSelectItem" runat="server" OnClick="btnSelectItem_Click"
                                                OnClientClick="return chkselect();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trBottom" runat="server" style="color: #666666; font-size: 12px; font-family: Verdana,Arial,Helvetica,sans-serif;">
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td align="left" colspan="2">
                                            <span style="color: #2A5F00; font-size: 11px; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                text-align: right;">Products Found:
                                                <asp:Label ID="foundPro" runat="server" Text="0" Font-Size="10px" Font-Bold="true"></asp:Label></span>
                                        </td>
                                        <%-- <td width="420px">
                                            <div class="paging" id="DivBottom" runat="server">
                                                <span id="trProduct" runat="server">
                                                    <asp:Label ID="lblPage" runat="server" Text="Page"></asp:Label>
                                                    &nbsp; </span>
                                            </div>--%>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="overflow: auto; font-size: 11px; color: #323232; font-family: Verdana,Arial,Helvetica,sans-serif;">
                                    <div id="zoom1-big" style="position: absolute; left: 387px; top: 65px; z-index: 0;">
                                        <img id="PopUpIconImage" alt="" src="/Admin/images/loading.gif" />
                                    </div>
                                    <div id="divProList" runat="server">
                                        <asp:GridView ID="gvListProducts" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                            CellPadding="1" CellSpacing="1" GridLines="None" BorderColor="#e7e7e7" BorderWidth="1"
                                            Width="100%" AllowPaging="true" PageSize="25" OnRowDataBound="gvListProducts_RowDataBound"
                                            OnPageIndexChanging="gvListProducts_PageIndexChanging1" OnRowEditing="gvListProducts_RowEditing">
                                            <FooterStyle BorderWidth="1px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                        <asp:Label ID="lblProductID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quanity" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtQty" Width="25" onkeypress="return onKeyPressBlockNumbers(event);"
                                                            Text="1" Style="text-align: center" MaxLength="2" CssClass="order-textfield"
                                                            runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product Name" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                        <input type="hidden" id="hdnitemprice" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"SalePrice") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product Code" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSKU1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="150px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price" HeaderStyle-ForeColor="White">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        $<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Variant" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White">
                                                    <ItemStyle HorizontalAlign="center" />
                                                    <ItemTemplate>
                                                        <input type="hidden" id="hdnVariantStatus" runat="server" value='' />
                                                        <a href="javascript:void(0);" id="avariantid" runat="server" style="color: #ba2b19;">
                                                            Variant</a>
                                                        <div style="display: none; position: absolute; width: 70%; background-color: #fbfbfb;
                                                            border: solid 2px #707070; z-index: 999; right: 70px;" id="divvariant" runat="server">
                                                            <div style="float: left; background-color: #707070; min-height: 30px; width: 93%;
                                                                text-align: left; color: #fff;">
                                                                &nbsp;<%# DataBinder.Eval(Container.DataItem,"Name") %></div>
                                                            <div style="float: right; background-color: #707070; height: 25px; width: 7%; padding-top: 5px;">
                                                                <a href="javascript:void(0);" id="divinnerclose" runat="server">
                                                                    <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a></div>
                                                            <div style="float: left; margin-left: 10px;">
                                                                <table align="left" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="left" style="font-weight: bold;">
                                                                            Half Price Drapes Price:&nbsp;
                                                                        </td>
                                                                        <td align="left" style="color: #ff0000; font-weight: bold;">
                                                                            $<asp:Label ID="lblvariantprice" ForeColor="Red" runat="server" Text='<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")))%>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div style="float: left; margin: 10px; text-align: left; width: 100%;" id="divAttributes"
                                                                runat="server">
                                                            </div>
                                                            <div style="float: left; width: 97%; text-align: left; padding: 3%;">
                                                                <asp:ImageButton ID="btnSelectGrid" runat="server" ImageUrl="~/Admin/images/add_to_selection_list.jpg"
                                                                    CommandName="Edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                                            </div>
                                                        </div>
                                                        <div style="display: none;" id="divshow">
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                            <AlternatingRowStyle CssClass="altrow" />
                                            <RowStyle BackColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px"
                                                Height="24px" HorizontalAlign="Left" />
                                            <EditRowStyle CssClass="list_table_cell_link" />
                                            <HeaderStyle BackColor="#F2F2F2" CssClass="list-table-title" Height="30px" HorizontalAlign="Left"
                                                Wrap="True" />
                                            <AlternatingRowStyle BackColor="#F2F2F2" BorderColor="White" BorderStyle="Solid"
                                                CssClass="list_table_cell_link" ForeColor="#444545" />
                                            <PagerSettings Position="TopAndBottom" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                font-size: 11px; padding-top: 10px;" colspan="2" id="cleartdid" runat="server">
                                <a style="color: #696A6A; text-decoration: none;" id="lkbAllowAll" class="list_table_cell_link"
                                    href="javascript:selectAll(true);">Check All</a>&nbsp; | <a id="lkbClearAll" style="color: #696A6A;
                                        text-decoration: none;" class="list_table_cell_link" href="javascript:selectAll(false);">
                                        Clear All</a>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:TextBox Visible="false" ID="txtProductIDs" runat="server" TextMode="multiline"
                                    Rows="5" Columns="30"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:HiddenField ID="HdnCustID" runat="server" Value="0" />
    <asp:HiddenField ID="HdnProductSKu" runat="server" Value="" />
    <div style="display: none;">
        <asp:Button ID="btnAddtocartwithvariant" runat="server" OnClick="btnAddtocartwithvariant_Click" />
        <input type="hidden" id="hdnVariProductId" runat="server" value="" />
        <input type="hidden" id="hdnVariQuantity" runat="server" value="" />
        <input type="hidden" id="hdnVariPrice" runat="server" value="" />
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        document.getElementById('zoom1-big').style.display = 'none';

        //alert();
    </script>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
