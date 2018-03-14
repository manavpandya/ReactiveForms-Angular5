<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductFabricGroupTypeList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductFabricGroupTypeList"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
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
                $(document).ready(function () { jAlert('Check at least One Product FabricGroupType!', 'Message', ''); });
                return false;
            }
            else {
                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Product FabricGroupType?', 'Confirmation', function (r) {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <span style="vertical-align: middle; margin-right: 3px; float: right;">
                    <a href="AddProductFabricGroupType.aspx">
                        <img alt="Add New Fabric Group Type" title="Add New Fabric Group Type" src="/App_Themes/<%=Page.Theme %>/images/add-new-product-fabric-group-type.png" />
                    </a>
                </span>
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
                                                    <img class="img-left" title="Gift Card Paroduct List" alt="Gift Card ParoductList"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/tax-class-list-icon.png" />
                                                    <h2>Product Fabric Group Type List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 65%"></td>
                                                        <td style="width: 10%" align="right">Search :
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list">
                                                                <asp:ListItem Value="FabricGroupName">Fabric Group Name</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:ImageButton ID="ibtnsearch" runat="server" OnClientClick="return validation();"
                                                                OnClick="ibtnsearch_Click" />
                                                        </td>
                                                        <td style="width: 5%; padding-right: 0px;">
                                                            <asp:ImageButton ID="ibtnShowall" runat="server" CommandName="ShowAll" OnClick="ibtnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:UpdatePanel ID="up1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvProductSize" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                            BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7" CellSpacing="1" Width="100%"
                                                            EmptyDataText="No Records Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                                                            PagerSettings-Mode="NumericFirstLast" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                            AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                            ViewStateMode="Enabled" OnRowCommand="gvProductSize_RowCommand" OnRowDataBound="gvProductSize_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFabricGroupTypeID" runat="server" Text='<%#Eval("FabricGroupTypeID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Fabric Group Name" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        Fabric Group Name
                                                                        <asp:ImageButton ID="lbProductName" runat="server" CommandArgument="DESC" CommandName="Name"
                                                                            AlternateText="Ascending Order" OnClick="Sorting" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("FabricGroupName")%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        Display Order
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%#Eval("DisplayOrderBy")%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:TemplateField HeaderText="Edit Fabric Group Type" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                            CommandArgument='<%# Eval("FabricGroupTypeID") %>'></asp:ImageButton>
                                                                        <asp:ImageButton ID="btnSave" runat="server" CommandArgument='<%#Eval("FabricGroupTypeID") %>'
                                                                            CommandName="Add" Visible="False" ImageUrl="~/Admin/images/save_icon.jpg" ValidationGroup="UpdatePrice" />
                                                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Stop" ImageUrl="~/Admin/images/cancel.jpg"
                                                                            Visible="False" CommandArgument='<%# Eval("FabricGroupTypeID") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                            <AlternatingRowStyle CssClass="altrow" />
                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvProductSize" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <div id="Productdata" runat="server">
                                                    <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                        href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right; padding-right: 0px;">
                                                            <asp:Button ID="btnDelete" runat="server" OnClientClick="return checkCount();" OnClick="btnDelete_Click" />
                                                            <div style="display: none">
                                                                <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDelete_Click" />
                                                            </div>
                                                        </span>
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="10" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
