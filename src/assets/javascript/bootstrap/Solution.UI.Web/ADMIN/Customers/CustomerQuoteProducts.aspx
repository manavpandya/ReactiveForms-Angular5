<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ADMIN/Admin.Master"
    CodeBehind="CustomerQuoteProducts.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.CustomerQuoteProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message'); });
                document.getElementById('<%=txtSearch.ClientID %>').focus();
                return false;
            }
            return true;
        }

        function SelectAll(on) {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }

        function chkSelect() {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {
                jAlert("Please select at least one Product.", "Message");
                return false;

            }
            else {
                jConfirm('Are you sure want to add selected Products?', 'Confirmation', function (r) {
                    if (r == true) {

                        document.getElementById('ContentPlaceHolder1_btnAddProductsTemp').click();
                        return true;
                    }
                    else {

                        return false;
                    }
                });
            }
            return false;

        }

        function SearchValidation() {
            if (document.getElementById('<%=txtSearch.ClientID%>') != null && document.getElementById('<%= txtSearch.ClientID%>').value == '') {
                jAlert('Please enter search value.', 'Message', '<%=txtSearch.ClientID%>');
                return false;
            }
            return true;
        }

    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <table>
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="175px"
                                AutoPostBack="true" Style="margin-top: 5px;" Visible="false">
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
                                                    <img class="img-left" title="Customer Quote Products" alt="Customer Quote Products"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/topic-list-icon.png" />
                                                    <h2>
                                                        Customer Quote Products</h2>
                                                </div>
                                                <div style="float: right;">
                                                    <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" ToolTip="Go to Customer Quote" />
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left">
                                            </td>
                                            <td align="right" colspan="2">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td valign="middle" align="right">
                                                        </td>
                                                        <td valign="middle" align="right">
                                                        </td>
                                                        <td valign="middle" align="right">
                                                        </td>
                                                        <td valign="middle" align="right">
                                                        </td>
                                                        <td align="left" style="width: 65px;">
                                                            Search By :
                                                        </td>
                                                        <td align="left" style="width: 115px;">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list" Width="110px"
                                                                AutoPostBack="false">
                                                                <asp:ListItem Text="SKU" Value="SKU"></asp:ListItem>
                                                                <asp:ListItem Text="Product Name" Value="Name"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtSearch" CssClass="order-textfield" Width="124px" runat="server"
                                                                MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClientClick="return SearchValidation();"
                                                                OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trAddProducts1" runat="server" visible="false">
                                            <td style="padding-left: 12px;">
                                                Note: Only Active Products with Available Inventory will be listed.
                                            </td>
                                            <td align="right" colspan="2" style="padding-right: 10px;">
                                                <asp:Button ID="btnAddProducts2" runat="server" OnClientClick="return chkSelect();" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div id="divGrid">
                                                    <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                                                    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                        CellPadding="2" CellSpacing="1" GridLines="None" Width="99%" PageSize="100" BorderColor="#E7E7E7"
                                                        BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" PagerSettings-Mode="NumericFirstLast"
                                                        OnPageIndexChanging="gvProducts_PageIndexChanging" OnRowCommand="gvProducts_RowCommand"
                                                        OnRowDataBound="gvProducts_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <HeaderTemplate>
                                                                    Select
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID">
                                                                <HeaderStyle BackColor=" #E7E7E7" />
                                                                <HeaderTemplate>
                                                                    Product Name
                                                                    <asp:ImageButton ID="btnProductName" runat="server" CommandArgument="DESC" CommandName="Name"
                                                                        AlternateText="Ascending Order" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID">
                                                                <HeaderStyle BackColor=" #E7E7E7" />
                                                                <HeaderTemplate>
                                                                    Product Code
                                                                    <asp:ImageButton ID="btnSKU" runat="server" CommandArgument="DESC" CommandName="SKU"
                                                                        AlternateText="Ascending Order" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <HeaderTemplate>
                                                                    Inventory
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInventory" runat="server" Text='<%# Eval("Inventory") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <HeaderTemplate>
                                                                    Price
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")),2) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <HeaderTemplate>
                                                                    Sale Price
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalePrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal( DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <HeaderTemplate>
                                                                    Store Name
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStore" runat="server" Text='<%# Eval("StoreName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trAddProducts" runat="server" visible="false">
                                            <td align="left" style="padding-left: 10px;">
                                                <a id="aAllowAll" href="javascript:SelectAll(true);">Check All</a>&nbsp;|&nbsp;<a
                                                    id="aClearAll" href="javascript:SelectAll(false);">Clear All</a>
                                            </td>
                                            <td align="right" colspan="2" style="padding-right: 10px;">
                                                <asp:Button ID="btnAddProducts" runat="server" OnClientClick="return chkSelect();" />
                                                <div style="display: none;">
                                                    <asp:Button ID="btnAddProductsTemp" OnClick="btnAddProductsTemp_Click" runat="server" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
