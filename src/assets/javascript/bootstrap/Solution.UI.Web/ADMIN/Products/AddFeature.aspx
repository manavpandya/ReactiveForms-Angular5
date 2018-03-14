<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="AddFeature.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.AddFeature" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
     <script type="text/javascript">
         function Checkfields() {

             if (document.getElementById('<%=txtfeaturename.ClientID %>').value == '') {
                 jAlert('Please enter Feature Name.', 'Message', '<%=txtfeaturename.ClientID %>');
                 return false;
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
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Email template" alt="Add Feature" src="/App_Themes/<%=Page.Theme %>/Images/emailTemplate.png">
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add Feature"></asp:Label></h2>
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
                                                            <span class="star">&nbsp</span>Store Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                             <asp:DropDownList ID="ddlStore" AutoPostBack="true" runat="server" CssClass="order-list"
                                                                Width="185px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Feature Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtfeaturename" CssClass="order-textfield" MaxLength="50"
                                                                Width="350px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                   
                                                    <tr class="altrow">
                                                     
                                                       
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">*</span> Body:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtFeatureBody" Rows="10"
                                                                Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                            <script type="text/javascript">
                                                                CKEDITOR.replace('<%= txtFeatureBody.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                                CKEDITOR.on('dialogDefinition', function (ev) {
                                                                    if (ev.data.name == 'image') {
                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                        btn.hidden = false;
                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                    }
                                                                    if (ev.data.name == 'link') {
                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                        btn.hidden = false;
                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                    }
                                                                });
                                                            </script>
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
                                                        <asp:ImageButton ID="btnSaveFeature" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClientClick="return Checkfields();" OnClick="btnSaveFeature_Click" />
                                                        <asp:ImageButton ID="btnCancelFeature" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            CausesValidation="false" OnClick="btnCancelFeature_Click" />
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
