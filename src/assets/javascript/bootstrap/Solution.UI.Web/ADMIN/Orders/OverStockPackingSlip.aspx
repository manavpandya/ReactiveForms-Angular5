<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OverStockPackingSlip.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.OverStockPackingSlip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            font-size: 12px;
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
            font-size: 12px;
        }
        .popup_cantain
        {
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            color: #4C4C4C;
            text-decoration: none;
        }
        .popup_cantain a
        {
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            color: #FE0000;
            text-decoration: none;
        }
        .popup_cantain a:hover
        {
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            color: #000;
            text-decoration: underline;
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
<body style="background: none !important; font-family: Arial, Helvetica, sans-serif;
    font-size: 11px; color: #2A2A2A;">
    <form id="form1" runat="server">
    <div>
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    </div>
    <div class="body12">
        <table width="100%" cellspacing="0" cellpadding="0" align="center" class="table">
            <tbody>
                <tr>
                    <td align="center" class="receiptfont" width="100%">
                        <div align="center" class="bkground123">
                            <table width="100%" cellspacing="0" cellpadding="0" align="center" class="popup_cantain">
                                <tbody>
                                    <tr>
                                        <td style="height: 14px">
                                        </td>
                                    </tr>
                                    <tr class="Printinvoice">
                                        <td valign="middle" align="right">
                                            <a title="Print Invoice" href="javascript:window.print();">
                                                <%--    <a title="Print Invoice" href="javascript:document.write(document.getElementById('form1').innerHTML);">--%>
                                                <img title="Print Invoice" alt="Print Invoice" src="/App_Themes/<%=Page.Theme %>/button/print.png" /></a>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr align="center" style="height: 85px">
                                        <td valign="middle" align="left" rowspan="1">
                                            <img alt="logo" border="0" runat="server" class="img_left" id="ImgStoreLogo" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" rowspan="1">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" class="receiptlineheight" style="height: 20px" rowspan="1">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="50%">
                                                        <span style="font-size: 16px; font-weight: bold;">Ship To :</span>
                                                    </td>
                                                    <td width="50%">
                                                        <span style="font-size: 16px; font-weight: bold;">Send To :</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="50%" align="left" valign="top">
                                                        <asp:Literal ID="ltrShiptoName" runat="server"></asp:Literal>
                                                    </td>
                                                    <td width="50%" align="left" valign="top">
                                                        <asp:Literal ID="ltrAddress" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellpadding="0" cellspacing="0" width="100%" class="datatable" style="border-collapse: collapse;">
                                                            <tr style="line-height: 50px; background-color: rgb(242,242,242);">
                                                                <th>
                                                                    Order Date
                                                                </th>
                                                                <th>
                                                                    Order Number
                                                                </th>
                                                                <th>
                                                                    Ship Via
                                                                </th>
                                                                <th>
                                                                    Ship Method
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <th>
                                                                    <b><span id="lblDate">
                                                                        <asp:Literal ID="ltrOrderdate" runat="server"></asp:Literal></span></b>
                                                                </th>
                                                                <th>
                                                                    <b><span id="lblOrderId">
                                                                        <asp:Literal ID="ltrorderNumber" runat="server"></asp:Literal></span></b>
                                                                </th>
                                                                <th>
                                                                    <b><span id="Span6">
                                                                        <asp:Literal ID="ltrshippedvia" runat="server"></asp:Literal></span></b>
                                                                </th>
                                                                <th>
                                                                    <b><span id="lblDeliveryMethod">
                                                                        <asp:Literal ID="ltrshippingMethod" runat="server"></asp:Literal></span></b>
                                                                </th>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Literal ID="ltrCart" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="popup_cantain">
                                        <td  align="left" valign="top">
                                            <asp:Literal ID="ltrOverStockInvoiceTopic" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr class="popup_cantain">
                                        <td align="left"  valign="top">
                                            <asp:Literal ID="ltrOverstockInstruction" runat="server"></asp:Literal>
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
        <asp:Button ID="btnInvoice" runat="server" />
        <input type="hidden" id="hdmemail" runat="server" value="" />
        <input type="hidden" id="hdntabid" runat="server" value="1" />
    </div>
    </form>
</body>
</html>
