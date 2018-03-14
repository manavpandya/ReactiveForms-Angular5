<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddYardageAvailability.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.AddYardageAvailability" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript">
        $(function () {
            for (var i = 0; i < 50; i++) {
                if (document.getElementById('ContentPlaceHolder1_grdProductStyleType_txtAvailableDate_' + i) != null) {
                    $('#ContentPlaceHolder1_grdProductStyleType_txtAvailableDate_' + i).datetimepicker({
                        showButtonPanel: true, ampm: false,
                        showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                        buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                    });
                }
            }
        });

        function checkvalidationsearchtype(btnid) {
            var AvailDate = btnid.replace('_btnSave_', '_txtAvailableDate_');
            if (document.getElementById(AvailDate) != null && document.getElementById(AvailDate).value != '') {
                var ADate = new Date(document.getElementById(AvailDate).value);
                var date = new Date();
                var CurrDate = ("0" + (date.getMonth() + 1).toString()).substr(-2) + "/" + ("0" + date.getDate().toString()).substr(-2) + "/" + (date.getFullYear().toString()).substr(0);
                var AvailableDate = ("0" + (ADate.getMonth() + 1).toString()).substr(-2) + "/" + ("0" + ADate.getDate().toString()).substr(-2) + "/" + (ADate.getFullYear().toString()).substr(0);
                if (CurrDate > AvailableDate) {
                    jAlert('Please Select Valid Date!', 'Message', document.getElementById(AvailDate));
                    return false;
                }
            }
            return true;
        }

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

        function chkselectcheckbox() {
            var allElts = document.getElementById('ContentPlaceHolder1_grdProductStyleType').getElementsByTagName('input')
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
                jAlert('Select at least one Record(s) !', 'Message');
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function keyRestrictforIntOnly(e, validchars) {
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
    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                                    <img class="img-left" title="Product Yardage Availability" alt="Product Yardage Availability"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/state-list-icon.png" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Product Yardage Availability" ID="lblTitle"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <center>
                                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label></center>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td style="width: 8%" align="center">
                                                            <span class="star">*</span>Fabric Type :
                                                        </td>
                                                        <td style="vertical-align: top">
                                                            <asp:DropDownList runat="server" ID="ddlFabricType" CssClass="add-product-list" Width="225px"
                                                                Height="20px" AutoPostBack="true" OnSelectedIndexChanged="ddlFabricType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" id="trTop" runat="server" visible="false">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <span><a id="A1" href="javascript:selectAll(true);">Check All</a> | <a id="A2" href="javascript:selectAll(false);">
                                                                Clear All</a> </span>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" id="trFabricDetails" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:GridView ID="grdProductStyleType" runat="server" AutoGenerateColumns="False"
                                                                BorderColor="#e7e7e7" BorderStyle="Solid" BorderWidth="1px" DataKeyNames="FabricCodeId"
                                                                EmptyDataText="No Record(s) Found." AllowSorting="false" EmptyDataRowStyle-ForeColor="Red"
                                                                EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                                GridLines="None" AllowPaging="True" OnRowDataBound="grdProductStyleType_RowDataBound"
                                                                OnRowCommand="grdProductStyleType_RowCommand" PageSize="50" PagerSettings-Mode="NumericFirstLast">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            &nbsp;Select
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblFabricCodeId" runat="server" Visible="false" Text='<%# Bind("FabricCodeId") %>'></asp:Label>
                                                                            &nbsp;<asp:CheckBox ID="chkActive" runat="server" />
                                                                              &nbsp;<asp:Label ID="lblWidth" runat="server" Visible="false" Text='<%# Bind("width") %>'></asp:Label>
                                                                            <asp:TextBox ID="txtWidth" runat="server" Width="70px" CssClass="order-textfield"
                                                                                Style="text-align: center;display:none;" Text='<%# Bind("width") %>' MaxLength="40"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            &nbsp;Code
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <HeaderTemplate>
                                                                            &nbsp;Width
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                        <ItemTemplate>
                                                                          
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <HeaderTemplate>
                                                                            &nbsp;Quantity On Hand
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblQtyOnHand" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"QtyOnHand")),2) %>'></asp:Label>
                                                                            <asp:TextBox ID="txtQtyOnHand" runat="server" Width="70px" MaxLength="6" CssClass="order-textfield"
                                                                                Style="text-align: center;" onkeypress="return keyRestrictforIntOnly(event,'0123456789');"
                                                                                Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"QtyOnHand")),2) %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <HeaderTemplate>
                                                                            &nbsp;Next Order Quantity
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblNextOrderQty" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"NextOrderQty")),2) %>'></asp:Label>
                                                                            <asp:TextBox ID="txtNextOrderQty" runat="server" Width="70px" CssClass="order-textfield"
                                                                                Style="text-align: center;" onkeypress="return keyRestrictforIntOnly(event,'0123456789');"
                                                                                Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"NextOrderQty")),2) %>'
                                                                                MaxLength="10"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <HeaderTemplate>
                                                                            &nbsp;Allow Quantity
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblAllowQty" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AllowQty")),2) %>'></asp:Label>
                                                                            <asp:TextBox ID="txtAllowQty" runat="server" Width="70px" CssClass="order-textfield"
                                                                                Style="text-align: center;" onkeypress="return keyRestrictforIntOnly(event,'0123456789');"
                                                                                Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AllowQty")),2) %>'
                                                                                MaxLength="10"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <HeaderTemplate>
                                                                            &nbsp;Available Date
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAvailableDate" runat="server" Visible="false" Text='<%# SetShortDate(Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableDate"))) %>'></asp:Label>
                                                                            <asp:TextBox ID="txtAvailableDate" runat="server" CssClass="textfield_small" Style="text-align: center;
                                                                                margin-right: 3px;" Text='<%# SetShortDate(Convert.ToString(DataBinder.Eval(Container.DataItem,"AvailableDate"))) %>'
                                                                                Width="100"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            &nbsp;No of Days
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtNoOfDays" runat="server" Width="70px" CssClass="order-textfield"
                                                                                Style="text-align: center;" onkeypress="return keyRestrictforIntOnly(event,'0123456789');"
                                                                                Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"NoOfDays")) %>'
                                                                                MaxLength="10"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <HeaderTemplate>
                                                                            &nbsp;Alert Quantity
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblAlertQty" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AlertQty")),2) %>'></asp:Label>
                                                                            <asp:TextBox ID="txtAlertQty" runat="server" Width="70px" onkeypress="return keyRestrictforIntOnly(event,'0123456789');"
                                                                                Style="text-align: center;" CssClass="order-textfield" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AlertQty")),2) %>'
                                                                                MaxLength="10"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Operations">
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" ID="_editLinkButton" Visible="false" ToolTip="Edit"
                                                                                CommandName="edit" CommandArgument='<%# Eval("FabricCodeID") %>'></asp:ImageButton>
                                                                            <asp:ImageButton ID="btnSave" OnClientClick="return checkvalidationsearchtype(this.id);"
                                                                                runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>'
                                                                                CommandName="Save" AlternateText="Save" />
                                                                            <asp:ImageButton ID="btnCancel" runat="server" Visible="false" CommandName="Cancel"
                                                                                Height="16px" Width="16px" AlternateText="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>' />
                                                                            <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="DeleteAdmin"
                                                                                CommandArgument='<%# Eval("FabricCodeID") %>' message="Are you sure want to delete current Record?"
                                                                                OnClientClick='return confirm(this.getAttribute("message"))'></asp:ImageButton>
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
                                                    <tr class="altrow" id="trBottom" runat="server" visible="false">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                                href="javascript:selectAll(false);">Clear All</a> </span>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                Visible="false" OnClientClick="return chkselectcheckbox();" OnClick="imgSave_Click" />
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
</asp:Content>
