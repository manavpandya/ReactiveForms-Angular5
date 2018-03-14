<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SearchOld.aspx.cs" Inherits="Solution.UI.Web.SearchOld" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script src="../js/SearchValidation.js" type="text/javascript"></script>
    <style type="text/css">
        .slidingDiv
        {
            height: 300px;
            padding: 20px;
            margin-top: 10px;
        }
        .show_hide
        {
            display: block;
        }
    </style>
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a>> <span>
            <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></span></div>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td align="left">
                                <table width="100%" border="0" cellpadding="5" cellspacing="0" class="table-none-Search"
                                    style="border: none;">
                                    <tbody>
                                        <tr>
                                            <td align="right" valign="top" style="width: 27%">
                                                Search
                                            </td>
                                            <td align="left" valign="top">
                                                :
                                            </td>
                                            <td align="left" style="width: 80%">
                                                <asp:Panel ID="pnlAdvanceSearch" runat="server" DefaultButton="imgButtonSearch">
                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="advance-teaxtfild" TabIndex="0"></asp:TextBox>
                                                    <a title="GO" href="#">
                                                        <asp:ImageButton ID="imgButtonSearch" ValidationGroup="Search" runat="server" align="top"
                                                            ToolTip="Go" ImageUrl="/images/go.png" OnClientClick="return CheckSearchValidation();"
                                                            OnClick="imgButtonSearch_Click" /></a> &nbsp; <a href="javascript:void(0);" class="show_hideImage"
                                                                onclick="ShowHideDiv(document.getElementById('ContentPlaceHolder1_hdnsearch').value);">
                                                                <%--<asp:LinkButton ID="hlAdvancedSearch" Style="color: #317099;" runat="server" ToolTip="Advanced Search"
                                                        Text="Advanced Search" TabIndex="1"></asp:LinkButton>--%>
                                                                <asp:Label ID="lblAdvancedSearch" Style="color: #141414;" runat="server"></asp:Label>
                                                                <input type="hidden" id="hdnsearch" runat="server" value="0" />
                                                            </a>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdImages" colspan="3" align="left">
                                                <div id="divImage" class="slidingDivImage">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" align="center" class="table-none"
                                                        style="border: none;">
                                                        <tr>
                                                            <td align="right">
                                                                Category
                                                            </td>
                                                            <td align="left">
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select-box" DataTextField="Name"
                                                                    DataValueField="CategoryID" AutoPostBack="false" Width="400px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                Search&nbsp;within&nbsp;Description
                                                            </td>
                                                            <td align="left">
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:RadioButtonList ID="rdoDescription" runat="server" RepeatDirection="Horizontal"
                                                                    Style="width: 10%;">
                                                                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                Show Image
                                                            </td>
                                                            <td align="left">
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                <asp:RadioButtonList ID="rdoPics" runat="server" RepeatDirection="Horizontal" Style="width: 10%;">
                                                                    <asp:ListItem Text="Yes" Value="true" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="Label1" runat="server" Text="Price From"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                :
                                                            </td>
                                                            <td align="left">
                                                                $
                                                                <asp:TextBox ID="txtSalePriceFrm" runat="server" CssClass="advance-quantity" MaxLength="7"
                                                                    Width="60px" onkeypress="return onKeyPressBlockNumbers(event);"></asp:TextBox>
                                                                <asp:Label ID="lblSalePriceTo" runat="server" Text="Price To  :"></asp:Label>
                                                                &nbsp;$&nbsp;<asp:TextBox ID="txtSalePriceTo" CssClass="advance-quantity" MaxLength="7"
                                                                    Width="60px" runat="server" onkeypress="return onKeyPressBlockNumbers(event);"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="advance-view-all" id="DivTop" runat="server" style="width: 99%;">
                    <table cellpadding="0" cellspacing="0" align="right">
                        <tr>
                            <td>
                                <asp:Label Style="color: #000000;" ID="Label2" runat="server" Text="Page(s)"></asp:Label>
                                &nbsp;:
                                <asp:LinkButton ID="lnkTopPrevious" runat="server" Style="color: #000000" OnClick="lnkPrevious_Click"
                                    Text="Previous |"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:DataList ID="dtlTopPaging" HorizontalAlign="Right" runat="server" RepeatDirection="Horizontal"
                                    OnItemCommand="RepeaterPaging_ItemCommand" OnItemDataBound="RepeaterPaging_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Pagingbtn" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                            CommandName="newpage" Text='<%# Eval("PageText") %> '></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td>
                                &nbsp;<asp:LinkButton ID="lnkTopNext" runat="server" Style="color: #000000" OnClick="lnkNext_Click"
                                    Text="| Next"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="advance-product-box">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table_none"
                        style="margin-top: 30px; margin-bottom: 5px;">
                        <tbody>
                            <tr id="trItemsCount" runat="server" visible="false">
                                <td align="right" style="padding-top: 5px; padding-right: 10px; font-size: 12px;
                                    color: #000000;">
                                    <asp:Label ID="lblItems" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                                        class="table" Style="border: solid 1px #e7e7e7" OnRowDataBound="grdProduct_RowDataBound">
                                        <EmptyDataRowStyle HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            <label style="color: Red">
                                                No product(s) found ...</label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                     <asp:Label ID="lblImageName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ImageName") %>'></asp:Label>
                                                     <asp:Label ID="lblTagName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TagName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Image">
                                                <HeaderTemplate>
                                                    <asp:Literal ID="ltShowImage" runat="server" Text="Image" Visible="true"></asp:Literal>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--   <a style="color: #141414;" href="/<%#DataBinder.Eval(Container.DataItem,"MainCategory") %>/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"SEName"))%>-<%#DataBinder.Eval(Container.DataItem,"ProductID")%>.aspx">--%>
                                                    <a class="pro-search" style="color: #141414;" href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>">
                                                        <img title="<%# DataBinder.Eval(Container.DataItem,"SEName")%>"  alt="<%# DataBinder.Eval(Container.DataItem,"SEName")%>"
                                                            src='<%# GetMicroImage(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName"))) %>' /><asp:Literal ID="lblTagImage" runat="server"></asp:Literal></a>
                                                    
                                                </ItemTemplate>
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle VerticalAlign="top" HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <HeaderTemplate>
                                                    Name
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%-- <a style="float: left; text-decoration: none; color: #141414;" href="/<%#DataBinder.Eval(Container.DataItem,"MainCategory") %>/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"SEName"))%>-<%#DataBinder.Eval(Container.DataItem,"ProductID")%>.aspx">--%>
                                                    <a style="float: left; text-decoration: none; color: #141414;" href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>">
                                                        <%#SetName(DataBinder.Eval(Container.DataItem,"PName").ToString()) %>
                                                    </a>
                                                </ItemTemplate>
                                                <HeaderStyle Width="50%" HorizontalAlign="left" />
                                                <ItemStyle VerticalAlign="top" HorizontalAlign="left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price">
                                                <HeaderTemplate>
                                                    Price
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    $<asp:Label ID="lblSalePrice" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SalePrice") %>'></asp:Label>
                                                    <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"Price") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="10%" HorizontalAlign="right" />
                                                <ItemStyle HorizontalAlign="right" VerticalAlign="top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SKU">
                                                <HeaderTemplate>
                                                    SKU
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%-- <a style="text-decoration: none;" href="/<%#DataBinder.Eval(Container.DataItem,"MainCategory") %>/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"SEName"))%>-<%#DataBinder.Eval(Container.DataItem,"ProductID")%>.aspx ">--%>
                                                    <a style="text-decoration: none;" href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>">
                                                        <asp:Label ID="lblId" runat="server" Text='<%#SetSKU(DataBinder.Eval(Container.DataItem,"SKU").ToString()) %>'></asp:Label></a>
                                                </ItemTemplate>
                                                <HeaderStyle Width="13%" />
                                                <ItemStyle VerticalAlign="top" HorizontalAlign="center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="advance-view-all" id="DivBottom" runat="server" style="width: 100%;">
                    <table cellpadding="0" cellspacing="0" align="right">
                        <tr>
                            <td>
                                <asp:Label Style="color: #000000;" ID="lblPagea" runat="server" Text="Page(s)"></asp:Label>
                                &nbsp;:
                                <asp:LinkButton ID="lnkbottomprevious" runat="server" Style="color: #000000" OnClick="lnkPrevious_Click"
                                    Text="Previous |"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:DataList ID="RepeaterPaging" HorizontalAlign="Right" runat="server" RepeatDirection="Horizontal"
                                    OnItemCommand="RepeaterPaging_ItemCommand" OnItemDataBound="RepeaterPaging_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Pagingbtn" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                            CommandName="newpage" Text='<%# Eval("PageText") %> '></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkbottomNext" runat="server" Style="color: #000000" OnClick="lnkNext_Click"
                                    Text="| Next"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="advance-product-box" id="divCategoryList" runat="server" visible="false">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table" style="margin-top: 30px;
                        margin-bottom: 10px;">
                        <tr>
                            <td align="left" style="padding: 2px 2px;">
                                <div style="background: none repeat scroll 0 0 #F1F1F1; border: 1px solid #e7e7e7;
                                    color: #000000; font-size: 12px; font-weight: bold; padding: 2px 2px;" class="advance-list"
                                    id="divListCat" runat="server">
                                    List of Categories
                                    <div style="padding-right: 2px; float: right;">
                                        <asp:Label ID="lblCategoryTotal" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td id="tblCategory" runat="server" align="left">
                                <div class="tab-advance-list" style="padding-top: 10px;">
                                    <asp:Repeater ID="rptCategory" runat="server" OnItemDataBound="rptCategory_ItemDataBound">
                                        <ItemTemplate>
                                            <div style="margin-bottom: 5px; min-height: 25px; width: 500px; text-align: left;
                                                font-size: 13px;">
                                                <asp:Label ID="lblCategory" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"CategoryId")%>'
                                                    runat="server"></asp:Label>
                                                <asp:Label ID="lblSeName" Text='<%# DataBinder.Eval(Container.DataItem,"SEName")%>'
                                                    Visible="false" runat="server"></asp:Label>
                                                &nbsp;<a style="color: #141414;" id="tagsename" runat="server">
                                                    <%#DataBinder.Eval(Container.DataItem,"Name") %>
                                                </a>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
