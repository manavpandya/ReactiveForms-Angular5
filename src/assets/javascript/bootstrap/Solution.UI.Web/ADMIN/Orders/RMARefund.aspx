<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMARefund.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.RMARefund" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RMA Refund</title>
    <script language="javascript" type="text/javascript">
        function RefundProduct(ProductID) {
            document.getElementById('hdnRefundProductID').value = ProductID
        }
        function RefundProductClick(ProductID, revalue) {
            document.getElementById("refundamaut").value = revalue;
            document.getElementById('hdnRefundProductID').value = ProductID
            document.getElementById('BtnRefundProduct').click();
        }

        function RefundProduct_Validation() {
            var allElts = document.getElementById("pnlRefund").getElementsByTagName("input");
            var i, cnt = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox" && elt.checked == true) {
                    cnt = cnt + 1;
                }
            }
            if (cnt == 0) {
                alert('Please Select Authorized Refund Product.');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
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
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }
        .datatable th
        {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }
        .receiptfont
        {
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            color: #4c4c4c;
        }
        .receiptlineheight
        {
            height: 15px;
        }
        .popup_cantain
        {
            font: 11px/14px Verdana,Arial,Helvetica,sans-serif;
            color: #4C4C4C;
            text-decoration: none;
        }
        .popup_cantain a
        {
            font: 11px/14px Verdana,Arial,Helvetica,sans-serif;
            color: #FE0000;
            text-decoration: none;
        }
        .popup_cantain a:hover
        {
            font: 11px/14px Verdana,Arial,Helvetica,sans-serif;
            color: #000;
            text-decoration: underline;
        }
        .Printinvoice
        {
        }
    </style>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <asp:Panel ID="pnlRefund" runat="server">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <center>
                        <asp:Label ID="lblMsgRefund" runat="server" Font-Bold="true" ForeColor="Red" CssClass="error"></asp:Label></center>
                </td>
            </tr>
            <tr>
                <td style="padding: 2px; padding-top: 10px; text-align: center;">
                    <asp:Literal ID="ltCartRefund" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr style="display: none;">
                <td align="right">
                    <asp:ImageButton ID="BtnRefundProduct" runat="server" ImageUrl="../images/Refund.gif"
                        OnClick="BtnRefundProduct_Click" />
                    <input type="hidden" id="refundamaut" runat="server" value="0" />
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 5px;">
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnRefundProductID" runat="server" />
        <asp:Button ID="btnRefundProductClick" Visible="false" runat="server" />
    </asp:Panel>
    </form>
</body>
</html>
