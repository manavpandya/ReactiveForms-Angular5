<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ItemwiseSalesReport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.ItemwiseSalesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style>
        .pageccc td
        {
            border-style: none !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {

            $('#ContentPlaceHolder1_txtFromdate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtTodate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });

    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <table>
                    <tr>
                        <td align="left">
                            Store :&nbsp;<asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list"
                                Width="175px" AutoPostBack="true" Style="margin-top: 5px;" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
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
                                                    <img class="img-left" title="Item Wise Sales Report" alt="Item Wise Sales Report"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                    <h2>
                                                        Item Wise Sales Report</h2>
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
                                                            From Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtFromdate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            To Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtTodate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            Search:<asp:TextBox ID="txtSearch" runat="server" CssClass="from-textfield" Width="150px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                                <br /><span style="font:10px;">(Search By: Name,SKU,OptionSku,UPC)</span>
                                                                
                                                                
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td align="right">
                                                        <asp:Button ID="btnExport" runat="server" ToolTip="Export" OnClick="btnExport_Click" />
                                                        </td>
                                                        <td align="right" style="display: none;">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div id="divGrid">
                                                    <asp:GridView ID="grvItemwiseSalesRpt" runat="server" CssClass="order-table" BorderStyle="Solid"
                                                        BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                        Width="100%" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true"
                                                        ShowHeaderWhenEmpty="True" ShowFooter="true" EmptyDataText="No Record(s) Found."
                                                        EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                        OnRowDataBound="grvItemwiseSalesRpt_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Item Name">
                                                                <HeaderTemplate>
                                                                    Item Name
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductId" Visible="false" runat="server" Text='<%# Eval("RefProductID")%>'></asp:Label>
                                                                    <asp:Label ID="lblPName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SKU">
                                                                <HeaderTemplate>
                                                                    SKU
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSKU" runat="server" Text='<%# Eval("SKU")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Option SKU">
                                                                <HeaderTemplate>
                                                                    Option SKU
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOptionSku" runat="server" Text='<%# Eval("OptionSku")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField  HeaderText="UPC">
                                                                <HeaderTemplate>
                                                                    UPC
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUPC" runat="server" Text='<%# Eval("UPC")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterTemplate>
                                                                    <strong>Total :</strong>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField  HeaderText="Quantity">
                                                                <HeaderTemplate>
                                                                    Quantity
                                                                    <asp:ImageButton ID="blImage" runat="server" OnClientClick="chkHeight();" ImageUrl="/App_Themes/<%=Page.Theme %>/icon/order-date.png"
                                                                        CommandArgument="ASC" CommandName="Quantity" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterTemplate>
                                                                    <strong>
                                                                        <asp:Label ID="lblGrandQtyTotal" runat="server"></asp:Label></strong>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Price">
                                                                <HeaderTemplate>
                                                                    Total Price
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    $<asp:Label ID="lblPrice" runat="server" Text='<%# string.Format("{0:0.00}", Eval("Totalprice"))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <strong>$<asp:Label ID="lblFPrice" runat="server"></asp:Label></strong>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                           
                                                            <asp:TemplateField HeaderText="Order Number">
                                                                <HeaderTemplate>
                                                                    Order#
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltrOrderNumber" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="altrow" VerticalAlign="top" />
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging pageccc" BorderStyle="None" />
                                                        <RowStyle CssClass="odd-row" VerticalAlign="top" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="#E7E7E7" />
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
    </div>
</asp:Content>
