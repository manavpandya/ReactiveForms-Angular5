<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LowInventoryAlert.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Controls.LowInventoryAlert" %>
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
                        <img src="/App_Themes/<%=Page.Theme %>/icon/item-by-sale.png" alt="Top 10 Low Inventory Items"
                            title="Low Inventory Alert" class="img-left" />
                        <h2>
                             Low Inventory Alert</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgLinvt','trLowInventoryAlert','tblLowInvtlst');">
                            <img class="minimize" title="Minimize" id="imgLinvt" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trLowInventoryAlert">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdLowInventoryAlert" runat="server" CssClass="dashboard-left" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex +1 %>.
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" Width="2%" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="productquerystring(<%# DataBinder.Eval(Container.DataItem,"ProductID") %>,<%#  DataBinder.Eval(Container.DataItem,"StoreID") %>);"
                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" title='<%# Convert.ToString(Eval("FullName")) %>'>
                                        <%#Eval("ProductName")%></a></ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                   Qty&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("INVENTORY")%>
                                    &nbsp;
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="tblLowInvtlst">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%">
                                </td>
                                <td align="right" width="50%">
                                    <a title="View List" href="/Admin/Reports/LowInventoryAlert.aspx?Storeid=<%=storeid%>" style="cursor: pointer;"><span>View List</span></a>
                                    <img class="img-right" title="View List" alt="View List" src="/App_Themes/<%=Page.Theme %>/icon/view-report.png" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>