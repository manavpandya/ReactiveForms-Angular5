<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="StoreCredit.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.StoreCredit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-1.2.6.min.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {
           
             if (document.getElementById("ContentPlaceHolder1_txtStoreCredit").value == '') {
                jAlert('Please enter StoreCredit Name.', 'Message', 'ContentPlaceHolder1_txtStoreCredit');
                return false;
             }
             else if (document.getElementById("ContentPlaceHolder1_txtStoreCreditAmount").value == '') {
                 jAlert('Please enter StoreCredit Amount.', 'Message', 'ContentPlaceHolder1_txtStoreCreditAmount');
                 return false;
             }

            return true;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
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
                                                <img class="img-left" title="Add Store Credit Option" alt="Add StoreCredit Option" src="/App_Themes/<%=Page.Theme %>/Images/payment-option-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="Add Store Credit" ID="lblTitle"></asp:Label></h2>
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
                                                
                                                <tr class="altrow">
                                                    <td style="width: 20%">
                                                        <span class="star">*</span>Store Credit :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtStoreCredit" CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 20%">
                                                        <span class="star">*</span>Store Credit Amount  :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtStoreCreditAmount" onkeypress="return keyRestrict(event,'0123456789.');" CssClass="order-textfield" 
                                                            ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 20%">
                                                        <span class="star">&nbsp;&nbsp;</span>Active :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkststus"  runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <td>
                                                </td>
                                                <td style="width: 80%">
                                                    <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                        OnClick="imgSave_Click" CausesValidation="true" OnClientClick="return Checkfields();" />
                                                    &nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                        OnClick="imgCancel_Click" />
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
            <tr>
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" alt="" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
