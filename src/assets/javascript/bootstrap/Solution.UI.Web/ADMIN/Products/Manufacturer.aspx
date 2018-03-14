<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Manufacturer.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Manufacturer"
    Theme="Gray" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            if (document.getElementById('<%=ddlStore.ClientID %>') != null && document.getElementById('<%=ddlStore.ClientID %>').selectedIndex == 0) {
                jAlert('Please select Store.', 'Message', '<%=ddlStore.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtname.ClientID %>') != null && document.getElementById('<%=txtname.ClientID %>').value == '') {
                jAlert('Enter Manufacturer Name.', 'Message', '<%=txtname.ClientID %>');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                                    <img class="img-left" title="Add Manufacture" alt="Add Manufacture" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Manufacture" ID="lblTitle"></asp:Label></h2>
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
                                                <asp:UpdatePanel ID="up1" runat="server">
                                                    <ContentTemplate>
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                            <tr class="oddrow">
                                                                <td style="width: 22%">
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="oddrow">
                                                                <td style="width: 22%">
                                                                    <span class="star">*</span> Store Name :
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="180px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td>
                                                                    <span class="star">*</span> Manufacture Name :
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtname" runat="server" Width="50%" CssClass="order-textfield"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr valign="top" class="oddrow">
                                                                <td>
                                                                    Description :
                                                                </td>
                                                                <td class="treeview">
                                                                    <textarea id="txtDescription" runat="server" rows="5" cols="10" style="resize: none;
                                                                        width: 210px; background: none repeat scroll 0 0 #FFFFFF; border: 1px solid #BCC0C1;"
                                                                        class="order-textfield"></textarea>
                                                                </td>
                                                            </tr>
                                                            <tr valign="top" class="altrow">
                                                                <td>
                                                                    SEKeywords :
                                                                </td>
                                                                <td>
                                                                    <textarea id="txtsekeywords" runat="server" rows="5" cols="10" style="resize: none;
                                                                        width: 210px; background: none repeat scroll 0 0 #FFFFFF; border: 1px solid #BCC0C1;"></textarea>
                                                                </td>
                                                            </tr>
                                                            <tr valign="top" class="oddrow">
                                                                <td>
                                                                    SEDescription :
                                                                </td>
                                                                <td>
                                                                    <textarea id="txtSedescription" runat="server" rows="5" cols="10" style="resize: none;
                                                                        width: 210px; background: none repeat scroll 0 0 #FFFFFF; border: 1px solid #BCC0C1;"></textarea>
                                                                </td>
                                                            </tr>
                                                            <tr valign="top" class="altrow">
                                                                <td>
                                                                    Display Order :
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDisplayorder" runat="server" Width="10%" CssClass="order-textfield"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr valign="top" class="oddrow">
                                                                <td>
                                                                    Brand Image :
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <img id="imgBrand" runat="server" height="60" width="60" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:FileUpload ID="fuBanner" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="ibtnUpload" runat="server" OnClick="ibtnUpload_Click" />
                                                                                &nbsp;&nbsp;
                                                                                <asp:ImageButton ID="ibtnDelete" runat="server" Visible="false" OnClick="ibtnDelete_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr valign="top" class="altrow">
                                                                <td>
                                                                    Status :
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkStatus" runat="server" />&nbsp;Published
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:ImageButton ID="ibtnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                        OnClientClick="return validation();" OnClick="imgSave_Click" />&nbsp;
                                                                    <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                        OnClick="imgCancle_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="ibtnUpload" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
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
