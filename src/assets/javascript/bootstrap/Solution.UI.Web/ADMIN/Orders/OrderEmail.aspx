<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderEmail.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.OrderEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css" media="all">
        .table-none-border
        {
            border: 1px solid #ececec;
        }
        
        .datatable table
        {
            border: 1px solid #eeeeee;
        }
        .datatable tr.alter_row
        {
            background-color: #f9f9f9;
        }
        .datatable a
        {
            color: #C72E1A;
            text-decoration: none;
        }
        .datatable a:hover
        {
            color: #C72E1A;
            text-decoration: underline;
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
    </style>
    <script type="text/javascript">
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" class="table-none-border">
            <tr>
                <td align="right" style="height: 12px">
                    <a href="javascript:void(0);" id="hrnewemail" style="vertical-align: bottom;" onclick="OpenCenterWindow('/Admin/WebMail/EmailInboxmaster.aspx?ShowType=Compose&Email=<%=Request.QueryString["OrderEmail"] %>&ID=Compose&OrderId=<%=Request.QueryString["Ono"]%>');"
                        title="New Email">
                        <img id="imgnewmail" alt="New Email" src="/App_Themes/<%=Page.Theme %>/images/Web-Mail-new-mail.png" border="0" /></a>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Literal ID="ltrOrderEmails" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
