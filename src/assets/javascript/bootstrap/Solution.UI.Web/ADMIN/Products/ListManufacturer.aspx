<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ListManufacturer.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ListManufacturer"
    Theme="Gray" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="/Admin/Products/Manufacturer.aspx">
                    <img alt="Add Manufacture" title="Add Manufacture" src="/App_Themes/<%=Page.Theme %>/images/add-manufacture.png" /></a></span>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App/images/spacer.gif" width="1" height="5">
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
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Manufacturer" alt="Add Manufacturer" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                    <h2>
                                                        Manufactures List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td align="left">
                                                            Store :
                                                            <asp:DropDownList ID="ddlStore" runat="server" Width="180px" CssClass="order-list"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            Search :
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:ImageButton ID="ibtnsearch" runat="server" OnClientClick="return validation();"
                                                                ImageUrl="" OnClick="ibtnsearch_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="gvManufacturer" AutoGenerateColumns="false" runat="server" GridLines="None"
                                                    Width="100%" CellPadding="2" CellSpacing="2" EmptyDataText="No Records Found."
                                                    RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" AllowPaging="True"
                                                    PageSize="15" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" OnPageIndexChanging="gvManufacturer_PageIndexChanging"
                                                    OnRowDataBound="gvManufacturer_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                <asp:HiddenField ID="hdnManid" runat="server" Value='<%#Eval("ManufactureID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Manufacturer  Name" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="230px" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hplEdit" runat="server" NavigateUrl='<%#"Manufacturer.aspx?Mode=edit&ID="+ DataBinder.Eval(Container.DataItem, "ManufactureID") +"&storeId="+ DataBinder.Eval(Container.DataItem, "StoreID")%>'
                                                                    ImageUrl="/App_Themes/gray/Images/edit.png"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="altrow" id="trbottom" runat="server">
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;
                                                        padding-right: 10px;">
                                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl='/App_Themes/gray/Images/delet.gif'
                                                            OnClientClick="return checkCount();" OnClick="btnDelete_Click" />
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
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
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
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                alert('Enter Manufacture Name');
                document.getElementById('<%=txtSearch.ClientID %>').focus();
                return false;
            }
            return true;
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
                alert('Select at least one record.');
                return false;
            }

        }
    </script>
</asp:Content>
