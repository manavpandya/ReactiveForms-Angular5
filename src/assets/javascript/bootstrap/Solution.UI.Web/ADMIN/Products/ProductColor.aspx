<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductColor.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductColor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function ChkValidate() {
            //if (document.getElementById('ddlProductColor') != null && document.getElementById("ddlProductColor").selectedIndex == 0) {
            //    alert('Please Select Product Color', 'Message');
            //    document.getElementById('ddlProductColor').focus();
            //    return false;
            //}
            return true;
        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Search Product Color(s)
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr id="trAddSeletedItems" runat="server">
                    <td style="text-align: right; height: 30px; padding: 2px; padding-right: 4px;">
                        <div class="slidingDivImage" style="padding-top: 8px; padding-bottom: 8px;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table cellpadding="1" cellspacing="1" width="100%">
                                            <tr style="display:none;">
                                                <td style="width: 100px;" align="right">
                                                    <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">Select
                                                        Color </span>:
                                                </td>
                                                <td align="left" style="padding-left: 3px; width: 140px;">
                                                    <asp:DropDownList ID="ddlProductColor" runat="server" CssClass="order-list" Style="width: 135px;"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlProductColor_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left">
                                                    <asp:Literal ID="ltrImagePath" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr class="oddrow">
                                                <td>
                                                    New&nbsp;Color&nbsp;Image&nbsp;:
                                                </td>
                                                <td colspan="2">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td width="19%">
                                                                            <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a;
                                                                                background: #f5f5f5; color: #000000;" TabIndex="25" />
                                                                            <br />
                                                                            <asp:Label ID="lblImgMsg" Font-Bold="true" runat="server" Text=" Color Image Size Should be (230 x 310) <br /> Roman Shade Image Size Should be (600 x 700)"></asp:Label>
                                                                        </td>
                                                                        <td align="left" width="10%">
                                                                            &nbsp;<asp:ImageButton ID="btnUpload" runat="server" style="margin-bottom: 10px;" AlternateText="Upload" OnClick="btnUpload_Click" />
                                                                        </td>
                                                                        <td valign="middle" align="left">
                                                                            <img alt="Upload" id="ImgLarge" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                runat="server" style="margin-bottom: 10px; border: 1px solid darkgray;" />
                                                                        </td>
                                                                        <td align="left">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px;">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="2">
                                                    &nbsp;<asp:ImageButton ID="btnAddToSelectionlist" runat="server" OnClientClick="return ChkValidate();"
                                                        OnClick="btnAddToSelectionlist_Click" />
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
