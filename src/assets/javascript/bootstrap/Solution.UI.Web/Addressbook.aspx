<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Addressbook.aspx.cs" Inherits="Solution.UI.Web.Addressbook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a>> <a href="/myaccount.aspx" title="My Account">
            My Account</a> > <span>Address Book</span></div>
    <div class="content-main">
        <div class="static-title">
            <table style="width: 100%;">
                <tr>
                    <td style="float: left; width: 94%;">
                        <span>Address Book </span>
                    </td>
                    <td style="float: right; width: 5%;">
                        <span><a href="MyAccount.aspx" style="color: #B92127; text-decoration: underline;"
                            title="BACK"><img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a> </span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td valign="top">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table">
                                                    <tbody>
                                                        <tr>
                                                            <th align="left">
                                                                Billing Address
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <%--<a title="New Billing Address" href="#">New Billing Address</a>--%>
                                                                <a href="/EditAddress.aspx?addtype=billingadd&type=new" style="text-decoration: underline;">
                                                                    Add New Billing Address</a>
                                                            </td>
                                                        </tr>
                                                        <asp:Literal ID="ltBilling" runat="server"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table">
                                                    <tbody>
                                                        <tr>
                                                            <th align="left">
                                                                Shipping Address
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <%--  <a title="New Shipping Address" href="#">New Shipping Address</a>--%>
                                                                <a href="/EditAddress.aspx?addtype=shippingadd&type=new" style="text-decoration: underline;">
                                                                    Add New Shipping Address</a>
                                                            </td>
                                                        </tr>
                                                        <asp:Literal ID="ltShipping" runat="server"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
   
</asp:Content>
