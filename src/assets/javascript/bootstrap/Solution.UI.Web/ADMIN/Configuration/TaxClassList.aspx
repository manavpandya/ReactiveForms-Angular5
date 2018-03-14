<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="TaxClassList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Tax_Management.TaxClassList"
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
                $(document).ready(function () { jAlert('Check at least One Tax Class!', 'Message', ''); });
                return false;
            }
            else {
                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Tax Class ?', 'Confirmation', function (r) {
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
                <span style="vertical-align: middle; margin-right: 3px; margin-top: 4px; float: left;">
                    <table>
                        <tr>
                            <td style=" align="left">
                                Store :
                                <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/configuration/TaxClass.aspx">
                    <img alt="Add Tax Class" title="Add Tax Class" src="/App_Themes/<%=Page.Theme %>/images/add-tax-class.png" /></a></span>
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
                                                    <img class="img-left" title="Tax Class List" alt="Tax Class List" src="/App_Themes/<%=Page.Theme %>/Images/tax-class-list-icon.png" />
                                                    <h2>
                                                        Tax Class List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right:0px;">
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 65%">
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list" Width="100px">
                                                                <asp:ListItem Value="TaxName">Tax Class</asp:ListItem>
                                                                <asp:ListItem Value="TaxCode">Tax Code</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 5%">
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
                                                        <asp:ObjectDataSource runat="server" ID="odsTaxClass" SelectMethod="GetDataByFilter"
                                                            StartRowIndexParameterName="startIndex" SortParameterName="sortBy" MaximumRowsParameterName="pageSize"
                                                            TypeName="Solution.Bussines.Components.TaxClassComponent" EnablePaging="true"
                                                            SelectCountMethod="GetCount">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hdnState" DbType="String" DefaultValue="TaxClassId"
                                                                    Name="CName" />
                                                                <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                                <asp:ControlParameter ControlID="ddlSearch" DbType="String" Name="pSearchBy" />
                                                                <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:HiddenField ID="hdnState" runat="server" />
                                                        <asp:GridView ID="gvTaxClass" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                            BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7" CellSpacing="1" Width="100%"
                                                            EmptyDataText="No Records Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                                                            PagerSettings-Mode="NumericFirstLast" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                            AllowSorting="True" DataSourceID="odsTaxClass" EmptyDataRowStyle-ForeColor="Red"
                                                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnRowDataBound="gvTaxClass_RowDataBound"
                                                            OnRowCommand="gvTaxClass_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        <asp:HiddenField ID="hdnTaxClassid" runat="server" Value='<%#Eval("TaxClassID") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Class" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        Tax Class
                                                                        <asp:ImageButton ID="lbtaxclass" runat="server" CommandArgument="DESC" CommandName="TaxName"
                                                                            AlternateText="Ascending Order" OnClick="Sorting" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("TaxName")%>
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
                                                                <asp:TemplateField HeaderText="Tax Code" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        Tax Code
                                                                        <asp:ImageButton ID="lbtaxcode" runat="server" CommandArgument="DESC" CommandName="TaxCode"
                                                                            OnClick="Sorting" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("TaxCode")%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Operations" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                            CommandArgument='<%# Eval("TaxClassID") %>'></asp:ImageButton>
                                                                        <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="DeleteTaxClass"
                                                                            CommandArgument='<%# Eval("TaxClassID") %>' message="Are you sure want to delete current Tax Class?"
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
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvTaxClass" />
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
