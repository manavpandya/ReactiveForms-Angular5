<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CouponsList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.CouponsList" %>

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
                $(document).ready(function () { jAlert('Check at least One Coupon Code !', 'Message'); });
                return false;
            }
            else {
                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Coupon Code ?', 'Confirmation', function (r) {
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
                <table>
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
                href="/Admin/Promotions/Coupon.aspx">
                <img alt="Add Coupon" title="Add Coupon" src="/App_Themes/<%=Page.Theme %>/images/add-coupon.png" /></a></span>
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
                                                <img class="img-left" title="Coupon Discount" alt="Coupon Discount" src="/App_Themes/<%=Page.Theme %>/Images/coupon-discount-icon.png" />
                                                <h2>Coupon Discount</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" style="padding-right: 0px;">Staus : &nbsp;
                                                        <asp:DropDownList ID="ddlSearch" CssClass="order-list" runat="server" Width="90px"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlstore_SelectedIndexChanged">
                                                            <asp:ListItem Value="" Text="All"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="In Active"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        Search : &nbsp;
                                                        <asp:TextBox ID="txtSearch" runat="server" MaxLength="500" CssClass="order-textfield"
                                                            Width="160px"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" runat="server" OnClientClick='return validation();' OnClick="btnSearch_Click" />
                                                        <asp:Button ID="btnshowall" runat="server" CommandName="ShowAll" OnClick="btnshowall_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #E3E3E3; font-weight: bold;" colspan="3">&nbsp;
                                                <div style="float: right">
                                                    <table>
                                                        <tr>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">
                                                                <div style='float: left; width: 10px; height: 10px; background-color: red'>
                                                                </div>
                                                            </td>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">Expired
                                                            </td>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">
                                                                <div style='float: left; width: 10px; height: 10px; background-color: green'>
                                                                </div>
                                                            </td>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">Not Expired
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:ObjectDataSource ID="gridObjectDataSource" runat="server" EnablePaging="True"
                                                TypeName="Solution.Bussines.Components.CouponComponent" SelectMethod="GetDataByFilter"
                                                StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                SelectCountMethod="GetCount">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdncoupon" DbType="String" DefaultValue="CouponID"
                                                        Name="CName" />
                                                    <asp:ControlParameter ControlID="ddlstore" DbType="Int32" DefaultValue="" Name="pStoreId" />
                                                    <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                    <asp:ControlParameter ControlID="hdnLoginID" DbType="Int32" Name="ploginid" DefaultValue="0" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hdncoupon" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="gridcoupon" CellSpacing="1" BorderStyle="Solid" BorderWidth="1"
                                                BorderColor="#E7E7E7" runat="server" AutoGenerateColumns="False" DataKeyNames="CouponID"
                                                EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                GridLines="None" AllowPaging="True" PageSize="10000"
                                                PagerSettings-Mode="NumericFirstLast" CellPadding="2" DataSourceID="gridObjectDataSource"
                                                OnRowCommand="gridcoupon_RowCommand" OnRowDataBound="gridcoupon_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                            <asp:HiddenField ID="hdncouponid" runat="server" Value='<%#Eval("CouponID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Coupon Code" HeaderStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            Coupon Code
                                                            <asp:ImageButton ID="sortcode" runat="server" CommandArgument="DESC" CommandName="CouponCode"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltStatusColor" runat="server"></asp:Literal>
                                                            <asp:Label ID="lbcouponcode" runat="server" Text='<%# bind("CouponCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Name" HeaderStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            Store Name
                                                            <asp:ImageButton ID="sortstore" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" Width="15%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbstorename" runat="server" Text='<%# bind("StoreName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ex. Date" HeaderStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            Ex. Date
                                                            <asp:ImageButton ID="sortexpire" runat="server" CommandArgument="DESC" CommandName="ExpirationDate"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbexpiredate" runat="server" Text='<%# bind("ExpirationDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="7%">
                                                        <HeaderTemplate>
                                                            Status 
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbStatus" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Typee" HeaderStyle-Width="7%">
                                                        <HeaderTemplate>
                                                            Type
                                                            
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbtype" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Updated By" HeaderStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            Updated By
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbupdatedby" runat="server" Text='<%# bind("Updatedby") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Updated On" HeaderStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            Updated On
                                                             
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbupdateddate" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Discount%" HeaderStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            Discount%
                                                            <asp:ImageButton ID="sortdiscount" runat="server" CommandArgument="DESC" CommandName="DiscountPercent"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbdiscountpercent" runat="server" Text='<%# String.Format("{0:F}", DataBinder.Eval(Container.DataItem,"DiscountPercent"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Expires" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbexpires" runat="server" Text='<%# bind("Expires") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View Usage" HeaderStyle-Width="8%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="hprlnkUsage" runat="server" Font-Underline="True" Text="View Usage"
                                                                ForeColor="#384557" ToolTip='<%# "View Usage For " + DataBinder.Eval(Container.DataItem, "CouponCode") %>'
                                                                NavigateUrl='<%# "CouponUsageList.aspx?CouponCode="+ DataBinder.Eval(Container.DataItem, "CouponCode")+"&StoreID="+ DataBinder.Eval(Container.DataItem, "StoreID") %>'
                                                                Style="cursor: pointer">
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="2%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="lnkbtnedit" ToolTip="Edit" CommandName="edit"
                                                                CommandArgument='<%# Eval("CouponID") %>'></asp:ImageButton>
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
