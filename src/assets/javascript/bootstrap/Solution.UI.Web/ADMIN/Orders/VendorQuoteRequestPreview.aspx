<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorQuoteRequestPreview.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.VendorQuoteRequestPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="" name="Keywords" />
    <meta content="" name="Description" />
    <title>Welcome to Half Price Drapes</title>
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 0px;
            font-family: Arial, Helvetica, sans-serif;
        }
        .datatable table
        {
            border: 1px solid #eeeeee;
            width: 100%;
        }
        .datatable tr.alter_row
        {
            background-color: #f9f9f9;
        }
        .datatable td
        {
            padding: 2px 2px;
            text-align: left;
            border: 1px solid #eeeeee;
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }
        .datatable th
        {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }
        .pop_footer_link
        {
            height: 30px;
            line-height: 30px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #2A2A2A;
            text-align: center;
        }
        .popup_docwidth
        {
            width: 650px;
            border: 1px solid #D5D4D4;
            background-color: #FFFFFF;
            margin: 0 auto;
        }
        .pop_header_row2
        {
            float: left;
            width: 650px;
            text-align: center;
            font-size: 12px;
            line-height: 28px;
            color: #000000;
            background: #f1f1f1;
            border-top: 1px solid #D5D4D4;
            border-bottom: 1px solid #D5D4D4;
        }
        .pop_header_row2 a
        {
            color: #000;
            text-decoration: none;
        }
        .pop_header_row2 a:hover
        {
            color: #F2570A;
            text-decoration: underline;
        }
        .popup_cantain
        {
            padding-left: 10px;
            font-size: 12px;
            color: #2f2f2f;
            text-decoration: none;
            line-height: 20px;
            font-weight: normal;
        }
        .popup_cantain a
        {
            font-size: 12px;
            color: #f2570a;
            text-decoration: none;
        }
        .popup_cantain a:hover
        {
            font-size: 12px;
            color: #2f2f2f;
            text-decoration: underline;
        }
        .user_bg
        {
            width: 300px;
            height: 60px;
            padding-top: 7px;
            padding-left: 7px;
            background: url(images/user_bg.gif) no-repeat left top;
        }
        .style2
        {
            float: left;
            width: 640px;
            height: 30px;
            line-height: 30px;
            font-size: 12px;
            color: #2f2f2f;
            padding-left: 10px;
            border-top: 1px solid #D5D4D4;
        }
        .style3
        {
            font-weight: bold;
        }
        .forgot_textfild
        {
            color: #2f2f2f;
            width: 150px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function refreshParent() {
            window.opener.location.href = window.opener.location.href;
            if (window.opener.progressWindow) {
                window.opener.progressWindow.close()
            }
            window.close();
        }

    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" class="popup_docwidth">
            <tbody>
                <tr>
                    <td class="pop_header">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tbody>
                                <tr>
                                    <td>
                                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tbody>
                                                <tr>
                                                    <td valign="middle" align="left" rowspan="1">
                                                        <img alt="logo" border="0" runat="server" class="img_left" id="ImgStoreLogo" />
                                                    </td>
                                                    <td width="20" style="padding: 10px;">
                                                        <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                                            <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="pop_header_row2" style="margin-top: 2px;">
                                        <asp:Literal ID="ltrHeaderLink" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr style="float: left; margin: 0; padding: 5px;">
                    <td>
                        <img alt="Welcome Banner" height="215" id="imgBanner" runat="server" title="Welcome Banner"
                            width="640" />
                    </td>
                </tr>
                <tr>
                    <td class="popup_cantain">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tbody>
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" class="popup_cantain" width="100%">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-left: 10px;">
                                                        Hi,
                                                        <br />
                                                        <span style="padding-left: 15px;">Please provide us with your quote for the items listed
                                                            below.</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <asp:Literal ID="ltrProductDetails" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 5px;">
                                                        <br />
                                                        <asp:Literal ID="ltrNotes" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Thank you,<br />
                                                        <asp:Literal ID="ltrFooter" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="pop_header_row2">
                        <asp:Literal ID="ltrBottomHead" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="pop_footer_link" style="background: #fff;">
                        &copy; 2012.
                        <asp:Label ID="lblStoreName" runat="server"></asp:Label>
                        All rights reserved.
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
