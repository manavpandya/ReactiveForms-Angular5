<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CustomerAvailabilityList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.CustomerAvailabilityList"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>

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

           function validation() {
               var str = $('#ContentPlaceHolder1_txtSearch').val();
               if (!str.replace(/\s/g, '').length) {
                   jAlert('Please enter search value.', 'Message', '<%=txtSearch.ClientID%>');
                return false;
            }
            else {
                if (event.keyCode == 13) {
                    document.getElementById('<%=btnSearch.ClientID%>').click();
                }
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
            <div class="create-new-order" style="width: 100%">
                <div style="vertical-align: middle; margin-right: 3px; float: left; display: none;">
                    <table style="margin-top: 5px; float: left;">
                        <tr>
                            <td align="left">Store :
                                <asp:DropDownList ID="ddlStore" runat="server" Width="185px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexchange" CssClass="order-list"
                                    Style="margin-left: 0px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" class="add-product" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <th width="100%">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Customer Availability Notification List" alt="Customer Availability Notification List" src="/App_Themes/<%=Page.Theme %>/Images/customer-list-icon.png" />
                                                    <h2>Inventory Availability Notification List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td style="padding-top: 0px; padding-bottom: 0px;">
                                                <table style="width: 100%; text-align: right">
                                                    <tr>
                                                        <td align="left" style="padding-right: 4px">Total Availability:
                                                            <asp:Label runat="server" ID="lbltotala"></asp:Label>
                                                        </td>
                                                        <td align="right" width="7%" valign="middle">Mail Status&nbsp;:
                                                        </td>
                                                        <td style="width: 2%">
                                                            <asp:DropDownList ID="ddlMailStatus" runat="server" Width="115px" AutoPostBack="true" OnSelectedIndexChanged="ddlMailStatus_SelectedIndexChanged"
                                                                CssClass="order-list">
                                                                <asp:ListItem Value="-1">All Mail</asp:ListItem>
                                                                <asp:ListItem Value="1">Mail Send</asp:ListItem>
                                                                <asp:ListItem Value="0">Mail not Send</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="middle" align="right">From Date:
                                                        </td>
                                                        <td width="6%" valign="middle" align="left">
                                                            <asp:TextBox ID="txtMailFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td width="3%" valign="middle" align="right">To Date:
                                                        </td>
                                                        <td valign="middle" width="6%" align="left">
                                                            <asp:TextBox ID="txtMailTo" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="right" width="3%" valign="middle">Search&nbsp;:
                                                        </td>
                                                        <td style="width: 2%">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" Width="115px" AutoPostBack="False"
                                                                CssClass="order-list">
                                                                <asp:ListItem Value="Name">Name</asp:ListItem>
                                                                <asp:ListItem Value="Email">Email</asp:ListItem>
                                                                <asp:ListItem Value="SKU" Selected="True">SKU</asp:ListItem>
                                                                <asp:ListItem Value="ProductName">Product Name</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="middle" width="2%">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Style="width: 124px; vertical-align: text-top"
                                                                ValidationGroup="SearchGroup"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 2%">
                                                            <asp:Button ID="btnSearch" runat="server" OnClientClick="return validation();" OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td style="padding: 0px 0px; width: 2%;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="javascript:chkHeight();"
                                                                CausesValidation="False" />
                                                            <div style="display: none;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <%-- <td colspan="5" align="left" style="padding-right: 4px">Total Availability:
                                                            <asp:Label runat="server" ID="lbltotala"></asp:Label>
                                                        </td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td style="padding-top: 0px; padding-bottom: 0px;">
                                                <asp:GridView ID="grdcustomernotify" AutoGenerateColumns="false" runat="server" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" DataKeyNames="ProductID" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                    HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" ShowHeaderWhenEmpty="true"
                                                    OnRowCommand="grdProduct_RowCommand" OnRowDataBound="grdProduct_RowDataBound" OnPageIndexChanging="grdcustomernotify_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                <asp:HiddenField ID="hdnAvailabilityid" runat="server" Value='<%#Eval("AvailabilityID") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Customer Name
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltName" runat="server"></asp:Literal>
                                                                <asp:Literal ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Customer Email
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltEmail" runat="server"></asp:Literal>
                                                                <asp:Literal ID="lblEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Product SKU
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSKU" runat="server" Text='<%# Bind("ProductSKU") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                            <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Child SKU
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblChildSKU" runat="server" Text='<%# Bind("childsku") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Product Name
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltProductName" runat="server"></asp:Literal>
                                                                <asp:Literal ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Mail Sent
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="center" />
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltMailSent" runat="server" Text='<%# Bind("MailSent") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Sent On
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblmaildate" runat="server" Text='<%# Eval("mailsenddate", "{0:dd MMM yyyy}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                View
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="center" />
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltviewmail" runat="server" Visible="false" Text='<%# Bind("MailID") %>'></asp:Literal>
                                                                <div id="aviewlink" runat="server">
                                                                    <a href="javascript:void(0);" style="color: #212121;" onclick="OpenCenterWindow('/admin/Reports/Maildetail.aspx?MID=<%# DataBinder.Eval(Container.DataItem,"MailID") %>',800,800);">
                                                                        <img src="/App_Themes/<%=Page.Theme %>/images/view-details.png" border="0" /></a>

                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="altrow" id="trBottom" runat="server">
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
