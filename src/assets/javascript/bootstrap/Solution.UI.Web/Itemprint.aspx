<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Itemprint.aspx.cs" Inherits="Solution.UI.Web.Itemprint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        body
        {
            color: #fff;
            font-family: Verdana,Arial,Helvetica,sans-serif;
        }
        
        .table_border
        {
            border: 1px solid #ECECEC;
            margin: 0 auto;
        }
        .style2
        {
            color: #363636;
            font-size: 12px;
        }
        .style3
        {
            font-size: 12px;
            color: #02414d;
        }
        .pop_title
        {
            padding: 0px 10px 0px 10px;
            font-size: 12px;
            font-weight: bold;
        }
        .style5
        {
            font-size: 12px;
            color: #ff0000;
            font-weight: bold;
        }
        .style6
        {
            font-size: 12px;
            color: #363636;
            font-weight: bold;
        }
        .popup_text
        {
            font-size: 11px;
            float: left;
            line-height: 16px;
            color: #363636;
        }
        .style9
        {
            font-size: 12px;
            color: #616161;
            font-weight: bold;
        }
        .print_title_bo
        {
            width: 100%;
            float: left;
            padding: 5px 0px 5px 0px;
            background: #ECECEC;
        }
        .table_border_none
        {
            float: left;
            margin: 0px 0px 0px 0px;
            padding: 0px;
        }
        .table_border_none td
        {
            float: left;
            margin: 0px 0px 0px 0px;
        }
        .img_left
        {
            float: left;
        }
        .img_right
        {
            float: right;
        }
        .print
        {
            float: right;
            padding: 10px 0px 0px 0px;
        }
        .close
        {
            float: right;
            padding: 10px 0px 0px 5px;
        }
         .item-tabing-text
        {
            float: left;
            padding: 0;
            margin-left:15px;
            width:670px;
        }
        .item-tabing-text p
        {
            line-height: 16px;
            padding: 5px 0;
        }
        .item-tabing-text ul
        {
            float: left;
            list-style: none outside none;
            padding: 0 0 10px 10px;
            width: 98%;
        }
        .item-tabing-text ul li
        {
            background: url("../images/bullet.jpg") no-repeat scroll left 6px transparent;
            color: #4F4F4F;
            float: left;
            padding: 0 0 0 10px;
           width: 98%;
        }
        
        .tabberlive .tabbertab
        {
            border-left: 1px solid #D9D9D9;
            border-right: 1px solid #D9D9D9;
            float: left;
            padding: 0 10px;
            position: relative;
            width: 98%;
        }
       
        .btnHide
        {
            display: block;
        }
    </style>
    <style type="text/css" media="print">
        .btnHide
        {
            display: none;
        }
    </style>
</head>
<body style="background-color: #FFFFFF; background-image: none;">
    <form id="form1" runat="server">
    <div id="popupup_docwidth1">
        <table width="700" class="table_border" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table width="100%" border="0" style="padding: 8px;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding: 10px;">
                                <a href="/" target="_blank">
                                    <img src="/images/logo.png" class="img_left" style="border: 0; padding: 5px 0 0 10px;" />
                                </a><a href="javascript:window.close();" title="CLOSE" class="btnHide">
                                    <img src="/images/close.png" alt="CLOSE" class="close" style="border: 0" title="CLOSE" /></a><a
                                        href="javascript:window.print();" class="btnHide" title="Print"><img src="/images/print.png"
                                            alt="PRINT" class="print" style="border: 0" title="PRINT" /></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div class="print_title_bo">
                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table_border_none">
                            <tbody>
                                <tr>
                                    <td class="pop_title" style="color: #000000; text-align: left;">
                                        <strong>
                                            <asp:Literal ID="litProductNamePart" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <table width="700" border="0" cellspacing="0" cellpadding="0" style="height: 200px">
                        <tr>
                            <td style="width: 330px" align="center" valign="middle">
                                <asp:Literal ID="litProductMainImage" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 292px" align="left" valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="color: #000;">
                                    <tr>
                                        <td colspan="3" style="height: 60px">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px; height: 35px" class="print_text style2">
                                            SKU:
                                        </td>
                                        <td>
                                        </td>
                                        <td style="width: 68">
                                            <span class="style3"><span class="style6">
                                                <asp:Literal ID="litItemNumber" runat="server"></asp:Literal>
                                            </span></span>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trRegularPrice">
                                        <td style="height: 35px" class="print_text style2">
                                            <asp:Literal ID="ltRetailName" runat="server"></asp:Literal>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="print_text style3">
                                            <span id="spanRetail" runat="server" class="style6" >
                                                <asp:Literal ID="ltRegularPrice" runat="server"></asp:Literal></span>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trYourPrice">
                                        <td class="print_text style2" style="height: 35px">
                                            Your Price :
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <strong style="color: #B92127; font-size: 12px;">
                                                <asp:Literal ID="litSalePrice" runat="server"></asp:Literal></strong>
                                        </td>
                                    </tr>
                                      <tr runat="server" id="trTodayOnly" visible="false">
                                        <td class="print_text style2" style="height: 35px; color: #FF0000;font-weight:bold;">
                                            Today Only :
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <strong  style="color: #B92127; font-size: 12px;">
                                                <asp:Literal ID="litTodayOnly" runat="server"></asp:Literal></strong>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trYouSave">
                                        <td style="height: 35px" class="print_text style2">
                                            You Save :
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <span style="color: #B92127; font-size: 12px;">
                                                <asp:Literal ID="litYourSave" runat="server"></asp:Literal></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td height="30" colspan="3">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="DivDescriptionTitle" runat="Server">
                <td colspan="3">
                    <div class="print_title_bo">
                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table_border_none">
                            <tbody>
                                <tr>
                                    <td class="pop_title" style="color: #000000; text-align: left;">
                                        <strong>Product Description</strong>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </td>
            </tr>
            <tr style="color: #5E5E5E; font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 12px;"
                id="DivDescription" runat="Server">
                <td colspan="3" class="item-tabing-text" style="padding: 5px; text-align: left">
                    <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none;">
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <asp:ImageButton ID="popupContactClose" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
        <asp:ImageButton ID="popupContactClose1" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
        <input type="hidden" id="hdnprice" runat="server" value="0" />
        <input type="hidden" id="hdnsaleprice" runat="server" value="0" />
        <input type="hidden" id="hdnYousave" runat="server" value="0" />
        <input type="hidden" id="hdnActual" runat="server" value="0" />
        <input type="hidden" id="hdnSaleActual" runat="server" value="0" />
    </div>
    </form>
</body>
</html>
