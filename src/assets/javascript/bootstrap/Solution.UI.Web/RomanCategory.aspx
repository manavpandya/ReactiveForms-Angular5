<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RomanCategory.aspx.cs" Inherits="Solution.UI.Web.RomanCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        /*Roman Category CSS*/
        .shades-cat-main
        {
            float: left;
            width: 100%;
            padding: 10px 0;
        }
        .shades-cat-row1
        {
            float: left;
            width: 100%;
            padding: 0 0 10px 0;
        }
        /*.shades-cat-row1 img{float:left; width:100%;}*/
        .shades-cat-pt1
        {
            float: left;
            width: 49.5%;
            padding: 10px 0 0 0;
        }
        .shades-cat-pt2
        {
            float: right;
            width: 49.5%;
            padding: 10px 0 0 0;
        }
        .shades-cat-bg
        {
            float: left;
            width: 48%;
            background: #e8e8e8;
            padding: 0;
        }
        .shades-cat-box
        {
            float: left;
            width: 65%;
            background: #e8e8e8;
            padding: 28px 20% 5px 15%;
        }
        .shades-cat-box h4
        {
            float: left;
            font-family: 'Telex' ,sans-serif;
            text-align: left;
            border-bottom: 1px solid #808080;
            font-size: 28px;
            line-height: 35px;
            padding: 0 0 10px 0;
        }
        .shades-cat-box p
        {
            float: left;
            font-family: 'Telex' ,sans-serif;
            font-size: 14px;
            color: #000404;
            line-height: 18px;
            padding: 15px 0 0 0;
            mini-height: 20px;
            overflow: hidden;
        }
        .shades-color-box
        {
            float: left;
            width: 86%;
            min-height:20px !important;
            overflow: hidden;
            background: #fff;
            margin: 16px 0 0 0;
            padding: 0 1% 5px;
        }
        .shades-color-box a img
        {
            width: 33px;
            height: 33px;
            margin: 5px 0 0 3px;
        }
        /*.shades-cat-pro{float:right; width:50%; background:#000;}*/
        .shades-cat-pro
        {
            float: right;
            width: 50%;
            background: #fff;
        }
        .shades-cat-pro a img
        {
            height: auto;
        }
        .shades-cat-box p a
        {
            float: left;
            font-family: 'Telex' ,sans-serif;
            font-size: 14px;
            color: #000404;
            text-decoration: none;
            margin: 0;
        }
        .shades-cat-row1 a img
        {
            float: left;
            width: 100%;
        }
    </style>
    <script type="text/javascript">

        function variantlink(lnk, vid) {
            document.getElementById('ContentPlaceHolder1_hdnLink').value = lnk;
            document.getElementById('ContentPlaceHolder1_hdnVariant').value = vid;
            __doPostBack('ctl00$ContentPlaceHolder1$btnSave', '');
        }
    
    </script>
     <div class="breadcrumbs" style="width: 67% !important;">
        <asp:Literal ID="ltbreadcrmbs" runat="server"></asp:Literal>
    </div>
    <div class="shades-cat-main">
        <div class="shades-cat-row1">
            <div class="f-like">
                <span>Like Us For A Chance To Win A $50 HPD Coupon. </span>
                <iframe src="http://www.facebook.com/plugins/like.php?app_id=166725596724792&amp;href=rURL<%=Request.RawUrl  %>&amp;send=false&amp;layout=button_count&amp;width=75
&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font&amp;height=21" scrolling="no"
                    frameborder="0" style="border: medium none; overflow: hidden; float: right; width: 82px;
                    height: 21px; margin-top: 2px; margin-right: 4px;" allowtransparency="true">
                </iframe>
            </div>
        </div>
        <div class="shades-cat-row1" style="display:none;">
            <%--  <a href="#">
                <img src="/images/roman-sades-banner.jpg" alt="" title=""></a>--%>
            <img id="imgBanner" runat="server" style="width: 1440px; height: 376px;" />
        </div>
        <asp:Repeater ID="RepRomanCategory" runat="server" OnItemDataBound="RepRomanCategory_ItemDataBound">
            <HeaderTemplate>
                <div class="shades-cat-row1">
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                <div class="shades-cat-pt1" id="Catbox" runat="server">
                    <div class="shades-cat-bg" style="height: 320px;">
                        <div class="shades-cat-box" style="height: 288px;">
                            <h4 style="height: 30px; font-size: 18px; line-height: 20px;">
                                <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                    Visible="false"></asp:Label>
                                <a style="color: #000000;" href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ProductURL")) %>"
                                    title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                    <%# SetName(Convert.ToString(DataBinder.Eval(Container.DataItem, "Name")))%></a>
                            </h4>
                            <p id="ColorMessage" runat="server" visible="false">
                                <asp:Literal ID="ltshiprange" runat="server" Text="Made to Measure.Ship in 3-4 Week."></asp:Literal>
                                </p>
                            <div class="shades-color-box" style="background: none;" id="divWithoutVariantOption"
                                runat="server" visible="false">
                            </div>
                            <div class="shades-color-box" id="divVariantOption" runat="server" visible="false"
                                style="float: left;">
                                <asp:Literal ID="ColorOption" runat="server"></asp:Literal>
                            </div>
                            <div class="shades-color-box" id="divForSinglecolorLine" runat="server" visible="false"
                                style="mini-height: 20px !important; background: none; float: left;">
                            </div>
                            <p id="ViewMoreOption" runat="server" visible="false">
                                <a href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ProductURL")) %>"
                                    title="and more">and more...</a></p>
                        </div>
                        <img title="" alt="" src="/images/sades-cat-box-shadow.jpg">
                    </div>
                    <div class="shades-cat-pro" id="catDisplay" runat="server" style="height: 320px;">
                        <a href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ProductURL")) %>">
                            <asp:Image ImageUrl='<%# GetIconImageProduct(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName"))) %>'
                                ID="imgSubCat" runat="server" AlternateText='<%# Server.HtmlEncode(Convert.ToString(DataBinder.Eval(Container.DataItem,"Name"))) %>' ToolTip='<%# Server.HtmlEncode(Convert.ToString(DataBinder.Eval(Container.DataItem,"Name"))) %>' Style="height: 320px;" />
                        </a>
                        <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                        <asp:Literal ID="lblTag" runat="server"></asp:Literal>
                        <asp:Label ID="lblTName" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div></FooterTemplate>
        </asp:Repeater>
    </div>
    <div style="display: none">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        <input type="hidden" id="hdnLink" runat="server" value="" />
        <input type="hidden" id="hdnVariant" runat="server" value="" />
    </div>
</asp:Content>
