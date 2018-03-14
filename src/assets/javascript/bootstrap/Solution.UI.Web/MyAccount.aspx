<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MyAccount.aspx.cs" Inherits="Solution.UI.Web.MyAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <a href="/" title="Home">Home </a>> <span>
            <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></div>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <div style="padding-bottom: 5px; text-align: center;">
                </div>
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="margin-bottom: 10px;"
                    class="table_none">
                    <tbody>
                        <tr>
                            <td width="150" style="padding: 10px;">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="table_none">
                                    <tbody>
                                        <tr>
                                            <th>
                                                Orders
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                See &amp; Modify Orders
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left: 85px;">
                                                <img src="/images/order.png" title="" alt="">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td width="300" valign="top">
                                <strong>Purchase History</strong><br />
                                <a href="/ViewOldOrders.aspx">View Older Orders</a>
                                <br />
                                <a href="/ViewRecentOrders.aspx">View Recent and Open Orders</a>
                            </td>
                            <td valign="top">
                            <%--    <strong>More Order Actions</strong><br />
                                <a href="/Wishlist.aspx">Wish List Items</a>
                                <br />--%>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin-bottom: 10px;"
                    class="table_none">
                    <tbody>
                        <tr>
                            <td width="150" style="padding: 10px;">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="table_none">
                                    <tbody>
                                        <tr>
                                            <th>
                                                Payment
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                Credit Cards &amp; Gift Cards
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left: 85px;">
                                                <img src="/images/payment.png" title="" alt="">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td width="300" valign="top">
                                <strong>Payment Options</strong><br />
                                <a href="/creditcarddetail.aspx?Type=1">Manage Payment Methods</a><br />
                                <a href="/creditcarddetail.aspx?Type=0">Add a Credit Card</a>
                            </td>
                            <td valign="top">
                                <%--  <strong>Gift Cards &amp; Gift Registry</strong><br />
                                <a href="#">View Gift Certificate/ Card Balance</a>
                                <br />
                                <a id="ctl00_ContentPlaceHolder1_A1" href="#">Purchase a Gift Card</a>
                                <br />--%>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin-bottom: 10px;"
                    class="table_none">
                    <tbody>
                        <tr>
                            <td width="150" style="padding: 10px;">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="table_none">
                                    <tbody>
                                        <tr>
                                            <th>
                                                Settings
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                Password &amp; E-Mail Settings
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left: 85px;">
                                                <img src="/images/setting.png" title="" alt="">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td width="300" valign="top">
                                <strong>Account Settings</strong><br />
                                <a href="/ChangePassword.aspx">Change Password</a>
                                <br />
                                <a href="/OneClickSettings.aspx">1-Click Settings</a>
                                <br />
                                <a style="display: none" href="javascript:void(0);">Forgot Password</a>
                                <br />
                            </td>
                            <td valign="top">
                                <strong>Address Book</strong><br />
                                <a href="/Addressbook.aspx">Manage Address Book</a>
                                <br />
                                <a href="/EditAddress.aspx?addtype=billingadd&type=new">Add New Billing Address</a>
                                <br />
                                <a href="/EditAddress.aspx?addtype=shippingadd&type=new">Add New Shipping Address</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <p style="display: none">
                    <strong>Order Points : 0</strong>
                </p>
            </div>
        </div>
    </div>
</asp:Content>
