<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderTaxReport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.OrderTaxReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">

        $(function () {

            $('#ContentPlaceHolder1_txtMailFrom').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtMailTo').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });


        function SearchValidation(id) {

            if (id == 0) {
                //                if (document.getElementById('ContentPlaceHolder1_ddlSearch').selectedIndex == 0) {
                //                    jAlert('Please Select Search By.', 'Required Information', 'ContentPlaceHolder1_ddlSearch');
                //                    //document.getElementById('ContentPlaceHolder1_ddlSearch').focus();
                //                    return false;
                //                }
                //                if (document.getElementById('ContentPlaceHolder1_txtSearch').value == '') {
                //                    jAlert('Please Enter Search Keyword.', 'Required Information', 'ContentPlaceHolder1_txtSearch');
                //                    //document.getElementById('ContentPlaceHolder1_txtSearch').focus();
                //                    return false;
                //                }
                //                else if (document.getElementById('ContentPlaceHolder1_txtSearch').value != '' && (document.getElementById('ContentPlaceHolder1_txtSearch').value.toString().toLowerCase() == 'search keyword' || document.getElementById('ContentPlaceHolder1_txtSearch').value.toString().toLowerCase() == 'searchkeyword')) {
                //                    jAlert('Please Enter Search Keyword.', 'Required Information', 'ContentPlaceHolder1_txtSearch');
                //                    //document.getElementById('ContentPlaceHolder1_txtSearch').focus();
                //                    return false;
                //                }

            }
            if (document.getElementById('ContentPlaceHolder1_txtMailFrom').value == '') {
                jAlert('Please Enter From Date.', 'Required Information', 'ContentPlaceHolder1_txtMailFrom');
                //document.getElementById('ContentPlaceHolder1_txtMailFrom').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtMailTo').value == '') {
                jAlert('Please Enter To Date.', 'Required Information', 'ContentPlaceHolder1_txtMailTo');
                // document.getElementById('ContentPlaceHolder1_txtMailTo').focus();
                return false;
            }

            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtMailFrom').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtMailTo').value);
            if (startDate > endDate) {
                jAlert('Please Select Valid Date.', 'Required Information', 'ContentPlaceHolder1_txtMailTo');
                return false;
            }
            return true;
        }

        function SelectAll(on) {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }

        function chkSelect() {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {
                jAlert("Please select at least one Mail.", "Message");
                return false;

            }
            else {
                jConfirm('Are you sure want to delete all selected Mail ?', 'Confirmation', function (r) {
                    if (r == true) {

                        document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
                        return true;
                    }
                    else {

                        return false;
                    }
                });
            }
            return false;

        }
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }

                
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <table>
                    <tr>
                        <td align="left">
                            Store :&nbsp;<asp:DropDownList ID="ddlStore" 
                                runat="server" CssClass="order-list"
                                Width="175px" AutoPostBack="true" Style="margin-top: 5px;" 
                                onselectedindexchanged="ddlStore_SelectedIndexChanged1">
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
                                            <th colspan="3">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Order Tax Report" alt="Order Tax Report" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                    <h2>
                                                        Order Tax Report</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left">
                                            </td>
                                            <td align="right" colspan="2">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td valign="middle" align="right">
                                                            From Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtMailFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            To Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtMailTo" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="left" style="width: 50px;">
                                                            State :
                                                        </td>
                                                        <td align="left" style="width: 180px;">
                                                            <asp:DropDownList ID="ddlState" runat="server" CssClass="order-list" Width="175px"
                                                                AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation(0);" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="3">
                                                <table border="0">
                                                    <tr>
                                                        <td align="left" valign="bottom" id="datetd" runat="server" style="display: none;">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div id="divGrid">
                                                    <asp:GridView ID="grvordertax" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                        CellPadding="2" CellSpacing="1" GridLines="None" Width="99%" PageSize="50" BorderColor="#E7E7E7"
                                                        BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" ShowFooter="true" PagerSettings-Mode="NumericFirstLast"
                                                        OnPageIndexChanging="grvordertax_PageIndexChanging" OnRowDataBound="grvordertax_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Sr No
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Order Number
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <%-- <asp:Label ID="lblfromEamil" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>--%>
                                                                    <a target="_blank" href='../Orders/Orders.aspx?id=<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'>
                                                                        <%# DataBinder.Eval(Container.DataItem, "OrderNumber")%>
                                                                    </a>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Store Name
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfromEamil" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StoreName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Order Date
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfromEamil" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderDate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lbltxttotal" runat="server" Text="Total Amount" />
                                                                </FooterTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Order Tax
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltoEamil" runat="server" Text='<%# String.Format("{0:C}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"OrderTax"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotaltax" runat="server" />
                                                                </FooterTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Order Total
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStorename" runat="server" Text='<%# String.Format("{0:C}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"OrderTotal"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Ship To Address
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSubject" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FullShippingAddress") %>'></asp:Label></a>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                        <FooterStyle BackColor="#E1E1E1" ForeColor="#000" Font-Bold="true" HorizontalAlign="Right"
                                                            BorderWidth="0" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trdelete" runat="server" visible="false">
                                            <td align="left" style="padding-left: 10px;">
                                            </td>
                                            <td align="right" colspan="2" style="padding-right: 10px;">
                                                <div style="display: none;">
                                                    <%--<asp:Button ID="btnDeleteTemp" runat="server" OnClick="btnDelete_Click" />--%>
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
