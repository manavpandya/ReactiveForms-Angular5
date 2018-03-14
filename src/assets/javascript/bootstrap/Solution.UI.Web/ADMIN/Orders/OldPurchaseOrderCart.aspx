<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OldPurchaseOrderCart.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.OldPurchaseOrderCart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--  <script type="text/javascript">
        function PrintPOcart() {
            w = window.open('', 'Print', 'directories=no, location=no, menubar=no, status=no,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=5,Width=5,left=0,top=0,visible=false,alwaysLowered=yes');
            document.getElementById("imgback").style.display = 'none';
            document.getElementById("PrintPOcartid").style.display = 'none';
            w.document.write(document.getElementById("divOrder").innerHTML);
            w.document.close();
            window.print();
            document.getElementById("imgback").style.display = 'block';
            document.getElementById("PrintPOcartid").style.display = 'block';
            w.close();
        }
    </script>--%>
    <style type="text/css" media="print">
        .Printinvoice
        {
            display: none;
        }
    </style>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div id="divOrder" runat="server">
        <table width="90%" border="0" cellspacing="0" cellpadding="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px;" width="15%">
                      <%--  <a href="javascript:history.go(-1);" id="imgback" runat="server" class="Printinvoice">
                            <img src="/App_Themes/<%=Page.Theme %>/images/back.png" style="padding-top: 10px;
                                padding-left: 10px;" alt="Go to Purchase Order" title="Go to Purchase Order" />
                        </a>&nbsp;--%>
                    </td>
                    <td style="text-align: right; font-size: 11px; padding-bottom: 10px;" width="55%">
                        <a id="PrintPOcartid" runat="server" href="javascript:window.print();" style="color: blue;
                            text-decoration: underline; font-weight: bold" class="Printinvoice">
                            <img src="/App_Themes/<%=Page.Theme %>/images/print.png" alt="Print" title="Print" /></a>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="color: #4D4D4C; text-align: left;" colspan="2">
                        <asp:Label ID="lblMsg" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="color: #4D4D4C; text-align: left;" colspan="2">
                        <table cellspacing="1" cellpadding="2" width="90%" class="table_none">
                            <tr>
                                <td style="padding-bottom: 10px; padding-left: 10px">
                                    <asp:Literal ID="ltOrderNo" runat="server"></asp:Literal>
                                </td>
                                <td style="padding-bottom: 10px; padding-right: 10px;" align="right">
                                    <asp:Literal ID="ltDate" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-bottom: 10px; padding-left: 10px">
                                    <asp:Literal ID="ltOrderNumber" runat="server"></asp:Literal>&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px;" colspan="2">
                                    <asp:Literal ID="ltVendorName" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 10px;">
                                </td>
                            </tr>
                            <tr style="padding-top: 20px;">
                                <td style="padding-left: 10px;" colspan="2">
                                    <div style="float: left; width: 861px;">
                                        <asp:Literal ID="litProducts" runat="Server"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellspacing="1" cellpadding="2" width="90%">
                                        <tr id="trsubtotal" runat="server" visible="false">
                                            <td>
                                                <asp:Literal ID="ltSubtotal" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td style="width: 300px;">
                                                <asp:Literal ID="litShipToAddr" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <asp:Literal ID="ltPoStatus" runat="Server" Visible="false"></asp:Literal>
                                        <tr valign="top">
                                            <td>
                                                <asp:Literal ID="litNotes" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <asp:Literal ID="ltstatus" runat="Server" Visible="false"></asp:Literal>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="litAddCost" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="ltTax" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <asp:Literal ID="ltShipping" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="ltAdjustmetns" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lbPOAmount" runat="Server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
