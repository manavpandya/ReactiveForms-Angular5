<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="SalesAgentOwnSalesReport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.SalesAgentOwnSalesReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-row1">
        <div class="create-new-order" style="width:100%; margin:4px;">
            <table>
                <tr>
                    <td align="left" style="padding-left: 4px; ">
                        Store :
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table
                        class="content-table" style="width:100%;background-color:#FFFFFF;">
                        <tr>
                            <td class="border-td-sub">
                                <table style="width:100%;" class="add-product">
                                    <tr>
                                        <th colspan="3">
                                            <div class="main-title-left">
                                                <img class="img-left" title="Sales Agent own sales" alt="Sales Agent own sales" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                <h2>
                                                    Sales Agent own sales</h2>
                                            </div>
                                        </th>
                                    </tr>
                                   
                                    <tr>
                                        <td align="center" colspan="3">
                                            <div id="divGrid">
                                              <asp:GridView ID="grdSalesAgentOrders" runat="server" CellPadding="0" CssClass="dashboard-left"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" BorderStyle="Solid" BorderColor="#DFDFDF" CellSpacing="1"   GridLines="None"
                        EmptyDataText="No Order(s) Found." EditRowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="even-row"
                      EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;" OnPageIndexChanging="grdSalesAgentOrders_PageIndexChanging" OnRowDataBound="grdSalesAgentOrders_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Number">
                                <ItemTemplate>
                                    <div style="float:left;padding-left:4px;"><img src="/admin/images/<%# Eval("StoreID")%>.png"  /></div>
                                    <div style="float:left;padding-left:4px;"><a href="/Admin/Orders/Orders.aspx?id=<%# Eval("OrderNumber")%>" style="font-weight: normal;
                                        text-decoration: underline; font-size: 11px;">
                                        <%# Eval("OrderNumber")%></a></div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Date">
                                <ItemTemplate>
                                    <%# String.Format("{0:MM/dd/yyyy}", Eval("OrderDate"))%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <a href="/Admin/Customers/Customer.aspx?mode=edit&CustID=<%# Eval("CustomerID") %>"
                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" title="Click to Edit Customer">
                                        <%# Eval("CustomerName")%></a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# Eval("Email")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Total">
                                <ItemTemplate>
                                    <%# String.Format("${0:0.00}", Eval("OrderTotal"))%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Eval("OrderStatus")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
