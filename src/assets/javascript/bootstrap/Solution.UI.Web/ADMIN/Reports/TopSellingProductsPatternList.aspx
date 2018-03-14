<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="TopSellingProductsPatternList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.TopSellingProductsPatternList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-row1">
        <div class="create-new-order" style="width:100%; margin:4px;">
            <table>
                <tr>
                    <td align="left" style="padding-left: 4px; ">
                        Store :
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
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
                                        <th colspan="3">
                                            <div class="main-title-left">
                                                <img class="img-left" title="Best Selling Pattern" alt="Best Selling Colors" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                <h2>
                                                    Best Selling Pattern</h2>
                                            </div>
                                        </th>
                                    </tr>
                                   
                                    <tr>
                                        <td align="center" colspan="3">
                                            <div id="divGrid">
                                                <asp:GridView ID="grdProductPatternSalesReport" runat="server" AutoGenerateColumns="False"
                                                    BorderStyle="Solid" CellPadding="2" CellSpacing="1" GridLines="None" Width="99%"
                                                    PageSize="20" BorderColor="#E7E7E7" BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found."
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="grdProductPatternSalesReport_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr No.">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>.
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Product Name
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <a  href="javascript:void(0);" onclick="productquerystring(<%# DataBinder.Eval(Container.DataItem,"RefProductID") %>,<%#  DataBinder.Eval(Container.DataItem,"StoreID") %>);"
                                                                    title='<%# Convert.ToString(Eval("ProductName")) %>'>
                                                                    <asp:Label ID="lblProductname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductName") %>'></asp:Label></a>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="45%" />
                                                        </asp:TemplateField>      
                                                            <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Pattern
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPattern" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Pattern")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>
                                                                                                          
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Sold Quantity
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SoldQty")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    <FooterStyle ForeColor="White" Font-Bold="true" BackColor="#545454" />
                                                </asp:GridView>
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
    </div>
</asp:Content>
