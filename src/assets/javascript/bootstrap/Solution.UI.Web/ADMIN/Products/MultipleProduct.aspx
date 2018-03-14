<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="MultipleProduct.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.MultipleProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function ChangeText(dropdownId, div1Id, div2Id, divLabel1Id, divLabel2Id) {
            var Dropdown = document.getElementById(dropdownId);
            var div1 = document.getElementById(div1Id);
            var div2 = document.getElementById(div2Id);
            var divLabel1 = document.getElementById(divLabel1Id);
            var divLabel2 = document.getElementById(divLabel2Id);

            if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('append') != -1) {
                div1.style.display = 'block';
                div2.style.display = 'none';
                divLabel1.innerHTML = '&nbsp;&nbsp;Add Text :&nbsp;';
                divLabel1.style.display = 'block';
                divLabel2.innerHTML = '';
                divLabel2.style.display = 'none';
            }
            else if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('replace') != -1) {
                div1.style.display = 'block';
                div2.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;Old Text :&nbsp;';
                divLabel1.style.display = 'block';
                divLabel2.innerHTML = '&nbsp;&nbsp;New Text :&nbsp;';
                divLabel2.style.display = 'block';
            }
            else if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('new') != -1) {
                div1.style.display = 'block';
                div2.style.display = 'none';
                divLabel1.innerHTML = '&nbsp;&nbsp;New Text :&nbsp;';
                divLabel1.style.display = 'block';
                divLabel2.innerHTML = '';
                divLabel2.style.display = 'none';
            }
            else {
                div1.style.display = 'none';
                div2.style.display = 'none';
                divLabel1.innerHTML = '';
                divLabel1.style.display = 'none';
                divLabel2.innerHTML = '';
                divLabel2.style.display = 'none';
            }

        }

        function ChangePrice(dropdownId, div1Id, divLabel1Id) {
            var Dropdown = document.getElementById(dropdownId);
            var div1 = document.getElementById(div1Id);
            var divLabel1 = document.getElementById(divLabel1Id);


            if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('percentage increment') != -1) {
                div1.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;Percentage Increment :&nbsp;';
                divLabel1.style.display = 'block';
            }
            else if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('percentage decrement') != -1) {
                div1.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;Percentage Decrement :&nbsp;';
                divLabel1.style.display = 'block';
            }
            else if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('amount increment') != -1) {
                div1.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;Amount Increment :&nbsp;';
                divLabel1.style.display = 'block';
            }
            else if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('amount decrement') != -1) {
                div1.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;Amount Decrement :&nbsp;';
                divLabel1.style.display = 'block';
            }
            else if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('new') != -1) {
                div1.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;New Amount :&nbsp;';
                divLabel1.style.display = 'block';
            }
            else {

                div1.style.display = 'none';
                divLabel1.innerHTML = '';
                divLabel1.style.display = 'none';
            }


        }

        function ChangeInventory(dropdownId, div1Id, divLabel1Id) {
            var Dropdown = document.getElementById(dropdownId);
            var div1 = document.getElementById(div1Id);
            var divLabel1 = document.getElementById(divLabel1Id);

            if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('increment') != -1) {
                div1.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;Increment Inventory :&nbsp;';
                divLabel1.style.display = 'block';
            }
            else if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('decrement') != -1) {
                div1.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;Decrement Inventory :&nbsp;';
                divLabel1.style.display = 'block';
            }
            else if (Dropdown.options[Dropdown.selectedIndex].text.toLowerCase().indexOf('new') != -1) {
                div1.style.display = 'block';
                divLabel1.innerHTML = '&nbsp;&nbsp;New Inventory :&nbsp;';
                divLabel1.style.display = 'block';
            }
            else {
                div1.style.display = 'none';
                divLabel1.innerHTML = '';
                divLabel1.style.display = 'none';
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
        function DoValidation() {

            var Dropdown1 = document.getElementById('<%=ddlPrice.ClientID %>');

            if (Dropdown1.selectedIndex > 0) {
                if (document.getElementById('<%=txtPrice.ClientID %>').value == "") {
                    jAlert('Enter value for Price.', 'Message', '<%=txtPrice.ClientID %>');
                    return false;
                }
            }

            var Dropdown2 = document.getElementById('<%=ddlSalesPrice.ClientID %>');

            if (Dropdown2.selectedIndex > 0) {
                if (document.getElementById('<%=txtSalesPrice.ClientID %>').value == "") {
                    jAlert('Enter value for Sale Price.', 'Message', '<%=txtSalesPrice.ClientID %>');
                    return false;
                }
            }

            var Dropdown3 = document.getElementById('<%=ddlInventory.ClientID %>');

            if (Dropdown3.selectedIndex > 0) {
                if (document.getElementById('<%=txtInventory.ClientID %>').value == "") {
                    jAlert('Enter value for Inventory.', 'Message', '<%=txtInventory.ClientID %>');
                    return false;
                }
            }
            return true;
        }

    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
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
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add State" alt="Add State" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Edit Multiple Product"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="trErrorMsg" runat="server" visible="false">
                                            <td colspan="2">
                                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left" height="30" style="width: 170px" valign="top">
                                                &nbsp; SEDescription :
                                            </td>
                                            <td align="left">
                                                &nbsp;<asp:DropDownList ID="ddlSEDescription" runat="server" CssClass="select_box"
                                                    Width="200px" onchange="javascript:ChangeText(this.id,'divSEDescription1','divSEDescription2','divSEDescriptionLabel1','divSEDescriptionLabel2');">
                                                    <asp:ListItem Selected="True">No Change</asp:ListItem>
                                                    <asp:ListItem Value="Append">Append Text</asp:ListItem>
                                                    <asp:ListItem Value="Replace">Replace Text</asp:ListItem>
                                                    <asp:ListItem Value="New">New Text</asp:ListItem>
                                                </asp:DropDownList>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSEDescriptionLabel1" style="display: none;">
                                                            </div>
                                                        </td>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSEDescriptionLabel2" style="display: none;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 320px; padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSEDescription1" style="display: none;">
                                                                <asp:TextBox ID="txtSEDescription1" TextMode="MultiLine" Height="80px" Width="300px"
                                                                    runat="server" CssClass="textfield_small"></asp:TextBox></div>
                                                        </td>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSEDescription2" style="display: none;">
                                                                <asp:TextBox ID="txtSEDescription2" TextMode="MultiLine" Height="80px" Width="300px"
                                                                    runat="server" CssClass="textfield_small"></asp:TextBox></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td align="left" height="30" style="width: 170px" valign="top">
                                                &nbsp; SEKeywords : &nbsp;
                                            </td>
                                            <td align="left">
                                                &nbsp;<asp:DropDownList ID="ddlSEKeyword" runat="server" CssClass="select_box" Width="200px"
                                                    onchange="javascript:ChangeText(this.id,'divSEKeywords1','divSEKeywords2','divSEKeywordsLabel1','divSEKeywordsLabel2');">
                                                    <asp:ListItem Selected="True">No Change</asp:ListItem>
                                                    <asp:ListItem Value="Append">Append Text</asp:ListItem>
                                                    <asp:ListItem Value="Replace">Replace Text</asp:ListItem>
                                                    <asp:ListItem Value="New">New Text</asp:ListItem>
                                                </asp:DropDownList>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSEKeywordsLabel1" style="display: none;">
                                                            </div>
                                                        </td>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSEKeywordsLabel2" style="display: none;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 320px; padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSEKeywords1" style="display: none;">
                                                                <asp:TextBox ID="txtSEKeywords1" runat="server" CssClass="textfield_small" Height="80px"
                                                                    Width="300px" TextMode="MultiLine"></asp:TextBox></div>
                                                        </td>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSEKeywords2" style="display: none;">
                                                                <asp:TextBox ID="txtSEKeywords2" runat="server" CssClass="textfield_small" Height="80px"
                                                                    Width="300px" TextMode="MultiLine"></asp:TextBox></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left" height="30" style="width: 170px" valign="top">
                                                &nbsp; SETitle : &nbsp;
                                            </td>
                                            <td align="left">
                                                &nbsp;<asp:DropDownList ID="ddlSETitle" runat="server" CssClass="select_box" Width="200px"
                                                    onchange="javascript:ChangeText(this.id,'divSETitle1','divSETitle2','divSETitleLabel1','divSETitleLabel2');">
                                                    <asp:ListItem Selected="True">No Change</asp:ListItem>
                                                    <asp:ListItem Value="Append">Append Text</asp:ListItem>
                                                    <asp:ListItem Value="Replace">Replace Text</asp:ListItem>
                                                    <asp:ListItem Value="New">New Text</asp:ListItem>
                                                </asp:DropDownList>
                                                <table width="100%" style="padding-top: 0px; padding-bottom: 0px">
                                                    <tr>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSETitleLabel1" style="display: none;">
                                                            </div>
                                                        </td>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSETitleLabel2" style="display: none;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 320px; padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSETitle1" style="display: none;">
                                                                <asp:TextBox TextMode="MultiLine" ID="txtSETitle1" runat="server" Height="80px" Width="300px"
                                                                    CssClass="textfield_small"></asp:TextBox></div>
                                                        </td>
                                                        <td style="padding-top: 0px; padding-bottom: 0px">
                                                            <div id="divSETitle2" style="display: none;">
                                                                <asp:TextBox TextMode="MultiLine" ID="txtSETitle2" runat="server" Height="80px" Width="300px"
                                                                    CssClass="textfield_small"></asp:TextBox></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td align="left" style="width: 170px; height: 30px" valign="top">
                                                &nbsp; Price :
                                            </td>
                                            <td align="left" style="height: 30px">
                                                &nbsp;<asp:DropDownList ID="ddlPrice" runat="server" CssClass="select_box" Width="200px"
                                                    onchange="javascript:ChangePrice(this.id,'divPrice','divPriceLabel');">
                                                    <asp:ListItem Selected="True">No Change</asp:ListItem>
                                                    <asp:ListItem>Percentage Increment</asp:ListItem>
                                                    <asp:ListItem>Percentage Decrement</asp:ListItem>
                                                    <asp:ListItem>Amount Increment</asp:ListItem>
                                                    <asp:ListItem>Amount Decrement</asp:ListItem>
                                                    <asp:ListItem>New</asp:ListItem>
                                                </asp:DropDownList>
                                                <div id="divPrice" style="display: none;">
                                                    &nbsp;
                                                    <div id="divPriceLabel" style="display: none; float: left; padding-top: 5px; padding-bottom: 5px;">
                                                    </div>
                                                    &nbsp;
                                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="textfield_small" Style="border: 1px Solid #7F9DB9;
                                                        margin-top: 5px; margin-bottom: 5px; float: left; width: 65px" MaxLength="6"
                                                        onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox></div>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left" valign="top" style="width: 170px; height: 30px;">
                                                &nbsp; SalesPrice :
                                            </td>
                                            <td align="left" style="height: 30px">
                                                &nbsp;<asp:DropDownList ID="ddlSalesPrice" runat="server" CssClass="select_box" Width="200px"
                                                    onchange="javascript:ChangePrice(this.id,'divSalesPrice','divSalesPriceLabel');">
                                                    <asp:ListItem Selected="True">No Change</asp:ListItem>
                                                    <asp:ListItem>Percentage Increment</asp:ListItem>
                                                    <asp:ListItem>Percentage Decrement</asp:ListItem>
                                                    <asp:ListItem>Amount Increment</asp:ListItem>
                                                    <asp:ListItem>Amount Decrement</asp:ListItem>
                                                    <asp:ListItem>New</asp:ListItem>
                                                </asp:DropDownList>
                                                <div id="divSalesPrice" style="display: none;">
                                                    &nbsp;
                                                    <div id="divSalesPriceLabel" style="display: none; float: left; padding-top: 5px;
                                                        padding-bottom: 5px;">
                                                    </div>
                                                    &nbsp;
                                                    <asp:TextBox ID="txtSalesPrice" runat="server" CssClass="textfield_small" Style="border: 1px Solid #7F9DB9;
                                                        margin-top: 5px; margin-bottom: 5px; float: left; width: 80px" MaxLength="6"
                                                        onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox></div>
                                            </td>
                                        </tr>
                                        <tr class="even-row" style="display:none;">
                                            <td align="left" valign="top" style="height: 30px; width: 170px;">
                                                &nbsp; Inventory :
                                            </td>
                                            <td align="left" style="height: 30px" class="font-black01">
                                                &nbsp;<asp:DropDownList ID="ddlInventory" runat="server" CssClass="select_box" Width="200px"
                                                    onchange="javascript:ChangeInventory(this.id,'divInventory','divInvetntoryLabel');">
                                                    <asp:ListItem Selected="True">No Change</asp:ListItem>
                                                    <asp:ListItem>Increment</asp:ListItem>
                                                    <asp:ListItem> Decrement</asp:ListItem>
                                                    <asp:ListItem>New</asp:ListItem>
                                                </asp:DropDownList>
                                                <div id="divInventory" style="display: none;">
                                                    &nbsp;
                                                    <div id="divInvetntoryLabel" style="display: none; float: left; padding-top: 5px;
                                                        padding-bottom: 5px;">
                                                    </div>
                                                    &nbsp;
                                                    <asp:TextBox ID="txtInventory" runat="server" CssClass="textfield_small" MaxLength="6"
                                                        Style="border: 1px Solid #7F9DB9; margin-top: 5px; float: left; margin-bottom: 5px;
                                                        width: 80px" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox></div>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                            </td>
                                            <td align="left">
                                                <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                    OnClientClick="return DoValidation();" OnClick="btnSave_Click" />
                                                <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                    OnClick="btnCancel_Click" />
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
</asp:Content>
