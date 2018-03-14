<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Category.aspx.cs" Inherits="Solution.UI.Web.Category" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        img, div, input
        {
            behavior: url("js/iepngfix.htc");
        }
    </style>
    <script type="text/javascript">
        function ShowModelQuick(id) {

            document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmquickview").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmquickview').height = '425px';
            document.getElementById('frmquickview').width = '750px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:750px;height:425px;border:solid 1px #7d7d7d;");
            document.getElementById('popupContact').style.width = '750px';
            document.getElementById('popupContact').style.height = '425px';
            window.scrollTo(0, 0);
            document.getElementById('btnreadmore').click();
            document.getElementById('frmquickview').src = '/QuickView.aspx?PID=' + id;

        }
        function adtocart(price, id) {
            if (document.getElementById('prepage') != null) {
                document.getElementById('prepage').style.display = '';
            }
            document.getElementById("ContentPlaceHolder1_hdnPrice").value = price;
            document.getElementById("ContentPlaceHolder1_hdnproductId").value = id;
            //document.getElementById("ContentPlaceHolder1_btnAddtocart").click();
        }
    </script>
    <div class="breadcrumbs">
        <asp:Literal ID="ltbreadcrmbs" runat="server"></asp:Literal>
    </div>
    <div class="featured-product-bg">
        <div class="fp-title">
            <h1>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></h1>
        </div>
        <div class="pro-cat-main">
            <div class="pro-cat-row">
                <asp:Repeater ID="RepSubCategory" runat="server" OnItemDataBound="RepSubCategory_ItemDataBound">
                    <HeaderTemplate>
                        
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                        <div class="pro-cat-box" id="Catbox" runat="server">
                            <div class="pro-cat-bg">
                                <div class="pro-cat-content">
                                    <h4>
                                    <a href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"SEName")) %>.html" title='<%#Eval("Name") %>'>
                                        <%#Eval("Name") %></a></h4>
                                    <div class="pro-cat-dec"><%#Eval("Description") %></div>
                                    <p> <a href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"SEName")) %>.html" title='<%#Eval("Name") %>'>and more...</a></p>
                                </div>
                                 <img title="" alt="" src="/images/sades-cat-box-shadow.jpg">
                            </div>


                            <div class="pro-cat-pro" id="CatDisplay" runat="server" style="height: 320px;">
                                
                                    
                                        <a href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"SEName")) %>.html"title='<%#Eval("Name") %>'>
                                            <asp:Image AlternateText='<%#Eval("Name").ToString()%>' ToolTip='<%#Eval("Name").ToString()%>' ImageUrl='<%# GetIconImageCategory(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName"))) %>'
                                                ID="imgCat" runat="server" Width="230" Height="309" />
                                        </a>
                                    
                                
                                
                                
                            </div>
                        </div>
                        <asp:Literal ID="ltbottom" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <FooterTemplate>
                        </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="fp-main" id="dvMessage" runat="server">
            <div class="fp-row1">
                <strong style="color: Red;">Content Not Found </strong>
            </div>
        </div>
        <div class="content-box1" id="divCatBanner" runat="server" visible="false">
            <asp:Repeater ID="RptCategoryDescription" runat="server" Visible="false">
                <ItemTemplate>
                    <div class="content-left">
                        <h1>
                            <%#Eval("HeaderText")%></h1>
                        <p>
                            <%#Eval("Description").ToString().Length<=0?Eval("Name"):Eval("Description")%>
                        </p>
                    </div>
                    <div class="content-right">
                        <asp:Image AlternateText='<%# Server.HtmlEncode(Convert.ToString(Eval("Name")))%>' ToolTip='<%# Server.HtmlEncode(Convert.ToString(Eval("Name")))%>' ImageUrl='<%# GetIconImageCategory(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName"))) %>'
                            ID="imgCat" class="img-right" Width="184" Height="141" runat="server" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <%-- commented--%>
    <div class="right-sub-section" style="display: none;">
        <h1>
            Best Sellers</h1>
        <div class="best-sellers-box">
            <asp:Repeater ID="rptBestSeller" runat="server" OnItemDataBound="rptBestSeller_ItemDataBound">
                <ItemTemplate>
                    <div id="ProboxBestSeller" runat="server" class="best-pro-box">
                        <div id="proDisplay" runat="server">
                            <div class="img-center">
                                <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx">
                                    <img src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' width="134"
                                        height="112" alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>" /></a></div>
                            <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                            <asp:Literal ID="lblTagImage" runat="server" Visible="false"></asp:Literal>
                            <asp:Label ID="lblTagImageName" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                            <h2 style="padding-top: 2px; padding-bottom: 2px; height: 42px;">
                                <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                    title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                    <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                            <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                            <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                            <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                            <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                            <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                            <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                            <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("RegularPrice")), 2)%>'></asp:Label>
                            <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                            <p>
                                <span><a href="javascript:void(0);" id="abestLink" runat="server" title="View More">
                                    <img src="/images/view_more.jpg" alt="View More" height="18" width="80" /></a></span></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
