<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="QuantityDiscountList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.QuantityDiscountList1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <asp:ScriptManager ID="SM1" runat="server">
        </asp:ScriptManager>
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <span style="vertical-align: middle; margin-right: 3px; margin-top: 4px; float: left;">
                    <table>
                        <tr>
                            <td style="width: 25%" align="left">
                                Store :
                                <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/Promotions/QuantityDiscount.aspx">
                    <img alt="Add Quantity Discount" title="Add Quantity Discount" src="/App_Themes/<%=Page.Theme %>/images/add-quantity-discount.png" /></a></span>
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
                                                    <img class="img-left" title="Quantity Discount List" alt="Quantity Discount List"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/quantity-discount-icon.png" />
                                                    <h2>
                                                        Quantity Discount</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 70%">
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:ImageButton ID="ibtnsearch" runat="server" OnClientClick="return validation();"
                                                                OnClick="btnGo_Click" />
                                                        </td>
                                                        <td style="width: 5%; padding-right: 0px;">
                                                            <asp:ImageButton ID="ibtnShowall" runat="server" OnClick="btnSearchall_Click" CommandName="ShowAll" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row" id="gvdiscount">
                                            <td>
                                                <center>
                                                    <asp:Label ID="lblMsg" runat="server" CssClass="font-red"></asp:Label></center>
                                                <asp:ObjectDataSource runat="server" ID="odsQuantityDiscount" SelectMethod="GetDataByFilter"
                                                    StartRowIndexParameterName="startIndex" SortParameterName="sortBy" MaximumRowsParameterName="pageSize"
                                                    TypeName="Solution.Bussines.Components.QuantityDiscountComponent" EnablePaging="true"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="hdnState" DbType="String" DefaultValue="TaxClassId"
                                                            Name="CName" />
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:HiddenField ID="hdnState" runat="server" />
                                                <asp:GridView ID="gvQuantityDiscount" runat="server" AutoGenerateColumns="false"
                                                    GridLines="None" Width="100%" BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7"
                                                    CellSpacing="1" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                    HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True" DataSourceID="odsQuantityDiscount"
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" OnRowDataBound="gvQuantityDiscount_RowDataBound" OnRowCommand="gvQuantityDiscount_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="15%">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkselect" runat="server" />
                                                                <asp:HiddenField ID="hdnQuantityDiscountID" runat="server" Value='<%#Eval("QuantityDiscountID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Discount Table Name" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="220px" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                Quantity Name
                                                                <asp:ImageButton ID="lbQuantityDiscount" runat="server" CommandArgument="DESC" CommandName="Name"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" Text='<%# Eval("Name")%>' runat="server"></asp:Label>
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="order-textfield" Text='<%# Eval("Name")%>'
                                                                    Width="150px" Visible="false" MaxLength="100"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Store Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                Store Name
                                                                <asp:ImageButton ID="lbstname" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                    OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#Eval("StoreName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit Quantity Discount" ItemStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton1" ImageUrl="~/App_Themes/Gray/images/Edit.gif"
                                                                    ToolTip="Edit" CommandName="Select" CommandArgument='<%# Eval("QuantityDiscountID") %>'>
                                                                </asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ImageUrl="~/App_Themes/Gray/images/Edit.gif"
                                                                    ToolTip="Edit" CommandName="EditQuantityDiscount" CommandArgument='<%# Eval("QuantityDiscountID") %>'>
                                                                </asp:ImageButton>
                                                                <asp:ImageButton ID="btnSave" runat="server" CommandArgument='<%# Eval("QuantityDiscountID") %>'
                                                                    ValidationGroup="" CommandName="Add" Visible="false" ImageUrl="/App_Themes/gray/Images/save.png" />
                                                                <asp:ImageButton ID="btnCancel" runat="server" CommandName="Stop" ImageUrl="~/Admin/images/cancel.jpg"
                                                                    Visible="false" CommandArgument='<%# Eval("QuantityDiscountID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table width="100%">
                                                    <tr id="trBottom" runat="server">
                                                        <td>
                                                            <%--    <span><a href="javascript:selectAll(true);">Check All</a> &nbsp;|&nbsp; <a href="javascript:selectAll(false);">
                                                                Clear All</a></span>--%>
                                                        </td>
                                                        <td align="right" style="width: 10%; padding-right: 0px;">
                                                            <asp:Button runat="server" ID="btndelete" ToolTip="Delete" OnClientClick="return chkselect();"
                                                                OnClick="btndelete_Click" />
                                                            <div style="display: none">
                                                                <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btndelete_Click" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row" id="TRUpdateQtyDiscount" runat="server" visible="false">
                                            <td>
                                                <asp:GridView ID="gvListQuantityDiscount" runat="server" AutoGenerateColumns="false"
                                                    GridLines="None" Width="100%" BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7"
                                                    CellSpacing="1" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                    HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound="gvListQuantityDiscount_RowDataBound"
                                                    ViewStateMode="Enabled" ShowFooter="True" OnRowCommand="gvListQuantityDiscount_RowCommand">
                                                    <FooterStyle CssClass="product_table" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantityDiscountTableID" runat="server" Text='<%#Eval("QuantityDiscountTableID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="15%">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkselect" runat="server" />
                                                                <asp:HiddenField ID="hdnQuantityDiscountID" runat="server" Value='<%#Eval("QuantityDiscountID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Low Quantity" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                Low Quantity
                                                                <asp:ImageButton ID="lbLowQuantity" runat="server" CommandArgument="DESC" CommandName="LowQuantity"
                                                                    OnClick="Sortingnew" Visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLowQuantity" runat="server" Text='<%#Eval("LowQuantity")%>'></asp:Label>
                                                                <asp:TextBox ID="txtAddLowQuantity" runat="server" Text='<%#Eval("LowQuantity")%>'
                                                                    CssClass="order-textfield" Visible="False" Width="40px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddLowQuantity"
                                                                    SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="Enter LowQuantity"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAddLowQuantity"
                                                                    SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="InValid"
                                                                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                &nbsp;<asp:TextBox ID="txtNewLowQuantity" runat="server" ValidationGroup="AddNew"
                                                                    CssClass="order-textfield" Visible="False" Width="50px" onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewLowQuantity"
                                                                        SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="Enter LowQuantity"
                                                                        ValidationGroup="AddNew"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNewLowQuantity"
                                                                            SetFocusOnError="true" Display="dynamic" CssClass="error" ValidationGroup="AddNew"
                                                                            ErrorMessage="InValid" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="High Quantity" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px"
                                                            HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                High Quantity
                                                                <asp:ImageButton ID="lbHighQuantity" runat="server" CommandArgument="DESC" CommandName="HighQuantity"
                                                                    OnClick="Sortingnew" Visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHighQuantity" runat="server" Text=' <%#Eval("HighQuantity")%>'></asp:Label>
                                                                <asp:TextBox ID="txtAddHighQuantity" runat="server" Text=' <%#Eval("HighQuantity")%>'
                                                                    CssClass="order-textfield" Visible="False" Width="40px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddHighQuantity"
                                                                    SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="Enter HighQuantity"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtAddHighQuantity"
                                                                    SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="InValid"
                                                                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ValidationGroup="AddNew" ID="txtNewHighQuantity" runat="server" CssClass="order-textfield"
                                                                    Visible="False" Width="50px" onkeypress="return isNumberKey(event)" MaxLength="5"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewHighQuantity"
                                                                        CssClass="error" ErrorMessage="Enter HighQuantity" SetFocusOnError="true" Display="dynamic"
                                                                        ValidationGroup="AddNew"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtNewHighQuantity"
                                                                            SetFocusOnError="true" Display="dynamic" ValidationGroup="AddNew" CssClass="error"
                                                                            ErrorMessage="InValid" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Discount Percent" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                Discount Percent
                                                                <asp:ImageButton ID="lbDiscount" runat="server" CommandArgument="DESC" CommandName="DiscountPercent"
                                                                    OnClick="Sortingnew" Visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# Math.Round(Convert.ToDecimal(Eval("DiscountPercent")), 2)%>'></asp:Label>
                                                                <asp:TextBox ID="txtAddDiscount" runat="server" Text='<%# Math.Round(Convert.ToDecimal(Eval("DiscountPercent")), 2)%>'
                                                                    CssClass="order-textfield" Visible="False" Width="40px" onkeypress="return  inputOnlyNumbers(event)"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddDiscount"
                                                                    SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="Enter Discount"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAddDiscount"
                                                                    SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="Invalid"
                                                                    ValidationExpression="^(100(?:\.0{1,2})?|0*?\.\d{1,2}|\d{1,2}(?:\.\d{1,2})?)$"></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtNewDiscount" runat="server" CssClass="order-textfield" Visible="False"
                                                                    Width="50px" ValidationGroup="AddNew" onkeypress="return  inputOnlyNumbers(event)" MaxLength="5"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNewDiscount"
                                                                    SetFocusOnError="true" Display="dynamic" CssClass="error" ValidationGroup="AddNew"
                                                                    ErrorMessage="Enter Discount"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                        ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtNewDiscount"
                                                                        SetFocusOnError="true" Display="dynamic" CssClass="error" ErrorMessage="Invalid"
                                                                        ValidationGroup="AddNew" ValidationExpression="^(100(?:\.0{1,2})?|0*?\.\d{1,2}|\d{1,2}(?:\.\d{1,2})?)$"></asp:RegularExpressionValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Created On" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Eval("CreatedOn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="70px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ImageUrl="~/App_Themes/Gray/images/Edit.gif"
                                                                    ToolTip="Edit" CommandName="Select" CommandArgument='<%# Eval("QuantityDiscountTableID") %>'>
                                                                </asp:ImageButton>
                                                                <asp:ImageButton ID="btnSave" runat="server" CommandArgument='<%# Eval("QuantityDiscountTableID") %>'
                                                                    ValidationGroup="" CommandName="Add" Visible="false" ImageUrl="~/App_Themes/Gray/images/save.png" />
                                                                <asp:ImageButton ID="btnCancel" runat="server" CommandName="Stop" ImageUrl="~/Admin/images/cancel.jpg"
                                                                    Visible="false" CommandArgument='<%# Eval("QuantityDiscountTableID") %>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:ImageButton ID="btnSave" ValidationGroup="AddNew" runat="server" CommandArgument='<%# Eval("QuantityDiscountID") %>'
                                                                    CommandName="Footer Add" Visible="False" ImageUrl="~/App_Themes/Gray/images/save.png"/>
                                                                <asp:ImageButton ID="btnCancel" runat="server" CommandName="Footer Stop" ImageUrl="~/Admin/images/cancel.jpg"
                                                                    Visible="False" />
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="altrow" id="TRUpdateQtyDiscount1" runat="server" visible="false">
                                            <td>
                                                <table width="100%">
                                                    <tr id="trLastBottom" runat="server">
                                                        <td>
                                                            <%--    <span><a href="javascript:selectAll(true);">Check All</a> &nbsp;|&nbsp; <a href="javascript:selectAll(false);">
                                                                Clear All</a></span>--%>
                                                        </td>
                                                        <td align="right" style="width: 27%; padding-right: 0px;">
                                                            <%-- <asp:ImageButton ID="btnAddNew" runat="server" OnCommand="AddNewDiscountRowCommand" />--%>
                                                            <asp:Button ID="btnAddNew" runat="server" OnCommand="AddNewDiscountRowCommand" />
                                                            <asp:Button runat="server" ID="btndeleteQuantitytable" ToolTip="Delete" OnClientClick="return chkselect_updategrid()"
                                                                OnClick="btndeleteQuantitytable_Click" />
                                                            <div style="display: none">
                                                                <asp:Button ID="btndeleteQuantitytableTemp" runat="server" ToolTip="Delete" OnClick="btndeleteQuantitytable_Click" />
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
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="10" />
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
                $(document).ready(function () {
                    jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>');
                });
                return false;
            }
            return true;
        }
        function inputOnlyNumbers(evt) {
            var e = window.event || evt;
            var charCode = e.which || e.keyCode;
            if (charCode < 31 || (charCode > 47 && charCode < 58) || charCode == 46) {
                return true;
            }
            return false;
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            else
                return true;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function chkselect() {

            var allElts = document.getElementById('gvdiscount').getElementsByTagName('INPUT');
            var i;
            var Chktrue1;
            Chktrue1 = 0;

            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue1 = Chktrue1 + 1;
                    }
                }
            }
            if (Chktrue1 < 1) {
                $(document).ready(function () { jAlert('Check at least One Quantity Name!', 'Message'); });
                return false;
            }
            else {
                return checkaa();
            }
            return true;

        }

        function checkaa() {
            jConfirm('Are you sure want to delete all selected Quantity Name?', 'Confirmation', function (r) {
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
    <script type="text/javascript">
        function chkselect_updategrid() {

            //var allElts = document.forms['aspnetForm'].elements;
            var allElts = document.getElementById('ContentPlaceHolder1_TRUpdateQtyDiscount').getElementsByTagName('INPUT');
            var i;
            var Chktrue1;
            Chktrue1 = 0;

            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue1 = Chktrue1 + 1;
                    }
                }
            }
            if (Chktrue1 < 1) {
                $(document).ready(function () { jAlert('Check at least One Quantity Discount!', 'Message'); });
                return false;
            }
            else {
                //    checkaanew();
                return confirm('Are you sure to delete selected Quantity Discount?');
            }
            return true;

        }
        function checkaanew() {
            jConfirm('Are you sure want to delete all selected Quantity Name?', 'Confirmation', function () {
                if (r == true) {

                    document.getElementById('ContentPlaceHolder1_btndeleteQuantitytableTemp').click();
                    return false;
                }
                else {

                    return false;
                }
            });
            return false;
        }
                                            
    </script>
</asp:Content>
