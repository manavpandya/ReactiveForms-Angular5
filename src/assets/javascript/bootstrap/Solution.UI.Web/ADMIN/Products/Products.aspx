<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Products"
    Theme="Gray" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Gray/css/style.css" rel="stylesheet" type="text/css" />

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
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:ImageButton ID="ibtnUpdate" runat="server" ImageUrl='/App_Themes/<%=Page.Theme %>/images/save-changes.png'
                        OnClick="ibtnUpdate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvProduct" AutoGenerateColumns="false" runat="server" CellPadding="2"
                        CellSpacing="1" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                        AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" HeaderStyle-ForeColor="#3c2b1b"
                        BorderColor="#e7e7e7" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                        Style="border: 1px solid #E7E7E7;" GridLines="None" OnRowDataBound="gvProduct_RowDataBound"
                        Width="100%" OnPageIndexChanging="gvProduct_PageIndexChanging" HeaderStyle-Height="30px">
                        <Columns>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <%#Eval("Name")%>
                                    <asp:HiddenField ID="hdnProductId" Value='<%#Eval("ProductID") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <ItemTemplate>
                                    <%#Eval("SKU")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Display Order">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDisplay" runat="server" Text='<%#Eval("DisplayOrder")%>' Width="50px"
                                        Style="text-align: center; border: 1px solid #BCC0C1;"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inventory">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtInventory" runat="server" Text='<%#Eval("Inventory")%>' Width="50px"
                                        Style="text-align: center; border: 1px solid #BCC0C1;"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Price">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrice" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "price")), 2)%>'
                                        runat="server" Width="70px" Style="text-align: center; border: 1px solid #BCC0C1;"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sale Price">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSaleprice" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "SalePrice")), 2)%>'
                                        runat="server" Width="70px" Style="text-align: center; border: 1px solid #BCC0C1;"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderText="CreatedOn">
                                <ItemTemplate>
                                    <%#Eval("CreatedOn")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="UpdatedOn">
                                <ItemTemplate>
                                    <%#Eval("Updatedon")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UpdatedBy">
                                <ItemTemplate>
                                    <%#Eval("AdminName")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                      <%--<a  href="javascript:void(0);" onclick="productquerystring(<%# Eval("ProductID") %>,<%# Eval("StoreID") %>);"--%>
                                                                   
                                    <asp:HyperLink ID="hplEdit" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/Images/Edit.gif"
                                        ToolTip="Edit" Target="_top" NavigateUrl='<%# geturl(Convert.ToString(DataBinder.Eval(Container.DataItem,"StoreID")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ProductID"))) %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle ForeColor="White" Font-Bold="true" BackColor="#696969" />
                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)

                return false;


            return true;
        }
        function isNumberKeyDisplay(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))

                return false;


            return true;
        }
        

    </script>
    </form>
</body>
</html>
