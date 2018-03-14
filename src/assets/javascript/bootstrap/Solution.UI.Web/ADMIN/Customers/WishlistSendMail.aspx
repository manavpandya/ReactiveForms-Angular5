<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WishlistSendMail.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Customers.WishlistSendMail" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Wish List Send Mail</title>
    <style type="text/css" media="print">
        .Printinvoice {
            display: none;
        }

        .order-textfield {
            background: none repeat scroll 0 0 #FFFFFF;
            border: 1px solid #BCC0C1;
            height: 17px;
            width: 224px;
        }
    </style>
    <link href="../../App_Themes/gray/css/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function onKeyPressBlockNumbers(e) {
            var key = window.event ? window.event.keyCode : e.which;

            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 0) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }

        function ShowLocation() {
            //window.parent.location.href = '/Admin/Promotions/CouponsList.aspx';
            window.opener.location = '/Admin/Promotions/CouponsList.aspx';
        }

    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
        <div>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table"
                style="padding: 2px;">
                <tbody>
                    <tr>
                        <td>
                            <img id="imgLogo" runat="server" />
                        </td>
                        <td style="text-align: right;">
                            <img id="imgMainDiv" runat="server" onclick="javascript:window.close();" class="close"
                                title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png"
                                style="cursor: pointer" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding: 5px;"></td>
                    </tr>
                    <tr>
                        <th colspan="2">
                            <div class="main-title-left" style="color: #fff; text-align: left;">
                                Wish List Item(s)
                            </div>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%">
                            <asp:Literal ID="ltCart" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px;">
                            <a href="/Admin/Promotions/CouponsList.aspx" style="color: red; fon-weight: bold; text-decoration: underline;"
                                onclick="target='_blank';">Click Here</a>
                            <%--    <a style="" href='javascript:void(0);' onclick='javascript:ShowLocation()'>Click Here</a>--%>
                        to view given coupon code.
                        </td>
                    </tr>
                    <tr>
                        <td>Coupon Code:&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtCouponCode" CssClass="order-textfield" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px;"></td>
                    </tr>


                    <tr>
                        <td class="ckeditor-table" style="width:100%;height:200px;">
                             <CKEditor:CKEditorControl ID="CKEditor1" BasePath="/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                        
                        </td>
                    </tr>

                    <tr id="trsendmail" runat="server">
                        <td>
                            <asp:ImageButton ImageUrl="/App_Themes/<%=Page.Theme %>/button/send-email.png" runat="server"
                                ID="btnSendMail" OnClick="btnSendMail_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div>
        </div>
    </form>
</body>
</html>
