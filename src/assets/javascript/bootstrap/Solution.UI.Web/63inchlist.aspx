<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="63inchlist.aspx.cs" Inherits="Solution.UI.Web._63inchlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="/js/bubbletooltips.js" type="text/javascript"></script>
    <script src="/js/jquery.tools.min.js" type="text/javascript"></script>
    <div class="breadcrumbs"><a href="/" title="Home">Home </a>
        <img src="/images/spacer.png" alt="63 Inch" title="63 Inch" class="breadcrumbs-bullet-icon" /><span> 63 Inch </span></div>
    <div class="featured-product-bg">
        <div class="fp-title">
            <h1>63Inch</h1>
        </div>
        <div class="pro-cat-main">
            <div class="kt-cat-row">
                <asp:Literal ID="ltrproduct" runat="server"></asp:Literal>

            </div>
        </div>
    </div>

    <style type="text/css">
        .tooltip {
            background: url("/images/black_arrow.png") repeat scroll 0 0 rgba(0, 0, 0, 0);
            color: #EEEEEE;
            display: none;
            font-size: 12px;
            height: 370px;
            padding: 11px;
            width: 230px;
        }

        .divtooltipover a {
            color: #FFFFFF;
        }

        .divtooltipover {
            width: 96%;
            padding: 5px 2%;
            font-size: 14px;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">

        $(function () {
            $(".ulcatallimg > li > a > img[title]").tooltip({
                offset: [10, 2], effect: 'slide'
            }).dynamic({
                bottom: { direction: 'down', bounce: true }
            });
        });
    </script>

</asp:Content>
