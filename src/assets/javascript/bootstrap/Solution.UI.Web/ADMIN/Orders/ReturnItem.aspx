<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnItem.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.ReturnItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Return Items</title>
    <script runat="server">
        string _ReturnUrl;
        public string SetImage(bool _Value)
        {
            if (_Value == true)
            {
                _ReturnUrl = "../Images/isActive.png";
            }
            else
            {
                _ReturnUrl = "../Images/isInactive.png";
            }
            return _ReturnUrl;
        }        
    </script>
    <script type="text/javascript">
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
        function SearchNewOrder(ono) {
            if (ono == "0")
                return;
            window.parent.location.href = '/admin/Orders/Orders.aspx?id=' + ono;
        }
    </script>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" class="table-none-border" style="border: none !important;">
            <tr id="trRMATitle" runat="server">
                <td align="left" style="padding-bottom: 10px; font-weight: bold;">
                    Return Items
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdReturnItemList" runat="server" CssClass="table-noneforOrder"
                        AutoGenerateColumns="False" BorderWidth="0" CellPadding="5" CellSpacing="5" Width="100%"
                        GridLines="None" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red"
                        OnRowDataBound="grdReturnItemList_RowDataBound">
                        <EmptyDataTemplate>
                            <span style="color: Red; font-size: 12px;">No Record(s) Found !</span>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    RMA NO.
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRMANO" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ReturnItemID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    New Order Number
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <a href='javascript:void(0);' onclick='javascript:SearchNewOrder(<%#DataBinder.Eval(Container.DataItem,"NewOrderNumber") %>)'>
                                        <asp:Label ID="lblNewOrderNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"NewOrderNumber") %>'></asp:Label>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Product
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PName") %>'></asp:Label>
                                    <asp:Label ID="lblImage" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ReturnImages") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Return Reason
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ReturnReason") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Notes from warehouse
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNotes" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ReturnNotes") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdReturnType" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ReturnType") %>' />
                                    <asp:HiddenField ID="hdIsReturn" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"IsReturn") %>' />
                                    <div id="divStatus" runat="server">
                                        <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsReturn"))) %>'>
                                    </div>
                                    <div id="divChangeStatus" runat="server">
                                        <a href='javascript:void(0);' id="aChangeItem" runat="server">Change Item </a>
                                    </div>
                                    <input type="hidden" id="hdnreturnid" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"ReturnItemID") %>' />
                                    <input type="hidden" id="hdnproductid" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                    <div id="divrefund" runat="server" visible="false">
                                    </div>
                                    <div id="divstore" runat="server" visible="false">
                                    </div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                <HeaderStyle HorizontalAlign="Center" />
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
