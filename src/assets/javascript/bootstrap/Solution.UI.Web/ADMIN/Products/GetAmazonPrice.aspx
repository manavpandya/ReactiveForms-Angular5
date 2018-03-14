<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="GetAmazonPrice.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.GetAmazonPrice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <script type="text/javascript" src="/admin/js/ScrollableGridPlugin.js?5335"></script>
  
    <script type="text/javascript" src="../../js/jquery-1.8.2.js"></script>
    <link href="../../css/jQuery.fancybox.css" rel="stylesheet" />
    <script type="text/javascript" src="../../js/jquery.elevatezoom.min.js"></script>
    <%--<script type="text/javascript" src="/admin/js/jquery-1.4.1.min.js"></script>--%>
    
    <script type="text/javascript" src="../../js/jquery.fancybox.pack.js"></script>
   <script type="text/javascript" src="/admin/js/jquery.countdownTimer.js"></script>
    <%--<script type="text/javascript" language="javascript">
          $(document).ready(function () { $('#ContentPlaceHolder1_grdamazon').Scrollable(); }
);
</script>--%>
    <style type="text/css">
        .divsetmiddle {
            vertical-align: middle;
            padding-top: 2px;
        }

        .divwidth {
            width: 70px;
        }
     
    </style>
    <style type="text/css">
        .fancybox-wrap {
            top: 40px !important;
        }

        .fancybox-desktop {
            top: 40px !important;
        }

        .fancybox-type-iframe {
            top: 40px !important;
        }

        .fancybox-opened {
            top: 40px !important;
        }

        .btn-small > [class*="icon-"] {
            margin-right: 6px !important;
        }
    </style>
    <style type="text/css">
        .fancybox-wrap {
            top: 40px !important;
        }

        .fancybox-desktop {
            top: 40px !important;
        }

        .fancybox-type-iframe {
            top: 40px !important;
        }

        .fancybox-opened {
            top: 40px !important;
        }

        .textleft {
            text-align: right !important;
        }

        .fancybox-wrap {
            top: 40px !important;
            width: 1100px !important;
        }

        .fancybox-desktop {
            top: 40px !important;
            width: 1100px !important;
        }

        .fancybox-type-iframe {
            top: 40px !important;
            width: 1100px !important;
        }

        .fancybox-opened {
            top: 40px !important;
            width: 1100px !important;
        }

        .fancybox-outer {
            width: 1100px !important;
        }

        .fancybox-opened .fancybox-skin {
            width: 1100px !important;
        }

        .fancybox-skin {
            width: 1100px !important;
        }

        .fancybox-inner {
            width: 1100px !important;
        }
    </style>
    <style type="text/css">
        .panel-body {
            padding: 0;
        }

        #no-more-tables th {
            background: #cdefff !important;
            border: 1px solid #ddd;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
        }

        .oddrow td {
            background: #fff;
            border-bottom: 1px solid #ddd;
        }

        .altrow td {
            background: #f9f9f9;
            border-bottom: 1px solid #ddd;
        }

        .oddrow:hover td {
            background: #ddd;
        }

        .altrow:hover td {
            background: #ddd;


        }
        .style {
   width: 95%;
   font-family: sans-serif;
   font-weight: bold;
   border-style: solid;
}

