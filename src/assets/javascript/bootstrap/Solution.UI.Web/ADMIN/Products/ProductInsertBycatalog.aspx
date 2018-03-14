<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductInsertBycatalog.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductInsertBycatalog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="txttablename" runat="server">
    </asp:TextBox>
    <asp:Button ID="btnAddProduct" runat="server" Text="Insert" 
            onclick="btnAddProduct_Click" />&nbsp;
    <asp:Button ID="btnAddProduct0" runat="server" Text="Insert" 
            onclick="btnAddProduct0_Click" />

             <asp:Button ID="btnAddProduct1" runat="server" Text="Insert Data" 
            onclick="btnAddProduct1_Click" />
              <asp:Button ID="btnAddProduct2" runat="server" Text="Insert Vendor Data" onclick="btnAddProduct2_Click" 
             />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="ImportDatawayFair" />
            <asp:Button ID="Button5" runat="server" onclick="Button5_Click" 
            Text="ImportAtg" />

        Category:&nbsp;<asp:TextBox ID="txtCatId" runat="server" Text=""></asp:TextBox>&nbsp;<asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
            Text="Amazon Product" />

             B1: <asp:TextBox ID="txtb1" runat="server" Text=""></asp:TextBox> 
             B2: <asp:TextBox ID="txtb2" runat="server" Text=""></asp:TextBox> 
             B3: <asp:TextBox ID="txtb3" runat="server" Text=""></asp:TextBox> 
             B4: <asp:TextBox ID="txtb4" runat="server" Text=""></asp:TextBox> 
             B5: <asp:TextBox ID="txtb5" runat="server" Text=""></asp:TextBox> 
            <asp:Button ID="btnAmazon" runat="server" onclick="btnAmazon_Click" 
            Text="Amazon Image" />
             <asp:Button ID="Button3" runat="server" 
            Text="Sename" onclick="Button3_Click" />
            <asp:Button ID="Button4" runat="server" 
            Text="Ebay Insert" onclick="Button4_Click"   />
            <asp:Button ID="btnfabricvendor" runat="server" 
            Text="Fabric Vendor" onclick="btnfabricvendor_Click"  />
            <asp:Button ID="btnCustomSKu" runat="server" 
            Text="Custom SKU" onclick="btnCustomSKu_Click"  />
            <asp:Button ID="btnRoman" runat="server" 
            Text="Roman SKU" onclick="btnRoman_Click"  />
         <asp:Button ID="btnSearchpattern" runat="server" 
            Text="Search pattern" onclick="btnSearchpattern_Click"  />
    </div>
    <asp:TextBox ID="txtastoreid" runat="server"></asp:TextBox>
    </form>
</body>
</html>
