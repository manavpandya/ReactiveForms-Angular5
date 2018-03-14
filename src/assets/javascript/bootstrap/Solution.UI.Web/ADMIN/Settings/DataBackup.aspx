<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="DataBackup.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.DataBackup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function validation() {
            jConfirm('Are you sure, Do you want to perform this action ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById('ContentPlaceHolder1_btnTemp').click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
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
                                                <img class="img-left" title="DataBase Backup" alt="DataBase Backup" src="/App_Themes/<%=Page.Theme %>/Images/header-links-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="DataBase Backup" ID="lblTitle"></asp:Label></h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <table style="height: 250px">
                                                <tr>
                                                    <td valign="top">
                                                        Database Backup :
                                                    </td>
                                                    <td valign="top">
                                                        <asp:ImageButton ID="btnBackupDatabase" runat="server" OnClick="btnBackupDatabase_Click"
                                                            OnClientClick="return validation();" />
                                                        <div style="display: none">
                                                            <asp:ImageButton ID="btnTemp" runat="server" OnClick="btnBackupDatabase_Click" />
                                                        </div>
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
