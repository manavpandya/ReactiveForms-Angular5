<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreCreditDetails.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.StoreCreditDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" class="table-none-border" style="border: none !important;">
            <tr id="trCredit" runat="server">
                <td align="left" style="padding: 10px;">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="left">
                                <b>Total Credit:&nbsp;<asp:Label ID="lbltotal" runat="server" CssClass="font-red"></asp:Label>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <b>Remaining Credit:&nbsp;<asp:Label ID="lblremaining" runat="server" CssClass="font-red"></asp:Label>
                                </b>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdRMARequestList" runat="server" CssClass="table-noneforOrder"
                        AutoGenerateColumns="False" BorderWidth="0" CellPadding="5" CellSpacing="5" Width="100%"
                        GridLines="None" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red"
                        OnRowDataBound="grdRMARequestList_RowDataBound">
                        <EmptyDataTemplate>
                            <span style="color: Red;">No Record(s) Found !</span>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    RMA No.
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRMANO" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ReturnItemID") %>'></asp:Label>
                                    <asp:Label ID="lbltotalamt" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"InitialAmount") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblbalance" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Balance") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Customer Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNewOrderNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Coupon Code
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CouponsCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Usage Credit($)
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%#Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Amount")), 2) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Usage Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNotes" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"usedDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
