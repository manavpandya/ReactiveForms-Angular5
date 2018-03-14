<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductRomanShadeYardage.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductRomanShadeYardage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {
            if (document.getElementById('<%=txtShadeName.ClientID %>').value == '') {
                jAlert('Please Enter Shade Name.', 'Message', '<%=txtShadeName.ClientID %>');
                return false;
            }
            return true;
        }

        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
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
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                                    <img class="img-left" title="Product Roman Shade Yardage" alt="Product Roman Shade Yardage"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/state-list-icon.png" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Product Roman Shade Yardage" ID="lblTitle"></asp:Label></h2>
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
                                                    <tr>
                                                        <td colspan="2">
                                                            <center>
                                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label></center>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Store Name :
                                                        </td>
                                                        <td style="width: 80%; vertical-align: top">
                                                            <asp:DropDownList runat="server" ID="ddlStoreName" CssClass="add-product-list" Width="225px"
                                                                Height="20px" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Shade Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtShadeName" CssClass="order-textfield" MaxLength="300"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Add Standard Allowance [Width]:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAddWidthStandardAllowance" CssClass="order-textfield"
                                                                Width="80px" MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (12.00)
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Add Standard Allowance [Length]:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAddLengthStandardAllowance" CssClass="order-textfield"
                                                                Width="80px" MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (8.09)
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow" style="display:none;">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Fabric Per Yard Cost:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtfabricyard" CssClass="order-textfield" Width="80px"
                                                                MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (12.00)
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Manufacturing Cost:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtmenufacturercost" CssClass="order-textfield" Width="80px"
                                                                MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (12.00)
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Mechanism Option:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtMechanism" CssClass="order-textfield" Width="80px"
                                                                MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (8.00)
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Duties (%):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDuties" CssClass="order-textfield" Width="80px"
                                                                MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (10.45)
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Lined (%):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtLined" CssClass="order-textfield" Width="80px"
                                                                MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (10.45)
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Lined & Interlined (%):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtLinedInterlined" CssClass="order-textfield" Width="80px"
                                                                MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (6.85)
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Blackout Lining (%):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtBlackoutLining" CssClass="order-textfield" Width="80px"
                                                                MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (7.44)
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Active:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkActive" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Display Order:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDisplayOrder" Width="80px" CssClass="order-textfield"
                                                                MaxLength="4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                                            <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                OnClick="imgCancle_Click" />
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
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
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
