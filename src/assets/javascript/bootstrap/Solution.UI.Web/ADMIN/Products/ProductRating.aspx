<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductRating.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductRating" %>

<%@ Register Assembly="Castle.Web.Controls.Rater" Namespace="Castle.Web.Controls"
    TagPrefix="cc1" %>
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
    <script type="text/javascript">
        function checkondelete(id, name) {
           
            var a;
            if (name == "Approve") {
                a = 'Are you sure want to Approve Rating?';
            }
            else
                a = 'Are you sure want to Disapprove Rating?';
            jConfirm(a, 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById("ContentPlaceHolder1_hdnDelete").value = id;
                    document.getElementById("ContentPlaceHolder1_hdnCommandName").value = name;
                    document.getElementById("ContentPlaceHolder1_btnhdnDelete").click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
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
                $(document).ready(function () { jAlert('Check at least One Product Review !', 'Message'); });
                return false;
            }
            else {
                var message = "Do you want to Approve all selected Product Reviews ?";
                return confirm(message);
            }
        }
        function checkCountfordis() {
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
                $(document).ready(function () { jAlert('Check at least One Product Review !', 'Message'); });
                return false;
            }
            else {
                var message = "Do you want to DisApprove all selected Product Reviews ?";
                return confirm(message);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order" style="width:100%;">
         <span style="vertical-align: middle; margin-right: 3px; float: left;">
             <table style="margin-top:4px; float: left">
                <tr>
                    <td align="right">
                        Store :
                    </td>
                    <td>
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="drpstore" runat="server" Width="170px" AutoPostBack="true"
                            CssClass="order-list" OnSelectedIndexChanged="drpstore_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            </span>
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
                            <span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/Products/AddProductRating.aspx">
                    <img alt="Add Review" title="Add Review" src="/App_Themes/<%=Page.Theme %>/images/add-product-review.png" /></a></span>
                        </tr>
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Product Reviews" alt="Product Reviews" src="/App_Themes/gray/Images/product-rating-icon.png" />
                                                <h2>
                                                    Product Reviews</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                           
                                                            <tr>
                                                                <td align="right">
                                                                    Products Review Status :
                                                                </td>
                                                                <td>
                                                                    &nbsp;&nbsp;
                                                                    <asp:DropDownList ID="ddlreviewstatus" runat="server" Width="125px" AutoPostBack="true"
                                                                        CssClass="order-list" OnSelectedIndexChanged="ddlreviewstatus_SelectedIndexChanged">
                                                                        <asp:ListItem Value="0" Selected="true">Pending</asp:ListItem>
                                                                        <asp:ListItem Value="1">Approved</asp:ListItem>
                                                                        <asp:ListItem Value="-1">Disapproved</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                TypeName="Solution.Bussines.Components.ProductComponent" SelectMethod="GetDataByFilter"
                                                StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                SelectCountMethod="GetCount">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdnrating" DbType="String" DefaultValue="RatingID"
                                                        Name="CName" />
                                                    <asp:ControlParameter ControlID="drpstore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                    <asp:ControlParameter ControlID="ddlreviewstatus" DbType="String" Name="pSearchBy" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hdnrating" runat="server" />
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
                                                    <td align="right" style="width: 40%;float:right;">
                                                        <asp:ImageButton ID="btnapproverating" runat="server" ToolTip="Approve" OnClientClick='return checkCount();'
                                                            OnClick="btnapproverating_Click" />
                                                       <asp:ImageButton ID="btnDisapproverating" runat="server" ToolTip="DisApprove" OnClientClick='return checkCountfordis();'
                                                            OnClick="btnDisapproverating_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="grdProductReview" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                PagerSettings-Mode="NumericFirstLast" CellPadding="2" DataSourceID="_gridObjectDataSource"
                                                BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1" OnRowDataBound="grdProductReview_RowDataBound"
                                                CellSpacing="1" OnRowCommand="grdProductReview_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField >
                                                        <HeaderTemplate>
                                                                    Select
                                                                </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                            <asp:HiddenField ID="hdnratingid" runat="server" Value='<%#Eval("RatingID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="30%">
                                                        <HeaderTemplate>
                                                            Product Name
                                                            <asp:ImageButton ID="lbName" runat="server" CommandArgument="DESC" CommandName="ProductName"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <a style="color: #212121; text-decoration: underline" href='<%# "Product.aspx?Mode=edit&ID=" + DataBinder.Eval(Container.DataItem,"ProductID")+"&StoreID=" + DataBinder.Eval(Container.DataItem,"StoreID") %>'>
                                                                <asp:Label ID="lbproductname" runat="server" Text='<%# bind("ProductName") %>'></asp:Label></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbcustomername" runat="server" Text='<%# bind("CustomerName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rating" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <cc1:Rater ID="Rater" runat="server" AutoPostback="True" designtimedragdrop="128"
                                                                Enabled="False" ScriptUrl="/App_Themes/<%=Page.Theme %>/js/CastleRater.js" Value='<%# bind("Rating") %>'></cc1:Rater>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Comments" ItemStyle-Width="40%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbcomments" runat="server" Text='<%# bind("Comments") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <HeaderTemplate>
                                                            Date
                                                            <asp:ImageButton ID="lbDate" runat="server" CommandArgument="DESC" CommandName="CreatedOn"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbcreatedon" runat="server" Text='<%# bind("CreatedOn") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approve" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="btnApprove" ImageUrl="/App_Themes/<%= Page.Theme %>/images/approve.png"
                                                                ToolTip="Approve" CommandName="Approve" CommandArgument='<%# Eval("RatingID") %>'
                                                                message='<%# Eval("RatingID") %>' OnClientClick='return checkondelete(this.getAttribute("message"),"Approve");'>
                                                            </asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Disapprove" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="btnUnApprove" ImageUrl="/App_Themes/<%= Page.Theme %>/images/disapprove.png"
                                                                ToolTip="Disapprove" CommandName="Disapprove" CommandArgument='<%# Eval("RatingID") %>'
                                                                message='<%# Eval("RatingID") %>' OnClientClick='return checkondelete(this.getAttribute("message"),"Disapprove");'>
                                                            </asp:ImageButton>
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
                                            <asp:Button ID="btnhdnDelete" runat="server" Text="Button" Style="display: none"
                                                OnClick="btnhdnDelete_Click" />
                                            <asp:HiddenField ID="hdnDelete" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnCommandName" runat="server" Value="0" />
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
