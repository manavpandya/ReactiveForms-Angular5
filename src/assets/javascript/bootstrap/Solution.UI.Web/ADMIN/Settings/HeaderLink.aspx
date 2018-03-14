﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="HeaderLink.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.HeaderLink"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-1.2.6.min.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {
            if (document.getElementById('<%=ddlstore.ClientID %>').selectedIndex == 0) {

                jAlert('Please select Store.', 'Message', '<%=ddlstore.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtheadername.ClientID %>').value == '') {

                jAlert('Please enter Header Name.', 'Message', '<%=txtheadername.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtheaderlink.ClientID %>').value == '') {

                jAlert('Please enter Header Link.', 'Message', '<%=txtheaderlink.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtdisplayorder.ClientID %>').value == '') {

                jAlert('Please enter Display Order.', 'Message', '<%=txtdisplayorder.ClientID %>');
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
                                                <img class="img-left" title="Add Header Link" alt="Add Header Link" src="/App_Themes/<%=Page.Theme %>/Images/header-links-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="Add Header Link" ID="lblTitle"></asp:Label></h2>
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
                                                        <asp:DropDownList ID="ddlstore" runat="server" Width="226px" CssClass="order-list">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 20%">
                                                        <span class="star">*</span>Header Name :
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:TextBox runat="server" ID="txtheadername" CssClass="order-textfield" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 20%">
                                                        <span class="star">*</span>Header Link :
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:TextBox runat="server" ID="txtheaderlink" CssClass="order-textfield" MaxLength="400"
                                                            Width="350px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 20%">
                                                        <span class="star">*</span>Display Order :
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:TextBox runat="server" ID="txtdisplayorder" CssClass="order-textfield" MaxLength="6"
                                                            Width="50px" onkeypress="return isNumberKey(event)"></asp:TextBox>
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
                                                        CausesValidation="true" OnClientClick="return Checkfields();" OnClick="btnSave_Click" />
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
