<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function chkselectcheckboxInv() {
            var allElts = document.getElementById('ContentPlaceHolder1_grdProduct').getElementsByTagName('input')
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {

                jAlert('Select at least one Product !', 'Message');
                return false;

            }
            else {
                return chkallinvnetoryupdate();
            }

            return true;

        }







        function chkallinvnetoryupdate() {
            jConfirm('Are you sure want to update inventory  ?', 'Confirmation', function (r) {
                if (r == true) {
                    chkHeight();
                    UpdateInv();
                    return true;
                }
                else {

                    return false;
                }

            });
            return false;
        }





        function UpdateInv() {
            document.getElementById('ContentPlaceHolder1_btnRefresh').click();
        }
    </script>

    <script type="text/javascript">
        function OpenClonePopup(StoreId, Id) {
            var popupurl = "ProductCloneOptions.aspx?StoreId=" + StoreId + "&Id=" + Id + "&Mode=clone";
            window.open(popupurl, "Productclone", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=450,height=150,left=250,top=350");
        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function openCenteredCrossSaleWindow2(mode) {
            // var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var StoreID = 1;
            if (StoreID.value != "0") {
                var width = 800;
                var height = 500;
                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));

                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('PrintProductBarcode.aspx', "Mywindow", windowFeatures);
                return false;
            }
            else {
                jAlert('Select Store!', 'Message', StoreID);
                return false;
            }
        }

    </script>
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 5px; float: left">
                        <tr>
                            <td align="left">Store :
                                <asp:DropDownList ID="ddlStore" runat="server" Width="185px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list"
                                    Style="margin-left: 0px" onchange="javascript:chkHeight();">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    id="ahAddProduct" runat="server">
                    <img alt="Add Product" title="Add Product" src="/App_Themes/<%=Page.Theme %>/images/add-product.png" /></a></span>
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
                                                    <img class="img-left" title="Product List" alt="Product List" src="/App_Themes/<%=Page.Theme %>/Images/list-product-icon.png" />
                                                    <h2>Product List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;" colspan="3">Search By
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td style="padding-top: 0px; padding-bottom: 0px;">
                                                <table style="width: 100%; text-align: right">
                                                    <tr>
                                                        <td align="left" style="width: 2%">Stock&nbsp;/&nbsp;Dropship&nbsp;:
                                                        </td>
                                                        <td align="left" style="width: 2%">
                                                            <asp:DropDownList ID="ddlProductTypeDelivery" Width="210px" AutoPostBack="true" runat="server"
                                                                class="order-list" OnSelectedIndexChanged="ddlProductTypeDelivery_SelectedIndexChanged"
                                                                onchange="javascript:chkHeight();">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" style="width: 2%">Status&nbsp;:
                                                        </td>
                                                        <td align="left" style="width: 2%">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="120px" AutoPostBack="True"
                                                                CssClass="order-list" onchange="javascript:chkHeight();">
                                                                <asp:ListItem>Active</asp:ListItem>
                                                                <asp:ListItem>InActive</asp:ListItem>
                                                                <asp:ListItem>Deleted</asp:ListItem>
                                                                <asp:ListItem>AdminOnly</asp:ListItem>
                                                                <asp:ListItem>DataVerify</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right" width="64%" valign="middle">Search&nbsp;:
                                                        </td>
                                                        <td style="width: 2%">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" Width="115px" AutoPostBack="False"
                                                                CssClass="order-list">
                                                                <asp:ListItem Value="UPC">UPC</asp:ListItem>
                                                                <asp:ListItem Value="SKU" Selected="True">SKU</asp:ListItem>
                                                                <asp:ListItem Value="Name">Product Name</asp:ListItem>
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
        
          <asp:Button ID="btnRefresh" runat="server" Text="Refresh Inventory" OnClick="btnRefresh_Click"  />
    </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 2%">Product&nbsp;Type&nbsp;:
                                                        </td>
                                                        <td align="left" style="width: 2%">
                                                            <asp:DropDownList ID="ddlProductType" runat="server" Width="210px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged" CssClass="order-list"
                                                                onchange="javascript:chkHeight();">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">Category :
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlCategory" runat="server" Width="210px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" CssClass="order-list"
                                                                onchange="javascript:chkHeight();">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td colspan="5" align="right" style="padding-right: 4px">Total Products:
                                                            <% =Solution.Bussines.Components.ProductComponent._count%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;" colspan="3">&nbsp;Update Multiple Products
                                                <div style="float: right">
                                                    <table>
                                                        <tr>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">
                                                                <div style='float: left; width: 10px; height: 10px; background-color: green'>
                                                                </div>
                                                            </td>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">Complete
                                                            </td>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">
                                                                <div style='float: right; width: 10px; height: 10px; background-color: yellow'>
                                                                </div>
                                                            </td>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">Product Description or SEO Details Missing
                                                            </td>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">
                                                                <div style='float: right; width: 10px; height: 10px; background-color: red'>
                                                                </div>
                                                            </td>
                                                            <td style="background-color: #E3E3E3; font-weight: normal">Dropshipper SKU or Assembly Items Missing
                                                            </td>
                                                            <%--  <td style="background-color: #E3E3E3; font-weight: normal">
                                                              <span style="font-size:24px;vertical-align:middle;color:Red" title="Low Level">•</span>
                                                                Low Level Inventory
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr class="altrow" id="trMultipleUpdates" runat="server">
                                            <td width="100%" style="padding-top: 0px; padding-bottom: 0px; padding-right: 0px;">
                                                <table width="100%" style="float: right; text-align: right">
                                                    <tr id="trMsg" runat="server" align="right">
                                                        <td style="float: right" colspan="2">
                                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="display: none;">Multiple Updates:
                                                        </td>
                                                        <td width="10%" style="display: none;">
                                                            <asp:DropDownList ID="ddlFieldName" AutoPostBack="False" runat="server" Width="100%"
                                                                CssClass="order-list">
                                                                <%-- <asp:ListItem>Inventory</asp:ListItem>--%>
                                                                <asp:ListItem>Price</asp:ListItem>
                                                                <asp:ListItem>SalePrice</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="10%" style="display: none;">
                                                            <asp:DropDownList ID="ddlOperation" AutoPostBack="False" Width="100%" runat="server"
                                                                CssClass="order-list">
                                                                <asp:ListItem>Add</asp:ListItem>
                                                                <asp:ListItem>Exact</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="10%" style="display: none;">
                                                            <asp:DropDownList ID="ddlValue" AutoPostBack="False" Width="100%" runat="server"
                                                                CssClass="order-list">
                                                                <asp:ListItem>Value</asp:ListItem>
                                                                <asp:ListItem>Percentage</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="10%" style="display: none;">
                                                            <asp:TextBox ID="txtValue" CssClass="order-textfield" runat="server" Width="100%"
                                                                onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                        </td>
                                                        <td width="2%" style="display: none;">
                                                            <asp:Button ID="btnUpdateMultiple" runat="server" OnClientClick="return checkCount('Update');"
                                                                OnClick="btnUpdate_Click" Width="100%" />
                                                        </td>
                                                        <td width="7%" align="right">
                                                            <asp:ImageButton ID="btnPrintBarcode" runat="server" Text="Print Barcode" OnClientClick="return openCenteredCrossSaleWindow2(1);" />
                                                        </td>
                                                        <td align="right" id="tdExportImport">
                                                            <asp:Button ID="btnecommexportprice" runat="server" OnClick="btnecommexportprice_Click"  CausesValidation="False"/>
                                                            <asp:Button ID="btnExportPrice" runat="server" OnClick="btnExportPrice_Click"
                                                                CausesValidation="False" />
                                                            <asp:Label ID="lblImportCSv" runat="server" Text="Import CSV :"></asp:Label>

                                                            <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />

                                                            <asp:LinkButton ID="btnUpload" AlternateText="Upload" ImageAlign="Top"
                                                                runat="server" Visible="false">Upload</asp:LinkButton>

                                                            <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" ValidationGroup="importfile" CausesValidation="true" />


                                                            <asp:RegularExpressionValidator ID="revImage" ControlToValidate="uploadCSV" ValidationExpression="(.*?)\.(csv)$"
                                                                Text="Select csv File Only (Ex.: .csv)" runat="server"
                                                                ValidationGroup="importfile" Display="Dynamic" ForeColor="Red" />
                                                            <asp:RequiredFieldValidator ID="reqfile" runat="server" Text="Please select.csv file" ValidationGroup="importfile" ControlToValidate="uploadCSV" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnExport" Style="width: 197px; height: 23px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server" CssClass="Export" OnClick="btnExport_Click" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="display: none">
                                                                            <asp:Button ID="btnOverStockProduct" Style="width: 125px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                                runat="server" Text="" CssClass="" OnClick="btnOverStockProduct_Click"
                                                                                OnClientClick="return chkselectcheckbox();" /></span>
                                                                        <asp:Button ID="btnAmazonProduct" Style="background: url(/App_Themes/Gray/update_amazon_product.jpg) no-repeat; width: 125px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server"
                                                                            Text="" CssClass="" OnClick="btnAmazonProduct_Click" OnClientClick="return chkselectcheckbox();" />
                                                                        <asp:Button ID="btneBayProduct" Style="width: 125px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server" Text="" CssClass="" Visible="false" OnClientClick="return chkselectcheckbox();"
                                                                            OnClick="btneBayProduct_Click" />
                                                                        <asp:Button ID="btnSearsProduct" Style="width: 125px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server" Text="" CssClass="" Visible="false" OnClick="btnSearsProduct_Click"
                                                                            OnClientClick="return chkselectcheckbox();" />
                                                                    </td>
                                                                    <td align="right">

                                                                        <asp:Button ID="btnOverStockUpdate" Style="background: url(/App_Themes/Gray/update_amazon_product.jpg) no-repeat; width: 125px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server"
                                                                            Text="" CssClass="" Visible="false" OnClick="btnOverStockUpdate_Click" OnClientClick="return chkselectcheckbox();" />
                                                                        <asp:Button ID="btnOverStockAllUpdate" Style="background: url(/App_Themes/Gray/update_amazon_product.jpg) no-repeat; width: 125px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server"
                                                                            Text="" CssClass="" Visible="false" OnClick="btnOverStockAllUpdate_Click" OnClientClick="return chkallinvnetory();" />
                                                                        <asp:Button ID="btnAmozonUpdate" Style="background: url(/App_Themes/Gray/images/update_amazon_inventory.jpg) no-repeat; width: 186px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server"
                                                                            Text="" CssClass="" Visible="false" OnClick="btnUpdate1_Click" OnClientClick="return chkselectcheckbox();" />
                                                                        <asp:Button ID="btneBayUpdate" Style="background: width: 186px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server" Text="" CssClass="" Visible="false"
                                                                            OnClick="btneBayUpdate_Click" OnClientClick="return chkselectcheckbox();" />
                                                                        <asp:Button ID="btnSearsUpdate" Style="background: width: 186px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server" Text="" CssClass="" Visible="false"
                                                                            OnClick="btnSearsUpdate_Click" OnClientClick="return chkselectcheckbox();" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnAmazonPrice" Style="background: url(/App_Themes/Gray/images/update_amazon_price.png) no-repeat; width: 182px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server"
                                                                            Text="" CssClass="" Visible="false" OnClick="btnAmazonPrice_Click" OnClientClick="return chkselectcheckbox();" />
                                                                        <asp:Button ID="btnSearsPrice" Style="width: 182px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server" Text="" CssClass="" Visible="false" OnClick="btnSearsPrice_Click"
                                                                            OnClientClick="return chkselectcheckbox();" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnAmazonImage" Style="background: url(/App_Themes/Gray/uploadamazonimage.png) no-repeat; width: 125px; height: 22px; border: 0; cursor: pointer; font-size: 9px;"
                                                                            runat="server"
                                                                            Text="" CssClass="" OnClick="btnAmazonImage_Click" OnClientClick="return chkselectcheckbox();" />
                                                                        <asp:ImageButton ID="btnApprove" runat="server" OnClick="btnApprove_Click" Visible="false"
                                                                            OnClientClick="return checkCount('Approve');" />
                                                                        <div style="display: none">
                                                                            <asp:Button ID="btnApproveTemp" runat="server" ToolTip="Approve" OnClick="btnApproveTemp_Click" />
                                                                        </div>
                                                                        <asp:Button ID="btnCheckAll" runat="server" Text="" Width="190px" OnClick="btnCheckAll_Click" OnClientClick="javascript:chkHeight();" />

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td style="padding-top: 0px; padding-bottom: 0px;">
                                                <asp:ObjectDataSource runat="server" ID="odsProducts" SelectMethod="GetProductfilterData"
                                                    StartRowIndexParameterName="startIndex" SortParameterName="sortBy" MaximumRowsParameterName="pageSize"
                                                    SelectCountMethod="GetCount" TypeName="Solution.Bussines.Components.ProductComponent"
                                                    EnablePaging="true">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="StoreId" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="ddlProductType" DbType="Int32" Name="ProductTypeId"
                                                            DefaultValue="" />
                                                        <asp:ControlParameter ControlID="ddlProductTypeDelivery" DbType="Int32" Name="ProductTypeDeliveryId"
                                                            DefaultValue="" />
                                                        <asp:ControlParameter ControlID="ddlCategory" DbType="Int32" Name="CategoryId" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="ddlSearch" DbType="String" Name="SearchBy" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="SearchValue" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="ddlStatus" DbType="String" Name="status" DefaultValue="" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:GridView ID="grdProduct" AutoGenerateColumns="false" runat="server" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" DataKeyNames="ProductID" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                    HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" ShowHeaderWhenEmpty="true"
                                                    OnRowDataBound="grdProduct_RowDataBound" DataSourceID="odsProducts" OnRowCommand="grdProduct_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                                                <asp:HiddenField ID="hdnProductTypeID" runat="server" Value='<%#Eval("ProductTypeID") %>' />
                                                                <asp:HiddenField ID="hdnProductTypeDeliveryID" runat="server" Value='<%#Eval("ProductTypeDeliveryID") %>' />
                                                                <asp:HiddenField ID="hdnUPC" runat="server" Value='<%#Eval("UPC") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="4%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                Product Name
                                                                <asp:ImageButton ID="btnProductName" runat="server" CommandArgument="DESC" CommandName="Name"
                                                                    AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltStatusColor" runat="server"></asp:Literal>
                                                                <asp:Literal ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SKU" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="8%"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                SKU
                                                                <asp:ImageButton ID="btnSKU" runat="server" CommandArgument="DESC" CommandName="SKU"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" CausesValidation="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSKU" runat="server" Text='<%# Bind("SKU") %>'></asp:Label>
                                                                <asp:Label ID="lblOptionSku" runat="server" Text='<%# Bind("OptionSku") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Inventory" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Inventory
                                                                <asp:ImageButton ID="btnInventory" runat="server" CommandArgument="DESC" CommandName="Inventory"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" CausesValidation="false" /> &nbsp;<a href="jaascript:void(0);" onclick="return chkselectcheckboxInv();"><img src="/App_Themes/gray/icon/inv-refresh.png" style="width:10px;margin-left:5px;" alt="Refresh Inventory" title="Refresh Inventory" /></a>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 14px; padding-right: 0px;" align="right">
                                                                            <span style="font-size: 24px; vertical-align: middle; color: Red; cursor: default;"
                                                                                title="Low Level" id="ImgToggelInventory" runat="server" visible="false">•</span>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="width: 30px; padding-left: 0px;" align="left">
                                                                            <asp:Label ID="lblInventory" runat="server" Text='<%#Bind("Inventory") %>'></asp:Label>&nbsp;
                                                                            Sales&nbsp;Channel&nbsp;Qty:&nbsp;<asp:Label ID="lblhemming" runat="server" Text='0'></asp:Label>


                                                                        </td>
                                                                        <td style="width: 45px;" align="left">
                                                                            <a id="aeditclone<%# Container.DataItemIndex %>" href="javascript:void(0);" onclick="ShowModelUserRegisterPopup(<%#Eval("ProductID") %>,<%#Eval("StoreID") %>,<%# Container.DataItemIndex %>);">Edit</a>
                                                                            <asp:TextBox ID="txtInventory" runat="server" Text='<%#Bind("Inventory") %>' onkeypress="return keyRestrict(event,'0123456789');"
                                                                                Visible="False" Width="80%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Price($)
                                                                <asp:ImageButton ID="btnPrice" runat="server" CommandArgument="DESC" CommandName="Price"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" CausesValidation="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")),2) %>'
                                                                    ItemStyle-Width="8%"></asp:Label>
                                                                <asp:TextBox ID="txtPrice" runat="server" Visible="false" Width="80%" onkeypress="return keyRestrict(event,'0123456789');"
                                                                    Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")),2) %>'
                                                                    MaxLength="10"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sale price" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Sale Price($)
                                                                <asp:ImageButton ID="btnSalePrice" runat="server" CommandArgument="DESC" CommandName="SalePrice"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" CausesValidation="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSalePrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal( DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>'></asp:Label>
                                                                <asp:TextBox ID="txtSalePrice" runat="server" Visible="false" Width="80%" onkeypress="return keyRestrict(event,'0123456789');"
                                                                    Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>'
                                                                    MaxLength="10"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Store Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreName" runat="server" Text='<%# Bind("StoreName") %>'></asp:Label>
                                                                <br />
                                                                <asp:Literal ID="ltotherstore" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last Updated On" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblebayupdatedon" runat="server" Text='<%# Bind("eBayLastUpdated","{0:MM/dd/yyyy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="eBay Store CategoryID" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblebayStoreCatID" runat="server" Text='<%# Bind("eBayStoreCategoryID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="eBay CategoryID" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblebayCatID" runat="server" Text='<%# Bind("eBayCategoryID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Updated On" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblupdatedon" runat="server" Text='<%# Bind("UpdatedOn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="6%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Updated By" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUpdatedBy" runat="server" Text='<%# Bind("AdminName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Clone" ItemStyle-Width="5%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltclone" runat="Server"></asp:Literal>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit Price" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="btnEditPrice" ToolTip="Edit" CommandName="EditPrice"
                                                                    AlternateText="Edit Price" CommandArgument='<%# Eval("ProductID") %>'></asp:ImageButton>
                                                                <asp:ImageButton ID="btnSave" runat="server" Visible="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                                                    CommandName="Save" AlternateText="Save" />
                                                                <asp:ImageButton ID="btnCancel" runat="server" Visible="false" CommandName="Cancel"
                                                                    Height="16px" Width="16px" AlternateText="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreID" runat="Server" Text='<%# Eval("StoreID") %>' Visible="False"></asp:Label>
                                                                <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl='<%# "~/admin/products/Product.aspx?StoreID=" + DataBinder.Eval(Container.DataItem,"StoreID")+"&ID=" + DataBinder.Eval(Container.DataItem,"ProductID")+"&Mode=edit" %>'
                                                                    ToolTip='<%# "Edit " + DataBinder.Eval(Container.DataItem,"Name") +" for StoreID " + DataBinder.Eval(Container.DataItem,"StoreID")  %>'></asp:HyperLink>
                                                            </ItemTemplate>
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
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;">
                                                        <asp:Button ID="btnEditMultiplePrice" runat="server" OnClick="btnEditMultiplePrice_Click"
                                                            OnClientClick="return checkCount('EditPrice');" />
                                                        <asp:Button ID="btnSaveMultiple" runat="server" OnClick="btnSaveMultiple_Click" Visible="False"
                                                            OnClientClick="return checkCount('EditPrice');" />
                                                        <asp:Button ID="btnCancelMultiple" runat="server" OnClick="btnCancelMultiple_Click"
                                                            Visible="False" />
                                                        <asp:Button ID="btnEditMultipleProduct" runat="server" OnClick="btnEditMultipleProduct_Click"
                                                            OnClientClick="return checkCount('EditPrice');" />
                                                        <asp:Button ID="btnMultipleDelete" runat="server" OnClientClick="return checkCount('Delete');"
                                                            OnClick="btnDelete_Click" ToolTip="Delete" Visible="false" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDelete_Click" />
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
        function chkselectcheckbox() {
            var allElts = document.getElementById('ContentPlaceHolder1_grdProduct').getElementsByTagName('input')
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {

                jAlert('Select at least one Product !', 'Message');
                return false;

            }
            return true;

        }

        function chkallinvnetory() {
            if (confirm('Are you sure want to update all inventory in overstock?')) {
                chkHeight();
                return true;
            }
            else {
                return false;
            }
            return true;

        }

        function checkCount(mode) {
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
                jAlert('Select at least one Product !', 'Message');
                return false;
            }
            else {
                if (mode.toString() == 'Update') {
                    var a = document.getElementById('<%=txtValue.ClientID %>').value;
                    if (a == "") {
                        $(document).ready(function () { jAlert('Please enter the value for update...', 'Message', '<%=txtValue.ClientID %>'); });
                        return false;
                    }
                    else {
                        chkHeight();
                    }
                }
                else if (mode.toString() == 'Delete') {
                    return ConfirmDelete();
                }
                else if (mode.toString() == 'Approve') {
                    return ConfirmApprove();
                }
                else {
                    chkHeight();
                }
            }
        }

        function ConfirmDelete() {
            jConfirm('Are you sure want to delete all selected Product(s) ?', 'Confirmation', function (r) {
                if (r == true) {
                    chkHeight();
                    document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }

        function ConfirmApprove() {
            jConfirm('Are you sure want to Approve all selected Item(s) ?', 'Confirmation', function (r) {
                if (r == true) {
                    chkHeight();
                    document.getElementById('ContentPlaceHolder1_btnApproveTemp').click();
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
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            else {
                chkHeight();
            }
            return true;
        }
        //        function onKeyPressBlockNumbers(e) {
        //            var key = window.event ? window.event.keyCode : e.which;
        //            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0)
        //                return key;
        //            var keychar = String.fromCharCode(key);
        //            var reg = /\d/;
        //            if (window.event)
        //                return event.returnValue = reg.test(keychar);
        //            else
        //                return reg.test(keychar);


        //        }

        function keyRestrict(e, validchars) {

            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
                return true;
            return false;
        }
        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }
        function ShowModelUserRegisterPopup(ProductId, SID, invId) {
            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay').height = '300px';
            document.getElementById('frmdisplay').width = '602px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:602px;height:240px;");
            document.getElementById('popupContact').style.width = '602px';
            window.scrollTo(0, 0);
            document.getElementById('btnreadmore').click();
            document.getElementById('frmdisplay').src = 'InventoryWareHouse.aspx?PID=' + ProductId + '&StoreID=' + SID + '&invId=' + invId;

        }
        
    </script>

    <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px; height: 300px; position: fixed;">
        <table border="0" cellspacing="0" cellpadding="0" class="table_border">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay" frameborder="0" height="300" scrolling="auto"></iframe>
                </td>
            </tr>
        </table>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div style="display: none;">
        <input type="button" id="btnreadmore" />

         

    </div>
</asp:Content>
