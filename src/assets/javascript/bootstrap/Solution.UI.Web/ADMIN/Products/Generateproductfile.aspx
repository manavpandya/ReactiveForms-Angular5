<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Generateproductfile.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Generateproductfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
  <div>
    <asp:TextBox ID="txtpassword" runat="server" TextMode="Password"></asp:TextBox>

    </div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" Visible="false" />
        <asp:Button ID="btnGenerate" runat="server"  Text="Get Data" OnClick="btnGenerate_Click"  />
        <asp:Button ID="btnexport" runat="server" Text="Export All product" OnClick="btnexport_Click" />
        <asp:Button ID="btnparentsku" runat="server" Text="Export All product" OnClick="btnparentsku_Click"  />
        <asp:Button ID="btnswatchexport" runat="server" Text="Export All Swatch product" OnClick="btnswatchexport_Click" />
    </form>
</body>
</html>
