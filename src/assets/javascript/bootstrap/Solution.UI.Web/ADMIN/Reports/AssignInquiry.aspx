<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignInquiry.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.AssignInquiry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
     var testresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                testresults = true
            else {
                testresults = false
            }
            return (testresults)
        }
        function ValidatePage() {
            var flag = false;
            var name;
          
         if (document.getElementById('<%=txtEmail.ClientID%>').value == '') {

                jAlert('Please enter EmailID.', 'Message2', '<%=txtEmail.ClientID%>');
                 return false;
             }
         else if ((document.getElementById('<%=txtEmail.ClientID%>').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('<%=txtEmail.ClientID%>').value)) {
            jAlert('Please enter Valid EmailID.', 'Message3', '<%=txtEmail.ClientID%>');
                return false;
            }
               
         else if ((document.getElementById('<%=txtMessage.ClientID%>').value).replace(/^\s*\s*$/g, '') == '') {
             jAlert('Please enter Message.', 'Message3', '<%=txtMessage.ClientID%>');
             return false;
            }
               
            
           

           
        }
        </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table_border">
        <tr>
            <td colspan="3" valign="middle">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="logo" width="80%">
                            <a href="#" title="Cash Register">
                                <img src="/App_Themes/<%=Page.Theme %>/images/logo.png" 
                                    style="float: left; padding: 10px 0 0 10px;" /></a>
                        </td>
                        <td align="right" width="20%" valign="top" style="padding-right: 10px; padding-top: 10px;">
                            <a href="javascript:window.close();" title="Close" class="close">
                                <img src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" alt="Close" style="border: 0px"
                                    title="Close" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left" style="color: #fff; height: 28px; background: #7d7d7d;
                border-top: 1px solid #E4E4E4; border-bottom: 1px solid #E4E4E4; padding-left: 10px;
                font-size: 12px; font-weight: bold;">
                <strong>Assign Inquiry Details</strong>
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <table width="100%" border="0" cellpadding="4" cellspacing="0" style="padding-top: 10px;
                    line-height: 20px; padding-left: 10px;">
                    <tr>
                        <td align="left" style="width: 10%;">
                            <b>To : </b>
                        </td>
                        <td align="left">
                          <asp:TextBox ID="txtEmail" runat="server" CssClass="contact-textaria"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>

                                            <td valign="top" align="left">
                                                <span class="required-red"></span>
                                                <b>Message : </b>
                                            </td>
                                            
                                            <td>
                                                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="contact-textaria"
                                                    Columns="45" Rows="5"></asp:TextBox>
                                            </td>
                                        </tr>
                  <tr>
                     <td></td>
                      <td>
                          <asp:Button ID="btnAssign" runat="server" Text="Assign Inquiry" OnClick="btnAssign_Click" OnClientClick="return ValidatePage()"; />
                           <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />

                                                   </td>
                     
                  </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
