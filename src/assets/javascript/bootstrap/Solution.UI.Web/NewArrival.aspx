<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NewArrival.aspx.cs" Inherits="Solution.UI.Web.NewArrival" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script type="text/javascript" src="/js/jquery.jcarousel.pack.js"></script>
    <script type="text/javascript" src="/js/gallery.js"></script>
    <script src="/js/popup.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/css/general.css" type="text/css" media="screen" />
    
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <div class="breadcrumbs" style="width: 67% !important;">
        <a href="/index.aspx" title="Home">Home </a>> <span id="spanbreadcrmbs" runat="server">New Arrival</span>
    </div>
    <asp:ScriptManager ID="Sc1" runat="server">
    </asp:ScriptManager>
      <div class="saleoutlate-banner">
        <img id="imgBanner" runat="server" hspace="0" border="0" vspace="0" class="img-left"/>
    </div>


<div style="float: left; margin: 10px; width: 100%;">

       <asp:UpdatePanel ID="UpdatePanelUpload" runat="server">
            <ContentTemplate>
                <div class="fp-main" id="dvMessage" runat="server" style="min-height: 200px; border: 1px solid #CCCCCC; width: 100% !important;"
                    visible="false">
                    <div class="fp-row1">
                        <div style="text-align: center; font-size: 18px; margin-top: 20px;">
                            <strong style="color: #848383;">Result Not Found</strong>
                        </div>
                    </div>
                </div>
                <div class="fp-main" id="topMiddle" runat="server" >
                    <div class="fp-row1">
                        <asp:Repeater ID="RptNewArrivalProduct" runat="server" OnItemDataBound="RptNewArrivalProduct_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                                <div class="fp-box" id="Probox" runat="server">
                                    <div class="fp-display" >
                                        <div class="fp-box-div" id="proDisplay" runat="server">
                                            <div class="img-center">
                                               
                                                <a id="innerAddtoCart" runat="server"  title='<%#Convert.ToString(Eval("Name"))%>'>

                                                    <img id='<%# "imgFeaturedProduct" + Convert.ToString(Container.ItemIndex +1) %>'
                                                 src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' alt='<%#Convert.ToString(Eval("Name"))%>'
                                                       title='<%#Convert.ToString(Eval("Name")) %>'
                                                        width="230" height="309" />
                                                     </a>
                                                    <img src="/images/quick-view-addtocart.png" id="imgAddToCart" runat="server"
                                                                alt="QUICK VIEW" class="preview" width="135" height="30" title="QUICK VIEW"/>

                                               
                                                <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                                                        <input type="hidden" id="hdnItemIndex" runat="server" value='<%#Container.ItemIndex%>' />
                                                <asp:Literal ID="lblTag" runat="server"></asp:Literal>
                                                <asp:Label ID="lblTName" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                                                     
                                            </div>
                                        </div>
                                        <h2 class="fp-box-h2" style="height: 45px;">
                                          
                                            <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title='<%#Convert.ToString(Eval("Name"))%>'>
                                                <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                                        <p class="fp-box-p">
                                            <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                                            <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                                          
                                            <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("Price")), 2)%>'></asp:Label>
                                            <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                                            <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                            <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                            <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                            <input type="hidden" id="hdnoptional" runat="server" value='<%#Convert.ToString(Eval("OptionalAccessories")).ToLower()%>' />
                                            <input type="hidden" id="hdnYourPrice" runat="server" value='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>' />
                                            <input type="hidden" id="hdnRegularPrice" runat="server" value='<%# Math.Round(Convert.ToDecimal(Eval("price")), 2)%>' />
                                            <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                                               <input type="hidden" id="ltrProductURL" runat="server" value='<%#Convert.ToString(Eval("ProductURL")).ToLower()%>' />
                                             <span><a href="javascript:void(0);"  id="aProductlink" runat="server" title="Add To Cart">
                                                <img style="margin-top: 5px;" src="/images/add-to-cart.png" alt="Add to Cart" title="Add to Cart">


                                                   </a>

                                                  <img id="outofStockDiv" visible="false" runat="server" style="margin-top: 5px;" src="/images/out-of-stock.png"
                                                    alt="OUT OF STOCK" title="OUT OF STOCK"/>
                                             </span>
                                        </p>
                                          <div class="rating" id="divSpace" runat="server" style="height: 20px; padding: 5px 0 0;">
                                        </div>
                                        <div class="rating" id="Divratinglist" runat="server">
                                            <asp:Literal ID="ltrating" runat="server"></asp:Literal><a style="cursor: default;">&nbsp;Rating
                                                <asp:Label ID="ltrRatingCount" runat="server"></asp:Label></a>
                                        </div>
                                    </div>
                                </div>
                                <asp:Literal ID="ltbottom" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul></FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
              
            </ContentTemplate>
          
        </asp:UpdatePanel>

    </div>
        <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
           <div style='float: right; background-color: transparent; right: -15px; top: -18px; position: absolute;'>
            <a href="javascript:void(0);" onclick="javascript:disablePopup();" title="">
                <img src="/images/popupclose.png" alt="" /></a>
        </div>
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <iframe id="frmdisplay1" frameborder="0" height="650" width="580" scrolling="auto"></iframe>
                    </td>
                </tr>
            </table>
        </div>
        </div>
         
        <div id="prepagequick" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 515px; width: 1015px; z-index: 1000; display: none;">
            <div style="border: 1px solid #ccc;">
                <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                    <tr>
                        <td>
                            <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc; width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">
                                <center>
                                    <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                    <b style="color: #fff;">Loading&nbsp;...&nbsp;...&nbsp;Please&nbsp;wait!</b></center>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
          <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div style="display: none;">
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <asp:ImageButton ID="popupContactClose" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hdnPrice" runat="server" />
        <input type="hidden" id="hdnproductId" runat="server" />
        <input type="hidden" id="hdnCompare" runat="server" />
        <input type="hidden" name="view" value="grid" id="view1" />
        <input type="hidden" id="hdnColorSelection" runat="server" value="" />
        <asp:HiddenField ID="hdncnt" runat="server" Value="12" />
        <asp:HiddenField ID="productcount" runat="server" Value="0" />
        <asp:HiddenField ID="divcount" runat="server" Value="1" />
        <input type="hidden" id="hdncheckproductid" value="0" runat="server" />
        <input type="hidden" id="hdncheckedproduct" value="0" runat="server" />
    </div>
    <script src="/js/JQueryStoreIndex.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/subcategoryscript.js"></script>
</asp:Content>
