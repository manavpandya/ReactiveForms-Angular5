<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ShippingServices.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.ShippingServices"
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
            else if (document.getElementById('<%=txtName.ClientID %>').value == '') {

                jAlert('Please enter Shipping Service Name.', 'Message', '<%=txtName.ClientID %>');
                return false;
            }
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
                                                    <img class="img-left" title="Add Shipping Service" alt="Add Shipping Service" src="/App_Themes/<%=Page.Theme %>/Images/shipping-services-icon.png" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Shipping Service" ID="lblTitle"></asp:Label></h2>
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
                                                            <span class="star">*</span>Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtName" CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Active:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkActive" runat="server" />
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
