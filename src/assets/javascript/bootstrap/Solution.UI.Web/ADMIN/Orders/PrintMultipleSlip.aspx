<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintMultipleSlip.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.PrintMultipleSlip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript">
        function PrintFun() {
            document.getElementById('imgPrint').setAttribute("class", "printBtn");
            document.getElementById('imgPrintTop').setAttribute("class", "printBtn");
            window.print();
        }
    </script>
    <style type="text/css">
        .printBtn
        {
            display: none;
        }
        
        .shopping_display table
        {
            border: 1px solid #f0f0f0;
            border-collapse: collapse;
            color: #5a5d61;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
        }
        .shopping_display table.list
        {
            border: 1px solid #f0f0f0;
            border-collapse: collapse;
            color: #5a5d61;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
        }
        .shopping_display table.list td
        {
            padding: 5px 3px 5px 8px;
            border: 1px solid #f0f0f0;
        }
        .shopping_display table.list th
        {
            background-color: #DDE0E5;
            border: 1px solid #f0f0f0;
            color: #384557;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 11px;
            font-weight: bold;
            padding: 5px 8px;
        }
        .shopping_display table.list td span.error
        {
            color: #f00;
        }
        .shopping_display table.list td span
        {
            color: #5a5d61;
            font-size: 10px;
        }
        .shopping_display table.list td a
        {
            color: #5a5d61;
            text-decoration: none;
        }
        .shopping_display table.list td a.view
        {
            color: #b7180d;
            text-decoration: underline;
        }
        .shopping_display table.list td a.view:hover
        {
            color: #b7180d;
            text-decoration: none;
        }
        .shopping_display table.list tr.altrow
        {
            background: #fafafa url(images/form_altrow_bg5.gif) repeat-x left bottom;
        }
        .shopping_display table.list tr.oddrow
        {
            background: #fff;
        }
        .shopping_display table.list tr.title
        {
            background: #9a9999;
            font-size: 11px;
            font-weight: bold;
            color: #fff;
            text-transform: uppercase;
        }
        .shopping_display table.list tr.title td
        {
            padding-left: 8px;
        }
        .shopping_display table.list input.textfield
        {
            border: 1px solid #cdd3d9;
            background: #fff;
            font-size: 12px;
            padding: 1px;
            height: 18px;
            color: #5a5d61;
        }
        .shopping_display table.list input.qty
        {
            border: 1px solid #cdd3d9;
            width: 20px;
            text-align: center;
            background: #fff;
            font-size: 12px;
            padding: 1px;
            height: 18px;
            color: #5a5d61;
        }
        .shopping_display table.list textarea.textarea
        {
            border: 1px solid #cdd3d9;
            background: #fff;
            font-size: 12px;
            line-height: 15px;
            padding: 1px;
            color: #5a5d61;
            font-family: Arial, Helvetica, sans-serif;
        }
    </style>
    <style type="text/css" media="print">
        .form
        {
            page-break-after: always;
        }
    </style>
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
            font-family:Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }
        .datatable th
        {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
               font-size: 11px;
            font-family:Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }
        .receiptfont
        {
             font-size: 11px;
            font-family:Arial, Helvetica, sans-serif;
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
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div style="height: 25px; padding-top: 5px; padding-right: 10px;">
        <asp:ImageButton ID="imgPrintTop" Style="float: right;" runat="server" AlternateText="Print"
            ToolTip="Print" OnClientClick="PrintFun();return true;" BorderStyle="None" OnClick="btnPrint_Click" />
    </div>
    <div class="body12">
        <asp:Literal ID="litSlip" runat="server"></asp:Literal>
    </div>
    <asp:ImageButton ID="imgPrint" Style="float: right; padding: 10px;" runat="server"
        AlternateText="Print" ToolTip="Print" OnClientClick="PrintFun();return true;"
        BorderStyle="None" OnClick="btnPrint_Click" />
    </form>
</body>
</html>
