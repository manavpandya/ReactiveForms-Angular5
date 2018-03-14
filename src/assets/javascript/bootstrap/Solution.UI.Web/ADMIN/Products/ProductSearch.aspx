<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductSearch.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.ProductSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Product</title>
    <script type="text/javascript">
        function ShowVariantdiv() {
            if (document.getElementById('divvariant')) {
                document.getElementById('divvariant').style.display = '';
            }
            if (document.getElementById('divvariant1')) {
                document.getElementById('divvariant1').style.display = '';
            }
            return false;
        }
        function HideVariantdiv(id) {
            if (document.getElementById(id)) {
                document.getElementById(id).style.display = 'none';
            }
            if (document.getElementById('divvariant1')) {
                document.getElementById('divvariant1').style.display = 'none';
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
    <script type="text/javascript">
        function ProductValidation() {
            if (document.getElementById('txtProductName') != null) {
                if ((document.getElementById('txtProductName').value).replace(/^\s*\s*$/g, '') == '') {
                    alert('Please Enter Product Name');
                    ShowVariantdiv();
                    document.getElementById('txtProductName').focus(); return false;
                }
            }
            if (document.getElementById('txtPrice') != null) {
                if ((document.getElementById('txtPrice').value).replace(/^\s*\s*$/g, '') == '') {
                    alert('Please Enter Price');
                    ShowVariantdiv();
                    document.getElementById('txtPrice').focus(); return false;
                }
            }
            return true;
        }

        function ChkSearch() {
            if (document.getElementById('txtSearch') != null) {
                if ((document.getElementById('txtSearch').value).replace(/^\s*\s*$/g, '') == '') {
                    alert('Please enter search keyword');
                    document.getElementById('txtSearch').focus(); return false;
                }
            }
            return true;
        }

        function chkselect(ids) {
            var allElts = document.getElementById("rdolist").getElementsByTagName('input');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "radio") {
                    if (elt.id == ids) {

                        elt.checked = true;
                    }
                    else {
                        elt.checked = false;
                    }
                }
            }
            return true;
        }
    </script>
</head>
<body style="background: none">
    <form id="form1" runat="server">
    <div style="padding: 4px;">
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Search Product
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td>
                        <div class="slidingDivImage" style="padding-top: 10px">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr style="display: none">
                                    <td style="padding: 5px">
                                        <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">Category
                                        </span>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCategory" runat="server" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">Manufacturer
                                        </span>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlManufacture" runat="server" class="product-type" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="padding-top: 10px">
                                    <td style="padding: 5px; width: 70px;">
                                        <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">Search
                                            By </span>:
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:DropDownList ID="ddlSearchby" runat="server" CssClass="order-list" Style="width: 110px;">
                                            <asp:ListItem Text="Product Name" Value="name"></asp:ListItem>
                                            <asp:ListItem Text="SKU" Value="SKU"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 360px;">
                                        <asp:TextBox ID="txtSearch" Width="350px" CssClass="order-textfield" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnGo" runat="server" OnClientClick="return ChkSearch();" OnClick="btnGo_Click" />
                                        &nbsp;<asp:ImageButton ID="btnAddOptionProduct" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr style="border-top: solid 1ox #e7e7e7">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="position: absolute; height: 770px; opacity: 0.9; filter: alpha(opacity=90);
                            background-color: #000000; width: 100%; z-index: 10000; display: none;" id="divvariant">
                        </div>
                        <div style="float: left; width: 100%; position: absolute; margin-left: 10px; display: none;
                            z-index: 1000000" id="divvariant1">
                            <table width="70%" align="center" style="margin-top: 50px;">
                                <tr>
                                    <td align="center" style="background-color: #fff;" valign="middle">
                                        <div style="width: 100%; background-color: #fbfbfb; right: 70px;">
                                            <div style="float: left; background-color: #707070; min-height: 30px; width: 93%;
                                                text-align: left; color: #fff;">
                                                <div style="padding-top: 10px; padding-left: 5px; font-weight: bold;">
                                                    Add Option Product</div>
                                            </div>
                                            <div style="float: right; background-color: #707070; height: 25px; width: 7%; padding-top: 5px;">
                                                <a href="javascript:void(0);" id="divinnerclose" runat="server" onclick="HideVariantdiv('divvariant');">
                                                    <img id="img1" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a></div>
                                            <div style="float: left; width: 100%; margin-left: 10px;">
                                                <table align="left">
                                                    <tr>
                                                        <td align="left" style="font-weight: bold;">
                                                            <span class="star">*</span>Product&nbsp;Name:
                                                        </td>
                                                        <td align="left" style="color: #ff0000; font-weight: bold;">
                                                            <asp:TextBox ID="txtProductName" runat="server" Width="430px" class="order-textfield"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="font-weight: bold;">
                                                            <span class="star">*</span>Price:
                                                        </td>
                                                        <td align="left" style="color: #ff0000; font-weight: bold;">
                                                            <asp:TextBox ID="txtPrice" class="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                runat="server" Width="100px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="font-weight: bold;">
                                                            <span class="star">&nbsp;</span>SKU:
                                                        </td>
                                                        <td align="left" style="color: #ff0000; font-weight: bold;">
                                                            <asp:Label ID="lblProductSku" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnSelectGrid" runat="server" OnClientClick="return ProductValidation();"
                                                                OnClick="btnSelectGrid_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="rdolist" style="border: 1px;">
                            <asp:GridView ID="grdProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                                BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                BorderStyle="Solid">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <label style="color: Red">
                                        No records found ...</label>
                                </EmptyDataTemplate>
                                <FooterStyle CssClass="content-table" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <input type="radio" name="rdchecknm" id="rdcheckid" runat="server" onclick="return chkselect(this.id);"
                                                enableviewstate="true" />
                                            <asp:Label ID="lblProductID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" ForeColor="White" HorizontalAlign="Center" />
                                        <ItemStyle VerticalAlign="top" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <HeaderTemplate>
                                            &nbsp;Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="50%" ForeColor="White" HorizontalAlign="left" />
                                        <ItemStyle VerticalAlign="top" HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <HeaderTemplate>
                                            &nbsp; SKU
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="20%" ForeColor="White" HorizontalAlign="left" />
                                        <ItemStyle VerticalAlign="top" HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <HeaderTemplate>
                                            Inventory
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInventory" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" ForeColor="White" HorizontalAlign="Center" />
                                        <ItemStyle VerticalAlign="top" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <HeaderTemplate>
                                            Price&nbsp;
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#    string.Format("{0:F}", DataBinder.Eval(Container.DataItem, "Price"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" ForeColor="White" HorizontalAlign="right" />
                                        <ItemStyle HorizontalAlign="right" VerticalAlign="top" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr style="border-top: solid 1ox #e7e7e7">
                    <td style="text-align: center; padding: 5px;">
                        <asp:ImageButton ID="btnSave" runat="server" Visible="false" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
