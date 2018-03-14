<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackingSlip.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.PackingSlip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Packing Slip</title>
    <style type="text/css">
        .datatable table
        {
            border: 1px solid #eeeeee;
        }
        .datatable tr.alter_row
        {
            background-color: #f9f9f9;
        }
        .datatable td
        {
            padding: 2px 2px;
            text-align: left;
            border: 1px solid #eeeeee;
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }
        .datatable th
        {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }
        .receiptfont
        {
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            color: #4c4c4c;
        }
        .receiptlineheight
        {
            height: 15px;
        }
        .Printinvoice
        {
        }
    </style>

    <style type="text/css" media="print">
        .Printinvoice
        {
            display: none;
        }
    </style>

    <script type="text/javascript">
        function SendInvoice() {
            window.parent.chkHeight();
            document.getElementById('hdmemail').value = window.parent.document.getElementById('ContentPlaceHolder1_ahrefMail').innerHTML;
            document.getElementById('hdntabid').value = window.parent.document.getElementById('ContentPlaceHolder1_hdnTabid').value;

            document.getElementById('btnInvoice').click();

        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div class="body12">
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
        <table width="60%" cellspacing="0" cellpadding="0" align="center" class="table">
            <tbody>
                <tr>
                    <td align="center" class="receiptfont" width="100%">
                        <div align="center" class="bkground123">
                            <table width="100%" cellspacing="0" cellpadding="0" align="center" class="signup-row">
                                <tbody>
                                    <tr>
                                        <td style="height: 14px" colspan="2">
                                        </td>
                                    </tr>
                                    <tr class="Printinvoice">
                                        <td valign="middle" align="right" colspan="2">
                                            <a title="Print Packing Slip" href="javascript:window.print();">
                                                <img title="Print Packing Slip" alt="Print Packing Slip" src="/App_Themes/<%=Page.Theme %>/button/print.png" /></a>
                                        </td>
                                    </tr>
                                    <tr align="center" style="height: 85px">
                                        <td valign="middle" align="center" rowspan="1" colspan="2">
                                            <img alt="logo" border="0" runat="server" class="img_left" id="ImgStoreLogo" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center" rowspan="1" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" class="receiptlineheight" style="height: 22px" rowspan="1"
                                            colspan="2">
                                            Ship To:<br />
                                            <b><span id="lblName">
                                                <asp:Literal ID="ltrName" runat="server"></asp:Literal>
                                            </span>
                                                <asp:Literal ID="ltrShippingAddress" runat="server">
                                                </asp:Literal>
                                            </b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center" style="height: 20px" rowspan="1" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" style="height: 20px" rowspan="1" colspan="2">
                                            -----------------------------------------------------------------------------------------------------------------------------------------
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td valign="middle" align="left" style="height: 20px">
                                                        Order Number:&nbsp; <b><span id="lblOrderId">
                                                            <asp:Literal ID="ltrorderNumber" runat="server"></asp:Literal></span></b>
                                                    </td>
                                                </tr>
                                                <tr id="trRefOrderNo" runat="server" visible="false">
                                                    <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                                        <asp:Literal ID="ltrstore" runat="server" Text="StoreName"></asp:Literal>
                                                        : <b><span id="lblRef">
                                                            <asp:Literal ID="ltrRef" runat="server"></asp:Literal></span></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                                        Order Date:&nbsp; <b><span id="lblDate">
                                                            <asp:Literal ID="ltrOrderdate" runat="server"></asp:Literal></span></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                                        <span id="lblDelMethod">Shipping Method:</span> <b><span id="lblDeliveryMethod">
                                                            <asp:Literal ID="ltrShippingMethod" runat="server"></asp:Literal></span></b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <img alt="" id="imgOrderBarcode" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" style="height: 20px" rowspan="1" colspan="2">
                                            Thank you for buying from
                                            <%=Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreName").ToString() %>.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" rowspan="1" colspan="2">
                                            <asp:Literal ID="ltrAddress" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <span class="error" id="lblMsg"></span>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="height: 20px">
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="left" style="height: 19px">
                        <table width="100%" cellspacing="0" cellpadding="0">
                            <tbody>
                                <tr>
                                    <td style="height: 19px">
                                        <asp:Literal ID="ltrCart" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="5" class="font-black04">
                                        <asp:Literal ID="ltInvoiceSignature" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="Printinvoice">
                    <td valign="middle" align="right">
                        <a title="Print Packing Slip" href="javascript:window.print();">
                            <img title="Print Packing Slip" alt="Print Packing Slip" src="/App_Themes/<%=Page.Theme %>/button/print.png" /></a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnInvoice" runat="server" OnClick="btnInvoice_Click" />
        <input type="hidden" id="hdmemail" runat="server" value="" />
        <input type="hidden" id="hdntabid" runat="server" value="1" />
    </div>
    </form>
</body>
</html>
