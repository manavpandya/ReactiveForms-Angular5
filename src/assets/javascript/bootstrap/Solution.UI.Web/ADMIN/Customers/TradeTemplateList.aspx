<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="TradeTemplateList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.TradeTemplateList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (on.toString() == 'false') {
                        if (myform.elements[i].checked) {
                            myform.elements[i].checked = false;
                        }
                    }
                    else {
                        myform.elements[i].checked = true;
                    }
                }
            }

        }
    </script>
    <script language="javascript" type="text/javascript">
        function checkCount() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Check at least One Trade Template !', 'Message'); });
                return false;
            }
            else {
                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Trade Template ?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order" style="width: 100%;">
            <span style="vertical-align: middle; margin-top: 4px; margin-right: 3px; float: left;">
                <table style="display:none;">
                    <tr>
                        <td>Store :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstore" CssClass="order-list" runat="server" Width="170px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlstore_SelectedIndexChanged">
                            </asp:DropDownList>

                        </td>
                        <asp:TextBox ID="hdnLoginID" runat="server" Visible="false"></asp:TextBox>
                    </tr>
                </table>
            </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                href="/Admin/Customers/TradeCustomerTemplate.aspx">
                <img alt="Add Template" title="Add Template" src="/App_Themes/<%=Page.Theme %>/images/add-coupon.png" /></a></span>
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                                <img class="img-left" title="Trade Templates" alt="Trade Templates" src="/App_Themes/<%=Page.Theme %>/Images/coupon-discount-icon.png" />
                                                <h2>Trade Templates</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" style="padding-right: 0px;">Search : &nbsp;
                                                        <asp:TextBox ID="txtSearch" runat="server" MaxLength="500" CssClass="order-textfield"
                                                            Width="160px"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" runat="server" OnClientClick='return validation();' OnClick="btnSearch_Click" />
                                                        <asp:Button ID="btnshowall" runat="server" CommandName="ShowAll" OnClick="btnshowall_Click" />
                                                    </td>
                                                </tr>

                                            </table>

                                            <asp:HiddenField ID="hdncoupon" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="gridtradetemplate" CellSpacing="1" BorderStyle="Solid" BorderWidth="1"
                                                BorderColor="#E7E7E7" runat="server" AutoGenerateColumns="False"
                                                EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                PagerSettings-Mode="NumericFirstLast" CellPadding="2"
                                                OnRowCommand="gridtradetemplate_RowCommand" OnRowDataBound="gridtradetemplate_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                            <asp:HiddenField ID="hdnTradeTemplateID" runat="server" Value='<%#Eval("TradeTemplateID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Template Name" HeaderStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            Event Name
                                                           
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbTradeTempName" runat="server" Text='<%# bind("TradeTempName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Name" HeaderStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            Store Name
                                                            
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" Width="15%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbstorename" runat="server" Text='HPD E-Commerce'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Active" HeaderStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            Active
                                                            
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" Width="15%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblactive" runat="server" Text='<%# bind("Active") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="2%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="lnkbtnedit" ToolTip="Edit" CommandName="edit"
                                                                CommandArgument='<%# Eval("TradeTemplateID") %>'></asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                <AlternatingRowStyle CssClass="altrow" />
                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%">
                                                <tr id="trBottom" runat="server">
                                                    <td class="style1">
                                                        <span><a href="javascript:selectAll(true);">Check All</a> &nbsp;|&nbsp; <a href="javascript:selectAll(false);">Clear All</a></span>
                                                    </td>
                                                    <td align="right" style="width: 10%; padding-right: 0px;">
                                                        <asp:Button ID="btndelete" runat="server" ToolTip="Delete" OnClientClick='return checkCount()'
                                                            OnClick="btndelete_Click" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btndelete_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                            </table>
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
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

