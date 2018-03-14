<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PriceMatch.aspx.cs" Inherits="Solution.UI.Web.PriceMatch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
     <link href='http://fonts.googleapis.com/css?family=Carrois+Gothic|Telex|Oxygen' rel='stylesheet' type='text/css' />
    <title></title>
    <script language="javascript" type="text/javascript" src="/js/PriceMatch.js"></script>
    <style type="text/css">
.error{color:red;font-size:12px;}
.message{font:arial;font-size:12px;font-weight:bold;text-align:center;margin:20px;padding-top:20px;display:none;height:470px;}
#popupup_docwidth1 {margin:0 auto;padding:10px;width:550px;overflow:hidden;}
#popup_bg1 {background:none repeat scroll 0 0 #ffffff;margin:0 0 0px;width:549px;}
#popup_header {background:none repeat scroll 0 0 #000000;border:1px solid #000000;float:left;height:35px;margin:0;width:550px;padding-top:5px;}
.addonProduct {float:left;text-align:center;width:100%;}
.description h2{font-size:12px;}
.descDetail {float:left;margin: 10px 15px 10px 10px;;width:95%;}
.description {color:#000000;font-size:12px;padding:0;text-align:justify;float:left;padding-left:10px;width:95%; margin:0px;padding-top:0px;}
.description .description .product {}
.title {color:#555555;float:left;font-size:14px;font-weight:bold;margin:8px;text-align:left;text-transform:uppercase;vertical-align:middle;width:550px;}
.productDetail {}
.img_center {float:left;margin:5px;text-align:center;vertical-align:middle;}
.productName {color:#555555;font-size:14px;font-weight:bold;padding:0;text-align:left;}
.img_center img {display:inline;width:164px;text-align:center;vertical-align:middle;border:1px solid #909090;}
#header1{width:505px;height:47px;padding:4px 0 0 0;text-align:center;font-family:Arial,Tahoma,verdana,Helvetica,sans-serif;}
#header img{margin-bottom:10px;}
.textfield_medium{font-family:Arial, Helvetica, sans-serif; font-size:11px; color:#2e2d2d; border:1px solid #d8d8d8; padding-left:2px; height:16px; background-color:#ffffff;}
body {background:none repeat scroll 0 0 #F6F1EB;color:#2E2D2D;font-family:Arial,Helvetica,sans-serif;font-size:12px;margin:0 auto;padding:0;}

.table td {
    border: 1px solid #EEECF7;
    color: #2E2D2D;
    font: 12px/18px Arial,Helvetica,sans-serif;
}

#popup_content h1 {
    color: #555555;
    font-family: Arial,Helvetica,sans-serif;
    font-size: 14px;
    padding: 0px 0 0px 10px;
    text-align: left;
}

a{color:red; text-decoration: none;}
a:hover {color:#2E2D2D; text-decoration:underline;}
 </style>
    <script type="text/javascript">
        function onKeyPressPhone(e) {
            var key = window.event ? window.event.keyCode : e.which;

            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div id="popupup_docwidth1">
        <table cellpadding="0" cellspacing="0" style="border: 1px solid #909090; padding: 0px"
            width="550px">
            <tr>
                <td>
                    <div id="popup_bg1">
                        <%--  <div id="header1" style="width: 550px; text-align: left;">
                            <table width="549px">
                                <tr>
                                    <td>
                                        <a href="/" target="_blank">
                                            <img style="border: none;" src='/images/logo.png' alt="" />
                                        </a>
                                    </td>
                                    <td>
                                        <a href="javascript:close();" style="margin-right: 10px; float: right; padding-bottom: 15px;
                                            margin-top: 0px;">
                                            <img style="border: none;" src="/images/close.png" title="CLOSE" alt="CLOSE" />
                                        </a>
                                    </td>
                                </tr>
                            </table>
                        </div>--%>
                        <div class="addonProduct" style="width: 550px;">
                            <div class="product" style="width: 550px;">
                                <div id="popup_content" style="text-align: center; background: #ffffff; width: 529px;"
                                    runat="server">
                                    <input type="hidden" id="hdnItemNo" name="hdnItemNo" value="1" />
                                    <h1 style="border-bottom: 1px solid #DDDDDD; color: #B92127; font-size: 20px; text-transform: uppercase;
                                        font-weight: normal; padding-bottom: 5px; width: 540px;">
                                        Price Match Request<%--<div style="float: right; font-size: 12px; font-weight: normal;
                                            color: #2E2D2D; text-transform: none;">
                                            <span class="error">*</span>Required Fields</div>--%>
                                    </h1>
                                    <div style="text-align: center;">
                                        <table cellpadding="4" cellspacing="0" style="text-align: center; width: 99%;">
                                            <tr>
                                                <td width="70%">
                                                </td>
                                                <td width="20%">
                                                    <span class="error">*</span>Required Fields
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="2" border="0" cellspacing="0" id="tblItem" style="text-align: center;
                                            margin-left: 8px; width: 100%; border-collapse: collapse;" class="table">
                                            <tbody>
                                                <tr>
                                                    <td width="70%">
                                                        Item Name
                                                    </td>
                                                    <td width="20%">
                                                        Competitor's Price<span class="error">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label id="txtItem1" runat="server" style="float: left; text-align: left">
                                                        </label>
                                                    </td>
                                                    <td>
                                                        <input type="text" runat="server" id="txtPrice1" style="width: 60px" maxlength="5"
                                                            class="textfield_medium" name="txtPrice1" onblur="getPrice();" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <table cellpadding="4" cellspacing="0" style="text-align: center; width: 99%;">
                                            <tr>
                                                <td align="left" colspan="2">
                                                </td>
                                                <td>
                                                    <table cellspacing="0" cellpadding="1" width="100%">
                                                        <tr>
                                                            <td align="right">
                                                                Competitor's Delivery Charge :
                                                            </td>
                                                            <td align="right" style="width: 88px;">
                                                                <input type="text" id="txtShipping" style="width: 60px; margin-right: 8px" maxlength="5"
                                                                    class="textfield_medium" name="txtShipping" onblur="getPrice();" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                Total Cost :
                                                            </td>
                                                            <td align="right">
                                                                <input type="text" id="txtTotalPrice" style="width: 60px; margin-right: 8px" maxlength="5"
                                                                    class="textfield_medium" name="txtTotalPrice" readonly="readonly" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="text-align: left;">
                                            <span style="font-weight: bold;">&nbsp;&nbsp;&nbsp;How did you see this price?</span>&nbsp;
                                            <input type="radio" id="rbtweb" name="rbtknow" value="Website" checked="checked"
                                                onclick="changeDiv(this.value);" /><span>Website</span>&nbsp;
                                            <input type="radio" id="rbtstore" name="rbtknow" value="Retail Store" onclick="changeDiv(this.value);" /><span>Retail
                                                Store</span>
                                            <div id="knowWeb" style="margin-top: 5px; margin-left: 8px; width: 100%; background-color: #eee;">
                                                <table cellpadding="2" cellspacing="0" align="center" border="0" class="table" style="background-color: #eee;
                                                    padding: 0px;">
                                                    <tr>
                                                        <td width="25%">
                                                            <span>Enter the competitor's website address you found the item(s) at:</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <input type="text" id="txtSweb" name="txtSweb" style="width: 400px" class="textfield_medium" />
                                                            <br />
                                                            (Example:http://www.amazon.com)
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="knowStore" style="display: none; margin-top: 5px; width: 100%; margin-left: 8px;">
                                                <table cellpadding="2" cellspacing="0" border="0" class="table" width="100%" style="background-color: #eee;
                                                    padding-left: 4px">
                                                    <tr>
                                                        <td width="25%">
                                                            <span class="error">*</span><span>Store Name:</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtSstore" name="txtSstore" style="width: 150px" class="textfield_medium" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="error">*</span><span>Store City:</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtScity" name="txtScity" style="width: 150px" class="textfield_medium" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="error">*</span><span>Store State:</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtSstate" name="txtSstate" style="width: 150px" class="textfield_medium" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="error">&nbsp;</span><span>Store Phone:</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtSphone" name="txtSphone" style="width: 150px" class="textfield_medium"
                                                                maxlength="20" />
                                                            &nbsp;&nbsp;&nbsp;(Ex:000-000-0000)
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="contactInfo" style="margin-top: 7px;">
                                                <table cellpadding="5" cellspacing="0" border="0" class="table" width="100%" align="center"
                                                    style="margin-left: 8px; border-collapse: collapse;">
                                                    <tr>
                                                        <td width="55%">
                                                            <span class="error">*</span>Your Name:
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtName" name="txtName" class="textfield_medium" width="90px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="error">*</span>Email Address:
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtEmail" name="txtEmail" class="textfield_medium" width="90px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="error">&nbsp;</span> Phone Number:
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtPhone" name="txtPhone" class="textfield_medium" width="90px"
                                                                maxlength="12" onkeypress="return onKeyPressPhone(event);" />&nbsp;&nbsp;&nbsp;(Ex:000-000-0000)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="error">*</span>Shipping ZipCode:
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtZipcode" name="txtZipcode" class="textfield_medium" width="90px"
                                                                maxlength="10" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="error">&nbsp;</span>Additional Comments:
                                                        </td>
                                                        <td>
                                                            <textarea id="txtComment" name="txtComment" cols="90" rows="3" style="resize: none;
                                                                width: 375px;"></textarea>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div style="background-color: #eee; margin-top: 5px; width: 98%; margin-left: 8px;
                                                    padding: 5px;">
                                                    Check to make sure your email address is accurate so that we can respond to your
                                                    request. Include your telephone number if you would like us to call you.
                                                </div>
                                                <div style="margin-top: 5px; text-align: right;">
                                                    <asp:ImageButton ID="btnSubmit" runat="Server" ImageUrl="/images/submit.png" OnClick="btnSubmit_Click"
                                                        OnClientClick="return onSubmit();" ToolTip="SUBMIT" />
                                                    <br />
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="divDone" runat="server" class="message">
                                    Price Match Information has been Successfully Sent to the Admin.<br />
                                    Your Information will be Followed as Early as Possible.
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
