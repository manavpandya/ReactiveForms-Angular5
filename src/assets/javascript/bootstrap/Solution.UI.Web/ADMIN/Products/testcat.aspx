<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testcat.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.testcat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="txtcat" runat="server"></asp:TextBox>
        <asp:Button ID="btngo" runat="server" Text="GO" OnClick="btngo_Click" />
    </div>
    </form>
</body>
</html>
