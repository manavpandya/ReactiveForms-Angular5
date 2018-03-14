<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ProductSearchLink.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.BulkImageReport" %>

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
    <script type="text/javascript">
        function openCenteredCrossSaleWindow(StoreID, ProductID) {

            var width = 1000;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));

            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('UpdateProductImage.aspx?StoreID=' + StoreID + '&Mode=edit&ProductID=' + ProductID + "&subWind", "Mywindow", windowFeatures);
        }
        function openCenteredCrossSaleWindowView(StoreID, ProductID) {

            var width = 1000;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));

            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('ViewProductImages.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + "&subWind", "Mywindow", windowFeatures);
        }
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>');
                return false;
            }
            else {
                chkHeight();
            }
            return true;
        }
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
    </script>

      <script language="javascript" type="text/javascript">


          function expandcollapse(obj, row) {
              var alldiv = document.getElementById('ContentPlaceHolder1_grdProductColorSalesReport').getElementsByTagName('div');


              if (alldiv != null && alldiv.length > 0) {
                  for (var i = 0; i < alldiv.length; i++) {
                      var divall = alldiv[i];

                      if (divall.id == obj) {
                          var div = document.getElementById(obj);
                          var img = obj.toString().replace('divchild', 'imgdiv'); //

                          if (div.style.display == "none") {
                              div.style.display = "block";
                              document.getElementById('ContentPlaceHolder1_hdnrowid').value = obj;
                              if (row == 'alt') {
                                  document.getElementById(img).src = "/images/minimize.png";
                              }
                              else {
                                  document.getElementById(img).src = "/images/minimize.png";
                              }
                              document.getElementById(img).alt = "Close to view other Listing";
                          }
                          else {
                              div.style.display = "none";
                              document.getElementById('ContentPlaceHolder1_hdnrowid').value = "";
                              if (row == 'alt') {
                                  document.getElementById(img).src = "/images/expand.gif";
                              }
                              else {
                                  document.getElementById(img).src = "/images/expand.gif";
                              }
                              document.getElementById(img).alt = "Expand to show Listing";
                          }
                      }
                      else if (divall.id != "" && divall.id != obj) {

                          var imgid = divall.id.toString().replace('divchild', 'imgdiv');
                          var img = document.getElementById(imgid);
                          divall.style.display = "none";
                          if (row == 'alt') {
                              img.src = "/images/expand.gif";
                          }
                          else {
                              img.src = "/images/expand.gif";
                          }
                          img.alt = "Expand to show Listing";

                      }
                  }
              }
          }
    </script>


    <style type="text/css">
        .auto-style1 {
            width: 46px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="content-row1">
        <div class="create-new-order" style="width: 100%; margin: 4px;">
            <table>
                <tr>
                    <td align="left" style="padding-left: 4px;" class="auto-style1">Store :
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%" align="right">Search :
                    </td>

                    <td style="width: 10%">
                        <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                    </td>
                    <td style="width: 5%">
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return validation();" />
                    </td>
                    <td style="width: 5%; padding-right: 0px;">
                        <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="javascript:chkHeight();" />
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
                                                <img class="img-left" title="Bulk Image Upload" alt="Bulk Image Upload" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                <h2>Product List</h2>
                                            </div>
                                        </th>
                                    </tr>

                                    <tr>
                                        <td align="center" colspan="3">
                                            <div id="divGrid">
                                                <asp:GridView ID="grdProductColorSalesReport" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID"
                                                    BorderStyle="Solid" CellPadding="2" CellSpacing="1" GridLines="None" Width="99%"
                                                    PageSize="20" BorderColor="#E7E7E7" BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found."
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="grdProductColorSalesReport_PageIndexChanging" OnRowDataBound="grdProductColorSalesReport_RowDataBound">
                                                    <Columns>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Image
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblimgname" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"ImageName")%>'></asp:Label>
                                                                  <asp:Label ID="lblstoreid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"StoreID")%>'></asp:Label>
                                                                  <asp:Label ID="lblProductID" runat="Server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID")%>' Visible="False"></asp:Label>
                                                                <div style="float: left;">
                                                                        <a href="javascript:expandcollapse('divchild<%# Eval("ProductID") %>', 'one');">
                                                                            <img src="/images/expand.gif" id="imgdiv<%# Eval("ProductID") %>" alt="" border="0" /></a></div>

                                                                <img src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' />

                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Product Name
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <%-- <a href="javascript:void(0);" onclick="productquerystring(<%# DataBinder.Eval(Container.DataItem,"ProductID") %>,<%#  DataBinder.Eval(Container.DataItem,"StoreID") %>);"
                                                                    title='<%# Convert.ToString(Eval("Name")) %>'>--%>

                                                                <asp:Label ID="lblProductname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>

                                                                <%--</a>--%>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="25%" />
                                                        </asp:TemplateField>
                                                 
                                                         <asp:TemplateField>
                                                            <HeaderTemplate>
                                                               Quantity
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Inventory1")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="7%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                SKU
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="7%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                UPC
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUPC" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UPC")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Color Option
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblColors" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Colors")%>'></asp:Label>

                                                                <%--<asp:Label ID="lblCount" runat="server" Text='<%# GetIconImageCount(Convert.ToString(Eval("ProductID")),Convert.ToString(Eval("ImageName"))) %>'></asp:Label>--%>
                                                                <asp:Label ID="lblCount" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                View Detail
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%--  <a id="aviewclone<%# Container.DataItemIndex %>" href="javascript:void(0);" onclick="openCenteredCrossSaleWindowView(<%#Eval("StoreID") %>,<%#Eval("ProductID") %>);">View</a>--%>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" Visible="false">
                                                            <ItemTemplate>
                                                                <%-- <asp:Label ID="lblStoreID" runat="Server" Text='<%# Eval("StoreID") %>' Visible="False">--%>

                                                             
                                                                <%-- <asp:ImageButton id="btnedit" runat="Server" OnClientClick='openCenteredCrossSaleWindow("<%#  DataBinder.Eval("StoreID")%>","<%# Eval("ProductID") %>");return false;' />--%>
                                                                <%-- <a id="aeditclone<%# Container.DataItemIndex %>" href="javascript:void(0);" onclick="openCenteredCrossSaleWindow(<%#Eval("StoreID") %>,<%#Eval("ProductID") %>);">Edit</a>--%>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sale Order">
                                                            <ItemTemplate>
                                                                <a id="asaleorderid" runat="server" href="javascript:void(0);">Create Sale Order</a>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Quote">
                                                            <ItemTemplate>
                                                                <a id="aquoteid" runat="server" href="javascript:void(0);">Create Quote</a>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="15%" />
                                                        </asp:TemplateField>
                                                               <asp:TemplateField ItemStyle-Width="45%">
                                                            <ItemTemplate>
                                                               <tr>
                                                                <td colspan="100%">
                                                                      <div id="divchild<%# Eval("ProductID") %>" style="display: none; position: relative; left: 15px; overflow: auto; width: 97%; margin-top: 10px;">
                                                                    <asp:GridView ID="grdVariant" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID"
                                                                        BorderStyle="Solid" CellPadding="2" CellSpacing="1" GridLines="None" Width="99%"
                                                                        PageSize="20" BorderColor="#E7E7E7" BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found."
                                                                        EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                        PagerSettings-Mode="NumericFirstLast"  >
                                                                        <Columns>
                                                                             <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Name
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVariant" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VariantValue")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left"  CssClass="border" Width="7%" />
                                                        </asp:TemplateField>
                                                               <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                SKU
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblvariantsku" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="7%" />
                                                        </asp:TemplateField>
                                                            <asp:TemplateField>
                                                            <HeaderTemplate>
                                                               Quantity
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblvariantInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Inventory")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="7%" />
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
                                                            </ItemTemplate>
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
         <div style="display: none;">
        <input type="hidden" id="hdnrowid" runat="server" value="" />
        
    </div>
    </div>
</asp:Content>
