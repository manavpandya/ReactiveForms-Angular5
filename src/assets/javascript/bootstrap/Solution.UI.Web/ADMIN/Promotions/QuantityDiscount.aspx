<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="QuantityDiscount.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.QuantityDiscount1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .error
        {
            color: red;
        }
        .rferror
        {
            color: red;
        }
    </style>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <asp:ScriptManager ID="SM1" runat="server">
        </asp:ScriptManager>
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <span style="vertical-align: middle; margin-right: 3px; margin-top: 4px; float: left;">
                    <table>
                        <tr>
                            <td style="width: 25%" align="left">
                                Store :
                                <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"></span>
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
                                                    <img class="img-left" title="Quantity Discount List" alt="Quantity Discount List"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/quantity-discount-icon.png" />
                                                    <h2>
                                                        Quantity Discount</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 70%">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        Name :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtName" runat="server" CssClass="textfild" MaxLength="50"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtName"
                                                                            CssClass="rferror" ErrorMessage="Enter Name" SetFocusOnError="True" ValidationGroup="AddNew"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                        </td>
                                                        <td style="width: 10%">
                                                        </td>
                                                        <td style="width: 5%">
                                                        </td>
                                                        <td style="width: 5%; padding-right: 0px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row" id="gvdiscount">
                                            <td>
                                                <asp:GridView ID="gvListQuantityDiscount" runat="server" AutoGenerateColumns="false"
                                                    GridLines="None" Width="100%" BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7"
                                                    CellSpacing="1" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                    HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="left" OnRowDataBound="gvListQuantityDiscount_RowDataBound"
                                                    ViewStateMode="Enabled" ShowFooter="True" OnRowCommand="gvListQuantityDiscount_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Low Quantity" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="130px"
                                                            HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                Low Quantity
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLowQuantity" runat="server" Text='<%#Eval("LowQuantity")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                &nbsp;<asp:TextBox ID="txtAddLowQuantity" runat="server" ValidationGroup="AddNew"
                                                                    CssClass="order-textfield" Visible="true" Width="50px" onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddLowQuantity"
                                                                        SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="Enter LowQuantity"
                                                                        ValidationGroup="AddNew"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAddLowQuantity"
                                                                            SetFocusOnError="true" Display="dynamic" CssClass="error" ValidationGroup="AddNew"
                                                                            ErrorMessage="InValid" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="High Quantity" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="170px"
                                                            HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                High Quantity
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHighQuantity" runat="server" Text=' <%#Eval("HighQuantity")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ValidationGroup="AddNew" ID="txtAddHighQuantity" runat="server" CssClass="order-textfield"
                                                                    Visible="true" Width="50px" onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddHighQuantity"
                                                                        CssClass="error" ErrorMessage="Enter HighQuantity" SetFocusOnError="true" Display="dynamic"
                                                                        ValidationGroup="AddNew"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtAddHighQuantity"
                                                                            SetFocusOnError="true" Display="dynamic" ValidationGroup="AddNew" CssClass="error"
                                                                            ErrorMessage="InValid" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtAddHighQuantity" ControlToCompare="txtAddLowQuantity" Operator="GreaterThan" ErrorMessage="Low Quantity Should be Less than High quantity" Display="Dynamic" Enabled="True" SetFocusOnError="True" CssClass="error" Type="Integer"></asp:CompareValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Discount Percent" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                Discount Percent
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("DiscountPercent")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAddDiscount" runat="server" CssClass="order-textfield" Visible="true" MaxLength="5"
                                                                    Width="50px" ValidationGroup="AddNew" onkeypress="return  inputOnlyNumbers(event)"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddDiscount"
                                                                    SetFocusOnError="true" Display="dynamic" CssClass="error" ValidationGroup="AddNew"
                                                                    ErrorMessage="Enter Discount"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                        ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAddDiscount"
                                                                        SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="Invalid"
                                                                        ValidationGroup="AddNew" ValidationExpression="^(100(?:\.0{1,2})?|0*?\.\d{1,2}|\d{1,2}(?:\.\d{1,2})?)$"></asp:RegularExpressionValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <FooterTemplate>
                                                                <strong>
                                                                    <asp:LinkButton ID="hlAddNew" runat="server" ValidationGroup="AddNew" CommandName="Add New"
                                                                        CssClass="link-font" Style="color: rgb(29, 50, 79); text-decoration: underline"
                                                                        Text="Add New"></asp:LinkButton>
                                                                </strong>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="18%" />
                                                            <ItemTemplate>
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
                                        <tr class="altrow" id="txBottom">
                                            <td>
                                                <span style="float: right; padding-right: 10px;">
                                                    <%--   <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" OnClick="btnSave_Click" />--%>
                                                    <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" ImageUrl="~/Admin/images/save.jpg"
                                                        ValidationGroup="AddNew" />
                                                    <%-- <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click"
                CausesValidation="False" />--%>
                                                    <asp:ImageButton ID="btnCancel" CausesValidation="False" runat="server" OnClick="btnCancel_Click"
                                                        ImageUrl="/Admin/images/Cancel_button.gif" />
                                                </span>
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
