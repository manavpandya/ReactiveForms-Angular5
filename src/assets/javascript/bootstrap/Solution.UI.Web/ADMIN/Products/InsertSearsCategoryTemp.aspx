<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsertSearsCategoryTemp.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.InsertSearsCategoryTemp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <div>
    <table>
    <tr>
    <td>Store :&nbsp;<asp:TextBox ID="txtstoreid" runat="server" Text="13"></asp:TextBox> </td>
    </tr>
    <tr>
    <td>
    <asp:TextBox ID="txtHTML" runat="server" TextMode="MultiLine" Width="1130px" Height="347px"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td>
    <asp:Button ID="btnAdd" runat="server" Text="Insert" OnClick="btnAdd_Click" />
    
    
    </td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
