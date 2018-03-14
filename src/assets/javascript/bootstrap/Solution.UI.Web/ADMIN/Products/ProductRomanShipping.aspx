<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductRomanShipping.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductRomanShipping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">

        function checkondelete(uniqueID) {

            jConfirm('Are you sure want to delete ?', 'Confirmation', function (r) {
                if (r == true) {
                    __doPostBack(uniqueID, '');
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
                $(document).ready(function () { jAlert('Check atleast one Feature!', 'Message'); });
                return false;
            }
            else {
                return confirm('Are you sure want to delete all Feature ?', 'Message');
            }
        }

    </script>
    <script runat="server">
        string _ReturnUrl;
        public string SetImage(bool _Value)
        {
            if (_Value == true)
            {
                _ReturnUrl = "../Images/active.gif";
            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";
            }
            return _ReturnUrl;
        }
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 4px; float: left; display: none;">
                        <tr>
                            <td align="right">
                                Store :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStore" runat="server" Width="180px" AutoPostBack="true"
                                    CssClass="order-list">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;">
                    <asp:ImageButton ID="ImgTagAddromanshipping" runat="server" Style="padding-top: 6px;"
                        OnClick="ImgTagAddromanshipping_Click" /></span>
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
                                                    <img class="img-left" title="Feature Template List" alt="Feature Template List" src="/App_Themes/<%=Page.Theme %>/Images/emailTemplate.png">
                                                    <h2>
                                                        Roman Shipping</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdshipping" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                    AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" OnRowCommand="grdshipping_RowCommand" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                    OnRowDataBound="grdshipping_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                From Width
                                                                <asp:ImageButton ID="btnFromwidth" runat="server" CommandArgument="DESC" CommandName="Fromwidth"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" Visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFromwidth" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Fromwidth")))%>'></asp:Label>
                                                               
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                To Width
                                                                <asp:ImageButton ID="btnTowidth" runat="server" CommandArgument="DESC" CommandName="Towidth"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" Visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTowidth" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Towidth")))%>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Cost
                                                                <asp:ImageButton ID="btnCost" runat="server" CommandArgument="DESC" CommandName="Cost"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" Visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                $<asp:Label ID="lblCost" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Cost")))%>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Active
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("RomanshippingId") %>'></asp:ImageButton>
                                                                <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="delete"
                                                                    message='<%# Eval("RomanshippingId") %>' CommandArgument='<%# Eval("RomanshippingId") %>'
                                                                    OnClientClick="return checkondelete(this.name);"></asp:ImageButton>
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
