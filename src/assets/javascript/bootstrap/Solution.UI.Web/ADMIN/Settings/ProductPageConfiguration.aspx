<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductPageConfiguration.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.ProductPageConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Admin/js/jquery-onoff.js" type="text/javascript"></script>
    <script src="/Admin/js/jquery-switch.js" type="text/javascript"></script>
    <script type="text/javascript">
        function MakeCheckedall(flag, id) {
            var arrflag = flag.split(',');
            var arrid = id.split(',');
            if (arrflag.length == arrid.length)
                for (var i = 0; i < arrflag.length; i++) {
                    if (arrflag[i].toString() == "true") {
                        document.getElementById(arrid[i].toString()).checked = true;
                    }
                    else {
                        document.getElementById(arrid[i].toString()).checked = false;
                    }
                }
    }

    function CheckState(id) {
        var i = "";
        if (document.getElementById(id).checked) {
            i = "on";
        }
        else {
            i = "off";
        }
        return i;
    }

    function Getstatusall(chkID, DivID, btnID, txtID) {
        var arrchk = chkID.split(',');
        var arrdiv = DivID.split(',');
        var arrtxt = txtID.split(',');
        if (arrchk.length == arrdiv.length)
            for (var i = 0; i < arrdiv.length; i++) {
                var state = CheckState(arrchk[i]);
                
                $('#' + arrdiv[i]).iphoneSwitch(state, { switch_on_container_path: '../images/iphone_switch_container_off.png' }, arrchk[i], btnID, arrtxt[i]);
            }
}              
    </script>
    <style type="text/css">
        body
        {
            font-family: Verdana, Geneva, sans-serif;
            font-size: 14px;
        }
        
        .left
        {
            float: none;
            width: 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%; padding-top: 5px;">
                <table>
                    <tr>
                        <td>
                            Store:
                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="product-type" Height="21px"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Product Page Configuration" alt="Product Page Configuration"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/page-configuration.png" />
                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server" Text="Product Page Configuration">
                                                        </asp:Label>
                                                    </h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr align="center" style="text-align: center;">
                                            <td align="center" style="padding-top: 20px">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 80%; padding: 0px">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>
                                                                                <h3>
                                                                                    <span style="font-size: 14px;" id="lblproconfig">PRODUCT DETAIL PAGE CONFIGURATION</span></h3>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                            <td align="right" style="width: 20%">
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="90%" cellspacing="0" cellpadding="0" border="0" style="border-color: #E7E7E7;
                                                    border-style: solid; border-width: 1px; width: 100%;">
                                                    <tbody>
                                                        <tr>
                                                            <th>
                                                                Status
                                                            </th>
                                                            <th align="left" style="padding-left: 10px;">
                                                                Name
                                                            </th>
                                                            <th>
                                                                Description
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divshippingtime" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkshippingtime" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                ShippingTime
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=ShippingTimeDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divreturnpolicy" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkreturnpolicy" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Return Policy
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=ReturnPolicyDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divrelatedPro" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkrelatedpro" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Related Products (You may also like)
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=RelatedDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divrecentlypro" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkrecentlypro" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Recently Viewed Products
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=RecentlyDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divsociallink" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chksociallink" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Social Media LInk
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=SocialDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divprintthispage" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkprintthispage" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Print This Page
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=PrintDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divtellafrnd" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chktellafrnd" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Tell A Friend
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=TellAFriendDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divviewmoreimages" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkviewmoreimages" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                View More Images
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=MoreImageDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none;">
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divestimateshipping" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkestimateshipping" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Estimate Shipping
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=EstimateShippingDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divpostreview" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkpostreview" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Post Review
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=PostReviewDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divaddtowishlist" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkaddtowishlist" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Add Wish List
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=WishlistDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divimagezoom" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkimagezoom" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Image Zoomer
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=ImageZoomDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divpricematch" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkpricematch" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Price Match
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=PriceMatchDesc%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10%" align="center">
                                                                <div class="left" id="divbookmarkpage" runat="server">
                                                                </div>
                                                                <div style="display: none">
                                                                    <asp:CheckBox ID="chkbookmarkpage" runat="server" />
                                                                </div>
                                                            </td>
                                                            <td width="17%" height="25px;" align="left" style="padding-left: 10px;">
                                                                Book Mark Page
                                                            </td>
                                                            <td width="73%" align="left" style="padding-left: 10px;">
                                                                <%=BookMarkPageDesc%>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ImageButton ID="imgbtnsave" runat="server" AlternateText="Save" ToolTip="Save"
                ImageUrl="~/App_Themes/Gray/images/save.gif" OnClick="imgbtnsave_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
