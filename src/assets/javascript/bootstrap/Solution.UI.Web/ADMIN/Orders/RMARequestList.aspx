<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMARequestList.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.RMARequestList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RMA List</title>
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
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" class="table-none-border" style="border: none !important;">
            <tr id="trRMATitle" runat="server">
                <td align="left" style="padding-bottom: 10px;">
                    Order #: <strong style="color: #4C4C4C;">
                        <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
                    </strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdRMARequestList" runat="server" CssClass="table-noneforOrder"
                        AutoGenerateColumns="False" BorderWidth="0" CellPadding="5" CellSpacing="5" Width="100%"
                        GridLines="None" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red"
                        OnRowDataBound="grdRMARequestList_RowDataBound">
                        <Columns>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblReturnItemID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"returnid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    RMA No.
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <a style="text-decoration: underline;cursor:pointer;" onclick='<%# "javascript:window.open(\"ReturnMerchandisePopUp.aspx?ID=" + DataBinder.Eval(Container.DataItem,"returnid")  +"\", \"\",\"height=600,width=820,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");" %>'>
                                        <asp:Label ID="lblRMAID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RMANo") %>'></asp:Label>
                                    </a>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    OrderNumber
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdOrderNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"OrderedNumber") %>' />
                                    <asp:HiddenField ID="hdReturnType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ReturnType") %>' />
                                    <asp:HiddenField ID="hdStoreName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"SToreName") %>' />
                                    <asp:HiddenField ID="hdnisReturnrequest" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"isReturnrequest") %>' />
                                    <a runat="server" target="_parent" id="lkOrderNumber" style="text-decoration: none">
                                        <asp:Label ID="OrderNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderedNumber") %>'></asp:Label>
                                    </a>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    CustomerName
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    OrderDate
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderDate","{0:dd MMM yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Product Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPro" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Quantity
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblquantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                              <asp:TemplateField>
                                <HeaderTemplate>
                                   Reason
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReaseon" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ReturnReason") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    ReturnStatus
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsReturn"))) %>'>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Status"))) %>'>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="creatermalinktr" runat="server">
                <td>
                    Create New RMA: <a href="javascript:void(0);" style="color:#D5321C;" target="_blank" id="creatermalink"
                        runat="server">Click Here</a>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
