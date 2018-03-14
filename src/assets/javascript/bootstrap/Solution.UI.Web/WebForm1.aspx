<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Solution.UI.Web.WebForm1" %>

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
    <td>Product Id: <asp:TextBox ID="txtProductid" Text="5565" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
    <td>Width : 
    </td>
    <td><asp:TextBox ID="txtWidth" runat="server"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td>Length : 
    </td>
    <td><asp:TextBox ID="txtLength" runat="server"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td>panel : 
    </td>
    <td><asp:TextBox ID="txtpanel" runat="server"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td colspan="2">
    <asp:Button ID="btnCalculate" Text="Save" runat="server" onclick="btnCalculate_Click" />
    </td>
    </tr>
     <tr>
    <td colspan="2">
    <asp:Literal ID="ltCalcuation" runat="server"></asp:Literal>
    </td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
