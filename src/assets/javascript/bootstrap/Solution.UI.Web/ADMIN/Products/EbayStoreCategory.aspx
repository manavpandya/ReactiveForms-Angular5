﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EbayStoreCategory.aspx.cs" Inherits="Solution.UI.Web.EbayStoreCategory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:TreeView ID="trvCategories" runat="server" PopulateNodesFromClient="False" ShowCheckBoxes="Leaf"
                Width="304px" ShowLines="True">
            </asp:TreeView>
            &nbsp:&nbsp;<asp:Button ID="btnAddCate" runat="server" Text="Set Category" OnClick="btnAddCate_Click">
            </asp:Button>
    </div>
    </form>
</body>
</html>
