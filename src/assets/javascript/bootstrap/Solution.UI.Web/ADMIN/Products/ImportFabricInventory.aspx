<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ImportFabricInventory.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ImportFabricInventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <asp:Literal ID="ltrCalenScript" runat="server"></asp:Literal>
    <script type="text/javascript" src="/js/popup.js"></script>
    <style type="text/css">
        .divfloatingcss {
            bottom: 0;
            right: 0;
            position: fixed;
            width: 10%;
            margin-right: 38%;
            background-image: url("/Admin/images/title-bg-trans.png");
            -webkit-border-top-left-radius: 15px;
            -webkit-border-top-right-radius: 15px;
            -moz-border-radius-topleft: 15px;
            -moz-border-radius-topright: 15px;
            border-top-left-radius: 15px;
            border-top-right-radius: 15px; /*-webkit-border-radius: 20px;
-moz-border-radius: 20px;
border-radius: 20px;*/
        }
    </style>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        var $j = jQuery.noConflict();
        $j(document).ready(function () {
            $j('#divfloating').attr("class", "divfloatingcss");
            $j(window).scroll(function () {
                if ($j(window).scrollTop() == $j(document).height() - $j(window).height()) {
                    $j('#divfloating').attr("class", "");
                }
                else {
                    $j('#divfloating').attr("class", "divfloatingcss");
                }
            });
        });
    </script>
    <script type="text/javascript">
        function ShowModelCredit(id) {
            document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '500px';
            document.getElementById('frmdisplay1').width = '780px';
            document.getElementById('frmdisplay1').scrolling = 'no';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:780px;height:500px;");
            document.getElementById('popupContact1').style.width = '780px';
            document.getElementById('popupContact1').style.height = '500px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }

        //function ClacuOnHandQty(x) {

        //    var QtyOnHand = 0;
        //    var BookedQty = document.getElementById('ContentPlaceHolder1_grdVendorPortal_txtBookedQty_' + x).value;
        //    var AvailQty = document.getElementById('ContentPlaceHolder1_grdVendorPortal_txtAvailQty_' + x).value;

        //    if (document.getElementById('ContentPlaceHolder1_grdVendorPortal_txtAvailQty_' + x).value == '') {
        //        AvailQty = 0;
        //        document.getElementById('ContentPlaceHolder1_grdVendorPortal_txtAvailQty_' + x).value = 0;
        //    }
        //    if (document.getElementById('ContentPlaceHolder1_grdVendorPortal_txtBookedQty_' + x).value == '') {
        //        BookedQty = 0;
        //        document.getElementById('ContentPlaceHolder1_grdVendorPortal_txtBookedQty_' + x).value = 0;
        //    }

        //    if (isNaN(QtyOnHand)) {
        //        QtyOnHand = 0;
        //    }
        //    QtyOnHand = parseInt(AvailQty) - parseInt(BookedQty);
        //    document.getElementById('ContentPlaceHolder1_grdVendorPortal_txtQtyOnHand_' + x).value = QtyOnHand;
        //}

        function ClacuOnHandQty(OrderQtyId, BookId, BookUploadId, BalQtyId, poqty) {
            var QtyOnHand = 0;

            if (document.getElementById(BookId) != null && document.getElementById(OrderQtyId) != null && document.getElementById(BookUploadId) != null) {
                document.getElementById(BookUploadId).value = 0;
                document.getElementById(BookId).value = 0;
                var ids = OrderQtyId.replace('txtqtyonhand', 'hdnvendorqty');
                document.getElementById(ids).value = '1';
                var BookedQty = document.getElementById(BookId).value;
                var OrderQty = document.getElementById(OrderQtyId).value;
                var BookUpload = document.getElementById(BookUploadId).value;

                if (OrderQty == '') {
                    OrderQty = 0;
                    document.getElementById(OrderQtyId).value = 0;
                }
                if (BookedQty == '') {
                    BookedQty = 0;
                    document.getElementById(BookId).value = 0;
                }
                if (BookUpload == '') {
                    BookBookUploadedQty = 0;
                    document.getElementById(BookUploadId).value = 0;
                }


                QtyOnHand = parseInt(OrderQty) - parseInt(BookedQty) - parseInt(BookUpload);
                document.getElementById(BalQtyId).value = parseInt(QtyOnHand);
                document.getElementById(poqty).value = parseInt(OrderQty);
            }
        }
        function ClacuOnHandQty1(OrderQtyId, BookId, BookUploadId, BalQtyId) {
            var QtyOnHand = 0;
            if (document.getElementById(BookId) != null && document.getElementById(OrderQtyId) != null && document.getElementById(BookUploadId) != null) {
                var BookedQty = document.getElementById(BookId).value;
                var OrderQty = document.getElementById(OrderQtyId).value;
                var BookUpload = document.getElementById(BookUploadId).value;

                if (OrderQty == '') {
                    OrderQty = 0;
                    document.getElementById(OrderQtyId).value = 0;
                }
                if (BookedQty == '') {
                    BookedQty = 0;
                    document.getElementById(BookId).value = 0;
                }
                if (BookUpload == '') {
                    BookBookUploadedQty = 0;
                    document.getElementById(BookUploadId).value = 0;
                }


                QtyOnHand = parseInt(OrderQty) - parseInt(BookedQty) - parseInt(BookUpload);
                document.getElementById(BalQtyId).value = QtyOnHand;
            }
        }
        function keyRestrictforIntOnly(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
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

        function call()
        {

         
            document.getElementById('ContentPlaceHolder1_btnexporta').click();
           
        }
    </script>
     <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
               <span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    id="ahAddProduct" href="~/ADMIN/Products/FabricVendorPortal.aspx" runat="server">
                    <img alt="Back" title="Back" src="/App_Themes/<%=Page.Theme %>/images/back.png" /></a></span>
            </div>
        </div>

    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="10" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr id="trvendortitle" runat="server">
                                        <td  style="display:none;">
                                            <asp:Literal ID="ltvendordetail" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr id="trvendordetail" runat="server">
                                    </tr>
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Import Fabric Inventory" alt="Import Fabric Inventory" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                <h2>Import Fabric Inventory</h2>
                                            </div>
                                            <div class="main-title-right" style="display: none;">
                                                <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                    <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/close.png" /></a>
                                            </div>
                                            <div class="main-title-right">
                                                <a title="Back" id="BackLink" runat="server" visible="false">
                                                    <img title="Back" alt="Back" src="/App_Themes/<%=Page.Theme %>/button/back.png" /></a>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td id="tdMainDiv">
                                            <div id="divMain" class="slidingDivMainDiv">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                    <tr class="altrow">
                                                        <td align="center">
                                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row">
                                                        <td align="left">
                                                            <fieldset>
                                                               
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr class="oddrow">
                                                                        <td align="left" style="display:none;">
                                                                            <span class="star">*</span>Fabric Category Name :
                                                                        </td>
                                                                        <td align="left"  style="display:none;">
                                                                            <asp:DropDownList ID="ddlFabricType" runat="server" class="product-type" OnSelectedIndexChanged="ddlFabricType_SelectedIndexChanged"
                                                                                AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td align="left"  style="display:none;">Seach By Code :</td>
                                                                        <td align="left"  style="display:none;">
                                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox></td>
                                                                        <td align="left"  style="display:none;">
                                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" /></td>
                                                                        <td align="left"  style="display:none;">
                                                                            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" /></td>
                                                                        
                                                                        <td>
                                                                            <asp:Label ID="lblImportCSv" runat="server" Text="Import CSV :"></asp:Label>
                                                                            <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" /></td>
                                                                        <td style="margin-left:950px;float:right;">
                                                                            <a href="~/ADMIN/Files/FabricVendorProducts.csv" style="display:none;" id="btnexporta" onclick="return true;" runat="server">
                                                                                </a>
                                                                             <asp:Button ID="btnexport1" OnClientClick="call();return false;" runat="server"     />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" id="trFabricDetails"  style="display:none;" runat="server" visible="false">
                                                        <td align="center">
                                                            <div id="div4" class="slidingDivDesc">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td valign="top" align="left" colspan="2">
                                                                            <asp:GridView ID="grdVendorPortal" runat="server" AutoGenerateColumns="False" BorderColor="#e7e7e7"
                                                                                BorderStyle="Solid" BorderWidth="1px" DataKeyNames="FabricCodeId" EmptyDataText="No Record(s) Found."
                                                                                AllowSorting="false" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                ViewStateMode="Enabled" Width="100%" GridLines="None" OnRowDataBound="grdVendorPortal_RowDataBound"
                                                                                AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="grdVendorPortal_PageIndexChanging" OnRowCommand="grdVendorPortal_RowCommand">
                                                                                <Columns>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Active
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblFabricVendorPortId" runat="server" Visible="true" Text='<%# Bind("FabricVendorPortId") %>'></asp:Label>
                                                                                            &nbsp;<asp:Label ID="lblFabricCodeId" runat="server" Visible="true" Text='<%# Bind("FabricCodeId") %>'></asp:Label>
                                                                                            <asp:Label ID="lblFabricTypeID" runat="server" Visible="false" Text='<%# Bind("FabricTypeID") %>'></asp:Label>

                                                                                            &nbsp;<asp:CheckBox ID="chkActive" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Fabric SKU
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemTemplate>
                                                                                            &nbsp;<asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Name
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemTemplate>
                                                                                            &nbsp;<asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Min Alert Qty
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemTemplate>
                                                                                            &nbsp;
                                                                                            <asp:TextBox ID="txtminqty1" Text='<%# Bind("MinQty") %>' runat="server" Width="60px" CssClass="order-textfield"></asp:TextBox>

                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Delivery Days
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemTemplate>
                                                                                            &nbsp;
                                                                                            <asp:TextBox ID="lbldays" Text="" runat="server" Width="60px" CssClass="order-textfield"></asp:TextBox>



                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Per Yard Cost
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemTemplate>
                                                                                            &nbsp;
                                                                                           
                                                                                             <asp:TextBox ID="lblyardcost" Text=' <%#String.Format("{0:0.00}",Convert.ToDecimal(Eval("yardprice")))%>' Width="60px" CssClass="order-textfield" runat="server"></asp:TextBox>

                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                                        <ItemTemplate>

                                                                                            <asp:Label ID="lblFabricOrderId" runat="server" Visible="false" Text='<%# Bind("FabricOrderId") %>'></asp:Label>
                                                                                            <asp:Label ID="lblsku" runat="server" Text='<%#Bind("code1") %>'></asp:Label>

                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Qty On Hand
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtqtyonhand" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"QtyOnHand"))  %>'></asp:TextBox>
                                                                                            <input type="hidden" id="hdnvendorqty" runat="server" value="0" />



                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Qty Booked
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtbookedqty" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;" Text='<%#DataBinder.Eval(Container.DataItem,"BookedQty")%>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Book & Upload
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtbookupload" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;" Text='<%#DataBinder.Eval(Container.DataItem,"QtyBoookedNAV")%>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Balance Qty
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtavailqty" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                Text=""></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;PO Order Qty
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtpoorderqty" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"QtyOnHand"))  %>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Prod. Date
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtProductionDate" runat="server" Width="70px" CssClass="from-textfield"
                                                                                                Style="text-align: left; margin-right: 3px;" Text='<%#  DataBinder.Eval(Container.DataItem,"ProductionDate")  %>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>



                                                                                    <asp:TemplateField Visible="False">
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Min. Inventory
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtMinQty" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"MinQty"))  %>'
                                                                                                MaxLength="6" Visible="false"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="False">
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Max. Order Inventory
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtMaxOrderQty" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"MinOrderQty"))  %>'
                                                                                                MaxLength="6" Visible="false"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="False">
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Quantity On Hand
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtQtyOnHand1" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"QtyOnHand"))  %>'
                                                                                                MaxLength="6" Visible="false"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Booked Quantity
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtBookedQty1" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"BookedQty"))  %>'
                                                                                                MaxLength="6" Visible="false"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="False">
                                                                                        <HeaderTemplate>
                                                                                            &nbsp;Available From Stock
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtAvailQty1" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"AvailQty"))  %>'
                                                                                                MaxLength="6" Visible="false"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="View" Visible="false">
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <a href="javascript:void(0);" id="atagView" runat="server" visible="true" title="View">
                                                                                                <img src="/App_Themes/Gray/images/view-details.png" />
                                                                                            </a>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Operations" Visible="false">
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="_deleteLinkButton"
                                                                                                CommandArgument='<%# Eval("FabricCodeId") %>' message='<%# Eval("FabricCodeId") %>'
                                                                                                OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){return true;}else{return false;}"
                                                                                                ImageUrl="~/App_Themes/Gray/images/delete-icon.png"></asp:ImageButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                <AlternatingRowStyle CssClass="altrow" />
                                                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="altrow">
                                                                        <td width="13%"></td>
                                                                        <td align="center">
                                                                            <div id="divfloating" class="divfloatingcss" style="width: 300px;">
                                                                                <div style="margin-bottom: 1px; margin-top: 3px;">
                                                                                    <asp:ImageButton ID="btnSave" runat="server"
                                                                                        OnClick="btnSave_Click" />
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <input type="button" id="btnhelpdescri" />
        <asp:ImageButton ID="popupContactClose" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div id="popupContact1" style="z-index: 1000001; width: 790px; height: 510px;">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay1" frameborder="0" height="510" width="790" scrolling="auto"></iframe>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
