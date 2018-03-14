<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.CustomerList"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function ShowModelCredit() {
            document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '237px';
            document.getElementById('frmdisplay1').width = '509px';
            document.getElementById('frmdisplay1').scrolling = 'no';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:510px;height:217px;");
            document.getElementById('popupContact1').style.width = '510px';
            document.getElementById('popupContact1').style.height = '217px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = '/ADMIN/customers/ExportCustomerVarification.aspx';
        }
       

       $(function () {

           $('#ContentPlaceHolder1_txtFromDate').datetimepicker({
               showButtonPanel: true, ampm: false,
               showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
               buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
              });
        });
    </script>
    <script type="text/javascript">
        function SearchValidation() {
            if (document.getElementById('<%=txtSearch.ClientID%>') != null && document.getElementById('<%= txtSearch.ClientID%>').value == '' && document.getElementById('<%=txtphone.ClientID%>') != null && document.getElementById('<%= txtphone.ClientID%>').value == '' && document.getElementById('<%=txtzipcode.ClientID%>') != null && document.getElementById('<%= txtzipcode.ClientID%>').value == '' && document.getElementById('<%=txtEmail.ClientID%>') != null && document.getElementById('<%= txtEmail.ClientID%>').value == '') {
                jAlert('Please enter search value.', 'Message', '<%=txtSearch.ClientID%>');
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function checkondelete(id) {
            jConfirm('Are you sure want to delete selected Customer ?', 'Confirmation', function (r) {
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
    <script runat="server">
        string _ReturnUrl;
        public string SetImage(bool _Value)
        {
            if (_Value == true)
            {
                _ReturnUrl = "../Images/active.gif";
            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";
            }
            return _ReturnUrl;
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
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/Customers/Customer.aspx">
                    <img alt="Add Customer" title="Add Customer" src="/App_Themes/<%=Page.Theme %>/images/add-customer.png" /></a></span>
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
                                                        Customer List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.CustomerComponent" SelectMethod="GetDataByFilter"
                                                    DeleteMethod="DeleteCustomerList" StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize"
                                                    SortParameterName="sortBy" SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                           <asp:ControlParameter ControlID="txtEmail" DbType="String" Name="pSearchemail" DefaultValue="" />
                                                         <asp:ControlParameter ControlID="txtzipcode" DbType="String" Name="pSearchzipcode" DefaultValue="" />
                                                         <asp:ControlParameter ControlID="txtphone" DbType="String" Name="pSearchphone" DefaultValue="" />
                                                         <asp:ControlParameter ControlID="txtisostback" DbType="Boolean" Name="ppostback" DefaultValue="" />
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
                                                                        Name :&nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"
                                                                            Width="217px"></asp:TextBox> 
                                                                    </td>
                                                                    <td valign="top" style="text-align: left">
                                                                        Email :&nbsp;<asp:TextBox ID="txtEmail" runat="server" CssClass="order-textfield"
                                                                            Width="217px"></asp:TextBox> 
                                                                    </td>
                                                                    <td valign="top" style="text-align: left">
                                                                        Zip code :&nbsp;<asp:TextBox ID="txtzipcode" runat="server" CssClass="order-textfield"
                                                                            Width="217px"></asp:TextBox> 
                                                                    </td>
                                                                    <td valign="top" style="text-align: left">
                                                                        Phone :&nbsp;<asp:TextBox ID="txtphone" runat="server" CssClass="order-textfield"
                                                                            Width="217px"></asp:TextBox> 
                                                                        <asp:TextBox ID="txtisostback" runat="server" Visible="false"></asp:TextBox>
                                                                    </td>
                                                                    <td valign="top" align="right" style="padding-right: 0px;">
                                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation();" />
                                                                        <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                                      
                                                                    </td>
                                                                        <td valign="top" style="text-align: left" runat="server" id="t1">From Date :
                                                                    </td>
                                                                    <td valign="top" style="text-align: left" runat="server" id="t2">
                                                                        <asp:TextBox ID="txtFromDate" runat="server" class="order-textfield" Style="width: 70px; margin-right: 5px;"></asp:TextBox>
                                                                    </td>
                                                                    <td valign="top" style="text-align: left" runat="server" id="t3">To Date :
                                                                    </td>
                                                                    <td valign="top" style="text-align: left" runat="server" id="t4">
                                                                        <asp:TextBox ID="txtToDate" runat="server" class="order-textfield" Style="width: 70px; margin-right: 5px;"></asp:TextBox>
                                                                    </td>
                                                                    <td valign="top" align="left" style="padding-right: 0px;">
                                                                       
                                                                        <asp:Button ID="btnExport" runat="server" ToolTip="Export"  OnClientClick="ShowModelCredit();return false;"   />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="5" align="right">
                                                                        <span style="float: right; padding-top: 3px; padding-right: 5px; font-size: 12px;
                                                                            font-family: Arial,Helvetica,sans-serif;">Total Record :
                                                                            <% =Solution.Bussines.Components.CustomerComponent._count.ToString()%></span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdCustomer" runat="server" AutoGenerateColumns="False" DataKeyNames="CustomerID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                    GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                    PagerSettings-Mode="NumericFirstLast" DataSourceID="_gridObjectDataSource" OnRowCommand="grdCustomer_RowCommand"
                                                    OnRowDataBound="grdCustomer_RowDataBound" CellPadding="2" CellSpacing="1" BorderStyle="solid"
                                                    BorderWidth="1" BorderColor="#e7e7e7">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnCustListID" runat="server" Value='<%#Eval("CustomerID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Customer ID
                                                                <asp:ImageButton ID="btnCustID" runat="server" CommandArgument="DESC" CommandName="CustomerID"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustID" runat="server" Text='<%# Eval("CustomerID") %>'></asp:Label>
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
                                                                Customer Name
                                                                <asp:ImageButton ID="btnCustName" runat="server" CommandArgument="DESC" CommandName="CustomerName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Store Name
                                                                <asp:ImageButton ID="btnStoreName" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreName" runat="server" Text='<%# Eval("StoreName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>'
                                                                    alt="" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Registered">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <img src='<%# SetRegisterImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsRegistered"))) %>'
                                                                    alt="" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deleted" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <img src='<%# SetImagefordelete(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Deleted"))) %>'
                                                                    alt="" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("CustomerID") %>'></asp:ImageButton>
                                                                <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" Visible="false" CommandName="delete"
                                                                    message='<%# Eval("CustomerID") %>' CommandArgument='<%# Eval("CustomerID") %>'
                                                                    OnClientClick='return checkondelete(this.getAttribute("message"));'></asp:ImageButton>
                                                            </ItemTemplate>
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
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="display: none">
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <asp:ImageButton ID="popupContactClose1" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
            <asp:Button ID="btntemp" runat="server" OnClick="btnExport_Click" />
    </div>
     <div id="popupContact1" style="z-index: 1000001; width: 750px; height: 350px;">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay1" frameborder="0" height="350px" width="750" scrolling="auto">
                    </iframe>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
