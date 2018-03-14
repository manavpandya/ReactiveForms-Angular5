<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailDetail.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.MailDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top:10px;float:right;">
             <asp:ImageButton ID="btnresend" runat="server" ImageUrl="/App_Themes/gray/images/send-mail.png" OnClick="btnresend_Click" />
            </div>
    <div>
    <asp:Literal ID="ltrDetails" runat="server"></asp:Literal>
        
    </div>
    </form>
</body>
</html>
