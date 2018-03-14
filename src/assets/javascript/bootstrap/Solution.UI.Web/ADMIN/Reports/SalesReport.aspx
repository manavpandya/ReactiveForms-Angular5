<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="SalesReport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.SalesReport" ValidateRequest="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">



        $(function () {

            $('#ContentPlaceHolder1_txtOrderFrom').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtOrderTo').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });

        function Datevisible() {

            if (document.getElementById('ContentPlaceHolder1_RadOrderByDays') != null) {

                if (document.getElementById('ContentPlaceHolder1_RadOrderByDays').selectedIndex == 3) {
                if (document.getElementById('ContentPlaceHolder1_datetd') != null) {
  document.getElementById('ContentPlaceHolder1_datetd').style.display = '';
     }
                }
                else {
                if (document.getElementById('ContentPlaceHolder1_datetd') != null) {
  document.getElementById('ContentPlaceHolder1_datetd').style.display = 'none';
   }
                }
//                var radio = document.getElementById('rdoList').getElementsByTagName('input');
//                
//                for (var j = 0; j < radio.length; j++) {

//                    if (radio[j].checked && j == 3) {
//                        if (document.getElementById('ContentPlaceHolder1_datetd') != null) {
//                            document.getElementById('ContentPlaceHolder1_datetd').style.display = '';
//                        }
//                    }
//                    else {
//                        if (document.getElementById('ContentPlaceHolder1_datetd') != null) {
//                            document.getElementById('ContentPlaceHolder1_datetd').style.display = 'none';
//                        }
//                    }
//                }
            }

        }

        function validation() {
            if (document.getElementById('ContentPlaceHolder1_RadOrderByDays') != null) {

//                var radio = document.getElementById('rdoList').getElementsByTagName('input');

//                for (var j = 0; j < radio.length; j++) {

                if (document.getElementById('ContentPlaceHolder1_RadOrderByDays').selectedIndex == 3) {
                        if (document.getElementById('ContentPlaceHolder1_txtOrderFrom').value == '') {
                            jAlert('Please Enter Start Date.','Required Information');
                            document.getElementById('ContentPlaceHolder1_txtOrderFrom').focus();
                            return false;
                        }
                        else if (document.getElementById('ContentPlaceHolder1_txtOrderTo').value == '') {
                            jAlert('Please Enter End Date.','Required Information');
                            document.getElementById('ContentPlaceHolder1_txtOrderTo').focus();
                            return false;
                        }

                        var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtOrderFrom').value);
                        var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtOrderTo').value);
                        if (startDate > endDate) {
                            jAlert('Please Select Valid Date.', 'Required Information');
                            return false;
                        }

                    }
                    
                //}
            }
            if (document.getElementById('prepage') != null) {
                document.getElementById('prepage').style.display = '';
            }
            return true;
        }


    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
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
                                                    <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/icon/report_icon.gif" />
                                                    <h2>
                                                        Sales Report</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td align="left">
                                                            Store :
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="175px" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;">
                                                Search By
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table border="0">
                                                    <tr>
                                                        <td align="left">
                                                        <div id="rdoList">
                                                            <asp:DropDownList ID="RadOrderByDays" runat="server" AutoPostBack="false" CssClass="order-list" Width="120px" 
                                                                onchange="Datevisible();">
                                                                <asp:ListItem Text=" Current Month" Value="Month" Selected="true"></asp:ListItem>
                                                                <asp:ListItem Text=" Current Week " Value="Week"></asp:ListItem>
                                                                <asp:ListItem Text=" Today" Value="Today"></asp:ListItem>
                                                                <asp:ListItem Text=" Customize" Value="Customize"></asp:ListItem>
                                                            </asp:DropDownList>
                                                             

                                                            </div>
                                                        </td>
                                                        <td align="left" valign="bottom" id="datetd" runat="server" style="display: none;">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        Start Date:
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:TextBox ID="txtOrderFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                    </td>
                                                                    <td valign="top">
                                                                        End Date:
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:TextBox ID="txtOrderTo" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="left">
                                                <asp:ImageButton ID="imgSearch" runat="server" AlternateText="Go" ToolTip="Go"
                                                    OnClick="imgSearch_Click" OnClientClick="return validation();" />
                                            </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="grvSalesReport" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                    CellPadding="2" CellSpacing="1" GridLines="None" Width="99%" PageSize="20" OnRowDataBound="grvSalesReport_RowDataBound"
                                                    OnRowCommand="grvSalesReport_RowCommand" BorderColor="#E7E7E7"  ShowFooter="true"
                                                    BorderWidth="1px" AllowPaging="true"
                                                    EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red" 
                                                    EmptyDataRowStyle-HorizontalAlign="Center"
                                                    PagerSettings-Mode="NumericFirstLast" 
                                                    onpageindexchanging="grvSalesReport_PageIndexChanging">
                                                    <%--<FooterStyle CssClass="product_table" />--%>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Store Name</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StoreID") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblStoreName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StoreName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            <b>Sub Total:</b>
                                                            </FooterTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Order Date</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                             
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Pending Orders</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpendingOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"panddingOrder") %>'></asp:Label>~
                                                                $<asp:Label ID="lblpendingOrderTot" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(Eval("panddingOrderTot")))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            <asp:Label ID="lblpanddingOrderTotf" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                <strong>Complete Orders Shipped Orders</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCompleteOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CompleteOrder") %>'></asp:Label>~
                                                                <asp:Label ID="lblCompleteOrderTot" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(Eval("CompleteOrderTot")))%>'></asp:Label>
                                                            </ItemTemplate>
                                                             <FooterTemplate>
                                                            <asp:Label ID="lblCompleteOrderTotf" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Shipped Orders Complete Orders</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShippedOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CompleteOrder") %>'></asp:Label>~
                                                                $<asp:Label ID="lblShippedOrderTot" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(Eval("CompleteOrderTot")))%>'></asp:Label>
                                                            </ItemTemplate>
                                                             <FooterTemplate>
                                                            <asp:Label ID="lblCompleteOrderTotship" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="right" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Declined Orders</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDeclinedOrders" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DeclinedOrders") %>'></asp:Label>~
                                                                $<asp:Label ID="lblDeclinedOrdersTot" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(Eval("DeclinedOrdersTot")))%>'></asp:Label>
                                                            </ItemTemplate>
                                                             <FooterTemplate>
                                                            <asp:Label ID="lblDeclinedOrdersTotf" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Cancelled Orders</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCancelledOrders" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CancelledOrders") %>'></asp:Label>~
                                                                $<asp:Label ID="lblCancelledOrdersTot" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(Eval("CancelledOrdersTot")))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            <asp:Label ID="lblCancelledOrdersTotf" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Sub Total</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AllDetails") %>'></asp:Label>~
                                                                $<asp:Label ID="lblSubTotalTot" runat="server" Text='<%#String.Format("{0:0.00}",Convert.ToDecimal(Eval("AllDetailsTot")))%>'></asp:Label>
                                                            </ItemTemplate>
                                                             <FooterTemplate>
                                                            <asp:Label ID="lblAllDetailsTotf" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                             <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                <strong>Order Slip</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnOrderSlip" runat="server" Text="Order Slip" CommandArgument=''
                                                                    ForeColor="black" CommandName="OrderSlip"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    <FooterStyle ForeColor="White" Font-Bold="true"  BackColor="#545454" />
                                                </asp:GridView>
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
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 200px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
