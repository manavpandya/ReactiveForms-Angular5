<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCompare.aspx.cs"
    Inherits="Solution.UI.Web.ProductCompare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
     <link href='http://fonts.googleapis.com/css?family=Carrois+Gothic|Telex|Oxygen' rel='stylesheet' type='text/css' />
    <title></title>
    <style type="text/css" runat="server">
        .pro-compare-main
        {
            width: 100%;
            float: left;
            font-family: Verdana,Geneva,sans-serif;
        }
        .pro-compare-row
        {
            width: 100%;
            float: left;
            padding: 5px 0;
            text-align: right;
        }
        .pro-compare-box
        {
            width: 200px;
            float: left;
            margin: 0 10px 10px 0;
            border: 1px solid #c2c2c2;
            padding: 0;
        }
        .pro-compare-box h2
        {
            width: 190px;
            float: left;
            padding: 5px;
            margin: 0;
            font-size: 14px;
            color: #B92127;
            background: #E8E8E8;
            height: 56px;
            overflow: hidden;
        }
        .img-center
        {
            width: 190px;
            font-size: 14px;
            color: #B92127;
            text-align: center;
            vertical-align: middle;
            height: 160px;
            margin: 5px;
            border: 1px solid #c2c2c2;
        }
        .pro-compar-pt2
        {
            width: 190px;
            float: left;
            padding: 0 5px;
            margin: 0;
            height: 350px;
        }
        .pro-compar-pt2
        {
            width: 190px;
            float: left;
            padding: 0 5px;
            margin: 0;
        }
        .pro-compar-pt2 p
        {
            width: 190px;
            float: left;
            font-size: 12px;
            color: #464646;
            text-align: left;
            line-height: 18px;
            padding: 0;
            margin: 0;
        }
        .pro-compar-pt2 ul
        {
            width: 200px;
            float: left;
            list-style: none;
            padding: 0;
        }
        .pro-compar-pt2 ul li
        {
            width: 190px;
            float: left;
            font-size: 12px;
            color: #464646;
            padding: 2px 5px;
        }
        li
        {
            margin-left: 15px;
        }
    </style>
    <%-- <style type="text/css" runat="server">
        .pro-compare-main
        {
            width: 100%;
            float: left;
        }
        .pro-compare-row
        {
            width: 100%;
            float: left;
            padding: 5px 0;
        }
        .pro-compare-box
        {
            width: 200px;
            float: left;
            margin: 0 10px 10px 0;
            border: 1px solid #c2c2c2;
            padding: 0;
            font-style: normal;
        }
        .pro-compare-box h2
        {
            width: 190px;
            float: left;
            padding: 5px;
            margin: 0;
            font-size: 14px;
            color: #B92127;
            background: #FEEDDB;
            height: 56px;
            overflow: hidden;
        }
        .pro-compare-box .img-center
        {
            width: 190px;
            float: left;
            font-size: 14px;
            color: #B92127;
            text-align: center;
            vertical-align: middle;
            height: 160px;
            margin: 5px; /*border: 1px solid #c2c2c2;*/
        }
        .pro-compare-box .pro-compar-pt2
        {
            width: 190px;
            float: left;
            padding: 0 5px;
            margin: 0;
            height: 425px;
        }
        .pro-compare-box .pro-compar-pt2 p
        {
            width: 190px;
            float: left;
            font-size: 12px;
            color: #464646;
            text-align: left;
            line-height: 18px;
            padding: 0;
            margin: 0;
        }
        .pro-compare-box .pro-compar-pt2 p strong
        {
            width: 190px;
            float: left;
            font-size: 12px;
            color: #464646;
            text-align: left;
            line-height: 18px;
            padding: 0;
            margin: 0;
            font-weight: normal;
        }
        .pro-compare-box .pro-compar-pt2 ul
        {
            width: 200px;
            float: left;
            list-style: none;
            padding: 0;
        }
        .pro-compare-box .pro-compar-pt2 ul li
        {
            width: 190px;
            float: left;
            font-size: 12px;
            color: #464646;
            padding: 2px 5px;
        }
    </style>--%>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div class="pro-compare-main" id="ComProDiv" runat="server">
        <asp:Literal ID="ltrPageCompro" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
