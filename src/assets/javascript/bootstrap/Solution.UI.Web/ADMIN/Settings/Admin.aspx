<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Admin.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.Admin" Theme="Gray" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {


            if (document.getElementById('<%=txtFName.ClientID %>').value == '') {

                jAlert('Please enter first name.', 'Message', '<%=txtFName.ClientID %>');
                return false;

            }
            else if (document.getElementById('<%=txtLName.ClientID %>').value == '') {

                jAlert('Please enter last name.', 'Message', '<%=txtLName.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>').value == '') {

                jAlert('Please enter email address.', 'Message', '<%=txtEmail.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail.ClientID %>').value)) {
                jAlert('Please enter valid email address.', 'Message', '<%=txtEmail.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtPassword.ClientID %>').value == '') {

                jAlert('Please password.', 'Message', '<%=txtPassword.ClientID %>');
                return false;
            }

            else if (document.getElementById('<%=txtPassword.ClientID %>').value.length < 6) {

                jAlert('Password length must be 6 characters.', 'Message', '<%=txtPassword.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtRetrypassword.ClientID %>').value == '') {

                jAlert('Please enter retype password.', 'Message', '<%=txtRetrypassword.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtPassword.ClientID %>').value != document.getElementById('<%=txtRetrypassword.ClientID %>').value) {

                jAlert('Retype password must be match with password.', 'Message', '<%=txtPassword.ClientID %>');
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        var testresults
        function checkemail1(str) {
            //var str=document.validation.emailcheck.value
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                testresults = true
            else {
                //alert("Please input a valid email address!")
                testresults = false
            }
            return (testresults)
        }
        function keypressspace() {

            var val = document.getElementById('<%=txtPassword.ClientID %>').value.replace(/\s/g, "");
            document.getElementById('<%=txtPassword.ClientID %>').value = val;
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
                                                    <img class="img-left" title="Add Admin" alt="Add Admin" src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Admin" ID="lblTitle"></asp:Label></h2>
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
                                                            <span class="star">*</span>First Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtFName" CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Last Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtLName" CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>E-Mail:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEmail" CssClass="order-textfield" MaxLength="100"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Password:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" onkeyup="keypressspace();" ID="txtPassword" CssClass="order-textfield"
                                                                TextMode="Password" MaxLength="10"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Retype Password:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" onkeyup="keypressspace();" ID="txtRetrypassword" CssClass="order-textfield"
                                                                TextMode="Password" MaxLength="10"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Is Sales Manager:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkIsSalesManager" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Is Vendor:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkVendor" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Select Vendor:
                                                        </td>
                                                        <td>
                                                          <asp:DropDownList ID="ddlFabricvendor" runat="server" class="product-type">
                                                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Status:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkStatus" runat="server" />
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