.colorDefinition {
    background: #000000;
    color : #FFFFFF;
    border-color: #cdefff;
}

 
.size_lg {
   font-size:20px;
   border-width: 3px;
   border-radius: 3px;
}
    </style>
    <%--<script type="text/javascript">
        $(function(){
            $('#m_timer').countdowntimer({
                minutes : 2,
                size : 'lg',
            timeUp : timeisUp

        });
        function timeisUp() {
            alert('test');
        }
        });
    </script>--%>
    <script type="text/javascript">
        //var $j = jQuery.noConflict();
        $(document).ready(function () {
            $('a.fancybox').fancybox();
        });
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
        function OpenCenterWindow(vendorid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(vendorid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
        function DeleteSortData(sortexpression) {
            if (document.getElementById("ContentPlaceHolder1_hdnremove") != null) {
                document.getElementById("ContentPlaceHolder1_hdnremove").value = sortexpression;
                document.getElementById("ContentPlaceHolder1_btnremove").click();
            }
        }
        function CheckPass() {
            if ((document.getElementById('ContentPlaceHolder1_txtpassword').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Password', 'Message', 'ContentPlaceHolder1_txtpassword');
                return false;
            }
            return true;
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left" style="width: 100% !important;">
                                                <img class="img-left" title="Amazon Product List" alt="Amazon Product List" src="/App_Themes/<%=Page.Theme %>/Images/emailTemplate.png" />
                                                <h2>Amazon Product List</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <div style="display: block;" id="password" runat="server" visible="true">
            <table border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                <tr>
                    <td valign="top" style="font-size: 11px;">
                        <span class="required-red"></span>Password
                    </td>
                    <td>:
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtpassword" runat="server" Style="font-size: 11px;" CssClass="order-textfield"
                            Width="143" TextMode="Password"></asp:TextBox>
                    </td>


                </tr>

                <tr class="oddrow">
                    <td colspan="2"></td>
                    <td align="left">
                        <asp:ImageButton ID="btnsubmit" OnClientClick="return CheckPass();"
                            OnClick="btnsubmit_Click" runat="server" ImageUrl="/App_Themes/Gray/images/submit.gif" />
                    </td>
                </tr>
            </table>
        </div>
                                    </tr>
                                    <div style="float: right; width: 100%; line-height: 40px;" id="divsearch" runat="server" visible="false"> 
                                         <tr class="">

                                        <td align="right">
                                            <table>
                                                <tr>

                                                    <td style="width: 30%" align="left">
                                                        <asp:Literal ID="ltrSortExpression" runat="server" Visible="false"></asp:Literal>
                                                        <asp:Label ID="lblOrder" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lbllastsync" runat="server"  Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        Auto Price Update:
                                                    </td>
                                                    <td align="right">
                                                        <asp:DropDownList ID="drpautoprice" runat="server" CssClass="order-list">
                                                            <asp:ListItem Value="">ALL</asp:ListItem>
                                                            <asp:ListItem Value="1">ON</asp:ListItem>
                                                            <asp:ListItem Value="0">OFF</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="right">
                                                        Lowest:
                                                    </td>
                                                    <td align="right">
                                                         <asp:DropDownList ID="drplowest" runat="server" CssClass="order-list">
                                                            <asp:ListItem Value="">ALL</asp:ListItem>
                                                            <asp:ListItem Value="Yes">YES</asp:ListItem>
                                                            <asp:ListItem Value="No">NO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td  align="right">Search By SKU / ASIN :
                                                        <asp:TextBox ID="txtsearch" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 5%">
                                                        <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" />
                                                    </td>
                                                    <td style="width: 5%">
                                                        <asp:Button ID="btnShowAll" runat="server" OnClick="btnShowAll_Click" />
                                                    </td>
                                                    <td>
                                                       <a style="color: red; text-decoration: none;" id="alog" runat="server" class="fancybox fancybox.iframe"
                                                                    href="/Admin/Products/Viewrepricerlog.aspx"><img src="/App_Themes/Gray/images/viewlog.png" /></a>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                      
                                        <tr>
                                            <td>
                                                 <asp:DataList CellPadding="5" RepeatDirection="Horizontal" runat="server" ID="dlPager" CssClass="grid_paging" HorizontalAlign="Right"
                                                OnItemCommand="dlPager_ItemCommand">
                                                <ItemTemplate>
                                                    <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            </td>
                                        </tr>
                                    <tr class="even-row">
                                        <td>
                                           
                                            <div class="panel-body">
                                                <div id="no-more-tables">
                                                    <asp:GridView ID="grdamazon" runat="server" AutoGenerateColumns="False" CssClass="contant-table" BorderColor="#e7e7e7"
                                                        BorderStyle="Solid" BorderWidth="1px" CellPadding="2" CellSpacing="1" DataKeyNames="ID" EmptyDataText="No Record(s) Found."
                                                        AllowSorting="true" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center" OnSorting="grdamazon_Sorting"
                                                        ViewStateMode="Enabled" Width="100%" OnRowDataBound="grdamazon_RowDataBound" OnPageIndexChanging="grdamazon_PageIndexChanging"
                                                        GridLines="None" AllowPaging="false" PageSize="25" PagerSettings-Mode="NumericFirstLast" OnRowCommand="grdamazon_RowCommand" OnRowEditing="grdamazon_RowEditing" OnRowCancelingEdit="grdamazon_RowCancelingEdit">
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <HeaderTemplate>
                                                                    &nbsp;ID
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle HorizontalAlign="left" />
                                                                <ItemTemplate>
                                                                    &nbsp;<asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField >
                                                                <HeaderTemplate>
                                                                  Auto Update
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <HeaderStyle HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    &nbsp;<asp:CheckBox ID="chkIsServiceupdate" runat="server" Checked='<%# Bind("IsServiceupdate") %>' Enabled="false" CssClass="divsetmiddle" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Image" ItemStyle-HorizontalAlign="left" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Image ImageUrl='<%# GetIconImageProduct(Convert.ToString(DataBinder.Eval(Container.DataItem, "ImageName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"StoreId")))%>'
                                                                        ID="imgProduct" runat="server" Width="100px" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" CssClass="center removetr" Width="7%"></ItemStyle>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField SortExpression="SKUID">
                                                                <HeaderTemplate>
                                                                    &nbsp;
                                                            SKU
                                                                    <asp:ImageButton ID="btnsortsku" OnClick="Sorting" runat="server" CommandName="SKUID" CommandArgument="Desc"></asp:ImageButton><asp:Literal runat="server" ID="ltrsortsku"></asp:Literal>/ Condition
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    &nbsp;<asp:Label ID="lblSkuID" runat="server" Text='<%# Bind("SkuId") %>'></asp:Label>
                                                                    <asp:TextBox ID="txtSkuID" runat="server" Text='<%# Bind("SkuId") %>' Visible="false"></asp:TextBox>
                                                                    <br />
                                                                    &nbsp;<asp:Label ID="lblCondition" runat="server" Text='<%# Bind("Condition") %>'></asp:Label>
                                                                    <asp:TextBox ID="txtCondition" runat="server" Text='<%# Bind("Condition") %>' Visible="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField SortExpression="ProductName">
                                                                <HeaderTemplate>
                                                                    &nbsp;
                                                           ProductName
                                                                    <asp:ImageButton ID="btnsortname" OnClick="Sorting" runat="server" CommandArgument="Desc" CommandName="ProductName"></asp:ImageButton><asp:Literal runat="server" ID="ltrsortname"></asp:Literal>
                                                                    / ASIN
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                <ItemTemplate>
                                                                    &nbsp;<a href='<%# Convert.ToString(Eval("sename")) %>' style="cursor:pointer;" runat="server" target="_blank"><asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label></a>
                                                                    <asp:TextBox ID="txtProductName" runat="server" Text='<%# Bind("ProductName") %>' Visible="false"></asp:TextBox>
                                                                    <br />
                                                                    &nbsp;<a href='<%# "http://www.amazon.com/dp/"+ Convert.ToString(Eval("ASINId")) %>' target="_blank"><asp:Label ID="lblASINId" runat="server" Text='<%# Bind("ASINId") %>'></asp:Label></a>
                                                                    <asp:TextBox ID="txtASINId" runat="server" Text='<%# Bind("ASINId") %>' Visible="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    &nbsp;Inv. / Status
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    &nbsp;Amazon:
                                                            <asp:Label ID="lblamazoninv" runat="server" Text='<%# Bind("AmazonInv") %>'></asp:Label>
                                                                    &nbsp;&nbsp; 
                                                            <asp:Label ID="lblamazonstatus" runat="server" Text='<%# Bind("Amazonstatus") %>'></asp:Label>
                                                                    <br />
                                                                    &nbsp;Website:
                                                            <asp:Label ID="lblwebsiteinv" runat="server" Text='<%# Bind("websiteinv") %>'></asp:Label>
                                                                    &nbsp;&nbsp;
                                                            <asp:Label ID="lblwebsitestatus" runat="server" Text='<%# Bind("websitestatus") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    &nbsp;All Vendor
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a style="color: red; text-decoration: none;" id="avendor" runat="server" class="fancybox fancybox.iframe"
                                                                        href="javascript:void(0);"><%# "Total ("+Eval("Vendorcount")+") Vendor"  %></a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="MaxPrice">
                                                                <HeaderTemplate>
                                                                    &nbsp;Max. Offer price
                                                                    <asp:ImageButton ID="btnsortmaxprice" OnClick="Sorting" runat="server" CommandArgument="Desc" CommandName="MaxPrice"></asp:ImageButton><asp:Literal runat="server" ID="ltrsortmaxprice"></asp:Literal>
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblmaxprice" runat="server" Text='<%# "$"+ String.Format("{0:0.00}", Eval("MaxPrice")) %>'></asp:Label>
                                                                    <asp:TextBox ID="txtmaxprice" CssClass="divwidth" runat="server" Text='<%# String.Format("{0:0.00}", Eval("MaxPrice")) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    &nbsp;Adj. Price
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblThresoldsubstract" runat="server" Text='<%# "$"+ String.Format("{0:0.00}", Eval("Thresoldsubstract")) %>'></asp:Label>
                                                                    <asp:TextBox ID="txtThresoldsubstract" CssClass="divwidth" runat="server" Text='<%# String.Format("{0:0.00}", Eval("Thresoldsubstract")) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="MinPrice">
                                                                <HeaderTemplate>
                                                                    &nbsp;Min. Offer Price
                                                                    <asp:ImageButton ID="btnsortminprice" OnClick="Sorting" runat="server" CommandArgument="Desc" CommandName="MinPrice"></asp:ImageButton><asp:Literal runat="server" ID="ltrsortminprice"></asp:Literal>
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdncustom" runat="server" Value='<%# Eval("IsCustom") %>' />
                                                                    <a style="text-decoration: none;" id="aminprice" runat="server" class="fancybox fancybox.iframe" href="javascript:void(0);"><asp:Label ID="lblminprice" runat="server" Text='<%# "$"+ String.Format("{0:0.00}", Eval("MinPrice")) %>'></asp:Label></a>
                                                                    <asp:TextBox CssClass="divwidth" ID="txtminprice" runat="server" Text='<%# String.Format("{0:0.00}", Eval("MinPrice")) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    &nbsp;Amazon Lowest Price
                                                           <br />
                                                                    +Shipping
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    &nbsp;<asp:Label ID="lblLowPrice" runat="server" Text='<%#"$"+ String.Format("{0:0.00}", Eval("LowPrice")) %>'></asp:Label>
                                                                    <asp:TextBox CssClass="divwidth" ID="txtLowPrice" runat="server" Text='<%# String.Format("{0:0.00}", Eval("LowPrice")) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>

                                                                    <br />
                                                                    &nbsp;<asp:Label ID="lblShippingPrice2" runat="server" Text='<%# "$"+ String.Format("{0:0.00}", Eval("ShippingPrice2")) %>'></asp:Label>&nbsp;
                                                            <asp:TextBox ID="txtShippingPrice2" runat="server" Text='<%# String.Format("{0:0.00}", Eval("ShippingPrice2")) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="FulfilledBy">
                                                                <HeaderTemplate>
                                                                    &nbsp;Is Lowest
                                                                    <asp:ImageButton ID="btnsortislowest" OnClick="Sorting" runat="server" CommandArgument="Desc" CommandName="FulfilledBy"></asp:ImageButton><asp:Literal runat="server" ID="ltrsortislowest"></asp:Literal>
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    &nbsp;<asp:Label ID="lblFulfilledBy" Visible="false" runat="server" Text='<%# Bind("FulfilledBy") %>'></asp:Label>
                                                                    <asp:TextBox ID="txtFulfilledBy" runat="server" Text='<%# Bind("FulfilledBy") %>' Visible="false"></asp:TextBox>
                                                                    <img id="imagstatus" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    &nbsp;Your Price
                                                                    <asp:ImageButton ID="btnsortyourprice" OnClick="Sorting" runat="server" CommandArgument="Desc" CommandName="YourPrice"></asp:ImageButton><asp:Literal runat="server" ID="ltrsortyourprice"></asp:Literal>
                                                                    <br />
                                                                    +Shipping
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    &nbsp;<asp:Label ID="lblYourPrice" runat="server" Text='<%# "$"+ String.Format("{0:0.00}", Eval("YourPrice")) %>'></asp:Label>
                                                                    <asp:TextBox CssClass="divwidth" ID="txtYourPrice" runat="server" Text='<%# String.Format("{0:0.00}", Eval("YourPrice")) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>

                                                                    <br />
                                                                    &nbsp;<asp:Label ID="lblShippingPrice1" runat="server" Text='<%# "$"+ String.Format("{0:0.00}", Eval("ShippingPrice1")) %>'></asp:Label>&nbsp;
                                                            <asp:TextBox ID="txtShippingPrice1" CssClass="divwidth" runat="server" Text='<%# String.Format("{0:0.00}", Eval("ShippingPrice1")) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    &nbsp;Vendor Lowest
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    $<asp:Label ID="lblvendorlowest" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    &nbsp;Formula Price
                                                                    <asp:ImageButton ID="btnsortformulaprice" OnClick="Sorting" runat="server" CommandArgument="Desc" CommandName="ThresoldPrice"></asp:ImageButton><asp:Literal runat="server" ID="ltrsortformulaprice"></asp:Literal><br />
                                                                    [Use Formula Price]
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkuseformula" runat="server" Checked='<%# Bind("UseFormulaPrice") %>' Enabled="false" CssClass="divsetmiddle" />

                                                                    <asp:Label ID="lblThresoldPrice" runat="server" Text='<%# "$"+ String.Format("{0:0.00}", Eval("ThresoldPrice")) %>'></asp:Label>
                                                                    <asp:TextBox ID="txtThresoldPrice" CssClass="divwidth" runat="server" Text='<%# String.Format("{0:0.00}", Eval("ThresoldPrice")) %>' Visible="false" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox><br />
                                                                    <asp:Label ID="lblwebsitePrice" runat="server" Text='<%# "On HPD $"+ String.Format("{0:0.00}", Eval("websitePrice")) %>' Style="color: #b92127;"></asp:Label>
                                                                    <asp:Label ID="lbllogdate" runat="server" Text='<%# Bind("logdate") %>' Visible="false"></asp:Label>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Operations">
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton CssClass="btnclass" Style="border: 1px solid #ba2b19;" runat="server" ID="_editLinkButton" ToolTip="Edit"
                                                                        CommandName="edit" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton>
                                                                    <asp:ImageButton CssClass="btnclass" ID="btnSave" Visible="false"
                                                                        runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
                                                                        CommandName="Save"></asp:ImageButton>&nbsp;<asp:ImageButton CssClass="btnclass" ID="btnCancel" runat="server" Visible="false" CommandName="Cancel"
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>'></asp:ImageButton>

                                                                    <asp:ImageButton CssClass="btnclass" runat="server" ID="getprice" ToolTip="Refresh" CommandName="GetPrice"
                                                                        CommandArgument='<%# Eval("SkuId") %>' Text="Get Price"></asp:ImageButton>
                                                                    <br />
                                                                    <div id="timer" class="style colorDefinition size_lg" runat="server"></div>
                                                                    <asp:ImageButton Style="margin-top: 5px;" CssClass="btnclass" runat="server" ID="updateprice" ToolTip="Update Price In Amazon" CommandName="updateprice" OnClientClick="javascript:if(confirm('Are you sure want to Update Price in Amazon?')){return true;}else{return false;}"
                                                                        CommandArgument='<%# Eval("SkuId") %>' Text="Update Price"></asp:ImageButton>
                                                                    <br />
                                                                    <asp:Label ID="lbllastupdate" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div>
                                            </div>



                                        </td>
                                    </tr></div>
                                  
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="display: none;">
            <asp:Button ID="btnremove" runat="server" OnClick="btnremove_Click" />
            <asp:Button ID="btntemp" runat="server" OnClick="btntemp_Click" />
            <asp:HiddenField ID="hdncurrenttemp" runat="server" Value="0" />
            <asp:HiddenField ID="hdnremove" runat="server" />
        </div>
    </div>
       <%----%>
    

</asp:Content>
