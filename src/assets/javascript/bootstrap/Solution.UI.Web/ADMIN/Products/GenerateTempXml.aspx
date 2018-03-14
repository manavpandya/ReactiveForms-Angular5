<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateTempXml.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.GenerateTempXml" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnGeneratexml" runat="server" Text="Generate XML" OnClick="btnGeneratexml_Click" />
        <asp:Button ID="GeneateFeed" runat="server" Text="Generate CSV" OnClick="GeneateFeed_Click" />

    </div>
    </form>
</body>
</html>
