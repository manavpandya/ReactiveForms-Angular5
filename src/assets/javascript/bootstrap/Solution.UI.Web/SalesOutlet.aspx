<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SalesOutlet.aspx.cs" Inherits="Solution.UI.Web.SalesOutlet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script type="text/javascript" src="/js/jquery.jcarousel.pack.js"></script>
    <script type="text/javascript" src="/js/gallery.js"></script>
    <script src="/js/popup.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/css/general.css" type="text/css" media="screen" />
    <script type="text/javascript">


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

    </script>
    <script type="text/javascript">

       
        
    </script>
    <script type="text/javascript">

        function ShowModelQuick(id) {

            document.getElementById("frmdisplayquick").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplayquick').height = '500px';
            document.getElementById('frmdisplayquick').width = '1000px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:1000px;height:500px;position:absolute;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact').style.width = '1000px';
            document.getElementById('popupContact').style.height = '500px';
            //            window.scrollTo(0, 0);



            document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id;
            centerPopup();
            loadPopup();

            //            centerPopup1();
            //            loadPopup1();
        }
     
      
    </script>
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <div class="breadcrumbs" style="width: 67% !important;">
        <a href="/Index.aspx" title="Home">Home </a>> <span id="spanbreadcrmbs" runat="server">
            Sales Outlet</span>
    </div>
    <div class="saleoutlate-banner">
        <img id="imgBanner" runat="server" hspace="0" border="0" vspace="0"  class="img-left">
    </div>
    <div style="float: left; margin: 10px; width: 100%;">
        <asp:UpdatePanel ID="UpdatePanelUpload" runat="server">
            <ContentTemplate>
                <div class="fp-main" id="dvMessage" runat="server" style="min-height: 200px; border: 1px solid #CCCCCC;
                    width: 100% !important;" visible="false">
                    <div class="fp-row1">
                        <div style="text-align: center; font-size: 18px; margin-top: 20px;">
                            <strong style="color: #848383;">Result Not Found</strong>
                        </div>
                    </div>
                </div>
                <div class="fp-main" id="topMiddle" runat="server">
                    <div class="fp-row1">
                        <asp:Repeater ID="RepProduct" runat="server" OnItemDataBound="RepProduct_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                                <div class="fp-box" id="Probox" runat="server">
                                    <div class="fp-display">
                                        <div class="fp-box-div" id="proDisplay" runat="server">
                                            <div class="img-center">
                                                <%--   <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                                <a id="innerAddtoCart" runat="server">
                                                    <asp:Image ImageUrl='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                        ID="imgName" AlternateText='<%#Convert.ToString(Eval("Name")) %>' ToolTip='<%#Convert.ToString(Eval("Name")) %>' runat="server" Width="230" Height="309" /></a><img id="imgAddToCart" runat="server" src="/images/quick-view-addtocart.png"
                                                                alt="QUICK VIEW" class="preview" width="135" height="30" title="QUICK VIEW">
                                                <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                                                 <input type="hidden" id="hdnItemIndex" runat="server" value='<%#Container.ItemIndex%>' />
                                                <asp:Literal ID="lblTag" runat="server"></asp:Literal>
                                                <asp:Label ID="lblTName" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                                            </div>
                                        </div>
                                        <h2 class="fp-box-h2" style="height: 45px;">
                                            <%-- <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                                title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                            <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                <%# SetName(Convert.ToString(Eval("Name")))%>
                                            </a>
                                        </h2>
                                        <p class="fp-box-p">
                                            <span>
                                                <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                                                <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                                            </span>
                                            <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                            <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                                            <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                            <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                            <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("price")), 2)%>'></asp:Label>
                                            <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                                            <input type="hidden" id="hdnRegularPrice" runat="server" value='<%#Convert.ToString(Eval("price")).ToLower()%>' />
                                            <input type="hidden" id="hdnYourPrice" runat="server" value='<%#Convert.ToString(Eval("SalePrice")).ToLower()%>' />
                                            <input type="hidden" id="ltrProductURL" runat="server" value='<%#Convert.ToString(Eval("ProductURL")).ToLower()%>' />
                                            <span><a href="javascript:void(0);" id="aProductlink" runat="server" title="Add to Cart">
                                                <img style="margin-top: 5px;" src="/images/add-to-cart.png" alt="Add to Cart" title="Add to Cart"></a>
                                                <img id="outofStockDiv" visible="false" runat="server" style="margin-top: 5px;" src="/images/out-of-stock.png"
                                                    alt="OUT OF STOCK" title="OUT OF STOCK">
                                            </span><span>
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
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="popupContact1" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
        <div style='float: right; background-color: transparent; right: -15px; top: -18px;
            position: absolute;'>
            <a href="javascript:void(0);" onclick="javascript:disablePopup();" title="">
                <img src="/images/popupclose.png" alt="" /></a>
        </div>
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <iframe id="frmdisplay1" frameborder="0" height="650" width="580" scrolling="auto">
                        </iframe>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
        <div style='float: right; background-color: transparent; right: -15px; top: -18px;
            position: absolute;'>
            <a href="javascript:void(0);" onclick="javascript:document.getElementById('ContentPlaceHolder1_popupContactClose').click();"
                title="">
                <img src="/images/popupclose.png" alt="" /></a>
        </div>
        <div style='float: right; background-color: transparent; right: -11px; top: 49%;
            position: absolute;'>
            <input type="button" class="next-productdisabled" id="subnext" runat="server">
        </div>
        <div style='float: left; background-color: transparent; left: -11px; top: 49%; position: absolute;'>
            <input type="button" class="pre-productdisabled" id="subpre" runat="server">
        </div>
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <iframe id="frmdisplayquick" frameborder="0" height="650" width="580" scrolling="auto">
                        </iframe>
                    </td>
                </tr>
            </table>
        </div>
        <div id="prepagequick" style="position: absolute; font-family: arial; font-size: 16;
            left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70);
            layer-background-color: white; height: 515px; width: 1015px; z-index: 1000; display: none;">
            <div style="border: 1px solid #ccc;">
                <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                    <tr>
                        <td>
                            <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc;
                                width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px; -moz-border-radius: 10px;
                                border-radius: 10px;">
                                <center>
                                    <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                    <b style="color: #fff;">Loading&nbsp;...&nbsp;...&nbsp;Please&nbsp;wait!</b></center>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%--<input type="button" class="prev-nav-btn" disabled="disabled">--%>
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
