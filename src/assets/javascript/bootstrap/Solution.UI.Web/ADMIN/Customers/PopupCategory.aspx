<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupCategory.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Customers.PopupCategory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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

        function checkCount(mode) {
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
                alert('Select at least one record !', 'Message');
                return false;
            }
            else {
                var Skus = "";
                var strSku = "";
                for (var i = 0; i < 20; i++) {
                    if (document.getElementById('grdProduct_txtDiscount_' + i)) {
                        if (document.getElementById('grdProduct_chkSelect_' + i).checked) {
                            var discount = parseFloat(document.getElementById('grdProduct_txtDiscount_' + i).value);
                            if (discount == 0) {
                                strSku = document.getElementById('grdProduct_lblProductName_' + i).innerHTML;
                                Skus = Skus + strSku + ', ';
                            }
                        }
                    }

                }
                if (Skus.length > 0) {
                    if (mode == 2) {
                        alert('Please enter discount for following Category(s): ' + Skus);
                    }
                    else {
                        alert('Please enter discount for following Category(s): ' + Skus);
                    }
                    return false;
                }
            }
            return true;
        }

        function refresh() {
            window.opener.location.href = window.opener.location.href;
            if (window.opener.progressWindow) {
                window.opener.progressWindow.close();
            }
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
    </script>
    <script type="text/javascript">
        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
                return true;
            return false;
        }

        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr id="trSearchItems" runat="server" style="display: none;">
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            <asp:Label ID="lblHeader" runat="server" Text=" Search Category(s)"></asp:Label>
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr id="trAddSeletedItems" runat="server" visible="false">
                    <td style="text-align: right; height: 30px; padding: 2px; padding-right: 4px;">
                        <asp:ImageButton ID="btnAddSelectedItems" runat="server" OnClick="btnAddSelectedItems_Click"
                            AlternateText="AddSelectedItems" />
                    </td>
                </tr>
                <tr id="trSelectedData" runat="server" align="center">
                    <td>
                        <div id="divselected" runat="server" style="font-size: 11px; color: #323232; font-family: Verdana,Arial,Helvetica,sans-serif;
                            padding-bottom: 4px; padding-top: 4px;">
                            <asp:GridView ID="grdSelected" runat="server" AutoGenerateColumns="False" CellPadding="0"
                                CellSpacing="1" GridLines="None" Width="99%" HeaderStyle-Height="20px" OnRowCommand="grdSelected_RowCommand"
                                OnRowDataBound="grdSelected_RowDataBound" CssClass="order-table" Style="border: solid 1px #dfdfdf;
                                border-right: none;">
                                <EmptyDataTemplate>
                                    <center>
                                        <span style="color: Red;">No Record(s) Found! </span>
                                    </center>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Product Name</HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewCategoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"CategoryId") %>'></asp:Label>
                                            <asp:Label ID="lblProductname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Productname") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="75%" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Product Discount</HeaderTemplate>
                                        <ItemTemplate>
                                            &nbsp;
                                            <asp:Label ID="lblCateDiscount" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"CategoryDiscount")), 2) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Remove</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btndel" runat="server" ToolTip="Delete Product" OnClientClick="javascript:if (confirm('Are you sure want to delete all selected Product(s)?')) { return true; } else { return false; }"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"RowNumber") %>' CommandName="delMe" />
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
                                                    </span>:
                                                </td>
                                                <td style="width: 320px;display:none;" valign="middle">
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select-box" DataTextField="Name"
                                                        DataValueField="CategoryID" AutoPostBack="false" Width="400px">
                                                    </asp:DropDownList>
                                                   
                                                </td>
                                                 <td style="width: 320px;" valign="middle">
                                                    <asp:TextBox ID="txtSearch" runat="server" Width="381px" Style="height: 18px;" CssClass="order-textfield"></asp:TextBox>
                                                </td>
                                                <td style="width: 400px;" valign="middle" align="right">
                                                    <asp:ImageButton Style="vertical-align: baseline; padding-bottom: 8px;" ID="btnGo"
                                                        runat="server" OnClientClick="return ChkSearch();" OnClick="btnGo_Click"  />
                                                    &nbsp;<asp:ImageButton Style="vertical-align: baseline; padding-bottom: 8px;" ID="btnShowAll"
                                                        runat="server" OnClientClick="if (document.getElementById('txtSearch') != null) {document.getElementById('txtSearch').value = '';}"
                                                        OnClick="btnShowAll_Click" AlternateText="ShowAll"  />
                                                    &nbsp;<asp:ImageButton ID="btnAddToSelectionlist" Style="margin-left: 50px;" runat="server"
                                                        OnClientClick="return checkCount();" OnClick="btnAddToSelectionlist_Click" Visible="false"
                                                        AlternateText="AddtoselectionList" />
                                                </td>
                                            </tr>
                                        </table>
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
                                            <asp:Label ID="lblCategoryID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <HeaderTemplate>
                                            &nbsp;Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" runat="server" Width="500px" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="50%" HorizontalAlign="left" />
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount(%)">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle />
                                        <ItemStyle />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscount" Text="0.00" runat="server" Width="70px" CssClass="order-textfield"
                                                Style="text-align: right" onkeypress="return keyRestrict(event,'0123456789.');"
                                                MaxLength="6"></asp:TextBox>
                                        </ItemTemplate>
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
