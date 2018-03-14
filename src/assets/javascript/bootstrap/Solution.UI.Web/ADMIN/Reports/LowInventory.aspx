<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="LowInventory.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.LowInventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }

        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            else {
                chkHeight();
            }
            return true;
        }
    </script>
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
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%; margin-top: 4px">
                <table>
                    <tr>
                        <td align="left" style="padding-left: 0px;">
                            Store :
                        </td>
                        <td align="left" colspan="2">
                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px" onchange="javascript:chkHeight();"
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
                                                    <img class="img-left" title="Low Inventory" alt="Low Inventory" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                    <h2>
                                                        Low Inventory</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                      <%--  <td style="width: 40%">
                                                        </td>--%>
                                                        <td style="width: 50%" align="right">
                                                            Search :
                                                        </td>
                                                       <td style="width: 13%" align="right">
                                                            <asp:DropDownList ID="ddlCategory" runat="server"  AutoPostBack="True" onchange="javascript:chkHeight();"
                                                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" CssClass="order-list">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:DropDownList ID="ddlSearch" AutoPostBack="False" Width="110px"  runat="server"
                                                                CssClass="order-list">
                                                                <asp:ListItem Value="sku">SKU</asp:ListItem>
                                                                <asp:ListItem Value="Name">Product Name</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return validation();" />
                                                        </td>
                                                        <td style="width: 5%; padding-right: 0px;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="javascript:chkHeight();"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div id="divGrid">
                                                    <asp:GridView ID="grdLowInventoryReport" runat="server" AutoGenerateColumns="False"
                                                        BorderStyle="Solid" CellPadding="2" CellSpacing="1" GridLines="None" Width="99%"
                                                        PageSize="50" BorderColor="#E7E7E7" BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found."
                                                        EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                        PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="grdLowInventoryReport_PageIndexChanging">
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
                                                                    <a href="javascript:void(0);" onclick="productquerystring(<%# DataBinder.Eval(Container.DataItem,"ProductID") %>,<%#  DataBinder.Eval(Container.DataItem,"StoreID") %>);"
                                                                        title='<%# DataBinder.Eval(Container.DataItem,"FullName") %>'>
                                                                        <asp:Label ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FullName") %>'></asp:Label></a>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="45%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    SKU
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Store Name
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStorename" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StoreName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Quantity
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="7%" />
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
    </div>
     <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
