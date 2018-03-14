<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ImageSize.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.ImageSize"
    MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td align="left" valign="top" style="height: 5px">
                        <img width="1" alt="" height="5" src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" />
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top" style="height: 5px">
                        <img width="1" alt="" height="5" src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" />
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table class="content-table" width="100%" cellpadding="0" cellspacing="0" border="0"
                            style="background-color: #FFFFFF">
                            <tr>
                                <td class="border-td-sub">
                                    <table class="add-product" width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img src="/App_Themes/<%= Page.Theme %>/Images/image-configuration-icon.png" alt="Image Configuration"
                                                        title="Image Configuration" class="img-left" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Image Configuration" ID="lblTitle"></asp:Label>
                                                    </h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="2" align="center">
                                                <center>
                                                    <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label></center>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" class="add-product" cellpadding="0" cellspacing="0" border="0">
                                                    <tr class="oddrow">
                                                        <td style="width: 25%;">
                                                            Store Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpstorename" Width="205px" runat="server" AutoPostBack="true"
                                                                OnSelectedIndexChanged="drpstorename_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:LinkButton ID="lnknewstore" runat="server" Visible="false">Creat new Store?</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <b>Image Type:</b>
                                                        </td>
                                                        <td>
                                                            <table width="220px">
                                                                <tr>
                                                                    <td style="width: 80px; text-align: center; padding: 0px; font-weight: bold;">
                                                                        Width (px)
                                                                    </td>
                                                                    <td style="width: 60px; text-align: center; font-weight: bold;">
                                                                        X
                                                                    </td>
                                                                    <td style="width: 80px; text-align: center; padding: 0px; font-weight: bold;">
                                                                        Height (px)
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Product Icon :
                                                        </td>
                                                        <td>
                                                            <table width="220px">
                                                                <tr>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtProductIconwidth" MaxLength="5" CssClass="order-textfield" runat="server"
                                                                            Style="width: 60px;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                                                            ForeColor="Red" ControlToValidate="txtProductIconwidth" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td style="width: 60px; text-align: center;">
                                                                        X
                                                                    </td>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtProductIconHeight" MaxLength="5" runat="server" CssClass="order-textfield"
                                                                            Style="width: 60px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtProductIconHeight" SetFocusOnError="true" ValidationGroup="Update"
                                                                            ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Product Medium :
                                                        </td>
                                                        <td>
                                                            <table width="220px">
                                                                <tr>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtProductMediumWidth" MaxLength="5" CssClass="order-textfield"
                                                                            Width="60px" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtProductMediumWidth"
                                                                            SetFocusOnError="true" ValidationGroup="Update" ForeColor="Red" runat="server"
                                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td style="width: 60px; text-align: center;">
                                                                        X
                                                                    </td>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtProductMediumHeight" MaxLength="5" CssClass="order-textfield"
                                                                            runat="server" onkeypress="return isNumberKey(event)" Width="60px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtProductMediumHeight"
                                                                            SetFocusOnError="true" ValidationGroup="Update" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td align="left" valign="top">
                                                            <span class="star">*</span>Product Large :
                                                        </td>
                                                        <td align="left" style="padding: 3px;">
                                                            <table width="220px">
                                                                <tr>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtProductLargeWidth" MaxLength="5" CssClass="order-textfield" runat="server"
                                                                            Style="width: 60px;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtProductLargeWidth" ForeColor="Red" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td style="width: 60px; text-align: center;">
                                                                        X
                                                                    </td>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtProductLargeHeight" MaxLength="5" runat="server" CssClass="order-textfield"
                                                                            Style="width: 60px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtProductLargeHeight" ForeColor="Red" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" valign="top">
                                                            <span class="star">*</span>Product Micro :
                                                        </td>
                                                        <td align="left" style="padding: 3px;">
                                                            <table width="220px">
                                                                <tr>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtProductMicroWidth" MaxLength="5" CssClass="order-textfield" runat="server"
                                                                            Style="width: 60px;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtProductMicroWidth" ForeColor="Red" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td style="width: 60px; text-align: center;">
                                                                        X
                                                                    </td>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtProductMicroHeight" MaxLength="5" runat="server" CssClass="order-textfield"
                                                                            Style="width: 60px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtProductMicroHeight" ForeColor="Red" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td align="left" valign="top">
                                                            <span class="star">*</span>Category Icon :
                                                        </td>
                                                        <td align="left" style="padding: 3px;">
                                                            <table width="220px">
                                                                <tr>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtCategoryIconWidth" MaxLength="5" CssClass="order-textfield" runat="server"
                                                                            Style="width: 60px;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtCategoryIconWidth" ForeColor="Red" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td style="width: 60px; text-align: center;">
                                                                        X
                                                                    </td>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtCategoryIconHeight" MaxLength="5" runat="server" CssClass="order-textfield"
                                                                            onkeypress="return isNumberKey(event)" Width="60px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtCategoryIconHeight" ForeColor="Red" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" valign="top">
                                                            <span class="star">*</span>Category Micro :
                                                        </td>
                                                        <td align="left" style="padding: 3px;">
                                                            <table width="220px">
                                                                <tr>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtCategoryMicroWidth" MaxLength="5" CssClass="order-textfield" runat="server"
                                                                            Style="width: 60px;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtCategoryMicroWidth" ForeColor="Red" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td style="width: 60px; text-align: center;">
                                                                        X
                                                                    </td>
                                                                    <td style="width: 80px; text-align: center; padding: 0px;">
                                                                        <asp:TextBox ID="txtCategoryMicroHeight" MaxLength="5" runat="server" CssClass="order-textfield"
                                                                            Style="width: 60px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                                                                            ControlToValidate="txtCategoryMicroHeight" ForeColor="Red" SetFocusOnError="true"
                                                                            ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" valign="top">
                                                        </td>
                                                        <td align="left" style="padding: 3px;">
                                                            <table width="220px">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="btnsave" runat="server" ValidationGroup="Update" AlternateText="Save"
                                                                            ToolTip="Save" OnClick="btnsave_Click" />
                                                                        &nbsp;<asp:ImageButton ID="btnclose" runat="server" AlternateText="Close" ToolTip="Cancel"
                                                                            OnClick="btnclose_Click" />
                                                                    </td>
                                                                </tr>
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
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" alt="" />
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
