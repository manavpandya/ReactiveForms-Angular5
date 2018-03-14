<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="TradeCustomerTemplate.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.TradeCustomerTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/<%=Page.Theme %>/js/CustomerValidation.js" type="text/javascript"
        language="javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        function CheckFirstname() {
            if ($('#ContentPlaceHolder1_txtFirstName').val() != '')
            { return true; }
            else { jAlert('Please Enter First Name!', 'Message'); }

            return false;

        }

        function checkEmail() {
            if (document.getElementById('ContentPlaceHolder1_txtEmail').value == '') {
                jAlert('Please Enter EmailID.', 'Required Information', 'ContentPlaceHolder1_txtEmail');
                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }
        }


        function ValidateTradePage()
        {
            if ($('#ContentPlaceHolder1_txtTemplatename').val() != '')
            { return true; }
            else {
                jAlert('Please Enter Template Name.', 'Required Information', 'ContentPlaceHolder1_txtTemplatename');
                document.getElementById('ContentPlaceHolder1_txtTemplatename').focus();
            }

            return false;
        }

        function openCenteredCrossSaleWindow(mode, clientid) {

            //createCookie('prskus', document.getElementById(clientid).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<% = ddlStoreName.SelectedValue %>';
            var IDs = document.getElementById(clientid).value;
            if (IDs != '') {
                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                if (mode == 1)
                    window.open('/Admin/Customers/PopUpProduct.aspx?&StoreID=' + StoreID + '&mode=' + mode + '&clientid=' + clientid + '&Ids=' + IDs, "Mywindow", windowFeatures);
                else if (mode == 2)
                    window.open('/Admin/Customers/PopupCategory.aspx?&StoreID=' + StoreID + '&mode=' + mode + '&clientid=' + clientid + '&Ids=' + IDs, "Mywindow", windowFeatures);
            }
            else {
                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                if (mode == 1)
                    window.open('/Admin/Customers/PopUpProduct.aspx?&StoreID=' + StoreID + '&mode=' + mode + '&clientid=' + clientid, "Mywindow", windowFeatures);
                else if (mode == 2)
                    window.open('/Admin/Customers/PopupCategory.aspx?&StoreID=' + StoreID + '&mode=' + mode + '&clientid=' + clientid, "Mywindow", windowFeatures);
            }

            return false;
        }



        function ShowHideButton(imgid, divid, divid1) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('expand.gif') > -1) {
                    document.getElementById(imgid).src = src.replace('expand.gif', 'minimize.png');
                    document.getElementById(imgid).title = 'Minimize';
                    document.getElementById(imgid).alt = 'Minimize';
                    document.getElementById(imgid).style.marginTop = "4px";
                    document.getElementById(imgid).className = 'minimize';
                    document.getElementById(divid).style.display = '';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = '';
                    }
                }
                else if (src.indexOf('minimize.png') > -1) {
                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    document.getElementById(divid).style.display = 'none';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = 'none';
                    }
                }
            }
        }

        function ShowCustDetail(id) {
            //document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '600px';
            document.getElementById('frmdisplay1').width = '1000px';
            document.getElementById('frmdisplay1').scrolling = 'no';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 100px; padding: 0px;width:1000px;height:500px;");
            document.getElementById('popupContact1').style.width = '1000px';
            document.getElementById('popupContact1').style.height = '600px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;


        }



    </script>
    <script type="text/javascript">
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
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;
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
                                                    <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Customer"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;">Template Information
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" style="padding-left: 10px;" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow" style="display:none;">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Store Name :
                                                        </td>
                                                        <td style="width: 80%; vertical-align: top">
                                                            <asp:DropDownList runat="server" ID="ddlStoreName" CssClass="add-product-list" Width="225px"
                                                                Height="20px" AutoPostBack="True" OnSelectedIndexChanged="ddlStoreName_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Template Name :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtTemplatename" CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Active :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkActiveTemplate" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%" class="add-product" id="tblDiscount" runat="server">
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                                            <tr>
                                                                                <th colspan="6">
                                                                                    <div class="main-title-left">
                                                                                        <img class="img-left" title="Category Wise Discount Details" alt="Category Wise Discount Details"
                                                                                            src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                        <h2>Category Wise Discount Details</h2>
                                                                                    </div>
                                                                                    <div class="main-title-right">
                                                                                        <a href="javascript:void(0);" class="show_hideCateInfo" onclick="return ShowHideButton('ImgCate','tdCate','divcatdiscount');">
                                                                                            <img id="ImgCate" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                                                    </div>
                                                                                </th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" id="tdCate" colspan="6">
                                                                                    <div id="divcatdiscount" class="slidingDivCateInfo">
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div style="overflow-y: auto;">
                                                                                                        <asp:GridView ID="grdCategory" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                                                                            EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                                            ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                                                                                            CellPadding="2" CellSpacing="1" PageSize="10" OnRowCommand="grdCategory_RowCommand"
                                                                                                            OnPageIndexChanging="grdCategory_PageIndexChanging" OnRowDeleting="grdCategory_RowDeleting">
                                                                                                            <EmptyDataTemplate>
                                                                                                                <center>
                                                                                                                    <span style="color: Red;">No Record(s) Found !</span></center>
                                                                                                            </EmptyDataTemplate>
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField Visible="False">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblMembershipDiscountID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'></asp:Label>
                                                                                                                        <asp:Label ID="lblCategoryID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CategoryId") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <HeaderTemplate>
                                                                                                                        Name
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblProName" runat="server" Text='<%# Bind("Productname") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <HeaderTemplate>
                                                                                                                        Discount
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDiscountPercent" runat="server" Text='<%# Bind("CategoryDiscount","{0:F2}") %>'></asp:Label>
                                                                                                                        <asp:TextBox ID="txtDiscountPercent" Style="text-align: center;" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                            runat="server" CssClass="order-textfield" Visible="false" Width="80px" MaxLength="6"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Edit Discount Percent">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton CommandName="Edit Percent" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'
                                                                                                                            ID="btnEdit" runat="server" CssClass="link-font" ImageUrl="~/Admin/images/file-icon.gif" />
                                                                                                                        <asp:ImageButton ID="btnSave" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CategoryId") %>'
                                                                                                                            CommandName="Add" Visible="False" ImageUrl="~/Admin/images/save.jpg" ToolTip="Save" />
                                                                                                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Stop" ImageUrl="~/Admin/images/cancel.jpg"
                                                                                                                            Visible="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'
                                                                                                                            ToolTip="Cancel" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton CommandName="Delete" ID="btnDelete" runat="server" CssClass="link-font"
                                                                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CategoryId") %>' ImageUrl="~/Admin/images/delete-icon.gif"
                                                                                                                            OnClientClick="javascript:if(confirm('Are you sure you want to delete this Record?')){ return true;} else { return false;};" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            <PagerSettings Mode="NumericFirstLast" Position="Bottom"></PagerSettings>
                                                                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                                        </asp:GridView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="right">
                                                                                                <td>
                                                                                                    <a id="a1" name="aRelated" onclick="openCenteredCrossSaleWindow(2,'ContentPlaceHolder1_hdnCatWiseDiscountids');"
                                                                                                        style="margin-right: 15px; font-weight: bold; cursor: pointer;" title="Select Category(s)">Select Category(s) </a>&nbsp;&nbsp;&nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                                            <tr>
                                                                                <th colspan="6">
                                                                                    <div class="main-title-left">
                                                                                        <img class="img-left" title="Product Wise Discount Details" alt="Product Wise Discount Details"
                                                                                            src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                        <h2>Product Wise Discount Details</h2>
                                                                                    </div>
                                                                                    <div class="main-title-right">
                                                                                        <a href="javascript:void(0);" class="show_hideProductInfo" onclick="return ShowHideButton('ImgDesc','tdDesc','divproddiscount');">
                                                                                            <img id="ImgDesc" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                                                    </div>
                                                                                </th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" id="tdDesc" colspan="6">
                                                                                    <div id="div2" class="slidingDivProductInfo">
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div id="divproddiscount" style="overflow-y: auto;">
                                                                                                        <asp:GridView ID="grdProduct" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                                                                            EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                                            ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                                                                                            CellPadding="2" CellSpacing="1" PageSize="10" OnRowDeleting="grdProduct_RowDeleting"
                                                                                                            OnRowCommand="grdProduct_RowCommand" OnPageIndexChanging="grdProduct_PageIndexChanging">
                                                                                                            <EmptyDataTemplate>
                                                                                                                <center>
                                                                                                                    <span style="color: Red;">No Record(s) Found !</span></center>
                                                                                                            </EmptyDataTemplate>
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField Visible="False">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblMembershipDiscountID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'></asp:Label>
                                                                                                                        <asp:Label ID="lblProductId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductId") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <HeaderTemplate>
                                                                                                                        Name
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblProName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="SKU">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblProSku" runat="server" Text='<%# Bind("Sku") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <HeaderTemplate>
                                                                                                                        Discount
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDiscountPercent" runat="server" Text='<%# Bind("ProductDiscount","{0:F2}") %>'></asp:Label>
                                                                                                                        <asp:TextBox ID="txtDiscountPercent" runat="server" Style="text-align: center;" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                            CssClass="order-textfield" Visible="false" Width="80px" MaxLength="6"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Edit Discount Percent">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton CommandName="Edit Percent" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'
                                                                                                                            ID="btnEdit" runat="server" CssClass="link-font" ImageUrl="~/Admin/images/file-icon.gif" />
                                                                                                                        <asp:ImageButton ID="btnSave" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductId") %>'
                                                                                                                            CommandName="Add" Visible="False" ImageUrl="~/Admin/images/save.jpg" ToolTip="Save" />
                                                                                                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Stop" ImageUrl="~/Admin/images/cancel.jpg"
                                                                                                                            Visible="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'
                                                                                                                            ToolTip="Cancel" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton CommandName="Delete" ID="btnDelete" runat="server" CssClass="link-font"
                                                                                                                            CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ProductId") %>' ImageUrl="~/Admin/images/delete-icon.gif"
                                                                                                                            OnClientClick="javascript:if(confirm('Are you sure you want to delete this Record?')){ return true;} else { return false;};" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            <PagerSettings Mode="NumericFirstLast" Position="Bottom"></PagerSettings>
                                                                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                                        </asp:GridView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="right">
                                                                                                <td>
                                                                                                    <a id="aRelated" title="Select Product(s)" name="aRelated" onclick="openCenteredCrossSaleWindow(1,'ContentPlaceHolder1_hdnProdWiseDiscountids');"
                                                                                                        style="margin-right: 15px; font-weight: bold; cursor: pointer;">Select Product(s)
                                                                                                    </a>&nbsp;&nbsp;&nbsp;
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


                                                    <tr class="oddrow">
                                                        <td align="center" colspan="2">
                                                            <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgSave_Click" OnClientClick="return ValidateTradePage();" />
                                                            <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                OnClick="imgCancle_Click" CausesValidation="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>


                        <div style="display: none;">
                            <asp:HiddenField ID="hdnCatWiseDiscountids" runat="server" />
                            <asp:HiddenField ID="hdnProdWiseDiscountids" runat="server" />
                            <asp:Button ID="btnCustDiscountDetailid" runat="server" OnClick="btnCustDiscountDetailid_Click" />
                            <asp:Button ID="btnProdDiscountDetailid" runat="server" OnClick="btnProdDiscountDetailid_Click" />
                            <input type="button" id="btnreadmore" />
                            <input type="button" id="btnhelpdescri" />



                            <input id="hdndeleteaddresid" type="hidden" runat="server" />
                            <asp:ImageButton ID="popupContactClose1" ImageUrl="/images/close.png" runat="server"
                                ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
                        </div>
        </div>
    </div>

    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div id="popupContact1" style="z-index: 1000001; width: 750px; height: 350px;">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay1" frameborder="0" height="350px" width="750" scrolling="auto"></iframe>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

