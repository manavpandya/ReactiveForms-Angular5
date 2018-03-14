<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ShippingMethodList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.ShippingMethodList"
    ValidateRequest="false" %>

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
                $(document).ready(function () { jAlert('Check at least One Shipping Method!', 'Message', ''); });
                return false;
            }
            else {

                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Shipping Method ?', 'Confirmation', function (r) {
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
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width:100%;">
                <span style="vertical-align: middle; margin-right: 3px;margin-top: 4px; float: left;">
                    <table>
                        <tr>
                            <td style="width: 24%" align="left">
                                Store :
                                <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/configuration/ShippingMethod.aspx">
                    <img alt="Add Shipping Methods" title="Add Shipping Methods" src="/App_Themes/<%=Page.Theme %>/images/add-shipping-method.png" /></a></span>
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
                                                    <img class="img-left" title="Shipping Methods" alt="Shipping Methods" src="/App_Themes/<%=Page.Theme %>/Images/shipping-methods-icon.png" />
                                                    <h2>
                                                        Shipping Methods</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td align="left">
                                                            Shipping Service :
                                                            <asp:DropDownList ID="ddlShippingService" runat="server" AutoPostBack="True" CssClass="order-list"
                                                                OnSelectedIndexChanged="ddlShippingService_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 2%">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:ImageButton ID="ibtnsearch" runat="server" OnClientClick="return validation();"
                                                                OnClick="btnGo_Click" />
                                                        </td>
                                                        <td style="width: 5%; padding-right:0px;">
                                                            <asp:ImageButton ID="ibtnShowall" runat="server" OnClick="btnSearchall_Click" CommandName="ShowAll" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:UpdatePanel ID="up1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ObjectDataSource runat="server" ID="odsShippingMethods" SelectMethod="GetDataByFilterforSM"
                                                            StartRowIndexParameterName="startIndex" SortParameterName="sortBy" MaximumRowsParameterName="pageSize"
                                                            TypeName="Solution.Bussines.Components.ShippingComponent" EnablePaging="true"
                                                            SelectCountMethod="GetCountforSM">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hdnState" DbType="String" DefaultValue="ShippingMethodID"
                                                                    Name="CName" />
                                                                <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                                <asp:ControlParameter ControlID="ddlShippingService" DbType="int32" Name="pShippingServiceID"
                                                                    DefaultValue="" />
                                                                <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:HiddenField ID="hdnState" runat="server" />
                                                        <asp:GridView ID="gvShippingMethods" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                            Width="100%" BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7" CellSpacing="1"
                                                            EmptyDataText="No Records Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                                                            PagerSettings-Mode="NumericFirstLast" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                            AllowSorting="True" DataSourceID="odsShippingMethods" EmptyDataRowStyle-ForeColor="Red"
                                                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnRowDataBound="gvShippingMethods_RowDataBound"
                                                            OnRowCommand="gvShippingMethods_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        <asp:HiddenField ID="hdnShippingMethodID" runat="server" Value='<%#Eval("ShippingMethodID") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        Name
                                                                        <asp:ImageButton ID="lbName" runat="server" CommandArgument="DESC" CommandName="Name"
                                                                            AlternateText="Ascending Order" OnClick="Sorting" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("Name")%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Shipping Service" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                       Shipping Service
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("ShippingService")%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Store Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        Store Name
                                                                        <asp:ImageButton ID="lbstname" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                            OnClick="Sorting" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("StoreName")%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Show On Client" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                       Show On Client
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"ShowOnClient"))) %>'
                                                                            alt="" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Show On Admin" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        Show On Admin
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"ShowOnAdmin"))) %>'
                                                                            alt="" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Real Time Shipping" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                       Real Time Shipping
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"RealTimeShipping"))) %>'
                                                                            alt="" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        Status
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>'
                                                                            alt="" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                            CommandArgument='<%# Eval("ShippingMethodID") %>'></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                            <AlternatingRowStyle CssClass="altrow" />
                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvShippingMethods" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <div id="data" runat="server" style="display: none">
                                                    <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                        href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;
                                                            padding-right: 0px;">
                                                            <asp:Button ID="btnDelete" runat="server" OnClientClick="return checkCount();" OnClick="btnDelete_Click" />
                                                            <div style="display: none">
                                                                <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDelete_Click" />
                                                            </div>
                                                        </span>
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
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="10" />
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
