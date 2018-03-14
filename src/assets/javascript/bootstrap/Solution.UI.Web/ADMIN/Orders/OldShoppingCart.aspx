<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OldShoppingCart.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.OldShoppingCart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css" media="all">
        .table-none-border
        {
            border: 1px solid #ececec;
        }
        
        .table-noneforOrder
        {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
            border-collapse: collapse;
            padding: 10px 0 0 0;
            font-size: 11px;
            color: #212121;
            border: 1px solid #ececec;
        }
        .table-noneforOrder td
        {
            padding: 5px 8px;
            background: #fff;
            font-size: 11px;
            color: #212121;
            line-height: 18px;
            max-height: 30px;
            border: 1px solid #DFDFDF;
        }
        .table-noneforOrder th
        {
            background: #f6f5f5;
            font-size: 11px;
            padding: 5px 8px;
            border-bottom: none;
            border: 1px solid #DFDFDF;
            color: #212121;
            font-weight: bold; /* text-align: left;*/
        }
        .table-noneforOrder-border
        {
            border: 1px solid #DFDFDF;
        }
        .table-noneforOrder-border td
        {
            padding: 5px 8px;
            background: #fff;
            font-size: 11px;
            color: #212121;
            line-height: 18px;
            max-height: 30px;
        }
        .table-noneforOrder a
        {
            color: #FE0000;
            text-decoration: none;
        }
        .table-noneforOrder a:hover
        {
            color: #212121;
            text-decoration: underline;
        }
        .table-noneforOrder td ul
        {
            float: left;
            list-style: none;
        }
    </style>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"> </asp:Label>
    <div class="table_border" id="divOrderList" runat="server">
        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
            CellPadding="0" CellSpacing="1" CssClass="table-noneforOrder" GridLines="None"
            Width="100%" AllowPaging="false">
            <EmptyDataTemplate>
                <center>
                    <span style="color: Red; font-weight: bold;">No Record Found !</span></center>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Number
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="25%" />
                    <ItemTemplate>
                        <a href='<%# "OldShoppingCart.aspx?Backup_Id=" + DataBinder.Eval(Container.DataItem,"OrderedShoppingCart_backup_Id").ToString()+"&ROno="+ DataBinder.Eval(Container.DataItem, "OrderNumber").ToString() + "-" +DataBinder.Eval(Container.DataItem, "RepeatOrderNo").ToString() %>'>
                            <%# DataBinder.Eval(Container.DataItem, "OrderNumber").ToString() + "-" + DataBinder.Eval(Container.DataItem, "RepeatOrderNo").ToString()%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Changed On
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="30%" />
                    <ItemTemplate>
                        <%#  DataBinder.Eval(Container.DataItem,"CreatedOn").ToString() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Original Order Number
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="30%" />
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Total($)
                    </HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="15%" />
                    <ItemTemplate>
                        <%#  Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Total")).ToString("f2") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div id="divOrder" runat="server">
        <br />
        <asp:HiddenField ID="hfCustomer" runat="server" />
        <asp:HiddenField ID="hfGateway" runat="server" />
        <table class="table-noneforOrder" style="font-size: 12px;" cellspacing="1" cellpadding="2"
            width="700px">
            <tr>
                <th align="left" style="border-right: 1px solid #F6F5F5;">
                    <a href="javascript:history.go(-1);">Back</a>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Literal ID="ltOrderNo" runat="server"></asp:Literal>
                </th>
                <th style="text-align: right; font-size: 12px;">
                    <asp:Literal ID="ltDate" runat="server"></asp:Literal>
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="float: left;">
                        <asp:Literal ID="litProducts" runat="Server"></asp:Literal>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    SubTotal
                </td>
                <td style="text-align: right;">
                    &nbsp;<b>$<asp:Literal ID="litOrgSubTotal" runat="Server"></asp:Literal></b>
                </td>
            </tr>
            <tr>
                <td>
                    Delivery Method
                </td>
                <td style="text-align: right;">
                    &nbsp;<asp:Literal ID="litOrgShippingMethod" runat="Server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    Discount
                </td>
                <td style="text-align: right;">
                    $<asp:Literal ID="litOrgDiscount" runat="Server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    Other Discount
                </td>
                <td style="text-align: right;">
                    &nbsp;$<asp:Literal ID="litOrgOtherDiscount" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    Tax
                </td>
                <td style="text-align: right;">
                    $<asp:Literal ID="litOrgTax" runat="Server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    Shipping Cost
                </td>
                <td style="text-align: right;">
                    &nbsp;$<asp:Literal ID="litOrgShippingCost" runat="Server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    Total
                </td>
                <td style="text-align: right;">
                    &nbsp;<b>$<asp:Literal ID="litOrgTotal" runat="Server"></asp:Literal></b>&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
