<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Vendorsku.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Vendorsku"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function Checkvalidation() {

            if (document.getElementById("ContentPlaceHolder1_ddlvendor") != null && document.getElementById("ContentPlaceHolder1_ddlvendor").selectedIndex == 0) {
                alert('Please select DropShipper.');
                document.getElementById("ContentPlaceHolder1_ddlvendor").focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtvendorSku') != null && document.getElementById('ContentPlaceHolder1_txtvendorSku').value == '') {
                alert('Please enter DropShipper SKU.');
                document.getElementById('ContentPlaceHolder1_txtvendorSku').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtProductName') != null && document.getElementById('ContentPlaceHolder1_txtProductName').value == '') {
                alert('Please enter Product Name.');
                document.getElementById('ContentPlaceHolder1_txtProductName').focus();
                return false;
            }


            return true;
        }
    </script>
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
                                                    <img class="img-left" title="Add DropShipper SKU" alt="Add DropShipper SKU" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add DropShipper SKU"></asp:Label></h2>
                                                </div>
                                            </th>
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
                                                        <td valign="top" width="10%">
                                                            <span class="star">*</span>DropShipper:
                                                        </td>
                                                        <td width="90%">
                                                            <asp:DropDownList ID="ddlvendor" runat="server" CssClass="order-list" Width="300px"
                                                                AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top" width="10%">
                                                            <span class="star">*</span>SKU:
                                                        </td>
                                                        <td width="90%">
                                                            <asp:TextBox runat="server" ID="txtvendorSku" CssClass="order-textfield" Width="250px"
                                                                MaxLength="100"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top" width="10%">
                                                            <span class="star">*</span>Product Name:
                                                        </td>
                                                        <td width="90%">
                                                            <asp:TextBox ID="txtProductName" runat="server" MaxLength="500" CssClass="order-textfield"
                                                                Width="500px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="altrow">
                                <td>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <td align="left" style="width: 13.5%">
                                        </td>
                                        <td style="width: 86.5%">
                                            <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                OnClick="btnSave_Click" ValidationGroup="Vendors" Style="padding-top: 4px; padding-bottom: 4px"
                                                OnClientClick="return Checkvalidation();" />
                                            <asp:ImageButton ID="btnCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                OnClick="btnCancle_Click" CausesValidation="false" Style="padding-top: 4px; padding-bottom: 4px" />
                                        </td>
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
