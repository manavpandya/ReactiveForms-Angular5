<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="BulkImageUpload.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.BulkImageUpload" %>
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
       function openCenteredCrossSaleWindow(StoreID,ProductID) {
           
            var width = 1000;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
          
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('UpdateProductImage.aspx?StoreID=' + StoreID +'&Mode=edit&ProductID=' + ProductID + "&subWind", "Mywindow", windowFeatures);
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
     <style type="text/css">
         .auto-style1 {
             width: 46px;
         }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="content-row1">
        <div class="create-new-order" style="width:100%; margin:4px;">
            <table>
                <tr>
                    <td align="left" style="padding-left: 4px; " class="auto-style1">
                        Store :
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                     <td style="width: 10%" align="right">
                                                            Search :
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
                                                <h2>
                                                    Bulk Image Upload</h2>
                                            </div>
                                        </th>
                                    </tr>
                                   
                                    <tr>
                                        <td align="center" colspan="3">
                                            <div id="divGrid">
                                                <asp:GridView ID="grdProductColorSalesReport" runat="server" AutoGenerateColumns="False"
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
                                                           
                                                                    <a  target="_blank" href="/">
                              
       
                                                             <img   src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' />
                                                                </a>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Product Name
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="productquerystring(<%# DataBinder.Eval(Container.DataItem,"ProductID") %>,<%#  DataBinder.Eval(Container.DataItem,"StoreID") %>);"
                                                                    title='<%# Convert.ToString(Eval("Name")) %>'>
                                                                    <asp:Label ID="lblProductname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label></a>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="25%" />
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
                                                             Image  Count
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCount" runat="server" Text='<%# GetIconImageCount(Convert.ToString(Eval("ProductID")),Convert.ToString(Eval("ImageName"))) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="4%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                View Detail
                                                            </HeaderTemplate>
                                                             <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                 <a id="aviewclone<%# Container.DataItemIndex %>" href="javascript:void(0);" onclick="openCenteredCrossSaleWindowView(<%#Eval("StoreID") %>,<%#Eval("ProductID") %>);">View</a>
                                                            </ItemTemplate>
                                                             <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreID" runat="Server" Text='<%# Eval("StoreID") %>' Visible="False">

                                                                </asp:Label><asp:Label ID="lblProductID" runat="Server" Text='<%# Eval("ProductID") %>' Visible="False"></asp:Label>
                                                               <%-- <asp:ImageButton id="btnedit" runat="Server" OnClientClick='openCenteredCrossSaleWindow("<%#  DataBinder.Eval("StoreID")%>","<%# Eval("ProductID") %>");return false;' />--%>
                                                                <a id="aeditclone<%# Container.DataItemIndex %>" href="javascript:void(0);" onclick="openCenteredCrossSaleWindow(<%#Eval("StoreID") %>,<%#Eval("ProductID") %>);">Edit</a>
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
