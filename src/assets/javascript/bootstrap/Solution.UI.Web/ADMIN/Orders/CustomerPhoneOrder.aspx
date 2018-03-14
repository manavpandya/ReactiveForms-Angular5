<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerPhoneOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.CustomerPhoneOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
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
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 2px;" cellpadding="3" cellspacing="3">
                        <tr>
                            <td style="padding-left: 0px; width: 30%" valign="top" align="left">
                                Store : &nbsp;
                                <asp:DropDownList ID="ddlStore" runat="server" Width="200px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/orders/PhoneOrder.aspx">
                    <img alt="Add PhoneOrder" title="Add PhoneOrder" src="/App_Themes/<%=Page.Theme %>/images/create-new-order.gif" /></a></span>
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
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Phone Orders" alt="Phone Orders" src="/App_Themes/<%=Page.Theme %>/Images/Phoneorder.png" />
                                                    <h2>
                                                        Phone Orders</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <%-- <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.CustomerComponent" SelectMethod="GetDataByFilter"
                                                    DeleteMethod="DeleteCustomerList" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
                                                    SortParameterName="sortBy" SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                    </SelectParameters>
                                                    <DeleteParameters>
                                                        <asp:Parameter Name="CustomerID" Type="Int32" />
                                                        <asp:Parameter Name="Val" Type="Int32" DefaultValue="1" />
                                                    </DeleteParameters>
                                                </asp:ObjectDataSource>--%>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 70%; padding-right: 0px;padding-bottom: 0px;" align="right">
                                                            <table>
                                                                <tr>
                                                                    <td valign="top" style="text-align: left">
                                                                        Search :&nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"
                                                                            Width="250px"></asp:TextBox><br />
                                                                        <span style="padding-left: 50px;">(eg. Name, Email, Zip Code) </span>
                                                                    </td>
                                                                    <td valign="top" style="padding-right: 0px;">
                                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation();" />
                                                                        <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="right" style="padding: 0px;">
                                                                        <span style="float: right; padding-top: 0px; padding-right: 5px; font-size: 12px;
                                                                            font-family: Arial,Helvetica,sans-serif;">
                                                                            <asp:Label ID="lblTotcount" runat="server"></asp:Label>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                         <td style="padding-top: 0px;">
                                                <asp:GridView ID="grdCustomer" runat="server" AutoGenerateColumns="False" DataKeyNames="CustomerID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    BorderWidth="1" BorderColor="#e7e7e7" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                    PagerSettings-Mode="NumericFirstLast" 
                                                    OnRowCommand="grdCustomer_RowCommand" OnRowDataBound="grdCustomer_RowDataBound"
                                                    CellPadding="2" CellSpacing="1" 
                                                    OnPageIndexChanging="grdCustomer_PageIndexChanging" 
                                                    onsorting="grdCustomer_Sorting" onrowediting="grdCustomer_RowEditing">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                <strong>Select </strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnCustListID" runat="server" Value='<%#Eval("CustomerID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                Customer ID
                                                                <%--<asp:ImageButton ID="btnCustID" runat="server" CommandArgument="DESC" CommandName="CustomerID"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustID" runat="server" Text='<%# Eval("CustomerID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Email
                                                                <asp:ImageButton ID="imgSortEmail" runat="server" CommandArgument="DESC" CommandName="Email"
                                                                    AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Customer Name
                                                                <asp:ImageButton ID="imgSortCustomerName" runat="server" CommandArgument="DESC" CommandName="CustomerName"
                                                                    AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Store Name
                                                                <asp:ImageButton ID="imgSortStoreName" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                    AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreName" runat="server" Text='<%# Eval("StoreName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <a id="tagCustomer" runat="server" visible="false">Select</a>
                                                                <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" CommandName="Edit"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="25">
                                                    </PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <asp:Button ID="btnhdnDelete" runat="server" Text="Button" OnClick="btnhdnDelete_Click"
                                                    Style="display: none;" />
                                                <asp:HiddenField ID="hdnCustDelete" runat="server" Value="0" />
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
    </div>
</asp:Content>
