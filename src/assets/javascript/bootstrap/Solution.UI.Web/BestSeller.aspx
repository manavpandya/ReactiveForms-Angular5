<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BestSeller.aspx.cs" Inherits="Solution.UI.Web.BestSeller" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script type="text/javascript" src="/js/jquery.jcarousel.pack.js"></script>
    <script type="text/javascript" src="/js/gallery.js"></script>
    <style type="text/css">
        img, div, input
        {
            behavior: url("js/iepngfix.htc");
        }
    </style>
       <script type="text/javascript">

           function IndexValidation() {
               if (document.getElementById("ContentPlaceHolder1_txtFrom") != null && document.getElementById("ContentPlaceHolder1_txtFrom").value == "") {
                   alert("Please enter from price.");
                   document.getElementById("ContentPlaceHolder1_txtFrom").focus();
                   return false;
               }
               else if (document.getElementById("ContentPlaceHolder1_txtTo") != null && document.getElementById("ContentPlaceHolder1_txtTo").value == "") {
                   alert("Please enter to price.");
                   document.getElementById("ContentPlaceHolder1_txtTo").focus();
                   return false;
               }
               else {
                   var fromPrice = parseFloat(document.getElementById("ContentPlaceHolder1_txtFrom").value);
                   var toPrice = parseFloat(document.getElementById("ContentPlaceHolder1_txtTo").value);
                   if (parseFloat(fromPrice) > parseFloat(toPrice)) {
                       alert("Please enter valid price range.");
                       document.getElementById("ContentPlaceHolder1_txtFrom").focus();
                       return false;
                   }
                   else {
                       document.getElementById("ContentPlaceHolder1_hdnallpages").value = '2';
                       return true;
                   }
               }
               document.getElementById("ContentPlaceHolder1_hdnallpages").value = '2';
               return true;
           }

           function CheckSelection() {
               document.getElementById("ContentPlaceHolder1_hdnallpages").value = '1';
               document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();

           }

           function ColorSelection(clrvalue) {


               document.getElementById("ContentPlaceHolder1_hdnallpages").value = '1';
               document.getElementById("ContentPlaceHolder1_hdnColorSelection").value = clrvalue;
               //document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();

               document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
           }
           function pagingSelection(pageval) {


               document.getElementById("ContentPlaceHolder1_hdnallpages").value = '8';
               document.getElementById("ContentPlaceHolder1_hdnpagenumber").value = pageval;
               if (document.getElementById("ContentPlaceHolder1_hdnquickview").value == '1') {
                   document.getElementById('form1').submit();
               }

           }
           function keyRestrict(e, validchars) {
               var key = '', keychar = '';
               key = getKeyCode(e);
               if (key == null) return true;
               keychar = String.fromCharCode(key);
               keychar = keychar.toLowerCase();
               validchars = validchars.toLowerCase();
               if (validchars.indexOf(keychar) != -1)
                   return true;
               if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
                   return true;
               return false;
           }
           function getKeyCode(e) {
               if (window.event)
                   return window.event.keyCode;
               else if (e)
                   return e.which;
               else
                   return null;
           }

           function unselectcheckbox(chkelement) {
               var allElts = document.getElementById('divPrice').getElementsByTagName('INPUT');
               var i;
               for (i = 0; i < allElts.length; i++) {
                   var elt = allElts[i];
                   if (elt.type == "checkbox") {
                       if (elt.name != chkelement) {
                           elt.checked = false;
                       }
                   }
               }
               CheckSelection();
           }

           //for newsletter
           function ValidNewSletter() {
               var element; if (document.getElementById('txtSubscriber'))
               { element = document.getElementById('txtSubscriber'); }
               if (document.getElementById('txtSubscriber'))
               { element = document.getElementById('txtSubscriber'); }
               if (element.value == '') {
                   alert('Please enter your E-mail Address.'); if (document.getElementById('txtSubscriber'))
                   { document.getElementById('txtSubscriber').focus(); }
                   if (document.getElementById('txtSubscriber'))
                   { document.getElementById('txtSubscriber').focus(); }
                   return false;
               }
               else if (element.value == 'Enter your E-Mail Address') {
                   alert('Please enter your E-Mail Address.'); if (document.getElementById('txtSubscriber'))
                   { document.getElementById('txtSubscriber').focus(); }
                   if (document.getElementById('txtSubscriber'))
                   { document.getElementById('txtSubscriber').focus(); }
                   return false;
               }
               else {
                   var testresults; var str = element.value; var filter = /^.+@.+\..{2,3}$/; if (filter.test(str))
                   { return true; }
                   else {
                       alert("Please enter valid E-Mail Address.")
                       if (document.getElementById('txtSubscriber'))
                       { document.getElementById('txtSubscriber').focus(); }
                       if (document.getElementById('txtSubscriber'))
                       { document.getElementById('txtSubscriber').focus(); }
                       return false;
                   }
               }
           }
           function clear_text() {
               if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value == "Enter your E-Mail Address")
               { document.getElementById('txtSubscriber').value = ""; }
               if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value == "Enter your E-Mail Address")
               { document.getElementById('txtSubscriber').value = ""; }
               return false;
           }
           function clear_NewsLetter(myControl) {
               if (myControl && myControl.value == "Enter your E-Mail Address")
                   myControl.value = "";
           }
           function ChangeNewsLetter(myControl) {
               if (myControl != null && myControl.value == '')
                   myControl.value = "Enter your E-Mail Address";
           }
           function Change() {
               if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
                   document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
                   document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; return false;
           }


           //For Search
           function clear_Search(myControl) {
               if (myControl && myControl.value == "Search by Keyword")
                   myControl.value = "";
           }
           function ChangeSearch(myControl) {
               if (myControl != null && myControl.value == '')
                   myControl.value = "Search by Keyword";
           }


           function ValidSearch() {
               var myControl;

               if (document.getElementById('txtSearch')) {
                   myControl = document.getElementById('txtSearch');
               }



               if (myControl.value == '' || myControl.value == 'Search by Keyword') {
                   alert("Please enter something to search");

                   if (document.getElementById('txtSearch')) {
                       document.getElementById('txtSearch').focus();
                   }


                   return false;
               }

               if (myControl.value.length < 3) {
                   alert("Please enter at least 3 characters to search");
                   myControl.focus();
                   return false;
               }
               return true;
           }
           function unselectcheckboxforCustom(chkelement) {
               var allElts = document.getElementById('divCustom').getElementsByTagName('INPUT');
               var i;
               for (i = 0; i < allElts.length; i++) {
                   var elt = allElts[i];
                   if (elt.type == "checkbox") {
                       if (elt.name != chkelement) {
                           elt.checked = false;
                       }
                   }
               }
               CheckSelection();
           }
    </script>
    <script type="text/javascript">
        function chkHeight() {

            if (document.getElementById('prepage')) {

                document.getElementById('prepage').style.display = '';
            }
        }

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
            // document.getElementById("ContentPlaceHolder1_btnAddtocart").click();
        }
    </script>
    <script type="text/javascript">

        function Loader() {
            if (document.getElementById('prepage') != null) {
                document.getElementById('prepage').style.display = '';
            }
        }
    </script>
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a>> <span>
            <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></span>
    </div>
    <asp:ScriptManager ID="Sc1" runat="server">
    </asp:ScriptManager>
    <div class="option-pro-main" id="hideIndexOptionDiv" runat="server">
        <div class="colors-box">
            <div class="option-probox-title">
                <span>Colors</span></div>
            <asp:Literal ID="ltrColor" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Header</span></div>
            <asp:Literal ID="ltrHeader" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Pattern</span></div>
            <asp:Literal ID="ltrPattern" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Fabric</span></div>
            <asp:Literal ID="ltrFabric" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Style</span></div>
            <asp:Literal ID="ltrStyle" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Custom</span></div>
            <div class="toggle1" id="divCustom">
                <ul id="mycarousel6" class="jcarousel-skin-tango2">
                    <li>
                        <ul class="option-pro">
                            <li class="pattern-pro-box">
                                <input type="checkbox" class="checkbox" name="chkCustom_Yes" value="Yes" onchange="unselectcheckboxforCustom('chkCustom_Yes');"
                                    onclick="unselectcheckboxforCustom('chkCustom_Yes');" />
                                <span>Yes</span></li>
                            <li class="pattern-pro-box">
                                <input type="checkbox" class="checkbox" name="chkCustom_No" value="No" onchange="unselectcheckboxforCustom('chkCustom_No');"
                                    onclick="unselectcheckboxforCustom('chkCustom_No');" />
                                <span>No</span> </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div class="price-box-main">
            <div class="option-probox-title">
                <span>Price</span>
            </div>
            <div class="toggle1" id="divPrice" style="height: 238px;">
                <div class="price-box-bg">
                    <ul>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_0" value="< 10" onchange="unselectcheckbox('chkPrice_0');" />
                            <span>Less than $10</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_1" value=">= 10 ~ <= 20" onchange="unselectcheckbox('chkPrice_1');"
                                onclick="unselectcheckbox('chkPrice_1');" />
                            <span>$10 to $20</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_2" value=">= 20 ~ <= 40" onchange="unselectcheckbox('chkPrice_2');"
                                onclick="unselectcheckbox('chkPrice_2');" />
                            <span>$20 to $40</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_3" value=">= 40 ~ <= 60" onchange="unselectcheckbox('chkPrice_3');"
                                onclick="unselectcheckbox('chkPrice_3');" />
                            <span>$40 to $60</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_4" value=">= 60 ~ <= 80" onchange="unselectcheckbox('chkPrice_4');"
                                onclick="unselectcheckbox('chkPrice_4');" />
                            <span>$60 to $80</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_5" value=">= 80 ~ <= 100"
                                onchange="unselectcheckbox('chkPrice_5');" onclick="unselectcheckbox('chkPrice_5');" />
                            <span>$80 to $100</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_6" value=">= 100 ~ <= 200"
                                onchange="unselectcheckbox('chkPrice_6');" onclick="unselectcheckbox('chkPrice_6');" />
                            <span>$100 to $200</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_7" value="> 200" onchange="unselectcheckbox('chkPrice_7');"
                                onclick="unselectcheckbox('chkPrice_7');" />
                            <span>Greater then $200</span></li>
                    </ul>
                    <div class="qty-box">
                        <p>
                            $<span>
                                <asp:TextBox ID="txtFrom" runat="server" MaxLength="3" Width="28px" CssClass="qty-input"
                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                            </span>to $<span>
                                <asp:TextBox ID="txtTo" runat="server" MaxLength="3" Width="28px" CssClass="qty-input"
                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                            </span>
                        </p>
                        <asp:ImageButton ID="btnIndexPriceGo" runat="server" ImageUrl="/images/go-button.jpg"
                            OnClientClick="return IndexValidation();" ToolTip="Go" OnClick="btnIndexPriceGo_Click" /><asp:ImageButton
                                ID="btnIndexPriceGo1" runat="server" Style="display: none;" ImageUrl="/images/go-button.jpg"
                                ToolTip="Go" OnClick="btnIndexPriceGo1_Click" />
                    </div>
                </div>
            </div>
        </div>
        <a href="#" id="toggle1">
            <img src="/images/option-arrow-up.png" height="31" width="28" alt="arrow" id="option-arrow"></a>
    </div>
    <div class="featured-product-bg">
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div class="fp-title">
                    <h1>
                        <asp:Literal ID="ltTitle" runat="server"></asp:Literal></h1>
                </div>
                <div class="numbering" id="divTopBestSeller" runat="server" visible="false">
                    <p>
                        <div id="divTopPaging" runat="server" visible="false">
                            <div class="paging-pt1">
                                Sort by:
                                <asp:DropDownList ID="ddlSortby" runat="server" CssClass="select-box" Style="width: 100px;"
                                    AutoPostBack="True" onchange="Loader();" OnSelectedIndexChanged="ddlSortby_SelectedIndexChanged">
                                    <asp:ListItem>Price Range</asp:ListItem>
                                    <asp:ListItem>Low to High</asp:ListItem>
                                    <asp:ListItem>High to Low</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="paging-pt3" style="width: 330px;">
                                <div class="paging-box" style="width: 330px;">
                                    <asp:LinkButton ID="lnkViewAll" runat="server" Style="height: 19px; width: 60px;
                                        float: right; margin-top: 0px; padding-left: 5px; margin-left: 18px;" Width="60"
                                        Height="19" OnClick="lnkViewAll_Click">View All</asp:LinkButton>
                                    <asp:LinkButton ID="lnktopNext" runat="server" OnClick="lnkNext_Click" Style="float: right;
                                        font-weight: normal; color: #505050; background: none;">Next</asp:LinkButton>
                                    <div>
                                        <asp:DataList ID="dlToppaging" runat="server" HorizontalAlign="Right" RepeatDirection="Horizontal"
                                            OnItemCommand="RepeaterPaging_ItemCommand" OnItemDataBound="RepeaterPaging_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="Pagingbtn" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="newpage" Text='<%# Eval("PageText") %> '></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                    <asp:LinkButton ID="lnkTopprevious" runat="server" OnClick="lnkPrevious_Click" Style="float: right;
                                        font-weight: normal; margin-right: 45px; color: #505050; background: none;">Previous</asp:LinkButton>
                                    <span style="float: right;">Pages</span>
                                </div>
                            </div>
                        </div>
                        <div class="paging-pt3" visible="false">
                            <asp:LinkButton ID="lnkViewAllPages" runat="server" Width="70px" OnClick="lnkViewAllPages_Click">View Pages</asp:LinkButton></div>
                    </p>
                </div>
                <div class="fp-main">
                    <div class="fp-row1">
                        <asp:Repeater ID="RptNewArrivalProduct" runat="server" OnItemDataBound="RptNewArrivalProduct_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                                <div class="fp-box" id="Probox" runat="server">
                                    <div class="fp-display">
                                        <div class="fp-box-div">
                                            <div class="img-center" id="proDisplay" runat="server">
                                                <%-- <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                                    title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                                <a href="javascript:void(0);" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                    <img id='<%# "imgFeaturedProduct" + Convert.ToString(Container.ItemIndex +1) %>'
                                                        src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                                        width="230" height="309" /><img src="/images/imgloader.gif" alt="" id='<%# "loader_img" + Convert.ToString(Container.ItemIndex +1) %>'
                                                            style="text-align: center; vertical-align: middle;" /><img src="/images/view-detail.png"
                                                                alt="View Detail" class="preview" width="150" height="30" title="View Detail"></a>
                                                <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                                                <asp:Literal ID="lblTag" runat="server"></asp:Literal>
                                                <asp:Label ID="lblTName" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                                            </div>
                                        </div>
                                        <h2 class="fp-box-h2" style="height: 30px;">
                                            <%--  <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                                title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                            <a href="javascript:void(0);" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                                        <p class="fp-box-p">
                                            <asp:Literal ID="ltrRegularPrice" Visible="false" runat="server"></asp:Literal>
                                            <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                                            <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("Price")), 2)%>'></asp:Label>
                                            <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                                            <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                            <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                            <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                            <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                                            <a id="anewArrival" runat="server" title="Add To Cart" visible="false">
                                                <img src="/images/add-to-cart.jpg" alt="Add To Cart" /></a>
                                        </p>
                                    </div>
                                </div>
                                <asp:Literal ID="ltbottom" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul></FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <asp:DropDownList Visible="false" ID="ddlbottomprice" runat="server" CssClass="select-box"
                    Style="width: 100px;" AutoPostBack="True" onchange="Loader();" OnSelectedIndexChanged="ddlbottomprice_SelectedIndexChanged">
                    <asp:ListItem>Price Range</asp:ListItem>
                    <asp:ListItem Value="Low to High">Low to High</asp:ListItem>
                    <asp:ListItem Value="High to Low">High to Low</asp:ListItem>
                </asp:DropDownList>
                <div class="numbering" id="divBottomPaging" runat="server" style="display: none;"
                    visible="false">
                    <span class="num-prev">
                        <asp:LinkButton ID="lnkBottomViewAll" Style="margin-right: 5px; font-weight: bold;"
                            runat="server" OnClick="lnkViewAll_Click">View All</asp:LinkButton>
                        <asp:LinkButton ID="lnkPrevious" runat="server" OnClick="lnkPrevious_Click">Previous</asp:LinkButton>
                    </span><span class="num-next">
                        <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next</asp:LinkButton></span>
                    <asp:DataList ID="RepeaterPaging" runat="server" HorizontalAlign="Center" RepeatDirection="Horizontal"
                        OnItemCommand="RepeaterPaging_ItemCommand" OnItemDataBound="RepeaterPaging_ItemDataBound">
                        <ItemTemplate>
                            <p>
                                <asp:LinkButton ID="Pagingbtn" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                    CommandName="newpage" Text='<%# Eval("PageText") %> '></asp:LinkButton></p>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div class="numbering" id="divViewAllPagesBottom" runat="server" style="display: none;"
                    visible="false">
                    <span class="num-prev">
                        <asp:LinkButton ID="lnkBottomViewAllPages" OnClientClick="chkHeight();" runat="server"
                            Visible="false" OnClick="lnkViewAllPages_Click" Style="font-weight: bold;">View Pages</asp:LinkButton></span></div>
                <div id="prepage" style="width: 100%; height: 100%; position: fixed; top: 0%; left: 0%;
                    z-index: 2000; display: none; background: url(../images/loaderbg.png) repeat !important;
                    display: none;">
                    <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                        <tr>
                            <td>
                                <img alt="" src="/images/loding.gif" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlSortby" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <%--  <iframe id="frmquickview" frameborder="0" height="425" width="750" scrolling="auto">
                        </iframe>--%>
                    </td>
                </tr>
            </table>
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
            <%--  <asp:Button ID="btnAddtocart" OnClick="btnAddToCart_Click" runat="server" />--%>
        </div>
        <div style="display: none;">
            <input type="hidden" id="hdnCompare" runat="server" />
            <input type="hidden" name="view" value="grid" id="view1" />
            <input type="hidden" id="hdnColorSelection" runat="server" value="" />
            <asp:HiddenField ID="hdncnt" runat="server" Value="12" />
            <asp:HiddenField ID="productcount" runat="server" Value="0" />
            <asp:HiddenField ID="divcount" runat="server" Value="1" />
            <input type="hidden" id="hdncheckproductid" value="0" runat="server" />
            <input type="hidden" id="hdnallpages" value="0" runat="server" />
            <input type="hidden" id="hdnpagenumber" value="0" runat="server" />
            <input type="hidden" id="hdnquickview" value="0" runat="server" />
            <input type="hidden" id="hdncheckedproduct" value="0" runat="server" />
            <asp:Button ID="btntempclick" runat="server" Text="" OnClick="btntempclick_Click" />
        </div>
    </div>
    <script type="text/javascript" src="js/script.js"></script>
</asp:Content>
