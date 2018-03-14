<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="AppConfigList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.AppConfigList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                $(document).ready(function () { jAlert('Check at least one Application Configuration!', 'Message', ''); });
                return false;
            }
            else {
                return DeleteConfirm();
            }
            return false;
        }
        function DeleteConfirm() {
            jConfirm('Are you sure want to delete all selected Application Configuration(s) ?', 'Confirmation', function (r) {
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
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <span style="vertical-align: middle; margin-top:4px; margin-right: 3px; float: left;">
                    <table>
                        <tr>
                            <td style="align="right">
                                Store :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStore" runat="server" Width="175px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/Settings/AppConfiguration.aspx">
                    <img alt="Add Configuration" title="Add Configuration" src="/App_Themes/<%=Page.Theme %>/images/add-configuration.png" /></a></span>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                                    <img class="img-left" title="Application Configuration" alt="Application Configuration"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/application-configuration-icon.png" />
                                                    <h2>
                                                        Application Configuration</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right:0px;">
                                                <span class="star" style="vertical-align: middle; text-align: center; padding-right: 20%;">
                                                    <asp:Label ID="lblmsg" runat="server"></asp:Label></span>
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.ConfigurationComponent" SelectMethod="GetDataByFilter"
                                                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="StoreId" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="ddlSearch" DbType="String" Name="SearchBy" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="SearchValue" DefaultValue="" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 60%">
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:DropDownList ID="ddlSearch" AutoPostBack="False" Width="175px" runat="server"
                                                                CssClass="order-list">
                                                                <asp:ListItem>ConfigName</asp:ListItem>
                                                                <asp:ListItem>ConfigValue</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return validation();" />
                                                        </td>
                                                        <td style="width: 5%; padding-right:0px;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdApplicationConfig" runat="server" AutoGenerateColumns="False"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                    DataSourceID="_gridObjectDataSource" OnRowCommand="grdApplicationConfig_RowCommand"
                                                    OnRowDataBound="grdApplicationConfig_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <HeaderTemplate>
                                                               Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                <asp:HiddenField ID="hdnConfigid" runat="server" Value='<%#Eval("AppConfigID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Config Name" SortExpression="ConfigName">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Config Name
                                                                <asp:ImageButton ID="btnConfigName" runat="server" CommandArgument="DESC" CommandName="ConfigName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblConfigName" runat="server" Text='<%# Bind("ConfigName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Config Value
                                                                <asp:ImageButton ID="btnConfigValue" runat="server" CommandArgument="DESC" CommandName="ConfigValue"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblConfigVal" runat="server" Text='<%# Bind("ConfigValue") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Store Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblConfigStoreName" runat="server" Text='<%# Bind("StoreName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Created On">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblConfigId" runat="server" Text='<%# Bind("CreatedOn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("AppConfigID") %>'></asp:ImageButton>
                                                                <asp:ImageButton runat="server" ID="_DeleteLinkButton" ToolTip="Delete" CommandName="DeleteAppConfig"
                                                                    CommandArgument='<%# Eval("AppConfigID") %>' message="Are you sure want to delete current Application Configuration ?"
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
                                        <tr class="altrow" runat="server" id="trBottom">
                                            <td >
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;
                                                        padding-right: 0px;">
                                                        <asp:Button ID="btnDeleteConfig" runat="server" OnClientClick="return checkCount();"
                                                            CommandName="DeleteMultiple" OnClick="btnDeleteConfig_Click" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDeleteConfig_Click" />
                                                        </div>
                                                    </span>
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
