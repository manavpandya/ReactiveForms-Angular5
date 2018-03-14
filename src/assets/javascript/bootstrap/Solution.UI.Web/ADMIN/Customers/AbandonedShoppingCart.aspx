<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="AbandonedShoppingCart.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.AbandonedShoppingCart"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message'); }); 
                document.getElementById('<%=txtSearch.ClientID %>').focus();
                return false;
            }
            return true;
        }
        function selectAll() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox' && myform.elements[i].id.toString().toLowerCase().indexOf('_chkselect_') > -1) {
                    if (document.getElementById('ContentPlaceHolder1_grdAbandonedShoppingCart_chkall').checked == false) {

                        if (myform.elements[i].checked) {
                            myform.elements[i].checked = false;
                        }
                    }
                    else {
                        myform.elements[i].checked = true;
                    }
                }
            }

        }

    </script>
    <script language="javascript" type="text/javascript">


//        function OpenViewCart(CNo, SNo) {

//            var width = 850;
//            var height = 600;
//            var left = parseInt((screen.availWidth / 2) - (width / 2));
//            var top = parseInt((screen.availHeight / 2) - (height / 2));
//            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
//            window.open('AbandonedShoppingCartitem.aspx?CNo=' + CNo + '&SNo=' + SNo, '', windowFeatures);
        //        }

        function OpenViewCart(CNo, SNo, StoreID) {

            var width = 850;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('AbandonedShoppingCartitem.aspx?CNo=' + CNo + '&SNo=' + SNo +'&StoreID=' +StoreID, '', windowFeatures);
        }

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
                if (elt.type == "checkbox" && elt.id.toString().toLowerCase().indexOf('contentplaceholder1_grdabandonedshoppingcart_chkall') <=-1) {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {
                jAlert("Please select at least one Abandoned Shopping Cart.", "Message");
                return false;

            }
            else {
                jConfirm('Are you sure want to delete all selected Abandoned Shopping Cart ?', 'Confirmation', function (r) {
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
        function chkSelectSend() {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox" && elt.id.toString().toLowerCase().indexOf('contentplaceholder1_grdabandonedshoppingcart_chkall') <= -1) {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {
                jAlert("Please select at least one Abandoned Shopping Cart.", "Message");
                return false;

            }
             
            return true;

        }
    </script>
    <script language="javascript" type="text/javascript">
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
                $(document).ready(function () { jAlert('Select at least one AbandonedShoppingCart!', 'Message'); });
                return false;
            }
            else {
                return ConfirmDelete();
            }
        }

        function ConfirmDelete() {
            jConfirm('Are you sure want to delete all selected AbandonedShoppingCart(s) ?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }
        
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <table>
                    <tr>
                        <td align="left">
                            Store :&nbsp;<asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list"
                                Width="175px" AutoPostBack="true"   Style="margin-top: 5px;"     OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
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
                                                    <img class="img-left" title="Abandoned Shopping Cart" alt="Abandoned Shopping Cart" src="/App_Themes/<%=Page.Theme %>/Images/topic-list-icon.png" />
                                                    <h2>
                                                        Abandoned Shopping Cart</h2>
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
                                                        </td>
                                                        <td valign="middle" align="right">
                                                        </td>
                                                        <td valign="middle" align="right">
                                                        </td>
                                                        <td valign="middle" align="right">
                                                        </td>
                                                        <td align="left" style="width: 65px;">
                                                            Search By :
                                                        </td>
                                                        <td align="left" style="width: 115px;">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list" Width="110px"
                                                                AutoPostBack="false">
                                                                <asp:ListItem Text="First Name" Value="FirstName"></asp:ListItem>
                                                                <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtSearch" onfocus="javascript:if(this.value=='Search Keyword'){this.value=''};"
                                                                onblur="javascript:if(this.value==''){this.value='Search Keyword'};" Text="Search Keyword"
                                                                CssClass="order-textfield" Width="124px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation(0);" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="return SearchValidation(1);" />
                                                        </td>
                                                         <td align="right">
                                                            <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" OnClientClick="return chkSelectSend();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div id="divGrid">
                                                    <asp:Label ID="lblEmailMsg" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                                                    <asp:GridView ID="grdAbandonedShoppingCart" runat="server" AutoGenerateColumns="False"
                                                        BorderStyle="Solid" CellPadding="2" CellSpacing="1" GridLines="None" Width="99%"
                                                        PageSize="50" BorderColor="#E7E7E7" BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found."
                                                        EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                        PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="grdAbandonedShoppingCart_PageIndexChanging"
                                                        OnRowCommand="grdAbandonedShoppingCart_RowCommand" OnRowDataBound="grdAbandonedShoppingCart_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <HeaderTemplate>
                                                                   <asp:CheckBox ID="chkall" runat="server" Text="&nbsp;Select" TextAlign="Right" onclick="selectAll();" onchange="selectAll();" />
                                                                  
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                      <input type="hidden" id="hdlallid" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"Email")+"*"+DataBinder.Eval(Container.DataItem,"ShoppingCartID")+"*"+DataBinder.Eval(Container.DataItem,"CustomerID")+"*"+DataBinder.Eval(Container.DataItem,"storeID") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cart Date">
                                                                <HeaderStyle BackColor=" #E7E7E7" />
                                                                <HeaderTemplate>
                                                                    Cart Date
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcartdate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CreatedOn")  %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID">
                                                                <HeaderStyle BackColor=" #E7E7E7" />
                                                                <HeaderTemplate>
                                                                    Customer ID
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustomerID")  %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID">
                                                                <HeaderStyle BackColor=" #E7E7E7" />
                                                                <HeaderTemplate>
                                                                    Shopping Cart ID
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShoppingCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ShoppingCartid")  %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <HeaderTemplate>
                                                                    First Name
                                                                    <%--  <asp:ImageButton ID="btnName" runat="server" CommandArgument="DESC" CommandName="FirstName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstName") + " " + DataBinder.Eval(Container.DataItem,"LastName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <HeaderTemplate>
                                                                    Email
                                                                    <%--     <asp:ImageButton ID="btnEmail" runat="server" CommandArgument="DESC" CommandName="Email"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <HeaderTemplate>
                                                                    Store Name
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStoreName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StoreName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle BackColor=" #E7E7E7" />
                                                                <HeaderTemplate>
                                                                    View Cart
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <%--  <a onclick="window.open('AbandonedShoppingCartitem.aspx?CNo=<%#DataBinder.Eval(Container.DataItem,"CustomerID")  %>&SNo=<%#DataBinder.Eval(Container.DataItem,"ShoppingCartid")  %>', '','height=750,width=850,scrollbars=1');"
                                                                        href="#" style="color: #212121;">View Cart </a>--%>
                                                                    <a onclick="OpenViewCart(<%#DataBinder.Eval(Container.DataItem,"CustomerID")  %>,<%#DataBinder.Eval(Container.DataItem,"ShoppingCartid")  %>,<%#DataBinder.Eval(Container.DataItem,"storeID")  %>);"
                                                                        href="#" style="color: #212121;">View Cart </a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField>
                                                                <HeaderStyle BackColor=" #E7E7E7" />
                                                                <HeaderTemplate>
                                                                   Mail Sent
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                  <asp:Label ID="IsMailSent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"IsMailSent") %>'>  </asp:Label> 

                                                                 
                                                                      </ItemTemplate>                                                             
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Edit">
                                                                <HeaderStyle BackColor="#E7E7E7" />
                                                                <HeaderTemplate>
                                                                    Send
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEmail" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/Email-Reply.jpg"
                                                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Email")+"*"+DataBinder.Eval(Container.DataItem,"ShoppingCartID")+"*"+DataBinder.Eval(Container.DataItem,"CustomerID")+"*"+DataBinder.Eval(Container.DataItem,"storeID") %>'
                                                                        CommandName="Email" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trdelete" runat="server" visible="false">
                                            <td align="left" style="padding-left: 10px;">
                                                <a id="aAllowAll" href="javascript:SelectAll(true);">Check All</a>&nbsp; | <a id="aClearAll"
                                                    href="javascript:SelectAll(false);">Clear All</a>
                                            </td>
                                            <td align="right" colspan="2" style="padding-right: 10px;">
                                                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" OnClientClick="return chkSelect();" />
                                                <div style="display: none;">
                                                    <asp:Button ID="btnDeleteTemp" runat="server" OnClick="btnDelete_Click" />
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
</asp:Content>
