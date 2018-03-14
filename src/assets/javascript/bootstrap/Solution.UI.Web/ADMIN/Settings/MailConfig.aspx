<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="MailConfig.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.OnePageMailConfig"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
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
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                                    <img class="img-left" title="Mail Configuration" alt="Mail Configuration" src="/App_Themes/<%=Page.Theme %>/Images/mail-configuration-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server">
                            Mail Configuration
                                                        </asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="2">
                                                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
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
                                                        <td style="width: 25%">
                                                            <span class="star">*</span>Store Name:
                                                        </td>
                                                        <td width="185px">
                                                            <asp:DropDownList ID="ddlStore" AutoPostBack="true" runat="server" CssClass="order-list"
                                                                Width="185px" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                          </td>
                                                          <td><asp:ImageButton ID="imgTestMail" runat="server" AlternateText="Test Mail" 
                                                                CausesValidation="true"  Height="23px"   onclick="imgTestMail_Click" 
                                                                ToolTip="Test Mail" ValidationGroup="AppConfig" />
                                                      </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Host:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtHost" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtHost" runat="server" ControlToValidate="txtHost"
                                                                ErrorMessage="Enter Host" SetFocusOnError="True" ValidationGroup="AppConfig"
                                                                Display="Dynamic" ForeColor="#FF0000"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Mail User Name :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMailUserName" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtMailUserName" runat="server" ControlToValidate="txtMailUserName"
                                                                ErrorMessage="Enter Mail User Name" SetFocusOnError="True" ValidationGroup="AppConfig"
                                                                Display="Dynamic" ForeColor="#FF0000"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="revtxtMailUserName" runat="server" Display="Dynamic"
                                                                ControlToValidate="txtMailUserName" ErrorMessage="Enter Valid User Name" ForeColor="#FF0000"
                                                                SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                ValidationGroup="AppConfig"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Mail Password :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMailPassword" runat="server" CssClass="order-textfield" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtMailPassword" runat="server" ControlToValidate="txtMailPassword"
                                                                Display="Dynamic" ErrorMessage="Enter Mail Password" SetFocusOnError="True" ValidationGroup="AppConfig"
                                                                ForeColor=" #FF0000"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Mail From Address :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMailFrom" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtMailFrom" runat="server" ControlToValidate="txtMailFrom"
                                                                Display="Dynamic" ErrorMessage="Enter Mail From Address" SetFocusOnError="True"
                                                                ValidationGroup="AppConfig" ForeColor=" #FF0000"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="revtxtMailFrom" runat="server" ControlToValidate="txtMailFrom"
                                                                ErrorMessage="Enter Valid E-Mail" ForeColor=" #FF0000" SetFocusOnError="True"
                                                                Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                ValidationGroup="AppConfig"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Mail To Address :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMailMe_ToAddress" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtMailMe_ToAddress" runat="server" ControlToValidate="txtMailMe_ToAddress"
                                                                ErrorMessage="Enter Mail To Address" SetFocusOnError="True" ValidationGroup="AppConfig"
                                                                Display="Dynamic" ForeColor=" #FF0000"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="revtxtMailMe_ToAddress" runat="server" ControlToValidate="txtMailMe_ToAddress"
                                                                ErrorMessage="Enter Valid E-Mail" ForeColor=" #FF0000" SetFocusOnError="True"
                                                                Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                ValidationGroup="AppConfig"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Contact Mail To Address :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtContactMail_ToAddress" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtContactMail_ToAddress" runat="server" ControlToValidate="txtContactMail_ToAddress"
                                                                ErrorMessage="Enter Contact Mail To Address " SetFocusOnError="True" ValidationGroup="AppConfig"
                                                                Display="Dynamic" ForeColor="#FF0000"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="revtxtContactMail_ToAddress" runat="server" ControlToValidate="txtContactMail_ToAddress"
                                                                ErrorMessage="Enter Valid E-Mail" ForeColor="#FF0000" SetFocusOnError="True"
                                                                Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                ValidationGroup="AppConfig"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            &nbsp;&nbsp;Send Customer Registration Mail? :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSendCustomerRegistrationMail" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td align="left" valign="top">
                                                        </td>
                                                        <td align="left" style="padding: 3px; padding-left: 0px;">
                                                            <table width="220px">
                                                                <tr>
                                                                    <td align="left" style="padding-left: 0px;">
                                                                        <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                            CausesValidation="true" Height="23px" OnClick="imgSave_Click" Width="57px" ValidationGroup="AppConfig" />
                                                                        &nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                            OnClick="imgCancel_Click" />
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
            </table>
        </div>
    </div>
</asp:Content>
