<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="EmailTemplateList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.EmailTemplateList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message'); });
                document.getElementById('<%=txtSearch.ClientID %>').focus();
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
                $(document).ready(function () { jAlert('Check atleast one Email Template!', 'Message'); });
                return false;
            }
            else {
                return confirm('Are you sure want to delete all selected Vendor(s)?', 'Message');
            }
        }

        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 4px; float: left">
                        <tr>
                            <td align="right">
                                Store :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStore" runat="server" Width="180px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="EmailTemplate.aspx">
                    <img alt="Add EmailTemplate" title="Add EmailTemplate" src="/App_Themes/<%=Page.Theme %>/images/add-email-template.png" /></a></span>
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
                                                    <img class="img-left" title="EmailTemplate List" alt="EmailTemplate List" src="/App_Themes/<%=Page.Theme %>/Images/emailTemplate.png">
                                                    <h2>
                                                        Email Templates</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.EmailTemplateComponent" SelectMethod="GetDataByFilter"
                                                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlSearch" DbType="String" Name="SearchBy" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="SearchValue" />
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="StoreID" DefaultValue="" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <table>
                                                    <tr>
                                                        <td style="width: 70%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            <asp:DropDownList ID="ddlSearch" AutoPostBack="False" Width="110px" runat="server" CssClass="order-list">
                                                                <asp:ListItem>Label</asp:ListItem>
                                                                <asp:ListItem>Subject</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtSearch" CssClass="order-textfield" runat="server" Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:Button ID="btnSearch" runat="server" OnClientClick="return validation();" OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td style="width: 5%; padding-right: 0px;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdEmailTemplate" runat="server" AutoGenerateColumns="False" DataKeyNames="TemplateID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnRowCommand="grdEmailTemplate_RowCommand"
                                                    BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                    GridLines="None" Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                    PagerSettings-Mode="NumericFirstLast" DataSourceID="_gridObjectDataSource" OnRowDataBound="grdEmailTemplate_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Label
                                                                <asp:ImageButton ID="btnLabel" runat="server" CommandArgument="DESC" CommandName="Label"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLabel" runat="server" Text='<%# Bind("Label") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField>
                                                            <HeaderTemplate>
                                                                EMail To
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="LabelEmailTo" runat="server" Text='<%# Bind("EmailTo") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                EMail From
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="LabelEmailFrom" runat="server" Text='<%# Bind("EmailFrom") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                EMail CC
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="LabelEmailCC" runat="server" Text='<%# Bind("EmailCC") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Subject
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="LabelSubject" runat="server" Text='<%# Bind("Subject") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Store Name
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="LabelStore" runat="server" Text='<%# Bind("StoreName") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("TemplateID") %>'></asp:ImageButton>
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
