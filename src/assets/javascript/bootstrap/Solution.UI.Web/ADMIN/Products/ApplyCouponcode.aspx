<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ApplyCouponcode.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ApplyCouponcode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            if (document.getElementById("ContentPlaceHolder1_ddlCouponCode") != null && document.getElementById("ContentPlaceHolder1_ddlCouponCode").selectedIndex == 0) {

                jAlert('Please Select Coupon Code.','Message');
             
                return false;
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Check at least One Product !', 'Message'); });
                return false;
            }
            else {
                var message = "Do you want to Apply Coupon Code for all selected Product ?";
                return confirm(message);
            }
        }
    </script>
     <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
               
                   
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                                    
                                                    <h2>
                                                        Apply Bulk Coupon Code</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td align="left">
                                                            Coupon Code :
                                                            <asp:DropDownList ID="ddlCouponCode" runat="server" Width="180px" CssClass="order-list"
                                                                AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                       
                                                    </tr>
                                                     <tr class="even-row">
                                            <td>
                                                Select Product Type :
                                                
                                                <asp:RadioButton ID="rdobtnNewarrival" GroupName="coupon" Text="New Arrival" runat="server" AutoPostBack="true" OnCheckedChanged="rdobtnNewarrival_CheckedChanged" />
                                                <asp:RadioButton ID="rdobtnbuyonegetone" GroupName="coupon" Text="Buy 1 Get 1" runat="server" AutoPostBack="true" OnCheckedChanged="rdobtnbuyonegetone_CheckedChanged" />

                                                
                                            </td>
                                        </tr>
                                                      <tr class="altrow">
                                        <td>
                                            <table width="100%">
                                                <tr id="trtop" visible="false" runat="server">
                                                    <td class="style1">
                                                        <span><a href="javascript:selectAll(true);">Check All</a> &nbsp;|&nbsp; <a href="javascript:selectAll(false);">
                                                            Clear All</a></span>
                                                    </td>
                                                    <td align="right" style="width: 40%;float:right;">
                                                        <asp:ImageButton ID="btnApply" runat="server" ToolTip="Apply" AlternateText="Apply" OnClientClick='return checkCount();' OnClick="btnApply_Click"
                                                             />
                                                       
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                                      <tr class="even-row">
                                            <td>
                                                 <asp:GridView ID="grdProductDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="false"
                                                CellPadding="2"
                                                BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1" 
                                                CellSpacing="1" >
                                                <Columns>
                                                    <asp:TemplateField >
                                                        <HeaderTemplate>
                                                                    Select
                                                                </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                          <asp:HiddenField ID="hdnproductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="30%">
                                                        <HeaderTemplate>
                                                            Product Name
                                                            
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblProductName" runat="server" Text='<%# bind("Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="30%">
                                                        <HeaderTemplate>
                                                            SKU
                                                            
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblSKU" runat="server" Text='<%# bind("SKU") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="30%">
                                                        <HeaderTemplate>
                                                            UPC
                                                            
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblUPC" runat="server" Text='<%# bind("UPC") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                              
                                                </Columns>
                                              
                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                <AlternatingRowStyle CssClass="altrow" />
                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
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
