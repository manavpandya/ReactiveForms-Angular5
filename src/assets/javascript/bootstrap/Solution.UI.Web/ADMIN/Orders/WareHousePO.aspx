<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="WareHousePO.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.WareHousePO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }

        function openCenteredWindow(url) {
            var width = 750;
            var height = 420;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open(url, "SubWind", windowFeatures);
        }
        function pdffiledownloa(flname) {
            document.getElementById("ContentPlaceHolder1_hdnfilename").value = flname;
            document.getElementById("ContentPlaceHolder1_btnDownloadfile").click();
        }

        function checkondelete(id) {
            jConfirm('Are you sure want to delete this Record(s) ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById("ContentPlaceHolder1_hdnCustDelete").value = id;
                    document.getElementById("ContentPlaceHolder1_btnhdnDelete").click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }

        function checkCount() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                alert('Select at least one Record(s) !', 'Message');
                return false;
            }
            return true;
        }


        var t;
        function selectAll(on) {
            var allElts = document.forms['form1'].elements;
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 2px;" cellpadding="3" cellspacing="3">
                        <tr>
                            <td style="padding-left: 0px;" align="left">
                                Store : &nbsp;
                                <asp:DropDownList ID="ddlStore" runat="server" Width="175px" AutoPostBack="true"
                                    CssClass="order-list" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
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
                                                    <img class="img-left" title="Warehouse Purchase Order" alt="Warehouse Purchase Order"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/customer-list-icon.png" />
                                                    <h2>
                                                        Warehouse Purchase Order</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row" id="lblPoOrdrs" runat="server">
                                            <td style="padding-top: 10px;">
                                                &nbsp;<span style="font-weight: bold;"> Existing WareHouse Purchase Orders</span>
                                            </td>
                                        </tr>
                                        <tr class="odd-row" id="trOldPO" runat="server">
                                            <td>
                                                <asp:GridView ID="gvOldPOrder" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                                    CellPadding="2" CellSpacing="1" BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7"
                                                    OnRowCommand="gvOldPOrder_RowCommand" OnRowEditing="gvOldPOrder_RowEditing" OnPageIndexChanging="gvOldPOrder_PageIndexChanging"
                                                    PageSize="20" OnRowDataBound="gvOldPOrder_RowDataBound">
                                                    <EmptyDataTemplate>
                                                        <center>
                                                            No Record(s) Found!</center>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                PO Number
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="openCenteredWindow('OldWareHousePOOrderCart.aspx?back=1&Ono=<%=Request.Params["Ono"] %>&PONo=<%# DataBinder.Eval(Container.DataItem, "PONumber").ToString() %>');"
                                                                    style="text-decoration: underline; color: #212121;">PO-<asp:Label ID="lblPOName"
                                                                        runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PONumber") %>'></asp:Label>
                                                                </a>
                                                                <input type="hidden" id="hdnpo" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"PONumber") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Request No.
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRegDetailNo" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <HeaderStyle HorizontalAlign="center" Font-Bold="True" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Vendor Name
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                <asp:Label ID="lblACost" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"AdditionalCost") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblTax" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"Tax") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblShipping" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"Shipping") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblAdjust" runat="server" Text='<%# MakePositive(DataBinder.Eval(Container.DataItem,"Adjustments").ToString())   %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblPOAmt" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"PoAmount") %>'
                                                                    Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="left" Font-Bold="True" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                PO Date
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PODate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <HeaderStyle HorizontalAlign="center" Font-Bold="True" Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Change Status
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#  CheckPending(DataBinder.Eval(Container.DataItem, "PONumber").ToString())%><br />
                                                                <%#  CheckPendingNew(DataBinder.Eval(Container.DataItem, "PONumber").ToString())%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <HeaderStyle HorizontalAlign="center" Font-Bold="True" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Resend
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ForeColor="#212121" Font-Underline="true" ID="lnkResend" runat="server"
                                                                    Text="Resend" CommandName="Edit"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <HeaderStyle HorizontalAlign="center" Font-Bold="True" Width="8%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Download Files
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltrpdfdownload" runat="server"></asp:Literal>
                                                                <%-- <%#  CheckDownloadPdf(DataBinder.Eval(Container.DataItem, "PONumber").ToString())%><br />   --%>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <HeaderStyle HorizontalAlign="center" Font-Bold="True" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Delete
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('Are you sure to delete this PO ?'); "
                                                                    runat="server" CommandName="Del" Font-Underline="true" ForeColor="#212121" Text="Delete"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <HeaderStyle Font-Bold="True" HorizontalAlign="center" Width="5%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50">
                                                    </PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <br />
                                                <asp:Literal ID="ltClearPOAmt" runat="server" Visible="false"></asp:Literal>
                                                <asp:Literal ID="ltUnClearPOAmt" runat="server" Visible="false"></asp:Literal>
                                                <asp:Literal ID="ltTotalPOAmt" runat="server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                &nbsp;<span style="font-weight: bold;"> Create New WareHouse Purchase Order</span>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdCustomer" runat="server" AutoGenerateColumns="False" DataKeyNames="WareHouseId"
                                                    EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                                    CellPadding="2" CellSpacing="1" BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7"
                                                    OnRowDataBound="grdCustomer_RowDataBound" OnPageIndexChanging="grdCustomer_PageIndexChanging"
                                                    PageSize="20">
                                                    <EmptyDataTemplate>
                                                        <center>
                                                            No Record(s) Found!</center>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                <asp:HiddenField ID="hdnWareHouseId" Visible="false" runat="server" Value='<%#Eval("WareHouseId") %>' />
                                                                <asp:Label ID="lblProductID" runat="server" Visible="false" Text='<%#Eval("ProductId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" Width="8%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Product
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                SKU
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSku" runat="server" Text='<%# Eval("Sku") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                Inventory
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("Inventory")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                Generate Purchase Order
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);" onclick="window.location.href='WarehouseGeneratePO.aspx?PID=<%# Eval("ProductId")%>'">
                                                                    Click Here</a>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="delete"
                                                                    message='<%# Eval("WareHouseId") %>' CommandArgument='<%# Eval("WareHouseId") %>'
                                                                    OnClientClick='return checkondelete(this.getAttribute("message"));'></asp:ImageButton>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50">
                                                    </PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <asp:Button ID="btnhdnDelete" runat="server" Text="Button" OnClick="btnhdnDelete_Click"
                                                    Style="display: none;" />
                                                <asp:HiddenField ID="hdnCustDelete" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                        <tr id="trCheckClearAll" runat="server" visible="true">
                                            <td style="text-align: left; color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                font-size: 11px; padding-top: 10px;" colspan="2" id="cleartdid" runat="server">
                                                <div style="text-align: left; width: 250px; float: left;">
                                                    <a style="color: #696A6A; text-decoration: none;" id="lkbAllowAll" class="list_table_cell_link"
                                                        href="javascript:selectAll(true);">Check All</a>&nbsp; | <a id="lkbClearAll" style="color: #696A6A;
                                                            text-decoration: none;" class="list_table_cell_link" href="javascript:selectAll(false);">
                                                            Clear All</a>
                                                </div>
                                                <div style="text-align: right; float: right;">
                                                    <asp:ImageButton ID="btnGenerateMultiPlePO" runat="server" OnClientClick="return checkCount();"
                                                        AlternateText="Generate Purchase Order" OnClick="btnGenerateMultiPlePO_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="text-align: left; width: 250px; float: left;">
                                                    <a id="lnkAddNew" runat="server" name="aRelated" style="margin-right: 15px; font-weight: bold;
                                                        cursor: pointer;">+ Add New Item(s) for Warehouse P.O. </a>
                                                </div>
                                                <div style="text-align: right; float: right;">
                                                    <asp:ImageButton ID="btnRequestVendorQuote" runat="server" OnClientClick="return checkCount();"
                                                        AlternateText="Request New Quote" OnClick="btnRequestVendorQuote_Click" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="btnAddManualQuote" runat="server" OnClientClick="return checkCount();"
                                                        AlternateText="Add Manual Quote" OnClick="btnAddManualQuote_Click" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="btnRefreshQuote" runat="server" AlternateText="Refresh Quote"
                                                        OnClick="btnRefreshQuote_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="text-align: left; padding-top: 20px;">
                                                    &nbsp;
                                                </div>
                                                <div style="text-align: right;">
                                                    <asp:ImageButton ID="btnGenerateVendorQuotePOtop" runat="server" AlternateText="Generate Vendor Quote P.O."
                                                        OnClick="btnGenerateVendorQuotePO_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grvRequestedQuote" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                                    CellPadding="2" CellSpacing="1" BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7"
                                                    PageSize="20" OnRowDataBound="grvRequestedQuote_RowDataBound">
                                                    <EmptyDataTemplate>
                                                        <center>
                                                            No Vendor Quotes(s) Found !</center>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductID" runat="server" Visible="false" Text='<%#Eval("ProductId") %>'></asp:Label>
                                                                <%--<asp:Label ID="lblProductOption" runat="server" Text='<%# Eval("ProductOption") %>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Vendor Quote(s)
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                            <b>:
                                                                                <asp:Label ID="lblSku" runat="server" Text='<%# Eval("Sku") %>'></asp:Label></b>
                                                                            <br />
                                                                            <asp:Literal ID="ltrProOption" runat="server"></asp:Literal>
                                                                        </td>
                                                                        <td align="right">
                                                                            <b>Inventory :
                                                                                <asp:Label ID="lblInventory" ForeColor="#FF0000" runat="server" Text='<%# Eval("Inventory")%>'></asp:Label></b>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:GridView ID="grvQuoteReplyDetails" runat="server" AutoGenerateColumns="False"
                                                                                EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                ViewStateMode="Enabled" Width="100%" class="table-noneforOrder" GridLines="None"
                                                                                AllowPaging="True" PagerSettings-Mode="NumericFirstLast" CellPadding="2" CellSpacing="1"
                                                                                BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7" Font-Size="12px" PageSize="20"
                                                                                OnRowCommand="grvQuoteReplyDetails_RowCommand" OnRowDataBound="grvQuoteReplyDetails_RowDataBound">
                                                                                <EmptyDataTemplate>
                                                                                    <center>
                                                                                        <span style="color: Red;">No Record(s) Found !</span></center>
                                                                                </EmptyDataTemplate>
                                                                                <Columns>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblProductID" runat="server" Visible="false" Text='<%#Eval("ProductId") %>'></asp:Label>
                                                                                            <asp:Label ID="lblVendorQuoteReplyID" runat="server" Visible="false" Text='<%#Eval("VendorQuoteReplyID") %>'></asp:Label>
                                                                                            <asp:Label ID="lblVendorQuoteID" runat="server" Text='<%#Eval("VendorQuoteID") %>'></asp:Label>
                                                                                            <asp:Label ID="lblVendorQuoteReqDetailsID" runat="server" Text='<%#Eval("VendorQuoteReqDetailsID") %>'></asp:Label>
                                                                                            <asp:Label ID="lblVendorID" runat="server" Text='<%#Eval("VendorID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <HeaderTemplate>
                                                                                            Request No.
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal ID="ltrRequestNo" runat="server"></asp:Literal>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <HeaderTemplate>
                                                                                            Vendor Name
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("VendorName") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <HeaderTemplate>
                                                                                            Location
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("Location") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Available
                                                                                            <br />
                                                                                            [in Days]
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAvailableDays" runat="server" Text='<%#Eval("AvailableDays") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Requested<br />
                                                                                            Quantity
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblRequestedQuantity" runat="server" Text='<%#Eval("RequestedQuantity") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Quoted<br />
                                                                                            Quantity
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblQuoteQuantity" runat="server" Text='<%#Eval("QuoteQuantity") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            PO
                                                                                            <br />
                                                                                            Quantity
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPOQty" Visible="false" runat="server"></asp:Label>
                                                                                            <asp:TextBox ID="txtPOQty" runat="server" Style="width: 60px; text-align: center;"
                                                                                                Text="0" onkeypress="return keyRestrict(event,'0123456789');" MaxLength="6" class="order-textfield"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Quoted<br />
                                                                                            Price($)
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblQuotedPrice" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")),2) %>'></asp:Label>
                                                                                            <asp:TextBox ID="txtQuotedPrice" MaxLength="10" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")),2) %>'
                                                                                                Style="width: 80px; text-align: center;" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                class="order-textfield"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Requested On
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblRequestedon" runat="server" Text='<%#Eval("Requestedon") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Submitted On
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblQuoteGivenOn" runat="server" Text='<%#Eval("QuoteGivenOn") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Notes
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblNotes" runat="server" Text='<%#Eval("Notes") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Resend
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblMailLogid" Visible="false" runat="server" Text='<%#Eval("MailLogid") %>'></asp:Label>
                                                                                            <asp:Literal ID="ltrResendMail" runat="server"></asp:Literal>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <HeaderTemplate>
                                                                                            Delete
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="btndel" runat="server" ImageUrl="~/Admin/images/btndel.gif"
                                                                                                ToolTip="Delete" OnClientClick="javascript:if(confirm('Are you sure,you want to delete this Quote ?')){ }else{return false;}"
                                                                                                CommandArgument='<%# Container.DataItemIndex %>' CommandName="delMe" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50">
                                                                                </PagerSettings>
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                <AlternatingRowStyle CssClass="altrow" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50">
                                                    </PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="text-align: left;">
                                                    &nbsp;
                                                </div>
                                                <div style="text-align: right;">
                                                    <asp:ImageButton ID="btnGenerateVendorQuotePO" runat="server" AlternateText="Generate Vendor Quote P.O."
                                                        OnClick="btnGenerateVendorQuotePO_Click" />
                                                </div>
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
    <div style="display: none;">
        <asp:Button ID="btnDownloadfile" runat="server" OnClick="btnDownloadfile_Click" />
        <input type="hidden" id="hdnfilename" runat="server" value="" />
    </div>
</asp:Content>
