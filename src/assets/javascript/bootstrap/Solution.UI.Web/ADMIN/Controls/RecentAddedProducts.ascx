<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecentAddedProducts.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.RecentAddedProducts" %>
<div>
     <script type="text/javascript">

         function productquerystring(productid, sid) {
             if (productid != null && sid != null) {
                 var storeid = sid;// document.getElementById('ContentPlaceHolder1_storeid').value;

                 if (storeid == 1) {
                     window.location.href = "/Admin/products/product.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                 }

                 else if (storeid == 3) {
                     window.location.href = "/Admin/products/ProductAmazon.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                 }
                 else if (storeid == 4) {
                     window.location.href = "/Admin/products/ProductOverStock.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                 }
                 else if (storeid == 7) {
                     window.location.href = "/Admin/products/ProductEBay.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                 }

                 else if (storeid == 8) {
                     window.location.href = "/Admin/products/ProductSears.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                 }

                 else {

                     window.location.href = "/Admin/products/product.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                 }





             }

         }

    </script>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/item-by-sale.png" alt="Recent Products"
                            title="Recent Products" class="img-left" />
                        <h2>
                            Recent Products</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgRecentItem','trRecentProduct','tblRecentItem');">
                            <img class="minimize" title="Minimize" id="imgRecentItem" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trRecentProduct">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdRecentProducts" runat="server" CssClass="dashboard-left" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red" Style="margin-bottom: 0 !important;"
                        OnRowDataBound="grdRecentProducts_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="SKU">
                                <ItemTemplate>
                                    <%# Eval("SKU") %>.
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="productquerystring(<%# DataBinder.Eval(Container.DataItem,"ProductID") %>,<%#  DataBinder.Eval(Container.DataItem,"StoreID") %>);"

                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" title='<%# Server.HtmlEncode(Convert.ToString(Eval("ToolTip"))) %>'>
                                        <%# Eval("ProductName")%></a>
                                    <input type="hidden" id="hdnStoreIId" runat="server" value='<%# Eval("StoreID") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Price">
                                <ItemTemplate>
                                    <%#String.Format("{0:C}", Convert.ToDecimal(Eval("Price")))%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="tblRecentItem">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%">
                                    <asp:Literal ID="ltrAddProduct" runat="server"></asp:Literal>
                                    <img class="img-right" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/Icon/Add_Product.gif" />
                                </td>
                                <td align="right" width="50%">
                                    <asp:Literal ID="ltrProductList" runat="server"></asp:Literal>
                                    <img class="img-right" title="Product List" alt="Product List" src="/App_Themes/<%=Page.Theme %>/icon/view-report.png" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>
