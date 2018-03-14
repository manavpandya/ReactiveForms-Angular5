﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="BulkOrderProcess.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.BulkOrderProcess" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
      <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript">

        var $j = jQuery.noConflict();
        $j(function () {

            $j('#ContentPlaceHolder1_txtFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $j('#ContentPlaceHolder1_txtToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });

             

        });
    </script>
    <script type="text/javascript">

        function ConfirmDelete(strmsg, cntrlnm) {
            jConfirmDynemicButton(strmsg, 'Confirmation', 'Yes', 'No', function (r) {
                if (r == true) {
                    document.getElementById(cntrlnm).onclick = function () { return true; };
                    //__doPostBack(cntrlnm, '');
                    document.getElementById(cntrlnm).click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }
        function ConfirmDelete1(strmsg, cntrlnm) {

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
                $(document).ready(function () { jAlert('Select at least One Rocord(s)!', 'Message', ''); });
                return false;
            }
            else {
                jConfirmDynemicButton(strmsg, 'Confirmation', 'Yes', 'No', function (r) {
                    if (r == true) {
                        document.getElementById(cntrlnm).onclick = function () { return true; };
                        //__doPostBack(cntrlnm, '');
                        document.getElementById(cntrlnm).click();
                        return true;
                    }
                    else {

                        return false;
                    }
                });
            }
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
                $(document).ready(function () { jAlert('Select at least One Rocord(s)!', 'Message', ''); });
                return false;
            }
            return true;
        }
    </script>
    <style type="text/css">
        .altrowmain td
        {
            background: #E3E3E3 !important;
            color: #000 !important;
        }
        .altrowmainsub td
        {
            background: #ffffff !important;
            color: #000 !important;
        }
        /*.content-table th{background:#545454 !important;color:#000 !important;}*/
    </style>
    <style type="text/css">
        .divfloatingcss
        {
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
        $(document).ready(function () {
            $('#divfloating').attr("class", "divfloatingcss");
            $(window).scroll(function () {
                if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                    $('#divfloating').attr("class", "");
                }
                else {
                    $('#divfloating').attr("class", "divfloatingcss");
                }
            });
        });
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
        function validation() {
           <%-- var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }--%>
            if (document.getElementById('ContentPlaceHolder1_txtFromDate') != null && document.getElementById('ContentPlaceHolder1_txtFromDate').value == '') {

                jAlert('Please enter start date.', 'Required', 'ContentPlaceHolder1_txtFromDate');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtToDate') != null && document.getElementById('ContentPlaceHolder1_txtToDate').value == '') {

                jAlert('Please enter End date.', 'Required', 'ContentPlaceHolder1_txtToDate');
                return false;
            }

            var date1 = new Date(document.getElementById('ContentPlaceHolder1_txtFromDate').value);
            var date2 = new Date(document.getElementById('ContentPlaceHolder1_txtToDate').value);
            
            if (date1 > date2) {
                jAlert('From date must be less than or equal to to date.', 'Required', 'ContentPlaceHolder1_txtFromDate');
                return false;
            }
            
            return true;
        }
    </script>
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
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
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left" style="width: 30% !important;padding:0 !important;text-align:left;">
                                                &nbsp;&nbsp;Multiple Shipping Upload
                                            </div>
                                            <div class="main-title-right" style="width: 70% !important; float: right; text-align: right;padding:0 !important;">
                                                Store :&nbsp;
                                                 <asp:DropDownList ID="ddlStore" runat="server" AutoPostBack="true" CssClass="order-list"
                            Width="190px" Height="20px"  OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                        </asp:DropDownList>  <asp:CheckBox ID="istrackingpartner" runat="server" Text="Tracking # uploaded to partner portal" />&nbsp;&nbsp;

                                             From Date:&nbsp;&nbsp;<asp:TextBox ID="txtFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                                            Style="margin-right: 3px;"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                                        To Date:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                                         
                                                Search By Order #:&nbsp;<asp:TextBox ID="txtSearch" CssClass="order-textfield" runat="server"></asp:TextBox>&nbsp;<asp:Button
                                                    ID="btnSearch" runat="server" OnClientClick="return validation();" OnClick="btnSearch_Click" />&nbsp; <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="return validation();" /></div>
                                        </th>
                                    </tr>
                                    <tr style="display:none;">
                                        <td>
                                            <asp:TextBox ID="txtAllOrernumber" Width="300px" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="btnupdateoverstock" OnClick="btnupdateoverstock_Click" runat="server" Text="Update" />
                                        </td>
                                        
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
                                                    <tr class="even-row" id="trFabricDetails" runat="server">
                                                        <td align="center">
                                                            <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr id="trTop" runat="server">
                                                                    <td class="border-none" align="left">
                                                                        <span><a style="color: #B92127; font-size: 12px; font-weight: normal;" href="javascript:selectAll(true);">
                                                                            Check All</a></span> <span><a style="color: #B92127; font-size: 12px; font-weight: normal;"
                                                                                href="javascript:selectAll(false);">Clear All</a> </span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%">
                                                                        <asp:GridView ID="grdAllorder" runat="server" GridLines="None" AutoGenerateColumns="false"
                                                                            CssClass="checklist-main border-right-all" AllowPaging="false" BorderColor="#d7d7d7" EmptyDataText="No Record(s) Found."
                                                                            BorderWidth="1px" BorderStyle="Solid" ShowFooter="false" OnRowDataBound="grdAllorder_RowDataBound"
                                                                            Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="&nbsp;Order #">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkselect" runat="server" />&nbsp;<a href="/admin/orders/Orders.aspx?id=<%# Eval("OrderNumber")%>" style="font-weight: normal;
                                        text-decoration: underline; font-size: 11px;">
                                        <%# Eval("OrderNumber")%><asp:Label ID="lblorderNumber"
                                                                                            Font-Bold="true" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>&nbsp;<asp:Label ID="Label1"
                                                                                            Font-Bold="true" runat="server" Text='<%#"(Ref. # "+ DataBinder.Eval(Container.DataItem,"Reforderid")+")" %>'></asp:Label>
                                                                                        <asp:Label ID="lblreforderid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"Reforderid") %>'></asp:Label></a>
                                                                                    </ItemTemplate>
                                                                                    <FooterStyle HorizontalAlign="Left" />
                                                                                    <HeaderStyle Width="70%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="&nbsp;Order Date">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbldate" runat="server" Font-Bold="true" Text='<%#DataBinder.Eval(Container.DataItem,"OrderDate") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                      
                                                                                        <tr>
                                                                                            <td colspan="100%" align="left">
                                                                                                <div id="divchild" style="margin: 2px 0 0 0; position: relative; left: 15px; overflow: auto;
                                                                                                    padding-bottom: 5px; width: 100%;">
                                                                                                    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="false" Width="98%"
                                                                                                        CssClass="table-noneforOrder" CellSpacing="5" CellPadding="5" OnRowDataBound="gvProducts_RowDataBound">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField Visible="false">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblCustomCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblPrice" runat="server" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")).ToString("f2") %>'
                                                                                                                        Visible="false"></asp:Label>
                                                                                                                    <asp:Label ID="lblVariantname" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'
                                                                                                                        Visible="false"></asp:Label>
                                                                                                                    <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'
                                                                                                                        Visible="false"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    Product Name
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Product Name") %>'></asp:Label>
                                                                                                                  
                                                                                                                    <div style="padding-left: 20px;">
                                                                                                                        <asp:Label ID="lblAssambly" runat="server"></asp:Label>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="left" Width="35%" />
                                                                                                                
                                                                                                                <ItemStyle Width="35%" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    SKU
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                              
                                                                                                                 <HeaderStyle Width="10%" />
                                                                                                                
                                                                                                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    Ordered Quantity
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblOrderQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Ordered Quantity") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                              
                                                                                                                 <HeaderStyle Width="10%" />
                                                                                                                
                                                                                                                <ItemStyle HorizontalAlign="center" Width="10%" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField Visible="false">
                                                                                                                <HeaderTemplate>
                                                                                                                    Available
                                                                                                                    <br />
                                                                                                                    Inventory
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblInventory" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Available Inventory") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                 
                                                                                                                
                                                                                                            </asp:TemplateField>
                                                                                                           
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    Shipped Quantity
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblShippedQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Shipped Quantity") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                
                                                                                                                <HeaderStyle Width="10%" />
                                                                                                                <ItemStyle HorizontalAlign="center" Width="10%" />
                                                                                                            </asp:TemplateField>
                                                                                                         
                                                                                                          
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    Price
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    $<asp:Label ID="lblUpgradePrice" runat="server" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"price")).ToString("f2") %>'></asp:Label>
                                                                                                                    
                                                                                                                </ItemTemplate>
                                                                                                                 
                                                                                                                  <HeaderStyle Width="5%" />
                                                                                                                <ItemStyle HorizontalAlign="Right" Width="5%" />
                                                                                                            </asp:TemplateField>
                                                                                                             <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    Tracking Number
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblTrackingNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TrackingNumber") %>'></asp:Label>
                                                                                                                    
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle  />
                                                                                                                 <HeaderStyle Width="12%" />
                                                                                                                
                                                                                                                <ItemStyle HorizontalAlign="left" Width="12%" />
                                                                                                            </asp:TemplateField>
                                                                                                             <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    Shipped Via
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblShippedVia" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ShippedVia") %>'></asp:Label>
                                                                                                                  
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                                 <HeaderStyle Width="10%"  />
                                                                                                                
                                                                                                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderTemplate>
                                                                                                                    Shipped On
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                  
                                                                                                                     <asp:Label ID="lblShippedOn" runat="server" Font-Bold="true" Text='<%# string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ShippedOn"))) %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblaccepted" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"IsAccepted") %>'></asp:Label>
                                                                                                                    <asp:Label ID="OrderItemID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderItemID") %>'></asp:Label>
                                                                                                                    <input type="hidden" id="hdncustom" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle  />
                                                                                                                 <HeaderStyle Width="15%" />
                                                                                                                
                                                                                                                <ItemStyle HorizontalAlign="center" Width="15%" />
                                                                                                            </asp:TemplateField>

                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <AlternatingRowStyle CssClass="altrowmain" HorizontalAlign="left" />
                                                                            <RowStyle CssClass="altrowmain" />
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trbottom" runat="server">
                                                                    <td class="border-none" align="left">
                                                                        <span><a style="color: #B92127; font-size: 12px; font-weight: normal;" href="javascript:selectAll(true);">
                                                                            Check All</a></span> <span><a style="color: #B92127; font-size: 12px; font-weight: normal;"
                                                                                href="javascript:selectAll(false);">Clear All</a> </span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" id="trsavevendor" runat="server" visible="false">
                                                        <td align="center">
                                                            <div id="divfloating" class="divfloatingcss" style="width: 380px;">
                                                                <div style="margin-bottom: 1px; margin-top: 3px;width:20%;float:center;width: 27%; text-align: right;"><asp:Button ID="btnUploadOrder" runat="server" ToolTip="Upload Order"
                                                            OnClientClick="return checkCount();" OnClick="btnUploadOrder_Click" /> </div> <div style="margin-bottom: 1px; margin-top: 3px;width:80%;float:left; width: 71%; text-align: left;">
                                                                    
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span style="padding-left: 20px"></span>
                                                            <input type="hidden" value="0" id="hdnVariantId" runat="server" />
                                                            <input type="hidden" value="0" id="hdnVariantIdGrid" runat="server" />
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
            <tr>
                <td>
                    <div style="display: none;">
                        <input type="hidden" id="hdnrelatedsku1" runat="server" value='' />
                        <input type="hidden" id="hdnrelatedsku" runat="server" value='' />
                    </div>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="20%" style="margin-top: 25%;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>