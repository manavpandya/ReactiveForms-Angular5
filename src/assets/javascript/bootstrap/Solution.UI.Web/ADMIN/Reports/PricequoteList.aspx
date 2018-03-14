<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="PricequoteList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.PricequoteList" %>

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                            <th colspan="3">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Inquiries List" alt="Inquiries List" src="/App_Themes/<%=Page.Theme %>/Images/contact-inquiry-icon.png" />
                                                    <h2>
                                                        Price Quote List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <%--  <tr class="altrow">
                                            <td align="left">
                                                <table cellpadding="3" cellspacing="3" width="100%">
                                                    <tr>
                                                        <td align="left" style="width: 65px;">
                                                            Search By :
                                                        </td>
                                                        <td align="left" style="width: 180px;">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list" Width="175px"
                                                                AutoPostBack="false">
                                                                <asp:ListItem Text="Search By" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                                                <asp:ListItem Text="Name" Value="Name"></asp:ListItem>
                                                                <asp:ListItem Text="Subject" Value="Subject"></asp:ListItem>
                                                                <asp:ListItem Text="Body" Value="Message"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtSearch" onfocus="javascript:if(this.value=='Search Keyword'){this.value=''};"
                                                                onblur="javascript:if(this.value==''){this.value='Search Keyword'};" Text="Search Keyword"
                                                                CssClass="order-textfield" Width="124px" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td align="right">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td valign="top" align="left">
                                                            From Date:
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtMailFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            To Date:
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtMailTo" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation(0);" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
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
                                        </tr>--%>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div id="divGrid">
                                                    <asp:GridView ID="grpricequotelist" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                        CellPadding="2" CellSpacing="1" GridLines="None" Width="99%" PageSize="50" BorderColor="#E7E7E7"
                                                        BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" PagerSettings-Mode="NumericFirstLast"
                                                        OnPageIndexChanging="grpricequotelist_PageIndexChanging" OnRowDataBound="grpricequotelist_OnRowDatabound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Name
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblname" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                   From Email
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfromEamil" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                              <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                   To Email
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltoEamil" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ToEmail") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Product Name
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblproductname" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Assigned To
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssignname" runat="server" Text="-"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="center" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    CreatedOn
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CreatedOn") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <HeaderTemplate>
                                                                    Date
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="left" />
                                                                <ItemTemplate>
                                                                    <%--  <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ContactDate") %>'></asp:Label>--%>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    View
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a href="javascript:void(0);" style="color: #212121;" onclick="OpenCenterWindow('viewpricequotedetail.aspx?pricequoteid=<%# DataBinder.Eval(Container.DataItem,"PriceQuoteid") %>',800,450);">
                                                                        <img src="/App_Themes/<%=Page.Theme %>/images/view-details.png" border="0" /></a>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <HeaderTemplate>
                                                                    Reply
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <%--  <a href="javascript:void(0);" style="color: #212121;" onclick="OpenCenterWindow('ContactSendMail.aspx?MID=<%# DataBinder.Eval(Container.DataItem,"ContactUsID") %>',800,400);">
                                                                        <img src="/App_Themes/<%=Page.Theme %>/images/Email-Reply.jpg" border="0" /></a>--%>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                        <FooterStyle ForeColor="White" Font-Bold="true" BackColor="#545454" />
                                                    </asp:GridView>
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
