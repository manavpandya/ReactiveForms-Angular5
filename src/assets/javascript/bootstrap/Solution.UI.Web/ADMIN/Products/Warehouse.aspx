<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Warehouse.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Warehouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function MakeBillingOtherVisible() {
            if (document.getElementById('ContentPlaceHolder1_ddlState') != null && document.getElementById('ContentPlaceHolder1_ddlState').options[document.getElementById('ContentPlaceHolder1_ddlState').selectedIndex].value == '-11') {
                SetBillingOtherVisible(true);
            }
            else {

                SetBillingOtherVisible(false);
            }
        }

        function SetBillingOtherVisible(IsVisible) {
            if (IsVisible && document.getElementById('DIVBillingOther') != null) {

                document.getElementById('DIVBillingOther').style.display = 'block';
            }
            else {

                if (document.getElementById('ContentPlaceHolder1_txtOtherState') != null) { document.getElementById('ContentPlaceHolder1_txtOtherState').value = ''; }
                if (document.getElementById('DIVBillingOther') != null) { document.getElementById('DIVBillingOther').style.display = 'none'; }

            }
        }

        function isNumberKey(event) {
            var retval = false;
            var charCode = (event.keyCode) ? event.keyCode : event.which;
            if (charCode > 31 && (charCode < 33 || charCode > 64) && (charCode < 91 || charCode > 96) && (charCode < 123 || charCode > 126))
                retval = true;
            if (charCode == 8 || (charCode > 35 && charCode < 41) || charCode == 46 || charCode == 9)
                retval = true;
            if (navigator.appName.indexOf('Microsoft') != -1)
                window.event.returnValue = retval;
            return retval;
        }

        function ValidatePage() {
            // For User Name and Password validation

            if (document.getElementById("ContentPlaceHolder1_txtWarehouseName") != null && document.getElementById("ContentPlaceHolder1_txtWarehouseName").value == '') {
                jAlert('Please enter Warehouse Name.', 'Message', 'ContentPlaceHolder1_txtWarehouseName');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtWarehouseName').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtAddress1") != null && document.getElementById("ContentPlaceHolder1_txtAddress1").value == '') {

                jAlert('Please enter Address1.', 'Message', 'ContentPlaceHolder1_txtAddress1');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtAddress1').offset().top }, 'slow');
                return false;
            }            
            else if (document.getElementById("ContentPlaceHolder1_txtCity") != null && document.getElementById("ContentPlaceHolder1_txtCity").value == '') {

                jAlert('Please enter City.', 'Message', 'ContentPlaceHolder1_txtCity');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtCity').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_ddlState") != null && document.getElementById("ContentPlaceHolder1_ddlState").selectedIndex == 0) {

                jAlert('Please select State.', 'Message', 'ContentPlaceHolder1_ddlState');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlState').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_ddlState") != null && document.getElementById("ContentPlaceHolder1_ddlState").options[document.getElementById("ContentPlaceHolder1_ddlState").selectedIndex].value == '-11') {

                if (document.getElementById("ContentPlaceHolder1_txtOtherState") != null && document.getElementById("ContentPlaceHolder1_txtOtherState").value == '') {
                    jAlert('Please enter Other State.', 'Message', 'ContentPlaceHolder1_txtOtherState');
                    $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtOtherState').offset().top }, 'slow');
                    return false;
                }
            }
            else if (document.getElementById("ContentPlaceHolder1_txtZipCode") != null && document.getElementById("ContentPlaceHolder1_txtZipCode").value == '') {

                jAlert('Please enter ZipCode.', 'Message', 'ContentPlaceHolder1_txtZipCode');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtZipCode').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_ddlcountry") != null && document.getElementById("ContentPlaceHolder1_ddlcountry").selectedIndex == 0) {

                jAlert('Please select Country.', 'Message', 'ContentPlaceHolder1_ddlcountry');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlcountry').offset().top }, 'slow');
                return false;
            }            
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
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Warehouse" alt="Add Warehouse" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add Warehouse"></asp:Label></h2>
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
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Warehouse Name :
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtWarehouseName" CssClass="order-textfield" 
                                                                MaxLength="400" Width="250px"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvWarehouseName" runat="server" ControlToValidate="txtWarehouseName"
                                                                ValidationGroup="Warehouse" ErrorMessage="Enter WarehouseName" SetFocusOnError="True"
                                                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Address 1 :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAddress1" MaxLength="500" 
                                                                CssClass="order-textfield" Width="250px"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ControlToValidate="txtAddress1"
                                                                ValidationGroup="Warehouse" ErrorMessage="Enter Address1" SetFocusOnError="True"
                                                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>Address 2 :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAddress2" MaxLength="500" 
                                                                CssClass="order-textfield" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>Apt/Suite # :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtSuite" MaxLength="100" 
                                                                CssClass="order-textfield" Width="140px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>City :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtCity" MaxLength="100" onkeypress="return isNumberKey(event)"
                                                                CssClass="order-textfield" Width="140px"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                                                                ValidationGroup="Warehouse" ErrorMessage="Enter City" SetFocusOnError="True"
                                                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Country :
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:DropDownList ID="ddlcountry" runat="server" CssClass="add-product-list" Width="225px"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlcountry_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>State/Province :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlState" runat="server" CssClass="add-product-list" Width="225px"
                                                                onchange="MakeBillingOtherVisible();">
                                                            </asp:DropDownList>
                                                            <div id="DIVBillingOther" style="display: none; padding-top: 5px;">
                                                                <span class="star">*</span>If Others, Specify &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtOtherState" MaxLength="30" onkeypress="return isNumberKey(event)"
                                                                    runat="server" CssClass="order-textfield" Width="78px"></asp:TextBox>
                                                                <asp:Label ID="lblBRFVOther" runat="server" CssClass="required-red" Visible="False"></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">*</span>Zip Code :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtZipCode" CssClass="order-textfield" 
                                                                MaxLength="15" Width="115px"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtZipCode"
                                                                ValidationGroup="Warehouse" ErrorMessage="Enter Zipcode" SetFocusOnError="True"
                                                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="oddrow">
                                <td>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="width: 13.5%">
                                            </td>
                                            <td style="width: 86.5%">
                                                <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                    Style="padding-top: 4px; padding-bottom: 4px" OnClientClick="return ValidatePage();"
                                                    OnClick="btnSave_Click" />
                                                <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                    CausesValidation="false" Style="padding-top: 4px; padding-bottom: 4px" OnClick="btnCancel_Click" />
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
