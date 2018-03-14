<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmazonContent.aspx.cs" Inherits="Solution.UI.Web.AmazonContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
            </span>
        </div>
        <div class="static-main" style="min-height: 200px;">
            <p>
                <asp:Literal ID="ltPage" runat="server"></asp:Literal></p>
        </div>
    </div>
    </form>
</body>
</html>
