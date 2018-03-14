<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="DynamicPagePropertyList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.DynamicPagePropertyList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    
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
                $(document).ready(function () { jAlert('Check at least One Record !', 'Message'); });
                return false;
            }
            else {
                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Record?', 'Confirmation', function (r) {
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
            <span style="vertical-align: middle; margin-top:4px; margin-right: 3px; float: left;">
               
            </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                href="/Admin/Settings/AddDynamicPageProperty.aspx">
                <img alt="Add Event" title="Add Event" src="/App_Themes/<%=Page.Theme %>/images/add-more.png" /></a></span>
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
                <td class="border-td" >
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub" >
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Page List" alt="Page List" src="/App_Themes/<%=Page.Theme %>/Images/coupon-discount-icon.png" />
                                                <h2>
                                                    Page List</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <table width="100%">
                                                <tr>
                                                   
                                                   
                                                </tr>
                                                  <tr>
                                                     <td style="background-color: #E3E3E3; font-weight: bold;" colspan="3">&nbsp;
                                              
                                                         </td>
                                                </tr>
                                            </table>
                                           
                                            <asp:HiddenField ID="hdncoupon" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="gridcoupon" CellSpacing="1" BorderStyle="Solid" BorderWidth="1"
                                                BorderColor="#E7E7E7" runat="server" AutoGenerateColumns="False" 
                                                EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                PagerSettings-Mode="NumericFirstLast" CellPadding="2"
                                                OnRowCommand="gridcoupon_RowCommand" OnRowDataBound="gridcoupon_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                            <asp:HiddenField ID="hdneventid" runat="server" Value='<%#Eval("ID") %>' />
                                                           
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Page Name" HeaderStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            Page Name
                                                           <%-- <asp:ImageButton ID="sortcode" runat="server" CommandArgument="DESC" CommandName="CouponCode"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbcouponcode" runat="server" Text='<%# bind("PageName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Title" HeaderStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            Title
                                                           <%-- <asp:ImageButton ID="sortstore" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" Width="15%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                           <asp:Label ID="lbtitle" runat="server" Text='<%# bind("Title") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="2%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="lnkbtnedit" ToolTip="Edit" CommandName="edit"
                                                                CommandArgument='<%# Eval("ID") %>'></asp:ImageButton>
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
                                                        <span><a href="javascript:selectAll(true);">Check All</a> &nbsp;|&nbsp; <a href="javascript:selectAll(false);">
                                                            Clear All</a></span>
                                                    </td>
                                                    <td align="right" style="width: 10%; padding-right:0px;">
                                                      
                                                        <asp:Button ID="btndelete" runat="server" ToolTip="Delete" OnClientClick='return checkCount()'
                                                            OnClick="btndelete_Click"  />
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
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
