<%@ Page Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerQuoteList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.CustomerQuoteList"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
     <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    function OpenQuoteComment(id) {
              disablePopup();
document.getElementById("frmdisplayquick").contentWindow.document.body.innerHTML= '';
              document.getElementById('frmdisplayquick').height = '680px';
              document.getElementById('frmdisplayquick').width = '1000px'; document.getElementById('popupContact').setAttribute("style", "z-index:1000001; top: 0px; padding:0px;width:1000px;height:700px;position:absolute;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
              document.getElementById('popupContact').style.width = '1000px';
              document.getElementById('popupContact').style.height ='707px';
              document.getElementById('frmdisplayquick').src = '/Admin/Orders/Viewcustomerquotecomments.aspx?StoreId=' + document.getElementById('ContentPlaceHolder1_ddlStore').options[document.getElementById('ContentPlaceHolder1_ddlStore').selectedIndex].value+ '&custquoteid=' + id;
              centerPopup();
              loadPopup();
          }


        function SearchValidation() {
            if (document.getElementById('<%=txtSearch.ClientID%>') != null && document.getElementById('<%= txtSearch.ClientID%>').value == '') {
                jAlert('Please enter search value.', 'Message', '<%=txtSearch.ClientID%>');
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function checkondelete(id) {
            jConfirm('Are you sure want to delete selected Customer Quote ?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById("ContentPlaceHolder1_hdnCustDelete").value = id;
                    //alert(document.getElementById("ContentPlaceHolder1_hdnCustDelete").value);
                    document.getElementById("ContentPlaceHolder1_btnhdnDelete").click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (on.toString() == 'false') {

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
                $(document).ready(function () { jAlert('Select at least one Customer Quote!', 'Message'); });
                return false;
            }
            else {
                return ConfirmDelete();
            }
        }

        function ConfirmDelete() {
            jConfirm('Are you sure want to delete all selected Customer Quote(s) ?', 'Confirmation', function (r) {
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
    <script runat="server">
        string _ReturnUrl;
        public string SetImage(string _FreeShipping)
        {
            if (_FreeShipping.ToString() == "0")
            {
                _ReturnUrl = "-";

            }
            else
            {
                _ReturnUrl = _FreeShipping;

            }
            return _ReturnUrl;
        }
        public void OpenFile(string filepath)
        {
            Response.Redirect(filepath);
        }
    </script>
    <script runat="server">
        string _ReturnUrlname;
        public string SetRegisterImage(bool _Value)
        {
            if (_Value == true)
            {
                _ReturnUrlname = "../Images/yes.png";
            }
            else
            {
                _ReturnUrlname = "../Images/no.png";
            }
            return _ReturnUrlname;
        }
    </script>
    <script runat="server">
        string _ReturnUrld;
        public string SetImagefordelete(bool _Value)
        {
            if (_Value == false)
            {
                _ReturnUrld = "../Images/yes.png";
            }
            else
            {
                _ReturnUrld = "../Images/no.png";
            }
            return _ReturnUrld;
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
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
                                 </asp:DropDownList>
                                  <asp:TextBox id="hdnLoginID" runat="server" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/Orders/CustomerQuote.aspx">
                    <img alt="Add Customer Quote" title="Add Customer Quote" src="/App_Themes/<%=Page.Theme %>/images/add-customer-quote.png" /></a></span>
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
                                                    <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/Images/customer-list-icon.png" />
                                                    <h2>
                                                        Customer Quote List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.CustomerQuoteComponent" SelectMethod="GetDataByFilter"
                                                    DeleteMethod="DeleteCustomerQuoteList" StartRowIndexParameterName="startIndex"
                                                    MaximumRowsParameterName="pageSize" SortParameterName="sortBy" SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                          <asp:ControlParameter ControlID="hdnLoginID" DbType="Int32" Name="ploginid" DefaultValue="0" />
                                                    </SelectParameters>
                                                    <DeleteParameters>
                                                        <asp:Parameter Name="CustomerID" Type="Int32" />
                                                        <asp:Parameter Name="Val" Type="Int32" DefaultValue="1" />
                                                    </DeleteParameters>
                                                </asp:ObjectDataSource>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="padding-right: 0px; width: 80%" align="right">
                                                            <table>
                                                                <tr>
                                                                    <td valign="top" style="text-align: left">
                                                                        Search :&nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"
                                                                            Width="200px"></asp:TextBox><br />
                                                                        <span style="padding-left: 50px;">(eg. Quote Number, Name, Email) </span>
                                                                    </td>
                                                                    <td valign="top" align="right" style="padding-right: 0px;">
                                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation();" />
                                                                        <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                                    </td>
                                                                </tr>
                                                                <%-- <tr>
                                                                    <td colspan="2" align="right">
                                                                        <span style="float: right; padding-top: 3px; padding-right: 5px; font-size: 12px;
                                                                            font-family: Arial,Helvetica,sans-serif;">Total Records :
                                                                            <% =Solution.Bussines.Components.CustomerQuoteComponent._count.ToString()%></span>
                                                                    </td>
                                                                </tr>--%>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdCustomerQuote" runat="server" AutoGenerateColumns="False" DataKeyNames="CustomerQuoteID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                    GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                    PagerSettings-Mode="NumericFirstLast" DataSourceID="_gridObjectDataSource" OnRowCommand="grdCustomerQuote_RowCommand"
                                                    OnRowDataBound="grdCustomerQuote_RowDataBound" CellPadding="2" CellSpacing="1"
                                                    BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7" >
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustomerID") %>'></asp:Label>
                                                                <asp:Label ID="lblStoreId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"StoreId") %>'></asp:Label>
                                                                <asp:Label ID="lblCustomerQuoteID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustomerQuoteID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                <asp:HiddenField ID="hdnCustomerQuoteID" runat="server" Value='<%#Eval("CustomerQuoteID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Middle" Width="7%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Quote Number
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnQuoteNumber" runat="server" CommandArgument="DESC" CommandName="QuoteNumber"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" Visible="false" />
                                                                <asp:Label ID="lblQuoteNumber" runat="server" Text='<%# Eval("QuoteNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Customer Name
                                                                <asp:ImageButton ID="btnCustName" runat="server" CommandArgument="DESC" CommandName="CustomerName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" Visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <a style="font-weight: bold; font-size: 12px; color: #D5321C;" href='<%# "CustomerQuoteView.aspx?Mode=view&ID=" + DataBinder.Eval(Container.DataItem,"CustomerQuoteID") %>'>
                                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName")%>'></asp:Label></a>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Email
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Created By
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreatedby" runat="server" Text='<%# Eval("CreatedName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Created On
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Eval("CreatedOn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Is Fulfilled
                                                            </HeaderTemplate>
                                                            <HeaderStyle BackColor=" #E7E7E7" />
                                                             <ItemTemplate>
<label id="fullorder">
                                                            <a id="orderno" href="javascript:void(0)" runat="server" target="_blank"  class="order-no" visible="false"><%#SetImage(Convert.ToString(DataBinder.Eval(Container.DataItem,"OrderNumber")))%></a>
                                                            <asp:HiddenField ID="hdnOrderNo" runat="server" Value='<%#Eval("OrderNumber") %>' />
<asp:Label ID="lblOrderno" runat="server" Text='<%# SetImage(Convert.ToString(DataBinder.Eval(Container.DataItem,"OrderNumber")))%>' Visible="false"></asp:Label> </label>
</ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        
                                                         <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                View Customer Quote Comments
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            <div id="divcomment" runat="server">
                                                              <a   style="font-weight: bold; font-size: 12px; color: #D5321C;" href="javascript:void(0)" onclick="OpenQuoteComment('<%#Eval("CustomerQuoteID") %>')" >
                                                              <asp:Label ID="lblQuoteid" runat="server" Text='<%# Eval("CustomerQuoteID")%>' Visible="false"></asp:Label>
                                                              View Customer Comment
                                                            </a>
                                                            </div>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Download PDF
                                                            </HeaderTemplate>
                                                            <HeaderStyle BackColor=" #E7E7E7" />
                                                            <ItemTemplate>
                                                                <input type="hidden" id="mainid" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"CustomerQuoteID") %>' />
                                                                <asp:Button ToolTip="Download PDF" CommandName="downloadpdf" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"QuoteNumber") %>'
                                                                    ID="dnwid" Visible="false" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" BackColor=" #E7E7E7" />
                                                            <HeaderTemplate>
                                                                Revise
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRevised" Visible="false" runat="server" Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsRevised")) %>'></asp:Label>
                                                                <a style="color: #212121;" id="tagQuoteNumber" title="Revise" runat="server">
                                                                    <img id="ImgQuoteNumber" runat="server" />
                                                                </a>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                        <tr class="altrow" runat="server" id="trBottom">
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;
                                                        padding-right: 0px;">
                                                        <asp:Button ID="btnDeleteQuote" runat="server" OnClientClick="return checkCount();"
                                                            OnClick="btnDeleteQuote_Click" CommandName="DeleteMultiple" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDeleteQuote_Click" />
                                                        </div>
                                                    </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
    
<div id="popupContact" style="z-index: 1000001; width: 700px; height: 328px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="border: 1px solid #444;
            background-color: #fff; font-size: 12px;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;View Customer Comments
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupContactClose" Style="position: relative;" 
                        runat="server" ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <iframe id="frmdisplayquick" frameborder="0" height="650" width="580" scrolling="auto">
                                </iframe>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
