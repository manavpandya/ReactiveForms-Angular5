<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="OnePageAppConfig.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.OnePageAppConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
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
                                                    <img class="img-left" title="One Page Application Configuration" alt="One Page Application Configuration"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/mail-configuration-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server">
                          Application 
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
                                                        <td style="width: 15%">
                                                            <span class="star">&nbsp</span>Store Name :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStore" AutoPostBack="true" runat="server" CssClass="order-list"
                                                                Width="185px" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td style="width: 15%">
                                                            <span class="star">&nbsp</span>Store Phone :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStorePhone" runat="server" CssClass="order-textfield" Width="185px" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp</span>Site SE-Title :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Width="500px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp</span>Site SE-Keywords :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSEKeywords" runat="server" CssClass="order-textfield" Width="500px"
                                                                Height="80px" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp</span>Site SE-Description :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSeDescription" runat="server" CssClass="order-textfield" Width="500px"
                                                                Height="80px" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp</span>Use SSL? :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkUseSSL" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp</span>Is Store ON? :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkStoreOn" runat="server" Checked="True" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp</span>Store Closed Message :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStoreClosedMsg" runat="server" CssClass="order-textfield" Width="500px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td align="left" valign="top">
                                                        </td>
                                                        <td align="left" style="padding: 3px; padding-left: 0px;">
                                                            <table width="220px">
                                                                <tr>
                                                                    <td align="left" style="padding-left: 0px;">
                                                                        <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                            OnClick="btnSave_Click" />
                                                                        &nbsp;<asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                            OnClick="btnCancel_Click" />
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
