<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddManualVendorQuote.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.AddManualVendorQuote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
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

            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 104)
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
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
                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
                                        style="padding: 2px;">
                                        <tbody>
                                            <tr>
                                                <th>
                                                    <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 0px;">
                                                        Generate Vendor Quote
                                                    </div>
                                                    <div class="main-title-right">
                                                        &nbsp;
                                                    </div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px; text-align: right;">
                                                    <a href="WareHousePO.aspx" style="margin-right: 30px;">
                                                        <img src="/App_Themes/<%=Page.Theme %>/images/back.png" alt="Go to Warehouse PO"
                                                            title="Go to Warehouse PO" />
                                                    </a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px">
                                                    <div id="rdolist" style="border: 5px sollid #e7e7e7;">
                                                        <asp:GridView ID="grdCart" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            class="order-table" Style="border: solid 1px #e7e7e7" OnRowDataBound="grdCart_RowDataBound">
                                                            <EmptyDataRowStyle HorizontalAlign="Center" />
                                                            <EmptyDataTemplate>
                                                                <label style="color: Red; text-align: center;">
                                                                    No Record(s) Found !</label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        <asp:Label ID="lblProductID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                        <asp:Label ID="lblQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <HeaderTemplate>
                                                                        &nbsp;Name
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                        <br />
                                                                        &nbsp;Options :
                                                                        <br />
                                                                        <asp:TextBox TextMode="MultiLine" ID="txtProductOption" Style="margin-left: 5px;
                                                                            margin-top: 5px; width: 250px; border: 1px solid rgb(218, 218, 218);" runat="Server"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="40%" HorizontalAlign="left" />
                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <HeaderTemplate>
                                                                        &nbsp; SKU
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Quantity">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtQuantity" Text='<%#DataBinder.Eval(Container.DataItem,"Inventory") %>'
                                                                            runat="server" Width="70px" Style="text-align: center" MaxLength="6" onkeypress="return keyRestrict(event,'0123456789');"
                                                                            class="order-textfield"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        Available Quantity
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAvailableQuantity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"AvailableQuantity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                                    <HeaderTemplate>
                                                                        Your Price<br />
                                                                        (per unit)
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPrice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                            Text='<%# String.Format("{0:0.00}",Convert.ToDouble(Convert.ToString(DataBinder.Eval(Container.DataItem,"Price")))) %>'
                                                                            CssClass="order-textfield" MaxLength="10" runat="server" Style="width: 60px;
                                                                            text-align: right;"></asp:TextBox>
                                                                        <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# String.Format("{0:0.00}",Convert.ToDouble(Convert.ToString(DataBinder.Eval(Container.DataItem,"Price")))) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle HorizontalAlign="center" />
                                                                    <HeaderTemplate>
                                                                        Available<br />
                                                                        [in Days]
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="drpAvailDays" runat="server" CssClass="order-list" Width="45px">
                                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="3 " Value="3"></asp:ListItem>
                                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                            <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px; border: 1px solid #e7e7e7;">
                                                    <table cellpadding="1" cellspacing="2" width="100%">
                                                        <tr>
                                                            <td style="width: 100px;">
                                                                Vendor :
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="order-list" Width="250px"
                                                                    AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td>
                                                                <asp:Label ID="lblmailtemplate" runat="server" Text="Mail Template :"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMailTemplate" runat="server" CssClass="order-list" Width="250px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>--%>
                                                        <%--<tr>
                                                            <td valign="top" style="width: 120px;">
                                                                <asp:Label ID="lblspeicalinstructions" runat="server" Text="Special Instructions:"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td class="ckeditor-table">
                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                Width="400px" Height="60px" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td>
                                                                Location :
                                                            </td>
                                                            <td align="left" style="padding-bottom: 10px;">
                                                                <asp:TextBox ID="txtLocation" runat="server" class="checkout-textfild" TextMode="MultiLine"
                                                                    Width="400px" Height="60px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Notes :
                                                            </td>
                                                            <td align="left" style="padding-bottom: 10px;">
                                                                <asp:TextBox ID="txtNotes" runat="server" class="checkout-textfild" TextMode="MultiLine"
                                                                    Width="400px" Height="60px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblLinks" runat="server" CssClass="error" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                            </td>
                                                            <td align="left" style="padding-bottom: 10px;">
                                                                <asp:ImageButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
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
