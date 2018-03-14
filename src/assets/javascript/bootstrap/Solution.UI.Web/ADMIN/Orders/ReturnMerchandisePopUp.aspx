<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnMerchandisePopUp.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.ReturnMerchandisePopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="content-table border-td"
           style="padding: 2px;">
            <tbody>
               <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Return Item Details
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td valign="top" style="color: #4D4D4C; text-align: left;" colspan="2">
                        <table width="100%" cellspacing="2" cellpadding="2" border="0" style="line-height: 20px;"
                            class="table_none">
                            <tbody>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px; padding-top: 10px;">
                                        <b>Order Number :</b>
                                    </td>
                                    <td style="padding-top: 10px;">
                                        <asp:Label runat="server" ID="lblOrderNum"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <b>Customer Name :</b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblCustomerName"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <b>E-Mail :</b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblEmail"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <b>Invoice Date :</b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblInvoiceDate"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <b>Product Name :</b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblProductName"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <b>SKU :</b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblMerchandiseCode"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <b>Quantity : </b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblQuantity"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <b>Return Reason :</b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblReturnResult"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <b>Any Additional Information :</b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblAnyAvailableInfo"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
