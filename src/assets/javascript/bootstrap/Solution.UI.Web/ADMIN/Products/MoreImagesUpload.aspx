<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoreImagesUpload.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.MoreImagesUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function DeleteImage(id) {
            document.getElementById('hdnimageUrl').value = id;
            document.getElementById('btnDeleteImg').click();
        }
        function ClearImage(id) {
            document.getElementById('hdnimageUrl').value = id;
            document.getElementById('btnClearDesc').click();
        }
    </script>
</head>
<body style="background: none">
    <form id="form1" runat="server">
    <div style="padding: 4px;">
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left;">
                            Upload More Images
                        </div>
                        <div class="main-title-right">
                            <a onclick="window.close();" class="show_hideMainDiv" href="javascript:void(0);">
                                <img src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" alt="close" title="close"
                                    class="close" id="imgMainDiv" /></a>
                            <input id="hdnimageUrl" runat="server" type="hidden" value="" />
                            <div style="display: none;">
                                <asp:Button ID="btnDeleteImg" runat="server" OnClick="btnDeleteImg_Click" />
                                <asp:Button ID="btnClearDesc" runat="server" OnClick="btnClearDesc_Click" />
                            </div>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td>
                        <div class="slidingDivImage">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="15%" valign="top" style="padding-top: 10px;">
                                        <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">&nbsp;&nbsp;
                                            Product Name </span>:
                                    </td>
                                    <td valign="top" style="padding-top: 10px;">
                                        <asp:Label ID="lblProductName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" colspan="2" style="padding-bottom: 10px;">
                                        <table id="tblOldImage" runat="server" visible="false" width="100%">
                                            <tr id="trOtherImg" runat="Server">
                                                <td width="15%" valign="middle" style="padding-top: 10px;">
                                                    <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">&nbsp;&nbsp;
                                                        Main&nbsp;Image&nbsp;</span>:
                                                </td>
                                                <td style="height: 20px;" valign="middle">
                                                    <asp:Literal ID="ltOldimages" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%" valign="top">
                                        <strong style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">&nbsp;&nbsp;
                                            Upload Image</strong> :
                                    </td>
                                    <td>
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="padding-bottom: 5px;">
                                            <tr align="left">
                                                <td style="width: 200px">
                                                    <asp:FileUpload ID="fileUploder" Width="190px" runat="server" BorderColor="#8C9CB1"
                                                        BorderWidth="1px" ForeColor="#333333" BorderStyle="Solid" />
                                                </td>
                                                <td style="Padding-left:40px;">
                                                    <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" ToolTip="Upload"
                                                        OnClick="btnUpload_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%" valign="top" style="padding-bottom: 10px;">
                                        &nbsp;&nbsp;&nbsp;&nbsp;Description :
                                    </td>
                                    <td style="padding-bottom: 5px;">
                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="order-textfield"
                                            Width="300px" Height="45px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border-top: solid 1px #e7e7e7;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="ltMoreimages" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
