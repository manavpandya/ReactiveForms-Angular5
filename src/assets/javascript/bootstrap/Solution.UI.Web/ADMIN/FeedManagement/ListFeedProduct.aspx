<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ListFeedProduct.aspx.cs" Inherits="Solution.UI.Web.ADMIN.FeedManagement.ListFeedProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
<script type="text/javascript">
//    function chkHeight() {
//        var windowHeight = 0;
//        windowHeight = $(document).height(); //window.innerHeight;

//        document.getElementById('prepage').style.height = windowHeight + 'px';
//        document.getElementById('prepage').style.display = '';
//    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="content-width">
        <div class="content-row1">
            <table width="100%">
                <tr>
                    <td align="left">
                        <table>
                            <tr>
                                <td>
                                    Store :
                                    <asp:DropDownList ID="ddlStore" Width="200px" runat="server" AutoPostBack="true"
                                        CssClass="order-list" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table>
                            <tr>
                                <td valign="middle" style="text-align: left">
                                    Search : &nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"
                                        Width="100px"></asp:TextBox><br />
                                </td>
                                <td valign="middle" style="padding-right: 0px; line-height: 22px;">
                                    <asp:Button ID="btnSearch" runat="server" OnClientClick="return SearchValidation();"
                                        OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnShowall" runat="server" OnClientClick="return vlvalue();" OnClick="btnShowall_Click" /><br />
                                    <span style="float: right; padding-right: 5px; font-size: 12px; font-family: Arial,Helvetica,sans-serif;">
                                        <asp:Label ID="lblTotcount" runat="server"></asp:Label>
                                    </span>
                                </td>
                                <td valign="top" align="right" style="vertical-align: top; padding-bottom: 9px">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
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
                                                <div class="main-title-left" style="width: 98% !important">
                                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                                    </asp:ScriptManager>
                                                    <img src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png" style="padding-top: 4px"
                                                        alt="Feed Product List" title="Feed Product List" class="img-left" />
                                                    <h2>
                                                        <span style="line-height: 25px;">Feed Product List</span> <span style="float: right;">
                                                            Feed Name :
                                                            <asp:DropDownList ID="ddlFeedName" runat="server" CssClass="order-list" Style="width: 170px !important"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlFeedName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="ddlbasestore" Visible="false" runat="server" Width="198px"
                                                                AutoPostBack="false">
                                                            </asp:DropDownList>
                                                            <asp:UpdatePanel ID="upl" runat="server" style="float: right;">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnIsBase" Text=""   Style="background: width: 119px; height: 25px;"
                                                                        runat="Server" OnClick="btnIsBase_Click" />
                                                                    <asp:Button ID="btnFromIsBase" Text=""   Style="background: width: 150px; height: 25px;"
                                                                        runat="Server" OnClick="btnFromIsBase_Click" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </span>
                                                    </h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <div class="content_box">
                                                    <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                        <tbody>
                                                            <tr>
                                                                <td class="border">
                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="gvFeedMaster" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                                                                CellSpacing="1" Width="100%" CssClass="product_table" GridLines="None" AllowPaging="True"
                                                                                OnPageIndexChanging="gvFeedMaster_PageIndexChanging" BorderWidth="1px" BorderStyle="Solid"
                                                                                BorderColor="#E7E7E7" EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled"
                                                                                OnRowCommand="gvFeedMaster_RowCommand" PageSize="50" OnRowDataBound="gvFeedMaster_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField Visible="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblFeedId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FeedId") %>'></asp:Label>
                                                                                            <asp:Label ID="lblProduct" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductId") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Select" Visible="False">
                                                                                        <HeaderStyle BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>Select</strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Left" BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>&nbsp;Product Name</strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle />
                                                                                        <ItemTemplate>
                                                                                            <a style="color: #000; text-decoration: underline;" href='<%# "GenerateExportpage.aspx?Mode=edit&StoreID=" + DataBinder.Eval(Container.DataItem,"StoreId") +"&ID=" + DataBinder.Eval(Container.DataItem,"ProductId") +"&FID="+DataBinder.Eval(Container.DataItem,"FeedId") %>'>
                                                                                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label></a>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="left" Width="40%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Left" BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>&nbsp;SKU</strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle Width="10%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblsku" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"sku") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="Left" BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>&nbsp;Related Feeds</strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle Width="45%" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Literal ID="ltfedd" runat="server"></asp:Literal>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Edit">
                                                                                        <HeaderStyle BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>Edit </strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:HyperLink ID="hprlnkEdit" runat="server" NavigateUrl='<%# "GenerateExportpage.aspx?Mode=edit&StoreID=" + DataBinder.Eval(Container.DataItem,"StoreId") +"&ID=" + DataBinder.Eval(Container.DataItem,"ProductId") +"&FID="+DataBinder.Eval(Container.DataItem,"FeedId") %>'
                                                                                                CssClass="link-font" Font-Underline="True"></asp:HyperLink>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Edit">
                                                                                        <HeaderStyle BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>Delete </strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="btnDelete" ImageUrl="~/Admin/images/delete-icon.gif" runat="server"
                                                                                                OnClientClick="javascript:return confirm('Are you sure to delete this record ?');"
                                                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductId") + "-" + DataBinder.Eval(Container.DataItem,"IsBase") + "-" + DataBinder.Eval(Container.DataItem,"FeedId") %>'
                                                                                                CommandName="delMe" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                <AlternatingRowStyle CssClass="altrow" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </tbody>
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
            </table>
        </div>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
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
