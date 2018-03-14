<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ProductRomanShippingAdd.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductRomanShippingAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


 <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
     <script type="text/javascript">
         function Checkfields() {

             if (document.getElementById('<%=txtfromwidth.ClientID %>').value == '') {

                 jAlert('Please enter from width.', 'Message', '<%=txtfromwidth.ClientID %>');
                 return false;
             }
           
             if (document.getElementById('<%=txttowidth.ClientID %>').value == '') {
                 jAlert('Please enter To width.', 'Message', '<%=txttowidth.ClientID %>');
                 return false;
             }
             if (document.getElementById('<%=txtcost.ClientID %>').value == '') {
                 jAlert('Please enter cost.', 'Message', '<%=txtcost.ClientID %>');
                 return false;
             }
             return true;
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
                                                    <img class="img-left" title="Add Email template" alt="Add Feature" src="/App_Themes/<%=Page.Theme %>/Images/emailTemplate.png">
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add Roman Shipping"></asp:Label></h2>
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
                                                            <span class="star">*</span>From Width:
                                                        </td>
                                                        <td style="width: 80%">
                                                           
                                                            <asp:TextBox runat="server" ID="txtfromwidth" CssClass="order-textfield" MaxLength="50"
                                                                Width="150px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>To Width:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txttowidth" CssClass="order-textfield" MaxLength="50"
                                                                Width="150px"  onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                        </td>
                                                    </tr>   
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Cost:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtcost" CssClass="order-textfield" MaxLength="50"
                                                                Width="150px"  onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                        </td>
                                                    </tr>                                                   
                                                    
                                                     <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>Active:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkIsActive" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="oddrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <td align="left" style="width:13%">
                                                    </td>
                                                    <td style="width: 87%">
                                                        <asp:ImageButton ID="btnSaveshipping" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClientClick="return Checkfields();" OnClick="btnSaveshipping_Click" />
                                                        <asp:ImageButton ID="btnCancelshipping" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            CausesValidation="false" OnClick="btnCancelshipping_Click" />
                                                    </td>
                                                </table>
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
