<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CategoryList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.categoryList"
    Theme="Gray" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width:100%;"><span style="vertical-align: middle; margin-right: 3px;margin-top: 4px; float: left;"><table><tr> <td style=" align="right">
                                                            Store :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStore" runat="server" Width="180px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
                                                            </asp:DropDownList>
                                                        </td></tr></table></span>
                <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="/Admin/Products/Category.aspx">
                    <img alt="Add Category" title="Add Category" src="/App_Themes/<%=Page.Theme %>/images/add-category.png" /></a></span></div>
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
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Country" alt="Add Country" src="/App_Themes/<%=Page.Theme %>/images/category-list-icon.png">
                                                    <h2>
                                                        Category List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right:0px;">
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                       
                                                        <td style="width: 30%">
                                                            Status :
                                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="120px" AutoPostBack="True"
                                                                CssClass="order-list" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                                <asp:ListItem>Active</asp:ListItem>
                                                                <asp:ListItem>InActive</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 40%" align="right">
                                                            Search :
                                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list" Style="width: 150px;">
                                                                <asp:ListItem Value="Name">Category Name</asp:ListItem>
                                                                <asp:ListItem Value="ParentCatName">Parent Category Name</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 15%">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield " Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 15%" style="text-align: right;">
                                                            <asp:ImageButton ID="ibtnsearch" runat="server" style="padding-right:5px;" OnClick="btnGo_Click" OnClientClick="return validation();"
                                                                ImageUrl="/App_Themes/<%=Page.Theme %>/Images/search.gif" />
                                                            <asp:ImageButton ID="btnShowAll" runat="server" OnClick="btnShowAll_Click" ImageUrl="/App_Themes/<%=Page.Theme %>/Images/showall.png" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6" align="center">
                                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:UpdatePanel ID="up1" runat="server">
                                                    <ContentTemplate>
                                                        <%-- <asp:ObjectDataSource runat="server" ID="odsCustomers" SelectMethod="GetDataByFilter"
                                                            StartRowIndexParameterName="startIndex" SortParameterName="sortBy" MaximumRowsParameterName="pageSize"
                                                            SelectCountMethod="GetCount" TypeName="Solution.Bussines.Components.CategoryComponent"
                                                            EnablePaging="true">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddlStatus" DbType="String" Name="status" DefaultValue="" />
                                                                <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="StoreID" />
                                                                <asp:ControlParameter ControlID="ddlSearch" DbType="String" Name="SearchBy" />
                                                                <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="SearchValue" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>--%>
                                                        <asp:HiddenField ID="hdnCategory" runat="server" />
                                                        <%-- <asp:GridView ID="gvCategory" AutoGenerateColumns="false" runat="server" GridLines="None"
                                                            Width="100%" CellPadding="2" CellSpacing="1" EmptyDataText="No Records Found!"
                                                            RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast"
                                                            AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True"
                                                            DataSourceID="odsCustomers" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                            ViewStateMode="Enabled" OnRowDataBound="gvCategory_RowDataBound" BorderWidth="1"
                                                            BorderColor="#e7e7e7">--%>
                                                        <asp:GridView ID="gvCategory" AutoGenerateColumns="false" runat="server" GridLines="None"
                                                            Width="100%" CellPadding="2" CellSpacing="1" EmptyDataText="No Records Found!"
                                                            RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast"
                                                            AllowPaging="True" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                            OnRowDataBound="gvCategory_RowDataBound" BorderWidth="1" BorderColor="#e7e7e7"
                                                            OnPageIndexChanging="gvCategory_PageIndexChanging" PageSize="20">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        <asp:HiddenField ID="hdnCatid" runat="server" Value='<%#Eval("CategoryID") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="7%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Category Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                            <tr style="border: 0px;">
                                                                                <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                    <strong>Name</strong>
                                                                                    <asp:ImageButton ID="lbName" runat="server" CommandArgument="DESC" CommandName="Name"
                                                                                        AlternateText="Ascending Order" OnClick="Sorting" ImageUrl="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <div style="text-align: justify; width: 300px;">
                                                                            <a href='<%# "Category.aspx?Mode=edit&ID="+ DataBinder.Eval(Container.DataItem, "CategoryID") +"&storeid="+DataBinder.Eval(Container.DataItem, "StoreID")    %>'>
                                                                                <%#Eval("Name").ToString().Length>50?Eval("Name").ToString().Substring(0,50):Eval("Name")%>
                                                                            </a>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Parent Category Name" HeaderStyle-HorizontalAlign="Left"
                                                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblParent" runat="server" Text='<%#Eval("ParentName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="left">
                                                                    <HeaderTemplate>
                                                                        <table cellpadding="0" cellspacing="1" border="0" align="center">
                                                                            <tr style="border: 0px;">
                                                                                <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                    <strong>Display&nbsp;Order</strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("DisplayOrder").ToString().Equals("999")?null:Eval("DisplayOrder")%>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Updated By" ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"
                                                                    HeaderStyle-HorizontalAlign="left">
                                                                    <HeaderTemplate>
                                                                        <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                            <tr style="border: 0px;">
                                                                                <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                    <strong>Updated By</strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("UpdatedBy")%>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Updated On" ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"
                                                                    HeaderStyle-HorizontalAlign="left">
                                                                    <HeaderTemplate>
                                                                        <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                            <tr style="border: 0px;">
                                                                                <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                    <strong>Updated On</strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("UpdatedOn")%>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="29px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="hplEdit" runat="server" ImageUrl="/App_Themes/gray/Images/Edit.gif"
                                                                            ToolTip="Edit" NavigateUrl='<%# "Category.aspx?Mode=edit&ID="+ DataBinder.Eval(Container.DataItem, "CategoryID") +"&storeid="+DataBinder.Eval(Container.DataItem, "StoreID")    %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="29px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                            <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                            <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvCategory" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr class="altrow" id="trBottom" runat="server">
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;">
                                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl='/App_Themes/<%=Page.Theme %>/Images/delet.gif'
                                                            OnClientClick="return checkCount();" OnClick="btnDelete_Click" ToolTip="Delete" />
                                                    </span>
                                                <div style="display: none;">
                                                    <asp:Button ID="btnDeleteTemp" runat="server" OnClick="btnDelete1_Click" /></div>
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
                jAlert('Check at least One Record!', 'Message');
                return false;
            }
            else {
                return checkaa();
            }
            return true;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Category ?', 'Confirmation', function (r) {
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
