<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerLevel.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.CustomerLevel"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
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
                                                    <img class="img-left" title="Add Customer Level" alt="Add Customer Level" src="/App_Themes/<%=Page.Theme %>/Images/customer-level-list-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add Customer Level"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" valign="middle" style="text-align: center">
                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">&nbsp;&nbsp;</span>Store Name:
                                                        </td>
                                                        <td style="width: 80%; vertical-align: top">
                                                            <asp:DropDownList runat="server" ID="drpStoreName" CssClass="add-product-list" Width="190px"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtName" CssClass="order-textfield" MaxLength="50"
                                                                Width="300px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="txtName"
                                                                CssClass="error" ErrorMessage="Enter Name" SetFocusOnError="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Discount Percent:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDiscPerc" CssClass="order-textfield" Width="300px" MaxLength="5"></asp:TextBox>
                                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtDiscPerc"
                                                                Display="Dynamic" CssClass="rferror" ErrorMessage="Invalid Input" Operator="DataTypeCheck"
                                                                SetFocusOnError="True" Type="Currency" ValueToCompare="^(100(?:\.0{1,2})?|0*?\.\d{1,2}|\d{1,2}(?:\.\d{1,2})?)$"
                                                                ForeColor="Red"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;OR
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>Discount Amount:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDiscAmount" CssClass="order-textfield" Width="300px" MaxLength="10"></asp:TextBox>
                                                            <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="txtDiscAmount"
                                                                Display="Dynamic" CssClass="rferror" ErrorMessage="Invalid Input" Operator="DataTypeCheck"
                                                                SetFocusOnError="True" Type="Currency" ValueToCompare="^(100(?:\.0{1,2})?|0*?\.\d{1,2}|\d{1,2}(?:\.\d{1,2})?)$"
                                                                ForeColor="Red"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>Has Free Shipping:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkHasFreeShipping" runat="server" Style="border: 0px;" CssClass="select_box"
                                                                Text="  Has Free Shipping" ToolTip="Has Free Shipping" Width="127px" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Has No Tax:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkHasnoTax" runat="server" Style="border: 0px;" CssClass="select_box"
                                                                Text="  Has No Tax" ToolTip="Has No Tax" Width="91px" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>Display Order:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="order-textfield" Width="73px"
                                                                MaxLength="5"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="REVOrder" runat="server" ControlToValidate="txtDisplayOrder"
                                                                CssClass="error" ErrorMessage="Order must be Numeric Value" SetFocusOnError="True"
                                                                ToolTip="Order must be Numeric Value" ValidationExpression="^\d+$" ForeColor="Red"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <td style="width: 12%">
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClick="btnSave_Click" />
                                                        <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                                    </td>
                                                </table>
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
