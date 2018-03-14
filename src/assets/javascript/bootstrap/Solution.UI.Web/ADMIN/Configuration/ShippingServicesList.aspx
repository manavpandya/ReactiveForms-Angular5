<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ShippingServicesList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.ShippingServicesList"
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <asp:ScriptManager ID="SM1" runat="server">
        </asp:ScriptManager>
        <div class="content-row1">
            <div class="create-new-order" style="width:100%;">
                <span style="vertical-align: middle; margin-right: 3px; margin-top: 4px; float: left;">
                    <table>
                        <tr>
                            <td>
                                Store :
                                <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/configuration/ShippingServices.aspx">
                    <img alt="Add Shipping Service" title="Add Shipping Service" src="/App_Themes/<%=Page.Theme %>/images/add-shipping-service.png" /></a></span>
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
                                                    <img class="img-left" title="Shipping Services" alt="Shipping Services" src="/App_Themes/<%=Page.Theme %>/Images/shipping-services-icon.png" />
                                                    <h2>
                                                        Shipping Services</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 15%">
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:ImageButton ID="ibtnsearch" runat="server" OnClientClick="return validation();"
                                                                OnClick="btnGo_Click" />
                                                        </td>
                                                        <td style="width: 5%; padding-right:0px;" >
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
                                                        <asp:ObjectDataSource runat="server" ID="odsShippingService" SelectMethod="GetDataByFilter"
                                                            StartRowIndexParameterName="startIndex" SortParameterName="sortBy" MaximumRowsParameterName="pageSize"
                                                            TypeName="Solution.Bussines.Components.ShippingComponent" EnablePaging="true"
                                                            SelectCountMethod="GetCount">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hdnState" DbType="String" DefaultValue="ShippingServiceID"
                                                                    Name="CName" />
                                                                <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                                <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:HiddenField ID="hdnState" runat="server" />
                                                        <asp:GridView ID="gvShippingService" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                            Width="100%" EmptyDataText="No Records Found!" BorderStyle="Solid" BorderWidth="1"
                                                            BorderColor="#E7E7E7" CellSpacing="1" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                                                            PagerSettings-Mode="NumericFirstLast" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                            AllowSorting="True" DataSourceID="odsShippingService" EmptyDataRowStyle-ForeColor="Red"
                                                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnRowCommand="gvShippingService_RowCommand"
                                                            OnRowDataBound="gvShippingService_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Shipping Service" ItemStyle-HorizontalAlign="Left"
                                                                    ItemStyle-Width="220px" HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        Shipping Service
                                                                        <asp:ImageButton ID="lbShippingService" runat="server" CommandArgument="DESC" CommandName="ShippingService"
                                                                            AlternateText="Ascending Order" OnClick="Sorting" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%# Eval("ShippingService")%>
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
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                       Status
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <img alt="" src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="70px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                            CommandArgument='<%# Eval("ShippingServiceID") %>'></asp:ImageButton>
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
                                                        <asp:AsyncPostBackTrigger ControlID="gvShippingService" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
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
