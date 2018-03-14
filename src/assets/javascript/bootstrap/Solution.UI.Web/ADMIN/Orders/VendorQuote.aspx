<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="VendorQuote.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.VendorQuote" %>

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
                                                    <center>
                                                        <span style="color: Red; padding-top: 10px;font-weight:bold;">Note : Vendors without Email Address will
                                                            not be listed!</span></center>
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
                                                                    <HeaderStyle Width="30%" HorizontalAlign="left" />
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
                                                                    <HeaderStyle Width="8%" HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtQuantity" Text='<%#DataBinder.Eval(Container.DataItem,"Inventory") %>'
                                                                            runat="server" Width="70px" Style="text-align: center" MaxLength="6" onkeypress="return keyRestrict(event,'0123456789');"
                                                                            class="order-textfield"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Price">
                                                                    <HeaderTemplate>
                                                                        Price&nbsp;
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        $<%# string.Format("{0:F}", DataBinder.Eval(Container.DataItem, "Price"))%>
                                                                        <asp:Label ID="lblPrice" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Price") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="8%" HorizontalAlign="right" />
                                                                    <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        Available Quantity
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAvailableQuantity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"AvailableQuantity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="center" Width="8%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        Vendor
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBoxList ID="chkVendor" RepeatDirection="horizontal" runat="server" DataTextField="Name"
                                                                            DataValueField="VendorID" RepeatColumns="3">
                                                                        </asp:CheckBoxList>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
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
                                                        <%--  <tr>
                                                            <td style="width: 100px;">
                                                                Vendor :
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="order-list" Width="250px"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblmailtemplate" runat="server" Text="Mail Template :"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMailTemplate" runat="server" CssClass="order-list" Width="250px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
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
