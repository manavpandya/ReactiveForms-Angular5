<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Country.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.EditCountry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {

            if (document.getElementById('<%=txtCountryName.ClientID%>').value == '') {

                jAlert('Please enter Country Name.', 'Message', '<%=txtCountryName.ClientID%>');
                return false;

            }
            else if (document.getElementById('<%=txttwoISOCode.ClientID%>').value == '') {

                jAlert('Please two letter ISO code.', 'Message', '<%=txttwoISOCode.ClientID%>');
                return false;
            }
            return true;
        }

        function clearfield() {

            if (document.getElementById("ContentPlaceHolder1_txtCountryName") != null) { document.getElementById("ContentPlaceHolder1_txtCountryName").value = ''; }
            if (document.getElementById("ContentPlaceHolder1_txttwoISOCode") != null) { document.getElementById("ContentPlaceHolder1_txttwoISOCode").value = ''; }
            if (document.getElementById("ContentPlaceHolder1_txtthreeISOCode") != null) { document.getElementById("ContentPlaceHolder1_txtthreeISOCode").value = ''; }
            if (document.getElementById("ContentPlaceHolder1_txtNumISOCode") != null) { document.getElementById("ContentPlaceHolder1_txtNumISOCode").value = ''; }
            if (document.getElementById("ContentPlaceHolder1_txtDisplayOrder") != null) { document.getElementById("ContentPlaceHolder1_txtDisplayOrder").value = ''; }
            return false;
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
                        <img src="/App/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                                    <img class="img-left" title="Add Country" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/Images/country-list-icon.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Country" ID="lblTitle"></asp:Label></h2>
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
                                                            <span class="star">*</span>Country Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtCountryName" CssClass="order-textfield" MaxLength="200"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Two Letter ISO Code:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txttwoISOCode" CssClass="order-textfield" MaxLength="2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Three Letter ISO Code:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtthreeISOCode" CssClass="order-textfield" MaxLength="3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Numeric ISO Code:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtNumISOCode" CssClass="order-textfield" MaxLength="3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Display Order:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDisplayOrder" onkeypress="return isNumberKey(event)"
                                                                CssClass="order-textfield" MaxLength="3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 80%">
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
