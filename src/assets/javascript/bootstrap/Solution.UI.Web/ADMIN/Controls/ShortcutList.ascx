<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShortcutList.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.ShortcutList" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-right">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/shortcuts.png" alt="Shortcuts" title="Shortcuts"
                            class="img-left">
                        <h2>
                            Shortcuts</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgShortcut','trShortcut','tempDiv');">
                            <img class="minimize" id="imgShortcut" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trShortcut">
                <td>
                    <ul>
                        <li><a title="Order List" href="/Admin/Orders/OrderList.aspx">Order List</a></li>
                        <li><a title="Product List" href="/Admin/Products/ProductList.aspx?StoreID=<%=Convert.ToString(Solution.Bussines.Components.DashboardComponent.ControlStoreID) %>">
                            Product List</a></li>
                        <li><a title="Add Product" href="/Admin/Products/Product.aspx?StoreID=<%=Convert.ToString(Solution.Bussines.Components.DashboardComponent.ControlStoreID) %>">
                            Add Product</a></li>
                        <li><a title="Category List" href="/Admin/Products/CategoryList.aspx">Category List</a></li>
                        <li><a title="Add Category" href="/Admin/Products/Category.aspx">Add Category</a></li>
                        <li><a title="Customer List" href="/Admin/Customers/CustomerList.aspx">Customer List</a></li>
                        <li><a title="Add Customer" href="/Admin/Customers/Customer.aspx">Add Customer</a></li>
                        <li><a title="Topic List" href="/Admin/Content/TopicList.aspx">Topic List</a></li>
                        <li><a title="Store List" href="/Admin/Settings/StoreList.aspx">Store List</a></li>
                        <li><a title="Admin List" href="/Admin/Settings/AdminList.aspx">Admin List</a></li>
                        <li><a title="Dashboard Configuration" href="/Admin/Settings/DashboardSetting.aspx">
                            Dashboard Configuration</a></li>
                    </ul>
                </td>
            </tr>
        </tbody>
    </table>
</div>
