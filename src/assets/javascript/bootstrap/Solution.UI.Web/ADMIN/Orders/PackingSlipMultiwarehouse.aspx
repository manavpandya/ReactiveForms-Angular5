<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackingSlipMultiwarehouse.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.PackingSlipMultiwarehouse" %>

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
    
</head>
<body style="background: none !important; font-family: Arial, Helvetica, sans-serif;
    font-size: 11px; color: #2A2A2A;">
    <form id="form1" runat="server">
    <div>
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    </div>
    <div style="float:right;padding-right:10px" class="Printinvoice">
      <a title="Print Packing Slip" href="javascript:window.print();">
                                                <img title="Print Packing Slip" alt="Print Packing Slip" src="/App_Themes/<%=Page.Theme %>/button/print.png" /></a>
    </div>
    <div class="body12">
       <asp:Literal ID="ltMultiwarehouse" runat="server"></asp:Literal>
    </div>
     <div style="float:right;padding-right:10px" class="Printinvoice">
       <a title="Print Packing Slip" href="javascript:window.print();">
                                                <img title="Print Packing Slip" alt="Print Packing Slip" src="/App_Themes/<%=Page.Theme %>/button/print.png" /></a>
     </div>
    </form>
</body>
</html>
