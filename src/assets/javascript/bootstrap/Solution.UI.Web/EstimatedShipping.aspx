<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EstimatedShipping.aspx.cs" Inherits="Solution.UI.Web.EstimatedShipping" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    
    <div id="content-right">
    <script language="javascript" type="text/javascript">

        var montharray = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        function addDate(ds, v) {
            var n;
            var d = ds;
            var n = d.getDate();
            var day = d.getDay();
            if (day == 6) { rem = 1 } else { rem = ((v + day) - ((v + day) % 7)) * 2 / 7; };
            //creating new date and return it 
            d1 = new Date();
            d1.setYear(d.getYear());
            d1.setMonth(d.getMonth());
            d1.setDate(n + v + rem);
            var tempd = d1.getDay();
            var tempv = 0;
            if (tempd == 0) tempv = 6;
            if (tempd == 6) tempv = 5;
            if (tempv >= 5) {
                d1.setDate(d1.getDate() + (7 - tempv));
            }
            return (d1);
        };
        function showDate(num) {

            var d = addDate(new Date(), num);
            document.write(montharray[d.getMonth()] + " " + d.getDate());
        };

    </script>
 <style type="text/css">
    .table1 {
    border-collapse: collapse;
    font-family: "Times New Roman",Times,serif;
    font-size: 12px;
    width: 100%;
}
.table1 th {
    background: none repeat scroll 0 0 #3D7453;
    border: 1px solid #62AC80;
    color: #FFFFFF;
    font-weight: bold;
    padding: 5px 8px;
    text-align: center;
}
.table1 td {
    border: 1px solid #62AC80;
    padding: 5px 3px;
    text-align: center;
}
</style>
        <div class="breadcrmbs">
            <a href="/index.aspx" title="Home">Home </a>> <span>
                <asp:Literal ID="litBreadTitle" Text="Estimated Delivery Date" runat="server"></asp:Literal></span></div>
        <div class="static-display">
            <h1 class="static-title">
                Estimated Delivery Date</h1>
            <p>
                Estimated Delivery Timeframe For Items
            </p>
            <table width="450" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td>
                            <table width="480" cellspacing="0" cellpadding="5" border="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <p align="left">
                                                <!--TEXT HERE-->
                                                We specialize in Last Minute Groomsmen Gifts. Choose express shipping and expedite
                                                engraving option and get your engraved gifts within 2-3 business days.</p>
                                            <p align="left">
                                                <!--TEXT HERE-->
                                                We offer expedite engraving service for extra $4 per item.</p>
                                            <table width="510" cellspacing="0" cellpadding="5" border="1" class="table1">
                                                <tbody>
                                                    <tr>
                                                        <th width="100" align="LEFT">
                                                            Item Options
                                                        </th>
                                                        <th width="100" align="LEFT">
                                                            Shipping Method
                                                        </th>
                                                        <th width="100" align="LEFT">
                                                            Approximate Processing Time
                                                        </th>
                                                        <th width="100" align="LEFT">
                                                            Approximate Shipping Time
                                                        </th>
                                                        <th width="100" align="CENTER">
                                                            Estimated Delivery Date
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td align="LEFT">
                                                            No Engraving
                                                        </td>
                                                        <td align="LEFT">
                                                            Priority USPS
                                                        </td>
                                                        <td align="LEFT">
                                                            1-2 Business Days
                                                        </td>
                                                        <td align="LEFT">
                                                            3-4 business Days
                                                        </td>
                                                        <td align="CENTER">
                                                            <script>                                                                showDate(4); </script>
                                                            -
                                                            <script>                                                                showDate(6); </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="LEFT">
                                                            With Engraving
                                                        </td>
                                                        <td align="LEFT">
                                                            Priority USPS
                                                        </td>
                                                        <td align="LEFT">
                                                            3-4 Business Days
                                                        </td>
                                                        <td align="LEFT">
                                                            3-4 business Days
                                                        </td>
                                                        <td align="CENTER">
                                                            <script>                                                                showDate(6); </script>
                                                            -
                                                            <script>                                                                showDate(8); </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="LEFT">
                                                            With Expedited Engraving Option
                                                        </td>
                                                        <td align="LEFT">
                                                            Priority USPS
                                                        </td>
                                                        <td align="LEFT">
                                                            1 Business Day
                                                        </td>
                                                        <td align="LEFT">
                                                            3-4 business Days
                                                        </td>
                                                        <td align="CENTER">
                                                            <script>                                                                showDate(4); </script>
                                                            -
                                                            <script>                                                                showDate(5); </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="LEFT">
                                                            No Engraving
                                                        </td>
                                                        <td align="LEFT">
                                                            ExpressMail
                                                        </td>
                                                        <td align="LEFT">
                                                            1-2 Business Days
                                                        </td>
                                                        <td align="LEFT">
                                                            1-2 Business Days
                                                        </td>
                                                        <td align="CENTER">
                                                            <script>                                                                showDate(2); </script>
                                                            -
                                                            <script>                                                                showDate(4); </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="LEFT">
                                                            With Engraving
                                                        </td>
                                                        <td align="LEFT">
                                                            ExpressMail
                                                        </td>
                                                        <td align="LEFT">
                                                            3-4 Business Days
                                                        </td>
                                                        <td align="LEFT">
                                                            1-2 Business Days
                                                        </td>
                                                        <td align="CENTER">
                                                            <script>                                                                showDate(4); </script>
                                                            -
                                                            <script>                                                                showDate(6); </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="LEFT">
                                                            Expedite Engraving Option
                                                        </td>
                                                        <td align="LEFT">
                                                            ExpressMail
                                                        </td>
                                                        <td align="LEFT">
                                                            1 Business Day
                                                        </td>
                                                        <td align="LEFT">
                                                            1-2 Business Days
                                                        </td>
                                                        <td align="CENTER">
                                                            <script>                                                                showDate(2); </script>
                                                            -
                                                            <script>                                                                showDate(4); </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <!--TEXT HERE-->
                                                            <p>
                                                                Engraving orders take 2-3 business days to process. If ordered today, your order
                                                                will ship by
                                                                <script>                                                                    showDate(3); </script>
                                                                Lighters are shipped without butane in them. Butane refils take up to 10
                                                                days to recieve and cannot be shipped by air. (<script>                                                                                                                   showDate(12); </script>)</p>
                                                            <p>
                                                                Note: This is an <b>Estimated Timeframe. It is not a guaranteed delivery timeframe.</b></p>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <p>
            </p>
        </div>
        <div class="right-sub-section">
            <h1>
                Best Sellers</h1>
            <div class="best-sellers-box">
                <asp:Repeater ID="rptBestSeller" runat="server" OnItemDataBound="rptBestSeller_ItemDataBound">
                    <ItemTemplate>
                        <div id="ProboxBestSeller" runat="server" class="best-pro-box">
                            <div id="proDisplay" runat="server">
                                <div class="img-center">
                                    <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx">
                                        <img src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' width="127"
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
                                <%-- <p>
                                        <span><a href="javascript:void(0);" id="abestLink" runat="server" title="Add to Cart">
                                            <img src="/images/add_to_cart.jpg" alt="Add to Cart" height="25" width="107" /></a></span></p>--%>
                                <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx">
                                    <img src="/images/view_more.jpg" alt="Add to Cart" height="18" width="80" style="margin-bottom: 10px" /></a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <uc1:LeftMenu ID="leftmenu" runat="server" />
</asp:Content>
