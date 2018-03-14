<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductRomanShadeYardageList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductRomanShadeYardageList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Shade Name.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 2px;" cellpadding="3" cellspacing="3">
                        <tr>
                            <td style="padding-left: 0px;" align="left">
                                Store : &nbsp;
                                <asp:DropDownList runat="server" ID="ddlStoreName" CssClass="add-product-list" Width="225px"
                                    Height="20px" AutoPostBack="true" OnSelectedIndexChanged="ddlStoreName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    id="AtagInsertProductyardage" runat="server" href="/Admin/Products/ProductRomanShadeYardage.aspx?StoreId=1">
                    <img alt="Add Roman Shade Yardage" title="Add Roman Shade Yardage" src="/App_Themes/<%=Page.Theme %>/images/add-searchproducttype.png" /></a></span>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                            <th colspan="4">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Search Product Type List" alt="Search Product Type List"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png">
                                                    <h2>
                                                        Search Product Type List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right" style="width: 100%;">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 70%">
                                                        </td>
                                                        <td align="right" style="width: 10%">
                                                            Search&nbsp;:
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="160px"
                                                                Style="vertical-align: middle; float: right"></asp:TextBox>
                                                        </td>
                                                        <td align="right" style="width: 5%;">
                                                            <asp:Button ID="btnSearch" runat="server" OnClientClick="return validation();" OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td align="right" style="width: 5%;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="4">
                                                <asp:GridView ID="grdSearchProductType" runat="server" AutoGenerateColumns="False"
                                                    DataKeyNames="RomanShadeId" EmptyDataText="No Record(s) Found." AllowSorting="false"
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" OnRowCommand="grdSearchProductType_RowCommand" Width="100%"
                                                    GridLines="None" AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast"
                                                    OnRowDataBound="grdSearchProductType_RowDataBound" OnPageIndexChanging="grdSearchProductType_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                &nbsp;Shade Name
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                &nbsp;<asp:Label ID="lblRomanShadeId" runat="server" Visible="false" Text='<%# Bind("RomanShadeId") %>'></asp:Label>
                                                                &nbsp;<asp:Label ID="lblShadeName" runat="server" Text='<%# Bind("ShadeName") %>'></asp:Label>
                                                                <asp:Label ID="lblStoreId" runat="server" Visible="false" Text='<%# Bind("StoreId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                &nbsp;Standard Allowance [Width]
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                            <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                            <ItemTemplate>
                                                                &nbsp;<asp:Label ID="lblWidthStandardAllowance" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"WidthStandardAllowance")),2) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                &nbsp;Standard Allowance [Length]
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                            <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                            <ItemTemplate>
                                                                &nbsp;<asp:Label ID="lblLengthStandardAllowance" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"LengthStandardAllowance")),2) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgIsActive" runat="server" AlternateText='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active")).ToString() %>'
                                                                    ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active")).ToString() %>'
                                                                    ImageUrl='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations">
                                                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("RomanShadeId") %>'></asp:ImageButton>
                                                                <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="DeleteAdmin"
                                                                    CommandArgument='<%# Eval("RomanShadeId") %>' message="Are you sure want to delete current Record?"
                                                                    OnClientClick='return confirm(this.getAttribute("message"))'></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
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
