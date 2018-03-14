<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductSwatchSKU.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.ProductSwatchSKU" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search Product(s)</title>
    <script type="text/javascript">
        function ChkSearch() {
            if (document.getElementById('txtSearch') != null) {
                if ((document.getElementById('txtSearch').value).replace(/^\s*\s*$/g, '') == '') {
                    alert('Please enter search keyword');
                    document.getElementById('txtSearch').focus(); return false;
                }
            }
            return true;
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
            if (count == 0) {
                alert('Select at least one Product !', 'Message');
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript" language="javascript">
        var t;
        function selectAll(on) {
            var allElts = document.forms['form1'].elements;
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }
        function unselectcheckbox(chkelement) {
            var allElts = document.getElementById('rdolist').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {

                    if (elt.id != chkelement) {

                        elt.checked = false;
                    }
                }
            }
        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Search Product(s)
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr id="trAddSeletedItems" runat="server" visible="false">
                    <td style="text-align: right; height: 30px; padding: 2px; padding-right: 4px;">
                        <asp:ImageButton ID="btnAddSelectedItems" runat="server" OnClientClick="javascript:window.close();"
                            OnClick="btnAddSelectedItems_Click" />
                    </td>
                </tr>
                <tr id="trSelectedData" runat="server" visible="false" align="center">
                    <td>
                        <div id="divselected" runat="server" style="font-size: 11px; color: #323232; font-family: Verdana,Arial,Helvetica,sans-serif;
                            padding-bottom: 4px;">
                            <asp:GridView ID="grdSelected" runat="server" AutoGenerateColumns="False" CellPadding="0"
                                CellSpacing="1" GridLines="None" Width="99%" HeaderStyle-Height="20px" OnRowCommand="grdSelected_RowCommand"
                                OnRowDataBound="grdSelected_RowDataBound" CssClass="order-table" Style="border: solid 1px #dfdfdf;
                                border-right: none;">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Product Name</HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            &nbsp;
                                            <asp:Label ID="lblCat" ForeColor="Black" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"catname") %>'></asp:Label>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="75%" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Product Code</HeaderTemplate>
                                        <ItemTemplate>
                                            &nbsp;
                                            <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Remove</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btndel" runat="server" ToolTip="Delete Product" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'
                                                CommandName="delMe" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" Width="8%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle Height="18px" />
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="slidingDivImage" style="padding-top: 8px; padding-bottom: 8px;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table cellpadding="1" cellspacing="1" width="100%">
                                            <tr>
                                                <td style="width: 67px;">
                                                    <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">Search
                                                        by </span>:
                                                </td>
                                                <td style="width: 115px;">
                                                    <asp:DropDownList ID="ddlSearchby" runat="server" CssClass="order-list" Style="width: 115px;">
                                                        <asp:ListItem Text="Product Name" Value="name"></asp:ListItem>
                                                        <asp:ListItem Text="SKU" Value="SKU"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 320px;">
                                                    <asp:TextBox ID="txtSearch" runat="server" Width="381px" Style="height: 18px;" CssClass="textboxcommonstyle"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:ImageButton ID="btnGo" runat="server" OnClientClick="return ChkSearch();" OnClick="btnGo_Click" />
                                                    &nbsp;<asp:ImageButton ID="btnShowAll" runat="server" OnClientClick="if (document.getElementById('txtSearch') != null) {document.getElementById('txtSearch').value = '';}"
                                                        OnClick="btnShowAll_Click" />
                                                    &nbsp;<asp:ImageButton ID="btnAddToSelectionlist" runat="server" OnClientClick="return checkCount();"
                                                        OnClick="btnAddToSelectionlist_Click" Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr id="trPaging" runat="server" visible="false" style="height: 25px;">
                    <td style="border-top: solid 1px #e7e7e7">
                        <div id="divTopPaging" runat="server" style="width: 99%;">
                            <div style="float: left; font-weight: bold;">
                                <label id="lblTotProducts" runat="server">
                                </label>
                            </div>
                            <table cellpadding="0" cellspacing="0" align="right">
                                <tr>
                                    <td style="font-weight: bold">
                                        Pages :&nbsp;
                                        <asp:LinkButton ID="lnkTopprevious" runat="server" Font-Bold="true" ForeColor="#000"
                                            OnClick="lnkPrevious_Click" Width="62px">Previous |</asp:LinkButton>
                                    </td>
                                    <td style="font-weight: bold">
                                        <asp:DataList ID="RepeaterPaging" runat="server" HorizontalAlign="Right" RepeatDirection="Horizontal"
                                            OnItemCommand="RepeaterPaging_ItemCommand" CssClass="grid_paging" OnItemDataBound="RepeaterPaging_ItemDataBound">
                                            <ItemStyle ForeColor="#000000" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="Pagingbtn" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="newpage" Text='<%# Eval("PageText") %> '></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                    <td style="font-weight: bold">
                                        &nbsp;<asp:LinkButton ID="lnktopNext" runat="server" Font-Bold="true" ForeColor="#000"
                                            OnClick="lnkNext_Click" Width="40px">  | Next </asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 2px">
                        <div id="rdolist" style="border: 5px sollid #e7e7e7;">
                            <asp:GridView ID="grdProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                                class="order-table" Style="border: solid 1px #e7e7e7" OnRowDataBound="grdProduct_RowDataBound">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <label style="color: Red">
                                        No records found ...</label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                            <asp:Label ID="lblProductID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <HeaderTemplate>
                                            &nbsp;Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="50%" HorizontalAlign="left" />
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <HeaderTemplate>
                                            &nbsp; SKU
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%" HorizontalAlign="left" />
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <HeaderTemplate>
                                            Inventory
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInventory" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <HeaderTemplate>
                                            Price&nbsp;
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#    string.Format("{0:F}", DataBinder.Eval(Container.DataItem, "Price"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" HorizontalAlign="right" />
                                        <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr id="trCheckClearAll" runat="server" visible="false">
                    <td style="text-align: left; color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                        font-size: 11px; padding-top: 10px;" colspan="2" id="cleartdid" runat="server">
                        <a style="color: #696A6A; text-decoration: none;" id="lkbAllowAll" class="list_table_cell_link"
                            href="javascript:selectAll(true);">Check All</a>&nbsp; | <a id="lkbClearAll" style="color: #696A6A;
                                text-decoration: none;" class="list_table_cell_link" href="javascript:selectAll(false);">
                                Clear All</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
