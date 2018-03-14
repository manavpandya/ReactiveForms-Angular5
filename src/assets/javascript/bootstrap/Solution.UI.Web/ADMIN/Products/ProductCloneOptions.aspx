<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCloneOptions.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.ProductCloneOptions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Clone Options</title>
</head>
<body style="background: none">
    <form id="form1" runat="server">
    <div style="padding: 4px;">
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Product Clone
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" align="center">
                            <tr style="text-align: center">
                                <td style="padding: 5px">
                                    Are you sure you want to Clone this product?
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr style="border-top: solid 1ox #e7e7e7;">
                    <td style="text-align: center; padding-bottom: 8px;">
                        <asp:ImageButton ID="btnSave" runat="server" onclick="btnSave_Click" 
                              />
                        <asp:ImageButton ID="btnCancel" runat="server" OnClientClick="window.close();" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
