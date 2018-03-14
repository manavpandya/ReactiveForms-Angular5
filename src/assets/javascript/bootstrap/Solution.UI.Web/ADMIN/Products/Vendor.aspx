<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Vendor.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Vendor"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function MakeBillingOtherVisible() {
            var index = document.getElementById('ContentPlaceHolder1_ddlState').options[document.getElementById('ContentPlaceHolder1_ddlState').selectedIndex].value;
            
            if (document.getElementById('ContentPlaceHolder1_ddlState') != null && document.getElementById('ContentPlaceHolder1_ddlState').options[document.getElementById('ContentPlaceHolder1_ddlState').selectedIndex].value == '-11')
                SetBillingOtherVisible(true);
            else
                SetBillingOtherVisible(false);
        }

        function SetBillingOtherVisible(IsVisible) {
            if (IsVisible && document.getElementById('ContentPlaceHolder1_divother') != null) {
                document.getElementById('ContentPlaceHolder1_divother').style.display = 'block';
            }
            else {
                if (document.getElementById('ContentPlaceHolder1_txtother') != null) {
                    document.getElementById('ContentPlaceHolder1_txtother').value = ''; 
                }
                if (document.getElementById('ContentPlaceHolder1_divother') != null) {
                    document.getElementById('ContentPlaceHolder1_divother').style.display = 'none';
                 }

            }
        }
    </script>
    <script type="text/javascript">
        function isNumberKey(event) {
            document.getElementById("ContentPlaceHolder1_txtname").value = document.getElementById("ContentPlaceHolder1_txtname").value.replace(/^\s+/, "");
            var retval = false;
            var charCode = (event.which) ? event.which : (window.event) ? window.event.keyCode : -1;
            if (charCode == -1 || charCode > 31 && (charCode < 33 || charCode > 64) && (charCode < 91 || charCode > 96) && (charCode < 123 || charCode > 126))
                retval = true;
            if (charCode == 8)
                retval = true;
            if (navigator.appName.indexOf('Microsoft') != -1)
                window.event.returnValue = retval;
            return retval;
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
                                                    <img class="img-left" title="Add Vendor/DropShipper" alt="Add Vendor/DropShipper" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add Vendor/DropShipper"></asp:Label></h2>
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
                                                            <span class="star">*</span>Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtVendorName" CssClass="order-textfield" MaxLength="400"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvVendorName" runat="server" ControlToValidate="txtVendorName"
                                                                ValidationGroup="Vendors" ErrorMessage="Enter Name" SetFocusOnError="True" Display="Dynamic"
                                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Email:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEmail" CssClass="order-textfield" MaxLength="200"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                                ValidationGroup="Vendors" ErrorMessage="Enter Email" SetFocusOnError="True" Display="Dynamic"
                                                                ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">*</span>Address:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAddress" CssClass="order-textfield" MaxLength="400"
                                                                Width="223" Height="60" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
                                                                ValidationGroup="Vendors" ErrorMessage="Enter Address" SetFocusOnError="True"
                                                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>City:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtCity" CssClass="order-textfield" MaxLength="400"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>Country:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCountry" runat="server" CssClass="select-box" Width="225px"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>State/Province:
                                                        </td>
                                                        <td valign="top">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlState" onchange="MakeBillingOtherVisible();" runat="server"
                                                                            CssClass="select-box" Style="width: 225px;">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td >
                                                                        <div id="divother" runat="Server" style="display: none; float: left;">
                                                                            <asp:Label ID="lt_other" runat="server" Text="If Others, Specify  "></asp:Label>
                                                                            <asp:TextBox ID="txtother" onkeypress="return isNumberKey(event)" runat="server"
                                                                                CssClass="order-textfield"  Width="124px" MaxLength="70" Style="margin-top: 5px;
                                                                                margin-left: 5px;"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>Zip Code:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtZipcode" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtZipcode"
                                                                ValidationGroup="Vendors" ErrorMessage="Enter Zip Code" SetFocusOnError="True"
                                                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Phone:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtPhone" CssClass="order-textfield" MaxLength="20" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                                                                ValidationGroup="Vendors" ErrorMessage="Enter Phone No" SetFocusOnError="True"
                                                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                     <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">&nbsp;&nbsp;</span>Is DropShipper?:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:CheckBox ID="chkIsDropShipper" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td style="width: 12%">
                                                            <span class="star">&nbsp;&nbsp;</span>Status:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:CheckBox ID="chkStatus" runat="server" Checked="true" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow" style="display:none;">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>E-Mail Template:
                                                        </td>
                                                        <td align="left" valign="middle">
                                                            <asp:DropDownList ID="ddlEmailTemplate" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="altrow">
                                <td>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <td align="left" style="width: 13.5%">
                                        </td>
                                        <td style="width: 86.5%">
                                            <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                OnClick="btnSave_Click" ValidationGroup="Vendors" Style="padding-top: 4px; padding-bottom: 4px" />
                                            <asp:ImageButton ID="btnCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                OnClick="btnCancle_Click" CausesValidation="false" Style="padding-top: 4px; padding-bottom: 4px" />
                                        </td>
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
