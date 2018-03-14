<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="WishList.aspx.cs" Inherits="Solution.UI.Web.WishList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <script type="text/javascript">
        function qtywish(id) {

            id = id.toString().replace('_btnAddtoCart_', '_txtQty_');
            if (document.getElementById(id) != null && (document.getElementById(id).value == '' || document.getElementById(id).value == '0' || document.getElementById(id).value == '00' || document.getElementById(id).value == '000')) {
                alert('Please enter valid qty.');
                document.getElementById(id).focus();
                return false;
            }
            else {
                chkHeight();
            }
            return true;
        }

        function SetDefault(id) {
            if (document.getElementById(id).value == '') {
                document.getElementById(id).value = 1;
            }

        }
        function chkHeight() {

            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
    </script>
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <a href="/myaccount.aspx" title="My Account">
            My Account</a> ><span> Wish List</span></div>
    <div class="content-main">
        <div class="static-title">
            <table style="width: 100%;">
                <tr>
                    <td style="float: left; width: 94%;">
                        <span>Wish List </span>
                    </td>
                    <td style="float: right; width: 5%;">
                        <span><a href="MyAccount.aspx" style="color: #B92127; text-decoration: underline;"
                            title="Back" id="abacklink" runat="server">
                            <img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a></span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td align="center" valign="middle" colspan="2" style="line-height: 25px; vertical-align: middle;">
                            <asp:Label ID="lblError" runat="server" Style="color: Red;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%--    <asp:Literal ID="ltCart" runat="server"></asp:Literal>--%>
                            <asp:Repeater ID="rptWishList" runat="server" OnItemCommand="rptWishList_ItemCommand"
                                OnItemDataBound="rptWishList_ItemDataBound">
                                <HeaderTemplate>
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table">
                                        <tbody>
                                            <tr>
                                                <th width="24%">
                                                    Image
                                                </th>
                                                <th width="30%" align="left">
                                                    Product
                                                </th>
                                                <th width="10%">
                                                    Price
                                                </th>
                                                <th width="9%">
                                                    Quantity
                                                </th>
                                                <th width="12%">
                                                    Sub Total
                                                </th>
                                                <th>
                                                    Add to Cart
                                                </th>
                                            </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" valign="top">
                                            <%-- <a id="lnkProductPage" runat="server">--%>
                                            <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>"  title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                <asp:Image ID="imgProduct" runat="server" AlternateText=""/></a>
                                            <input type="hidden" id="hdnWishListID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"WishListID") %>' />
                                            <input type="hidden" id="hdnProductID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                            <input type="hidden" id="hdnrealtedProductID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"RelatedproductID") %>' />
                                            <input type="hidden" id="hdnproductTypeID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ProductTypeID") %>' />
                                            <input type="hidden" id="hdnVariantNames" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantNames") %>' />
                                            <input type="hidden" id="hdnVariantValues" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantValues") %>' />
                                        </td>
                                        <td valign="top">
                                            <%-- <a href="#" id="lnkProductName" runat="server" title="">--%>
                                            <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>"  title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>" >
                                                <%# DataBinder.Eval(Container.DataItem,"ProductName") %></a>
                                            <asp:Literal ID="ltrVariant" runat="server"></asp:Literal>
                                        </td>
                                        <td align="right" valign="top">
                                            <asp:Label ID="lblPrice" runat="server" Text='<%# String.Format("{0:C}",DataBinder.Eval(Container.DataItem, "Price"))  %>'></asp:Label>
                                        </td>
                                        <td align="center" valign="top">
                                            <asp:TextBox ID="txtQty" MaxLength="3" onkeypress="return isNumberKey(event);" runat="server"
                                                onblur="SetDefault(this.id);" CssClass="wish-list-quantity" Text=' <%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:TextBox>
                                            <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"WishListID") %>'
                                                CommandName="Remove" ToolTip="Remove" OnClientClick="javascript:if(confirm('Are you sure want to delete this item?')){document.getElementById('prepage').style.display = '';return true;}else{return false;};">Remove</asp:LinkButton>
                                        </td>
                                        <td align="right" valign="top">
                                            <asp:Label ID="lblSubTotal" runat="server" Text='<%#  String.Format("{0:C}",DataBinder.Eval(Container.DataItem, "Subtotal")) %>'></asp:Label>
                                            <asp:ImageButton ID="btnAddtoCart" runat="server" ImageUrl="/images/add-to-cart.jpg"
                                                    OnClientClick="return qtywish(this.id);"
                                                    AlternateText="Add To Cart" CommandArgument='0' style="display:none;"
                                                    CommandName="AddCart" Visible="false" />
                                        </td>
                                        <%--<td valign="top" align="center">--%>
                                            
                                                
                                        <%--</td>--%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody> </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <span style="margin-left: 45%;">
                                <asp:Label ID="lblEmptyWishList" runat="server" Text="Wish List is Empty" Font-Bold="true"
                                    ForeColor="Red"></asp:Label></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-bottom: 10px;">
                            <asp:ImageButton ID="btnAnotherItem" runat="server" alt="ADD ANOTHER ITEM" title="ADD ANOTHER ITEM"
                                ImageUrl="/images/add-another-item-small.png" OnClick="btnAnotherItem_Click" />
                        </td>
                        <td align="right" style="padding-bottom: 10px;">
                            <asp:ImageButton ID="btnUpdateWishlist" runat="server" alt="UPDATE WISHLIST" title="UPDATE WISHLIST"
                                ImageUrl="/images/update-wishlist.png" OnClick="btnUpdateWishlist_Click" OnClientClick="javascript:document.getElementById('prepage').style.display = '';chkHeight();" />
                        </td>
                    </tr>
                </table>
            </div>
            <%--    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
                top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
                height: 100%; width: 100%; z-index: 1000; display: none;">
                <table width="100%" style="padding-top: 100px;" align="center">
                    <tr>
                        <td align="center" style="color: #fff;" valign="middle">
                            <img alt="" src="/images/loding.png" /><br />
                            <b>Loading ... ... Please wait!</b>
                        </td>
                    </tr>
                </table>
            </div>--%>
            <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
                top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
                height: 100%; width: 100%; z-index: 1000; display: none;">
                <div style="border: 1px solid #ccc;">
                    <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                        <tr>
                            <td>
                                <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc;
                                    width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px; -moz-border-radius: 10px;
                                    border-radius: 10px;">
                                    <center>
                                        <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                        <b style="color: #fff;">Loading ... ... Please wait!</b></center>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
