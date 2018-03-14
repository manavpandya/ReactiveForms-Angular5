<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CountryList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.configuration.ListCountry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtCountry.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtCountry.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function checkondelete(id) {
            jConfirm('Are you sure want to delete selected Country ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById("ContentPlaceHolder1_hdnDelete").value = id;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="/Admin/configuration/Country.aspx">
                    <img alt="Add Country" title="Add Country" src="/App_Themes/<%=Page.Theme %>/images/add-country.png" /></a></span>
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
                                            <th colspan="4">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Country List" alt="Country List" src="/App_Themes/<%=Page.Theme %>/Images/country-list-icon.png">
                                                    <h2>
                                                        Country List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right" width="80%">
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.CountryComponent" SelectMethod="GetDataByFilter"
                                                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="hdnCountry" DbType="String" DefaultValue="CountryID"
                                                            Name="CName" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:HiddenField ID="hdnCountry" runat="server" />
                                            </td>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 70%">
                                                        </td>
                                                        <td align="right" style="width: 10%">
                                                            Search&nbsp;:
                                                        </td>
                                                        <td width="10%" valign="middle">
                                                            <asp:TextBox ID="txtCountry" runat="server" CssClass="order-textfield" Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td width="5%">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return validation();" />
                                                        </td>
                                                        <td width="5%">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="4">
                                                <asp:GridView ID="grdCountry" runat="server" AutoGenerateColumns="False" DataKeyNames="CountryID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    CellPadding="2" CellSpacing="1" EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled"
                                                    OnRowCommand="grdCountry_RowCommand" Width="100%" GridLines="None" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                    DataSourceID="_gridObjectDataSource" OnRowDataBound="grdCountry_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <headertemplate>
                                                                Name
                                                                <asp:ImageButton ID="lbName" runat="server" CommandArgument="DESC" CommandName="tb_Country.Name"
                                                                    AlternateText="Ascending Order"  OnClick="Sorting" />
                                                            </headertemplate>
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <headertemplate>
                                                               Two Letter ISO Code
                                                                <asp:ImageButton ID="lbTLISO" runat="server" CommandArgument="DESC" CommandName="tb_Country.TwoLetterISOCode"
                                                                    AlternateText="Ascending Order"  OnClick="Sorting" />
                                                            </headertemplate>
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("TwoLetterISOCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Three Letter ISO Code
                                                                <asp:ImageButton ID="lbThLISO" runat="server" CommandArgument="DESC" CommandName="tb_Country.ThreeLetterISOCode"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("ThreeLetterISOCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("CountryID") %>'></asp:ImageButton>
                                                                <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="DeleteCountry"
                                                                    CommandArgument='<%# Eval("CountryID") %>' message='<%# Eval("CountryID") %>'
                                                                    OnClientClick='return checkondelete(this.getAttribute("message"));'></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <asp:Button ID="btnhdnDelete" runat="server" Text="Button" OnClick="btnhdnDelete_Click"
                                                    Style="display: none;" />
                                                <asp:HiddenField ID="hdnDelete" runat="server" Value="0" />
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
