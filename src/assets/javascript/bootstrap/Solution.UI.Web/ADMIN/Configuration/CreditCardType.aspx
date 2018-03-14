<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CreditCardType.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.CreditCardType"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-1.2.6.min.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {
            if (document.getElementById("ContentPlaceHolder1_ddlStore").selectedIndex == 0) {
                jAlert('Please select Store.', 'Message', 'ContentPlaceHolder1_ddlStore');
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtcredit").value == '') {
                jAlert('Please enter Credit Card Type.', 'Message', 'ContentPlaceHolder1_txtcredit');
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtcode").value == '') {
                jAlert('Please enter Credit Card Code.', 'Message', 'ContentPlaceHolder1_txtcode');
                return false;
            }
            return true;
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
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
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
                                                <img class="img-left" title="Add Credit Card Type" alt="Add Credit Card Type" src="/App_Themes/<%=Page.Theme %>/Images/credit-card-list-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="Add Credit Card Type" ID="lblTitle"></asp:Label></h2>
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
                                                    <td style="width: 20%">
                                                        <span class="star">*</span>Store Name :
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="226px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 20%">
                                                        <span class="star">*</span>Credit Card Type :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtcredit" CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 20%">
                                                        <span class="star">*</span>Card Code :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtcode" CssClass="order-textfield" MaxLength="10"
                                                            onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 20%">
                                                        <span class="star">&nbsp;&nbsp;</span>Status :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkststus" Text=" Active" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <td>
                                                </td>
                                                <td style="width: 80%">
                                                    <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                        CausesValidation="true" OnClick="btnSave_Click" OnClientClick="return Checkfields();" />
                                                    &nbsp;<asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                        OnClick="btnCancel_Click" />
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
            <tr>
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" alt="" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
