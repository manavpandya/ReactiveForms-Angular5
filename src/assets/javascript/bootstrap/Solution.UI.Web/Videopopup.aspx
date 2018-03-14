<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Videopopup.aspx.cs" Inherits="Solution.UI.Web.Videopopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link href="css/style.css?78788" rel="stylesheet" type="text/css" />
    <style type="text/css" >
        .readymade-detail-pt1-pro {
    background-color: #f8f8f8;
    background-image: linear-gradient(to bottom, #fcfcfc, #f4f3f3);
    background-repeat: repeat-x;
    border: 1px solid #ddd;
    color: #b92127;
    cursor: pointer;
    font-size: 14px;
    padding: 0;
    text-transform: uppercase;
    width: 98%;
    margin-left: 4px; margin-top: 5px; padding-left: 4px;
    line-height:2;
    }
        .static-detail
        {
            width: 98% ! important;border-top:none; border: 1px solid rgb(229, 229, 229); margin-left: 4px; padding-left: 4px; margin-bottom: 5px;border-top:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="ltrVediodetail" runat="server"></asp:Literal><br />
    <asp:Literal ID="ltvide" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
