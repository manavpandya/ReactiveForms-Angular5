<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Variant.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Variant"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
     function OpenPopup(VId,VariValId,RId) 
        {
            var width = 850;
            var height = 650;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            var popupurl = "ProductSearch.aspx?VId="+VId+"&VariValId="+VariValId+"&ID=<%=Request["ID"] %>&StoreID=<%=Request["StoreID"] %>&RId="+RId+"";
            window.open(popupurl, "ProductSearch", windowFeatures);
            //window.open(popupurl, "ProductSearch", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800,height=650,left=150,top=80");

        }

        function AddOptionOpenPopup() 
        {
            var popupurl = "ProductSearch.aspx?StoreID=<%=Request["StoreID"] %>";
            window.open(popupurl, "ProductSearch", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=600,height=550,left=250,top=80");
        }

        function GotoProduct() {
            var productid = '<%=Request["Id"] %>'
            if (productid != '' && productid != null) {
                window.location.href = 'Product.aspx?StoreId=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>&Mode=edit';
            }
        }

        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
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

        function SetRowIndex(rowindex)
        {
            if(document.getElementById('ContentPlaceHolder1_hdnGrIndex'))
            {
                document.getElementById('ContentPlaceHolder1_hdnGrIndex').value = rowindex;
            }
        }

        function ConfirmDeleteOption()
        {
            return confirm('Are you sure, you want to Delete this Option? - ' + document.getElementById('ContentPlaceHolder1_hdnOptionName').value);
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
    <script type="text/javascript">
        function UpdateConformation() {
            var allElts = document.forms['form1'].elements;
            var i;
            var count = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];

                if (elt.type == "checkbox") {
                    if (elt.checked == true)
                        count++;
                }
            }
            if (count > 0) {
                return true;
            }
            else {
                jAlert('Please select atleast one Option Value to update.', 'Message');
                return false;
            }
        }


        function Conformation() {
            var allElts = document.forms['form1'].elements;
            var i;
            var count = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];

                if (elt.type == "checkbox") {
                    if (elt.checked == true)
                        count++;
                }
            }
            if (count > 0) {
                var ans = confirm('Are you sure, you want to delete the Option Value(s)?');
                if (ans)
                    return true;
                else
                    return false;
            }
            else {
                jAlert('Please select atleast one Option Value to delete.', 'Message');
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function ValidatePage() {
            if (document.getElementById('ContentPlaceHolder1_txtOptionName')) {
                if ((document.getElementById('ContentPlaceHolder1_txtOptionName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Option Name', 'Message', 'ContentPlaceHolder1_txtOptionName');
                    return false;
                }
            }
            if (document.getElementById('ContentPlaceHolder1_txtOptionValue')) {
                if ((document.getElementById('ContentPlaceHolder1_txtOptionValue').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please enter Option Value', 'Message', 'ContentPlaceHolder1_txtOptionValue')
                    return false;
                }
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
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td>
                    <a href="javascript:void(0);" title="Go To Product" onclick="return GotoProduct();">
                        <img src="/App_Themes/<%=Page.Theme %>/images/go-to-product.png" alt="Go To Product"
                            title="Go To Product" class="img-right" height="23" />
                    </a>
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
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
                                            <div class="main-title-left">
                                                <img class="img-left" title="Product Options" alt="Product Options" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                <h2>
                                                    Product Options</h2>
                                            </div>
                                            <div class="main-title-right" style="display: none;">
                                                <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                    <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/close.png" /></a>
                                            </div>
                                        </th>
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
                                                    <tr>
                                                        <td style="padding-left: 20px">
                                                            <table cellpadding="0" cellspacing="0" width="90%" align="left">
                                                                <tr valign="top">
                                                                    <td style="width: 90px">
                                                                        <b>Product Name :</b>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblProductName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span style="padding-left: 20px"></span>
                                                            <input type="hidden" value="0" id="hdnVariantId" runat="server" />
                                                            <asp:DataList CssClass="product_table" HorizontalAlign="Center" RepeatDirection="Horizontal"
                                                                Width="97%" ID="dltVariant" runat="server" ShowHeader="true" BorderColor="#e7e7e7"
                                                                BorderWidth="2px" BorderStyle="Solid" RepeatColumns="3" OnItemCommand="dltVariant_ItemCommand">
                                                                <HeaderTemplate>
                                                                    Option List
                                                                </HeaderTemplate>
                                                                <HeaderStyle BackColor="#e7e7e7" Font-Bold="true" Font-Size="12px" Font-Names="Arial,Helvetica,sans-serif"
                                                                    BorderWidth="1px" BorderColor="#eeeeee" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Text='<%# DataBinder.Eval(Container.DataItem,"VARIANTNAME") %>' CommandName='<%#DataBinder.Eval(Container.DataItem,"VARIANTID") %>'
                                                                        ID="lb_Att" CssClass="attribute_text" runat="server"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="gridalt" Width="33%" BackColor="#f3f3f3" BorderWidth="1px" BorderColor="#eeeeee" />
                                                                <AlternatingItemStyle CssClass="gridalt" Width="33%" BackColor="#f3f3f3" BorderWidth="1px"
                                                                    BorderColor="#eeeeee" />
                                                            </asp:DataList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span style="padding-left: 20px"></span>
                                                            <asp:ImageButton ID="btnAddOption" ToolTip="Add Option" runat="server" OnClick="btnAddOption_Click" />
                                                            &nbsp;&nbsp;<asp:ImageButton ID="btnAddOptionValue" ToolTip="Add Option Value" runat="server"
                                                                OnClick="btnAddOptionValue_Click" />
                                                            &nbsp;&nbsp;<asp:ImageButton ID="btnDeleteOption" Visible="false" ToolTip="Delete Option"
                                                                runat="server" OnClientClick="return ConfirmDeleteOption()" AlternateText="Delete Option"
                                                                OnClick="btnDeleteOption_Click" />
                                                            <asp:HiddenField ID="hdnOptionName" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row" id="trAddOption" runat="server" visible="false">
                                                        <td align="center">
                                                            <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Option Name :
                                                                    </td>
                                                                    <td width="88%" align="left">
                                                                        <asp:TextBox ID="txtOptionName" runat="server" MaxLength="100" class="order-textfield"
                                                                            Width="250px"></asp:TextBox>
                                                                        <asp:DropDownList ID="ddlOptionName" Width="250px" runat="server" Visible="false"
                                                                            CssClass="order-list">
                                                                        </asp:DropDownList>
                                                                        <input type="hidden" value="0" id="hdnOption" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Option Value :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtOptionValue" runat="server" MaxLength="100" Width="250px" class="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow" valign="top" style="display: none">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;&nbsp;</span>Option Value Description :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtOptionValueDescription" runat="server" CssClass="order-textfield"
                                                                            Width="400px" Columns="4" Height="60px" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>Option Price :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtOptionPrice" runat="server" MaxLength="9" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                            Width="100px" CssClass="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>Option SKU :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtOptionSKU" MaxLength="500" runat="server" Width="250px" CssClass="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>Option Header :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtOptionHeader" runat="server" Width="250px" CssClass="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>Option UPC :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtOptionUPC" MaxLength="100" runat="server" Width="250px" CssClass="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>Display Order :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtDisplayOrder" runat="server" MaxLength="9" onkeypress="return keyRestrict(event,'0123456789');"
                                                                            Width="100px" CssClass="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSavecancel" runat="server" visible="false">
                                        <td align="center">
                                            <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                                <tr class="oddrow">
                                                    <td width="10%">
                                                    </td>
                                                    <td width="88%" align="left">
                                                        <asp:ImageButton ID="btnSaveOption" runat="server" OnClientClick="return ValidatePage();"
                                                            OnClick="btnSaveOption_Click" />
                                                        <asp:ImageButton ID="btnCancelOption" runat="server" OnClick="btnCancelOption_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table cellpadding="0" cellspacing="0" width="99%">
                                                <tr id="trVariantValue" runat="server" visible="false">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <strong>
                                                                        <asp:Label ID="lblValue" runat="server" Text="Option Values for:"></asp:Label></strong>
                                                                    <asp:TextBox ID="txtEditVariantName" runat="server" CssClass="order-textfield" MaxLength="100"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvEditVariantName" runat="server" ControlToValidate="txtEditVariantName"
                                                                        ErrorMessage="*" SetFocusOnError="true" ForeColor="Red" ValidationGroup="EditVariantName"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="btnUpdateVariantName" runat="server" OnClick="btnUpdateVariantName_OnClick"
                                                                        Text="" ValidationGroup="EditVariantName" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:GridView ID="grdVariantValue" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            CssClass="table-noneforOrder">
                                                            <EmptyDataRowStyle HorizontalAlign="Center" />
                                                            <EmptyDataTemplate>
                                                                <label style="color: Red">
                                                                    No records found ...</label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVariantValueID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValueID") %>'></asp:Label>
                                                                        <asp:Label ID="lblVariantID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <strong>Select </strong>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <strong>Option Value </strong>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VariantValue") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <strong>Option SKU</strong>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle Width="10%" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtgrdoptionsku" runat="server" MaxLength="500" Text='<%#Convert.ToString( DataBinder.Eval(Container.DataItem,"SKU")) %>'
                                                                            Width="100px" CssClass="order-textfield"></asp:TextBox>
                                                                        <asp:Label ID="lblOptionSKU" Visible="false" runat="server" Text='<%#Convert.ToString( DataBinder.Eval(Container.DataItem,"SKU")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <strong>Option Header</strong>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle Width="10%" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtgrdoptionheader" runat="server" Text='<%#Convert.ToString( DataBinder.Eval(Container.DataItem,"Header")) %>'
                                                                            Width="100px" CssClass="order-textfield"></asp:TextBox>
                                                                        <asp:Label ID="lblOptionHeader" Visible="false" runat="server" Text='<%#Convert.ToString( DataBinder.Eval(Container.DataItem,"Header")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <strong>Option UPC</strong>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle Width="10%" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtgrdoptionupc" runat="server" MaxLength="100" Text='<%#Convert.ToString( DataBinder.Eval(Container.DataItem,"UPC")) %>'
                                                                            Width="100px" CssClass="order-textfield"></asp:TextBox>
                                                                        <asp:Label ID="lblOptionUPC" Visible="false" runat="server" Text='<%#Convert.ToString( DataBinder.Eval(Container.DataItem,"UPC")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <strong>Display Order</strong>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle Width="10%" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtgrddisplayorder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'
                                                                            Width="100px" Style="text-align: center" CssClass="order-textfield" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                                        <asp:Label ID="lblDisplayOrder" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Price">
                                                                    <HeaderTemplate>
                                                                        Option Price
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtgrdOptionPrice" CssClass="order-textfield" runat="server" Text='<%# string.Format("{0:F}", DataBinder.Eval(Container.DataItem, "VariantPrice"))%>'
                                                                            Width="70px" onkeypress="return keyRestrict(event,'0123456789.');" Style="text-align: center"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="top" />
                                                                </asp:TemplateField>
                                                                <%--     <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <strong>Option Product SKU</strong>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle Width="15%" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOptionProductSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Sku") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <strong>Change Product</strong>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle Width="12%" />
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <a title="Click Here to Change Product" target="content" class="list_lin" style="cursor: pointer;
                                                                            font-weight: bold;" onclick="JavaScript:OpenPopup('<%#DataBinder.Eval(Container.DataItem,"VariantID") %>','<%#DataBinder.Eval(Container.DataItem,"VariantValueID") %>',<%#Container.DataItemIndex %>);">
                                                                            Change</a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr class="altrow" id="trCheckall" runat="server" visible="false">
                                                    <td>
                                                        <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                            href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;
                                                                padding-right: 10px;">
                                                                <asp:ImageButton ID="btnUpdateVariantValue" runat="server" OnClientClick="return UpdateConformation();"
                                                                    AlternateText="Update Variant Value" OnClick="btnUpdateVariantValue_Click" />
                                                                &nbsp;
                                                                <asp:ImageButton ID="btnDeleteOptionValue" OnClientClick="return Conformation();"
                                                                    runat="server" AlternateText="Delete Variant Value" OnClick="btnDeleteOptionValue_Click" />&nbsp;
                                                                &nbsp; </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
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
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <!--start tab--->
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-1.2.6.min.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/tabs.js" type="text/javascript"></script>
    <!--end tab--->
</asp:Content>
