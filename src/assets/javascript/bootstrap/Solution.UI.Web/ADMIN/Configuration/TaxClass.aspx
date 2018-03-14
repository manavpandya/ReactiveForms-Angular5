<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="TaxClass.aspx.cs" Inherits="Solution.UI.Web.ADMIN.TaxManagement.TaxClass"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {

            if (document.getElementById('<%=ddlStore.ClientID %>').selectedIndex == 0) {

                jAlert('Please Select Store.', 'Message', '<%=ddlStore.ClientID %>');
                return false;
            }

            else if (document.getElementById('<%=txtTaxClass.ClientID %>').value == '') {

                jAlert('Please enter Tax Class.', 'Message', '<%=txtTaxClass.ClientID %>');
                return false;
            }

            else if (document.getElementById('<%=txtTaxCode.ClientID %>').value == '') {

                jAlert('Please enter Tax Code.', 'Message', '<%=txtTaxCode.ClientID %>');
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
    <div id="content-width">
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
                                                    <img class="img-left" title="Add Tax Class" alt="Add Tax Class" src="/App_Themes/<%=Page.Theme %>/Images/tax-class-list-icon.png" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Tax Class" ID="lblTitle"></asp:Label></h2>
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
                                                        <td>
                                                            <span class="star">*</span>Store Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="185px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Tax Class:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtTaxClass" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Tax Code:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtTaxCode" CssClass="order-textfield" MaxLength="50"
                                                                onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Display Order:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDisplayOrder" CssClass="order-textfield" MaxLength="5"
                                                                onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td align="left" valign="top">
                                                        </td>
                                                        <td align="left">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <td style="width: 80%; padding-left: 0px;">
                                                                    <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                        OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                                                    &nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                        OnClick="imgCancel_Click" />
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
