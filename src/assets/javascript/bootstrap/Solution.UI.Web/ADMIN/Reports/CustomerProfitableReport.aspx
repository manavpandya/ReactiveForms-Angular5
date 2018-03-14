<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerProfitableReport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.CustomerProfitableReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order" style="width: 100%; margin-top: 4px">
            <table>
                <tr>
                    <td align="left" style="padding-left: 0px;">
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
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th colspan="3">
                                            <div class="main-title-left">
                                                <img class="img-left" title="Profitable Customers" alt="Profitable Customers" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                <h2>
                                                    Profitable Customers</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <div id="divGrid">
                                                <asp:GridView ID="grdCustomerProfitableReport" runat="server" AutoGenerateColumns="False"
                                                    BorderStyle="Solid" CellPadding="2" CellSpacing="1" GridLines="None" Width="99%"
                                                    PageSize="20" BorderColor="#E7E7E7" BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found."
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="grdCustomerProfitableReport_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr No.">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>.
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Customer Name
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <a href="/Admin/Customers/Customer.aspx?mode=edit&CustID=<%# Eval("CustomerID") %>">
                                                                    <asp:Label ID="lblcutomername" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label></a>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Email
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" Width="40%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Total Profit
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProfit" runat="server" Text='<%# String.Format ("{0:C}",Convert.ToDecimal( DataBinder.Eval(Container.DataItem,"TotalProfit")))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" Width="20%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    <FooterStyle ForeColor="White" Font-Bold="true" BackColor="#545454" />
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
