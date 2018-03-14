<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderLog.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.OrderLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order Log</title>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" class="table-none-border" style="border: none !important;">
            <tr>
                <td align="left" style="padding-bottom:10px;">
                    <%--Order #: <strong style="color: #4C4C4C;">
                        <%=Request.QueryString["ONo"]%></strong>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdOrderLog" runat="server" CssClass="table-noneforOrder" AutoGenerateColumns="False"
                        BorderWidth="0" CellPadding="5" CellSpacing="5" Width="100%" GridLines="None"
                        EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red"
                        EmptyDataText="No Order Log(s) Found.">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Created By
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#Eval("AdminName") %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#Eval("Status") %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                     <%#Eval("Description") %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Created Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#Eval("LogDate") %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
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
