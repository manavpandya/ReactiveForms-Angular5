<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportCustomerVarification.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.ExportCustomerVarification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
    <script src="/js/jquery-1.3.2.js" type="text/javascript"> </script>
      <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts.js"></script>

    <link rel="stylesheet" type="text/css" href="/css/jquery.alerts.css" />
   
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
   
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    
</head>

   <body style="background: none">
    <form id="form1" runat="server">
    <div id="popupcontactdetails" style="z-index: 1000001; width: 500px; height: 213px;padding:2px 0 0 3px">
        <table width="100%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;
            font-size: 12px; font-family: Arial,Helvetica,sans-serif;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;Export Customer
                </td>
                <td align="right" valign="top">
                   
                    <asp:ImageButton ID="popupviewdetailclose" Style="position: relative;" ImageUrl="/App_Themes/Gray/images/cancel-icon.png"
                        runat="server" ToolTip="Close" OnClientClick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose1').click();return false;" > </asp:ImageButton>
                   
                </td>
            </tr>
            <tr style="background-color: White">
                <td colspan="2" valign="top">
                 
                    <div style="display: block;" id="password" runat="server">
                        <table border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                            <tr>
                                <td valign="top" style="font-size: 11px;">
                                    <span class="required-red"></span>Password
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtpassword" runat="server" Style="font-size: 11px;" CssClass="order-textfield"
                                        Width="143" TextMode="Password"></asp:TextBox>
                                </td>

                                 
                            </tr>
                           
                            <tr class="oddrow">
                                <td colspan="2">
                                </td>
                                <td align="left">
                                    <asp:ImageButton ID="btnsubmit" OnClientClick="return checkFields(); return false;"
                                        OnClick="btnsubmit_Click" runat="server" ImageUrl="/App_Themes/Gray/images/submit.gif" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 80px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function checkFields() {
            if (document.getElementById('<%=txtpassword.ClientID %>') == null || document.getElementById('<%=txtpassword.ClientID %>').selectedIndex == "") {
                jAlert('Please Enter Password.', 'Required Information', '<%=txtpassword.ClientID %>');
                return false;
            }
            //document.getElementById("prepage").style.display = 'block';
            return true;
        }
    </script>
    </form>

</body>
</html>
