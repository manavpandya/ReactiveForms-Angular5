<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCustomerDetail.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.ViewCustomerDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>


    <script type="text/javascript" src="/js/popup.js">
       
    </script>
    <script type="text/javascript">
        function productquerystring(productid, sid) {
            if (productid != null && sid != null) {
                var storeid = sid;// document.getElementById('ContentPlaceHolder1_storeid').value;

                if (storeid == 1) {
                    window.parent.location.href = "/Admin/products/product.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit";
                }
                else if (storeid == 2) {
                    window.parent.location.href = "/Admin/products/ProductEBay.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit";
                }
                else if (storeid == 3) {
                    window.parent.location.href = "/Admin/products/ProductAmazon.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit";
                }
                else if (storeid == 4) {
                    window.parent.location.href = "/Admin/products/ProductBuy.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit";
                }
                else if (storeid == 5) {
                    window.parent.location.href = "/Admin/products/ProductNewEgg.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit";
                }
                else if (storeid == 6) {
                    window.parent.location.href = "/Admin/products/ProductSears.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit";
                }
                else if (storeid == 7) {
                    window.parent.location.href = "/Admin/products/ProductBestBuy.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit";
                }
                else if (storeid == 8) {
                    window.parent.location.href = "/Admin/products/ProductYahoo.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit";
                }

            }

        }
    </script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Tabdisplay(id) {
            document.getElementById('ContentPlaceHolder1_hdnTabid').value = id;
            for (var i = 1; i < 13; i++) {

                var divid = "divtab" + i.toString()
                var liid = "li" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('li' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }

        }

        function iframeAutoheight(iframe) {
            var height = iframe.contentWindow.document.body.scrollHeight;
            iframe.height = height + 5;
        }
        function iframereload(iframe) {
            //chkHeight();
            document.getElementById(iframe).src = document.getElementById(iframe).src;
        }


    </script>
</head>
<body style="background: none">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/tabs.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
        <div class="content-row2">
            <div id="popupcontactdetails" style="z-index: 1000001; width: 1000; height: 500px; padding: 2px 0 0 3px">
                <div class="slidingDivSEO">
                </div>
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td valign="top">
                                <div id="tab-container">
                                    <ul class="menu" style="margin-bottom: -1px;">
                                        <li id="ordernotes1" class="active" runat="server" onclick='$("#ordernotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.order-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>Revenues</li>
                                        <li id="privatenotes1" runat="server" onclick='$("#ordernotes1").removeClass("active");$("#privatenotes1").addClass("active"); $("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.private-notes").fadeIn();$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>Products</li>
                                        <%--<li id="giftnotes1" runat="server" onclick='$("#giftnotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.gift-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");$("div.my-account").css("display", "none");'></li>--%>
                                    </ul>
                                    <div style="float: right; margin-right: 20px;">
                                        <%--     <asp:ImageButton ID="popupviewdetailclose" Style="position: relative; padding-right: 3px;
                                        padding-top: 4px;" ImageUrl="/App_Themes/Gray/images/cancel-icon.png" runat="server"
                                        ToolTip="Close" OnClientClick="javascript:window.parent.location.href=window.parent.location.href;">
                                    </asp:ImageButton>--%>

                                        <asp:ImageButton ID="imgClosee" OnClientClick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose1').click();return false;"
                                            ImageUrl="~/images/Close.png" runat="server" ToolTip="Close"></asp:ImageButton>



                                    </div>
                                    <span class="clear"></span>
                                    <div class="tab-content-2 order-notes" id="ordernotes" runat="server">
                                        <%--<div>
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td class="border-td-sub" style="line-height: 20px;">
                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="table table-border-display table-striped link no-margin-bottom background-default">
                                                            <tbody>
                                                                <tr>
                                                                    <th colspan="2">
                                                                        <div class="main-title-left">

                                                                            <h4>Customer Information</h4>
                                                                        </div>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="height: 15px;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 15%;" valign="top">Customer Number
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblCustomderID" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Company Name
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Name
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Shipping Address
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Literal ID="ltShipAddress" runat="server"></asp:Literal>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Email
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">Phone
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>--%>

                                        <div style="height: 400px; overflow: auto">
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td class="border-td-sub" style="line-height: 20px;">
                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <th colspan="2">
                                                                        <div class="main-title-left">

                                                                            <h4>Revenues</h4>
                                                                        </div>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="height: 15px;">
                                                                        <asp:GridView ID="grdRevenue" AutoGenerateColumns="false" runat="server" BorderStyle="Solid"
                                                                            BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                                            Width="100%" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                                            EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                            ShowFooter="true" ShowHeaderWhenEmpty="true" OnRowDataBound="grdRevenue_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Order" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        Order No
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:void(0);" onclick="javascript:window.parent.location.href='/Admin/Orders/Orders.aspx?id=<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'" title="Order No" style="text-decoration: underline; color: #B92127;">
                                                                                            <asp:Label ID="lblOrderNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        Status
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrderStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderStatus") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        Date
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderDate") %>'></asp:Label>
                                                                                        <asp:Label ID="lblTotalOrderNumberTemp" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FinalOrderTotalCount") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <b>Total (<asp:Label ID="lblTotalOrderNumber" runat="server" Text=''></asp:Label>&nbsp;Orders)</b>
                                                                                    </FooterTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Sub Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                                    <HeaderTemplate>
                                                                                        Sub Total
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrderSubtotal" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "OrderSubtotal")), 2)%>'></asp:Label>
                                                                                        <asp:Label ID="lblFinalOrderSubtotalTemp" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FinalOrderSubtotal")), 2)%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <b>
                                                                                            <asp:Label ID="lblFinalOrderSubtotal" runat="server" Text=''></asp:Label></b>
                                                                                    </FooterTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                                                    <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Coupon/Discount" ItemStyle-HorizontalAlign="Right"
                                                                                    HeaderStyle-HorizontalAlign="Right">
                                                                                    <HeaderTemplate>
                                                                                        Coupon/Discount
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCouponDiscount" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "CouponDiscountAmount")), 2)%>'></asp:Label>
                                                                                        <asp:Label ID="lblFinalDiscountTemp" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FinalDiscount")), 2)%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <b>
                                                                                            <asp:Label ID="lblFinalDiscount" runat="server" Text=''></asp:Label></b>
                                                                                    </FooterTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                                                    <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Shipping" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        Shipping
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblShipping" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "OrderShippingCosts")), 2)%>'></asp:Label>
                                                                                        <asp:Label ID="lblFinalShippingTemp" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FinalShipping")), 2)%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <b>
                                                                                            <asp:Label ID="lblFinalShipping" runat="server" Text=''></asp:Label></b>
                                                                                    </FooterTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                                                    <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Tax" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                                    <HeaderTemplate>
                                                                                        Tax
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrderTax" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "OrderTax")), 2)%>'></asp:Label>
                                                                                        <asp:Label ID="lblFinalTaxTemp" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FinalTax")), 2)%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <b>
                                                                                            <asp:Label ID="lblFinalTax" runat="server" Text=''></asp:Label></b>
                                                                                    </FooterTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                                                    <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                                    <HeaderTemplate>
                                                                                        Total
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrdertotal" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "OrderTotal")), 2)%>'></asp:Label>
                                                                                        <asp:Label ID="lblFinalOrdertotalTemp" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"FinalOrdertotal")),2)%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <b>
                                                                                            <asp:Label ID="lblFinalOrdertotal" runat="server" Text=''></asp:Label></b>
                                                                                    </FooterTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                                                    <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                            <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                            <HeaderStyle Font-Bold="false" />
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="tab-content-2 private-notes" id="privatenotes" runat="server">
                                        <div style="height: 400px; overflow: auto">
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td class="border-td-sub" style="line-height: 20px;">
                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <th colspan="2">
                                                                        <div class="main-title-left">

                                                                            <h4>Products</h4>
                                                                        </div>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="height: 15px;">
                                                                        <asp:GridView ID="grdProduct" AutoGenerateColumns="false" runat="server" BorderStyle="Solid"
                                                                            BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                                            Width="100%" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                                            EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                            ShowFooter="true" ShowHeaderWhenEmpty="true" OnRowDataBound="grdProduct_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        Order No
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:void(0);" onclick="javascript:window.parent.location.href='/Admin/Orders/Orders.aspx?id=<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'" title="Order No" style="text-decoration: underline; color: #B92127;">
                                                                                            <asp:Label ID="lblorderno" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        Item
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <a href="javascript:void(0);" onclick="productquerystring(<%# DataBinder.Eval(Container.DataItem,"ProductID") %>,<%# DataBinder.Eval(Container.DataItem,"StoreID") %>);" title="Product Id" style="text-decoration: underline; color: #B92127;">
                                                                                            <asp:Label ID="lblItem" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        Code
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                    <HeaderTemplate>
                                                                                        Name
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductName") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="40%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="40%"></ItemStyle>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                                    <HeaderTemplate>
                                                                                        Quantity
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                                        <asp:Label ID="lblTotalOrderNumberTemp" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FinalOrderTotalCount") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <b>Total (<asp:Label ID="lblTotalOrderNumber" runat="server" Text=''></asp:Label>&nbsp;Products)</b>
                                                                                    </FooterTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="15%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Right" Width="15%"></ItemStyle>
                                                                                    <FooterStyle HorizontalAlign="Right" Width="15%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Sub Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                                    <HeaderTemplate>
                                                                                        Sub Total
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrderSubtotal" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "OrderSubtotal")), 2)%>'></asp:Label>
                                                                                        <asp:Label ID="lblFinalOrderSubtotalTemp" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FinalOrderSubtotal")), 2)%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <b>
                                                                                            <asp:Label ID="lblFinalOrderSubtotal" runat="server" Text=''></asp:Label></b>
                                                                                    </FooterTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="15%"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Right" Width="15%"></ItemStyle>
                                                                                    <FooterStyle HorizontalAlign="Right" Width="15%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                            <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                            <HeaderStyle Font-Bold="false" />
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <%-- <div class="tab-content-2 gift-notes" id="giftnotes" runat="server">
                                        
                                    </div>--%>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
            <table width="100%" style="padding-top: 80px;" align="center">
                <tr>
                    <td class="LoadingMsg" align="center" style="color: #fff;" valign="middle">
                        <img alt="" src="/images/loding.png" /><br />
                        <b>Loading ... ... Please wait!</b>
                    </td>
                </tr>
            </table>

            <input id="storeid" type="hidden" runat="server" />
        </div>

    </form>
</body>
</html>
