<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="OrderIncompleteList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.OrderIncompleteList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function SearchValidation() {
            if (document.getElementById('<%=txtSearch.ClientID%>') != null && document.getElementById('<%= txtSearch.ClientID%>').value == '') {
               jAlert('Please enter search value.', 'Message', '<%=txtSearch.ClientID%>');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="content-width">

        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/Images/customer-list-icon.png" />
                                                    <h2>Incomplete Order List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">

                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="padding-right: 0px; width: 80%" align="right">
                                                            <table>
                                                                <tr>
                                                                    <td valign="top" style="text-align: left">Search by Order Number &nbsp; : &nbsp; <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"
                                                                        Width="200px"></asp:TextBox><br />
                                                                        <%--<span style="padding-left: 50px;">(eg. Quote Number, Name, Email) </span>--%>
                                                                    </td>
                                                                    <td valign="top" align="right" style="padding-right: 0px;">
                                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation();" />
                                                                        <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                                    </td>
                                                                </tr>

                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdIncompleteOrder" runat="server" AutoGenerateColumns="False"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%" OnRowCommand="grdIncompleteOrder_RowCommand"
                                                    GridLines="None" AllowPaging="false" PageSize="<%$ appSettings:GridPageSize %>"
                                                    PagerSettings-Mode="NumericFirstLast"
                                                    CellPadding="2" CellSpacing="1"
                                                    BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustomerID") %>'></asp:Label>
                                                                <asp:Label ID="lblStoreId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"StoreId") %>'></asp:Label>
                                                                <asp:Label ID="lblOrderno" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Order Date
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblorderdate" runat="server" Text='<%# Eval("orderdate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                          <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Order Number
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderno1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                       

                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Customer Name
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>


                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Customer Email
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblemail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>



                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                Order Total($)
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderTotal" runat="server" Text='<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"OrderTotal")))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>



                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Created By
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>


                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" BackColor=" #E7E7E7" />
                                                            <HeaderTemplate>
                                                                Continue Order
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%--<asp:HyperLink ID="acontinueorder"  href='<%# String.Format("/admin/orders/Phoneorder.aspx?Ono={0}&CustID={1}",DataBinder.Eval(Container.DataItem,"OrderNumber"),DataBinder.Eval(Container.DataItem,"CustomerID"))%>' runat="server">Continue Order


                                                                </asp:HyperLink>--%>

                                                                <asp:ImageButton ID="btncontinueorder" AlternateText="Continue Order" runat="server" CommandName="Add" CommandArgument='<%# Eval("ShoppingCardID") %>' ForeColor="#B92127" />

                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="altrow" runat="server" id="trBottom">
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
